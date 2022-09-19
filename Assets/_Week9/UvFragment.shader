Shader "Unlit/BasicFragment"
{
    Properties
    {
        _Color ("Color", Color) = (1, 0, 0, 1)
        _FrequencyA ("Frequency A", Float) = 5
        _SpeedA ("Speed A", Float) = 1
        _FrequencyB ("Frequency B", Float) = 5
        _SpeedB ("Speed B", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _Color;
            float _FrequencyA;
            float _FrequencyB;
            float _SpeedA;
            float _SpeedB;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float tA = length(i.uv - float2(0.25, 0.5)) * _FrequencyA + _Time.y * _SpeedA;
                float valA = sin(tA) * 0.5 + 0.5;
                
                float tB = length(i.uv - float2(0.75, 0.5)) * _FrequencyB + _Time.y * _SpeedB;
                float valB = sin(tB) * 0.5 + 0.5;
                
                //add together, then multiply with main color
                return float4((valA + valB) * float3(1, 1, 1), 1) * _Color;
            }
            ENDCG
        }
    }
}
