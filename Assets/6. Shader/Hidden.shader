Shader "Custom/Standard_With_Stencil"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        
        [NoScaleOffset] _BumpMap ("Normal Map (입체 디테일)", 2D) = "bump" {}
    }
    SubShader
    {
        // 돋보기가 먼저 그려진 후(Geometry) 그 위에 그려지도록 설정
        Tags { "RenderType"="Opaque" "Queue"="Transparent+1" }
        LOD 200

        // 입체적인 3D 물체이므로 앞뒤 관계를 명확히 하기 위해 ZWrite를 켭니다.
        ZWrite On 

        Stencil
        {
            Ref 1
            Comp Equal
            Pass Keep
        }

        CGPROGRAM
        // 유니티 정석 Standard 라이팅 시스템을 그대로 사용합니다 (빛 반사, 그림자 완벽 지원)
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpMap;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // 1. 기존 텍스처와 색상 적용
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            
            // 2. 금속 재질 및 부드러움(반사광) 적용
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
            
            // 3. 입체감을 주는 노멀맵 적용
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
        }
        ENDCG
    }
    FallBack "Diffuse"
}