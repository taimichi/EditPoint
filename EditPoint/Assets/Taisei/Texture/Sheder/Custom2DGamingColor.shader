Shader "Custom/2DSelectSeparatedColors"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _OutlineColor("Outline Color", Color) = (0, 0, 0, 1)
        _FlashColor("Flash Color", Color) = (1, 1, 1, 1)
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
                #pragma fragment frag_outline
                #include "UnityCG.cginc"

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    float4 color : COLOR;
                };

                struct v2f_outline
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                    float4 color : COLOR;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float _OutlineThickness;
                fixed4 _OutlineColor;

                v2f_outline vert(appdata_t v)
                {
                    v2f_outline o;

                    // ローカル空間でのオフセット計算
                    float3 normal = float3(v.vertex.xy, 0);
                    normal = normalize(normal);
                    v.vertex.xy += normal.xy * _OutlineThickness;

                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.color = _OutlineColor; // アウトラインの色を指定
                    return o;
                }

                half4 frag_outline(v2f_outline i) : SV_Target
                {
                    half4 outlineColor = half4(i.color.rgb, 1.0); // アウトラインはフルアルファ

                    return outlineColor;
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
                #pragma fragment frag_texture
                #include "UnityCG.cginc"

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    float4 color : COLOR;
                };

                struct v2f_texture
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                    float4 color : COLOR;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                fixed4 _FlashColor; // 点滅する色を指定

                v2f_texture vert(appdata_t v)
                {
                    v2f_texture o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.color = v.color;
                    return o;
                }

                half4 frag_texture(v2f_texture i) : SV_Target
                {
                    half4 texColor = tex2D(_MainTex, i.uv) * i.color;

                    // 時間に基づいて点滅する色を計算
                    float alpha = 0.5 + 0.5 * sin(_Time.y * 5); // 周期を5秒に設定
                    half4 flashColor = half4(_FlashColor.rgb, _FlashColor.a * alpha);

                    // テクスチャ色と点滅色を乗算して返す
                    return texColor * flashColor;
                }
                ENDCG
            }
        }
}
