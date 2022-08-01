using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//We'll do this in week 3 - but this is a custom inspector that allows us to change parameters on the fly!
//It also means we don't have to keep right-clicking for the context menu to run our functions
[CustomEditor(typeof(ObjectSpawner_3))]
public class ObjectSpawnerEditor : Editor
{
    private bool _autoChange;

    public override void OnInspectorGUI()
    {
        var spawner = (ObjectSpawner_3)target;

        _autoChange = EditorGUILayout.Toggle("Auto Change", _autoChange);

        EditorGUI.BeginChangeCheck();
        base.OnInspectorGUI();
        if(EditorGUI.EndChangeCheck())
        {
            if(_autoChange) spawner.SpawnObjects(spawner.Seed);
        }

        EditorGUILayout.Space(10f);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Spawn Objects (New Seed)")) {
            spawner.SpawnObjects();
        }
        if (GUILayout.Button("Respawn Objects (Same Seed)")) {
            spawner.SpawnObjects(spawner.Seed);
        }
        if (GUILayout.Button("Colorize Objects")) {
            spawner.ColorizeObjects();
        }
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Clear Objects")) {
            spawner.ClearObjects();
        }
    }
}
