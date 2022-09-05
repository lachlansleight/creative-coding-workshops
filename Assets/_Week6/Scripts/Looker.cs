using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looker : MonoBehaviour
{

    public Transform Target;
    public float LerpSpeed = 6f;
    [Range(0f, 3600f)] public float RotationSpeed = 360f;

    private float _rotation;
    private float _velocity;
    
    public void Update()
    {
        //var targetForward = Target.position - transform.position;
        //transform.rotation = Quaternion.LookRotation(targetForward, Vector3.up);
        //var targetRotation = Quaternion.LookRotation(targetForward, Vector3.up);
        //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, LerpSpeed * Time.deltaTime);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);

        var offset = Target.position - transform.position;
        offset.y = 0f;
        offset.Normalize();
        var yRotation = Mathf.Atan2(offset.z, offset.x) * Mathf.Rad2Deg * -1f + 90f;
        _rotation = Mathf.SmoothDampAngle(_rotation, yRotation, ref _velocity, 1f);
        
        transform.rotation = Quaternion.Euler(0f, _rotation, 0f);
    }
}
