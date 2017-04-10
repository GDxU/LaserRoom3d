Shader "Custom/MyEdgeDetect" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "" {}
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		Pass{
			CGPROGRAM
			#pragma target 3.0   
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			uniform float4 _MainTex_TexelSize;
			uniform half _SampleDistance;
			sampler2D _CameraDepthNormalsTexture;
			sampler2D _MainTex;

			struct v2f {
			   float4 pos : SV_POSITION;
			   float4 scrPos:TEXCOORD0;
		       float2 uv[5] : TEXCOORD1;
			};
			//Vertex Shader
			v2f vert (appdata_base v){
			    v2f o;
			    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
			    o.scrPos=ComputeScreenPos(o.pos);
				float2 uv = v.texcoord.xy;
				o.uv[0] = uv;
		
	
				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					uv.y = 1-uv.y;
				#endif
				
				// calc coord for the X pattern
				// maybe nicer TODO for the future: 'rotated triangles'
		
				o.uv[1] = uv + _MainTex_TexelSize.xy * half2(1,1) * _SampleDistance;
				o.uv[2] = uv + _MainTex_TexelSize.xy * half2(-1,-1) * _SampleDistance;
				o.uv[3] = uv + _MainTex_TexelSize.xy * half2(-1,1) * _SampleDistance;
				o.uv[4] = uv + _MainTex_TexelSize.xy * half2(1,-1) * _SampleDistance;
				 
				return o;
			}
			inline bool SameNormal(half3 normalA,half3 normalB)
			{
				half3 normalDelta=abs(normalA-normalB);
				return (normalDelta.x+normalDelta.y)<0.3;
			}
			inline bool StrictSameNormal(half3 normalA,half3 normalB)
			{
				half3 normalDelta=abs(normalA-normalB);
				return (normalDelta.x+normalDelta.y)<0.01;
			}
			//Fragment Shader
			half4 frag (v2f i) : COLOR{
			   half4 sample0 = tex2D(_CameraDepthNormalsTexture, i.scrPos.xy);
			    half sample0Depth;
			    half sample1Depth;
				half sample2Depth;
				half sample3Depth;
				half sample4Depth;
			    half3 sample0Normal;
			    half3 sample1Normal;
			    half3 sample2Normal;
			    half3 sample3Normal;
			    half3 sample4Normal;
			   DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.scrPos.xy), sample0Depth, sample0Normal);
			   DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.uv[1].xy), sample1Depth, sample1Normal);
			   DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.uv[2].xy), sample2Depth, sample2Normal);
			   DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.uv[3].xy), sample3Depth, sample3Normal);
			   DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.uv[4].xy), sample4Depth, sample4Normal);

				half edge = 1.0;
				if(StrictSameNormal(sample1Normal,sample3Normal) || StrictSameNormal(sample1Normal,sample4Normal))
				{
					edge*=SameNormal(sample1Normal,sample2Normal);
					edge*=SameNormal(sample3Normal,sample4Normal);
				}
				//Antichamber easter egg way:
				half cmpdepth=sample0Depth*0.02;
				edge*=cmpdepth>abs(sample1Depth+sample2Depth-2*sample0Depth);
				edge*=cmpdepth>abs(sample3Depth+sample4Depth-2*sample0Depth);
				return edge * tex2D(_MainTex, i.uv[0]);
				//return sample0;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}