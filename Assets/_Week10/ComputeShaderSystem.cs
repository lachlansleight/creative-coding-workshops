using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ComputeShaderSystem : MonoBehaviour
{

    //note that this struct has a multiple of four number of floats in it!
    //it must match *exactly* the struct definition in our compute and rendering shaders
    private struct InstanceData
    {
        public Vector3 position;
        public Quaternion rotation;
        public float scale;
        public Vector3 velocity;
        public Color color;
        //we don't need this for functionality, but we do want our struct to be have a size divisible by four floats
        //why? because your GPU is designed for float4s so it is slightly slower if you give it chunks of data
        //     not divisible by the size of four floats (16 bytes in case you're curious)
        public float padding;
    }

    [Header("Config")]
    public Mesh InstancedMesh;
    public Material TemplateMaterial;
    public ComputeShader TemplateComputeShader;
    [Range(64, 4194240)] public int InstanceCount = 1000;

    [Header("Spawn Settings")]
    public float SpawnRadius = 5f;

    [Header("Simulation Settings")]
    [Range(0f, 24f)] public float VelocityDamping = 0f;
    [Range(0f, 1f)] public float InstanceScale = 0.025f;
    public Vector3 AttractorPosition = Vector3.zero;
    public float AttractorStrength = 0f;

    private ComputeBuffer _instanceBuffer;
    private ComputeShader _compute;
    private Material _material;
    private InstanceData[] _startData;

    //This matches the size of each threadgroup defined in our ComputeShader's numthreads statement
    //If we change that, we have to change this too or everything breaks
    private const int ThreadGroupSize = 64;

    private Bounds _drawBounds;

    public void Awake()
    {
        var structSize = Marshal.SizeOf(typeof(InstanceData)); //just a handy way to get the size of a data type
        
        //This is the compute buffer that holds all of the data used to run our simulation and render instances
        _instanceBuffer = new ComputeBuffer(InstanceCount, structSize, ComputeBufferType.Default);
        //Populate our buffer with starting values
        ResetInitialData();
        
        //We create a new instance of our compute shader (in case we want to run more than one of them at once)
        //and assign the buffer we just created to it
        _compute = Instantiate(TemplateComputeShader);
        _compute.SetBuffer(0, "_InstanceBuffer", _instanceBuffer);
        
        //We do the same for our material
        _material = Instantiate(TemplateMaterial);
        _material.SetBuffer("_InstanceBuffer", _instanceBuffer);
        
        //This is just a trick to allow us to mess with material properties in play mode
        var mr = gameObject.AddComponent<MeshRenderer>();
        mr.material = _material;
        mr.enabled = false;
        
        //set up some basic, large bounds so that our procedural rendering doesn't get culled
        _drawBounds = new Bounds(Vector3.zero, Vector3.one * 1000f);
    }

    
    public void Update()
    {
        //Before we run the simulation, we update any dynamically changing variables on the compute shader
        _compute.SetFloat("_DeltaTime", Time.deltaTime);
        _compute.SetFloat("_VelocityDamping", VelocityDamping); 
        _compute.SetVector("_AttractorPosition", AttractorPosition); 
        _compute.SetFloat("_AttractorStrength", AttractorStrength);
        _compute.SetFloat("_Scale", InstanceScale);
        
        //Run a single simulation step
        //Each thread group has 64 threads (see the numthreads statement in the compute shader), so we need enough
        //thread groups to have one thread per instance (more is OK)
        _compute.Dispatch(0, Mathf.CeilToInt(InstanceCount / (float)ThreadGroupSize), 1, 1);
        
        //Draw the instances!
        Graphics.DrawMeshInstancedProcedural(InstancedMesh, 0, _material, _drawBounds, InstanceCount);
    }

    [ContextMenu("Reset Data")]
    public void ResetInitialData()
    {
        //Creates a big chunk of data and sends it over to the GPU.
        //Note that this takes ages if you have MANY points, so we do it once and store it
        //in case we want to reset the simulation
        if (_startData == null || _startData.Length < InstanceCount) {
            _startData = new InstanceData[InstanceCount];
            for (var i = 0; i < _startData.Length; i++) {
                _startData[i].position = Random.insideUnitSphere * SpawnRadius;
                _startData[i].rotation = Random.rotation;
                _startData[i].scale = InstanceScale;
                _startData[i].velocity = Vector3.zero;
                _startData[i].color = Random.ColorHSV(0f, 1f);
                _startData[i].padding = 0f;
            }
        }
        
        //Actually sends the data to the CPU
        _instanceBuffer.SetData(_startData);
    }

    //We need to do this to ensure that the graphics resources are properly removed
    public void OnDestroy()
    {
        Destroy(_material);
        Destroy(_compute);
        _instanceBuffer.Release();
    }
}
