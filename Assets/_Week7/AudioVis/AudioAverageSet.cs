using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunity.AudioVis
{
    public class AudioAverageSet : MonoBehaviour
    {
        [Header("Config")]
        public SoundCapture Audio;
        [Tooltip("The raw signal to use for visualization")] public SoundCapture.DataSource DataSource = SoundCapture.DataSource.PeakVolume;
        [Tooltip("Frequency bin to use - if using a single frequency bin as the data source")] public int FrequencyBin = 0;

        [Header("Basic Values")]
        [Range(0f, 1f)] [Tooltip("Raw, per-frame audio signal")] public float Momentary;
        [Range(0f, 1f)] public float HalfSecondAverage;
        [Range(0f, 1f)] public float OneSecondAverage;
        [Range(0f, 1f)] public float FiveSecondAverage;
        [Range(0f, 1f)] public float TenSecondAverage;
        [Range(0f, 1f)] public float ThirtySecondAverage;

        [Header("Combination Values")]
        [Range(-1f, 1f)] [Tooltip("Momentary / FiveSecond")] public float Flicker;
        [Range(-1f, 1f)] [Tooltip("HalfSecond / FiveSecond")] public float Pulse;
        [Range(-1f, 1f)] [Tooltip("FiveSecond / ThirtySecond")] public float Vibe;
        
        private TimeAverager _halfSecond;
        private TimeAverager _second;
        private TimeAverager _fiveSecond;
        private TimeAverager _tenSecond;
        private TimeAverager _thirtySecond;

        public void Awake()
        {
            _halfSecond = new TimeAverager(30);
            _second = new TimeAverager(60);
            _fiveSecond = new TimeAverager(300);
            _tenSecond = new TimeAverager(600);
            _thirtySecond = new TimeAverager(1800);

            if (Audio == null) Audio = FindObjectOfType<SoundCapture>();
            if (Audio == null) {
                Debug.LogError("AudioAverageSet failed to find a SoundCapture component in the scene! Disabling");
                enabled = false;
            }
        }

        public void Update()
        {
            //Gets the raw data from the SoundCapture component
            Momentary = GetRawData();
            
            // Update the time-averagers using the new frame's raw data
            HalfSecondAverage = _halfSecond.Update(Momentary);
            OneSecondAverage = _second.Update(Momentary);
            FiveSecondAverage = _fiveSecond.Update(Momentary);
            TenSecondAverage = _tenSecond.Update(Momentary);
            ThirtySecondAverage = _thirtySecond.Update(Momentary);

            // Update the combination values - more could be added here depending on use-case!
            Flicker = Momentary - FiveSecondAverage;
            Pulse = HalfSecondAverage - FiveSecondAverage;
            Vibe = FiveSecondAverage - ThirtySecondAverage;
            //Flicker = Mathf.Clamp01((Momentary / (FiveSecondAverage + 0.0001f)) - 1f);
            //Pulse = Mathf.Clamp01((HalfSecondAverage / (FiveSecondAverage + 0.0001f)) - 1f);
            //Vibe = Mathf.Clamp01((FiveSecondAverage / (ThirtySecondAverage + 0.0001f)) - 1f);
        }
        
        private float GetRawData()
        {
            switch (DataSource) {
                case SoundCapture.DataSource.AverageVolume:
                    return Audio.AverageVolume;
                case SoundCapture.DataSource.PeakVolume:
                    return Audio.PeakVolume;
                case SoundCapture.DataSource.SingleBand:
                    return Audio.BarData[Mathf.Clamp(FrequencyBin, 0, Audio.BarData.Length)];
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // A simple class that records a list of samples over a period of time and computes the running average
        public class TimeAverager
        {
            public float Value;
            private float[] _samples;
            private int _index;
            private bool _full;

            public TimeAverager(int count)
            {
                _samples = new float[count];
                _index = 0;
                _full = false;
                Value = 0f;
            }

            public float Update(float sample)
            {
                _samples[_index] = sample;
                _index++;
                if (_index >= _samples.Length) {
                    _full = true;
                    _index = 0;
                }

                var avg = 0f;
                for (var i = 0; i < _samples.Length; i++) {
                    if (i >= _index && !_full) break;
                    avg += _samples[i];
                }

                avg /= _full ? _samples.Length : _index;
                Value = avg;

                return Value;
            }

            public void Reset()
            {
                _index = 0;
                _full = false;
                Value = 0f;
            }
        }
    }
}