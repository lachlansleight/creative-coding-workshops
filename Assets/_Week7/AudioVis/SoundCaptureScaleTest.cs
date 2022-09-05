using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunity.AudioVis
{

    //A simple component to visualize the raw audio data coming out of SoundCapture
    public class SoundCaptureScaleTest : MonoBehaviour
    {
        public SoundCapture.DataSource Target = SoundCapture.DataSource.AverageVolume;
        public int BandIndex = 0;

        public float MinScale = 0.1f;
        public float ScaleAmount = 1f;

        private SoundCapture _sc;
        
        public void Awake()
        {
            _sc = FindObjectOfType<SoundCapture>();
        }

        public void Update()
        {
            transform.localScale = new Vector3(MinScale, MinScale + ScaleAmount * GetValue(), MinScale);
        }

        private float GetValue()
        {
            switch (Target) {
                case SoundCapture.DataSource.AverageVolume:
                    return _sc.AverageVolume;
                case SoundCapture.DataSource.PeakVolume:
                    return _sc.PeakVolume;
                case SoundCapture.DataSource.SingleBand:
                    return _sc.BarData[Mathf.Clamp(BandIndex, 0, _sc.BarData.Length)];
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}