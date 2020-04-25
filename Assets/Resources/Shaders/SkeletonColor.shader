Shader "Custom/SkeletonColor" {
	Properties {
		_Cutoff ("Shadow alpha cutoff", Range(0,1)) = 0.1
		_MainTex ("Texture to blend", 2D) = "black" {}
		_ColorOffSet ("ColorOffSet", Color) = (1,1,1,1)
		_ColorBrightness ("ColorBrightness", Color) = (0,0,0,0)   


		//_Color ("Main Color", Color) = (1,1,1,0)
		//_SpecColor ("Spec Color", Color) = (1,1,1,1)
        //_Emission ("Emmisive Color", Color) = (1,1,1,1)


        //_Shininess ("Shininess", Range (0.01, 1)) = 0.7
	}
	// 2 texture stage GPUs
	SubShader {
		Tags { "Queue"="Transparent" 
		"IgnoreProjector"="True"
		"RenderType"="Transparent"   
        "PreviewType"="Plane"     
        "CanUseSpriteAtlas"="True"     
		}
		LOD 100

		Cull Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		Lighting Off

		Pass {
				//Material
				//{
				//Diffuse [_Color]
                //Ambient [_Color]
               // Shininess [_Shininess]
                //Specular [_SpecColor]
                //Emission [_Emission]
				//}
				Lighting Off

				ColorMaterial AmbientAndDiffuse

				AlphaTest GEqual[_Cutoff]

				SetTexture [_MainTex] {
					ConstantColor [_ColorOffSet]
					Combine Constant * Primary
				}

				SetTexture [_MainTex] {

					ConstantColor [_ColorBrightness]
					Combine Constant + Previous
				}
				SetTexture [_MainTex] {
					Combine Texture * Previous
				}
				SetTexture [_MainTex] {
					ConstantColor(0,0,0,0)
					Combine  Previous Lerp(Texture) Constant
				}
			
					 
		}

		Pass {
			Name "Caster"
			Tags { "LightMode"="ShadowCaster" }
			Offset 1, 1
			
			Fog { Mode Off }
			ZWrite On
			ZTest LEqual
			Cull Off
			Lighting Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"
			struct v2f { 
				V2F_SHADOW_CASTER;
				float2  uv : TEXCOORD1;
			};

			uniform float4 _MainTex_ST;

			v2f vert (appdata_base v) {
				v2f o;
				TRANSFER_SHADOW_CASTER(o)
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}

			uniform sampler2D _MainTex;
			uniform fixed _Cutoff;
			uniform float4 _ColorOffSet;
			uniform float4 _ColorBrightness;

			float4 frag (v2f i) : COLOR {
				fixed4 texcol = tex2D(_MainTex, i.uv);
				texcol *= _ColorOffSet;
				texcol +=_ColorBrightness;
				//clip(texcol.a - _Cutoff);
				//SHADOW_CASTER_FRAGMENT(i)
				return texcol;
			}
			ENDCG
		}
	}
}