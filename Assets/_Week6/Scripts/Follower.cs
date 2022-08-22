using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform Target;

    public float LerpSpeed = 4f;
    public float MaxSpeed = 4f;
    public float SmoothTime = 2f;
    
    private Vector3 _velocity;

    public void Update()
    {
        //transform.position = Vector3.Lerp(transform.position, Target.position, Time.deltaTime * LerpSpeed);
        //transform.position = Vector3.MoveTowards(transform.position, Target.position, MaxSpeed * Time.deltaTime);
        transform.position =
            Vector3.SmoothDamp(transform.position, Target.position, ref _velocity, SmoothTime, MaxSpeed);
    }
}
