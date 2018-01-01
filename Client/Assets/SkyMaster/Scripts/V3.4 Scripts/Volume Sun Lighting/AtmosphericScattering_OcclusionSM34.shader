// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
// Upgrade NOTE: replaced 'unity_World2Shadow' with 'unity_WorldToShadow'

// Upgrade NOTE: replaced 'unity_World2Shadow' with 'unity_WorldToShadow'

Shader "Hidden/AtmosphericScattering_OcclusionSM34" {

//Properties {		
	//_ToggleForward ("_ToggleForward", Float) = 0 //0=defered
//}

CGINCLUDE
	#pragma target 3.0
	#pragma only_renderers d3d11 d3d9 opengl
	
//	#pragma multi_compile _ ATMOSPHERICS_OCCLUSION
//	#pragma multi_compile _ ATMOSPHERICS_OCCLUSION_FULLSKY
//	
//	#if !defined(SHADER_API_D3D11)
//		#undef ATMOSPHERICS_OCCLUSION_FULLSKY
//	#endif
	
	/* this forces the HW PCF path required for correctly sampling the cascaded shadow map
	   render texture (a fix is scheduled for 5.2) */
	#pragma multi_compile SHADOWS_NATIVE

	#include "UnityCG.cginc"
	#include "AtmosphericScatteringSM34.cginc"

	UNITY_DECLARE_SHADOWMAP	(u_SMCascadedShadowMap);	
	uniform float3 			u_SMCameraPosition;
	uniform float3 			u_SMViewportCorner;
	uniform float3 			u_SMViewportRight;
	uniform float3 			u_SMViewportUp;
	uniform sampler2D		u_SMCollectedOcclusionData;
	uniform float4			u_SMCollectedOcclusionData_TexelSize;
	uniform float4			u_SMCollectedOcclusionDataScaledTexelSize;
	uniform float			u_SMOcclusionSkyRefDistance;

	//v3.4
	uniform float			u_SMOcclusionSkyToggle;//0 = no, 1 = occlude
	uniform float			_ToggleForward;//0 = defered, 1 = forward
	uniform float 			backLightDepth;
	uniform float 			backLightIntensity;
	
	struct v2f {
		float4 pos	: SV_POSITION;
		float2 uv	: TEXCOORD0;
		float3 ray	: TEXCOORD2;
	};
	
	v2f vert(appdata_img v) {
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);

		//v3.4.1
//		o.uv = v.texcoord.xy;
//		if(_ToggleForward==1){
//			o.uv.y = 1.05 - v.texcoord.y;
//		}
		//v3.4.1
		o.uv = v.texcoord.xy;
		if(_ToggleForward==1){
			#if defined(UNITY_REVERSED_Z) //v3.4.9c
				o.uv.y = 1.05 - v.texcoord.y;  //v3.4.9c
			#endif
		}

		o.ray = u_SMViewportCorner + o.uv.x * u_SMViewportRight + o.uv.y * u_SMViewportUp;
		return o;
	}
	
	inline fixed4 getCascadeWeights_splitSpheres(float3 wpos) {
		float3 fromCenter0 = wpos.xyz - unity_ShadowSplitSpheres[0].xyz;
		float3 fromCenter1 = wpos.xyz - unity_ShadowSplitSpheres[1].xyz;
		float3 fromCenter2 = wpos.xyz - unity_ShadowSplitSpheres[2].xyz;
		float3 fromCenter3 = wpos.xyz - unity_ShadowSplitSpheres[3].xyz;
		float4 distances2 = float4(dot(fromCenter0,fromCenter0), dot(fromCenter1,fromCenter1), dot(fromCenter2,fromCenter2), dot(fromCenter3,fromCenter3));
#if !defined(SHADER_API_D3D11)
		fixed4 weights = float4(distances2 < unity_ShadowSplitSqRadii);
		weights.yzw = saturate(weights.yzw - weights.xyz);
#else
		fixed4 weights = float4(distances2 >= unity_ShadowSplitSqRadii);
#endif
		return weights;
	}

	inline float4 getShadowCoord(float4 wpos, fixed4 cascadeWeights) {
#if defined(SHADER_API_D3D11)
		return mul(unity_WorldToShadow[(int)dot(cascadeWeights, float4(1,1,1,1))], wpos);
#else
		float3 sc0 = mul(unity_WorldToShadow[0], wpos).xyz;
		float3 sc1 = mul(unity_WorldToShadow[1], wpos).xyz;
		float3 sc2 = mul(unity_WorldToShadow[2], wpos).xyz;
		float3 sc3 = mul(unity_WorldToShadow[3], wpos).xyz;
		return float4(sc0 * cascadeWeights[0] + sc1 * cascadeWeights[1] + sc2 * cascadeWeights[2] + sc3 * cascadeWeights[3], 1);
#endif
	}

	float frag_collect(const v2f i, const int it) {
		const float itF = 1.f / (float)it;
		const float itFM1 = 1.f / (float)(it - 1);
		
		float rawDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
		
//#if !defined(ATMOSPHERICS_OCCLUSION_FULLSKY)
//		UNITY_BRANCH
//		if(rawDepth > 0.9999999f)
//			return 0.75f;
//#endif
			
		float occlusion = 0.f;
//#if defined(ATMOSPHERICS_OCCLUSION_FULLSKY)
//		UNITY_BRANCH

		//v3.4.9c
		bool depthCheck;
		#if defined(UNITY_REVERSED_Z)
				depthCheck = rawDepth  < 0.0000001f; //1-TerrainDepth; //v2.1.19
		#else
				depthCheck = rawDepth > 0.9999999f; //1-TerrainDepth; //v2.1.19
		#endif		

		//if(rawDepth > 0.9999999f) {
		if(depthCheck) { //v3.4.9c
		//if(rawDepth > 0.9999999f) {
		//if(rawDepth  < 0.0000001f) { //v3.4.9 //v3.4.9c
			if(u_SMOcclusionSkyToggle == 1){//v3.4
				float3 worldDir = i.ray * u_SMOcclusionSkyRefDistance;			
				float4 worldPos = float4(0.f, 0.f, 0.f, 1.f);
				
				float fracStep = 0.f;
				for(int i = 0; i < it; ++i, fracStep += itF) {
					worldPos.xyz = u_SMCameraPosition + worldDir * fracStep * fracStep;
					
					float4 cascadeWeights = getCascadeWeights_splitSpheres(worldPos.xyz);
					bool inside = dot(cascadeWeights, float4(1,1,1,1)) < 4;
					float3 samplePos = getShadowCoord(worldPos, cascadeWeights);
					occlusion += inside ? UNITY_SAMPLE_SHADOW(u_SMCascadedShadowMap, samplePos) : 1.f;
				}
			}else{
				return 0.75f;
			}
		} else
//#endif
		{
			//v3.4.9c
			bool depthCheck;
			#if defined(UNITY_REVERSED_Z)
					float depthDiff = 0.0002f - 0.0000001*backLightDepth - rawDepth;
					depthCheck = depthDiff < 0; //1-TerrainDepth; //v2.1.19
			#else
					float depthDiff = 0.9998f + 0.0001*backLightDepth - rawDepth;
					depthCheck = 0 < depthDiff; //1-TerrainDepth; //v2.1.19
			#endif	

			if(depthCheck) { //v3.4.9c
			//float depthDiff = 0.0002f - 0.0000001*backLightDepth - rawDepth; //v3.4.9c
			//if(depthDiff < 0) { //v3.4.9 //v3.4.9c
			//if(rawDepth > 0.0002f - 0.0000001*backLightDepth) { //v3.4.9 //if(rawDepth < 0.9998f + 0.0001*backLightDepth) {//v3.4.3 - 0.9998 is just below the distance to ATOLL distant mountains
				float depth = Linear01Depth(rawDepth);
				float3 worldDir = i.ray * depth;
				
				float4 worldPos = float4(u_SMCameraPosition + worldDir, 1.f);
				float3 deltaStep = -worldDir * itFM1;
				
				for(int i = 0; i < it; ++i, worldPos.xyz += deltaStep) {
					float4 cascadeWeights = getCascadeWeights_splitSpheres(worldPos.xyz);
					bool inside = dot(cascadeWeights, float4(1,1,1,1)) < 4;
					float3 samplePos = getShadowCoord(worldPos, cascadeWeights);
					occlusion += inside ? UNITY_SAMPLE_SHADOW(u_SMCascadedShadowMap, samplePos) : 1.f;
				}
			}else{
				float depth = Linear01Depth(rawDepth);
				float3 worldDir = i.ray * depth;
				
				float4 worldPos = float4(u_SMCameraPosition + worldDir, 1.f);
				float3 deltaStep = -worldDir * itFM1;
				
				for(int i = 0; i < it; ++i, worldPos.xyz += deltaStep) {
					float4 cascadeWeights = getCascadeWeights_splitSpheres(worldPos.xyz);
					bool inside = dot(cascadeWeights, float4(1,1,1,1)) < 4;
					float3 samplePos = getShadowCoord(worldPos, cascadeWeights);
					occlusion += inside ? UNITY_SAMPLE_SHADOW(u_SMCascadedShadowMap, samplePos) : 1.f;
				}

				//occlusion = occlusion*backLightIntensity;
				occlusion = occlusion - occlusion*(pow(depthDiff,backLightIntensity))*10000;
			}
		}

		return occlusion * itF;
	}
	
	fixed4 frag_collect64(v2f i) : SV_Target { return frag_collect(i, 64); }
	fixed4 frag_collect164(v2f i) : SV_Target { return frag_collect(i, 164); }
	fixed4 frag_collect244(v2f i) : SV_Target { return frag_collect(i, 244); }

ENDCG

SubShader {
	ZTest Always Cull Off ZWrite Off
	
	Pass {
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag_collect64
		ENDCG
	}
	
	Pass {
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag_collect164
		ENDCG
	}
	
	Pass {
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag_collect244
		ENDCG
	}
}
Fallback off
}

