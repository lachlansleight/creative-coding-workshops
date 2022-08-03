using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShelfObject))]
public class ShelfObjectEditor : Editor
{
    private SerializedProperty _prefab;
    private SerializedProperty _size;
    private SerializedProperty _center;

    //We need to get the properties OnEnable - which is fired when this custom inspector first appears
    private void OnEnable()
    {
        _prefab = serializedObject.FindProperty("Prefab");
        _size = serializedObject.FindProperty("Size");
        _center = serializedObject.FindProperty("Center");
    }

    public override void OnInspectorGUI()
    {
        //This has to come first
        serializedObject.Update();
        
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(_prefab, new GUIContent("Prefab"));
        if (EditorGUI.EndChangeCheck()) { //If anything between the changed checks changed, this is run
            //We get the mesh's bounds and size and populate those properties with that information
            var mesh = ((GameObject)_prefab.objectReferenceValue).GetComponent<MeshFilter>().sharedMesh;
            var size = mesh.bounds.size;
            var center = mesh.bounds.center;
            center.y = -mesh.bounds.min.y;
            _size.vector3Value = size;
            _center.vector3Value = center;
        }

        //Show the size and center properties, but in read-only mode
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(_size, new GUIContent("Size"));
        EditorGUILayout.PropertyField(_center, new GUIContent("Center"));
        EditorGUI.EndDisabledGroup();

        //This has to come last
        serializedObject.ApplyModifiedProperties();
    }
}
