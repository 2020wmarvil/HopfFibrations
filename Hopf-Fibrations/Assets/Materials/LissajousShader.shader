Shader "Unlit/LissajousShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _Saturation ("Saturation", Range(0.0, 1.0)) = 0.5
        _Value ("Value", Range(0.0, 1.0)) = 0.5

        _LightDir ("LightDir", Vector) = (.25, .5, .5, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _Saturation;
            float _Value;

            float4 _LightDir;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToClipPos(v.normal);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float hue2rgb(float p, float q, float t) {
                if(t < 0) t += 1;
                if(t > 1) t -= 1;
                if(t < 1.0/6.0) return p + (q - p) * 6 * t;
                if(t < 1.0/2.0) return q;
                if(t < 2.0/3.0) return p + (q - p) * (2.0/3.0 - t) * 6;
                return p;
            }

            float3 hslToRgb(float h){
                float r;
                float g;
                float b;

                if(_Saturation == 0){
                    r = _Value;
                    g = _Value;
                    b = _Value;
                }else{

                    float q = 0;

                    if (_Value < 0.5)
                    {
                        q = _Value * (1 + _Saturation);
                    }
                    else {
                        q = _Value + _Saturation - _Value * _Saturation;
                    }

                    float p = 2 * _Value - q;
                    float OneThird = 1.0 / 3.0;
                    r = hue2rgb(p, q, h + OneThird);
                    g = hue2rgb(p, q, h);
                    b = hue2rgb(p, q, h - OneThird);
                }

                return float3(r, g, b);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 col = hslToRgb(i.uv.x);
                float towardsLight = dot(i.normal, _LightDir);
                towardsLight = towardsLight * 0.5 + 0.5 + 0.2;
                col.rgb *= towardsLight;

                return float4(col, 1);
            }
            ENDCG
        }
    }
}
