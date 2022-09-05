using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ProceduralPlane : MonoBehaviour
{

    public int Cells = 4;
    
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
        var vertexCount = (Cells + 1) * (Cells + 1);
        _verts = new Vector3[vertexCount];
        _normals = new Vector3[vertexCount];
        _colors = new Color[vertexCount];
        _uvs = new Vector2[vertexCount];
        _indices = new int[Cells * Cells * 2 * 3]; //two tris per cell, three indices per tri

        _mf = GetComponent<MeshFilter>();
        _mf.sharedMesh = _mesh;

        _vertexCount = vertexCount;
    }

    public void Recalculate()
    {
        if(_mf == null) Initialize();
        if ((Cells + 1) * (Cells + 1) != _vertexCount) Initialize();

        for (var y = 0; y < Cells + 1; y++) {
            var yPos = Mathf.Lerp(-0.5f, 0.5f, Mathf.InverseLerp(0, Cells + 1, y));
            for (var x = 0; x < Cells + 1; x++) {
                var xPos = Mathf.Lerp(-0.5f, 0.5f, Mathf.InverseLerp(0, Cells + 1, x));
                var index = x + y * (Cells + 1);
                _verts[index] = new Vector3(xPos, 0f, yPos);
                _normals[index] = Vector3.up;
                _uvs[index] = new Vector2(xPos + 0.5f, yPos + 0.5f);
                _colors[index] = Color.white;
            }
        }

        for (var i = 0; i < Cells; i++) {
            for (var j = 0; j < Cells; j++) {
                var tri = (j + i * Cells) * 6;
                var v = j + (i * (Cells + 1));
                
                _indices[tri + 0] = v + 0;
                _indices[tri + 1] = v + Cells + 2;
                _indices[tri + 2] = v + 1;
                
                _indices[tri + 3] = v + 0;
                _indices[tri + 4] = v + Cells + 1;
                _indices[tri + 5] = v + Cells + 2;
            }
        }
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
