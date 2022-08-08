using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePlacer_1 : MonoBehaviour
{
    [Header("Positioning")]
    public Vector2 Start = new Vector2(-500f, -500f);
    public Vector2 Size = new Vector2(1000f, 1000f);
    public float MaxHeight = 600f; //this is my terrain height
    public LayerMask ValidLayers;
    
    [Header("Appearance")]
    public GameObject[] TreePrefabs;
    [Range(0f, 1f)] public float SizeVariance = 0.1f;

    [Header("Debug")]
    public int DebugSpawnCount = 1000;

    [ContextMenu("Spawn Debug")]
    public void SpawnDebug()
    {
        Spawn(DebugSpawnCount);
    }
    
    public void Spawn(int count)
    {
        for (var i = 0; i < count; i++) {
            //We shine a virtual laster from a start point
            var rayPos = new Vector3(Start.x + Size.x * Random.value, MaxHeight, Start.y + Size.y * Random.value);
            
            //If that laser hits nothing, we stop here
            if (!Physics.Raycast(rayPos, Vector3.down, out var hit, MaxHeight)) continue;
            
            //If it hit an object on an invalid layer, we also stop here
            if (!MaskContainsLayer(ValidLayers, hit.collider.gameObject.layer)) continue;

            //But if it *does* hit something, a bunch of information about the ray hit is stored in the 'hit' variable
            PlaceTreeAt(hit.point);
        }
    }

    //Given a position that we successfully touch the terrain at, 
    public void PlaceTreeAt(Vector3 position)
    {
        //Spawns a random prefab at the provided position - and gives it a random rotation and scale
        var newObj = Instantiate(TreePrefabs[Random.Range(0, TreePrefabs.Length)]);
        newObj.transform.parent = transform;
        newObj.transform.position = position;
        newObj.transform.eulerAngles = new Vector3(0f, Random.value * 360f, 0f);
        newObj.transform.localScale = Vector3.one * Random.Range(1f - SizeVariance, 1f + SizeVariance);
        
        //We need to do this to make sure that our new object will be seen by other raycasts etc. this frame
        Physics.SyncTransforms();
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
    
    //I swear I've rewritten this function so many times, so now I just copy-paste it whenever I need it
    //Don't worry if you don't understand it, bitwise coding is used for basically nothing other than this specific purpose
    public static bool MaskContainsLayer(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
}
