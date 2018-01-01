// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/GlobalFogSkyMaster" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "black" {}
	_ColorRamp ("Colour Palette", 2D) = "gray" {}
	_Close ("Close", float) = 0.0 
	_Far ("Far", float) = 0.0 
	v3LightDir("v3LightDir", Vector) = (0,0,0)
	FogSky("FogSky",float) = 0.0
	_TintColor("Color Tint", Color) = (0,0,0,0)
	ClearSkyFac("Clear Sky Factor",float) = 1.0
}

CGINCLUDE

	#include "UnityCG.cginc"
	#include "Lighting.cginc"

	uniform sampler2D _MainTex;
	uniform sampler2D_float _CameraDepthTexture;
	
	//SM v1.7
	uniform sampler2D _ColorRamp;
	uniform float _Close;
	uniform float _Far;
	uniform float3 v3LightDir;		// light source
	uniform float FogSky;	
	fixed4 _TintColor; //float3(680E-8, 1550E-8, 3450E-8);
	uniform float ClearSkyFac;
	// x = fog height
	// y = FdotC (CameraY-FogHeight)
	// z = k (FdotC > 0.0)
	// w = a/2
	uniform float4 _HeightParams;
	
	// x = start distance
	uniform float4 _DistanceParams;
	
	int4 _SceneFogMode; // x = fog mode, y = use radial flag
	float4 _SceneFogParams;
	#ifndef UNITY_APPLY_FOG
	half4 unity_FogColor;
	half4 unity_FogDensity;
	#endif	

	uniform float4 _MainTex_TexelSize;
	
	// for fast world space reconstruction
	uniform float4x4 _FrustumCornersWS;
	uniform float4 _CameraWS;
	
	//SM v1.7
	uniform float luminance, Multiplier1, Multiplier2,Multiplier3,bias, lumFac, contrast,turbidity;
	//uniform float mieDirectionalG = 0.7,0.913; 
	float mieDirectionalG;
	float mieCoefficient;//0.054
	float reileigh;
	
	uniform float e = 2.71828182845904523536028747135266249775724709369995957;
	uniform float pi = 3.141592653589793238462643383279502884197169;
	uniform float n = 1.0003;
	uniform float N = 2.545E25; 								
	uniform float pn = 0.035;
	uniform float3 lambda = float3(680E-9, 550E-9, 450E-9);
	uniform float3 K = float3(0.686, 0.678, 0.666);//const vec3 K = vec3(0.686, 0.678, 0.666);
	uniform float v = 4.0;		
	uniform float rayleighZenithLength = 8.4E3;
	uniform float mieZenithLength = 1.25E3;	
	uniform float EE = 1000.0;
	uniform float sunAngularDiameterCos = 0.999956676946448443553574619906976478926848692873900859324;
	// 66 arc seconds -> degrees, and the cosine of that
	float cutoffAngle = 3.141592653589793238462643383279502884197169/1.95;
	float steepness = 1.5;

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		float2 uv_depth : TEXCOORD1;
		float4 interpolatedRay : TEXCOORD2;
	};
	
	v2f vert (appdata_img v)
	{
		v2f o;
		half index = v.vertex.z;
		v.vertex.z = 0.1;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord.xy;
		o.uv_depth = v.texcoord.xy;
		
		#if UNITY_UV_STARTS_AT_TOP
		if (_MainTex_TexelSize.y < 0)
			o.uv.y = 1-o.uv.y;
		#endif				
		
		o.interpolatedRay = _FrustumCornersWS[(int)index];
		o.interpolatedRay.w = index;
		
		return o;
	}
	
	// Applies one of standard fog formulas, given fog coordinate (i.e. distance)
	half ComputeFogFactor (float coord)
	{
		float fogFac = 0.0;
		if (_SceneFogMode.x == 1) // linear
		{
			// factor = (end-z)/(end-start) = z * (-1/(end-start)) + (end/(end-start))
			fogFac = coord * _SceneFogParams.z + _SceneFogParams.w;
		}
		if (_SceneFogMode.x == 2) // exp
		{
			// factor = exp(-density*z)
			fogFac = _SceneFogParams.y * coord; fogFac = exp2(-fogFac);
		}
		if (_SceneFogMode.x == 3) // exp2
		{
			// factor = exp(-(density*z)^2)
			fogFac = _SceneFogParams.x * coord; fogFac = exp2(-fogFac*fogFac);
		}
		return saturate(fogFac);
	}

	// Distance-based fog
	float ComputeDistance (float3 camDir, float zdepth)
	{
		float dist; 
		if (_SceneFogMode.y == 1)
			dist = length(camDir);
		else
			dist = zdepth * _ProjectionParams.z;
		// Built-in fog starts at near plane, so match that by
		// subtracting the near value. Not a perfect approximation
		// if near plane is very large, but good enough.
		dist -= _ProjectionParams.y;
		return dist;
	}

	// Linear half-space fog, from https://www.terathon.com/lengyel/Lengyel-UnifiedFog.pdf
	float ComputeHalfSpace (float3 wsDir)
	{
		float3 wpos = _CameraWS + wsDir;
		float FH = _HeightParams.x;
		float3 C = _CameraWS;
		float3 V = wsDir;
		float3 P = wpos;
		float3 aV = _HeightParams.w * V;
		float FdotC = _HeightParams.y;
		float k = _HeightParams.z;
		float FdotP = P.y-FH;
		float FdotV = wsDir.y;
		float c1 = k * (FdotP + FdotC);
		float c2 = (1-2*k) * FdotP;
		float g = min(c2, 0.0);
		g = -length(aV) * (c1 - g * g / abs(FdotV+1.0e-5f));
		return g;
	}
	
//SM v1.7
float3 totalRayleigh(float3 lambda){
	float pi = 3.141592653589793238462643383279502884197169;
	float n = 1.0003; // refraction of air
	float N = 2.545E25; //molecules per air unit volume 								
	float pn = 0.035;		 
	return (8.0 * pow(pi, 3.0) * pow(pow(n, 2.0) - 1.0, 2.0) * (6.0 + 3.0 * pn)) / (3.0 * N * pow(lambda, float3(4.0,4.0,4.0)) * (6.0 - 7.0 * pn));
}

float rayleighPhase(float cosTheta)
{    
	return (3.0 / 4.0) * (1.0 + pow(cosTheta, 2.0));
} 
      
float3 totalMie(float3 lambda, float3 K, float T)
{   
 	float pi = 3.141592653589793238462643383279502884197169;
 	float v = 4.0; 
	float c = (0.2 * T ) * 10E-18;
	return 0.434 * c * pi * pow((2.0 * pi) / lambda, float3(v - 2.0,v - 2.0,v - 2.0)) * K;
} 

float hgPhase(float cosTheta, float g)
{   
	float pi = 3.141592653589793238462643383279502884197169;
	return (1.0 / (4.0*pi)) * ((1.0 - pow(g, 2.0)) / pow(1.0 - 2.0*g*cosTheta + pow(g, 2.0), 1.5));
} 

float sunIntensity(float zenithAngleCos)
{       
	float cutoffAngle = 3.141592653589793238462643383279502884197169/1.95;//pi/
	float steepness = 1.5;
	float EE = 1000.0;
	return EE * max(0.0, 1.0 - exp(-((cutoffAngle - acos(zenithAngleCos))/steepness)));
} 

float logLuminance(float3 c)
{        
	return log(c.r * 0.2126 + c.g * 0.7152 + c.b * 0.0722);
}

float3 tonemap(float3 HDR) 
{
	float Y = logLuminance(HDR);
	float low = exp(((Y*lumFac+(1.0-lumFac))*luminance) - bias - contrast/2.0);
	float high = exp(((Y*lumFac+(1.0-lumFac))*luminance) - bias + contrast/2.0);
	float3 ldr = (HDR.rgb - low) / (high - low);
	return float3(ldr);
}

	half4 ComputeFog (v2f i, bool distance, bool height) : SV_Target
	{
		half4 sceneColor = tex2D(_MainTex, i.uv);
		
		// Reconstruct world space position & direction
		// towards this screen pixel.
		float rawDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture,i.uv_depth);
		float dpth = Linear01Depth(rawDepth);
		float4 wsDir = dpth * i.interpolatedRay;
		float4 wsPos = _CameraWS + wsDir;
		
		//SM v1.7
		float3 lightDirection = v3LightDir;// _WorldSpaceLightPos0.xyz;  
		float  cosTheta = dot(normalize(wsDir), lightDirection);		
				
		float3 up = float3(0.0, 0.0, 1.0);			
		float3 lambda = float3(680E-8, 550E-8, 450E-8); 
		float3 K = float3(0.686, 0.678, 0.666);
		float  rayleighZenithLength = 8.4E3;
		float  mieZenithLength = 1.25E3;
		//float  mieCoefficient = 0.054;
		float  pi = 3.141592653589793238462643383279502884197169;		
		float3 betaR = totalRayleigh(lambda) * reileigh * 1000;		
		float3 lambda1 = float3(_TintColor.r,_TintColor.g,_TintColor.b)*0.0000001;//  680E-8, 1550E-8, 3450E-8);
		lambda = lambda1;
		float3 betaM = totalMie(lambda1, K, turbidity * Multiplier2) * mieCoefficient; 
		float zenithAngle = acos(max(0.0, dot(up, normalize(lightDirection))));        
		float sR = rayleighZenithLength / (cos(zenithAngle) + 0.15 * pow(93.885 - ((zenithAngle * 180.0) / pi), -1.253));        
		float sM = mieZenithLength / (cos(zenithAngle) + 0.15 * pow(93.885 - ((zenithAngle * 180.0) / pi), -1.253));		
		float  rPhase = rayleighPhase(cosTheta*0.5+0.5);
		float3 betaRTheta = betaR * rPhase;
		float  mPhase = hgPhase(cosTheta, mieDirectionalG) * Multiplier1;
		float3 betaMTheta = betaM * mPhase;	
	 	float3 Fex = exp(-(betaR * sR + betaM * sM));
		float  sunE = sunIntensity(dot(lightDirection, up));
		float3 Lin = ((betaRTheta + betaMTheta) / (betaR + betaM)) * (1 - Fex) + sunE*Multiplier3*0.0001;
		float  sunsize = 0.0001;
		float3 L0 = 1.5 * Fex + (sunE * 1.0 * Fex)*sunsize;
		float3 FragColor = tonemap(Lin+L0);
		
		
		
		
		
		
		

		// Compute fog distance
		float g = _DistanceParams.x;
		if (distance)
			g += ComputeDistance (wsDir, dpth);
		if (height)
			g += ComputeHalfSpace (wsDir);

		// Compute fog amount
		half fogFac = ComputeFogFactor (max(0.0,g));//*1.5;
		// Do not fog skybox
		//if (rawDepth >= 0.999999){
		if (rawDepth >= 0.999995  ){
			if(FogSky <= 0){
				fogFac = 1.0;
			}else{
				if (distance){
					fogFac = fogFac*ClearSkyFac;
				}
			}
		}
		//return fogFac; // for debugging
		
		// Lerp between fog color & original scene color
		// by fog amount
		//return lerp (unity_FogColor, sceneColor, fogFac);
		
		
		//SM v1.7
		float4 Final_fog_color = lerp (unity_FogColor+float4(FragColor,1),sceneColor, fogFac) ;			
		float Dist = ComputeDistance (wsDir, dpth);
		if(_Far >0){
			if(Dist > _Close ){
				if(Dist < _Far){ 				
					float greyscale = tex2D(_MainTex, i.uv).r;					
					Final_fog_color = Final_fog_color*tex2D(_ColorRamp, float2(Dist/_Far, 0.5));
				}
			}
		}
		
		return Final_fog_color;
				
	}

ENDCG

SubShader
{
	ZTest Always Cull Off ZWrite Off Fog { Mode Off }

	// 0: distance + height
	Pass
	{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma target 3.0
		half4 frag (v2f i) : SV_Target { return ComputeFog (i, true, true); }
		ENDCG
	}
	// 1: distance
	Pass
	{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma target 3.0
		half4 frag (v2f i) : SV_Target { return ComputeFog (i, true, false); }
		ENDCG
	}
	// 2: height
	Pass
	{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma target 3.0
		half4 frag (v2f i) : SV_Target { return ComputeFog (i, false, true); }
		ENDCG
	}
}

Fallback off

}
