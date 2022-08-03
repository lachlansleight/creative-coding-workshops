using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Similar to Shelf, this class lets me precalculate the mesh size and offset ahead of time, which makes it far easier to place items on shelves

[CreateAssetMenu(fileName="ShelfObject", menuName="ProcGen/Shelf Object", order=101)]
public class ShelfObject : ScriptableObject
{
    public GameObject Prefab;
    public Vector3 Size;
    public Vector3 Center;

    [ContextMenu("Calculate Vectors")]
    public void CalculateVectors()
    {
        var mesh = Prefab.GetComponent<MeshFilter>().sharedMesh;
        Size = mesh.bounds.size;
        Center = mesh.bounds.center;
        Center.y = -mesh.bounds.min.y;
    }
}
