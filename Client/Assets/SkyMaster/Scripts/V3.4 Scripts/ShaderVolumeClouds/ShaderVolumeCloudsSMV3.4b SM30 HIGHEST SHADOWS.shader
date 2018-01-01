// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "SkyMaster/ShaderVolumeClouds-Desktop-SM3.0 HIGHEST SHADOWS" {
    Properties {
        _SunColor ("_SunColor", Color) = (0.95,0.95,0.95,0.8)
        _ShadowColor ("_ShadowColor", Color) = (0.05,0.05,0.1,0.3)
        _ColorDiff ("_ColorDiff", Float ) = 0.5
        _CloudMap ("_CloudMap", 2D) = "white" {}
        _CloudMap1 ("_CloudMap1", 2D) = "white" {}
        _Density ("_Density", Float ) = -0.4
        _Coverage ("_Coverage", Float ) = 4250
        _Transparency ("_Transparency", Float ) = 1
        _Velocity1 ("_Velocity1", Vector ) = (1,23,0,0)
        _Velocity2 ("_Velocity2", Vector ) = (1,22,0,0)   
        _LightingControl ("_LightingControl", Vector) = (1,1,-1,0)       
        _HorizonFactor ("_HorizonFactor", Range(0, 10)) = 2    
        _EdgeFactors ("_EdgeFactor2", Vector) = (0,0.52,-1,0) 
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
        //_Mode ("_Mode", Range(0,5)) = 0
        _CutHeight ("_CutHeight", Float) = 240

        //SCATTER
        _Control ("Control Color", COLOR) = (1,1,1)
        _Color ("Color", COLOR) = (1,1,1) 
        _FogColor ("Fog Color", COLOR) = (1,1,1) 
        _FogFactor ("Fog factor", float) = 1
        _FogUnity ("Fog on/off(1,0)", float) = 0
       // _PaintMap ("_CloudMap", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            //"Queue"="Transparent"
           // "RenderType"="Transparent"

          // "Queue"="Geometry"
//          "Queue"="AlphaTest"
//          // "RenderType"="Cutout"
//            "RenderType"="Transparent"


          "Queue"="AlphaTest"
          //"RenderType"="Transparent"
          "RenderType"="TransparentCutout"

 // "Queue"="Geometry"
  //          "RenderType"="Opaque"
        }      
        Blend One One 
        ZWrite on
        Cull off
        Offset 1,1
        Ztest LEqual

     //   AlphaTest Greater 1.5






















         Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
                       
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_fog 
            #pragma multi_compile_shadowcaster  
            #pragma multi_compile_fwdadd_fullshadows    
        //    #pragma addshadow               
            #pragma target 3.0

            uniform sampler2D _CloudMap; 
            uniform float4 _CloudMap_ST;
            uniform sampler2D _CloudMap1; 
            uniform float4 _CloudMap1_ST;
            //uniform float4 _LightColor0;          
            uniform float _Density;
            uniform float _Coverage;
            uniform float _Transparency;         
            uniform float2 _EdgeFactors;
            uniform float2 _Velocity1;
            uniform float2 _Velocity2;
            uniform float _Cutoff;
            uniform float _CutHeight;

            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;    
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;               
                float2 uv0 : TEXCOORD1;
                float4 worldPos : TEXCOORD2;               
            };
            VertexOutput vert (VertexInput v) {           
             	VertexOutput o;    
                o.uv0 = v.texcoord0;    
                o.pos = UnityObjectToClipPos(v.vertex );                         
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                            
                 float2 UVs = _Density*float2(i.worldPos.x,i.worldPos.z);
                float4 TimingF = 0.0012;
                float2 UVs1 = _Velocity1*TimingF*_Time.y + UVs;
                float4 cloudTexture = tex2D(_CloudMap,UVs1+_CloudMap_ST);
                float4 cloudTexture1 = tex2D(_CloudMap1,UVs1+_CloudMap1_ST);
                float2 UVs2 = (_Velocity2*TimingF*_Time.y + float2(_EdgeFactors.x,_EdgeFactors.y) + UVs);
                float4 Texture1 = tex2D(_CloudMap,UVs2+_CloudMap_ST); 
                float4 Texture2 = tex2D(_CloudMap1,UVs2+_CloudMap1_ST); 

                float DER = i.worldPos.y*0.001;               
                float3 normalA = (((DER*(_Coverage+((cloudTexture.rgb*2)-1)))-(1-(Texture1.rgb*2))));             	
             	float3 normalN = normalize(normalA); 

				float change_h =_CutHeight;
				float PosDiff = 0.0006*(i.worldPos.y-change_h);
             	float DER1 = -(i.worldPos.y+0)*PosDiff;
             	float PosTDiff = i.worldPos.y*PosDiff;
             	if(i.worldPos.y > change_h){             		
             		DER1 = (1-cloudTexture1.a) *  PosTDiff;
             		//DER1 =  PosTDiff;
             	}

             	float shaper = _Transparency*((DER1*saturate((_Coverage+cloudTexture1.a)))-Texture2.a);
              
                clip(shaper - _Cutoff+0.4);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }



       
    }
   // FallBack "Diffuse"
   FallBack "Transparent/Cutout/Diffuse"
}