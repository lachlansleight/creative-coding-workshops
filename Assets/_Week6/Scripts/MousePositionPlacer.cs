using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePositionPlacer : MonoBehaviour
{
    public Camera MainCam;

    public void Update()
    {
        //This is a way to take in the position of the mouse cursor and use it to cast a ray into the scene
        //We move the transform to the hit position to place whatever object this component is on at the mouse's position
        if (!Physics.Raycast(MainCam.ScreenPointToRay(Input.mousePosition), out var hit)) return;
        transform.position = hit.point;
    }
}
