Shader "Custom/2DLayerOutline"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness("Outline Thickness", Range(0.0, 0.1)) = 0.005
    }
        SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
            LOD 100

            Pass
            {
                Name "OUTLINE"
                Tags { "LightMode" = "Always" }

                Cull Off
                ZWrite Off
                Blend SrcAlpha OneMinusSrcAlpha

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    float4 color : COLOR;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                    float4 color : COLOR;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _OutlineColor;
                float _OutlineThickness;

                v2f vert(appdata_t v)
                {
                    v2f o;

                    // モデル空間でオフセットを計算
                    float3 offset = normalize(mul((float3x3)unity_WorldToObject, v.vertex.xyz)) * _OutlineThickness;
                    v.vertex.xyz += offset;

                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.color = v.color;
                    return o;
                }

                half4 frag(v2f i) : SV_Target
                {
                    half4 c = tex2D(_MainTex, i.uv);
                    if (c.a == 0)
                    {
                        discard;
                    }
                    return half4(_OutlineColor.rgb, _OutlineColor.a * i.color.a);
                }
                ENDCG
            }

            Pass
            {
                Name "TEXTURE"
                Tags { "LightMode" = "Always" }

                Cull Back
                ZWrite Off
                Blend SrcAlpha OneMinusSrcAlpha

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    float4 color : COLOR;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                    float4 color : COLOR;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.color = v.color;
                    return o;
                }

                half4 frag(v2f i) : SV_Target
                {
                    half4 c = tex2D(_MainTex, i.uv) * i.color;
                    return c;
                }
                ENDCG
            }
        }
}
