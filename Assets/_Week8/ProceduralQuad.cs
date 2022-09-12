using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ProceduralQuad : MonoBehaviour
{
    private Mesh _mesh;
    private Vector3[] _verts;
    private Vector3[] _normals;
    private Color[] _colors;
    private Vector2[] _uvs;
    private int[] _indices;

    private MeshFilter _mf;
    private int _vertexCount;

    public void Initialize()
    {
        _mesh = new Mesh();
        var vertexCount = 4;
        _verts = new Vector3[vertexCount];
        _normals = new Vector3[vertexCount];
        _colors = new Color[vertexCount];
        _uvs = new Vector2[vertexCount];
        _indices = new int[2 * 3]; //two tris per cell, three indices per tri

        _mf = GetComponent<MeshFilter>();
        _mf.sharedMesh = _mesh;

        _vertexCount = vertexCount;
    }

    public void Recalculate()
    {
        if(_mf == null) Initialize();
        Initialize();

        _verts[0] = new Vector3(-0.5f, 0f, -0.5f);
        _verts[1] = new Vector3(-0.5f, 0f, 0.5f);
        _verts[2] = new Vector3(0.5f, 0f, 0.5f);
        _verts[3] = new Vector3(0.5f, 0f, -0.5f);

        _uvs[0] = new Vector2(0f, 0f);
        _uvs[1] = new Vector2(0f, 1f);
        _uvs[2] = new Vector2(1f, 1f);
        _uvs[3] = new Vector2(1f, 0f);
        _colors[0] = Color.red;
        _colors[1] = Color.green;
        _colors[2] = Color.yellow;
        _colors[3] = Color.blue;
        
        for (var i = 0; i < 4; i++) {
            _normals[i] = Vector3.up;
        }

        _indices[0] = 0;
        _indices[1] = 1;
        _indices[2] = 2;
        
        _indices[3] = 0;
        _indices[4] = 2;
        _indices[5] = 3;
    }

    public void Assign()
    {
        if (_mesh == null) Initialize();
        
        _mesh.SetVertices(_verts);
        _mesh.SetNormals(_normals);
        _mesh.SetColors(_colors);
        _mesh.SetUVs(0, _uvs);
        _mesh.SetIndices(_indices, MeshTopology.Triangles, 0);
        
        _mesh.RecalculateBounds();
        _mesh.RecalculateTangents();
        _mesh.RecalculateNormals();
    }

    public void Update()
    {
        Recalculate();
        Assign();
    }
}
