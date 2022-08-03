using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Shelf", menuName="ProcGen/Shelf", order=100)]
public class Shelf : ScriptableObject
{
    // This class lets me assign arbitrary data to a prefab GameObject, which makes other parts of my code cleaner and more intuitive
    // See ShelfSpawner for an example of how I use this class.
    public GameObject Prefab;
    public float Width;
    public float Depth;
    public float[] ShelfLevels;
    public float ShelfHeight;
}
