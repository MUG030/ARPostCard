Shader "Custom/Snow"
{
    Properties {
        _BaseMap ("Albedo (RGB)", 2D) = "white" {}
        _BaseColor ("Color", Color) = (1,1,1,1)
        _Smoothness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Snow("Snow", Range(0,2))= 0.0
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
                float3 normal       : NORMAL;
            };

            struct Varyings {
                float4 positionHCS  : SV_POSITION;
                float2 uv           : TEXCOORD0;
                float3 worldNormal  : NORMAL;
            };

            sampler2D _BaseMap;
            float4 _BaseColor;
            half _Smoothness;
            half _Metallic;
            half _Snow;

            Varyings vert(Attributes input) {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                // output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                output.uv = input.uv;
                output.worldNormal = TransformObjectToWorldNormal(input.normal);
                return output;
            }

            half4 frag(Varyings input) : SV_Target {
                half3 albedo = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv).rgb * _BaseColor.rgb;
                half3 normal = input.worldNormal;
                half d = dot(normal, half3(0, 1, 0));
                half4 white = half4(1,1,1,1);
                albedo = lerp(albedo, white, d*_Snow);
                return half4(albedo, 1);
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit"
}
