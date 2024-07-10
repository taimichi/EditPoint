Shader "Custom/2DOutline"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness("Outline Thickness", Range(0.0, 0.03)) = 0.005
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
                    // calculate outline
                    v.vertex.xy += normalize(v.vertex.xy) * _OutlineThickness;
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.color = v.color; // Pass the color to fragment shader
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

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.color = v.color; // Pass the color to fragment shader
                    return o;
                }

                half4 frag(v2f i) : SV_Target
                {
                    half4 c = tex2D(_MainTex, i.uv) * i.color; // Apply vertex color
                    return c;
                }
                ENDCG
            }
        }
}
