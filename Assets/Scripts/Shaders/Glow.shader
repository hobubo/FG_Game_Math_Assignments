Shader "Unlit/Glow"
{
    Properties
    {
        _FGColor ("Foreground Color", Color) = (1,1,1,1)
        _BGColor ("Background Color", Color) = (1,1,1,1)
        _Speed ("Animation Speed", float) = 4.0
        _StripeSize ("StripeSize", float) = 100
        _BoxSize ("Box Size", Vector) = (0.6, 0.6, 0,0)
        _Range ("Range", Range(0,1)) = 0.3
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        ZWrite Off
        ZTest Less
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

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

            fixed4 _BGColor;
            fixed4 _FGColor;
            fixed2 _BoxSize;
            fixed _StripeSize;
            fixed _Speed;
            fixed _Range;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Obligatory Inigo Quilez ripoff
                // https://iquilezles.org/articles/distfunctions2d/
                fixed2 uv = (2.0*i.uv) - 1;
                fixed2 dist = abs(uv) - _BoxSize;
                fixed outerDist = length(max(dist, 0.0));
                if(outerDist <= 0.0) discard;
                fixed square = outerDist + length(min(max(dist.x, dist.y), 0.0));
                fixed animated = sin(square*_StripeSize-_Time.y*_Speed);
                animated = smoothstep(-1.0, 1.0, animated);
                fixed alpha = clamp(lerp(0.0, 1.0, _Range-square),0.0,1.0);
                //return fixed4(alpha,alpha,alpha,1);
                fixed4 finalColor = lerp(_BGColor, _FGColor, animated);
                return fixed4(finalColor.rgb, finalColor.w*alpha);
            }
            ENDCG
        }
    }
}
