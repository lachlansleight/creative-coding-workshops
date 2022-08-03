using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseExample : MonoBehaviour
{
    public FractalNoise Noise;

    [ContextMenu("Draw Noise Debug Lines")]
    public void NoiseTest()
    {
        for (var i = 0f; i < 1f; i += 0.01f) {
            for (var j = 0f; j < 1f; j += 0.01f) {
                var noiseVal = Noise.GetNoise(i, j);
                
                //We love Debug.DrawLine
                Debug.DrawLine(
                    new Vector3(i, 0f, j), 
                    new Vector3(i, noiseVal, j),
                    Color.Lerp(Color.white, Color.magenta, noiseVal), 
                    10f
                );
            }
        }
    }
}
