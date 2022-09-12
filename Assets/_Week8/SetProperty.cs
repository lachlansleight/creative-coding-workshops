using System.Collections;
using System.Collections.Generic;
using Lunity.AudioVis;
using UnityEngine;

public class SetProperty : MonoBehaviour
{

    public float Multiplier = 1f;
    public AudioAverageSet Audio;
    public string PropertyName;
    public Renderer TargetRenderer;

    
    void Update()
    {
        TargetRenderer.material.SetFloat(PropertyName, Audio.Pulse * Multiplier);
    }
}
