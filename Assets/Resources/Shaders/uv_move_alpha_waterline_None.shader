// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:33100,y:32666,varname:node_3138,prsc:2|custl-6350-OUT;n:type:ShaderForge.SFN_Tex2d,id:3553,x:32326,y:32518,ptovrint:False,ptlb:mask,ptin:_mask,varname:node_3553,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-562-UVOUT;n:type:ShaderForge.SFN_Multiply,id:999,x:32550,y:32701,varname:node_999,prsc:2|A-3553-RGB,B-8839-RGB;n:type:ShaderForge.SFN_Panner,id:562,x:32123,y:32539,varname:node_562,prsc:2,spu:0,spv:1|UVIN-7414-UVOUT,DIST-9010-OUT;n:type:ShaderForge.SFN_TexCoord,id:7414,x:32123,y:32325,varname:node_7414,prsc:2,uv:0;n:type:ShaderForge.SFN_Time,id:8136,x:31746,y:32483,varname:node_8136,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:8628,x:32470,y:33056,ptovrint:False,ptlb:node_8628,ptin:_node_8628,varname:node_8628,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:6350,x:32794,y:32887,varname:node_6350,prsc:2|A-999-OUT,B-8628-RGB;n:type:ShaderForge.SFN_Slider,id:795,x:31589,y:32653,ptovrint:False,ptlb:Mask move,ptin:_Maskmove,varname:node_795,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-5,cur:0,max:5;n:type:ShaderForge.SFN_Multiply,id:9010,x:31934,y:32561,varname:node_9010,prsc:2|A-8136-T,B-795-OUT;n:type:ShaderForge.SFN_Tex2d,id:8839,x:32117,y:32805,ptovrint:False,ptlb:maintexture,ptin:_maintexture,varname:node_8839,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:1046,x:33246,y:33589,varname:node_1046,prsc:2;proporder:3553-8628-795-8839;pass:END;sub:END;*/

Shader "Shader Forge/uv_move_alpha_waterline_None" {
    Properties {
        _mask ("mask", 2D) = "white" {}
        _node_8628 ("node_8628", 2D) = "white" {}
        _Maskmove ("Mask move", Range(-5, 5)) = 0
        _maintexture ("maintexture", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#ifndef UNITY_PASS_FORWARDBASE
			#define UNITY_PASS_FORWARDBASE
			#endif
            //#define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _mask; uniform float4 _mask_ST;
            uniform sampler2D _node_8628; uniform float4 _node_8628_ST;
            uniform float _Maskmove;
            uniform sampler2D _maintexture; uniform float4 _maintexture_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
                float4 node_8136 = _Time + _TimeEditor;
                float2 node_562 = (i.uv0+(node_8136.g*_Maskmove)*float2(0,1));
                float4 _mask_var = tex2D(_mask,TRANSFORM_TEX(node_562, _mask));
                float4 _maintexture_var = tex2D(_maintexture,TRANSFORM_TEX(i.uv0, _maintexture));
                float4 _node_8628_var = tex2D(_node_8628,TRANSFORM_TEX(i.uv0, _node_8628));
                float3 finalColor = ((_mask_var.rgb*_maintexture_var.rgb)*_node_8628_var.rgb);
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
