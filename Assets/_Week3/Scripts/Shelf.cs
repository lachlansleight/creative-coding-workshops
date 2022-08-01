using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Shelf", menuName="ProcGen/Shelf", order=100)]
public class Shelf : ScriptableObject
{
    public GameObject Prefab;
    public float Width;
    public float Depth;
    public float[] ShelfLevels;
    public float ShelfHeight;
}
