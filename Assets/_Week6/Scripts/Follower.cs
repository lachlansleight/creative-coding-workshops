using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform Target;

    public void Update()
    {
        transform.position = Target.position;
    }
}
