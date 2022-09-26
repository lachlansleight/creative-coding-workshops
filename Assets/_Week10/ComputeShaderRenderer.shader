Shader "Custom/ComputeShaderRenderer"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows addshadow vertex:vert
        // Magic function make procedural instancing work
        #pragma instancing_options procedural:proc

        // Note that we've changed this to 4.5, since we're now using DX11 features
        #pragma target 4.5
        
        #include "./ScaryMath.hlsl"
        
#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
        // This is the struct copied directly from our ComputeShader
        struct InstanceData {
            float3 position;
            float4 rotation;
            float scale;
            float3 velocity;
            float4 color;
            float padding;
        };
        StructuredBuffer<InstanceData> _InstanceBuffer;
#endif

        struct Input
        {
            float2 uv_MainTex;
            float4 color: COLOR;
        };

        sampler2D _MainTex;
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        
        //This is kind of a 'pre shader step' that Unity does for each instance
        //Setting unity_ObjectToWorld is kind of like setting transform.position, transform.rotation and transform.scale for a MeshRenderer
        void proc()
        {
        #if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
            InstanceData instance = _InstanceBuffer[unity_InstanceID];
            //GetTransformMatrix comes from ScaryMath.hlsl
            unity_ObjectToWorld = GetTransformMatrix(instance.position, instance.rotation, instance.scale * float3(1, 1, 1));
        #endif
        }
        
        //We can do per-vertex operations here based on our input data
        //For example, if we had a comet mesh we could elongate its tail based on its velocity or something
        void vert (inout appdata_full v) 
        {
        #if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
            InstanceData instance = _InstanceBuffer[unity_InstanceID];
            v.color = instance.color;
        #endif
        }

        //A completely normal surface shader!
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color * IN.color;
            
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
            o.Emission = c.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
