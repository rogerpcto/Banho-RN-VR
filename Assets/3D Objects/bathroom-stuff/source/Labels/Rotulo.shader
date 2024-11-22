Shader "Custom/Rotulo"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        _MainTex("Texture", 2D) = "white" {}
        _SpecularColor("Specular Color", Color) = (1, 1, 1, 1)
        _Shininess("Shininess", Range(1, 128)) = 16
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            Name "MainPass"
            Tags { "LightMode" = "UniversalForward" }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;    // World-space position
                float3 worldNormal : TEXCOORD2; // World-space normal
            };

            // Uniforms
            sampler2D _MainTex;
            float4 _BaseColor;
            float4 _SpecularColor;
            float _Shininess;
            float4 _MainTex_ST;

            // Vertex Shader
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.worldPos = TransformObjectToWorld(IN.positionOS);
                OUT.worldNormal = TransformObjectToWorldNormal(IN.normalOS);
                return OUT;
            }

            // Fragment Shader
            half4 frag(Varyings IN) : SV_Target
            {
                // Clamp UVs to [0, 1]
                float2 uvClamped = saturate(IN.uv);

                // Sample the texture with clamped UVs
                half4 texColor = tex2D(_MainTex, uvClamped);

                // Check if the UVs are out of bounds
                bool isOutOfBounds = (IN.uv.x < 0.0 || IN.uv.x > 1.0 || IN.uv.y < 0.0 || IN.uv.y > 1.0);

                // Blend between texture and base color based on bounds
                float3 baseColor = isOutOfBounds ? _BaseColor.rgb : texColor.rgb;

                // Normalize the world normal
                float3 normalWS = normalize(IN.worldNormal);

                // Get main light data
                Light mainLight = GetMainLight();

                // Calculate diffuse lighting
                float3 lightDir = normalize(mainLight.direction);
                float diff = max(dot(normalWS, lightDir), 0.0);
                float3 diffuse = baseColor * diff * mainLight.color;

                // Add ambient lighting using Unity's global SH
                float3 ambient = baseColor * SampleSH(normalWS);

                // Calculate specular reflection
                float3 viewDir = normalize(_WorldSpaceCameraPos - IN.worldPos);
                float3 halfDir = normalize(lightDir + viewDir);
                float spec = pow(max(dot(normalWS, halfDir), 0.0), _Shininess);
                float3 specular = _SpecularColor.rgb * spec * mainLight.color;

                // Combine all lighting contributions
                float3 finalColor = diffuse + ambient + specular;

                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }
}
