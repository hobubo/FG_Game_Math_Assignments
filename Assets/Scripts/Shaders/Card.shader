Shader "Custom/Card"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Smoothness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        _desaturate("Desaturate", Range(0,1)) = 0.0
        _desaturateTint ("Desaturate Tint", Color) = (1,1,1,1)

        
        _cardQualityMask ("Card Quality Mask", 2D) = "white" {}
        _cardQualityColor ("Card Quality Color", Color) = (1,1,1,1)
        _cardQualitySmoothness ("Card Quality Smoothness", Range(0,1)) = 0.79
        _cardQualityMetallic ("Card Quality Metallic", Range(0,1)) = 1.0
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

        struct Input
        {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        half _Smoothness;
        half _Metallic;

        half _desaturate;
        fixed3 _desaturateTint;

        sampler2D _cardQualityMask;
        fixed4 _cardQualityColor;
        half _cardQualitySmoothness;
        half _cardQualityMetallic;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        fixed3 desaturate(fixed3 colorIn, float amount)
        {
            fixed3 coeff = float3(0.3f, 0.59f, 0.11f);
            float gray = dot(colorIn, coeff);
            return lerp(colorIn, gray*_desaturateTint, amount);
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 col = tex2D(_MainTex, IN.uv_MainTex);

            float t = tex2D(_cardQualityMask, IN.uv_MainTex).r * _cardQualityColor.a;
            fixed3 finalColor = lerp(col.rgb, _cardQualityColor.rgb, t);

            o.Albedo = desaturate(finalColor, _desaturate);
            o.Metallic = lerp(_Metallic, _cardQualityMetallic, t);
            o.Smoothness = lerp(_Smoothness, _cardQualitySmoothness, t);
            o.Alpha = col.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
