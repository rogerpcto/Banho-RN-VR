Shader "Custom/Agua"
{
    Properties
    {
        _Color ("Water Color", Color) = (0.2, 0.5, 1.0, 0.5)
        _EllipseCenter ("Ellipse Center (World)", Vector) = (0, 0, 0)
        _EllipseRadii ("Ellipse Radii (X, Y)", Vector) = (1, 1, 0)
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _NoiseScale ("Noise Scale", Float) = 1.0
        _Speed ("Wave Speed", Float) = 0.5
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 200

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

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
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _NoiseTex;
            float4 _Color;
            float4 _EllipseCenter;  // Centro da elipse (em coordenadas de mundo)
            float2 _EllipseRadii;   // Raios da elipse nos eixos X e Z
            float _NoiseScale;
            float _Speed;

            // Vertex Shader
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            // Fragment Shader
            fixed4 frag (v2f i) : SV_Target
            {
                // Converte para coordenadas locais em relação ao centro da elipse
                float2 relativePos = float2(i.worldPos.x - _EllipseCenter.x, i.worldPos.z - _EllipseCenter.z);

                // Fórmula da elipse: (x^2 / a^2) + (z^2 / b^2) <= 1
                float ellipseEquation = pow(relativePos.x / _EllipseRadii.x, 2) + pow(relativePos.y / _EllipseRadii.y, 2);
                if (ellipseEquation > 1.0)
                {
                    discard; // Descarte pixels fora da elipse
                }

                // Movimento da textura de noise para simular ondas
                float2 noiseUV = i.uv * _NoiseScale + _Time.y * _Speed;
                float noiseValue = tex2D(_NoiseTex, noiseUV).r;

                // Aplica a cor com base no noise
                fixed4 col = _Color;
                col.a *= noiseValue; // Transparência baseada no noise
                return col;
            }
            ENDCG
        }
    }

    FallBack "Transparent"
}
