using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfSpawner : MonoBehaviour
{
    public Shelf Shelf;
    public ShelfObject[] Objects;
    [Range(0f, 0.25f)] public float Gap = 0.05f;
    [Range(0f, 0.25f)] public float JitterZ = 0.05f;
    [Range(0f, 90f)] public float RandomRotation = 5f;

    [ContextMenu("Test Spawn")]
    public void TestSpawn()
    {
        SpawnShelf(Shelf, Objects);
    }

    [ContextMenu("Spawn Lots")]
    public void SpawnLots()
    {
        
    }

    public void SpawnShelf(Shelf shelf, ShelfObject[] objects)
    {
        //Place the shelf at a random position and rotation on the ground
        var shelfPos = Random.insideUnitCircle * 10f;
        var shelfObj = Instantiate(shelf.Prefab);
        shelfObj.transform.position = new Vector3(shelfPos.x, 0f, shelfPos.y);
        shelfObj.transform.rotation = Quaternion.Euler(0f, Random.value * 360f, 0f);
        shelfObj.transform.parent = transform;
        
        //For each actual shelf in the set of shelves
        for (var i = 0; i < shelf.ShelfLevels.Length; i++) {
            
            //We walk along the shelf's width until we run out of space
            var x = -shelf.Width * 0.5f;
            var spaceRemaining = shelf.Width;
            while (spaceRemaining > 0f) {
                var validObjects = GetObjectsThatFit(objects, spaceRemaining, shelf.ShelfHeight);
                //If there are no more objects that fit in the provided space, we stop the loop here
                if (validObjects.Count == 0) break;
                var nextObject = validObjects[Random.Range(0, validObjects.Count)];
                var pos = x + nextObject.Size.x * 0.5f;

                //Place the object into the shelf!
                var newObj = Instantiate(nextObject.Prefab);
                newObj.transform.parent = shelfObj.transform;
                newObj.transform.localPosition = new Vector3(pos, shelf.ShelfLevels[i], Random.Range(-JitterZ, JitterZ) * 0.5f) + nextObject.Center;
                newObj.transform.localRotation = Quaternion.Euler(0f, Random.Range(-RandomRotation, RandomRotation) * 0.5f, 0f);

                x += nextObject.Size.x + Gap;
                spaceRemaining -= nextObject.Size.x + Gap;
            }
        }
    }

    //Utility method that makes our larger function a little easier to read
    private List<ShelfObject> GetObjectsThatFit(ShelfObject[] objects, float spaceRemaining, float height)
    {
        var output = new List<ShelfObject>();
        foreach (var obj in objects) {
            if(obj.Size.x <= spaceRemaining && obj.Size.y <= height) {
                output.Add(obj);
            }
        }
        return output;
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
