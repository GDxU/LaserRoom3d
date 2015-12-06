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
				return (normalDelta.x+normalDelta.y)<0.7;
			}
			inline bool StrictSameNormal(half3 normalA,half3 normalB)
			{
				half3 normalDelta=abs(normalA-normalB);
				return (normalDelta.x+normalDelta.y)<10000;
			}
			inline half CheckSame (half2 centerNormal, float centerDepth, half4 theSample)
			{
				// difference in normals
				// do not bother decoding normals - there's no need here
				half2 diff = abs(centerNormal - theSample.xy) * 1;
				half isSameNormal = (diff.x + diff.y) * 1 < 0.1;
				// difference in depth
				float sampleDepth = DecodeFloatRG (theSample.zw);
				float zdiff = abs(centerDepth-sampleDepth);
				// scale the required threshold by the distance
				half isSameDepth = zdiff * 1 < 0.3 * centerDepth;
	
				// return:
				// 1 - if normals and depth are similar enough
				// 0 - otherwise
		
				return isSameNormal * isSameDepth;
			}
			inline bool CheckDist(float dist,float3 sampleNormal)
			{
				return (abs(dist*sampleNormal.z)<0.001);
			}
			//Fragment Shader
			half4 frag (v2f i) : COLOR{
			   half4 sample0 = tex2D(_CameraDepthNormalsTexture, i.scrPos.xy);
			    float sample0Depth;
			    float sample1Depth;
				float sample2Depth;
				float sample3Depth;
				float sample4Depth;
			    float3 sample0Normal;
			    float3 sample1Normal;
			    float3 sample2Normal;
			    float3 sample3Normal;
			    float3 sample4Normal;
			   DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.scrPos.xy), sample0Depth, sample0Normal);
			   DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.uv[1].xy), sample1Depth, sample1Normal);
			   DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.uv[2].xy), sample2Depth, sample2Normal);
			   DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.uv[3].xy), sample3Depth, sample3Normal);
			   DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.uv[4].xy), sample4Depth, sample4Normal);
			   //half edge=1.0;
			   //half threshold=0.2;
			   //half thresholdrev=0.1;
			   //edge=SameNormal(sample1Normal,sample2Normal)||SameNormal(sample3Normal,sample4Normal);*/
			   //edge*=abs(sample1Normal - sample2Normal)>threshold || abs(sample3Normal - sample4Normal)>threshold?0:1;
				//depthValue = Linear01Depth (depthValue);
				//half avgDepth=(sample1Depth+sample2Depth+sample3Depth+sample4Depth)/4;
			   //half4 o;
			   //o.rbg=abs((avgDepth-sample0Depth);
			   //o.a=1;
			   //return o;

				half edge = 1.0;
				edge*=SameNormal(sample1Normal,sample2Normal);
				edge*=SameNormal(sample3Normal,sample4Normal);
				//edge *= CheckSame(sample1.xy, DecodeFloatRG(sample1.zw), sample2);
				//edge *= CheckSame(sample3.xy, DecodeFloatRG(sample3.zw), sample4);
				//half avgDepth=(sample1Depth+sample2Depth+sample3Depth+sample4Depth)/4;
				//edge *= abs((avgDepth-sample0Depth))<0.0005;
				//Antichamber easter egg way:
				half cmpdepth=sample0Depth*0.015;
				edge*=cmpdepth>abs(sample1Depth+sample2Depth-2*sample0Depth);
				edge*=cmpdepth>abs(sample3Depth+sample4Depth-2*sample0Depth);
				return edge * tex2D(_MainTex, i.uv[0]);
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}