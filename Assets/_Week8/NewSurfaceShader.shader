Shader "Custom/NewSurfaceShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        
        _OffsetAmount ("Offset Amount", Float) = 0.3
        _OffsetWavelength ("Offset Wavelength", Float) = 10
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert
        #pragma target 3.0

        sampler2D _MainTex;
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float _OffsetAmount;
        float _OffsetWavelength;
        
        struct Input
        {
            float2 uv_MainTex;
        };

        float3 getPosition(float3 input) {
            float3 o = input;
            o += _OffsetAmount * float3(0, 1, 0) * sin(_Time.y + length(input) * _OffsetWavelength);
            return o;
        }
        
        void vert (inout appdata_full v) {
            //we can do whatever we want to the input vertex here!
            float3 p = getPosition(v.vertex);
            float3 p0 = getPosition(v.vertex.xyz + float3(-0.0001, 0, 0));
            float3 p1 = getPosition(v.vertex.xyz + float3(0.0001, 0, 0));
            float3 p2 = getPosition(v.vertex.xyz + float3(0, 0, -0.0001));
            float3 p3 = getPosition(v.vertex.xyz + float3(0, 0, 0.0001));
            v.normal = -cross(p1 - p0, p3 - p2);
            v.vertex.xyz = p;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
