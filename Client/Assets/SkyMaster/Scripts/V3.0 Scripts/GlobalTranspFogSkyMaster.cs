using System;
using UnityEngine;

namespace Artngame.SKYMASTER
{
	[ExecuteInEditMode]
	[RequireComponent (typeof(Camera))]
	[AddComponentMenu ("Image Effects/Rendering/Global Fog Sky Master")]
	public class GlobalTranspFogSkyMaster : PostEffectsBaseSkyMaster
	{
		[Tooltip("Apply distance-based fog?")]
		public bool  distanceFog = true;
		[Tooltip("Distance fog is based on radial distance from camera when checked")]
		public bool  useRadialDistance = false;
		[Tooltip("Apply height-based fog?")]
		public bool  heightFog = true;
		[Tooltip("Fog top Y coordinate")]
		public float height = 1.0f;
		[Range(0.00001f,10.0f)]
		public float heightDensity = 2.0f;
		[Tooltip("Push fog away from the camera by this amount")]
		public float startDistance = 0.0f;
		
		//SM v1.7
		public Gradient DistGradient = new Gradient();
		public Vector2 GradientBounds = Vector2.zero;
		public float luminance = 0.8f;
		public float lumFac =0.9f; 
		public float ScatterFac = 24.4f;//scatter factor
		public float TurbFac= 324.74f;//turbidity scale
		public float HorizFac = 1;//sun horizon multiplier
		public float turbidity = 10f;
		public float reileigh = 0.8f;
		public float mieCoefficient = 0.1f;
		public float mieDirectionalG = 0.7f; 		
		public float bias = 0.6f;
		public float contrast = 4.12f;
		public Transform Sun;
		public bool FogSky = false;
		public Vector3 TintColor = new Vector3(68,155,345); //68, 155, 345
		public float ClearSkyFac = 1;
		
		public Shader fogShader = null;
		private Material fogMaterial = null;
		public SkyMasterManager SkyManager;
		Texture2D colourPalette;
		bool Made_texture = false;
		
		void Update(){
//			if(SkyManager!=null){
//				if(SkyManager.Current_Time > 8 & SkyManager.Current_Time <20){
//					height = 166;
//				}else{
//					height = 100;
//				}
//			}
			
			
		}
		
		public override bool CheckResources ()
		{
			CheckSupport (true);
			
			fogMaterial = CheckShaderAndCreateMaterial (fogShader, fogMaterial);
			
			if (!isSupported)
				ReportAutoDisable ();
			return isSupported;
		}
		
		//[ImageEffectOpaque]
		void OnRenderImage (RenderTexture source, RenderTexture destination)
		{
			if (CheckResources()==false || (!distanceFog && !heightFog))
			{
				Graphics.Blit (source, destination);
				return;
			}
			
			Camera cam = GetComponent<Camera>();
			Transform camtr = cam.transform;
			float camNear = cam.nearClipPlane;
			float camFar = cam.farClipPlane;
			float camFov = cam.fieldOfView;
			float camAspect = cam.aspect;
			
			Matrix4x4 frustumCorners = Matrix4x4.identity;
			
			float fovWHalf = camFov * 0.5f;
			
			Vector3 toRight = camtr.right * camNear * Mathf.Tan (fovWHalf * Mathf.Deg2Rad) * camAspect;
			Vector3 toTop = camtr.up * camNear * Mathf.Tan (fovWHalf * Mathf.Deg2Rad);
			
			Vector3 topLeft = (camtr.forward * camNear - toRight + toTop);
			float camScale = topLeft.magnitude * camFar/camNear;
			
			topLeft.Normalize();
			topLeft *= camScale;
			
			Vector3 topRight = (camtr.forward * camNear + toRight + toTop);
			topRight.Normalize();
			topRight *= camScale;
			
			Vector3 bottomRight = (camtr.forward * camNear + toRight - toTop);
			bottomRight.Normalize();
			bottomRight *= camScale;
			
			Vector3 bottomLeft = (camtr.forward * camNear - toRight - toTop);
			bottomLeft.Normalize();
			bottomLeft *= camScale;
			
			frustumCorners.SetRow (0, topLeft);
			frustumCorners.SetRow (1, topRight);
			frustumCorners.SetRow (2, bottomRight);
			frustumCorners.SetRow (3, bottomLeft);
			
			var camPos= camtr.position;
			float FdotC = camPos.y-height;
			float paramK = (FdotC <= 0.0f ? 1.0f : 0.0f);
			fogMaterial.SetMatrix ("_FrustumCornersWS", frustumCorners);
			fogMaterial.SetVector ("_CameraWS", camPos);
			fogMaterial.SetVector ("_HeightParams", new Vector4 (height, FdotC, paramK, heightDensity*0.5f));
			fogMaterial.SetVector ("_DistanceParams", new Vector4 (-Mathf.Max(startDistance,0.0f), 0, 0, 0));
			
			//SM v1.7
			fogMaterial.SetFloat("luminance",luminance);
			fogMaterial.SetFloat("lumFac",lumFac);
			fogMaterial.SetFloat("Multiplier1",ScatterFac);
			fogMaterial.SetFloat("Multiplier2",TurbFac);
			fogMaterial.SetFloat("Multiplier3",HorizFac);
			fogMaterial.SetFloat("turbidity",turbidity);
			fogMaterial.SetFloat("reileigh",reileigh);
			fogMaterial.SetFloat("mieCoefficient",mieCoefficient);
			fogMaterial.SetFloat("mieDirectionalG",mieDirectionalG);
			fogMaterial.SetFloat("bias",bias);
			fogMaterial.SetFloat("contrast",contrast);
			fogMaterial.SetVector("v3LightDir",-Sun.forward);
			fogMaterial.SetVector("_TintColor",new Vector4(TintColor.x,TintColor.y,TintColor.z,1));//68, 155, 345
			
			float Foggy = 0;
			if(FogSky){
				Foggy=1;
			}
			fogMaterial.SetFloat("FogSky",Foggy);
			fogMaterial.SetFloat("ClearSkyFac",ClearSkyFac);
			
			var sceneMode= RenderSettings.fogMode;
			var sceneDensity = 0.01f;//RenderSettings.fogDensity;//v3.0
			var sceneStart= RenderSettings.fogStartDistance;
			var sceneEnd= RenderSettings.fogEndDistance;
			Vector4 sceneParams;
			bool  linear = (sceneMode == FogMode.Linear);
			float diff = linear ? sceneEnd - sceneStart : 0.0f;
			float invDiff = Mathf.Abs(diff) > 0.0001f ? 1.0f / diff : 0.0f;
			sceneParams.x = sceneDensity * 1.2011224087f; // density / sqrt(ln(2)), used by Exp2 fog mode
			sceneParams.y = sceneDensity * 1.4426950408f; // density / ln(2), used by Exp fog mode
			sceneParams.z = linear ? -invDiff : 0.0f;
			sceneParams.w = linear ? sceneEnd * invDiff : 0.0f;
			fogMaterial.SetVector ("_SceneFogParams", sceneParams);
			fogMaterial.SetVector ("_SceneFogMode", new Vector4((int)sceneMode, useRadialDistance ? 1 : 0, 0, 0));
			
			int pass = 0;
			if (distanceFog && heightFog)
				pass = 0; // distance + height
			else if (distanceFog)
				pass = 1; // distance only
			else
				pass = 2; // height only
			
			
			//SM v1.7
			//if (colourPalette == null) 
			{
				Color[] colourArray = new Color[256];			
				for(int i=0;i<colourArray.Length;i++){
					colourArray[i] = DistGradient.Evaluate((float)i/(float)colourArray.Length);
				}	
				
				if(!Made_texture){
					colourPalette = new Texture2D (256, 10, TextureFormat.ARGB32, false);
					colourPalette.filterMode = FilterMode.Point;
					colourPalette.wrapMode = TextureWrapMode.Clamp;
					Made_texture=true;
				}
				
				
				for(int x = 0; x < 256; x++){
					for(int y = 0; y < 10; y++){
						colourPalette.SetPixel(x,y,colourArray[x]);
					}
				}
				
				colourPalette.Apply();
				
				
			}
			
			
			CustomGraphicsBlit (source, destination, fogMaterial, pass,DistGradient, GradientBounds, colourPalette);
		}
		
		static void CustomGraphicsBlit (RenderTexture source, RenderTexture dest, Material fxMaterial, int passNr,Gradient DistGradient, Vector2 GradientBounds, Texture2D colourPalette )
		{
			RenderTexture.active = dest;
			
			fxMaterial.SetTexture ("_MainTex", source);
			
			
			
			
			
			fxMaterial.SetTexture ("_ColorRamp", colourPalette);
			
			if(GradientBounds != Vector2.zero){
				fxMaterial.SetFloat ("_Close", GradientBounds.x);
				fxMaterial.SetFloat ("_Far", GradientBounds.y);
			}
			
			
			
			
			GL.PushMatrix ();
			GL.LoadOrtho ();
			
			fxMaterial.SetPass (passNr);
			
			GL.Begin (GL.QUADS);
			
			GL.MultiTexCoord2 (0, 0.0f, 0.0f);
			GL.Vertex3 (0.0f, 0.0f, 3.0f); // BL
			
			GL.MultiTexCoord2 (0, 1.0f, 0.0f);
			GL.Vertex3 (1.0f, 0.0f, 2.0f); // BR
			
			GL.MultiTexCoord2 (0, 1.0f, 1.0f);
			GL.Vertex3 (1.0f, 1.0f, 1.0f); // TR
			
			GL.MultiTexCoord2 (0, 0.0f, 1.0f);
			GL.Vertex3 (0.0f, 1.0f, 0.0f); // TL
			
			GL.End ();
			GL.PopMatrix ();
		}
	}
}
