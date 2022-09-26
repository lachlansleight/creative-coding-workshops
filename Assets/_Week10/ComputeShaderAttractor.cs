using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeShaderAttractor : MonoBehaviour
{
    //This is just a simple class to populate the attractor position/strength
    //using the mouse.
    
    public float MaxForce = 10f;
    public float Distance = 5f;
    public ComputeShaderSystem ShaderSystem;

    private Camera _camera;

    public void Awake()
    {
        _camera = Camera.main;
    }
    
    public void Update()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = Distance;
        var position = _camera.ScreenToWorldPoint(mousePos);
        ShaderSystem.AttractorPosition = position;
        ShaderSystem.AttractorStrength = Input.GetMouseButton(0) ? MaxForce : 0f;
    }
}
