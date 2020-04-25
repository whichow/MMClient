// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:33200,y:32636,varname:node_3138,prsc:2|custl-3074-OUT;n:type:ShaderForge.SFN_Tex2d,id:4613,x:32420,y:32409,ptovrint:False,ptlb:node_4613,ptin:_node_4613,varname:node_4613,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-9511-UVOUT;n:type:ShaderForge.SFN_Panner,id:9511,x:32065,y:32356,varname:node_9511,prsc:2,spu:0,spv:1|UVIN-2271-UVOUT,DIST-6703-OUT;n:type:ShaderForge.SFN_TexCoord,id:2271,x:31828,y:32296,varname:node_2271,prsc:2,uv:0;n:type:ShaderForge.SFN_Slider,id:7834,x:31371,y:32611,ptovrint:False,ptlb:UV speed,ptin:_UVspeed,varname:node_7834,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-6,cur:0,max:6;n:type:ShaderForge.SFN_Multiply,id:6703,x:31779,y:32483,varname:node_6703,prsc:2|A-9169-T,B-7834-OUT;n:type:ShaderForge.SFN_Time,id:9169,x:31467,y:32462,varname:node_9169,prsc:2;n:type:ShaderForge.SFN_Dot,id:9363,x:32015,y:32722,varname:node_9363,prsc:2,dt:0|A-3090-OUT,B-3911-OUT;n:type:ShaderForge.SFN_ViewReflectionVector,id:3911,x:31731,y:32836,varname:node_3911,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:3090,x:31731,y:32701,prsc:2,pt:False;n:type:ShaderForge.SFN_Slider,id:7548,x:31937,y:32921,ptovrint:False,ptlb:Specular intensity,ptin:_Specularintensity,varname:node_7548,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.3501931,max:5;n:type:ShaderForge.SFN_Multiply,id:8980,x:32260,y:32744,varname:node_8980,prsc:2|A-9363-OUT,B-7548-OUT;n:type:ShaderForge.SFN_Multiply,id:2595,x:32475,y:32719,varname:node_2595,prsc:2|A-8980-OUT,B-7400-OUT;n:type:ShaderForge.SFN_Slider,id:7400,x:32275,y:32917,ptovrint:False,ptlb:Gloss intensity,ptin:_Glossintensity,varname:node_7400,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1.411321,max:6;n:type:ShaderForge.SFN_Add,id:7914,x:32634,y:32628,varname:node_7914,prsc:2|A-4613-RGB,B-2595-OUT;n:type:ShaderForge.SFN_Tex2d,id:6870,x:32343,y:33123,ptovrint:False,ptlb:mask,ptin:_mask,varname:node_6870,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-4397-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:2053,x:32407,y:33484,ptovrint:False,ptlb:maintexture,ptin:_maintexture,varname:node_2053,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-925-UVOUT;n:type:ShaderForge.SFN_Multiply,id:1134,x:32561,y:33220,varname:node_1134,prsc:2|A-6870-RGB,B-2053-RGB;n:type:ShaderForge.SFN_Panner,id:4397,x:32170,y:33171,varname:node_4397,prsc:2,spu:0,spv:1|UVIN-7248-UVOUT,DIST-6900-OUT;n:type:ShaderForge.SFN_Time,id:811,x:31735,y:33249,varname:node_811,prsc:2;n:type:ShaderForge.SFN_Multiply,id:6900,x:31950,y:33266,varname:node_6900,prsc:2|A-811-TSL,B-469-OUT;n:type:ShaderForge.SFN_Slider,id:469,x:31735,y:33415,ptovrint:False,ptlb:node_469,ptin:_node_469,varname:node_469,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-5,cur:0,max:5;n:type:ShaderForge.SFN_TexCoord,id:7248,x:31767,y:33048,varname:node_7248,prsc:2,uv:0;n:type:ShaderForge.SFN_Add,id:3074,x:32862,y:32925,varname:node_3074,prsc:2|A-7914-OUT,B-1134-OUT;n:type:ShaderForge.SFN_Panner,id:925,x:32119,y:33538,varname:node_925,prsc:2,spu:0,spv:1|UVIN-5161-UVOUT,DIST-3277-OUT;n:type:ShaderForge.SFN_Time,id:5739,x:31662,y:33680,varname:node_5739,prsc:2;n:type:ShaderForge.SFN_Multiply,id:3277,x:31877,y:33697,varname:node_3277,prsc:2|A-5739-TSL,B-9713-OUT;n:type:ShaderForge.SFN_Slider,id:9713,x:31662,y:33846,ptovrint:False,ptlb:node_469_copy,ptin:_node_469_copy,varname:_node_469_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-5,cur:0,max:5;n:type:ShaderForge.SFN_TexCoord,id:5161,x:31694,y:33479,varname:node_5161,prsc:2,uv:0;proporder:4613-7834-7548-7400-6870-2053-469-9713;pass:END;sub:END;*/

Shader "Shader Forge/uv_move_alpha_water_None" {
    Properties {
        _node_4613 ("node_4613", 2D) = "white" {}
        _UVspeed ("UV speed", Range(-6, 6)) = 0
        _Specularintensity ("Specular intensity", Range(0, 5)) = 0.3501931
        _Glossintensity ("Gloss intensity", Range(0, 6)) = 1.411321
        _mask ("mask", 2D) = "white" {}
        _maintexture ("maintexture", 2D) = "white" {}
        _node_469 ("node_469", Range(-5, 5)) = 0
        _node_469_copy ("node_469_copy", Range(-5, 5)) = 0
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#ifndef UNITY_PASS_FORWARDBASE
			#define UNITY_PASS_FORWARDBASE
			#endif
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _node_4613; uniform float4 _node_4613_ST;
            uniform float _UVspeed;
            uniform float _Specularintensity;
            uniform float _Glossintensity;
            uniform sampler2D _mask; uniform float4 _mask_ST;
            uniform sampler2D _maintexture; uniform float4 _maintexture_ST;
            uniform float _node_469;
            uniform float _node_469_copy;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
////// Lighting:
                float4 node_9169 = _Time + _TimeEditor;
                float2 node_9511 = (i.uv0+(node_9169.g*_UVspeed)*float2(0,1));
                float4 _node_4613_var = tex2D(_node_4613,TRANSFORM_TEX(node_9511, _node_4613));
                float4 node_811 = _Time + _TimeEditor;
                float2 node_4397 = (i.uv0+(node_811.r*_node_469)*float2(0,1));
                float4 _mask_var = tex2D(_mask,TRANSFORM_TEX(node_4397, _mask));
                float4 node_5739 = _Time + _TimeEditor;
                float2 node_925 = (i.uv0+(node_5739.r*_node_469_copy)*float2(0,1));
                float4 _maintexture_var = tex2D(_maintexture,TRANSFORM_TEX(node_925, _maintexture));
                float3 finalColor = ((_node_4613_var.rgb+((dot(i.normalDir,viewReflectDirection)*_Specularintensity)*_Glossintensity))+(_mask_var.rgb*_maintexture_var.rgb));
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
