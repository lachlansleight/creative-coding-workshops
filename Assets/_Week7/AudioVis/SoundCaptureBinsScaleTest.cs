using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunity.AudioVis
{
    public class SoundCaptureBinsScaleTest : MonoBehaviour
    {
        private SoundCapture _sc;
        
        public void Start()
        {
            StartCoroutine(InitializeAfterBarData());
        }

        private IEnumerator InitializeAfterBarData()
        {
            //waits for the sound capture FFT data to be initially populated, then creates the cubes
            _sc = FindObjectOfType<SoundCapture>();
            while (_sc.BarData.Length == 0) yield return null;
            for (var i = 0; i < _sc.BarData.Length; i++) {
                var newObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                var scaleTest = newObj.AddComponent<SoundCaptureScaleTest>();
                scaleTest.Target = SoundCapture.DataSource.SingleBand;
                scaleTest.BandIndex = i;
                scaleTest.transform.parent = transform;
                scaleTest.transform.localPosition = new Vector3(scaleTest.MinScale * i, 0f, 0f);
            }
        }
    }
}