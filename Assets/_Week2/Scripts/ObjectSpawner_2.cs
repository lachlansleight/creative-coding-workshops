using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner_2 : MonoBehaviour
{
    public PrimitiveType ObjectType = PrimitiveType.Cube;
    [Space(10)] //Space is a nice way to keep your inspector organized
    [Range(1, 10000)] public int ObjectCount = 500;
    [Range(0f, 10f)] public float SpawnArea = 5f;
    [Range(0f, 2f)] public float ObjectSizeMultiplier = 1f;

    [Header("Noise settings")]
    [Range(0f, 5f)] public float NoiseAmplitude = 1f;
    [Range(0f, 10f)] public float NoiseFrequency = 1f;

    //ContextMenu lets us call functions in edit-mode, which is great for iterating quickly!
    [ContextMenu("Spawn Objects")]
    public void SpawnObjects()
    {
        ClearObjects();

        //get how many per side - since we will have the square of this number in total objects, we use the square root
        //e.g. if we want 100 objects, that's 10 per side (sqrt(100) = 10)
        var sideCount = Mathf.FloorToInt(Mathf.Sqrt(ObjectCount));
        
        //to make sure there's no gaps, we set the object size with code
        var objectSize = SpawnArea / sideCount;
        
        //Spawn a bunch of primitives in a grid (the XZ plane)
        for (var i = 0; i < sideCount; i++) {
            //the x position is constant for this whole row, so we calculate it here
            
            //we get a float from 0 to 1 out of i by dividing it by sideCount
            //then we remap it to -SpawnArea * 0.5 -> SpawnArea * 0.5
            var x = SpawnArea * ((float)i / sideCount) - SpawnArea * 0.5f;
            
            for (var j = 0; j < sideCount; j++) {
                var z = SpawnArea * ((float)j / sideCount) - SpawnArea * 0.5f;
                
                //Create a new object, make sure it's a child of this object, and give it a random position, rotation and scale
                var newObj = GameObject.CreatePrimitive(ObjectType);
                newObj.transform.parent = transform;
                newObj.transform.localPosition = new Vector3(x, GetHeight(x, z), z);
                newObj.transform.localRotation = Quaternion.identity;
                
                //now, object size represents a multiplier if we want them to be smaller and larger than this procedural value
                newObj.transform.localScale = Vector3.one * ObjectSizeMultiplier * objectSize;
            }
        }
    }

    //Takes in a XZ position and returns whatever the height should be at that point
    private float GetHeight(float x, float z)
    {
        return Mathf.PerlinNoise(x * NoiseFrequency, z * NoiseFrequency) * NoiseAmplitude;
    }

    [ContextMenu("Clear Objects")]
    public void ClearObjects()
    {
        //We iterate backwards through children, since if we go forwards we end up changing child indices
        for (var i = transform.childCount - 1; i >= 0; i--) {
            //DestroyImmediate is for edit-mode, Destroy is for play-mode (including in the built application)
            if (Application.isPlaying) Destroy(transform.GetChild(i).gameObject);
            else DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}
