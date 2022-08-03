using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FractalNoise
{
    [Range(1, 5)] public int Octaves;
    public float Frequency;
    public float FrequencyMultiplier;
    public float AmplitudeMultiplier;

    //This is how multi-octave noise is calculated
    //Each pass, we increase the frequency (making the details finer) and decrease the amplitude (decreasing the effect the finer details have)
    //Then we add that to the total - this produces pleasing results and is used EVERYWHERE in computer graphics!
    public float GetNoise(float x, float y)
    {
        var sum = 0f;
        var freq = Frequency;
        var amp = 1f;
        var ampSum = 0f;
        for (var i = 0; i < Octaves; i++) {
            sum += Mathf.PerlinNoise(x * freq, y * freq) * amp;
            ampSum += amp;
            freq *= FrequencyMultiplier;
            amp *= AmplitudeMultiplier;
        }

        //We divide by the amplitude sum so that the final output is in the range 0 to 1
        return sum / ampSum;
    }
}