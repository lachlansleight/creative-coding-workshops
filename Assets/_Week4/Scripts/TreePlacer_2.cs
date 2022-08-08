using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePlacer_2 : MonoBehaviour
{
    [Header("Positioning")]
    public Vector2 Start = new Vector2(-500f, -500f);
    public Vector2 Size = new Vector2(1000f, 1000f);
    public float MaxHeight = 600f; //this is my terrain height
    public LayerMask ValidLayers;
    
    [Header("Appearance")]
    public TreeSetup[] TreePrefabs;
    public GameObject[] GrassPrefabs;
    [Range(0f, 1f)] public float SizeVariance = 0.1f;
    public int GrassPerTree = 3;
    public float GrassDistanceOffsetMin = 0f;
    public float GrassDistanceOffsetMax = 0.5f;

    [Header("Debug")]
    public int DebugSpawnCount = 1000;

    [ContextMenu("Spawn Debug")]
    public void SpawnDebug()
    {
        Spawn(DebugSpawnCount);
    }

    [ContextMenu("Check Object Counts")]
    public void CheckObjectCounts()
    {
        var totalCount = 0;
        for (var i = 0; i < transform.childCount; i++) {
            totalCount += transform.GetChild(i).childCount;
        }

        Debug.Log($"{transform.childCount} top-level objects, {totalCount} objects in total");
    }
    
    public void Spawn(int count)
    {
        for (var i = 0; i < count; i++) {
            //We choose the tree and scale now, because we need that information to perform the Spherecast
            var tree = TreePrefabs[Random.Range(0, TreePrefabs.Length)];
            var scale = Random.Range(1f - SizeVariance, 1f + SizeVariance);
            
            var rayPos = new Vector3(Start.x + Size.x * Random.value, MaxHeight, Start.y + Size.y * Random.value);
            
            //This time, we do a sphere cast, which is kinda like a 'thick' raycast with a provided radius
            //We use the tree's canopy radius (set in the scriptable object) to make sure that it won't intersect any other trees
            if (!Physics.SphereCast(new Ray(rayPos, Vector3.down), tree.CanopyRadius * scale, out var hit, MaxHeight)) continue;

            //If it hit an object on an invalid layer, we also stop here
            if (!MaskContainsLayer(ValidLayers, hit.collider.gameObject.layer)) continue;

            //But if it *does* hit something, a bunch of information about the ray hit is stored in the 'hit' variable
            var newTree = PlaceTreeAt(tree.Prefab, hit.point, scale);
            
            //We place grass and other detail objects around the base of the tree, using its preconfigured trunk radius
            PlaceGrassAt(newTree.transform, hit.point, tree.TrunkRadius * scale);
        }
    }

    //Given a position that we successfully touch the terrain at, 
    public GameObject PlaceTreeAt(GameObject prefab, Vector3 position, float scale)
    {
        //Spawns a random prefab at the provided position - and gives it a random rotation and scale
        var newObj = Instantiate(prefab);
        newObj.transform.parent = transform;
        newObj.transform.position = position;
        newObj.transform.eulerAngles = new Vector3(0f, Random.value * 360f, 0f);
        newObj.transform.localScale = Vector3.one * scale;
        
        //We need to do this to make sure that our new object will be seen by other raycasts etc. this frame
        Physics.SyncTransforms();

        return newObj;
    }

    public void PlaceGrassAt(Transform parent, Vector3 position, float radius)
    {
        var count = Random.Range(0, GrassPerTree + 1); //Random.Range is non-inclusive of the upper bound
        if (count == 0) return;
        for (var i = 0; i < count; i++) {
            //Spawn a new grass prefab and make the tree its parent
            var newObj = Instantiate(GrassPrefabs[Random.Range(0, GrassPrefabs.Length)]);
            newObj.transform.parent = parent;
            
            //Cast a ray at a random position around the trunk and place the grass at that point
            var angle = Random.value * Mathf.PI * 2f;
            var r = radius + Random.Range(GrassDistanceOffsetMin, GrassDistanceOffsetMax);
            var rayPos = position + new Vector3(r * Mathf.Cos(angle), 0f, r * Mathf.Sin(angle));
            rayPos.y = MaxHeight;
            if (!Physics.Raycast(rayPos, Vector3.down, out var hit, MaxHeight, ValidLayers)) continue;
            newObj.transform.position = hit.point;
            
            //Give the grass a random rotation and scale
            newObj.transform.eulerAngles = new Vector3(0f, Random.value * 360f, 0f);
            newObj.transform.localScale = Vector3.one * Random.Range(1f - SizeVariance, 1f + SizeVariance);
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
    
    public static bool MaskContainsLayer(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
}
