// Shader created with Shader Forge v1.37 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.37;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:1168,x:32719,y:32712,varname:node_1168,prsc:2|emission-9588-RGB;n:type:ShaderForge.SFN_Tex2d,id:9588,x:32408,y:32851,ptovrint:False,ptlb:Texture,ptin:_Texture,varname:node_9588,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-8912-OUT;n:type:ShaderForge.SFN_TexCoord,id:7960,x:31840,y:32818,varname:node_7960,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Time,id:3610,x:31632,y:32981,varname:node_3610,prsc:2;n:type:ShaderForge.SFN_Multiply,id:3255,x:31971,y:32998,varname:node_3255,prsc:2|A-3610-T,B-6647-OUT;n:type:ShaderForge.SFN_Append,id:6647,x:31767,y:33115,varname:node_6647,prsc:2|A-3960-OUT,B-412-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3960,x:31564,y:33210,ptovrint:False,ptlb:Speed_U,ptin:_Speed_U,varname:node_3960,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:412,x:31603,y:33290,ptovrint:False,ptlb:Speed_V,ptin:_Speed_V,varname:node_412,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Add,id:8912,x:32142,y:32874,varname:node_8912,prsc:2|A-7960-UVOUT,B-3255-OUT;proporder:9588-3960-412;pass:END;sub:END;*/

Shader "GameShader/UV Animation" {
    Properties {
        _Texture ("Texture", 2D) = "white" {}
        _Speed_U ("Speed_U", Float ) = 0
        _Speed_V ("Speed_V", Float ) = 0
        _Alpha("Alpha",range(0,1)) = 0.1
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 100
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#ifndef UNITY_PASS_FORWARDBASE
			#define UNITY_PASS_FORWARDBASE
			#endif
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform float _Speed_U;
            uniform float _Speed_V;
            uniform float _Alpha;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 node_3610 = _Time + _TimeEditor;
                float2 node_8912 = (i.uv0+(node_3610.g*float2(_Speed_U,_Speed_V)));
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(node_8912, _Texture));
                float3 emissive = _Texture_var.rgb;
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,_Alpha);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    //CustomEditor "ShaderForgeMaterialInspector"
}
