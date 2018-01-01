// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/AtmosphericScattering_DeferredSM34" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "black" {}

		//v3.6
		 _MainTex2 ("Base (RGB)", 2D) = "white" {}
		 _TextureAmount ("_TextureAmount", Float) = 0
	}

	CGINCLUDE
	#pragma vertex vert
	#pragma fragment frag

	//#pragma multi_compile _ ATMOSPHERICS ATMOSPHERICS_PER_PIXEL
	//#pragma multi_compile _ ATMOSPHERICS_OCCLUSION
	//#pragma multi_compile _ ATMOSPHERICS_OCCLUSION_EDGE_FIXUP
	// #pragma multi_compile _ ATMOSPHERICS_DEBUG

	#include "UnityCG.cginc"
	#include "AtmosphericScatteringSM34.cginc"

	uniform sampler2D		_MainTex;
	uniform float4			_MainTex_TexelSize;
	
	uniform float4x4		_FrustumCornersWS;
	uniform float4			_CameraWS;


	//v3.6
	uniform sampler2D _MainTex2;
	float4 _MainTex2_ST;

	//v3.4
	uniform float			u_SMFogSkyToggle;//0 = no, 1 = occlude
	uniform float			u_SMFogHorizonLower;

	uniform float _TextureAmount;

	struct v2f {
		float4 pos				: SV_POSITION;
		float2 uv				: TEXCOORD0;
		float2 uv_depth			: TEXCOORD1;
		float4 interpolatedRay	: TEXCOORD2;
		float4 tex : TEXCOORD3; //v3.6
	};
	
	v2f vert(appdata_img v) {
		v2f o;
		half index = v.vertex.z;
		v.vertex.z = 0.1;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord.xy;
		o.uv_depth = v.texcoord.xy;
		
#if UNITY_UV_STARTS_AT_TOP
		if (_MainTex_TexelSize.y < 0.f)
			o.uv.y = 1.f - o.uv.y;
#endif
		
		o.interpolatedRay = _FrustumCornersWS[(int)index];
		o.interpolatedRay.w = index;


		//v3.6
		float3 vertnorm = normalize(v.vertex.xyz);     
        float2 vertuv   = vertnorm.xz / (vertnorm.y*0.4 + 0.2);
        o.tex = float4( vertuv.xy * 0.3, 0, 0 );

		
		return o;
	}

	struct ScatterInput {
		float2 pos;
		half4 scatterCoords1;
		half3 scatterCoords2;
	};
	
	half4 frag(
		v2f i,
#ifdef SHADER_API_D3D11
		UNITY_VPOS_TYPE vpos : SV_Position
#else
		UNITY_VPOS_TYPE vpos : VPOS
#endif
	) : SV_Target {
		half4 sceneColor = tex2D(_MainTex, i.uv);	
		float rawDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv_depth);
	
		// Don't double-scatter on skybox
	//	UNITY_BRANCH
	//	if(rawDepth > 0.9999999f)
	//		return sceneColor;

	//v3.4
	if(rawDepth > 0.9999999f){

		if(u_SMFogSkyToggle == 1){
			//float depth = Linear01Depth(rawDepth) - 2*tex.a*tex1.a*tex2.a*Linear01Depth(0.05*cos(rawDepth*1*111)   );
			float depth = Linear01Depth(rawDepth);

			float4 wsDir = depth * i.interpolatedRay ;
			float4 wsPos = _CameraWS + wsDir + _CameraWS*14*u_SMFogHorizonLower;
			
			// Apply scattering
			ScatterInput si;
			si.pos = vpos.xy ; 

			VOLUND_TRANSFER_SCATTER(wsPos.xyz, si);
			VOLUND_APPLY_SCATTER(si, sceneColor.rgb);

			return sceneColor;
		}else{
			return sceneColor;
		}

	}else{

	   		//v3.6
	 		//float depth = Linear01Depth(rawDepth) - tex.a*tex1.a*tex2.a*Linear01Depth(0.5*cos(rawDepth*_Time.x*111)   );
	 		//float depth = Linear01Depth(rawDepth) - 2*tex.a*tex1.a*tex2.a*Linear01Depth(0.05*cos(rawDepth*1*111)   );
	 		float depth = Linear01Depth(rawDepth) ;
			//float depth = Linear01Depth(rawDepth);

			//v3.6
			float4 _CloudSpeed = float4(0.1,0.1,0,0);
			float _CloudDensity = 1;
			float _CloudSize = 1;
			float _CloudSharpness = 0.994;
			float _CloudCover = 1.8;//2.8  v3.4.2
			float _Forward=0;

			float2 offset = _Time.y * _CloudSpeed.xy;
	        float4 tex = tex2D( _MainTex2, ( i.tex.xy ) + offset );        
	        float2 offset1 = 0.5*_Time.y * _CloudSpeed.xy;
	        float4 tex1 = tex2D( _MainTex2, ( i.tex.xy ) + offset1 );        
	        float2 offset2 = 0.7*_Time.y * _CloudSpeed.xy+_CloudDensity;
	        float4 tex2 = tex2D( _MainTex2, ( i.tex.xy )*_CloudSize + offset2 )+0;

			if(_Forward==0 || _Forward==2 || _Forward==3){
		        tex = max( tex - ( 1 - _CloudCover*2 ), 0 );
		        tex1 = max( tex1 - ( 1 - _CloudCover*2 ), 0 );
		        tex2 = max( tex2 - ( 1 - _CloudCover*2 ), 0 ); 		
		        tex.a = 1.0 - pow( _CloudSharpness, tex.a * 255 );
	        }
	        
	        if(_Forward==4){
		        tex = max( tex - ( 1 - _CloudCover*1.4 ), 0 );
		        tex1 = max( tex1 - ( 1 - _CloudCover*1.4 ), 0 );
		        tex2 = max( tex2 - ( 1 - _CloudCover*1.4 ), 0 ); 		
		        tex.a = 1.0 - pow( _CloudSharpness, tex.a * 255 );
	        }         
	        float Light = 1;
	     	//float4 res = float4 (Light * _Color.r*1, Light * _Color.g*1, Light * _Color.b*1, tex.a*tex1.a*tex2.a);
	   		//END v3.6

		//float depth = Linear01Depth(rawDepth) - 2*tex.a*tex1.a*tex2.a*Linear01Depth(0.05*cos(rawDepth*1*111)   );

			float4 wsDir = depth * i.interpolatedRay ;
			float4 wsPos = _CameraWS + wsDir;
			
			// Apply scattering
			ScatterInput si;
			si.pos = vpos.xy; 

//			if(_TextureAmount>0){
//				sceneColor.rgb *= _TextureAmount*0.05*tex.a*tex1.a*tex2.a;
//			}

			VOLUND_TRANSFER_SCATTER(wsPos.xyz, si);
			VOLUND_APPLY_SCATTER(si, sceneColor.rgb);

			//v3.4.2
			float luminance = 0.33*sceneColor.r + 0.33*sceneColor.g + 0.33*sceneColor.b;
			if(_TextureAmount>0){
				//if(luminance > 0.0){
				sceneColor.rgb = sceneColor.rgb/1.2 +  _TextureAmount*(luminance+float3(1,1,1)*0.1)*0.05*tex.a*tex1.a*tex2.a*sceneColor.rgb;					
				//}				
				//sceneColor.rgb *= _TextureAmount*(luminance+float3(1,1,1)*0.1)*0.05*tex.a*tex1.a*tex2.a;				
//				if(luminance > 0.6){
//							sceneColor.rgb *= _TextureAmount*0.05*tex.a*tex1.a*tex2.a;					
//				}
			}

			//v3.4
			//#define VOLUND_TRANSFER_SCATTER(pos, o) VolundTransferScatterOcclusion(pos, o.scatterCoords1, o.scatterCoords2)
			//#define VOLUND_APPLY_SCATTER(i, color) color = VolundApplyScatterOcclusion(i.scatterCoords1, i.scatterCoords2, i.pos.xy, color)

			//VolundTransferScatterOcclusion(wsPos.xyz, si.scatterCoords1, si.scatterCoords2);
			//VolundApplyScatterOcclusion(si.scatterCoords1, si.scatterCoords2, si.pos.xy, sceneColor.rgb);

			return sceneColor; //v3.6
		}
	}
	ENDCG

	SubShader {
		ZTest Always Cull Off ZWrite Off		
		Pass {
			CGPROGRAM
				#pragma target 5.0
				#pragma only_renderers d3d11
			ENDCG
		}
	}

	SubShader {
		ZTest Always Cull Off ZWrite Off		
		Pass {
			CGPROGRAM
				#pragma target 3.0
				#pragma only_renderers d3d9 opengl
			ENDCG
		}
	}

	Fallback off
}
