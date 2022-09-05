using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform Target;
    public float LerpRate = 0.5f;
    public float MaxSpeed = 10f;
    public float SmoothTime = 1f;

    private float _scaleVelocity;

    private Vector3 _velocity;

    public void Update()
    {
        //transform.position = Vector3.Lerp(transform.position, Target.position, LerpRate * Time.deltaTime);
        //transform.position = Vector3.MoveTowards(transform.position, Target.position, MaxSpeed * Time.deltaTime);
        transform.position = Vector3.SmoothDamp(transform.position, Target.position, ref _velocity, SmoothTime, MaxSpeed);

        transform.localScale = Vector3.one * Mathf.SmoothDamp(transform.localScale.x, Input.GetMouseButton(0) ? 2f : 1f,
            ref _scaleVelocity, SmoothTime);
    }
}
