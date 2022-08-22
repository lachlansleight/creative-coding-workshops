using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looker : MonoBehaviour
{

    public Transform Target;

    public void Update()
    {
        var targetForward = Target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(targetForward, Vector3.up);
    }
}
