// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/SpriteOutline" {
  
    Properties
    {
       _MainTex ("Sprite Texture", 2D) = "white" {}
       _Color ("Alpha Color Key", Color) = (0,0,0,1)
    }
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Opaque" 
            "Queue" = "Transparent+1" 
        }
 
        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha 
 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile DUMMY PIXELSNAP_ON
 
            sampler2D _MainTex;
            float4 _Color;
 
            struct Vertex
            {
                float4 vertex : POSITION;
                float2 uv_MainTex : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
            };
 
            struct Fragment
            {
                float4 vertex : POSITION;
                float2 uv_MainTex : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
            };
 
            Fragment vert(Vertex v)
            {
                Fragment o;
 
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv_MainTex = v.uv_MainTex;
                o.uv2 = v.uv2;
 
                return o;
            }
 
            float4 frag(Fragment IN) : COLOR
            {
                float4 o = float4(1, 0, 0, 0.2);
 
                half4 c = tex2D (_MainTex, IN.uv_MainTex);
                o.rgba = c.rgba;
                if(c.a > 0.05 && c.a < 0.85)
                {
                    o = float4(1, 0, 0, 0.2);
                }
                else
                {
                    
                }
 
                return o;
            }
 
            ENDCG
        }
    }
}