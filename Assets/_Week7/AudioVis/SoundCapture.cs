using UnityEngine;
using CSCore;
using CSCore.SoundIn;
using CSCore.Codecs.WAV;
using CSCore.CoreAudioAPI;
using WinformsVisualization.Visualization;
using CSCore.DSP;
using CSCore.Streams;

namespace Lunity.AudioVis
{

    // This file was modified from something I found like 10 years ago.
    // I'm sorry, original creator for not attributing it to you, I downloaded this when I was a teenager learning
    // to code and didn't know anything about open source software :'c
    public class SoundCapture : MonoBehaviour
    {
        public enum DataSource
        {
            AverageVolume,
            PeakVolume,
            SingleBand
        }
        
        [Header("Configuration")]
        [Range(3, 120)]
        [Tooltip("Now many discrete frequency bands to use for visualization")]
        public int FftBinCount = 30;
        
        [Range(20, 20000)] 
        [Tooltip("Minimum frequency, in Hz, to use for audio visualization")]
        public int MinimumFrequency = 20;
        
        [Range(20, 20000)] 
        [Tooltip("Maximum frequency, in Hz, to use for audio visualization")]
        public int MaximumFrequency = 10000;

        [Header("Advanced")]
        [Range(0, 250)] 
        [Tooltip("Audio visualization latency in milliseconds")]
        public int Latency = 20;
        
        [Tooltip("Number of FFT samples to use when setting up FFT Bins")]
        public FftSize FftSize = FftSize.Fft4096;

        [Header("Output")]
        [Tooltip("The name of the device that is being used for visualization (read only)")]
        public string DeviceName;
        [Range(0f, 1f)]
        [Tooltip("A single value representing the current average volume of all the frequency ranges")]
        public float AverageVolume;
        [Range(0f, 1f)]
        [Tooltip("A single value representing the current peak volume across all frequency ranges")]
        public float PeakVolume;
        [Tooltip("The output FFT data that can be used for audio visualization (read only)")]
        [Range(0f, 1f)] 
        public float[] BarData;

        SpectrumBase _spectrum;
        WasapiCapture _capture;
        WaveWriter _writer;
        float[] _fftBuffer;
        SingleBlockNotificationStream _notificationSource;
        IWaveSource _finalSource;
        private byte[] _rawBuffer;

        ///Creates an audio capture device, begins capture and sets up all the data structures needed to store audio data
        private void Initialize()
        {
            if (_capture != null) Cleanup();
            
            // This uses wasapi to get any sound data played by the computer
            // Note that wasapi is a Windows thing, so this will *not* work on Mac or Linux!
            _capture = new WasapiLoopbackCapture(Latency);
            _capture.Initialize();
            var source = new SoundInSource(_capture).ToSampleSource();
            _capture.DataAvailable += Capture_DataAvailable;
            
            var notificationSource = new SingleBlockNotificationStream(source);
            notificationSource.SingleBlockRead += NotificationSource_SingleBlockRead;
            _finalSource = notificationSource.ToWaveSource();
            _rawBuffer = new byte[_finalSource.WaveFormat.BytesPerSecond / 2];

            // Actual fft data computation structure
            _fftBuffer = new float[(int) FftSize];
            _spectrum = new SpectrumBase()
            {
                SpectrumProvider = new BasicSpectrumProvider(
                    _capture.WaveFormat.Channels,
                    _capture.WaveFormat.SampleRate, 
                    FftSize
                ),
                UseAverage = true,
                IsXLogScale = true,
                ScalingStrategy = ScalingStrategy.Linear,
                MinimumFrequency = MinimumFrequency,
                MaximumFrequency = MaximumFrequency,
                SpectrumResolution = FftBinCount,
                FftSize = FftSize,
            };

            DeviceName = _capture.Device.FriendlyName;
            _capture.Start();
        }

        public void Reinitialize()
        {
            Cleanup();
            Initialize();
        }

        ///Logs all available audio devices to the console - for debugging
        public void LogDevices()
        {
            var devices = MMDeviceEnumerator.EnumerateDevices(DataFlow.All);
            for (var i = 0; i < devices.Count; i++) {
                var device = devices[i];
                Debug.Log($"Device {device.FriendlyName} ({device.DeviceID}) - {device.DataFlow}");
            }
        }
        
        void Update()
        {
            //Starts audio capture if it's not already running
            if (_capture == null) Initialize();

            //Gets audio data and splits it into frequency ranges for visualization purposes
            _spectrum.SpectrumProvider.GetFftData(_fftBuffer, this);
            var values = _spectrum.GetSpectrumPoints(1d, _fftBuffer);
            BarData = values;

            //Calculates the RMS and peak volumes across all frequency bins
            PeakVolume = 0f;
            AverageVolume = 0f;
            for (var i = 0; i < BarData.Length; i++) {
                var d = BarData[i];
                AverageVolume += d * d;
                PeakVolume = Mathf.Max(PeakVolume, d);
            }
            AverageVolume /= BarData.Length;
            AverageVolume = Mathf.Sqrt(AverageVolume);
        }
        
        private void Capture_DataAvailable(object sender, DataAvailableEventArgs e)
        {
            while ((_finalSource.Read(_rawBuffer, 0, _rawBuffer.Length)) > 0) { }
        }

        private void NotificationSource_SingleBlockRead(object sender, SingleBlockReadEventArgs e)
        {
            (_spectrum.SpectrumProvider as BasicSpectrumProvider)?.Add(e.Left, e.Right);
        }

        void OnDisable()
        {
            Cleanup();
        }

        /// Stops audio capture and properly disposes of any ongoing processes
        public void Cleanup()
        {
            if (_capture == null) return;

            try { _capture.Stop(); } 
            catch { /* ignore */ }

            try { _capture.Dispose(); } 
            catch { /* ignore */ }
        }
    }
}