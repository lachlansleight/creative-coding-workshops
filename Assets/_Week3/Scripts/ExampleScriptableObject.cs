using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class can now be created in the right click 'create asset menu' in the same way you'd make things like materials
// Using ScriptableObjects, you can make any arbitrary data into serialized assets which can be shared, modified, tracked, etc!

[CreateAssetMenu(fileName = "ExampleScriptableObject", menuName = "ExampleScriptableObject", order = 100)]
public class ExampleScriptableObject : ScriptableObject
{
    public float Speed;
}
