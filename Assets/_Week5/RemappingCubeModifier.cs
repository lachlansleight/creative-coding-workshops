using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemappingCubeModifier : MonoBehaviour
{
    [Header("Coloring")]
    public float MinHeight = 0f;
    public float MaxHeight = 10f; 
    public Gradient ColorByHeight;

    [Header("Sizing")]
    public float MinDistance = 0f;
    public float MaxDistance = 3f;
    public float MinScale = 0.1f;
    public float MaxScale = 2f;

    [ContextMenu("Color Cubes")]
    public void ColorCubes()
    {
        var cubeParent = transform.Find("Cubes");
        if (cubeParent == null) return;

        //Maps cube heights into colors using our gradient
        for (var i = 0; i < cubeParent.childCount; i++) {
            var cubeHeight = cubeParent.GetChild(i).position.y;
            var colorT = Mathf.InverseLerp(MinHeight, MaxHeight, cubeHeight);
            var color = ColorByHeight.Evaluate(colorT);
            SetColor(cubeParent.GetChild(i).GetComponent<Renderer>(), color);
        }
    }

    [ContextMenu("Size Cubes")]
    public void SizeCubes()
    {
        var cubeParent = transform.Find("Cubes");
        if (cubeParent == null) return;
        
        var sphereParent = transform.Find("Spheres");
        if (sphereParent == null) return;

        //don't do anything if there aren't any spheres
        if (sphereParent.childCount == 0) return;
        
        for (var i = 0; i < cubeParent.childCount; i++) {
            var cube = cubeParent.GetChild(i);
            
            //Find the minimum distance between this cube and all the spheres
            //Very inefficient, but we're doing this at edit time soooo :D
            //We start at float.MaxValue since it's guaranteed to be bigger than the first distance
            var closestSphereDistance = float.MaxValue;
            for (var j = 0; j < sphereParent.childCount; j++) {
                var dist = (cube.position - sphereParent.GetChild(j).position).magnitude;
                closestSphereDistance = Mathf.Min(dist, closestSphereDistance);
            }

            //Set the cube's z-scale based on this distance
            var distT = Mathf.InverseLerp(MinDistance, MaxDistance, closestSphereDistance);
            var scale = Mathf.Lerp(MinScale, MaxScale, distT);
            cube.localScale = new Vector3(
                cube.localScale.x,
                cube.localScale.y,
                scale
            );
        }
    }
    
    //Copied from Week 2's ObjectSpawner_3.cs - check that script for more info on how this works
    private void SetColor(Renderer target, Color color)
    {
        var propertyBlock = new MaterialPropertyBlock();
        propertyBlock.SetColor("_Color", color);
        target.SetPropertyBlock(propertyBlock);
    }
}
