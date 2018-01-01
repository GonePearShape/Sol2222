// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "SkyMaster/ShaderVolumeClouds-MobileSM-2.0" {
    Properties {
        _SunColor ("_SunColor", Color) = (0.95,0.95,0.95,0.8)
        _ShadowColor ("_ShadowColor", Color) = (0.05,0.05,0.1,0.3)
        _ColorDiff ("_ColorDiff", Float ) = 0.5
        _CloudMap ("_CloudMap", 2D) = "white" {}
        _CloudMap1 ("_CloudMap1", 2D) = "white" {}
        _Density ("_Density", Float ) = -0.4
        _Coverage ("_Coverage", Float ) = 4250
        _Transparency ("_Transparency", Float ) = 1
        _Velocity1 ("_Velocity1", Float ) = 23
        _Velocity2 ("_Velocity2", Float ) = 22   
        _LightingControl ("_LightingControl", Vector) = (1,1,-1,0)       
        _HorizonFactor ("_HorizonFactor", Range(0, 10)) = 2    
        _EdgeFactors ("_EdgeFactor2", Vector) = (0,0.52,-1,0) 
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
        //_Mode ("_Mode", Range(0,5)) = 0
        _CutHeight ("_CutHeight", Float) = 240
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
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #pragma multi_compile_fog 
            #pragma multi_compile_fwdbase                      
            #pragma target 2.0

			uniform sampler2D _CloudMap; 
            uniform float4 _CloudMap_ST;
            uniform sampler2D _CloudMap1; 
            uniform float4 _CloudMap1_ST;
            uniform float4 _LightColor0;
            uniform float4 _SunColor;
            uniform float4 _ShadowColor;
            uniform float _ColorDiff;
            uniform float _Density;
            uniform float _Coverage;
            uniform float _Transparency;         
            uniform float _HorizonFactor;
            uniform float4 _LightingControl;
            uniform float2 _EdgeFactors;
            uniform float _Velocity1;
            uniform float _Velocity2;
            // uniform int _Mode;
            uniform float _CutHeight;

            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;    
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 worldPos : TEXCOORD1;                         
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {           
             	VertexOutput o;    
                o.uv0 = v.texcoord0;    
                o.pos = UnityObjectToClipPos(v.vertex );                         
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
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
                float2 UVs2 = (_Velocity2*TimingF*_Time.y + float2(_EdgeFactors.x,_EdgeFactors.y) + UVs);
                float4 Texture1 = tex2D(_CloudMap,UVs2+_CloudMap_ST); 
                float4 Texture2 = tex2D(_CloudMap1,UVs2+_CloudMap1_ST); 

                float DER = i.worldPos.y*0.001;               
                float3 normalA = (((DER*(_Coverage+((cloudTexture.rgb*2)-1)))-(1-(Texture1.rgb*2))));             	
             	float3 normalN = normalize(normalA); 

				
             	float DER1 = -(i.worldPos.y+0)*PosDiff;
             	float PosTDiff = i.worldPos.y*PosDiff;
             	if(i.worldPos.y > change_h){             		
             		DER1 = (1-cloudTexture1.a) *  PosTDiff;
             		//DER1 =  PosTDiff;
             	}

             	float shaper = _Transparency*((DER1*saturate((_Coverage+cloudTexture1.a)))-Texture2.a);

                float3 lightDirect = normalize(_WorldSpaceLightPos0.xyz);
               	lightDirect.y = -lightDirect.y;
               
                float ColDiff =  _ColorDiff+((1+(DER*_LightingControl.r*_LightingControl.g))*0.5); 

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
	             	ColB = _LightColor0.rgb*_SunColor.a*(1-cloudTexture1.a)*_SunColor.rgb*DER1*(1-verticalFactor);
             	//}

                float3 endColor = lerp(_ShadowColor.rgb*(1),ColB, Lerpfactor);

                float4 Fcolor = float4(endColor,saturate(shaper));
                UNITY_APPLY_FOG(i.fogCoord, Fcolor);
                return Fcolor;

            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            //ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd
            #pragma multi_compile_fog
            #pragma target 2.0

            uniform sampler2D _CloudMap; 
            uniform float4 _CloudMap_ST;
            uniform sampler2D _CloudMap1; 
            uniform float4 _CloudMap1_ST;
            uniform float4 _LightColor0;
            uniform float4 _SunColor;
            uniform float4 _ShadowColor;
            uniform float _ColorDiff;
            uniform float _Density;
            uniform float _Coverage;
            uniform float _Transparency;           
            uniform float _HorizonFactor;
            uniform float4 _LightingControl;         
            uniform float2 _EdgeFactors;
            uniform float _Velocity1;
            uniform float _Velocity2;
            uniform float _CutHeight;

            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;    
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 worldPos : TEXCOORD1;                         
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {           
             	VertexOutput o;    
                o.uv0 = v.texcoord0;    
                o.pos = UnityObjectToClipPos(v.vertex );                         
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
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
             	}

             	float shaper = _Transparency*((DER1*saturate((_Coverage+cloudTexture1.a)))-Texture2.a);

                float3 lightDirect = normalize(_WorldSpaceLightPos0.xyz);
               	lightDirect.y = -lightDirect.y;
               
                float ColDiff =  _ColorDiff+((1+(DER*_LightingControl.r*_LightingControl.g))*0.5); 

                float verticalFactor = dot(lightDirect, float3(0,1,0));
             	float Lerpfactor = (ColDiff+(_ShadowColor.a*(dot(lightDirect,normalN)-1)*ColDiff));

                float ColB = _SunColor.rgb;
                //if(_Mode==0){
	                change_h =10;   
	                PosDiff =  0.0006*(i.worldPos.y-change_h);  
	                PosTDiff = i.worldPos.y*PosDiff;          
	             	DER1 = -(i.worldPos.y+0)*PosDiff;

	             	if(i.worldPos.y > change_h){             		
	             		DER1 = (1-cloudTexture1.a) *  PosTDiff;
	             	}
	             	//ColB =  _LightColor0.rgb*_SunColor.a*(1-cloudTexture1.a)*_SunColor.rgb*DER1*(1-verticalFactor);
	             	ColB =  normalN*_SunColor.a*(1-cloudTexture1.a)*_SunColor.rgb*DER1*(1-verticalFactor);
             	//}

                //float3 endColor = lerp(_LightColor0.rgb*_ShadowColor.rgb,ColB, Lerpfactor);
                float3 endColor = _LightColor0.rgb*_ShadowColor.rgb*(1-(Texture1.g*1));

                float4 Fcolor = float4(saturate(shaper)*endColor,shaper);               
                return Fcolor*0.1;

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
            #pragma target 2.0

            uniform sampler2D _CloudMap; 
            uniform float4 _CloudMap_ST;
            uniform sampler2D _CloudMap1; 
            uniform float4 _CloudMap1_ST;
            //uniform float4 _LightColor0;          
            uniform float _Density;
            uniform float _Coverage;
            uniform float _Transparency;         
            uniform float2 _EdgeFactors;
             uniform float _Velocity1;
            uniform float _Velocity2;
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
    FallBack "Diffuse"
}