// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.30 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.30;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|emission-3463-OUT,alpha-7799-A;n:type:ShaderForge.SFN_Color,id:7241,x:32186,y:32635,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.07843138,c2:0.3921569,c3:0.7843137,c4:1;n:type:ShaderForge.SFN_Tex2d,id:7799,x:32186,y:32837,ptovrint:False,ptlb:node_7799,ptin:_node_7799,varname:node_7799,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:9cbe9a1eb2e3e6346a9006768e0355c8,ntxv:0,isnm:False|UVIN-2584-OUT;n:type:ShaderForge.SFN_Add,id:3463,x:32442,y:32781,varname:node_3463,prsc:2|A-7241-RGB,B-7799-RGB;n:type:ShaderForge.SFN_TexCoord,id:7906,x:31280,y:32713,varname:node_7906,prsc:2,uv:0;n:type:ShaderForge.SFN_Panner,id:3659,x:31484,y:32713,varname:node_3659,prsc:2,spu:0,spv:1|UVIN-7906-UVOUT;n:type:ShaderForge.SFN_Slider,id:9357,x:31265,y:33006,ptovrint:False,ptlb:node_9357,ptin:_node_9357,varname:node_9357,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5328007,max:1;n:type:ShaderForge.SFN_Multiply,id:2584,x:31955,y:32895,varname:node_2584,prsc:2|A-3659-UVOUT,B-9357-OUT;n:type:ShaderForge.SFN_Sin,id:3909,x:31593,y:33049,varname:node_3909,prsc:2|IN-7942-TSL;n:type:ShaderForge.SFN_Time,id:7942,x:31373,y:33082,varname:node_7942,prsc:2;n:type:ShaderForge.SFN_Multiply,id:5981,x:31777,y:32967,varname:node_5981,prsc:2|A-9357-OUT,B-3909-OUT;proporder:7241-7799-9357;pass:END;sub:END;*/

Shader "Shader Forge/WaveRim" {
    Properties {
        _Color ("Color", Color) = (0.07843138,0.3921569,0.7843137,1)
        _node_7799 ("node_7799", 2D) = "white" {}
        _node_9357 ("node_9357", Range(0, 1)) = 0.5328007
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
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
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float4 _Color;
            uniform sampler2D _node_7799; uniform float4 _node_7799_ST;
            uniform float _node_9357;
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
////// Emissive:
                float4 node_5453 = _Time + _TimeEditor;
                float2 node_3659 = (i.uv0+node_5453.g*float2(0,1));
                float2 node_2584 = (node_3659*_node_9357);
                float4 _node_7799_var = tex2D(_node_7799,TRANSFORM_TEX(node_2584, _node_7799));
                float3 emissive = (_Color.rgb+_node_7799_var.rgb);
                float3 finalColor = emissive;
                return fixed4(finalColor,_node_7799_var.a);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
