// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "SkyMaster/ShaderVolumeClouds-Desktop-SM3.0 HIGH" {
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

        _CoverageOffset ("_Coverage Offset", Float ) = 0
        _ColorDiffOffset ("_ColorDiff Offset", Float ) = -1

        //SCATTER
        _Control ("Control Color", COLOR) = (1,1,1)
        _Color ("Color", COLOR) = (1,1,1) 
        _FogColor ("Fog Color", COLOR) = (1,1,1) 
        _FogFactor ("Fog factor", float) = 1
        _FogUnity ("Fog on/off(1,0)", float) = 0
        _PaintMap ("_CloudMap", 2D) = "white" {}

        _TransparencyOffset ("_Transparency Offset", Float ) = 0
        _Brightness ("Brightness Factors", Vector ) = (1,1,1,1)

    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }       
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #include "UnityCG.cginc"

            //SCATTER
            #include "AutoLight.cginc"
            #include "Lighting.cginc"

            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #pragma multi_compile_fog 
            #pragma multi_compile_fwdbase                      
            #pragma target 3.0

			uniform sampler2D _CloudMap; 
            uniform float4 _CloudMap_ST;
            uniform sampler2D _CloudMap1; 
            uniform float4 _CloudMap1_ST;
            //float4 _LightColor0;
            uniform float4 _SunColor;
            uniform float4 _ShadowColor;
            uniform float _ColorDiff;
            uniform float _Density;
            uniform float _Coverage;
            uniform float _Transparency;         
            uniform float _HorizonFactor;
            uniform float4 _LightingControl;
            uniform float2 _EdgeFactors;
            uniform float2 _Velocity1;
            uniform float2 _Velocity2;
            // uniform int _Mode;
            uniform float _CutHeight;
            uniform float4 _FogColor ;
            uniform float _FogFactor;
            uniform float _FogUnity;

            //uniform sampler2D _PaintMap;
            uniform float _CoverageOffset;
            uniform float _ColorDiffOffset;

            uniform sampler2D _PaintMap;
            uniform float4 _PaintMap_ST;

            uniform float4 _Brightness;
            uniform float _TransparencyOffset;

            //SCATTER			
			float3 _Color;
			float3 _Control;
			static const float pi = 3.141592653589793238462643383279502884197169;
				static const float rayleighZenithLength = 8.4E3;
				static const float mieZenithLength = 1.25E3;
				static const float3 up = float3(0.0, 1.0, 0.0);				
			
				static const float sunAngularDiameterCos = 0.999956676946448443553574619906976478926848692873900859324;	
			

				float3 sunPosition;
				float3 betaR;
				float3 betaM;

				float fog_depth = 2.0;
				float mieCoefficient =0.05;
				float mieDirectionalG = 0.8;				
				float ExposureBias = 1.0;
				float rayleighPhase(float cosTheta)
				{
					return (3.0 / (16.0*pi)) * (1.0 + pow(cosTheta, 2.0));
				}		
								
				float hgPhase(float cosTheta, float g)
				{   float POW = pow(g, 2.0);
					return (0.25 / pi) * ((1.0 - POW) / pow(1.0 - 2.0*g*cosTheta + POW, 1.5)); 
				}



            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;    
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 worldPos : TEXCOORD1;    
                   float3 ForwLight: TEXCOORD2;  
                   float3 camPos:TEXCOORD3;  
                     float3 normal : TEXCOORD4;                      
                LIGHTING_COORDS(5,6)                    
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {           
             	VertexOutput o;    
                o.uv0 = v.texcoord0;    
                o.pos = UnityObjectToClipPos(v.vertex );                         
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);

                //SCATTER
                //  TANGENT_SPACE_ROTATION;
                // o.ForwLight = mul(rotation,ObjSpaceLightDir(v.vertex)); //ObjSpaceLightDir(v.vertex);
                o.ForwLight =ObjSpaceLightDir(v.vertex); //ObjSpaceLightDir(v.vertex);
                o.camPos = normalize(WorldSpaceViewDir(v.vertex));
                o.normal = v.normal;
                TRANSFER_VERTEX_TO_FRAGMENT(o);		


                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {


            	float change_h = _CutHeight;//240;
				float PosDiff = 0.0006*(i.worldPos.y-change_h);

                float2 UVs = _Density*float2(i.worldPos.x,i.worldPos.z);
                float4 TimingF = 0.0012;

                float2 UVs1 = _Velocity1*TimingF*_Time.y + UVs;

                float4 cloudTexture = tex2D(_CloudMap,UVs1+_CloudMap_ST);
                float4 cloudTexture1 = tex2D(_CloudMap1,UVs1+_CloudMap1_ST);

                //_PaintMap
               // float4 paintTexture1 = tex2D(_PaintMap,UVs1+_CloudMap1_ST);
               float4 paintTexture1 = tex2D(_PaintMap,UVs1*_PaintMap_ST.zw*_PaintMap_ST.xy);

                float2 UVs2 = (_Velocity2*TimingF*_Time.y + float2(_EdgeFactors.x,_EdgeFactors.y) + UVs);

                float4 Texture1 = tex2D(_CloudMap,UVs2+_CloudMap_ST); 
                float4 Texture2 = tex2D(_CloudMap1,UVs2+_CloudMap1_ST); 

                float DER = i.worldPos.y*0.001;               
                float3 normalA = (((DER*((_Coverage + _CoverageOffset)+((cloudTexture.rgb*2)-1)))-(1-(Texture1.rgb*2))));             	
             	float3 normalN = normalize(normalA); 


             	//SCATTER          


//            	float4 return1 = tex2D (_MainTex,IN.uv.xy )* tex2D(_BorderTex, IN.uv.xy+float2(_Time.x*_Speed.x,_Time.x*_Speed.y)) * IN.color * _Intensity;			
//				float4 Combined1 = (dot(IN.ViewDir,IN.PointLights));		
//				//Volume
//				fixed4 col = tex2D(_MainTex, IN.uv) * IN.color.a;              
//               	fixed lerp_vec = max (0.3, min(dot (IN.normal, IN.ForwLight),0.5)	);              
               	fixed atten = LIGHT_ATTENUATION(i);               
               //	float3 finalCol = (_Glow)* IN.color.rgb + col * float3(_SunColor.r,_SunColor.g,_SunColor.b) * atten * lerp_vec * 1* _LightIntensity * float4(Combined1.xyz,1)* _SunLightIntensity + _MinLight;			
				//float4 FINAL = _Control.x*return1 + _Control.y*float4(1.0-exp2(-FinalColor*ExposureBias),0) + _Control.z*float4(min(finalCol.rgb,1),col.a);


                

				
             	float DER1 = -(i.worldPos.y+0)*PosDiff;
             	float PosTDiff = i.worldPos.y*PosDiff;
             	if(i.worldPos.y > change_h){             		
             		DER1 = (1-cloudTexture1.a) *  PosTDiff;
             		//DER1 =  PosTDiff;
             	}

     //        	float shaper = _Transparency*((DER1*saturate((_Coverage-(0.8*PosDiff)+cloudTexture1.a)))-Texture2.a); /////////////////// DIFFERENCE

             	float shaper = (_Transparency-0.46+_TransparencyOffset)*((DER1*saturate(((_Coverage + _CoverageOffset)-(0.8*PosDiff)+cloudTexture1.a*Texture2.a)))-0); /////////////////// DIFFERENCE



                float3 lightDirect = normalize(_WorldSpaceLightPos0.xyz);
               	lightDirect.y = -lightDirect.y;
               
                float ColDiff =  (_ColorDiff+_ColorDiffOffset)+((1+(DER*_LightingControl.r*_LightingControl.g))*0.5); 

                float verticalFactor = dot(lightDirect, float3(0,1,0));
             	float Lerpfactor = (ColDiff+(_ShadowColor.a*(dot(lightDirect,normalN)-1)*ColDiff));

                float ColB = _SunColor.rgb;
               // if(_Mode==0){
	                change_h =10;   
	                PosDiff =  0.0006*(i.worldPos.y-change_h);  
	                PosTDiff = i.worldPos.y*PosDiff;          
	             	DER1 = -(i.worldPos.y+0)*PosDiff;

	             	if(i.worldPos.y > change_h){             		
	             		//DER1 = (1-cloudTexture1.a) *  PosTDiff;
	             		DER1 = (1-cloudTexture1.a) *  PosTDiff;
	             	}
	             	ColB =(_SunColor.a-0.1*_Brightness.w)*(1-cloudTexture1.a)*_SunColor.rgb*DER1*(1-verticalFactor);///////
             	//}

             	//SCATTER
             	//float diff = saturate(dot(normalN, normalize(i.ForwLight)));//worldPos
             	float diff = saturate(dot((-i.camPos), normalize(i.ForwLight)))+0.7;//worldPos

             	//float diff2 = (dot(float3(0,1,0), (i.normal)));//worldPos
             	float diff2 = distance(_WorldSpaceCameraPos,i.worldPos)*distance(_WorldSpaceCameraPos,i.worldPos);//worldPos

               // float3 endColor =   lerp(_ShadowColor.rgb*(diff*_LightColor0.rgb*atten),(FinalColor*_LightColor0.rgb*atten*diff)*ColB, Lerpfactor  );  //+ 11*float4(1.0-exp2(-FinalColor*0.04),0) ;

   //            float3 finalCol = diff*_LightColor0.rgb* atten;	
    	       float3 finalCol = diff*_SunColor.rgb* atten;	

    	       float3 endColor = _Control.x*lerp(_ShadowColor.rgb*_Brightness.x,(0.7)*ColB*_Brightness.y, (Lerpfactor))*diff*atten + _Control.z*float4(min(finalCol.rgb,1),Texture1.a);
             //  float3 endColor = _Control.x*lerp(_ShadowColor.rgb*2,(0.2)*ColB, (Lerpfactor))*diff*atten + _Control.z*float4(min(finalCol.rgb,1),Texture1.a);

              //    float3 endColor =   lerp(_ShadowColor.rgb*(diff*1*atten),(FinalColor*1*atten*diff)*ColB, Lerpfactor  ) + _LightColor0.rgb; 

              //  float4 Fcolor = float4((endColor),saturate(shaper - 0.01*(_HorizonFactor*0.00001*diff2)  ))*atten;///////////////////
               float4 Fcolor = float4(saturate(endColor + (_FogFactor-9)*diff2*_FogColor*0.00000001 + 0),saturate(shaper - 0.01*(_HorizonFactor*0.00001*diff2)  )) ;

               if(_FogUnity==1){
                UNITY_APPLY_FOG(i.fogCoord, Fcolor);
               }
                //return Fcolor*1.1;
               // return Fcolor*1.0;
                return (float4(Fcolor.r,Fcolor.g,Fcolor.b, Fcolor.a*paintTexture1.a))*_Brightness.z;
            }
            ENDCG
        }





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

            uniform float _CoverageOffset;
            uniform float _ColorDiffOffset;
            uniform float _TransparencyOffset;

            uniform sampler2D _PaintMap;
            uniform float4 _PaintMap_ST;

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
                            
                float change_h = _CutHeight;//240;
				float PosDiff = 0.0006*(i.worldPos.y-change_h);

                float2 UVs = _Density*float2(i.worldPos.x,i.worldPos.z);
                float4 TimingF = 0.0012;

                float2 UVs1 = _Velocity1*TimingF*_Time.y + UVs;

                float4 cloudTexture = tex2D(_CloudMap,UVs1+_CloudMap_ST);
                float4 cloudTexture1 = tex2D(_CloudMap1,UVs1+_CloudMap1_ST);

                //_PaintMap
               // float4 paintTexture1 = tex2D(_PaintMap,UVs1+_CloudMap1_ST);
               float4 paintTexture1 = tex2D(_PaintMap,UVs1*_PaintMap_ST.zw*_PaintMap_ST.xy);

                float2 UVs2 = (_Velocity2*TimingF*_Time.y + float2(_EdgeFactors.x,_EdgeFactors.y) + UVs);

                float4 Texture1 = tex2D(_CloudMap,UVs2+_CloudMap_ST); 
                float4 Texture2 = tex2D(_CloudMap1,UVs2+_CloudMap1_ST); 

                float DER = i.worldPos.y*0.001;               
                float3 normalA = (((DER*((_Coverage + _CoverageOffset)+((cloudTexture.rgb*2)-1)))-(1-(Texture1.rgb*2))));             	
             	float3 normalN = normalize(normalA); 

             	float DER1 = -(i.worldPos.y+0)*PosDiff;
             	float PosTDiff = i.worldPos.y*PosDiff;
             	if(i.worldPos.y > change_h){             		
             		DER1 = (1-cloudTexture1.a) *  PosTDiff;
             	}

             	float shaper = (_Transparency-0.46+_TransparencyOffset)*((DER1*saturate(((_Coverage + _CoverageOffset)-(0.8*PosDiff)+cloudTexture1.a*Texture2.a)))-0); /////////////////// DIFFERENCE
              
                clip(shaper*paintTexture1 - _Cutoff+0.4);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }



       
    }
    FallBack "Diffuse"
}