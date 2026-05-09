Shader "Custom/Hidden_Reader_Color"
{
    Properties 
    { 
        _Color ("Object Color", Color) = (1, 0, 0, 1) 
    }
    
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent+1" }
        
        Pass
        {
            ZWrite Off
            
            Stencil
            {
                Ref 1
                Comp Equal
            }
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            
            struct appdata 
            { 
                float4 vertex : POSITION; 
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
            struct v2f 
            { 
                float4 pos : SV_POSITION; 
                UNITY_VERTEX_OUTPUT_STEREO
            };
            
            float4 _Color;
            
            v2f vert(appdata v) 
            { 
                v2f o; 
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.pos = UnityObjectToClipPos(v.vertex); 
                return o; 
            }
            
            fixed4 frag(v2f i) : SV_Target 
            { 
                return _Color; 
            }
            ENDCG
        }
    }
}