using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner_1 : MonoBehaviour
{
    public PrimitiveType ObjectType = PrimitiveType.Cube;
    [Range(1, 10000)] public int ObjectCount = 500;
    [Range(0f, 10f)] public float SpawnSphereSize = 5f;
    [Range(0f, 2f)] public float ObjectSize = 1f;

    //ContextMenu lets us call functions in edit-mode, which is great for iterating quickly!
    [ContextMenu("Spawn Objects")]
    public void SpawnObjects()
    {
        ClearObjects();
        
        for (var i = 0; i < ObjectCount; i++) {
            //Create a new object, make sure it's a child of this object, and give it a random position, rotation and scale
            var newObj = GameObject.CreatePrimitive(ObjectType);
            newObj.transform.parent = transform;
            newObj.transform.localPosition = Random.insideUnitSphere * SpawnSphereSize;
            newObj.transform.localRotation = Random.rotation;
            newObj.transform.localScale = Vector3.one * ObjectSize;
            
            //To make things interesting, let's make the objects smaller the further they are away from the center of the spawn sphere
            newObj.transform.localScale *= 1f - newObj.transform.localPosition.magnitude / SpawnSphereSize;
        }
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
