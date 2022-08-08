using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="TreeSetup", menuName = "ProcGen/TreeSetup", order=102)]
public class TreeSetup : ScriptableObject
{
    public GameObject Prefab;
    public float TrunkRadius;
    public float CanopyRadius;
}
