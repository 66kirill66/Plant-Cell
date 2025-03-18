Shader "Custom/LiquidEffectWithTexture"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {} // Основная текстура
        _Speed("Speed", Float) = 0.3        // Скорость анимации
        _WaveStrength("Wave Strength", Float) = 0.05 // Амплитуда волн
        _Color("Color", Color) = (0.5, 0.5, 1, 1)    // Цвет жидкости
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100
            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                sampler2D _MainTex;      // Основная текстура
                float4 _Color;          // Цвет жидкости
                float _Speed;           // Скорость анимации
                float _WaveStrength;    // Амплитуда волн

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                // Вершинный шейдер
                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                // Фрагментный шейдер
                float4 frag(v2f i) : SV_Target
                {
                    float2 uv = i.uv;

                    // Движение волн вдоль осей x и y с учетом времени (_Speed)
                    uv.x += sin(uv.y * 4.0 + _Time.y * _Speed) * _WaveStrength;
                    uv.y += cos(uv.x * 4.0 + _Time.y * _Speed) * _WaveStrength;

                    // Получаем цвет текстуры
                    float4 texColor = tex2D(_MainTex, uv);

                    // Генерация "текучей" текстуры с шумом
                    float noise = frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);

                    // Итоговый цвет: текстура, умноженная на шум и цвет жидкости
                    return float4(texColor.rgb * _Color.rgb * noise, texColor.a);
                }
                ENDCG
            }
        }
}

