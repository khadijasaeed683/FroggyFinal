Shader "TextMeshPro/Bitmap" {

	Properties
    {
        _MainTex ("Font Atlas", 2D) = "white" {}
        _FaceTex ("Font Texture", 2D) = "white" {}
        [HDR]_FaceColor ("Text Color", Color) = (1,1,1,1)
        _DotSize ("Dot Size", Float) = 0.1
        _GlowIntensity ("Glow Intensity", Float) = 1.0
        _ClipRect("Clip Rect", vector) = (-32767, -32767, 32767, 32767)
        _StencilComp("Stencil Comparison", Float) = 8
        _Stencil("Stencil ID", Float) = 0
        _StencilOp("Stencil Operation", Float) = 0
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255
        _CullMode("Cull Mode", Float) = 0
        _ColorMask("Color Mask", Float) = 15
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" "IgnoreProjector" = "True" "RenderType" = "Transparent" }

        Stencil
        {
            Ref[_Stencil]
            Comp[_StencilComp]
            Pass[_StencilOp]
            ReadMask[_StencilReadMask]
            WriteMask[_StencilWriteMask]
        }

        Lighting Off
        Cull [_CullMode]
        ZTest [unity_GUIZTestMode]
        ZWrite Off
        Fog { Mode Off }
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask[_ColorMask]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float4 mask : TEXCOORD2;
            };

            uniform sampler2D _MainTex;
            uniform sampler2D _FaceTex;
            uniform float4 _FaceTex_ST;
            uniform fixed4 _FaceColor;
            uniform float _DotSize;
            uniform float _GlowIntensity;
            uniform float4 _ClipRect;

            v2f vert (appdata_t v)
            {
                float4 vert = v.vertex;
                vert.xy += (vert.w * 0.5) / _ScreenParams.xy;
                float4 vPosition = UnityPixelSnap(UnityObjectToClipPos(vert));

                fixed4 faceColor = v.color;
                faceColor *= _FaceColor;

                v2f OUT;
                OUT.vertex = vPosition;
                OUT.color = faceColor;
                OUT.texcoord0 = v.texcoord0;
                OUT.texcoord1 = TRANSFORM_TEX(v.texcoord1, _FaceTex);

                // Calculate mask for circular dots
                float2 center = (v.vertex.xy / _ScreenParams.xy) * 0.5 + 0.5;
                float distance = length(v.vertex.xy - center);
                float dot = smoothstep(_DotSize, _DotSize - 0.01, distance);
                float glow = smoothstep(0.0, 0.1, 0.1 - distance);
                OUT.mask = float4(dot, glow, 0, 1);

                return OUT;
            }

            fixed4 frag (v2f IN) : SV_Target
            {
                fixed4 color = tex2D(_MainTex, IN.texcoord0);
                color = fixed4(tex2D(_FaceTex, IN.texcoord1).rgb * IN.color.rgb, IN.color.a * color.a);

                // Apply circular dot and glow effect
                fixed4 glowColor = float4(1, 1, 1, 1) * (IN.mask.y * _GlowIntensity);
                fixed4 dotColor = float4(1, 1, 1, 1) * IN.mask.x;

                color = lerp(color, glowColor, glowColor.a);
                color *= dotColor;

                // Clip and apply clipping rectangle
                #if UNITY_UI_CLIP_RECT
                    half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(IN.mask.xy)) * IN.mask.zw);
                    color *= m.x * m.y;
                #endif

                #if UNITY_UI_ALPHACLIP
                    clip(color.a - 0.001);
                #endif

                return color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
