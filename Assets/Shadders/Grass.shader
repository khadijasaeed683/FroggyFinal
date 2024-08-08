Shader "Unlit/Grass"
{
    Properties
    {
        _Color ("Color", Color) = (0.5, 1, 0.5, 1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _WaveSpeed ("Wave Speed", Range(0.0, 1.0)) = 0.5
        _WaveFrequency ("Wave Frequency", Range(0.0, 10.0)) = 2.0
        _WaveHeight ("Wave Height", Range(0.0, 1.0)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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

            sampler2D _MainTex;
            float4 _Color;
            float _WaveSpeed;
            float _WaveFrequency;
            float _WaveHeight;


            v2f vert (appdata v)
            {
                v2f o;
                float wave = sin(v.vertex.x * _WaveFrequency + _Time * _WaveSpeed) * _WaveHeight;
                v.vertex.y += wave;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
