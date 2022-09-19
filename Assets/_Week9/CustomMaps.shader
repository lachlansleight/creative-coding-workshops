Shader "Custom/CustomMaps"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Normal ("Normal", 2D) = "bump" {}
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _ExtraMap ("Extra Map", 2D) = "black" {}
        _ExtraColor ("Extra Color", Color) = (1, 0.5, 0.25, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        sampler2D _Normal;
        sampler2D _ExtraMap;
        fixed4 _ExtraColor;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            float extra = tex2D(_ExtraMap, IN.uv_MainTex).r;
            extra -= 0.05;
            extra = max(0, extra);
            
            o.Albedo = c.rgb;
            o.Normal = UnpackNormal (tex2D (_Normal, IN.uv_MainTex));
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
            
            float tPos = 3 * _Time.y + IN.uv_MainTex.x * 2;
            o.Emission = 50 * extra * _ExtraColor * pow(saturate(sin(tPos)), 2);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
