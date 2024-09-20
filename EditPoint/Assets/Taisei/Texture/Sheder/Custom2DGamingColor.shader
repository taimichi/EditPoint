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

                    // ���[�J����Ԃł̃I�t�Z�b�g�v�Z
                    float3 normal = float3(v.vertex.xy, 0);
                    normal = normalize(normal);
                    v.vertex.xy += normal.xy * _OutlineThickness;

                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.color = _OutlineColor; // �A�E�g���C���̐F���w��
                    return o;
                }

                half4 frag_outline(v2f_outline i) : SV_Target
                {
                    half4 outlineColor = half4(i.color.rgb, 1.0); // �A�E�g���C���̓t���A���t�@

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
                fixed4 _FlashColor; // �_�ł���F���w��

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

                    // ���ԂɊ�Â��ē_�ł���F���v�Z
                    float alpha = 0.5 + 0.5 * sin(_Time.y * 5); // ������5�b�ɐݒ�
                    half4 flashColor = half4(_FlashColor.rgb, _FlashColor.a * alpha);

                    // �e�N�X�`���F�Ɠ_�ŐF����Z���ĕԂ�
                    return texColor * flashColor;
                }
                ENDCG
            }
        }
}
