using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireVisuals : MonoBehaviour
{

    [Header("Status")]
    [Range(0f, 1f)] public float FireStrength = 0f;

    [Header("Config")]
    [Range(0f, 1f)] public float FlameThreshold = 0.2f;
    [Range(0f, 1f)] public float MaxSmokeThreshold = 0.7f;
    public float MaxFlameEmit = 20f;
    public float MaxSmokeEmit = 10f;

    [Header("Objects")]
    public ParticleSystem FlameParticles;
    public ParticleSystem SmokeParticles;

    public void Update()
    {
        //The smoke starts immediately and reaches its max at some threshold
        var smokeT = Mathf.InverseLerp(0f, MaxSmokeThreshold, FireStrength);
        var smokeEm = SmokeParticles.emission;
        smokeEm.rateOverTime = MaxSmokeEmit * smokeT;
        
        //The flame starts at some threshold and reaches its max at 1
        var flameT = Mathf.InverseLerp(FlameThreshold, 1f, FireStrength);
        var flameEm = FlameParticles.emission;
        flameEm.rateOverTime = MaxFlameEmit * flameT;

        
    }

}
