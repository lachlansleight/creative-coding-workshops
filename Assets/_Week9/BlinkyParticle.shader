Shader "Unlit/BlinkyParticle"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorTint ("Color Tint", Color) = (1, 1, 1, 1)
        _BlinkSpeed ("Blink Speed", Range(0, 10)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100
            
        //Additive Blending
        //Blend One One
        
        //Alpha Blending
        Blend SrcAlpha OneMinusSrcAlpha
        
        //no depth
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float timeOffset : TEXCOORD1;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            fixed4 _ColorTint;
            float _BlinkSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.timeOffset = v.uv.z;
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float blink = sin(100 * i.timeOffset + _Time.y * _BlinkSpeed) * 0.5 + 0.5;
                fixed4 texCol = tex2D(_MainTex, i.uv);
                fixed4 col = texCol * _ColorTint * i.color;
                col.a = blink * texCol.r * texCol.a;
                return col;
            }
            ENDCG
        }
    }
}
