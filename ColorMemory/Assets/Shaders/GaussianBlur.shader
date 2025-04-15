Shader "Custom/MaskedUIBlur"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _BlurRadius ("Blur Radius", Range(0.0, 10.0)) = 2.0
    }

    SubShader
    {
        Tags { "Queue" = "Overlay" }
        Pass
        {
            // 1. Stencil Buffer ���� (UI Masking�� �°� Stencil ���)
            Stencil
            {
                Ref 1
                Comp Equal
                Pass Keep
            }

            // 2. �� ��꿡 �ʿ��� �Ű����� ����
            ZWrite Off
            ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _BlurRadius;

            // ���� ����ü
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

            // �⺻ ���� ���̴�
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // ����þ� �� �Լ� (1D ���� ��)
            float Gaussian(float x, float sigma)
            {
                return exp(-(x * x) / (2.0 * sigma * sigma)) / (sigma * sqrt(2.0 * UNITY_PI));
            }

            float4 frag(v2f i) : SV_Target
            {
                float2 texelSize = 1.0 / _ScreenParams.xy;

                float4 col = float4(0, 0, 0, 0);
                float totalWeight = 0.0;

                // 1D ����þ� �� (���� ����)
                for (int x = -5; x <= 5; ++x)
                {
                    float weight = Gaussian(x, _BlurRadius);
                    float2 offset = float2(x, 0) * texelSize * _BlurRadius;
                    col += tex2D(_MainTex, i.uv + offset) * weight;
                    totalWeight += weight;
                }

                return col / totalWeight;
            }
            ENDCG
        }

        // �� ��° Pass: ���� �� (���� ���� �̾ ����)
        Pass
        {
            Stencil
            {
                Ref 1
                Comp Equal
                Pass Keep
            }

            ZWrite Off
            ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _BlurRadius;

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // ���� ���� ��
            float4 frag(v2f i) : SV_Target
            {
                float2 texelSize = 1.0 / _ScreenParams.xy;

                float4 col = float4(0, 0, 0, 0);
                float totalWeight = 0.0;

                // ���� ���� ��
                for (int y = -5; y <= 5; ++y)
                {
                    float weight = Gaussian(y, _BlurRadius);
                    float2 offset = float2(0, y) * texelSize * _BlurRadius;
                    col += tex2D(_MainTex, i.uv + offset) * weight;
                    totalWeight += weight;
                }

                return col / totalWeight;
            }
            ENDCG
        }
    }

    FallBack "UI/Default"
}
