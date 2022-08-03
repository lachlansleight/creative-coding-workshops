using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShelfSpawner))]
public class ShelfSpawnerEditor : Editor
{

    private SerializedProperty _gap;

    public void OnEnable()
    {
        _gap = serializedObject.FindProperty("Gap");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var shelfSpawner = (ShelfSpawner)target;
        base.OnInspectorGUI();

        //shelfSpawner.Gap = EditorGUILayout.Slider(shelfSpawner.Gap, 0f, 1f);
        EditorGUILayout.PropertyField(_gap, new GUIContent("Gap"));

        if (GUILayout.Button("Spawn Object"))
        {
            shelfSpawner.TestSpawn();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
