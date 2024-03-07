Shader "CustomRenderTexture/Diffuse"
{
    /*Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }*/
    Properties {
        _BaseMap ("Albedo (RGB)", 2D) = "white" {}
        _BaseColor ("Color", Color) = (1,1,1,1)
        _Smoothness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Snow("Snow", Range(0,2))= 0.0
    }
    SubShader
    {
        Tags {
            "RenderType"="Opaque"
            "RenderPipeline"="UniversalPipeline"
        }
        LOD 100

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float fogFactor: TEXCOORD1;
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            CBUFFER_END

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.fogFactor = ComputeFogFactor(o.vertex.z);
                o.normal = TransformObjectToWorldNormal(v.normal);
                return o;
            }

            /*float4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                // ライト情報を取得
                Light light = GetMainLight();

                // ピクセルの法線とライトの方向の内積を計算する
                float t = dot(i.normal, light.direction);
                // 内積の値を0以上の値にする
                t = max(0, t);
                // 拡散反射光を計算する
                float3 diffuseLight = light.color * t;

                // 拡散反射光を反映
                col.rgb *= diffuseLight;
                // apply fog
                col.rgb = MixFog(col.rgb, i.fogFactor);
                return col;
            }*/

            sampler2D _BaseMap;
            float4 _BaseColor;
            float _Smoothness;
            float _Metallic;
            float _Snow;

            float4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 albedo = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv);
                // apply base color
                albedo *= _BaseColor;

                // get lighting information
                Light light = GetMainLight();

                // calculate the dot product of the pixel's normal and the light's direction
                float t = dot(i.normal, light.direction);
                // clamp the dot product to a value of 0 or higher
                t = max(0, t);
                // calculate the diffuse light
                float3 diffuseLight = light.color * t;

                // apply diffuse light
                albedo.rgb *= diffuseLight;

                // apply fog
                albedo.rgb = MixFog(albedo.rgb, i.fogFactor);

                // Snow effect
                half d = dot(i.normal, half3(0, 1, 0));
                half4 white = half4(1,1,1,1);
                albedo.rgb = lerp(albedo.rgb, white.rgb, d*_Snow);

                // apply metallic and smoothness
                albedo.a = _Metallic;
                albedo.g = _Smoothness;

                return albedo;
            }
            ENDHLSL
        }
    }
}
