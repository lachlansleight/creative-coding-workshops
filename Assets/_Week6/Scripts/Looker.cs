using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looker : MonoBehaviour
{

    public Transform Target;
    public float LerpRate = 4f;
    public float RotateRate = 90f;

    public void Update()
    {
        var targetForward = Vector3.RotateTowards(transform.forward, Target.position - transform.position, RotateRate * Time.deltaTime * Mathf.Deg2Rad, 0f);
        transform.rotation = Quaternion.LookRotation(targetForward, Vector3.up);
        return;
        var targetRotation = Quaternion.LookRotation(Target.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * LerpRate);
    }
}
