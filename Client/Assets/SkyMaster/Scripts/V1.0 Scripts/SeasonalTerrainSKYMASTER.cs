using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Artngame.SKYMASTER{

	[ExecuteInEditMode]
public class SeasonalTerrainSKYMASTER : MonoBehaviour 
{

		//v3.4
		[Tooltip("For use with curve based system")]
		public float VFogDistance = 200;//if DistanceFogOn
		public bool DistanceFogOn = true;
		public bool HeightFogOn = true;
		public bool SkyFogOn = false;
		public float fogDensity = 0.9f;
		public float fogGradientDistance = 3500;//offset gradient extend to horizon

		//v3.3e
		public bool UseFogCurves = false;//use curves to override TOD definition for certain volume fog parameters
		[SerializeField]
		public AnimationCurve heightOffsetFogCurve = new AnimationCurve(new Keyframe(0,1),new Keyframe(0.75f,1),new Keyframe(0.85f,1),new Keyframe(0.95f,1),new Keyframe(1,1));
		[SerializeField]
		public AnimationCurve luminanceVFogCurve = new AnimationCurve(new Keyframe(0,0),new Keyframe(0.75f,0),new Keyframe(0.85f,0),new Keyframe(0.9f,0),new Keyframe(1,0));//-0.5 to 8
		[SerializeField]
		public AnimationCurve lumFactorFogCurve = new AnimationCurve(new Keyframe(0,1),new Keyframe(0.75f,1),new Keyframe(0.85f,1),new Keyframe(0.9f,1),new Keyframe(1,1));//-1 to 15
		[SerializeField]
		public AnimationCurve scatterFacFogCurve = new AnimationCurve(new Keyframe(0,1),new Keyframe(0.75f,1),new Keyframe(0.85f,1),new Keyframe(0.9f,1),new Keyframe(1,1));//-55 to 55
		[SerializeField]
		public AnimationCurve turbidityFogCurve = new AnimationCurve(new Keyframe(0,1),new Keyframe(0.75f,1),new Keyframe(0.85f,1),new Keyframe(0.9f,1),new Keyframe(1,1));//0 to 100
		[SerializeField]
		public AnimationCurve turbFacFogCurve = new AnimationCurve(new Keyframe(0,1),new Keyframe(0.75f,1),new Keyframe(0.85f,1),new Keyframe(0.9f,1),new Keyframe(1,1));//0 to 1500
		[SerializeField]
		public AnimationCurve horizonFogCurve= new AnimationCurve(new Keyframe(0,1),new Keyframe(1,1));//-55 to 55
		[SerializeField]
		public AnimationCurve contrastFogCurve = new AnimationCurve(new Keyframe(0,1),new Keyframe(0.75f,1),new Keyframe(0.85f,1),new Keyframe(0.9f,1),new Keyframe(1,1));//1 to 200

		//v3.4.3
		public void setVFogCurvesPresetA(){
			heightOffsetFogCurve = new AnimationCurve(new Keyframe(0,1),new Keyframe(0.75f,1),new Keyframe(0.85f,1),new Keyframe(0.95f,1),new Keyframe(1,1));
			luminanceVFogCurve = new AnimationCurve(new Keyframe(0,1),new Keyframe(0.75f,1),new Keyframe(0.85f,1),new Keyframe(0.9f,1),new Keyframe(1,1));//-0.5 to 8
			lumFactorFogCurve = new AnimationCurve(new Keyframe(0,1),new Keyframe(0.75f,1),new Keyframe(0.85f,1),new Keyframe(0.9f,1),new Keyframe(1,1));//-1 to 15
			scatterFacFogCurve = new AnimationCurve(new Keyframe(0,1),new Keyframe(0.75f,1),new Keyframe(0.85f,1),new Keyframe(0.9f,1),new Keyframe(1,1));//-55 to 55
			turbidityFogCurve = new AnimationCurve(new Keyframe(0,1),new Keyframe(0.75f,1),new Keyframe(0.85f,1),new Keyframe(0.9f,1),new Keyframe(1,1));//0 to 100
			turbFacFogCurve = new AnimationCurve(new Keyframe(0,1),new Keyframe(0.75f,1),new Keyframe(0.85f,1),new Keyframe(0.9f,1),new Keyframe(1,1));//0 to 1500
			horizonFogCurve= new AnimationCurve(new Keyframe(0,1),new Keyframe(1,1));//-55 to 55
			contrastFogCurve = new AnimationCurve(new Keyframe(0,1),new Keyframe(0.75f,1),new Keyframe(0.85f,1),new Keyframe(0.9f,1),new Keyframe(1,1));//1 to 200
			AddFogHeightOffset = 0;//100;
			fogDensity = 150;//30;
			DistanceFogOn = false;
			VFogDistance = 0;
		}
		public void setVFogCurvesPresetC(){
			//heightOffsetFogCurve = new AnimationCurve(new Keyframe(0,55),new Keyframe(0.75f,45),new Keyframe(0.85f,180),new Keyframe(0.95f,54),new Keyframe(1,55));
			heightOffsetFogCurve = new AnimationCurve(new Keyframe(0,55),new Keyframe(0.75f,45),new Keyframe(0.85f,60),new Keyframe(0.90f,130),new Keyframe(0.95f,54),new Keyframe(1,55));
			luminanceVFogCurve = new AnimationCurve(new Keyframe(0,2),new Keyframe(0.75f,2),new Keyframe(0.85f,9),new Keyframe(0.9f,2),new Keyframe(1,2));//-0.5 to 8
			lumFactorFogCurve = new AnimationCurve(new Keyframe(0,0.2f),new Keyframe(0.75f,0.2f),new Keyframe(0.85f,-0.1f),new Keyframe(0.9f,0.2f),new Keyframe(1,0.2f));//-1 to 15
			scatterFacFogCurve = new AnimationCurve(new Keyframe(0,12),new Keyframe(0.75f,20),new Keyframe(0.85f,50f),new Keyframe(0.9f,20),new Keyframe(1,12));//-55 to 55
			turbidityFogCurve = new AnimationCurve(new Keyframe(0,12),new Keyframe(0.75f,1),new Keyframe(0.85f,108f),new Keyframe(0.9f,5),new Keyframe(1,12));//0 to 100
			turbFacFogCurve = new AnimationCurve(new Keyframe(0,12),new Keyframe(0.75f,12f),new Keyframe(0.85f,58f),new Keyframe(0.9f,4),new Keyframe(1,2));//0 to 1500
			horizonFogCurve= new AnimationCurve(new Keyframe(0,1),new Keyframe(1,1));//-55 to 55
			contrastFogCurve = new AnimationCurve(new Keyframe(0,6),new Keyframe(0.75f,1.5f),new Keyframe(0.85f,1.45f),new Keyframe(0.9f,1.5f),new Keyframe(1,6));//1 to 200
			//contrastFogCurve = new AnimationCurve(new Keyframe(0,6),new Keyframe(0.75f,1.5f),new Keyframe(0.85f,0.5f),new Keyframe(0.9f,1.5f),new Keyframe(1,6));//1 to 200
			AddFogHeightOffset = 0;
			fogDensity = 200;
			DistanceFogOn = false;
			VFogDistance = 0;
		}
		public void setVFogCurvesPresetD(){
			//heightOffsetFogCurve = new AnimationCurve(new Keyframe(0,55),new Keyframe(0.75f,45),new Keyframe(0.85f,180),new Keyframe(0.95f,54),new Keyframe(1,55));
			heightOffsetFogCurve = new AnimationCurve(new Keyframe(0,55),new Keyframe(0.75f,45),new Keyframe(0.85f,60),new Keyframe(0.90f,130),new Keyframe(0.95f,54),new Keyframe(1,55));
			luminanceVFogCurve = new AnimationCurve(new Keyframe(0,2),new Keyframe(0.75f,-0.6f),new Keyframe(0.85f,9),new Keyframe(0.9f,2),new Keyframe(1,2));//-0.5 to 8
			lumFactorFogCurve = new AnimationCurve(new Keyframe(0,0.2f),new Keyframe(0.75f,0.2f),new Keyframe(0.85f,-0.1f),new Keyframe(0.9f,0.2f),new Keyframe(1,0.2f));//-1 to 15
			scatterFacFogCurve = new AnimationCurve(new Keyframe(0,12),new Keyframe(0.75f,20),new Keyframe(0.85f,50f),new Keyframe(0.9f,20),new Keyframe(1,12));//-55 to 55
			turbidityFogCurve = new AnimationCurve(new Keyframe(0,12),new Keyframe(0.75f,1),new Keyframe(0.85f,108f),new Keyframe(0.9f,3),new Keyframe(1,12));//0 to 100
			turbFacFogCurve = new AnimationCurve(new Keyframe(0,12),new Keyframe(0.75f,12f),new Keyframe(0.85f,58f),new Keyframe(0.9f,3),new Keyframe(1,2));//0 to 1500
			horizonFogCurve= new AnimationCurve(new Keyframe(0,1),new Keyframe(1,1));//-55 to 55
			contrastFogCurve = new AnimationCurve(new Keyframe(0,6),new Keyframe(0.75f,5.5f),new Keyframe(0.85f,3.45f),new Keyframe(0.9f,2.5f),new Keyframe(1,6));//1 to 200
			//contrastFogCurve = new AnimationCurve(new Keyframe(0,6),new Keyframe(0.75f,1.5f),new Keyframe(0.85f,0.5f),new Keyframe(0.9f,1.5f),new Keyframe(1,6));//1 to 200
			AddFogHeightOffset = 40;
			fogDensity = 150;
			DistanceFogOn = false;
			VFogDistance = 0;
		}
		public void setVFogCurvesPresetB(){
			//heightOffsetFogCurve = new AnimationCurve(new Keyframe(0,55),new Keyframe(0.75f,45),new Keyframe(0.85f,180),new Keyframe(0.95f,54),new Keyframe(1,55));
			heightOffsetFogCurve = new AnimationCurve(new Keyframe(0,55),new Keyframe(0.75f,45),new Keyframe(0.85f,60),new Keyframe(0.90f,130),new Keyframe(0.95f,54),new Keyframe(1,55));
			luminanceVFogCurve = new AnimationCurve(new Keyframe(0,2),new Keyframe(0.75f,2),new Keyframe(0.85f,9),new Keyframe(0.9f,2),new Keyframe(1,2));//-0.5 to 8
			lumFactorFogCurve = new AnimationCurve(new Keyframe(0,0.2f),new Keyframe(0.75f,0.2f),new Keyframe(0.85f,-0.25f),new Keyframe(0.9f,0.2f),new Keyframe(1,0.2f));//-1 to 15
			scatterFacFogCurve = new AnimationCurve(new Keyframe(0,12),new Keyframe(0.75f,20),new Keyframe(0.85f,50f),new Keyframe(0.9f,20),new Keyframe(1,12));//-55 to 55
			turbidityFogCurve = new AnimationCurve(new Keyframe(0,12),new Keyframe(0.75f,20),new Keyframe(0.85f,108f),new Keyframe(0.9f,20),new Keyframe(1,12));//0 to 100
			turbFacFogCurve = new AnimationCurve(new Keyframe(0,12),new Keyframe(0.75f,12f),new Keyframe(0.85f,58f),new Keyframe(0.9f,12),new Keyframe(1,2));//0 to 1500
			horizonFogCurve= new AnimationCurve(new Keyframe(0,1),new Keyframe(1,1));//-55 to 55
			contrastFogCurve = new AnimationCurve(new Keyframe(0,6),new Keyframe(0.75f,1.5f),new Keyframe(0.85f,1.45f),new Keyframe(0.9f,1.5f),new Keyframe(1,6));//1 to 200
			//contrastFogCurve = new AnimationCurve(new Keyframe(0,6),new Keyframe(0.75f,1.5f),new Keyframe(0.85f,0.5f),new Keyframe(0.9f,1.5f),new Keyframe(1,6));//1 to 200
			AddFogHeightOffset = -100;
			fogDensity = 200;
			DistanceFogOn = false;
			VFogDistance = 0;
		}

		//v3.3 - volume fog application speed
		public float VolumeFogSpeed = 1;

		//v3.0.3
		public bool tagBasedTreeGrab = false;//get terrain trees by their "SkyMasterTree" tag, no to be enable if tag not defined

		//transparent fog
		public bool UseTranspVFog = false;//use volume fog that affects clouds, it does not support looking at objects through clouds

		//v3.0a
		public float AddFogHeightOffset=0;//extra offset factor in the fog height
		public float AddFogDensityOffset = 0;//extra density over the preset one
		//public float AddShaftsIntensity=0;//extra shaft intensity overwater
		public float AddShaftsIntensityUnder = 0;//extra shaft intensity underwater
		public float AddShaftsSizeUnder = 0;//extra shaft length underwater
		public float ShaftBlurRadiusOffset = 0;

		//v3.0
		public bool Lerp_gradient = false;
		public bool FogHeightByTerrain = false;//ovveride preset fog height based on terrain height
		public float FogheightOffset = 0;
		public float FogdensityOffset = 0;
		float FogUnityOffset = 0;

		//v2.2 - trees Unity terrain
		public List<GameObject> TreePefabs = new List<GameObject>();
		public Color currentTerrainTreeCol = Color.white;
		public bool AlwaysUpdateBillboards = false;//add small rotation to camera to trigger refresh
		//public Color prevTerrainTreeCol = Color.white;
		Transform Cam_transf;



	//v2.1
		public List<GlobalFogSkyMaster> GradientHolders = new List<GlobalFogSkyMaster>();//if != null copy gradient from component GlobalFogSkyMaster
		public bool StereoMode = false; // cardboard setup left/right camera
		GlobalFogSkyMaster SkyFogL;
		SunShaftsSkyMaster SunShaftsL;
		GlobalFogSkyMaster SkyFogR;
		SunShaftsSkyMaster SunShaftsR;//check if null and fill if SteroMode is on
		public GameObject LeftCam;
		public GameObject RightCam;

		GlobalTranspFogSkyMaster SkyFogLT;
		GlobalTranspFogSkyMaster SkyFogRT;

	//v1.7
	GlobalTranspFogSkyMaster SkyFogTransp;
	GlobalFogSkyMaster SkyFog;
	SunShaftsSkyMaster SunShafts;
	public bool ImageEffectFog=false;
		public bool Use_both_fogs = false;

	public bool ImageEffectShafts=false;
	//public bool ImageEffectSkyUpate=false; // get image effect volume fog parameters from SkymasterManager parameters.
	public int FogPreset = 0;
		public bool UpdateLeafMat = false;
		public List<Material> LeafMats;
		public Color Rays_day_color = new Color(236f/255f,49f/255f,20f/255f,255f/255f);
		public Color Rays_night_color = new Color(236f/255f,236f/255f,236f/255f,255f/255f);
		public float Shafts_intensity = 1.45f;
		public float Moon_Shafts_intensity = 0.45f;//v3.0
		public bool Mesh_moon = false; //If mesh used, smooth out shafts
		public bool Glow_moon = false; //Enable additive mode in shafts image effect
		public bool Glow_sun = false; // Additive shafts

	GameObject[] SkyMaster_TREE_objects;
	public Color TreeA_color = Color.white;
	public Color Terrain_tint = Color.white;
	public Color Grass_tint = Color.white;
	public bool Enable_trasition = false;	
		public TerrainData tData;//v3.3b
	public Material TerrainMat;

	Color Starting_grass_tint;
	Color Starting_terrain_tint;

	public float Trans_speed_tree=0.4f;
	public float Trans_speed_terr=1f;
	public float Trans_speed_grass=0.4f;
	public float Trans_speed_sky = 0.4f;

		//v1.2.5
		public bool Mesh_Terrain = false;
		public bool Foggy_Terrain = false;//Use with terrain fog shader - mesh terrain
		public bool Fog_Sky_Update = false;

		public Vector3 SUN_POS;
		public SkyMasterManager SkyManager;
		public float fog_depth = 0.29f;// 1.5f;
		public float reileigh = 1.3f;//2.0f;
		public float mieCoefficient = 1;//0.1f;
		public float mieDirectionalG = 0.1f;
		public float ExposureBias = 0.11f;//0.15f; 
		const float n = 1.0003f; 
		const float N = 2.545E25f;  
		const float pn = 0.035f;  
		public Vector3 lambda =  new Vector3(680E-9f, 550E-9f, 450E-9f);//new Vector3(680E-9f, 550E-9f, 450E-9f);
		public Vector3 K = new Vector3(0.9f, 0.5f, 0.5f);//new Vector3(0.686f, 0.678f, 0.666f);

void Start () {

	//v1.2.5
	if(!Mesh_Terrain){
		if (Terrain.activeTerrain != null) {
			tData = Terrain.activeTerrain.terrainData;
			Starting_grass_tint = tData.wavingGrassTint;
		}
	}

			if (TerrainMat != null) {//v3.3
				Starting_terrain_tint = TerrainMat.color;
			}

	//v2.2
			//currentTerrainTreeCol = Color.red;	//v3.3e
			Cam_transf = Camera.main.transform;

	//v3.0
	RunPresets(SkyFog,SunShafts,10,true);
	if (SkyFogTransp != null) {
		RunPresetsT(SkyFogTransp, SunShafts, 10, true);
	}
}
void OnApplicationQuit() {

			if(!Mesh_Terrain){
				if(tData != null){
	tData.wavingGrassTint = Starting_grass_tint;
				}
			}

			if (TerrainMat != null) {//v3.3
				TerrainMat.color = Starting_terrain_tint;
			}
}

		void Update(){

			if (Application.isPlaying) {
				//v3.4.8
				if (Cam_transf == null) {
					Cam_transf = Camera.main.transform;
				}
			}

			if (AlwaysUpdateBillboards) {
				if(Application.isPlaying){
					if(toggle_rot==1){
						//Cam_transf.Rotate(Vector3.up,SmallRotFactor);
						//toggle_rot = 0;
						Cam_transf.Rotate(Vector3.up,SmallRotFactor);
					}
					if(currentTerrainTreeCol != TreeA_color){ //start rotating the camera little by little so is not visible, until effect ends. Use slow rate to make sure rotation is enough
						if(toggle_rot == 0){
							//Cam_transf.Rotate(Vector3.up,SmallRotFactor);
							toggle_rot = 1;
						}
					}else{
						toggle_rot = 0;
					}
				}
			}
		}
		int toggle_rot = 0;
		public float SmallRotFactor = 0.0001f;

		//v3.4.8
		Material[] mats;

void LateUpdate () {

			//v3.3b
			if(!Mesh_Terrain){
				if (tData != null) {
					//tData.wavingGrassTint = Starting_grass_tint;
				} else {
					if (Terrain.activeTerrain != null) {
						tData = Terrain.activeTerrain.terrainData;
					}
				}
			}

			if (SkyFogTransp == null) {
				SkyFogTransp = Camera.main.GetComponent<GlobalTranspFogSkyMaster> ();
			}

			if (SkyFog == null) {
				//find in camera
				SkyFog = Camera.main.GetComponent<GlobalFogSkyMaster> ();
				if (SkyFog == null) {
					if (ImageEffectFog) {
						Debug.Log ("Please enter the GlobalFogSkyMaster script in the camera to enable the volumetric fog");
					}
				} else {
					//Debug.Log ("GlobalFogSkyMaster script found in the camera, enable 'ImageEffectFog' option to use the volumetric fog");
				}
			} else {
				//v2.0.1
				if(SkyFog.Sun == null){
					SkyFog.Sun = SkyManager.SunObj.transform;
				}
				if(SkyFog.SkyManager == null){
					SkyFog.SkyManager = SkyManager;
				}
			}

			if (SunShafts == null) {
				//find in camera
				SunShafts = Camera.main.GetComponent<SunShaftsSkyMaster> ();
				if (SunShafts == null) {
					if (ImageEffectShafts) {
						Debug.Log ("Please enter the SunShaftsSkyMaster script in the camera to enable the Sun Shafts");
					}
				}
			} else {
				//v2.0.1 - auto assign sun if not present
				if(SunShafts.sunTransform == null){
					SunShafts.sunTransform = SkyManager.SunObj.transform;
				}
			}

			//v2.1
			if(StereoMode){
				if(SkyFog != null & LeftCam != null & RightCam != null){
					//try to grab effect, if does not exist add/copy from main camera SkyFog
					if(SkyFogL == null){
						SkyFogL = LeftCam.GetComponent(typeof(GlobalFogSkyMaster)) as GlobalFogSkyMaster;
						if(SkyFogL == null){//if not found, copy from main
							LeftCam.AddComponent<GlobalFogSkyMaster>();
						}
					}
					if(SkyFogR == null){
						SkyFogR = RightCam.GetComponent(typeof(GlobalFogSkyMaster)) as GlobalFogSkyMaster;
						if(SkyFogR == null){//if not found, copy from main
							RightCam.AddComponent<GlobalFogSkyMaster>();
						}
					}

					if(SkyFogLT == null){
						SkyFogLT = LeftCam.GetComponent(typeof(GlobalTranspFogSkyMaster)) as GlobalTranspFogSkyMaster;
						if(SkyFogLT == null){//if not found, copy from main
							LeftCam.AddComponent<GlobalTranspFogSkyMaster>();
						}
					}
					if(SkyFogRT == null){
						SkyFogRT = RightCam.GetComponent(typeof(GlobalTranspFogSkyMaster)) as GlobalTranspFogSkyMaster;
						if(SkyFogRT == null){//if not found, copy from main
							RightCam.AddComponent<GlobalTranspFogSkyMaster>();
						}
					}

					if(SunShaftsL == null){
						SunShaftsL = LeftCam.GetComponent(typeof(SunShaftsSkyMaster)) as SunShaftsSkyMaster;
						if(SunShaftsL == null){//if not found, copy from main
							LeftCam.AddComponent<SunShaftsSkyMaster>();
						}
					}
					if(SunShaftsR == null){
						SunShaftsR = RightCam.GetComponent(typeof(SunShaftsSkyMaster)) as SunShaftsSkyMaster;
						if(SunShaftsR == null){//if not found, copy from main
							RightCam.AddComponent<SunShaftsSkyMaster>();
						}
					}

					if(SkyFogL != null & SkyFogR != null & SunShaftsL != null & SunShaftsR != null){
						float speed = 10;
						if(!Application.isPlaying){
							speed = 10000;
						}


						if(SkyFogTransp != null && SkyFogLT != null & SkyFogRT != null & UseTranspVFog){
							RunPresetsT(SkyFogLT,SunShaftsL,speed,false);
							RunPresetsT(SkyFogRT,SunShaftsR,speed,false);
						}else{
							RunPresets(SkyFogL,SunShaftsL,speed,false);
							RunPresets(SkyFogR,SunShaftsR,speed,false);
						}

					}

				}else{
					Debug.Log ("Please enter the left/right cameras in the script parameters to use stereo mode");
				}
			}else{
				float speed = 10;
				if(!Application.isPlaying){
					speed = 10000;
				}

				if(SkyFogTransp != null & UseTranspVFog){
					RunPresetsT(SkyFogTransp,SunShafts,speed,false);
				}else{
					RunPresets(SkyFog,SunShafts,speed,false);
				}
			}

			if (ImageEffectFog) { //v3.3a
				if (UseTranspVFog) {
					if (SkyFog != null) {
						SkyFog.enabled = false;
					}
					if (SkyFogTransp != null) {
						SkyFogTransp.enabled = true;
					}
					if (SkyFogLT != null & SkyFogRT != null) {
						SkyFogLT.enabled = true;
						SkyFogRT.enabled = true;
					}
					if (SkyFogL != null & SkyFogR != null) {
						SkyFogL.enabled = false;
						SkyFogR.enabled = false;
					}
				} else {
					if (SkyFog != null) {
						SkyFog.enabled = true;
					}
					if (SkyFogTransp != null) {
						SkyFogTransp.enabled = false;
					}
					if (SkyFogLT != null & SkyFogRT != null) {
						SkyFogLT.enabled = false;
						SkyFogRT.enabled = false;
					}
					if (SkyFogL != null & SkyFogR != null) {
						SkyFogL.enabled = true;
						SkyFogR.enabled = true;
					}
				}
			}

			//v2.1 - run presets moved to function

//////////////////////////////////////////////// CHANGE STANDALONE TREE COLOR and GRASS TINT ///////////////////////
if (Enable_trasition){
		/////////////////// TREE COLOR //////////////////////
				 
				//v2.2
				if(currentTerrainTreeCol != TreeA_color){
					currentTerrainTreeCol =  Color.Lerp (currentTerrainTreeCol,TreeA_color,Trans_speed_tree*Time.deltaTime);
					Shader.SetGlobalVector("_UnityTerrainTreeTintColorSM", currentTerrainTreeCol);
				}
//				if(TreePefabs != null){
//					for (int i = 0;i<TreePefabs.Count;i++){
//						if(TreePefabs[i] !=null){
//							for(int j=0;j<TreePefabs[i].GetComponent<Renderer>().sharedMaterials.Length;j++){
//								if(TreePefabs[i].GetComponent<Renderer>().sharedMaterials[j].name.Contains("Leaf")
//								   | TreePefabs[i].GetComponent<Renderer>().sharedMaterials[j].name.Contains("leaf")
//								   | TreePefabs[i].GetComponent<Renderer>().sharedMaterials[j].name.Contains("Leaves")
//								   | TreePefabs[i].GetComponent<Renderer>().sharedMaterials[j].name.Contains("leaves")
//								   ){
//									if(TreePefabs[i].GetComponent<Renderer>().sharedMaterials[j].HasProperty("_Color")){
//										if(TreePefabs[i].GetComponent<Renderer>().sharedMaterials[j].color != TreeA_color){
//											Color current = TreePefabs[i].GetComponent<Renderer>().sharedMaterials[j].color;
//											TreePefabs[i].GetComponent<Renderer>().sharedMaterials[j].color = Color.Lerp (current,TreeA_color,Trans_speed_tree*Time.deltaTime);
//										}
//									}
//								}
//							}
//						}
//					}
//				}	
				//v3.4.8
				if(TreePefabs != null){
					for (int i = 0;i<TreePefabs.Count;i++){
						if(TreePefabs[i] !=null){
							mats = TreePefabs [i].GetComponent<Renderer> ().sharedMaterials;
							for(int j=0;j<mats.Length;j++){
								if(mats[j].name.Contains("Leaf")
									|| mats[j].name.Contains("leaf")
									|| mats[j].name.Contains("Leaves")
									|| mats[j].name.Contains("leaves")
								){
									if(mats[j].HasProperty("_Color")){
										if(mats[j].color != TreeA_color){
											Color current = mats[j].color;
											mats[j].color = Color.Lerp (current,TreeA_color,Trans_speed_tree*Time.deltaTime);
										}
									}
								}
							}
						}
					}
				}
				 
		if(tagBasedTreeGrab && SkyMaster_TREE_objects == null){//v3.0.3
			SkyMaster_TREE_objects = GameObject.FindGameObjectsWithTag("SkyMasterTree");
		}
		//if(TreeA_color_prev != TreeA_color | Terrain_tint_prev!=Terrain_tint | Grass_tint_prev!=Grass_tint){
		if(SkyMaster_TREE_objects != null & Application.isPlaying){
			if(SkyMaster_TREE_objects.Length > 0){
				
						for (int i = 0;i<SkyMaster_TREE_objects.Length;i++){
							//v1.7 - handle Speed tree first
//							if(SkyMaster_TREE_objects[i].GetComponent<Renderer>().sharedMaterials.Length >4){
//								//Debug.l
//								if(SkyMaster_TREE_objects[i].GetComponent<Renderer>().sharedMaterials[2].color != TreeA_color){
//									Color current = SkyMaster_TREE_objects[i].GetComponent<Renderer>().sharedMaterials[2].color;
//									SkyMaster_TREE_objects[i].GetComponent<Renderer>().sharedMaterials[2].color = Color.Lerp (current,TreeA_color,Trans_speed_tree*Time.deltaTime);
//								}
//								if(SkyMaster_TREE_objects[i].GetComponent<Renderer>().sharedMaterials[4].color != TreeA_color){
//									Color current = SkyMaster_TREE_objects[i].GetComponent<Renderer>().sharedMaterials[4].color;
//									SkyMaster_TREE_objects[i].GetComponent<Renderer>().sharedMaterials[4].color = Color.Lerp (current,TreeA_color,Trans_speed_tree*Time.deltaTime);
//								}
//							}else{
//								if(SkyMaster_TREE_objects[i].GetComponent<Renderer>().sharedMaterials[1].color != TreeA_color){
//									Color current = SkyMaster_TREE_objects[i].GetComponent<Renderer>().sharedMaterials[1].color;
//									SkyMaster_TREE_objects[i].GetComponent<Renderer>().sharedMaterials[1].color = Color.Lerp (current,TreeA_color,Trans_speed_tree*Time.deltaTime);
//								}
//							}
							//Search in materials for Leaf or Leaves
							for(int j=0;j<SkyMaster_TREE_objects[i].GetComponent<Renderer>().sharedMaterials.Length;j++){
								if(SkyMaster_TREE_objects[i].GetComponent<Renderer>().sharedMaterials[j].name.Contains("Leaf")
								   | SkyMaster_TREE_objects[i].GetComponent<Renderer>().sharedMaterials[j].name.Contains("leaf")
								   | SkyMaster_TREE_objects[i].GetComponent<Renderer>().sharedMaterials[j].name.Contains("Leaves")
								   | SkyMaster_TREE_objects[i].GetComponent<Renderer>().sharedMaterials[j].name.Contains("leaves")
								   ){
									if(SkyMaster_TREE_objects[i].GetComponent<Renderer>().sharedMaterials[j].color != TreeA_color){
										Color current = SkyMaster_TREE_objects[i].GetComponent<Renderer>().sharedMaterials[j].color;
										SkyMaster_TREE_objects[i].GetComponent<Renderer>().sharedMaterials[j].color = Color.Lerp (current,TreeA_color,Trans_speed_tree*Time.deltaTime);
									}
								}
							}
//							if(UpdateLeafMat){
//								for(int j=0;j<LeafMats.Count;j++){
//									if(LeafMats[j].color != TreeA_color){
//										Color current = LeafMats[j].color;
//										LeafMats[j].color = Color.Lerp (current,TreeA_color,Trans_speed_tree*Time.deltaTime);
//									}
//								}
//							}
						}

			}
		}

				//v1.7
				if(Application.isPlaying){
					if(UpdateLeafMat){
						for(int j=0;j<LeafMats.Count;j++){
							if(LeafMats[j].color != TreeA_color){
								Color current = LeafMats[j].color;
								LeafMats[j].color = Color.Lerp (current,TreeA_color,Trans_speed_tree*Time.deltaTime);
							}
						}
					}
				}

		//v1.2.5
		if(!Mesh_Terrain){
			if(tData.wavingGrassTint != Grass_tint){
				Color current1 = tData.wavingGrassTint;		 
				tData.wavingGrassTint = Color.Lerp (current1,Grass_tint,Trans_speed_grass*Time.deltaTime);

			}
		}

		//if(TerrainMat != null && TerrainMat.color != Terrain_tint & Application.isPlaying){ //v3.4
		if(TerrainMat != null && TerrainMat.color != Terrain_tint){
					if (Application.isPlaying) {
						Color current2 = TerrainMat.color;
						TerrainMat.color = Color.Lerp (current2, Terrain_tint, Trans_speed_terr * Time.deltaTime);
					} else {
						//Color current2 = TerrainMat.color;
						TerrainMat.color = Terrain_tint;
					}
		}		
	}
	//////////////////////////////////////////////// CHANGE STANDALONE TREE COLOR and GRASS TINT ///////////////////////

	if(Foggy_Terrain){
		if(TerrainMat != null && SkyManager != null){

					//Debug.Log ("AA");
					if(Fog_Sky_Update){
						reileigh = SkyManager.m_fRayleighScaleDepth;
						reileigh = SkyManager.m_Kr;
						mieCoefficient = SkyManager.m_Km;
						//fog_depth = 1.5f;
						TerrainMat.SetVector("sunPosition", -SkyManager.SunObj.transform.forward.normalized);
						TerrainMat.SetVector("betaR", totalRayleigh(lambda) * reileigh);
						TerrainMat.SetVector("betaM", totalMie(lambda, K, fog_depth) * mieCoefficient);
						TerrainMat.SetFloat("fog_depth", fog_depth);
						TerrainMat.SetFloat("mieCoefficient", mieCoefficient);
						TerrainMat.SetFloat("mieDirectionalG", mieDirectionalG);    
						TerrainMat.SetFloat("ExposureBias", ExposureBias); 
					}else{
						TerrainMat.SetVector("sunPosition", -SkyManager.SunObj.transform.forward.normalized);
						//TerrainMat.SetVector("sunPosition", SUN_POS);
						TerrainMat.SetVector("betaR", totalRayleigh(lambda) * reileigh);
						TerrainMat.SetVector("betaM", totalMie(lambda, K, fog_depth) * mieCoefficient);
						TerrainMat.SetFloat("fog_depth", fog_depth);
						TerrainMat.SetFloat("mieCoefficient", mieCoefficient);
						TerrainMat.SetFloat("mieDirectionalG", mieDirectionalG);    
						TerrainMat.SetFloat("ExposureBias", ExposureBias); 
					}
		}
	}

   }// end LateUpdate

		static Vector3 totalRayleigh(Vector3 lambda)
		{
			Vector3 result = new Vector3((8.0f * Mathf.Pow(Mathf.PI, 3.0f) * Mathf.Pow(Mathf.Pow(n, 2.0f) - 1.0f, 2.0f) * (6.0f + 3.0f * pn)) / (3.0f * N * Mathf.Pow(lambda.x, 4.0f) * (6.0f - 7.0f * pn)),
			                             (8.0f * Mathf.Pow(Mathf.PI, 3.0f) * Mathf.Pow(Mathf.Pow(n, 2.0f) - 1.0f, 2.0f) * (6.0f + 3.0f * pn)) / (3.0f * N * Mathf.Pow(lambda.y, 4.0f) * (6.0f - 7.0f * pn)),
			                             (8.0f * Mathf.Pow(Mathf.PI, 3.0f) * Mathf.Pow(Mathf.Pow(n, 2.0f) - 1.0f, 2.0f) * (6.0f + 3.0f * pn)) / (3.0f * N * Mathf.Pow(lambda.z, 4.0f) * (6.0f - 7.0f * pn)));
			return result;
		}
		
		static Vector3 totalMie(Vector3 lambda, Vector3 K, float T)
		{
			float c = (0.2f * T) * 10E-17f;
			Vector3 result = new Vector3(
				0.434f * c * Mathf.PI * Mathf.Pow((2.0f * Mathf.PI) / lambda.x, 2.0f) * K.x,
				0.434f * c * Mathf.PI * Mathf.Pow((2.0f * Mathf.PI) / lambda.y, 2.0f) * K.y,
				0.434f * c * Mathf.PI * Mathf.Pow((2.0f * Mathf.PI) / lambda.z, 2.0f) * K.z
				);
			return result;
		}

		// Peset handle function
		void RunPresets(GlobalFogSkyMaster SkyFog, SunShaftsSkyMaster SunShafts, float speed, bool Init){

//			if(SkyFog!=null){
//				if(ImageEffectFog){
//
//					float SpeedFactor = SkyManager.SPEED/200;
//
//					if(FogPreset == 0 ){
//						SkyFog.distanceFog = false;
//						SkyFog.useRadialDistance = false;
//						SkyFog.heightFog = true;
//						SkyFog.height = 246.69f;
//						//SkyFog.heightDensity = 0.0065f;
//						FogheightOffset = Mathf.Lerp(FogheightOffset,0 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
//						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
//						//lower at night
//						if(SkyManager != null){
//							if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
//								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.005f + FogdensityOffset,Time.deltaTime);//0.0065f
//								if(!Application.isPlaying){SkyFog.heightDensity = 0.005f + AddFogDensityOffset + FogUnityOffset;}
//							}else{
//								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0025f + FogdensityOffset,Time.deltaTime);//0.0015f
//								if(!Application.isPlaying){SkyFog.heightDensity = 0.0025f + AddFogDensityOffset + FogUnityOffset;}
//							}
//							if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){
//								if(ImageEffectShafts){
//									//apply moon to the shafts script at night and change color to white
//									if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
//									}else{
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
//									}
//									SunShafts.sunColor = Rays_day_color;
//									SunShafts.sunTransform = SkyManager.SunObj.transform;
//									SunShafts.useDepthTexture = false;
//									
//									SunShafts.radialBlurIterations = 2;
//									SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.86f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
//									SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,0.4f,0.4f*Time.deltaTime);
//									if(Glow_sun){
//										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Add;
//									}else{
//										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Screen;
//									}
//								}
//								
//							}else{
//								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
//								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
//								
//								if(ImageEffectShafts){
//									//apply moon to the shafts script at night and change color to white
//									if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
//									}else{
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
//									}
//									SunShafts.sunColor = Rays_night_color;
//									SunShafts.sunTransform = SkyManager.MoonObj.transform;
//									SunShafts.useDepthTexture = true;
//									
//									if(Mesh_moon){
//										SunShafts.radialBlurIterations = 3;
//										SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.58f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
//										SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,0.2f,0.4f*Time.deltaTime);
//									}else{
//										SunShafts.radialBlurIterations = 2;
//										SunShafts.sunShaftBlurRadius = 5.86f + ShaftBlurRadiusOffset;
//										SunShafts.maxRadius = 0.4f;
//									}
//									if(Glow_moon){
//										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Add;
//									}else{
//										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Screen;
//									}
//								}
//							}
//						}
//						//SkyFog.startDistance = 1;
//						if(!Application.isPlaying | Init){
//							SkyFog.startDistance = 1;
//						}else{
//							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,1,Time.deltaTime*speed);
//						}
//				//		SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,20900,Time.deltaTime*Trans_speed_sky*speed));//
//						if(!Application.isPlaying){
//							SkyFog.GradientBounds = new Vector2(0,15000);//20900
//						}else{
//							SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,15000,Time.deltaTime*Trans_speed_sky*speed));//
//						}
//
//						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance,3.3f,Time.deltaTime*Trans_speed_sky);//0.47
//						SkyFog.lumFac =0.24f;
//						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,34.16f,Time.deltaTime*Trans_speed_sky);//
//						SkyFog.TurbFac=51.6f;//3.7
//						SkyFog.HorizFac =0.4f;
//						SkyFog.turbidity =200;//14111
//						SkyFog.reileigh=10;//411
//						SkyFog.mieCoefficient=0.054f;
//						SkyFog.mieDirectionalG=0.913f;
//						SkyFog.bias =0.42f;
//						SkyFog.contrast=1.51f;
//						//SkyFog.Sun
//						SkyFog.FogSky = false;
//						SkyFog.TintColor = new Vector3(68,155,345);
//						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,5,Time.deltaTime*Trans_speed_sky); //set this so foggy sky in preset 1 matches the preset 0 color, then lerp to smooth out
//						
//					}
//					if(FogPreset == 1 ){ //winter fog
//						SkyFog.distanceFog = true;
//						SkyFog.useRadialDistance = true;
//						SkyFog.heightFog = true;
//						SkyFog.height = 446.69f;//
//						//SkyFog.heightDensity = 0.0065f;
//						FogheightOffset = Mathf.Lerp(FogheightOffset,0 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
//						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
//						//lower at night
//						if(SkyManager != null){
//							//if(SkyManager.Current_Time > 8 & SkyManager.Current_Time <=21.5f){
//							if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
//								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0585f + FogdensityOffset,Time.deltaTime);
//								if(!Application.isPlaying){SkyFog.heightDensity = 0.0585f + AddFogDensityOffset + FogUnityOffset;}
//							}else{
//								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f + FogdensityOffset,Time.deltaTime);
//								if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f + AddFogDensityOffset + FogUnityOffset;}
//							}
//							if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){	
//								if(ImageEffectShafts){
//									//apply moon to the shafts script at night and change color to white
//									if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
//									}else{
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
//									}
//									SunShafts.sunColor = Rays_day_color;
//									SunShafts.sunTransform = SkyManager.SunObj.transform;
//									SunShafts.useDepthTexture = false;
//								}
//								
//							}else{
//								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
//								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
//								
//								if(ImageEffectShafts){
//									//apply moon to the shafts script at night and change color to white
//									if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
//									}else{
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
//									}
//									SunShafts.sunColor = Rays_night_color;
//									SunShafts.sunTransform = SkyManager.MoonObj.transform;
//									SunShafts.useDepthTexture = true;
//								}
//							}
//						}
//						if(!Application.isPlaying | Init){
//							SkyFog.startDistance = 1;
//						}else{
//							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,1,Time.deltaTime*speed);
//						}
//						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,0,Time.deltaTime*Trans_speed_sky*speed));//
//						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance,2.94f,Time.deltaTime*Trans_speed_sky*speed);//
//						SkyFog.lumFac =0.24f;
//						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,273f,Time.deltaTime*Trans_speed_sky);//
//						SkyFog.TurbFac=3.7f;
//						SkyFog.HorizFac =0.4f;
//						SkyFog.turbidity =14111;
//						SkyFog.reileigh=411;
//						SkyFog.mieCoefficient=0.054f;
//						SkyFog.mieDirectionalG=0.913f;
//						SkyFog.bias =0.42f;
//						SkyFog.contrast=1.51f;
//						//SkyFog.Sun
//						SkyFog.FogSky = true;
//						SkyFog.TintColor = new Vector3(68,155,345);
//						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,1,Time.deltaTime*Trans_speed_sky); 
//					}
//					if(FogPreset == 2 ){
//						SkyFog.distanceFog = false;
//						SkyFog.useRadialDistance = false;
//						SkyFog.heightFog = true;
//						SkyFog.height = 2372.5f;//
//						//SkyFog.heightDensity = 0.0065f;
//						FogheightOffset = Mathf.Lerp(FogheightOffset,0 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
//						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
//						//lower at night
//						if(SkyManager != null){
//							if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
//								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0195f + FogdensityOffset,Time.deltaTime);
//								if(!Application.isPlaying){SkyFog.heightDensity = 0.0195f + AddFogDensityOffset + FogUnityOffset;}
//							}else{
//								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f + FogdensityOffset,Time.deltaTime);
//								if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f + AddFogDensityOffset + FogUnityOffset;}
//							}
//							if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){
//								if(ImageEffectShafts){
//									//apply moon to the shafts script at night and change color to white
//									if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
//									}else{
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
//									}
//									SunShafts.sunColor = Rays_day_color;
//									SunShafts.sunTransform = SkyManager.SunObj.transform;
//									SunShafts.useDepthTexture = false;
//									
//									SunShafts.radialBlurIterations = 2;
//									SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.86f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
//									SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,0.4f,0.4f*Time.deltaTime);
//									if(Glow_sun){
//										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Add;
//									}else{
//										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Screen;
//									}
//								}
//								
//							}else{
//								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
//								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
//								
//								if(ImageEffectShafts){
//									//apply moon to the shafts script at night and change color to white
//									if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
//									}else{
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
//									}
//									SunShafts.sunColor = Rays_night_color;
//									SunShafts.sunTransform = SkyManager.MoonObj.transform;
//									SunShafts.useDepthTexture = true;
//									
//									if(Mesh_moon){
//										SunShafts.radialBlurIterations = 3;
//										SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.58f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
//										SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,0.2f,0.4f*Time.deltaTime);
//									}else{
//										SunShafts.radialBlurIterations = 2;
//										SunShafts.sunShaftBlurRadius = 5.86f + ShaftBlurRadiusOffset;
//										SunShafts.maxRadius = 0.4f;
//									}
//									if(Glow_moon){
//										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Add;
//									}else{
//										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Screen;
//									}
//								}
//							}
//						}
//						if(!Application.isPlaying | Init){
//							SkyFog.startDistance = 1;
//						}else{
//							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,1,Time.deltaTime*speed);
//						}
//						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,20900,Time.deltaTime*Trans_speed_sky*speed));//
//						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance,2.64f,Time.deltaTime*Trans_speed_sky);//
//						SkyFog.lumFac =0.24f;
//						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,34.16f,Time.deltaTime*Trans_speed_sky);//
//						SkyFog.TurbFac=3.7f;
//						SkyFog.HorizFac =0.4f;
//						SkyFog.turbidity =14111;
//						SkyFog.reileigh=411;
//						SkyFog.mieCoefficient=0.054f;
//						SkyFog.mieDirectionalG=0.913f;
//						SkyFog.bias =0.42f;
//						SkyFog.contrast=1.51f;
//						//SkyFog.Sun
//						SkyFog.FogSky = false;
//						SkyFog.TintColor = new Vector3(68,155,345);
//						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,5,Time.deltaTime*Trans_speed_sky); //set this so foggy sky in preset 1 matches the preset 0 color, then lerp to smooth out
//						
//					}
//					//PRESET HAZE 3
//					if(FogPreset == 3 ){ //winter fog
//						SkyFog.distanceFog = true;
//						SkyFog.useRadialDistance = true;
//						SkyFog.heightFog = true;
//						SkyFog.height = Mathf.Lerp(SkyFog.height,550.69f,Time.deltaTime);//
//						//SkyFog.heightDensity = 0.0065f;
//						FogheightOffset = Mathf.Lerp(FogheightOffset,0 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
//						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
//						//lower at night
//						if(SkyManager != null){
//							//if(SkyManager.Current_Time > 8 & SkyManager.Current_Time <=21.5f){
//							if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
//								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.49f + FogdensityOffset,Time.deltaTime);//1.49f
//								if(!Application.isPlaying){SkyFog.heightDensity = 0.49f + AddFogDensityOffset + FogUnityOffset;}
//							}else{
//								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f + FogdensityOffset,Time.deltaTime);
//								if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f + AddFogDensityOffset + FogUnityOffset;}
//							}
//							if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){	
//								if(ImageEffectShafts){
//									//apply moon to the shafts script at night and change color to white
//									if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
//									}else{
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
//									}
//									SunShafts.sunColor = Rays_day_color;
//									SunShafts.sunTransform = SkyManager.SunObj.transform;
//									SunShafts.useDepthTexture = false;
//								}
//								
//							}else{
//								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
//								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
//								
//								if(ImageEffectShafts){
//									//apply moon to the shafts script at night and change color to white
//									if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
//									}else{
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
//									}
//									SunShafts.sunColor = Rays_night_color;
//									SunShafts.sunTransform = SkyManager.MoonObj.transform;
//									SunShafts.useDepthTexture = true;
//								}
//							}
//						}
//						if(!Application.isPlaying | Init){
//							SkyFog.startDistance = 1;
//						}else{
//							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,1,Time.deltaTime*speed);
//						}
//						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,500,Time.deltaTime*Trans_speed_sky*speed));//1500
//						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance,3f,Time.deltaTime*Trans_speed_sky*speed);//20
//						SkyFog.lumFac =0.24f;
//						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,34.16f,Time.deltaTime*Trans_speed_sky);//
//						SkyFog.TurbFac=3.7f;
//						SkyFog.HorizFac =0.4f;
//						SkyFog.turbidity =14111;
//						SkyFog.reileigh=3.21f;
//						SkyFog.mieCoefficient=0.15f;
//						SkyFog.mieDirectionalG=0.913f;
//						SkyFog.bias =0.42f;
//						SkyFog.contrast=1.51f;
//						//SkyFog.Sun
//						SkyFog.FogSky = true;
//						SkyFog.TintColor = new Vector3(68,155,345);
//						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,3.7638f,Time.deltaTime*Trans_speed_sky); 
//					}
//					//PRESET HAZE 4
//					if(FogPreset == 4 ){ //winter fog
//						SkyFog.distanceFog = true;
//						SkyFog.useRadialDistance = true;
//						SkyFog.heightFog = true;
//						SkyFog.height = Mathf.Lerp(SkyFog.height,917.69f,Time.deltaTime);//
//						//SkyFog.heightDensity = 0.0065f;
//						FogheightOffset = Mathf.Lerp(FogheightOffset,0 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
//						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
//						//lower at night
//						if(SkyManager != null){
//							//if(SkyManager.Current_Time > 8 & SkyManager.Current_Time <=21.5f){
//							if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
//								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0065f + FogdensityOffset,Time.deltaTime);//1.49f
//								if(!Application.isPlaying){SkyFog.heightDensity = 0.0065f + AddFogDensityOffset + FogUnityOffset;}
//							}else{
//								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f + FogdensityOffset,Time.deltaTime);
//								if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f + AddFogDensityOffset + FogUnityOffset;}
//							}
//							if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){	
//								if(ImageEffectShafts){
//									//apply moon to the shafts script at night and change color to white
//									if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
//									}else{
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
//									}
//									SunShafts.sunColor = Rays_day_color;
//									SunShafts.sunTransform = SkyManager.SunObj.transform;
//									SunShafts.useDepthTexture = false;
//								}
//								
//							}else{
//								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
//								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
//								
//								if(ImageEffectShafts){
//									//apply moon to the shafts script at night and change color to white
//									if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
//									}else{
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
//									}
//									SunShafts.sunColor = Rays_night_color;
//									SunShafts.sunTransform = SkyManager.MoonObj.transform;
//									SunShafts.useDepthTexture = true;
//								}
//							}
//						}
//						if(!Application.isPlaying | Init){
//							SkyFog.startDistance = 1;
//						}else{
//							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,1,Time.deltaTime*speed);
//						}
//						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,20900,Time.deltaTime*Trans_speed_sky*speed));//1500
//						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance,4.2f,Time.deltaTime*Trans_speed_sky*speed);//20
//						SkyFog.lumFac =0.24f;
//						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,34.16f,Time.deltaTime*Trans_speed_sky);//
//						SkyFog.TurbFac=3.7f;
//						SkyFog.HorizFac =0.4f;
//						SkyFog.turbidity = Mathf.Lerp(SkyFog.turbidity,111,Time.deltaTime);//
//						SkyFog.reileigh=411f;
//						SkyFog.mieCoefficient=0.054f;
//						SkyFog.mieDirectionalG=0.913f;
//						SkyFog.bias =0.42f;
//						SkyFog.contrast=1.05f;
//						//SkyFog.Sun
//						SkyFog.FogSky = true;
//						SkyFog.TintColor = new Vector3(68,155,345);
//						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,4.5638f,Time.deltaTime*Trans_speed_sky); 
//					}
//					if(FogPreset == 5 ){ //dual fog - volume and standard - v.2.0.1
//						SkyFog.distanceFog = true;
//						SkyFog.useRadialDistance = true;
//						SkyFog.heightFog = true;
//						SkyFog.height = 446.69f;//
//						//SkyFog.heightDensity = 0.0065f;
//						FogheightOffset = Mathf.Lerp(FogheightOffset,0 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
//						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
//						//lower at night
//						if(SkyManager != null){
//							//if(SkyManager.Current_Time > 8 & SkyManager.Current_Time <=21.5f){
//							if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
//								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0185f + FogdensityOffset,Time.deltaTime);
//								if(!Application.isPlaying){SkyFog.heightDensity = 0.0185f + AddFogDensityOffset + FogUnityOffset;}
//							}else{
//								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f + FogdensityOffset,Time.deltaTime*12);
//								if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f + AddFogDensityOffset + FogUnityOffset;}
//							}
//							if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){	
//								if(ImageEffectShafts){
//									//apply moon to the shafts script at night and change color to white
//									if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
//									}else{
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
//									}
//									SunShafts.sunColor = Rays_day_color;
//									SunShafts.sunTransform = SkyManager.SunObj.transform;
//									SunShafts.useDepthTexture = false;
//								}
//								
//								//SkyFog.Sun = SkyManager.SunObj.transform;
//								
//							}else{
//								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
//								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
//								
//								if(ImageEffectShafts){
//									//apply moon to the shafts script at night and change color to white
//									if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
//									}else{
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
//									}
//									SunShafts.sunColor = Rays_night_color;
//									SunShafts.sunTransform = SkyManager.MoonObj.transform;
//									SunShafts.useDepthTexture = true;
//								}
//								
//								//SkyFog.Sun = SkyManager.MoonObj.transform;
//							}
//						}
//						if(!Application.isPlaying | Init){
//							SkyFog.startDistance = 1;
//						}else{
//							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,1,Time.deltaTime*speed);
//						}
//						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,1,Time.deltaTime*Trans_speed_sky*speed));//
//						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance,2.7f,Time.deltaTime*Trans_speed_sky*speed);//
//						SkyFog.lumFac =0.24f;
//						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,273f,Time.deltaTime*Trans_speed_sky);//
//						SkyFog.TurbFac=3.7f;
//						SkyFog.HorizFac =0.4f;
//						SkyFog.turbidity =14111;
//						SkyFog.reileigh=411;
//						SkyFog.mieCoefficient=0.054f;
//						SkyFog.mieDirectionalG=0.913f;
//						SkyFog.bias =0.42f;
//						SkyFog.contrast=1.51f;
//						//SkyFog.Sun
//						SkyFog.FogSky = false;
//						SkyFog.TintColor = new Vector3(68,155,345);
//						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,1,Time.deltaTime*Trans_speed_sky); 
//					}
//
//					if(FogPreset == 6 ){//v3.0 preset - red sun
//						SkyFog.distanceFog = true;
//						SkyFog.useRadialDistance = true;
//						SkyFog.heightFog = true;
//						SkyFog.height = 274.06f;
//						//SkyFog.heightDensity = 0.0065f;
//						FogheightOffset = Mathf.Lerp(FogheightOffset,0 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
//						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
//						//lower at night
//						if(SkyManager != null){
//
//							if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
//								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,3.99f + FogdensityOffset,Time.deltaTime);
//								if(!Application.isPlaying){SkyFog.heightDensity = 3.99f + AddFogDensityOffset + FogUnityOffset;}
//							}else{
//								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,3.99f + FogdensityOffset,Time.deltaTime);
//								if(!Application.isPlaying){SkyFog.heightDensity = 3.99f + AddFogDensityOffset + FogUnityOffset;}
//							}
//
//							if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){
//								if(ImageEffectShafts){
//									//apply moon to the shafts script at night and change color to white
//									if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
//									}else{
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
//									}
//									SunShafts.sunColor = Rays_day_color;
//									SunShafts.sunTransform = SkyManager.SunObj.transform;
//									SunShafts.useDepthTexture = false;
//									
//									SunShafts.radialBlurIterations = 2;
//									SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.86f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
//									SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,0.4f,0.4f*Time.deltaTime);
//									if(Glow_sun){
//										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Add;
//									}else{
//										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Screen;
//									}
//								}
//								
//							}else{
//								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
//								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
//								
//								if(ImageEffectShafts){
//									//apply moon to the shafts script at night and change color to white
//									if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
//									}else{
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
//									}
//									SunShafts.sunColor = Rays_night_color;
//									SunShafts.sunTransform = SkyManager.MoonObj.transform;
//									SunShafts.useDepthTexture = true;
//									
//									if(Mesh_moon){
//										SunShafts.radialBlurIterations = 3;
//										SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.58f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
//										SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,0.2f,0.4f*Time.deltaTime);
//									}else{
//										SunShafts.radialBlurIterations = 2;
//										SunShafts.sunShaftBlurRadius = 5.86f + ShaftBlurRadiusOffset;
//										SunShafts.maxRadius = 0.4f;
//									}
//									if(Glow_moon){
//										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Add;
//									}else{
//										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Screen;
//									}
//								}
//							}
//						}
//						if(!Application.isPlaying | Init){
//							SkyFog.startDistance = 1;
//						}else{
//							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,1,Time.deltaTime*speed);
//						}
//						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,32900,Time.deltaTime*Trans_speed_sky*speed));//
//						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance,1.9f,Time.deltaTime*Trans_speed_sky);//
//						SkyFog.lumFac =0.31f;
//						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,34.16f,Time.deltaTime*Trans_speed_sky);//
//						SkyFog.TurbFac=0.06f;
//						SkyFog.HorizFac =68.41f;
//						SkyFog.turbidity =14111;
//						SkyFog.reileigh=474;
//						SkyFog.mieCoefficient=0.054f;
//						SkyFog.mieDirectionalG=0.913f;
//						SkyFog.bias =0.42f;
//						SkyFog.contrast=0.67f;
//						//SkyFog.Sun
//						SkyFog.FogSky = true;
//						SkyFog.TintColor = new Vector3(36,108,345);
//						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,2.95f,Time.deltaTime*Trans_speed_sky); //set this so foggy sky in preset 1 matches the preset 0 color, then lerp to smooth out
//						
//					}
//
//
//
//					if(FogPreset == 7 ){ //freezing winter fog v3.0
//						SkyFog.distanceFog = false;//SkyFog.distanceFog = true;
//						SkyFog.useRadialDistance = true;
//						SkyFog.heightFog = true;
//						SkyFog.height = 546.69f;//
//						//SkyFog.heightDensity = 0.0065f;
//							//FogHeightByTerrain = false; //v3.0
//						FogheightOffset = Mathf.Lerp(FogheightOffset,50 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
//						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
//						//lower at night
//						if(SkyManager != null){
//							//if(SkyManager.Current_Time > 8 & SkyManager.Current_Time <=21.5f){
//							if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
//								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0132f + FogdensityOffset,Time.deltaTime * SpeedFactor);//0.032
//								if(!Application.isPlaying){SkyFog.heightDensity = 0.0132f + AddFogDensityOffset + FogUnityOffset;}
//							}else{
//								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0125f + FogdensityOffset,Time.deltaTime * SpeedFactor);
//								if(!Application.isPlaying){SkyFog.heightDensity = 0.0125f + AddFogDensityOffset + FogUnityOffset;}
//							}
//							if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){	
//								if(ImageEffectShafts){
//									//apply moon to the shafts script at night and change color to white
//									if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime * SpeedFactor);
//									}else{
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime * SpeedFactor);
//									}
//									SunShafts.sunColor = Rays_day_color;
//									SunShafts.sunTransform = SkyManager.SunObj.transform;
//									SunShafts.useDepthTexture = false;
//								}
//								
//							}else{
//								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
//								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
//								
//								if(ImageEffectShafts){
//									//apply moon to the shafts script at night and change color to white
//									if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime * SpeedFactor);
//									}else{
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime * SpeedFactor);
//									}
//									SunShafts.sunColor = Rays_night_color;
//									SunShafts.sunTransform = SkyManager.MoonObj.transform;
//									SunShafts.useDepthTexture = true;
//								}
//							}
//						}
//						if(!Application.isPlaying | Init){
//							SkyFog.startDistance = 1;
//						}else{
//							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,1,Time.deltaTime*speed);
//						}
//						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,20900,Time.deltaTime*Trans_speed_sky*speed));//
//						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance,0.6f,Time.deltaTime*Trans_speed_sky*speed);//
//						SkyFog.lumFac =0.24f;
//						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,34.16f,Time.deltaTime*Trans_speed_sky);//
//						SkyFog.TurbFac=0.07f;
//						SkyFog.HorizFac =0.4f;
//						SkyFog.turbidity =14;
//						SkyFog.reileigh=411;
//						SkyFog.mieCoefficient=0.054f;
//						SkyFog.mieDirectionalG=0.913f;
//						SkyFog.bias =0.42f;
//						SkyFog.contrast=2.51f;
//						//SkyFog.Sun
//						SkyFog.FogSky = true;
//						SkyFog.TintColor = new Vector3(68,587.6f,345);
//						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,4.5f,Time.deltaTime*Trans_speed_sky); 
//					}
//
//
//					if(FogPreset == 9 | FogPreset == 10){//v3.0 - UNDERWATER
//						SkyFog.distanceFog = false;
//						SkyFog.useRadialDistance = false;
//						SkyFog.heightFog = true;
//						SkyFog.height = 27.69f;
//						//SkyFog.heightDensity = 0.0065f;
//						FogheightOffset = Mathf.Lerp(FogheightOffset,0 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
//						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
//						//lower at night
//						if(SkyManager != null){
//							if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
//								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.02f + FogdensityOffset,Time.deltaTime);//0.0065f //0.002
//								if(!Application.isPlaying){SkyFog.heightDensity = 0.02f + AddFogDensityOffset + FogUnityOffset;}
//							}else{
//								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.015f + FogdensityOffset,Time.deltaTime);//0.0015f //0.0015
//								if(!Application.isPlaying){SkyFog.heightDensity = 0.015f + AddFogDensityOffset + FogUnityOffset;}
//							}
//							if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){
//								if(ImageEffectShafts){
//									//apply moon to the shafts script at night and change color to white
//									if(FogPreset == 10){ 
//										if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
//											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
//										}else{
//											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity+AddShaftsIntensityUnder,0.4f*Time.deltaTime);
//										}
//
//										SunShafts.radialBlurIterations = 2;
//										SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.86f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
//										SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,0.4f + AddShaftsSizeUnder,0.4f*Time.deltaTime);
//										if(Glow_sun){
//											SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Add;
//										}else{
//											SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Screen;
//										}
//										SunShafts.useDepthTexture = false;
//									}else{
//										if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
//											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
//										}else{
//											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,1.1f+AddShaftsIntensityUnder,0.4f*Time.deltaTime);
//											SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.4f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
//											SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,1.52f + AddShaftsSizeUnder,0.4f*Time.deltaTime);
//										}
//
//										SunShafts.radialBlurIterations = 2;
//										//SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.86f,0.4f*Time.deltaTime);
//										//SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,0.4f,0.4f*Time.deltaTime);
//										//if(Glow_sun){
//										//	SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Screen;
//										//}else{
//											SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Screen;
//										//}
//										SunShafts.useDepthTexture = true;
//									}
//									SunShafts.sunColor = Rays_day_color;
//									SunShafts.sunTransform = SkyManager.SunObj.transform;
//
//									
//
//								}
//								
//							}else{
//								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
//								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
//								
//								if(ImageEffectShafts){
//									//apply moon to the shafts script at night and change color to white
//									if(FogPreset == 10){
//										if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
//											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
//										}else{
//											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity+AddShaftsIntensityUnder,0.4f*Time.deltaTime);
//										}
//									}else{
//										if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
//											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
//										}else{
//											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,1.1f+AddShaftsIntensityUnder,0.4f*Time.deltaTime);
//											SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.4f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
//											SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,1.52f + AddShaftsSizeUnder,0.4f*Time.deltaTime);
//										}
//									}
//									SunShafts.sunColor = Rays_night_color;
//									SunShafts.sunTransform = SkyManager.MoonObj.transform;
//									SunShafts.useDepthTexture = true;
//									
//									if(Mesh_moon){
//										SunShafts.radialBlurIterations = 3;
//										SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.58f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
//										SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,0.2f + AddShaftsSizeUnder,0.4f*Time.deltaTime);
//									}else{
//										SunShafts.radialBlurIterations = 2;
//										SunShafts.sunShaftBlurRadius = 5.86f + ShaftBlurRadiusOffset;
//										SunShafts.maxRadius = 0.4f + AddShaftsSizeUnder;
//									}
//									if(Glow_moon){
//										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Add;
//									}else{
//										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Screen;
//									}
//								}
//							}
//						}
//						if(!Application.isPlaying | Init){
//							SkyFog.startDistance = 1;
//						}else{
//							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,1,Time.deltaTime*speed);
//						}
//						//		SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,20900,Time.deltaTime*Trans_speed_sky*speed));//
//						if(!Application.isPlaying){
//							SkyFog.GradientBounds = new Vector2(0,5000);//20900
//						}else{
//							SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,5000,Time.deltaTime*Trans_speed_sky*speed));//
//						}
//						
//						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance,3.2f,Time.deltaTime*Trans_speed_sky);//0.47 //1.2
//						SkyFog.lumFac =0.97f;
//						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,34.16f,Time.deltaTime*Trans_speed_sky);//
//						SkyFog.TurbFac=3.7f;//3.7
//						SkyFog.HorizFac =0.4f;
//						SkyFog.turbidity =14111;//14111
//						SkyFog.reileigh=411;//411
//						SkyFog.mieCoefficient=0.054f;
//						SkyFog.mieDirectionalG=0.913f;
//						SkyFog.bias =0.42f;
//						SkyFog.contrast=3.8f;
//						//SkyFog.Sun
//						SkyFog.FogSky = true;
//						SkyFog.TintColor = new Vector3(88.88f,555,145);
//						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,1,Time.deltaTime*Trans_speed_sky); //set this so foggy sky in preset 1 matches the preset 0 color, then lerp to smooth out
//						
//					}
//
//					//PRESET HAZE 11
//					if(FogPreset == 11 ){ //v3.0 sky
//						SkyFog.distanceFog = true;
//						SkyFog.useRadialDistance = false;
//						SkyFog.heightFog = true;
//					//	SkyFog.height = Mathf.Lerp(SkyFog.height,505.69f,Time.deltaTime);//
//						//SkyFog.heightDensity = 0.0065f;
//						FogheightOffset = Mathf.Lerp(FogheightOffset,0 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
//						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
//						//lower at night
//						if(SkyManager != null){
//							//if(SkyManager.Current_Time > 8 & SkyManager.Current_Time <=21.5f){
//							if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
//								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.001f + FogdensityOffset,Time.deltaTime);//1.49f
//								SkyFog.height = Mathf.Lerp(SkyFog.height,1505.69f,Time.deltaTime*0.1f);//
//								if(!Application.isPlaying | Init){
//									SkyFog.heightDensity = 0.001f + AddFogDensityOffset + FogUnityOffset;
//									SkyFog.height = 1505.69f;//
//								}//0.012
//
//							}else{
//								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.001f + FogdensityOffset,Time.deltaTime);
//								SkyFog.height = Mathf.Lerp(SkyFog.height,15.69f,Time.deltaTime);//
//								if(!Application.isPlaying | Init){
//									SkyFog.heightDensity = 0.0010f + AddFogDensityOffset + FogUnityOffset;
//									SkyFog.height = 15.69f;//
//								}
//
//							}
//							if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){	
//								if(ImageEffectShafts){
//									//apply moon to the shafts script at night and change color to white
//									if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
//									}else{
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
//									}
//									SunShafts.sunColor = Rays_day_color;
//									SunShafts.sunTransform = SkyManager.SunObj.transform;
//									SunShafts.useDepthTexture = true;//false;
//								}
//								
//							}else{
//								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
//								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
//								
//								if(ImageEffectShafts){
//									//apply moon to the shafts script at night and change color to white
//									if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
//									}else{
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
//									}
//									SunShafts.sunColor = Rays_night_color;
//									SunShafts.sunTransform = SkyManager.MoonObj.transform;
//									SunShafts.useDepthTexture = true;
//								}
//							}
//						}
//						//SkyFog.startDistance = 200;
//						if(!Application.isPlaying | Init){
//							SkyFog.startDistance = 200;
//						}else{
//							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,200,Time.deltaTime*speed);
//						}
//
//						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,1,Time.deltaTime*Trans_speed_sky*speed));//1500
//						if(!Application.isPlaying | Init){
//							SkyFog.GradientBounds = new Vector2(0,1);//1500
//							SkyFog.luminance = 4.2f;
//							SkyFog.ScatterFac = 34.16f;
//							SkyFog.turbidity =  111f;
//							SkyFog.ClearSkyFac =4.8f;
//						}
//
//						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance,4.2f,Time.deltaTime*Trans_speed_sky*speed);//20
//						SkyFog.lumFac =0.14f;
//						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,34.16f,Time.deltaTime*Trans_speed_sky);//
//						SkyFog.TurbFac=3.7f;
//						SkyFog.HorizFac =0.4f;
//						SkyFog.turbidity = Mathf.Lerp(SkyFog.turbidity,111,Time.deltaTime);//
//						SkyFog.reileigh=411f;
//						SkyFog.mieCoefficient=0.054f;
//						SkyFog.mieDirectionalG=0.913f;
//						SkyFog.bias =0.42f;
//						SkyFog.contrast=2.05f;//1.05f
//						//SkyFog.Sun
//						SkyFog.FogSky = false; //SkyFog.FogSky = true;
//						SkyFog.TintColor = new Vector3(68,155,345);
//						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,4.8f,Time.deltaTime*Trans_speed_sky); 
//					}
//
//					//PRESET HAZE 12
//					if(FogPreset == 12 ){ //v3.0 sky
//						SkyFog.distanceFog = false;
//						SkyFog.useRadialDistance = false;
//						SkyFog.heightFog = true;
//						//	SkyFog.height = Mathf.Lerp(SkyFog.height,505.69f,Time.deltaTime);//
//						//SkyFog.heightDensity = 0.0065f;
//						FogheightOffset = Mathf.Lerp(FogheightOffset,0 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
//						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
//						//lower at night
//						if(SkyManager != null){
//							//if(SkyManager.Current_Time > 8 & SkyManager.Current_Time <=21.5f){
//							if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
//								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.031f + FogdensityOffset,Time.deltaTime);//1.49f
//								SkyFog.height = Mathf.Lerp(SkyFog.height,1505.69f,Time.deltaTime*0.1f);//
//								if(!Application.isPlaying | Init){
//									SkyFog.heightDensity = 0.031f + AddFogDensityOffset + FogUnityOffset;
//									SkyFog.height = 1505.69f;//
//								}//0.012
//								
//							}else{
//								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.001f + FogdensityOffset,Time.deltaTime);
//								SkyFog.height = Mathf.Lerp(SkyFog.height,15.69f,Time.deltaTime);//
//								if(!Application.isPlaying | Init){
//									SkyFog.heightDensity = 0.0010f + AddFogDensityOffset + FogUnityOffset;
//									SkyFog.height = 15.69f;//
//								}
//								
//							}
//							if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){	
//								if(ImageEffectShafts){
//									//apply moon to the shafts script at night and change color to white
//									if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
//									}else{
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
//									}
//									SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.4f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
//									SunShafts.sunColor = Rays_day_color;
//									SunShafts.sunTransform = SkyManager.SunObj.transform;
//									SunShafts.useDepthTexture = true;
//								}
//								
//							}else{
//								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
//								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
//								
//								if(ImageEffectShafts){
//									//apply moon to the shafts script at night and change color to white
//									if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
//									}else{
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Moon_Shafts_intensity,0.4f*Time.deltaTime);
//									}
//									SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.4f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
//									SunShafts.sunColor = Rays_night_color;
//									SunShafts.sunTransform = SkyManager.MoonObj.transform;
//									SunShafts.useDepthTexture = true;
//								}
//							}
//						}
//						//SkyFog.startDistance = 200;
//						if(!Application.isPlaying | Init){
//							SkyFog.startDistance = 200;
//						}else{
//							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,200,Time.deltaTime*speed);
//						}
//						
//						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,21111,Time.deltaTime*Trans_speed_sky*speed));//1500
//						if(!Application.isPlaying | Init){
//							SkyFog.GradientBounds = new Vector2(0,21111);//1500
//							SkyFog.luminance = 0.84f;
//							SkyFog.ScatterFac = 34.16f;
//							SkyFog.turbidity =  61.41f;
//							SkyFog.ClearSkyFac =4.8f;
//						}
//						
//						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance, 0.84f,Time.deltaTime*Trans_speed_sky*speed);//20
//						SkyFog.lumFac =0.16f;
//						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,34.16f,Time.deltaTime*Trans_speed_sky);//
//						SkyFog.TurbFac=5.4f;
//						SkyFog.HorizFac =0.4f;
//						SkyFog.turbidity = Mathf.Lerp(SkyFog.turbidity,61.41f,Time.deltaTime);//
//						SkyFog.reileigh=303.24f;
//						SkyFog.mieCoefficient=0.074f;
//						SkyFog.mieDirectionalG=0.88f;
//						SkyFog.bias =0.62f;
//						SkyFog.contrast=3.75f;//1.05f
//						//SkyFog.Sun
//						SkyFog.FogSky = true; //SkyFog.FogSky = true;
//						SkyFog.TintColor = new Vector3(35.8f,155,345);
//						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,10.91f,Time.deltaTime*Trans_speed_sky); 
//					}
//
//					//PRESET HAZE 13-4
//					if(FogPreset == 13 | FogPreset == 14 ){ //v3.0 foggy - rain
//						SkyFog.distanceFog = false;//true
//						SkyFog.useRadialDistance = true;
//						SkyFog.heightFog = true;
//						//	SkyFog.height = Mathf.Lerp(SkyFog.height,505.69f,Time.deltaTime);//
//						//SkyFog.heightDensity = 0.0065f;
//						FogheightOffset = Mathf.Lerp(FogheightOffset,30 + AddFogHeightOffset, Time.deltaTime * SpeedFactor * 20);//v3.0
//						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
//						//lower at night
//						if(SkyManager != null){
//							//if(SkyManager.Current_Time > 8 & SkyManager.Current_Time <=21.5f){
//							if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
//								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.05f + FogdensityOffset,Time.deltaTime);//1.49f
//								SkyFog.height = Mathf.Lerp(SkyFog.height,1505.69f,Time.deltaTime*0.1f);//
//								if(!Application.isPlaying | Init){
//									SkyFog.heightDensity = 0.05f + AddFogDensityOffset + FogUnityOffset;
//									SkyFog.height = 1505.69f;//
//								}//0.012
//								
//							}else{
//								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.05f + FogdensityOffset,Time.deltaTime);
//								SkyFog.height = Mathf.Lerp(SkyFog.height,15.69f,Time.deltaTime);//
//								if(!Application.isPlaying | Init){
//									SkyFog.heightDensity = 0.05f + AddFogDensityOffset + FogUnityOffset;
//									SkyFog.height = 15.69f;//
//								}
//								
//							}
//							if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){	
//								if(ImageEffectShafts){
//									//apply moon to the shafts script at night and change color to white
//									if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
//									}else{
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
//									}
//									SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,0,0.4f*Time.deltaTime);
//									SunShafts.sunColor = Rays_day_color;
//									SunShafts.sunTransform = SkyManager.SunObj.transform;
//									SunShafts.useDepthTexture = true;
//								}
//								
//							}else{
//								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
//								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
//								
//								if(ImageEffectShafts){
//									//apply moon to the shafts script at night and change color to white
//									if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
//									}else{
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
//									}
//									SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,0,0.4f*Time.deltaTime);
//									SunShafts.sunColor = Rays_night_color;
//									SunShafts.sunTransform = SkyManager.MoonObj.transform;
//									SunShafts.useDepthTexture = true;
//								}
//							}
//						}
//						//SkyFog.startDistance = 20;
//						if(!Application.isPlaying | Init){
//							SkyFog.startDistance = 20;
//						}else{
//							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,20,Time.deltaTime*speed);
//						}
//						
//						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,1500,Time.deltaTime*Trans_speed_sky*speed));//1500
//						if(!Application.isPlaying | Init){
//							SkyFog.GradientBounds = new Vector2(0,1500);//1500
//							SkyFog.luminance = 0.9f;
//							SkyFog.ScatterFac = 1f;
//							SkyFog.turbidity =  2f;
//							SkyFog.ClearSkyFac =2f;
//						}
//						
//						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance, 0.9f,Time.deltaTime*Trans_speed_sky*speed);//20
//						SkyFog.lumFac =0.7f;
//						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,1f,Time.deltaTime*Trans_speed_sky);//
//						SkyFog.TurbFac=0.02f;
//						SkyFog.HorizFac =0.4f;
//						SkyFog.turbidity = Mathf.Lerp(SkyFog.turbidity,2f,Time.deltaTime);//
//						SkyFog.reileigh=303.24f;
//						SkyFog.mieCoefficient=0.074f;
//						SkyFog.mieDirectionalG=0.88f;
//						SkyFog.bias =0.62f;
//						SkyFog.contrast=3.45f;//1.05f
//						//SkyFog.Sun
//						SkyFog.FogSky = true; //SkyFog.FogSky = true;
//						SkyFog.TintColor = new Vector3(81.8f,155,26);
//						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,2f,Time.deltaTime*Trans_speed_sky); 
//					}
//
//					//OVVERIDE FOG HEIGHT - v3.0
//					if(FogHeightByTerrain){
//						if(Mesh_Terrain){
//							SkyFog.height = this.transform.position.y + 25 + FogheightOffset;// + AddFogHeightOffset;
//						}
//						if(Terrain.activeTerrain != null){
//							SkyFog.height = this.transform.position.y + 25 + FogheightOffset;// + AddFogHeightOffset;
//						}
//					}
//
//
//					//v2.1 - Add gradient if one defined and is different than current
//					if(GradientHolders.Count > 0){
//						if(FogPreset < GradientHolders.Count){
//							if(GradientHolders[FogPreset] != null){
//								if(GradientHolders[FogPreset].DistGradient != SkyFog.DistGradient){
//									//v3.0 lerp gradients
//									if(Lerp_gradient){
//										//make sure they have same key number
//										int Key_diff = Mathf.Abs(SkyFog.DistGradient.colorKeys.Length -  GradientHolders[FogPreset].DistGradient.colorKeys.Length);
////										for(int i=0;i<Key_diff;i++){
////											SkyFog.DistGradient.SetKeys
////										}
////										if(Key_diff > 0){
////											if(SkyFog.DistGradient.colorKeys.Length < GradientHolders[FogPreset].DistGradient.colorKeys.Length){
////												SkyFog.DistGradient.colorKeys.
////											}
////										}
//										//Debug.Log("b="+Key_diff);
//										//Debug.Log("b1="+SkyFog.DistGradient.colorKeys.Length);
//										//Debug.Log("b2="+GradientHolders[FogPreset].DistGradient.colorKeys.Length);
//										//Key_diff = Mathf.Abs(SkyFog.DistGradient.colorKeys.Length -  GradientHolders[FogPreset].DistGradient.colorKeys.Length);
//										if(Key_diff == 0){
//											//Debug.Log(Key_diff);
//											GradientColorKey[] Keys = new GradientColorKey[8]; 
//											for(int i=0;i<GradientHolders[FogPreset].DistGradient.colorKeys.Length;i++){
//												Keys[i] = new GradientColorKey(Color.Lerp(SkyFog.DistGradient.colorKeys[i].color,GradientHolders[FogPreset].DistGradient.colorKeys[i].color,Time.deltaTime*1.4f),
//												                               Mathf.Lerp(SkyFog.DistGradient.colorKeys[i].time,GradientHolders[FogPreset].DistGradient.colorKeys[i].time,Time.deltaTime*0.4f));
//												                               //GradientHolders[FogPreset].DistGradient.colorKeys[i].time);
//												//SkyFog.DistGradient.colorKeys[i].color = Color.Lerp(SkyFog.DistGradient.colorKeys[i].color,GradientHolders[FogPreset].DistGradient.colorKeys[i].color,Time.deltaTime); 
//												//SkyFog.DistGradient.colorKeys[i].time = Mathf.Lerp(SkyFog.DistGradient.colorKeys[i].time,GradientHolders[FogPreset].DistGradient.colorKeys[i].time,Time.deltaTime);
//											}
//											SkyFog.DistGradient.SetKeys(Keys,SkyFog.DistGradient.alphaKeys);
//											//Debug.Log(Key_diff);
//										}else{
//											//SkyFog.DistGradient = GradientHolders[FogPreset].DistGradient; //this should only happen at game start, then volume fog gradient will have 8 keys
//											//Debug.Log(Key_diff);
//											//Debug.Log("A="+SkyFog.DistGradient.colorKeys.Length);
//											//Debug.Log("A1="+GradientHolders[FogPreset].DistGradient.colorKeys.Length);
//											SkyFog.DistGradient.SetKeys(GradientHolders[FogPreset].DistGradient.colorKeys,GradientHolders[FogPreset].DistGradient.alphaKeys);
//										}
//									}else{
//										//SkyFog.DistGradient = GradientHolders[FogPreset].DistGradient; //replaces gradient, if one is defined
//										//Debug.Log("in2");
//										SkyFog.DistGradient.SetKeys(GradientHolders[FogPreset].DistGradient.colorKeys,GradientHolders[FogPreset].DistGradient.alphaKeys);
//									}
//								}
//							}
//						}
//					}
//
//					// disable unity fog in winter and foggy case
//					// and change to appropriate preset
//					if(SkyManager != null){
//						if(!Use_both_fogs){//v2.0.1
//							if(SkyManager.Weather == SkyMasterManager.Weather_types.Foggy
//							   | SkyManager.Weather == SkyMasterManager.Weather_types.HeavyFog
//							   | SkyManager.Season == 4 ){
//								SkyManager.Use_fog = false;
//								RenderSettings.fog = false;
//								RenderSettings.fogDensity = 0.0005f;
//								FogPreset = 1;
//							}else{
//								//FogPreset = 0;
//								RenderSettings.fogDensity = 0.00005f;
//							}
//						}
//					}
//				}
//			}

			if(SkyFog!=null){
				if(ImageEffectFog){
					
					//v3.0 - new automatic TOD
					bool is_DayLight   = (SkyManager.AutoSunPosition && SkyManager.Rot_Sun_X > 0 ) | (!SkyManager.AutoSunPosition && SkyManager.Current_Time > ( 9.0f + SkyManager.Shift_dawn) & SkyManager.Current_Time <= (21.9f + SkyManager.Shift_dawn));
					bool is_DayLightA  = (SkyManager.AutoSunPosition && SkyManager.Rot_Sun_X > 0 ) | (!SkyManager.AutoSunPosition && SkyManager.Current_Time > ( 8.0f + SkyManager.Shift_dawn) & SkyManager.Current_Time <= (21.5f + SkyManager.Shift_dawn));
					bool is_after_216  = (SkyManager.AutoSunPosition && SkyManager.Rot_Sun_X < 5 ) | (!SkyManager.AutoSunPosition && SkyManager.Current_Time >  (21.6f + SkyManager.Shift_dawn));
					bool is_after_79   = (SkyManager.AutoSunPosition && SkyManager.Rot_Sun_X > 5 ) | (!SkyManager.AutoSunPosition && SkyManager.Current_Time >  (7.9f + SkyManager.Shift_dawn));
					
					float SpeedFactor = VolumeFogSpeed*(SkyManager.SPEED/200);//v3.3
					
					if(FogPreset == 0 ){
						SkyFog.distanceFog = false;
						SkyFog.useRadialDistance = false;
						SkyFog.heightFog = true;
						SkyFog.height = 246.69f;
						//SkyFog.heightDensity = 0.0065f;
						FogheightOffset = Mathf.Lerp(FogheightOffset,0 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
						//lower at night
						if(SkyManager != null){
							//if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
							if(is_DayLightA){
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.005f + FogdensityOffset,Time.deltaTime);//0.0065f
								if(!Application.isPlaying){SkyFog.heightDensity = 0.005f + AddFogDensityOffset + FogUnityOffset;}
							}else{
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0025f + FogdensityOffset,Time.deltaTime);//0.0015f
								if(!Application.isPlaying){SkyFog.heightDensity = 0.0025f + AddFogDensityOffset + FogUnityOffset;}
							}
							//if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){
							if(is_DayLight){
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
									if(is_after_216 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_day_color;
									SunShafts.sunTransform = SkyManager.SunObj.transform;
									SunShafts.useDepthTexture = false;
									
									SunShafts.radialBlurIterations = 2;
									SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.86f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
									SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,0.4f,0.4f*Time.deltaTime);
									if(Glow_sun){
										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Add;
									}else{
										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Screen;
									}
								}
								
							}else{
								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
								
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
									if(is_after_79 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Moon_Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_night_color;
									SunShafts.sunTransform = SkyManager.MoonObj.transform;
									SunShafts.useDepthTexture = true;
									
									if(Mesh_moon){
										SunShafts.radialBlurIterations = 3;
										SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.58f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
										SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,0.2f,0.4f*Time.deltaTime);
									}else{
										SunShafts.radialBlurIterations = 2;
										SunShafts.sunShaftBlurRadius = 5.86f + ShaftBlurRadiusOffset;
										SunShafts.maxRadius = 0.4f;
									}
									if(Glow_moon){
										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Add;
									}else{
										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Screen;
									}
								}
							}
						}
						if(!Application.isPlaying | Init){
							SkyFog.startDistance = 1;
						}else{
							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,1,Time.deltaTime*speed);
						}
						//		SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,20900,Time.deltaTime*Trans_speed_sky*speed));//
						if(!Application.isPlaying){
							SkyFog.GradientBounds = new Vector2(0,15000);//20900
						}else{
							SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,15000,Time.deltaTime*Trans_speed_sky*speed));//
						}
						
						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance,3.3f,Time.deltaTime*Trans_speed_sky);//0.47
						SkyFog.lumFac =0.24f;
						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,34.16f,Time.deltaTime*Trans_speed_sky);//
						SkyFog.TurbFac=51.6f;//3.7
						SkyFog.HorizFac =0.4f;
						SkyFog.turbidity =200;//14111
						SkyFog.reileigh=10;//411
						SkyFog.mieCoefficient=0.054f;
						SkyFog.mieDirectionalG=0.913f;
						SkyFog.bias =0.42f;
						SkyFog.contrast=1.51f;
						//SkyFog.Sun
						SkyFog.FogSky = false;
						SkyFog.TintColor = new Vector3(68,155,345);
						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,5,Time.deltaTime*Trans_speed_sky); //set this so foggy sky in preset 1 matches the preset 0 color, then lerp to smooth out
						
					}
					if(FogPreset == 1 ){ //winter fog
						SkyFog.distanceFog = true;
						SkyFog.useRadialDistance = true;
						SkyFog.heightFog = true;
						SkyFog.height = 446.69f;//
						//SkyFog.heightDensity = 0.0065f;
						FogheightOffset = Mathf.Lerp(FogheightOffset,0 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
						//lower at night
						if(SkyManager != null){
							//if(SkyManager.Current_Time > 8 & SkyManager.Current_Time <=21.5f){
							//if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
							if(is_DayLightA){
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0585f + FogdensityOffset,Time.deltaTime);
								if(!Application.isPlaying){SkyFog.heightDensity = 0.0585f + AddFogDensityOffset + FogUnityOffset;}
							}else{
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f + FogdensityOffset,Time.deltaTime);
								if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f + AddFogDensityOffset + FogUnityOffset;}
							}
							//if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){	
							if(is_DayLight){
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
									if(is_after_216 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_day_color;
									SunShafts.sunTransform = SkyManager.SunObj.transform;
									SunShafts.useDepthTexture = false;
								}
								
							}else{
								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
								
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
									if(is_after_79 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Moon_Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_night_color;
									SunShafts.sunTransform = SkyManager.MoonObj.transform;
									SunShafts.useDepthTexture = true;
								}
							}
						}
						if(!Application.isPlaying | Init){
							SkyFog.startDistance = 1;
						}else{
							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,1,Time.deltaTime*speed);
						}
						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,0,Time.deltaTime*Trans_speed_sky*speed));//
						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance,2.94f,Time.deltaTime*Trans_speed_sky*speed);//
						SkyFog.lumFac =0.24f;
						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,273f,Time.deltaTime*Trans_speed_sky);//
						SkyFog.TurbFac=3.7f;
						SkyFog.HorizFac =0.4f;
						SkyFog.turbidity =14111;
						SkyFog.reileigh=411;
						SkyFog.mieCoefficient=0.054f;
						SkyFog.mieDirectionalG=0.913f;
						SkyFog.bias =0.42f;
						SkyFog.contrast=1.51f;
						//SkyFog.Sun
						SkyFog.FogSky = true;
						SkyFog.TintColor = new Vector3(68,155,345);
						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,1,Time.deltaTime*Trans_speed_sky); 
					}
					if(FogPreset == 2 ){
						SkyFog.distanceFog = false;
						SkyFog.useRadialDistance = false;
						SkyFog.heightFog = true;
						SkyFog.height = 2372.5f;//
						//SkyFog.heightDensity = 0.0065f;
						FogheightOffset = Mathf.Lerp(FogheightOffset,0 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
						//lower at night
						if(SkyManager != null){
							//if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
							if(is_DayLightA){
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0195f + FogdensityOffset,Time.deltaTime);
								if(!Application.isPlaying){SkyFog.heightDensity = 0.0195f + AddFogDensityOffset + FogUnityOffset;}
							}else{
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f + FogdensityOffset,Time.deltaTime);
								if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f + AddFogDensityOffset + FogUnityOffset;}
							}
							//if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){
							if(is_DayLight){
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
									if(is_after_216 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_day_color;
									SunShafts.sunTransform = SkyManager.SunObj.transform;
									SunShafts.useDepthTexture = false;
									
									SunShafts.radialBlurIterations = 2;
									SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.86f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
									SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,0.4f,0.4f*Time.deltaTime);
									if(Glow_sun){
										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Add;
									}else{
										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Screen;
									}
								}
								
							}else{
								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
								
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
									if(is_after_79 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Moon_Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_night_color;
									SunShafts.sunTransform = SkyManager.MoonObj.transform;
									SunShafts.useDepthTexture = true;
									
									if(Mesh_moon){
										SunShafts.radialBlurIterations = 3;
										SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.58f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
										SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,0.2f,0.4f*Time.deltaTime);
									}else{
										SunShafts.radialBlurIterations = 2;
										SunShafts.sunShaftBlurRadius = 5.86f + ShaftBlurRadiusOffset;
										SunShafts.maxRadius = 0.4f;
									}
									if(Glow_moon){
										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Add;
									}else{
										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Screen;
									}
								}
							}
						}
						if(!Application.isPlaying | Init){
							SkyFog.startDistance = 1;
						}else{
							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,1,Time.deltaTime*speed);
						}
						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,20900,Time.deltaTime*Trans_speed_sky*speed));//
						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance,2.64f,Time.deltaTime*Trans_speed_sky);//
						SkyFog.lumFac =0.24f;
						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,34.16f,Time.deltaTime*Trans_speed_sky);//
						SkyFog.TurbFac=3.7f;
						SkyFog.HorizFac =0.4f;
						SkyFog.turbidity =14111;
						SkyFog.reileigh=411;
						SkyFog.mieCoefficient=0.054f;
						SkyFog.mieDirectionalG=0.913f;
						SkyFog.bias =0.42f;
						SkyFog.contrast=1.51f;
						//SkyFog.Sun
						SkyFog.FogSky = false;
						SkyFog.TintColor = new Vector3(68,155,345);
						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,5,Time.deltaTime*Trans_speed_sky); //set this so foggy sky in preset 1 matches the preset 0 color, then lerp to smooth out
						
					}
					//PRESET HAZE 3
					if(FogPreset == 3 ){ //winter fog
						SkyFog.distanceFog = true;
						SkyFog.useRadialDistance = true;
						SkyFog.heightFog = true;
						SkyFog.height = Mathf.Lerp(SkyFog.height,550.69f,Time.deltaTime);//
						//SkyFog.heightDensity = 0.0065f;
						FogheightOffset = Mathf.Lerp(FogheightOffset,0 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
						//lower at night
						if(SkyManager != null){
							//if(SkyManager.Current_Time > 8 & SkyManager.Current_Time <=21.5f){
							//if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
							if(is_DayLightA){
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.49f + FogdensityOffset,Time.deltaTime);//1.49f
								if(!Application.isPlaying){SkyFog.heightDensity = 0.49f + AddFogDensityOffset + FogUnityOffset;}
							}else{
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f + FogdensityOffset,Time.deltaTime);
								if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f + AddFogDensityOffset + FogUnityOffset;}
							}
							//if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){
							if(is_DayLight){
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
									if(is_after_216 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_day_color;
									SunShafts.sunTransform = SkyManager.SunObj.transform;
									SunShafts.useDepthTexture = false;
								}
								
							}else{
								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
								
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
									if(is_after_79 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Moon_Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_night_color;
									SunShafts.sunTransform = SkyManager.MoonObj.transform;
									SunShafts.useDepthTexture = true;
								}
							}
						}
						if(!Application.isPlaying | Init){
							SkyFog.startDistance = 1;
						}else{
							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,1,Time.deltaTime*speed);
						}
						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,500,Time.deltaTime*Trans_speed_sky*speed));//1500
						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance,3f,Time.deltaTime*Trans_speed_sky*speed);//20
						SkyFog.lumFac =0.24f;
						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,34.16f,Time.deltaTime*Trans_speed_sky);//
						SkyFog.TurbFac=3.7f;
						SkyFog.HorizFac =0.4f;
						SkyFog.turbidity =14111;
						SkyFog.reileigh=3.21f;
						SkyFog.mieCoefficient=0.15f;
						SkyFog.mieDirectionalG=0.913f;
						SkyFog.bias =0.42f;
						SkyFog.contrast=1.51f;
						//SkyFog.Sun
						SkyFog.FogSky = true;
						SkyFog.TintColor = new Vector3(68,155,345);
						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,3.7638f,Time.deltaTime*Trans_speed_sky); 
					}
					//PRESET HAZE 4
					if(FogPreset == 4 ){ //winter fog
						SkyFog.distanceFog = true;
						SkyFog.useRadialDistance = true;
						SkyFog.heightFog = true;
						SkyFog.height = Mathf.Lerp(SkyFog.height,917.69f,Time.deltaTime);//
						//SkyFog.heightDensity = 0.0065f;
						FogheightOffset = Mathf.Lerp(FogheightOffset,0 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
						//lower at night
						if(SkyManager != null){
							//if(SkyManager.Current_Time > 8 & SkyManager.Current_Time <=21.5f){
							//if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
							if(is_DayLightA){
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0065f + FogdensityOffset,Time.deltaTime);//1.49f
								if(!Application.isPlaying){SkyFog.heightDensity = 0.0065f + AddFogDensityOffset + FogUnityOffset;}
							}else{
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f + FogdensityOffset,Time.deltaTime);
								if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f + AddFogDensityOffset + FogUnityOffset;}
							}
							//if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){
							if(is_DayLight){
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
									if(is_after_216 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_day_color;
									SunShafts.sunTransform = SkyManager.SunObj.transform;
									SunShafts.useDepthTexture = false;
								}
								
							}else{
								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
								
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
									if(is_after_79 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Moon_Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_night_color;
									SunShafts.sunTransform = SkyManager.MoonObj.transform;
									SunShafts.useDepthTexture = true;
								}
							}
						}
						if(!Application.isPlaying | Init){
							SkyFog.startDistance = 1;
						}else{
							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,1,Time.deltaTime*speed);
						}
						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,20900,Time.deltaTime*Trans_speed_sky*speed));//1500
						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance,4.2f,Time.deltaTime*Trans_speed_sky*speed);//20
						SkyFog.lumFac =0.24f;
						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,34.16f,Time.deltaTime*Trans_speed_sky);//
						SkyFog.TurbFac=3.7f;
						SkyFog.HorizFac =0.4f;
						SkyFog.turbidity = Mathf.Lerp(SkyFog.turbidity,111,Time.deltaTime);//
						SkyFog.reileigh=411f;
						SkyFog.mieCoefficient=0.054f;
						SkyFog.mieDirectionalG=0.913f;
						SkyFog.bias =0.42f;
						SkyFog.contrast=1.05f;
						//SkyFog.Sun
						SkyFog.FogSky = true;
						SkyFog.TintColor = new Vector3(68,155,345);
						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,4.5638f,Time.deltaTime*Trans_speed_sky); 
					}
					if(FogPreset == 5 ){ //dual fog - volume and standard - v.2.0.1
						SkyFog.distanceFog = true;
						SkyFog.useRadialDistance = true;
						SkyFog.heightFog = true;
						SkyFog.height = 446.69f;//
						//SkyFog.heightDensity = 0.0065f;
						FogheightOffset = Mathf.Lerp(FogheightOffset,0 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
						//lower at night
						if(SkyManager != null){
							//if(SkyManager.Current_Time > 8 & SkyManager.Current_Time <=21.5f){
							//if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
							if(is_DayLightA){
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0185f + FogdensityOffset,Time.deltaTime);
								if(!Application.isPlaying){SkyFog.heightDensity = 0.0185f + AddFogDensityOffset + FogUnityOffset;}
							}else{
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f + FogdensityOffset,Time.deltaTime*12);
								if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f + AddFogDensityOffset + FogUnityOffset;}
							}
							//if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){	
							if(is_DayLight){
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
									if(is_after_216 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_day_color;
									SunShafts.sunTransform = SkyManager.SunObj.transform;
									SunShafts.useDepthTexture = false;
								}
								
								//SkyFog.Sun = SkyManager.SunObj.transform;
								
							}else{
								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
								
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
									if(is_after_79 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Moon_Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_night_color;
									SunShafts.sunTransform = SkyManager.MoonObj.transform;
									SunShafts.useDepthTexture = true;
								}
								
								//SkyFog.Sun = SkyManager.MoonObj.transform;
							}
						}
						if(!Application.isPlaying | Init){
							SkyFog.startDistance = 1;
						}else{
							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,1,Time.deltaTime*speed);
						}
						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,1,Time.deltaTime*Trans_speed_sky*speed));//
						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance,2.7f,Time.deltaTime*Trans_speed_sky*speed);//
						SkyFog.lumFac =0.24f;
						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,273f,Time.deltaTime*Trans_speed_sky);//
						SkyFog.TurbFac=3.7f;
						SkyFog.HorizFac =0.4f;
						SkyFog.turbidity =14111;
						SkyFog.reileigh=411;
						SkyFog.mieCoefficient=0.054f;
						SkyFog.mieDirectionalG=0.913f;
						SkyFog.bias =0.42f;
						SkyFog.contrast=1.51f;
						//SkyFog.Sun
						SkyFog.FogSky = false;
						SkyFog.TintColor = new Vector3(68,155,345);
						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,1,Time.deltaTime*Trans_speed_sky); 
					}
					
					if(FogPreset == 6 ){//v3.0 preset - red sun
						SkyFog.distanceFog = true;
						SkyFog.useRadialDistance = true;
						SkyFog.heightFog = true;
						SkyFog.height = 274.06f;
						//SkyFog.heightDensity = 0.0065f;
						FogheightOffset = Mathf.Lerp(FogheightOffset,0 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
						//lower at night
						if(SkyManager != null){
							
							//if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
							if(is_DayLightA){
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,3.99f + FogdensityOffset,Time.deltaTime);
								if(!Application.isPlaying){SkyFog.heightDensity = 3.99f + AddFogDensityOffset + FogUnityOffset;}
							}else{
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,3.99f + FogdensityOffset,Time.deltaTime);
								if(!Application.isPlaying){SkyFog.heightDensity = 3.99f + AddFogDensityOffset + FogUnityOffset;}
							}
							
							//if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){
							if(is_DayLight){
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
									if(is_after_216 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_day_color;
									SunShafts.sunTransform = SkyManager.SunObj.transform;
									SunShafts.useDepthTexture = false;
									
									SunShafts.radialBlurIterations = 2;
									SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.86f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
									SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,0.4f,0.4f*Time.deltaTime);
									if(Glow_sun){
										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Add;
									}else{
										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Screen;
									}
								}
								
							}else{
								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
								
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
									if(is_after_79 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Moon_Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_night_color;
									SunShafts.sunTransform = SkyManager.MoonObj.transform;
									SunShafts.useDepthTexture = true;
									
									if(Mesh_moon){
										SunShafts.radialBlurIterations = 3;
										SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.58f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
										SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,0.2f,0.4f*Time.deltaTime);
									}else{
										SunShafts.radialBlurIterations = 2;
										SunShafts.sunShaftBlurRadius = 5.86f + ShaftBlurRadiusOffset;
										SunShafts.maxRadius = 0.4f;
									}
									if(Glow_moon){
										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Add;
									}else{
										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Screen;
									}
								}
							}
						}
						if(!Application.isPlaying | Init){
							SkyFog.startDistance = 1;
						}else{
							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,1,Time.deltaTime*speed);
						}
						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,32900,Time.deltaTime*Trans_speed_sky*speed));//
						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance,1.9f,Time.deltaTime*Trans_speed_sky);//
						SkyFog.lumFac =0.31f;
						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,34.16f,Time.deltaTime*Trans_speed_sky);//
						SkyFog.TurbFac=0.06f;
						SkyFog.HorizFac =68.41f;
						SkyFog.turbidity =14111;
						SkyFog.reileigh=474;
						SkyFog.mieCoefficient=0.054f;
						SkyFog.mieDirectionalG=0.913f;
						SkyFog.bias =0.42f;
						SkyFog.contrast=0.67f;
						//SkyFog.Sun
						SkyFog.FogSky = true;
						SkyFog.TintColor = new Vector3(36,108,345);
						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,2.95f,Time.deltaTime*Trans_speed_sky); //set this so foggy sky in preset 1 matches the preset 0 color, then lerp to smooth out
						
					}
					
					
					
					if(FogPreset == 7 ){ //freezing winter fog v3.0
						SkyFog.distanceFog = false;//SkyFog.distanceFog = true;
						SkyFog.useRadialDistance = true;
						SkyFog.heightFog = true;
						SkyFog.height = 546.69f;//
						//SkyFog.heightDensity = 0.0065f;
						//FogHeightByTerrain = false; //v3.0
						FogheightOffset = Mathf.Lerp(FogheightOffset,50 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
						//lower at night
						if(SkyManager != null){
							//if(SkyManager.Current_Time > 8 & SkyManager.Current_Time <=21.5f){
							//if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
							if(is_DayLightA){
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0132f + FogdensityOffset,Time.deltaTime * SpeedFactor);//0.032
								if(!Application.isPlaying){SkyFog.heightDensity = 0.0132f + AddFogDensityOffset + FogUnityOffset;}
							}else{
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0125f + FogdensityOffset,Time.deltaTime * SpeedFactor);
								if(!Application.isPlaying){SkyFog.heightDensity = 0.0125f + AddFogDensityOffset + FogUnityOffset;}
							}
							//if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){	
							if(is_DayLight){
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
									if(is_after_216 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime * SpeedFactor);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime * SpeedFactor);
									}
									SunShafts.sunColor = Rays_day_color;
									SunShafts.sunTransform = SkyManager.SunObj.transform;
									SunShafts.useDepthTexture = false;
								}
								
							}else{
								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
								
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
									if(is_after_79 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime * SpeedFactor);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Moon_Shafts_intensity,0.4f*Time.deltaTime * SpeedFactor);
									}
									SunShafts.sunColor = Rays_night_color;
									SunShafts.sunTransform = SkyManager.MoonObj.transform;
									SunShafts.useDepthTexture = true;
								}
							}
						}
						if(!Application.isPlaying | Init){
							SkyFog.startDistance = 1;
						}else{
							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,1,Time.deltaTime*speed);
						}
						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,20900,Time.deltaTime*Trans_speed_sky*speed));//
						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance,0.6f,Time.deltaTime*Trans_speed_sky*speed);//
						SkyFog.lumFac =0.24f;
						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,34.16f,Time.deltaTime*Trans_speed_sky);//
						SkyFog.TurbFac=0.07f;
						SkyFog.HorizFac =0.4f;
						SkyFog.turbidity =14;
						SkyFog.reileigh=411;
						SkyFog.mieCoefficient=0.054f;
						SkyFog.mieDirectionalG=0.913f;
						SkyFog.bias =0.42f;
						SkyFog.contrast=2.51f;
						//SkyFog.Sun
						SkyFog.FogSky = true;
						SkyFog.TintColor = new Vector3(68,587.6f,345);
						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,4.5f,Time.deltaTime*Trans_speed_sky); 
					}
					
					
					if(FogPreset == 9 | FogPreset == 10){//v3.0 - UNDERWATER
						SkyFog.distanceFog = false;
						SkyFog.useRadialDistance = false;
						SkyFog.heightFog = true;
						SkyFog.height = 27.69f;
						//SkyFog.heightDensity = 0.0065f;
						FogheightOffset = Mathf.Lerp(FogheightOffset,0 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
						//lower at night
						if(SkyManager != null){
							//if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
							if(is_DayLightA){
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.02f + FogdensityOffset,Time.deltaTime);//0.0065f //0.002
								if(!Application.isPlaying){SkyFog.heightDensity = 0.02f + AddFogDensityOffset + FogUnityOffset;}
							}else{
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.015f + FogdensityOffset,Time.deltaTime);//0.0015f //0.0015
								if(!Application.isPlaying){SkyFog.heightDensity = 0.015f + AddFogDensityOffset + FogUnityOffset;}
							}
							//if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){
							if(is_DayLight){
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									if(FogPreset == 10){ 
										//	if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
										if(is_after_216 ){
											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
										}else{
											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity+AddShaftsIntensityUnder,0.4f*Time.deltaTime);
										}
										
										SunShafts.radialBlurIterations = 2;
										SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.86f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
										SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,0.4f + AddShaftsSizeUnder,0.4f*Time.deltaTime);
										if(Glow_sun){
											SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Add;
										}else{
											SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Screen;
										}
										SunShafts.useDepthTexture = false;
									}else{
										//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
										if(is_after_216 ){
											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
										}else{
											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,1.1f+AddShaftsIntensityUnder,0.4f*Time.deltaTime);
											SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.4f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
											SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,1.52f + AddShaftsSizeUnder,0.4f*Time.deltaTime);
										}
										
										SunShafts.radialBlurIterations = 2;
										//SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.86f,0.4f*Time.deltaTime);
										//SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,0.4f,0.4f*Time.deltaTime);
										//if(Glow_sun){
										//	SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Screen;
										//}else{
										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Screen;
										//}
										SunShafts.useDepthTexture = true;
									}
									SunShafts.sunColor = Rays_day_color;
									SunShafts.sunTransform = SkyManager.SunObj.transform;
									
									
									
								}
								
							}else{
								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
								
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									if(FogPreset == 10){
										//if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
										if(is_after_79 ){
											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
										}else{
											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Moon_Shafts_intensity+AddShaftsIntensityUnder,0.4f*Time.deltaTime);
										}
									}else{
										//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
										if(is_after_216 ){
											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
										}else{
											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,1.1f+AddShaftsIntensityUnder,0.4f*Time.deltaTime);
											SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.4f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
											SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,1.52f + AddShaftsSizeUnder,0.4f*Time.deltaTime);
										}
									}
									SunShafts.sunColor = Rays_night_color;
									SunShafts.sunTransform = SkyManager.MoonObj.transform;
									SunShafts.useDepthTexture = true;
									
									if(Mesh_moon){
										SunShafts.radialBlurIterations = 3;
										SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.58f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
										SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,0.2f + AddShaftsSizeUnder,0.4f*Time.deltaTime);
									}else{
										SunShafts.radialBlurIterations = 2;
										SunShafts.sunShaftBlurRadius = 5.86f + ShaftBlurRadiusOffset;
										SunShafts.maxRadius = 0.4f + AddShaftsSizeUnder;
									}
									if(Glow_moon){
										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Add;
									}else{
										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Screen;
									}
								}
							}
						}
						if(!Application.isPlaying | Init){
							SkyFog.startDistance = 1;
						}else{
							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,1,Time.deltaTime*speed);
						}
						//		SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,20900,Time.deltaTime*Trans_speed_sky*speed));//
						if(!Application.isPlaying){
							SkyFog.GradientBounds = new Vector2(0,5000);//20900
						}else{
							SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,5000,Time.deltaTime*Trans_speed_sky*speed));//
						}
						
						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance,3.2f,Time.deltaTime*Trans_speed_sky);//0.47 //1.2
						SkyFog.lumFac =0.97f;
						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,34.16f,Time.deltaTime*Trans_speed_sky);//
						SkyFog.TurbFac=3.7f;//3.7
						SkyFog.HorizFac =0.4f;
						SkyFog.turbidity =14111;//14111
						SkyFog.reileigh=411;//411
						SkyFog.mieCoefficient=0.054f;
						SkyFog.mieDirectionalG=0.913f;
						SkyFog.bias =0.42f;
						SkyFog.contrast=3.8f;
						//SkyFog.Sun
						SkyFog.FogSky = true;
						SkyFog.TintColor = new Vector3(88.88f,555,145);
						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,1,Time.deltaTime*Trans_speed_sky); //set this so foggy sky in preset 1 matches the preset 0 color, then lerp to smooth out
						
					}
					
					//PRESET HAZE 11
					if(FogPreset == 11 ){ //v3.0 sky
						SkyFog.distanceFog = true;
						SkyFog.useRadialDistance = false;
						SkyFog.heightFog = true;
						//	SkyFog.height = Mathf.Lerp(SkyFog.height,505.69f,Time.deltaTime);//
						//SkyFog.heightDensity = 0.0065f;
						FogheightOffset = Mathf.Lerp(FogheightOffset,0 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
						//lower at night
						if(SkyManager != null){
							//if(SkyManager.Current_Time > 8 & SkyManager.Current_Time <=21.5f){
							//if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
							if(is_DayLightA){
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.001f + FogdensityOffset,Time.deltaTime);//1.49f
								SkyFog.height = Mathf.Lerp(SkyFog.height,1505.69f,Time.deltaTime*0.1f);//
								if(!Application.isPlaying | Init){
									SkyFog.heightDensity = 0.001f + AddFogDensityOffset + FogUnityOffset;
									SkyFog.height = 1505.69f;//
								}//0.012
								
							}else{
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.001f + FogdensityOffset,Time.deltaTime);
								SkyFog.height = Mathf.Lerp(SkyFog.height,15.69f,Time.deltaTime);//
								if(!Application.isPlaying | Init){
									SkyFog.heightDensity = 0.0010f + AddFogDensityOffset + FogUnityOffset;
									SkyFog.height = 15.69f;//
								}
								
							}
							//if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){
							if(is_DayLight){
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
									if(is_after_216 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_day_color;
									SunShafts.sunTransform = SkyManager.SunObj.transform;
									SunShafts.useDepthTexture = true;//false;
								}
								
							}else{
								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
								
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
									if(is_after_79 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Moon_Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_night_color;
									SunShafts.sunTransform = SkyManager.MoonObj.transform;
									SunShafts.useDepthTexture = true;
								}
							}
						}
						//SkyFog.startDistance = 200;
						if(!Application.isPlaying | Init){
							SkyFog.startDistance = 200;
						}else{
							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,200,Time.deltaTime*speed);
						}
						
						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,1,Time.deltaTime*Trans_speed_sky*speed));//1500
						if(!Application.isPlaying | Init){
							SkyFog.GradientBounds = new Vector2(0,1);//1500
							SkyFog.luminance = 4.2f;
							SkyFog.ScatterFac = 34.16f;
							SkyFog.turbidity =  111f;
							SkyFog.ClearSkyFac =4.8f;
						}
						
						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance,4.2f,Time.deltaTime*Trans_speed_sky*speed);//20
						SkyFog.lumFac =0.14f;
						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,34.16f,Time.deltaTime*Trans_speed_sky);//
						SkyFog.TurbFac=3.7f;
						SkyFog.HorizFac =0.4f;
						SkyFog.turbidity = Mathf.Lerp(SkyFog.turbidity,111,Time.deltaTime);//
						SkyFog.reileigh=411f;
						SkyFog.mieCoefficient=0.054f;
						SkyFog.mieDirectionalG=0.913f;
						SkyFog.bias =0.42f;
						SkyFog.contrast=2.05f;//1.05f
						//SkyFog.Sun
						SkyFog.FogSky = false; //SkyFog.FogSky = true;
						SkyFog.TintColor = new Vector3(68,155,345);
						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,4.8f,Time.deltaTime*Trans_speed_sky); 
					}
					
					//PRESET HAZE 12
					if(FogPreset == 12 ){ //v3.0 sky
						SkyFog.distanceFog = false;
						SkyFog.useRadialDistance = false;
						SkyFog.heightFog = true;
						//	SkyFog.height = Mathf.Lerp(SkyFog.height,505.69f,Time.deltaTime);//
						//SkyFog.heightDensity = 0.0065f;
						FogheightOffset = Mathf.Lerp(FogheightOffset,0 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
						//lower at night
						if(SkyManager != null){
							//if(SkyManager.Current_Time > 8 & SkyManager.Current_Time <=21.5f){
							//if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
							if(is_DayLightA){
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.031f + FogdensityOffset,Time.deltaTime);//1.49f
								SkyFog.height = Mathf.Lerp(SkyFog.height,1505.69f,Time.deltaTime*0.1f);//
								if(!Application.isPlaying | Init){
									SkyFog.heightDensity = 0.031f + AddFogDensityOffset + FogUnityOffset;
									SkyFog.height = 1505.69f;//
								}//0.012
								
							}else{
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.001f + FogdensityOffset,Time.deltaTime);
								SkyFog.height = Mathf.Lerp(SkyFog.height,15.69f,Time.deltaTime);//
								if(!Application.isPlaying | Init){
									SkyFog.heightDensity = 0.0010f + AddFogDensityOffset + FogUnityOffset;
									SkyFog.height = 15.69f;//
								}
								
							}
							//if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){	
							if(is_DayLight){
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
									if(is_after_216 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.4f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
									SunShafts.sunColor = Rays_day_color;
									SunShafts.sunTransform = SkyManager.SunObj.transform;
									SunShafts.useDepthTexture = true;
								}
								
							}else{
								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
								
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
									if(is_after_79 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Moon_Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.4f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
									SunShafts.sunColor = Rays_night_color;
									SunShafts.sunTransform = SkyManager.MoonObj.transform;
									SunShafts.useDepthTexture = true;
								}
							}
						}
						//SkyFog.startDistance = 200;
						if(!Application.isPlaying | Init){
							SkyFog.startDistance = 200;
						}else{
							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,200,Time.deltaTime*speed);
						}
						
						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,21111,Time.deltaTime*Trans_speed_sky*speed));//1500
						if(!Application.isPlaying | Init){
							SkyFog.GradientBounds = new Vector2(0,21111);//1500
							SkyFog.luminance = 0.84f;
							SkyFog.ScatterFac = 34.16f;
							SkyFog.turbidity =  61.41f;
							SkyFog.ClearSkyFac =4.8f;
						}
						
						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance, 0.84f,Time.deltaTime*Trans_speed_sky*speed);//20
						SkyFog.lumFac =0.16f;
						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,34.16f,Time.deltaTime*Trans_speed_sky);//
						SkyFog.TurbFac=5.4f;
						SkyFog.HorizFac =0.4f;
						SkyFog.turbidity = Mathf.Lerp(SkyFog.turbidity,61.41f,Time.deltaTime);//
						SkyFog.reileigh=303.24f;
						SkyFog.mieCoefficient=0.074f;
						SkyFog.mieDirectionalG=0.88f;
						SkyFog.bias =0.62f;
						SkyFog.contrast=3.75f;//1.05f
						//SkyFog.Sun
						SkyFog.FogSky = true; //SkyFog.FogSky = true;
						SkyFog.TintColor = new Vector3(35.8f,155,345);
						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,10.91f,Time.deltaTime*Trans_speed_sky); 
					}
					
					//PRESET HAZE 13-4
					if(FogPreset == 13 | FogPreset == 14 ){ //v3.0 foggy - rain
						SkyFog.distanceFog = false;
						SkyFog.useRadialDistance = true;
						SkyFog.heightFog = true;
						//	SkyFog.height = Mathf.Lerp(SkyFog.height,505.69f,Time.deltaTime);//
						//SkyFog.heightDensity = 0.0065f;
						FogheightOffset = Mathf.Lerp(FogheightOffset,30 + AddFogHeightOffset, Time.deltaTime * SpeedFactor * 20);//v3.0
						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
						//lower at night
						if(SkyManager != null){
							//if(SkyManager.Current_Time > 8 & SkyManager.Current_Time <=21.5f){
							//if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
							if(is_DayLightA){
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.05f + FogdensityOffset,Time.deltaTime);//1.49f
								SkyFog.height = Mathf.Lerp(SkyFog.height,1505.69f,Time.deltaTime*0.1f);//
								if(!Application.isPlaying | Init){
									SkyFog.heightDensity = 0.05f + AddFogDensityOffset + FogUnityOffset;
									SkyFog.height = 1505.69f;//
								}//0.012
								
							}else{
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.05f + FogdensityOffset,Time.deltaTime);
								SkyFog.height = Mathf.Lerp(SkyFog.height,15.69f,Time.deltaTime);//
								if(!Application.isPlaying | Init){
									SkyFog.heightDensity = 0.05f + AddFogDensityOffset + FogUnityOffset;
									SkyFog.height = 15.69f;//
								}
								
							}
							//if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){	
							if(is_DayLight){
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
									if(is_after_216 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
									}
									SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,0,0.4f*Time.deltaTime);
									SunShafts.sunColor = Rays_day_color;
									SunShafts.sunTransform = SkyManager.SunObj.transform;
									SunShafts.useDepthTexture = true;
								}
								
							}else{
								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
								
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
									if(is_after_79 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
									}
									SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,0,0.4f*Time.deltaTime);
									SunShafts.sunColor = Rays_night_color;
									SunShafts.sunTransform = SkyManager.MoonObj.transform;
									SunShafts.useDepthTexture = true;
								}
							}
						}
						//SkyFog.startDistance = 20;
						if(!Application.isPlaying | Init){
							SkyFog.startDistance = 20;
						}else{
							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,20,Time.deltaTime*speed);
						}
						
						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,1500,Time.deltaTime*Trans_speed_sky*speed));//1500
						if(!Application.isPlaying | Init){
							SkyFog.GradientBounds = new Vector2(0,1500);//1500
							SkyFog.luminance = 0.9f;
							SkyFog.ScatterFac = 24f;
							SkyFog.turbidity =  61.41f;
							SkyFog.ClearSkyFac = 2f;
						}
						
						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance, 0.9f,Time.deltaTime*Trans_speed_sky*speed);//20
						SkyFog.lumFac =0.7f;
						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,24f,Time.deltaTime*Trans_speed_sky);//
						SkyFog.TurbFac=0.02f;
						SkyFog.HorizFac =0.4f;
						SkyFog.turbidity = Mathf.Lerp(SkyFog.turbidity,61.41f,Time.deltaTime);//
						SkyFog.reileigh=303.24f;
						SkyFog.mieCoefficient=0.074f;
						SkyFog.mieDirectionalG=0.88f;
						SkyFog.bias =0.62f;
						SkyFog.contrast=3.45f;//1.05f
						//SkyFog.Sun
						SkyFog.FogSky = true; //SkyFog.FogSky = true;
						SkyFog.TintColor = new Vector3(81.8f,155,26);
						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,2f,Time.deltaTime*Trans_speed_sky); 
					}

					//PRESET HAZE 15
					if(FogPreset == 15){ //v3.2 foggy - epic
						SkyFog.distanceFog = false;
						SkyFog.useRadialDistance = false;
						SkyFog.heightFog = true;
						//	SkyFog.height = Mathf.Lerp(SkyFog.height,505.69f,Time.deltaTime);//
						//SkyFog.heightDensity = 0.0065f;
						FogheightOffset = Mathf.Lerp(FogheightOffset,113 + AddFogHeightOffset, Time.deltaTime * SpeedFactor * 20);//v3.0
						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
						//lower at night
						if(SkyManager != null){
							//if(SkyManager.Current_Time > 8 & SkyManager.Current_Time <=21.5f){
							//if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
							if(is_DayLight){
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f + FogdensityOffset,Time.deltaTime);//1.49f
								SkyFog.height = Mathf.Lerp(SkyFog.height,155.69f,Time.deltaTime*0.1f);//
								SkyFog.luminance = Mathf.Lerp(SkyFog.luminance, 1.9f,Time.deltaTime*Trans_speed_sky*speed);
								SkyFog.bias = Mathf.Lerp(SkyFog.bias, 1.66f,Time.deltaTime*Trans_speed_sky*speed);//v3.3
								SkyFog.contrast = Mathf.Lerp(SkyFog.contrast, 6.52f,Time.deltaTime*Trans_speed_sky*speed);//v3.3
								SkyFog.mieCoefficient = Mathf.Lerp(SkyFog.mieCoefficient, 0.13f,Time.deltaTime*Trans_speed_sky*speed);//v3.3
								SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,180f,Time.deltaTime*Trans_speed_sky);//
								SkyFog.mieDirectionalG =Mathf.Lerp(SkyFog.mieDirectionalG,0.93f,Time.deltaTime*Trans_speed_sky);//
								SkyFog.TurbFac =Mathf.Lerp(SkyFog.TurbFac,320f,Time.deltaTime*Trans_speed_sky);//
								SkyFog.HorizFac =Mathf.Lerp(SkyFog.HorizFac,421f,Time.deltaTime*Trans_speed_sky);//
								SkyFog.lumFac =Mathf.Lerp(SkyFog.lumFac,0.21f,Time.deltaTime*Trans_speed_sky);//
								SkyFog.turbidity = Mathf.Lerp(SkyFog.turbidity,9.7f,Time.deltaTime*Trans_speed_sky);//
								SkyFog.reileigh = Mathf.Lerp(SkyFog.reileigh,0.8f,Time.deltaTime*Trans_speed_sky);//
								if(!Application.isPlaying | Init){
									SkyFog.heightDensity = 0.0015f + AddFogDensityOffset + FogUnityOffset;
									SkyFog.height = 155.69f;//
									SkyFog.luminance = 1.9f;
									SkyFog.bias =1.66f;//v3.3
									SkyFog.contrast=6.52f;//1.05f
									SkyFog.mieCoefficient=0.13f;
									SkyFog.ScatterFac = 180f;
									SkyFog.mieDirectionalG=0.93f;
									SkyFog.TurbFac=320f;
									SkyFog.HorizFac =421f;
									SkyFog.lumFac =0.21f;
									SkyFog.turbidity = 9.7f;
									SkyFog.reileigh=0.8f;
								}//0.012
								SkyFog.Sun = SkyManager.SunObj.transform;

							}else{
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0045f + FogdensityOffset,Time.deltaTime*Trans_speed_sky*speed);//v3.3 - 0.0015 to 0.0075
								SkyFog.height = Mathf.Lerp(SkyFog.height,155.69f,Time.deltaTime);//
								SkyFog.luminance = Mathf.Lerp(SkyFog.luminance, 3.04f,Time.deltaTime*Trans_speed_sky*speed*1);//v3.3 - 2.6 to 2.98
								SkyFog.bias = Mathf.Lerp(SkyFog.bias, 1.3f,Time.deltaTime*Trans_speed_sky*speed);//v3.3
								//SkyFog.contrast = Mathf.Lerp(SkyFog.contrast, 2.0f,Time.deltaTime*Trans_speed_sky*speed*1);//v3.3
								SkyFog.mieCoefficient = Mathf.Lerp(SkyFog.mieCoefficient, 0.3f,Time.deltaTime*Trans_speed_sky*speed);//v3.3
								SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,10f,Time.deltaTime*Trans_speed_sky*1);//
								SkyFog.mieDirectionalG =Mathf.Lerp(SkyFog.mieDirectionalG,0.7f,Time.deltaTime*Trans_speed_sky);//
								SkyFog.TurbFac =Mathf.Lerp(SkyFog.TurbFac,2f,Time.deltaTime*Trans_speed_sky*1);//
								SkyFog.HorizFac =Mathf.Lerp(SkyFog.HorizFac,10f,Time.deltaTime*Trans_speed_sky*1);//
								SkyFog.lumFac =Mathf.Lerp(SkyFog.lumFac,0.4f,Time.deltaTime*Trans_speed_sky);//
								SkyFog.turbidity = Mathf.Lerp(SkyFog.turbidity,0.0f,Time.deltaTime*Trans_speed_sky*speed*4);//
								SkyFog.reileigh = Mathf.Lerp(SkyFog.reileigh,0.01f,Time.deltaTime*Trans_speed_sky);//
								if(!Application.isPlaying | Init){
									SkyFog.heightDensity = 0.0045f + AddFogDensityOffset + FogUnityOffset;
									SkyFog.height = 155.69f;//
									SkyFog.luminance = 3.04f;
									SkyFog.bias =1.3f;//v3.3
									SkyFog.contrast=1.0f;//1.05f
									SkyFog.mieCoefficient=0.3f;
									SkyFog.ScatterFac = 10f;
									SkyFog.mieDirectionalG=0.7f;
									SkyFog.TurbFac=2;
									SkyFog.HorizFac =10;
									SkyFog.lumFac =0.4f;
									SkyFog.turbidity = 0.0f;
									SkyFog.reileigh=0.01f;
								}
								if (SkyManager.Rot_Sun_X < -11) {
									SkyFog.Sun = SkyManager.MoonObj.transform;
									SkyFog.contrast = Mathf.Lerp(SkyFog.contrast, 0.7f,Time.deltaTime*Trans_speed_sky*speed*0.2f);//v3.3
								} else {
									SkyFog.Sun = SkyManager.SunObj.transform;
									SkyFog.contrast = Mathf.Lerp(SkyFog.contrast, 2.5f,Time.deltaTime*Trans_speed_sky*speed*0.1f);//v3.3
								}

							}
							//if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){	

							if(is_DayLight){
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
									if(is_after_216 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.4f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
									SunShafts.sunColor = Rays_day_color;
									SunShafts.sunTransform = SkyManager.SunObj.transform;
									SunShafts.useDepthTexture = true;
								}

							}else{
								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}

								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
									if(is_after_79 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Moon_Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.4f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
									SunShafts.sunColor = Rays_night_color;
									SunShafts.sunTransform = SkyManager.MoonObj.transform;
									SunShafts.useDepthTexture = true;
								}
							}

//							if(is_DayLight){
//								if(ImageEffectShafts){
//									//apply moon to the shafts script at night and change color to white
//									//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
//									if(is_after_216 ){
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
//									}else{
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
//									}
//									SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,0,0.4f*Time.deltaTime);
//									SunShafts.sunColor = Rays_day_color;
//									SunShafts.sunTransform = SkyManager.SunObj.transform;
//									SunShafts.useDepthTexture = true;
//								}
//
//							}else{
//								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
//								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
//
//								if(ImageEffectShafts){
//									//apply moon to the shafts script at night and change color to white
//									//if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
//									if(is_after_79 ){
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
//									}else{
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
//									}
//									SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,0,0.4f*Time.deltaTime);
//									SunShafts.sunColor = Rays_night_color;
//									SunShafts.sunTransform = SkyManager.MoonObj.transform;
//									SunShafts.useDepthTexture = true;
//								}
//							}
						}
						//SkyFog.startDistance = 20;
						if(!Application.isPlaying | Init){
							SkyFog.startDistance = 20;
						}else{
							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,20,Time.deltaTime*speed);
						}

						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,2000,Time.deltaTime*Trans_speed_sky*speed));//1500
						if(!Application.isPlaying | Init){
							SkyFog.GradientBounds = new Vector2(0,1000);//1500
							//SkyFog.luminance = 2.6f;
						//	SkyFog.ScatterFac = 180f;
							SkyFog.turbidity =  9.7f;
							SkyFog.ClearSkyFac = 2f;
						}

						//SkyFog.luminance = Mathf.Lerp(SkyFog.luminance, 2.6f,Time.deltaTime*Trans_speed_sky*speed);//20
					//	SkyFog.lumFac =0.21f;
					//	SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,180f,Time.deltaTime*Trans_speed_sky);//v3.3
					//	SkyFog.TurbFac=320f;
					//	SkyFog.HorizFac =421f;
					//	SkyFog.turbidity = Mathf.Lerp(SkyFog.turbidity,9.7f,Time.deltaTime);//
					//	SkyFog.reileigh=0.8f;
					//	SkyFog.mieCoefficient=0.13f;//v3.3
					//	SkyFog.mieDirectionalG=0.93f;
					//	SkyFog.bias =1.66f;
					//	SkyFog.contrast=6.52f;//1.05f //v3.3
						//SkyFog.Sun
						SkyFog.FogSky = true; //SkyFog.FogSky = true;
						SkyFog.TintColor = new Vector3(65.4f,155,345);
						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,6.25f,Time.deltaTime*Trans_speed_sky); 
					}
					

					
					
					//v2.1 - Add gradient if one defined and is different than current
					if(GradientHolders.Count > 0){
						if(FogPreset < GradientHolders.Count){
							if(GradientHolders[FogPreset] != null){
								if(GradientHolders[FogPreset].DistGradient != SkyFog.DistGradient){
									//v3.0 lerp gradients
									if(Lerp_gradient){
										//make sure they have same key number
										int Key_diff = Mathf.Abs(SkyFog.DistGradient.colorKeys.Length -  GradientHolders[FogPreset].DistGradient.colorKeys.Length);
										//										for(int i=0;i<Key_diff;i++){
										//											SkyFog.DistGradient.SetKeys
										//										}
										//										if(Key_diff > 0){
										//											if(SkyFog.DistGradient.colorKeys.Length < GradientHolders[FogPreset].DistGradient.colorKeys.Length){
										//												SkyFog.DistGradient.colorKeys.
										//											}
										//										}
										//Debug.Log("b="+Key_diff);
										//Debug.Log("b1="+SkyFog.DistGradient.colorKeys.Length);
										//Debug.Log("b2="+GradientHolders[FogPreset].DistGradient.colorKeys.Length);
										//Key_diff = Mathf.Abs(SkyFog.DistGradient.colorKeys.Length -  GradientHolders[FogPreset].DistGradient.colorKeys.Length);
										if(Key_diff == 0){
											//Debug.Log(Key_diff);
											GradientColorKey[] Keys = new GradientColorKey[8]; 
											for(int i=0;i<GradientHolders[FogPreset].DistGradient.colorKeys.Length;i++){
												Keys[i] = new GradientColorKey(Color.Lerp(SkyFog.DistGradient.colorKeys[i].color,GradientHolders[FogPreset].DistGradient.colorKeys[i].color,Time.deltaTime*1.4f),
												                               Mathf.Lerp(SkyFog.DistGradient.colorKeys[i].time,GradientHolders[FogPreset].DistGradient.colorKeys[i].time,Time.deltaTime*0.4f));
												//GradientHolders[FogPreset].DistGradient.colorKeys[i].time);
												//SkyFog.DistGradient.colorKeys[i].color = Color.Lerp(SkyFog.DistGradient.colorKeys[i].color,GradientHolders[FogPreset].DistGradient.colorKeys[i].color,Time.deltaTime); 
												//SkyFog.DistGradient.colorKeys[i].time = Mathf.Lerp(SkyFog.DistGradient.colorKeys[i].time,GradientHolders[FogPreset].DistGradient.colorKeys[i].time,Time.deltaTime);
											}
											SkyFog.DistGradient.SetKeys(Keys,SkyFog.DistGradient.alphaKeys);
											//Debug.Log(Key_diff);
										}else{
											//SkyFog.DistGradient = GradientHolders[FogPreset].DistGradient; //this should only happen at game start, then volume fog gradient will have 8 keys
											//Debug.Log(Key_diff);
											//Debug.Log("A="+SkyFog.DistGradient.colorKeys.Length);
											//Debug.Log("A1="+GradientHolders[FogPreset].DistGradient.colorKeys.Length);
											SkyFog.DistGradient.SetKeys(GradientHolders[FogPreset].DistGradient.colorKeys,GradientHolders[FogPreset].DistGradient.alphaKeys);
										}
									}else{
										//SkyFog.DistGradient = GradientHolders[FogPreset].DistGradient; //replaces gradient, if one is defined
										//Debug.Log("in2");
										SkyFog.DistGradient.SetKeys(GradientHolders[FogPreset].DistGradient.colorKeys,GradientHolders[FogPreset].DistGradient.alphaKeys);
									}
								}
							}
						}
					}
					
					// disable unity fog in winter and foggy case
					// and change to appropriate preset
					if(SkyManager != null){
						if(!Use_both_fogs){//v2.0.1
							if(SkyManager.Weather == SkyMasterManager.Weather_types.Foggy
							   | SkyManager.Weather == SkyMasterManager.Weather_types.HeavyFog
							   | SkyManager.Season == 4 ){
								SkyManager.Use_fog = false;
								RenderSettings.fog = false;
								RenderSettings.fogDensity = 0.0005f;
								FogPreset = 1;
							}else{
								//FogPreset = 0;
								RenderSettings.fogDensity = 0.00005f;
							}
						}
					}

					//v3.3e
					if(UseFogCurves){
						float calcPoint = SkyManager.calcColorTime;
						//SkyFog.height = heightOffsetFogCurve.Evaluate(calcPoint);
				//		AddFogHeightOffset = heightOffsetFogCurve.Evaluate(calcPoint);
						FogheightOffset = heightOffsetFogCurve.Evaluate(calcPoint) + AddFogHeightOffset;
				//		AddFogHeightOffset = 0;

						if (!FogHeightByTerrain) {
							SkyFog.height = FogheightOffset;
						}

						SkyFog.FogSky = SkyFogOn;
						SkyFog.heightFog = HeightFogOn;
						SkyFog.distanceFog = DistanceFogOn;
						//if (DistanceFogOn) {
						SkyFog.startDistance = VFogDistance;//if DistanceFogOn
						//}
						//AddFogHeightOffset = 0;
						SkyFog.GradientBounds.y =  fogGradientDistance;
						SkyFog.heightDensity =  fogDensity*0.0001f;
						//FogheightOffset = heightOffsetFogCurve.Evaluate(calcPoint);
						SkyFog.luminance = luminanceVFogCurve.Evaluate(calcPoint);
						SkyFog.lumFac = lumFactorFogCurve.Evaluate (calcPoint);
						SkyFog.ScatterFac = scatterFacFogCurve.Evaluate (calcPoint);
						SkyFog.turbidity = turbidityFogCurve.Evaluate (calcPoint);
						SkyFog.TurbFac = turbFacFogCurve.Evaluate (calcPoint);
						SkyFog.HorizFac = horizonFogCurve.Evaluate (calcPoint);
						SkyFog.contrast = contrastFogCurve.Evaluate (calcPoint);
					}

					//OVVERIDE FOG HEIGHT - v3.0
					if(FogHeightByTerrain){
						if(Mesh_Terrain){
							SkyFog.height = this.transform.position.y + 25 + FogheightOffset;// + AddFogHeightOffset;
						}
						if(Terrain.activeTerrain != null){
							SkyFog.height = this.transform.position.y + 25 + FogheightOffset;// + AddFogHeightOffset;
						}
					}

				}//END check if imageffectfog
			}//END check if fog script is null
		
		}//END RunPresets

		// Peset handle function
		void RunPresetsT(GlobalTranspFogSkyMaster SkyFog, SunShaftsSkyMaster SunShafts, float speed, bool Init){
			
			if(SkyFog!=null){
				if(ImageEffectFog){

					//v3.0 - new automatic TOD
					bool is_DayLight   = (SkyManager.AutoSunPosition && SkyManager.Rot_Sun_X > 0 ) | (!SkyManager.AutoSunPosition && SkyManager.Current_Time > ( 9.0f + SkyManager.Shift_dawn) & SkyManager.Current_Time <= (21.9f + SkyManager.Shift_dawn));
					bool is_DayLightA  = (SkyManager.AutoSunPosition && SkyManager.Rot_Sun_X > 0 ) | (!SkyManager.AutoSunPosition && SkyManager.Current_Time > ( 8.0f + SkyManager.Shift_dawn) & SkyManager.Current_Time <= (21.5f + SkyManager.Shift_dawn));
					bool is_after_216  = (SkyManager.AutoSunPosition && SkyManager.Rot_Sun_X < 5 ) | (!SkyManager.AutoSunPosition && SkyManager.Current_Time >  (21.6f + SkyManager.Shift_dawn));
					bool is_after_79   = (SkyManager.AutoSunPosition && SkyManager.Rot_Sun_X > 5 ) | (!SkyManager.AutoSunPosition && SkyManager.Current_Time >  (7.9f + SkyManager.Shift_dawn));

					float SpeedFactor = VolumeFogSpeed*(SkyManager.SPEED/200);//v3.3
					
					if(FogPreset == 0 ){
						SkyFog.distanceFog = false;
						SkyFog.useRadialDistance = false;
						SkyFog.heightFog = true;
						SkyFog.height = 246.69f;
						//SkyFog.heightDensity = 0.0065f;
						FogheightOffset = Mathf.Lerp(FogheightOffset,0 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
						//lower at night
						if(SkyManager != null){
							//if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
							if(is_DayLightA){
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.005f + FogdensityOffset,Time.deltaTime);//0.0065f
								if(!Application.isPlaying){SkyFog.heightDensity = 0.005f + AddFogDensityOffset + FogUnityOffset;}
							}else{
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0025f + FogdensityOffset,Time.deltaTime);//0.0015f
								if(!Application.isPlaying){SkyFog.heightDensity = 0.0025f + AddFogDensityOffset + FogUnityOffset;}
							}
							//if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){
							if(is_DayLight){
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
									if(is_after_216 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_day_color;
									SunShafts.sunTransform = SkyManager.SunObj.transform;
									SunShafts.useDepthTexture = false;
									
									SunShafts.radialBlurIterations = 2;
									SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.86f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
									SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,0.4f,0.4f*Time.deltaTime);
									if(Glow_sun){
										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Add;
									}else{
										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Screen;
									}
								}
								
							}else{
								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
								
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
									if(is_after_79 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Moon_Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_night_color;
									SunShafts.sunTransform = SkyManager.MoonObj.transform;
									SunShafts.useDepthTexture = true;
									
									if(Mesh_moon){
										SunShafts.radialBlurIterations = 3;
										SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.58f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
										SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,0.2f,0.4f*Time.deltaTime);
									}else{
										SunShafts.radialBlurIterations = 2;
										SunShafts.sunShaftBlurRadius = 5.86f + ShaftBlurRadiusOffset;
										SunShafts.maxRadius = 0.4f;
									}
									if(Glow_moon){
										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Add;
									}else{
										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Screen;
									}
								}
							}
						}
						if(!Application.isPlaying | Init){
							SkyFog.startDistance = 1;
						}else{
							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,1,Time.deltaTime*speed);
						}
						//		SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,20900,Time.deltaTime*Trans_speed_sky*speed));//
						if(!Application.isPlaying){
							SkyFog.GradientBounds = new Vector2(0,15000);//20900
						}else{
							SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,15000,Time.deltaTime*Trans_speed_sky*speed));//
						}
						
						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance,3.3f,Time.deltaTime*Trans_speed_sky);//0.47
						SkyFog.lumFac =0.24f;
						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,34.16f,Time.deltaTime*Trans_speed_sky);//
						SkyFog.TurbFac=51.6f;//3.7
						SkyFog.HorizFac =0.4f;
						SkyFog.turbidity =200;//14111
						SkyFog.reileigh=10;//411
						SkyFog.mieCoefficient=0.054f;
						SkyFog.mieDirectionalG=0.913f;
						SkyFog.bias =0.42f;
						SkyFog.contrast=1.51f;
						//SkyFog.Sun
						SkyFog.FogSky = false;
						SkyFog.TintColor = new Vector3(68,155,345);
						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,5,Time.deltaTime*Trans_speed_sky); //set this so foggy sky in preset 1 matches the preset 0 color, then lerp to smooth out
						
					}
					if(FogPreset == 1 ){ //winter fog
						SkyFog.distanceFog = true;
						SkyFog.useRadialDistance = true;
						SkyFog.heightFog = true;
						SkyFog.height = 446.69f;//
						//SkyFog.heightDensity = 0.0065f;
						FogheightOffset = Mathf.Lerp(FogheightOffset,0 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
						//lower at night
						if(SkyManager != null){
							//if(SkyManager.Current_Time > 8 & SkyManager.Current_Time <=21.5f){
							//if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
							if(is_DayLightA){
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0585f + FogdensityOffset,Time.deltaTime);
								if(!Application.isPlaying){SkyFog.heightDensity = 0.0585f + AddFogDensityOffset + FogUnityOffset;}
							}else{
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f + FogdensityOffset,Time.deltaTime);
								if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f + AddFogDensityOffset + FogUnityOffset;}
							}
							//if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){	
							if(is_DayLight){
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
									if(is_after_216 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_day_color;
									SunShafts.sunTransform = SkyManager.SunObj.transform;
									SunShafts.useDepthTexture = false;
								}
								
							}else{
								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
								
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
									if(is_after_79 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Moon_Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_night_color;
									SunShafts.sunTransform = SkyManager.MoonObj.transform;
									SunShafts.useDepthTexture = true;
								}
							}
						}
						if(!Application.isPlaying | Init){
							SkyFog.startDistance = 1;
						}else{
							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,1,Time.deltaTime*speed);
						}
						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,0,Time.deltaTime*Trans_speed_sky*speed));//
						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance,2.94f,Time.deltaTime*Trans_speed_sky*speed);//
						SkyFog.lumFac =0.24f;
						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,273f,Time.deltaTime*Trans_speed_sky);//
						SkyFog.TurbFac=3.7f;
						SkyFog.HorizFac =0.4f;
						SkyFog.turbidity =14111;
						SkyFog.reileigh=411;
						SkyFog.mieCoefficient=0.054f;
						SkyFog.mieDirectionalG=0.913f;
						SkyFog.bias =0.42f;
						SkyFog.contrast=1.51f;
						//SkyFog.Sun
						SkyFog.FogSky = true;
						SkyFog.TintColor = new Vector3(68,155,345);
						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,1,Time.deltaTime*Trans_speed_sky); 
					}
					if(FogPreset == 2 ){
						SkyFog.distanceFog = false;
						SkyFog.useRadialDistance = false;
						SkyFog.heightFog = true;
						SkyFog.height = 2372.5f;//
						//SkyFog.heightDensity = 0.0065f;
						FogheightOffset = Mathf.Lerp(FogheightOffset,0 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
						//lower at night
						if(SkyManager != null){
							//if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
							if(is_DayLightA){
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0195f + FogdensityOffset,Time.deltaTime);
								if(!Application.isPlaying){SkyFog.heightDensity = 0.0195f + AddFogDensityOffset + FogUnityOffset;}
							}else{
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f + FogdensityOffset,Time.deltaTime);
								if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f + AddFogDensityOffset + FogUnityOffset;}
							}
							//if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){
							if(is_DayLight){
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
									if(is_after_216 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_day_color;
									SunShafts.sunTransform = SkyManager.SunObj.transform;
									SunShafts.useDepthTexture = false;
									
									SunShafts.radialBlurIterations = 2;
									SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.86f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
									SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,0.4f,0.4f*Time.deltaTime);
									if(Glow_sun){
										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Add;
									}else{
										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Screen;
									}
								}
								
							}else{
								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
								
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
									if(is_after_79 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Moon_Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_night_color;
									SunShafts.sunTransform = SkyManager.MoonObj.transform;
									SunShafts.useDepthTexture = true;
									
									if(Mesh_moon){
										SunShafts.radialBlurIterations = 3;
										SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.58f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
										SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,0.2f,0.4f*Time.deltaTime);
									}else{
										SunShafts.radialBlurIterations = 2;
										SunShafts.sunShaftBlurRadius = 5.86f + ShaftBlurRadiusOffset;
										SunShafts.maxRadius = 0.4f;
									}
									if(Glow_moon){
										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Add;
									}else{
										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Screen;
									}
								}
							}
						}
						if(!Application.isPlaying | Init){
							SkyFog.startDistance = 1;
						}else{
							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,1,Time.deltaTime*speed);
						}
						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,20900,Time.deltaTime*Trans_speed_sky*speed));//
						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance,2.64f,Time.deltaTime*Trans_speed_sky);//
						SkyFog.lumFac =0.24f;
						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,34.16f,Time.deltaTime*Trans_speed_sky);//
						SkyFog.TurbFac=3.7f;
						SkyFog.HorizFac =0.4f;
						SkyFog.turbidity =14111;
						SkyFog.reileigh=411;
						SkyFog.mieCoefficient=0.054f;
						SkyFog.mieDirectionalG=0.913f;
						SkyFog.bias =0.42f;
						SkyFog.contrast=1.51f;
						//SkyFog.Sun
						SkyFog.FogSky = false;
						SkyFog.TintColor = new Vector3(68,155,345);
						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,5,Time.deltaTime*Trans_speed_sky); //set this so foggy sky in preset 1 matches the preset 0 color, then lerp to smooth out
						
					}
					//PRESET HAZE 3
					if(FogPreset == 3 ){ //winter fog
						SkyFog.distanceFog = true;
						SkyFog.useRadialDistance = true;
						SkyFog.heightFog = true;
						SkyFog.height = Mathf.Lerp(SkyFog.height,550.69f,Time.deltaTime);//
						//SkyFog.heightDensity = 0.0065f;
						FogheightOffset = Mathf.Lerp(FogheightOffset,0 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
						//lower at night
						if(SkyManager != null){
							//if(SkyManager.Current_Time > 8 & SkyManager.Current_Time <=21.5f){
							//if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
							if(is_DayLightA){
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.49f + FogdensityOffset,Time.deltaTime);//1.49f
								if(!Application.isPlaying){SkyFog.heightDensity = 0.49f + AddFogDensityOffset + FogUnityOffset;}
							}else{
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f + FogdensityOffset,Time.deltaTime);
								if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f + AddFogDensityOffset + FogUnityOffset;}
							}
							//if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){
							if(is_DayLight){
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
									if(is_after_216 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_day_color;
									SunShafts.sunTransform = SkyManager.SunObj.transform;
									SunShafts.useDepthTexture = false;
								}
								
							}else{
								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
								
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
									if(is_after_79 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Moon_Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_night_color;
									SunShafts.sunTransform = SkyManager.MoonObj.transform;
									SunShafts.useDepthTexture = true;
								}
							}
						}
						if(!Application.isPlaying | Init){
							SkyFog.startDistance = 1;
						}else{
							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,1,Time.deltaTime*speed);
						}
						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,500,Time.deltaTime*Trans_speed_sky*speed));//1500
						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance,3f,Time.deltaTime*Trans_speed_sky*speed);//20
						SkyFog.lumFac =0.24f;
						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,34.16f,Time.deltaTime*Trans_speed_sky);//
						SkyFog.TurbFac=3.7f;
						SkyFog.HorizFac =0.4f;
						SkyFog.turbidity =14111;
						SkyFog.reileigh=3.21f;
						SkyFog.mieCoefficient=0.15f;
						SkyFog.mieDirectionalG=0.913f;
						SkyFog.bias =0.42f;
						SkyFog.contrast=1.51f;
						//SkyFog.Sun
						SkyFog.FogSky = true;
						SkyFog.TintColor = new Vector3(68,155,345);
						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,3.7638f,Time.deltaTime*Trans_speed_sky); 
					}
					//PRESET HAZE 4
					if(FogPreset == 4 ){ //winter fog
						SkyFog.distanceFog = true;
						SkyFog.useRadialDistance = true;
						SkyFog.heightFog = true;
						SkyFog.height = Mathf.Lerp(SkyFog.height,917.69f,Time.deltaTime);//
						//SkyFog.heightDensity = 0.0065f;
						FogheightOffset = Mathf.Lerp(FogheightOffset,0 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
						//lower at night
						if(SkyManager != null){
							//if(SkyManager.Current_Time > 8 & SkyManager.Current_Time <=21.5f){
							//if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
							if(is_DayLightA){
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0065f + FogdensityOffset,Time.deltaTime);//1.49f
								if(!Application.isPlaying){SkyFog.heightDensity = 0.0065f + AddFogDensityOffset + FogUnityOffset;}
							}else{
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f + FogdensityOffset,Time.deltaTime);
								if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f + AddFogDensityOffset + FogUnityOffset;}
							}
							//if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){
							if(is_DayLight){
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
									if(is_after_216 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_day_color;
									SunShafts.sunTransform = SkyManager.SunObj.transform;
									SunShafts.useDepthTexture = false;
								}
								
							}else{
								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
								
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
									if(is_after_79 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Moon_Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_night_color;
									SunShafts.sunTransform = SkyManager.MoonObj.transform;
									SunShafts.useDepthTexture = true;
								}
							}
						}
						if(!Application.isPlaying | Init){
							SkyFog.startDistance = 1;
						}else{
							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,1,Time.deltaTime*speed);
						}
						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,20900,Time.deltaTime*Trans_speed_sky*speed));//1500
						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance,4.2f,Time.deltaTime*Trans_speed_sky*speed);//20
						SkyFog.lumFac =0.24f;
						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,34.16f,Time.deltaTime*Trans_speed_sky);//
						SkyFog.TurbFac=3.7f;
						SkyFog.HorizFac =0.4f;
						SkyFog.turbidity = Mathf.Lerp(SkyFog.turbidity,111,Time.deltaTime);//
						SkyFog.reileigh=411f;
						SkyFog.mieCoefficient=0.054f;
						SkyFog.mieDirectionalG=0.913f;
						SkyFog.bias =0.42f;
						SkyFog.contrast=1.05f;
						//SkyFog.Sun
						SkyFog.FogSky = true;
						SkyFog.TintColor = new Vector3(68,155,345);
						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,4.5638f,Time.deltaTime*Trans_speed_sky); 
					}
					if(FogPreset == 5 ){ //dual fog - volume and standard - v.2.0.1
						SkyFog.distanceFog = true;
						SkyFog.useRadialDistance = true;
						SkyFog.heightFog = true;
						SkyFog.height = 446.69f;//
						//SkyFog.heightDensity = 0.0065f;
						FogheightOffset = Mathf.Lerp(FogheightOffset,0 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
						//lower at night
						if(SkyManager != null){
							//if(SkyManager.Current_Time > 8 & SkyManager.Current_Time <=21.5f){
							//if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
							if(is_DayLightA){
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0185f + FogdensityOffset,Time.deltaTime);
								if(!Application.isPlaying){SkyFog.heightDensity = 0.0185f + AddFogDensityOffset + FogUnityOffset;}
							}else{
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f + FogdensityOffset,Time.deltaTime*12);
								if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f + AddFogDensityOffset + FogUnityOffset;}
							}
							//if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){	
							if(is_DayLight){
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
									if(is_after_216 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_day_color;
									SunShafts.sunTransform = SkyManager.SunObj.transform;
									SunShafts.useDepthTexture = false;
								}
								
								//SkyFog.Sun = SkyManager.SunObj.transform;
								
							}else{
								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
								
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
									if(is_after_79 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Moon_Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_night_color;
									SunShafts.sunTransform = SkyManager.MoonObj.transform;
									SunShafts.useDepthTexture = true;
								}
								
								//SkyFog.Sun = SkyManager.MoonObj.transform;
							}
						}
						if(!Application.isPlaying | Init){
							SkyFog.startDistance = 1;
						}else{
							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,1,Time.deltaTime*speed);
						}
						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,1,Time.deltaTime*Trans_speed_sky*speed));//
						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance,2.7f,Time.deltaTime*Trans_speed_sky*speed);//
						SkyFog.lumFac =0.24f;
						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,273f,Time.deltaTime*Trans_speed_sky);//
						SkyFog.TurbFac=3.7f;
						SkyFog.HorizFac =0.4f;
						SkyFog.turbidity =14111;
						SkyFog.reileigh=411;
						SkyFog.mieCoefficient=0.054f;
						SkyFog.mieDirectionalG=0.913f;
						SkyFog.bias =0.42f;
						SkyFog.contrast=1.51f;
						//SkyFog.Sun
						SkyFog.FogSky = false;
						SkyFog.TintColor = new Vector3(68,155,345);
						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,1,Time.deltaTime*Trans_speed_sky); 
					}
					
					if(FogPreset == 6 ){//v3.0 preset - red sun
						SkyFog.distanceFog = true;
						SkyFog.useRadialDistance = true;
						SkyFog.heightFog = true;
						SkyFog.height = 274.06f;
						//SkyFog.heightDensity = 0.0065f;
						FogheightOffset = Mathf.Lerp(FogheightOffset,0 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
						//lower at night
						if(SkyManager != null){
							
							//if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
							if(is_DayLightA){
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,3.99f + FogdensityOffset,Time.deltaTime);
								if(!Application.isPlaying){SkyFog.heightDensity = 3.99f + AddFogDensityOffset + FogUnityOffset;}
							}else{
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,3.99f + FogdensityOffset,Time.deltaTime);
								if(!Application.isPlaying){SkyFog.heightDensity = 3.99f + AddFogDensityOffset + FogUnityOffset;}
							}
							
							//if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){
							if(is_DayLight){
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
									if(is_after_216 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_day_color;
									SunShafts.sunTransform = SkyManager.SunObj.transform;
									SunShafts.useDepthTexture = false;
									
									SunShafts.radialBlurIterations = 2;
									SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.86f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
									SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,0.4f,0.4f*Time.deltaTime);
									if(Glow_sun){
										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Add;
									}else{
										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Screen;
									}
								}
								
							}else{
								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
								
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
									if(is_after_79 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Moon_Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_night_color;
									SunShafts.sunTransform = SkyManager.MoonObj.transform;
									SunShafts.useDepthTexture = true;
									
									if(Mesh_moon){
										SunShafts.radialBlurIterations = 3;
										SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.58f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
										SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,0.2f,0.4f*Time.deltaTime);
									}else{
										SunShafts.radialBlurIterations = 2;
										SunShafts.sunShaftBlurRadius = 5.86f + ShaftBlurRadiusOffset;
										SunShafts.maxRadius = 0.4f;
									}
									if(Glow_moon){
										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Add;
									}else{
										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Screen;
									}
								}
							}
						}
						if(!Application.isPlaying | Init){
							SkyFog.startDistance = 1;
						}else{
							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,1,Time.deltaTime*speed);
						}
						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,32900,Time.deltaTime*Trans_speed_sky*speed));//
						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance,1.9f,Time.deltaTime*Trans_speed_sky);//
						SkyFog.lumFac =0.31f;
						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,34.16f,Time.deltaTime*Trans_speed_sky);//
						SkyFog.TurbFac=0.06f;
						SkyFog.HorizFac =68.41f;
						SkyFog.turbidity =14111;
						SkyFog.reileigh=474;
						SkyFog.mieCoefficient=0.054f;
						SkyFog.mieDirectionalG=0.913f;
						SkyFog.bias =0.42f;
						SkyFog.contrast=0.67f;
						//SkyFog.Sun
						SkyFog.FogSky = true;
						SkyFog.TintColor = new Vector3(36,108,345);
						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,2.95f,Time.deltaTime*Trans_speed_sky); //set this so foggy sky in preset 1 matches the preset 0 color, then lerp to smooth out
						
					}
					
					
					
					if(FogPreset == 7 ){ //freezing winter fog v3.0
						SkyFog.distanceFog = false;//SkyFog.distanceFog = true;
						SkyFog.useRadialDistance = true;
						SkyFog.heightFog = true;
						SkyFog.height = 546.69f;//
						//SkyFog.heightDensity = 0.0065f;
						//FogHeightByTerrain = false; //v3.0
						FogheightOffset = Mathf.Lerp(FogheightOffset,50 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
						//lower at night
						if(SkyManager != null){
							//if(SkyManager.Current_Time > 8 & SkyManager.Current_Time <=21.5f){
							//if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
							if(is_DayLightA){
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0132f + FogdensityOffset,Time.deltaTime * SpeedFactor);//0.032
								if(!Application.isPlaying){SkyFog.heightDensity = 0.0132f + AddFogDensityOffset + FogUnityOffset;}
							}else{
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0125f + FogdensityOffset,Time.deltaTime * SpeedFactor);
								if(!Application.isPlaying){SkyFog.heightDensity = 0.0125f + AddFogDensityOffset + FogUnityOffset;}
							}
							//if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){	
							if(is_DayLight){
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
									if(is_after_216 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime * SpeedFactor);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime * SpeedFactor);
									}
									SunShafts.sunColor = Rays_day_color;
									SunShafts.sunTransform = SkyManager.SunObj.transform;
									SunShafts.useDepthTexture = false;
								}
								
							}else{
								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
								
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
									if(is_after_79 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime * SpeedFactor);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Moon_Shafts_intensity,0.4f*Time.deltaTime * SpeedFactor);
									}
									SunShafts.sunColor = Rays_night_color;
									SunShafts.sunTransform = SkyManager.MoonObj.transform;
									SunShafts.useDepthTexture = true;
								}
							}
						}
						if(!Application.isPlaying | Init){
							SkyFog.startDistance = 1;
						}else{
							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,1,Time.deltaTime*speed);
						}
						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,20900,Time.deltaTime*Trans_speed_sky*speed));//
						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance,0.6f,Time.deltaTime*Trans_speed_sky*speed);//
						SkyFog.lumFac =0.24f;
						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,34.16f,Time.deltaTime*Trans_speed_sky);//
						SkyFog.TurbFac=0.07f;
						SkyFog.HorizFac =0.4f;
						SkyFog.turbidity =14;
						SkyFog.reileigh=411;
						SkyFog.mieCoefficient=0.054f;
						SkyFog.mieDirectionalG=0.913f;
						SkyFog.bias =0.42f;
						SkyFog.contrast=2.51f;
						//SkyFog.Sun
						SkyFog.FogSky = true;
						SkyFog.TintColor = new Vector3(68,587.6f,345);
						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,4.5f,Time.deltaTime*Trans_speed_sky); 
					}
					
					
					if(FogPreset == 9 | FogPreset == 10){//v3.0 - UNDERWATER
						SkyFog.distanceFog = false;
						SkyFog.useRadialDistance = false;
						SkyFog.heightFog = true;
						SkyFog.height = 27.69f;
						//SkyFog.heightDensity = 0.0065f;
						FogheightOffset = Mathf.Lerp(FogheightOffset,0 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
						//lower at night
						if(SkyManager != null){
							//if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
							if(is_DayLightA){
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.02f + FogdensityOffset,Time.deltaTime);//0.0065f //0.002
								if(!Application.isPlaying){SkyFog.heightDensity = 0.02f + AddFogDensityOffset + FogUnityOffset;}
							}else{
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.015f + FogdensityOffset,Time.deltaTime);//0.0015f //0.0015
								if(!Application.isPlaying){SkyFog.heightDensity = 0.015f + AddFogDensityOffset + FogUnityOffset;}
							}
							//if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){
							if(is_DayLight){
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									if(FogPreset == 10){ 
									//	if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
										if(is_after_216 ){
											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
										}else{
											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity+AddShaftsIntensityUnder,0.4f*Time.deltaTime);
										}
										
										SunShafts.radialBlurIterations = 2;
										SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.86f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
										SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,0.4f + AddShaftsSizeUnder,0.4f*Time.deltaTime);
										if(Glow_sun){
											SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Add;
										}else{
											SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Screen;
										}
										SunShafts.useDepthTexture = false;
									}else{
										//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
										if(is_after_216 ){
											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
										}else{
											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,1.1f+AddShaftsIntensityUnder,0.4f*Time.deltaTime);
											SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.4f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
											SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,1.52f + AddShaftsSizeUnder,0.4f*Time.deltaTime);
										}
										
										SunShafts.radialBlurIterations = 2;
										//SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.86f,0.4f*Time.deltaTime);
										//SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,0.4f,0.4f*Time.deltaTime);
										//if(Glow_sun){
										//	SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Screen;
										//}else{
										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Screen;
										//}
										SunShafts.useDepthTexture = true;
									}
									SunShafts.sunColor = Rays_day_color;
									SunShafts.sunTransform = SkyManager.SunObj.transform;
									
									
									
								}
								
							}else{
								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
								
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									if(FogPreset == 10){
										//if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
										if(is_after_79 ){
											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
										}else{
											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Moon_Shafts_intensity+AddShaftsIntensityUnder,0.4f*Time.deltaTime);
										}
									}else{
										//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
										if(is_after_216 ){
											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
										}else{
											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,1.1f+AddShaftsIntensityUnder,0.4f*Time.deltaTime);
											SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.4f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
											SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,1.52f + AddShaftsSizeUnder,0.4f*Time.deltaTime);
										}
									}
									SunShafts.sunColor = Rays_night_color;
									SunShafts.sunTransform = SkyManager.MoonObj.transform;
									SunShafts.useDepthTexture = true;
									
									if(Mesh_moon){
										SunShafts.radialBlurIterations = 3;
										SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.58f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
										SunShafts.maxRadius = Mathf.Lerp(SunShafts.maxRadius,0.2f + AddShaftsSizeUnder,0.4f*Time.deltaTime);
									}else{
										SunShafts.radialBlurIterations = 2;
										SunShafts.sunShaftBlurRadius = 5.86f + ShaftBlurRadiusOffset;
										SunShafts.maxRadius = 0.4f + AddShaftsSizeUnder;
									}
									if(Glow_moon){
										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Add;
									}else{
										SunShafts.screenBlendMode = SunShaftsSkyMaster.ShaftsScreenBlendMode.Screen;
									}
								}
							}
						}
						if(!Application.isPlaying | Init){
							SkyFog.startDistance = 1;
						}else{
							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,1,Time.deltaTime*speed);
						}
						//		SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,20900,Time.deltaTime*Trans_speed_sky*speed));//
						if(!Application.isPlaying){
							SkyFog.GradientBounds = new Vector2(0,5000);//20900
						}else{
							SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,5000,Time.deltaTime*Trans_speed_sky*speed));//
						}
						
						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance,3.2f,Time.deltaTime*Trans_speed_sky);//0.47 //1.2
						SkyFog.lumFac =0.97f;
						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,34.16f,Time.deltaTime*Trans_speed_sky);//
						SkyFog.TurbFac=3.7f;//3.7
						SkyFog.HorizFac =0.4f;
						SkyFog.turbidity =14111;//14111
						SkyFog.reileigh=411;//411
						SkyFog.mieCoefficient=0.054f;
						SkyFog.mieDirectionalG=0.913f;
						SkyFog.bias =0.42f;
						SkyFog.contrast=3.8f;
						//SkyFog.Sun
						SkyFog.FogSky = true;
						SkyFog.TintColor = new Vector3(88.88f,555,145);
						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,1,Time.deltaTime*Trans_speed_sky); //set this so foggy sky in preset 1 matches the preset 0 color, then lerp to smooth out
						
					}
					
					//PRESET HAZE 11
					if(FogPreset == 11 ){ //v3.0 sky
						SkyFog.distanceFog = true;
						SkyFog.useRadialDistance = false;
						SkyFog.heightFog = true;
						//	SkyFog.height = Mathf.Lerp(SkyFog.height,505.69f,Time.deltaTime);//
						//SkyFog.heightDensity = 0.0065f;
						FogheightOffset = Mathf.Lerp(FogheightOffset,0 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
						//lower at night
						if(SkyManager != null){
							//if(SkyManager.Current_Time > 8 & SkyManager.Current_Time <=21.5f){
							//if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
							if(is_DayLightA){
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.001f + FogdensityOffset,Time.deltaTime);//1.49f
								SkyFog.height = Mathf.Lerp(SkyFog.height,1505.69f,Time.deltaTime*0.1f);//
								if(!Application.isPlaying | Init){
									SkyFog.heightDensity = 0.001f + AddFogDensityOffset + FogUnityOffset;
									SkyFog.height = 1505.69f;//
								}//0.012
								
							}else{
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.001f + FogdensityOffset,Time.deltaTime);
								SkyFog.height = Mathf.Lerp(SkyFog.height,15.69f,Time.deltaTime);//
								if(!Application.isPlaying | Init){
									SkyFog.heightDensity = 0.0010f + AddFogDensityOffset + FogUnityOffset;
									SkyFog.height = 15.69f;//
								}
								
							}
							//if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){
							if(is_DayLight){
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
									if(is_after_216 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_day_color;
									SunShafts.sunTransform = SkyManager.SunObj.transform;
									SunShafts.useDepthTexture = true;//false;
								}
								
							}else{
								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
								
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
									if(is_after_79 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Moon_Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunColor = Rays_night_color;
									SunShafts.sunTransform = SkyManager.MoonObj.transform;
									SunShafts.useDepthTexture = true;
								}
							}
						}
						//SkyFog.startDistance = 200;
						if(!Application.isPlaying | Init){
							SkyFog.startDistance = 200;
						}else{
							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,200,Time.deltaTime*speed);
						}
						
						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,1,Time.deltaTime*Trans_speed_sky*speed));//1500
						if(!Application.isPlaying | Init){
							SkyFog.GradientBounds = new Vector2(0,1);//1500
							SkyFog.luminance = 4.2f;
							SkyFog.ScatterFac = 34.16f;
							SkyFog.turbidity =  111f;
							SkyFog.ClearSkyFac =4.8f;
						}
						
						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance,4.2f,Time.deltaTime*Trans_speed_sky*speed);//20
						SkyFog.lumFac =0.14f;
						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,34.16f,Time.deltaTime*Trans_speed_sky);//
						SkyFog.TurbFac=3.7f;
						SkyFog.HorizFac =0.4f;
						SkyFog.turbidity = Mathf.Lerp(SkyFog.turbidity,111,Time.deltaTime);//
						SkyFog.reileigh=411f;
						SkyFog.mieCoefficient=0.054f;
						SkyFog.mieDirectionalG=0.913f;
						SkyFog.bias =0.42f;
						SkyFog.contrast=2.05f;//1.05f
						//SkyFog.Sun
						SkyFog.FogSky = false; //SkyFog.FogSky = true;
						SkyFog.TintColor = new Vector3(68,155,345);
						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,4.8f,Time.deltaTime*Trans_speed_sky); 
					}
					
					//PRESET HAZE 12
					if(FogPreset == 12 ){ //v3.0 sky
						SkyFog.distanceFog = false;
						SkyFog.useRadialDistance = false;
						SkyFog.heightFog = true;
						//	SkyFog.height = Mathf.Lerp(SkyFog.height,505.69f,Time.deltaTime);//
						//SkyFog.heightDensity = 0.0065f;
						FogheightOffset = Mathf.Lerp(FogheightOffset,0 + AddFogHeightOffset, Time.deltaTime * SpeedFactor);//v3.0
						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
						//lower at night
						if(SkyManager != null){
							//if(SkyManager.Current_Time > 8 & SkyManager.Current_Time <=21.5f){
							//if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
							if(is_DayLightA){
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.031f + FogdensityOffset,Time.deltaTime);//1.49f
								SkyFog.height = Mathf.Lerp(SkyFog.height,1505.69f,Time.deltaTime*0.1f);//
								if(!Application.isPlaying | Init){
									SkyFog.heightDensity = 0.031f + AddFogDensityOffset + FogUnityOffset;
									SkyFog.height = 1505.69f;//
								}//0.012
								
							}else{
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.001f + FogdensityOffset,Time.deltaTime);
								SkyFog.height = Mathf.Lerp(SkyFog.height,15.69f,Time.deltaTime);//
								if(!Application.isPlaying | Init){
									SkyFog.heightDensity = 0.0010f + AddFogDensityOffset + FogUnityOffset;
									SkyFog.height = 15.69f;//
								}
								
							}
							//if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){	
							if(is_DayLight){
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
									if(is_after_216 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.4f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
									SunShafts.sunColor = Rays_day_color;
									SunShafts.sunTransform = SkyManager.SunObj.transform;
									SunShafts.useDepthTexture = true;
								}
								
							}else{
								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
								
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
									if(is_after_79 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Moon_Shafts_intensity,0.4f*Time.deltaTime);
									}
									SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.4f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
									SunShafts.sunColor = Rays_night_color;
									SunShafts.sunTransform = SkyManager.MoonObj.transform;
									SunShafts.useDepthTexture = true;
								}
							}
						}
						//SkyFog.startDistance = 200;
						if(!Application.isPlaying | Init){
							SkyFog.startDistance = 200;
						}else{
							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,200,Time.deltaTime*speed);
						}
						
						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,21111,Time.deltaTime*Trans_speed_sky*speed));//1500
						if(!Application.isPlaying | Init){
							SkyFog.GradientBounds = new Vector2(0,21111);//1500
							SkyFog.luminance = 0.84f;
							SkyFog.ScatterFac = 34.16f;
							SkyFog.turbidity =  61.41f;
							SkyFog.ClearSkyFac =4.8f;
						}
						
						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance, 0.84f,Time.deltaTime*Trans_speed_sky*speed);//20
						SkyFog.lumFac =0.16f;
						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,34.16f,Time.deltaTime*Trans_speed_sky);//
						SkyFog.TurbFac=5.4f;
						SkyFog.HorizFac =0.4f;
						SkyFog.turbidity = Mathf.Lerp(SkyFog.turbidity,61.41f,Time.deltaTime);//
						SkyFog.reileigh=303.24f;
						SkyFog.mieCoefficient=0.074f;
						SkyFog.mieDirectionalG=0.88f;
						SkyFog.bias =0.62f;
						SkyFog.contrast=3.75f;//1.05f
						//SkyFog.Sun
						SkyFog.FogSky = true; //SkyFog.FogSky = true;
						SkyFog.TintColor = new Vector3(35.8f,155,345);
						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,10.91f,Time.deltaTime*Trans_speed_sky); 
					}
					
					//PRESET HAZE 13-4
					if(FogPreset == 13 | FogPreset == 14 ){ //v3.0 foggy - rain
						SkyFog.distanceFog = false;
						SkyFog.useRadialDistance = true;
						SkyFog.heightFog = true;
						//	SkyFog.height = Mathf.Lerp(SkyFog.height,505.69f,Time.deltaTime);//
						//SkyFog.heightDensity = 0.0065f;
						FogheightOffset = Mathf.Lerp(FogheightOffset,30 + AddFogHeightOffset, Time.deltaTime * SpeedFactor * 20);//v3.0
						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
						//lower at night
						if(SkyManager != null){
							//if(SkyManager.Current_Time > 8 & SkyManager.Current_Time <=21.5f){
							//if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
							if(is_DayLightA){
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.05f + FogdensityOffset,Time.deltaTime);//1.49f
								SkyFog.height = Mathf.Lerp(SkyFog.height,1505.69f,Time.deltaTime*0.1f);//
								if(!Application.isPlaying | Init){
									SkyFog.heightDensity = 0.05f + AddFogDensityOffset + FogUnityOffset;
									SkyFog.height = 1505.69f;//
								}//0.012
								
							}else{
								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.05f + FogdensityOffset,Time.deltaTime);
								SkyFog.height = Mathf.Lerp(SkyFog.height,15.69f,Time.deltaTime);//
								if(!Application.isPlaying | Init){
									SkyFog.heightDensity = 0.05f + AddFogDensityOffset + FogUnityOffset;
									SkyFog.height = 15.69f;//
								}
								
							}
							//if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){	
							if(is_DayLight){
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
									if(is_after_216 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
									}
									SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,0,0.4f*Time.deltaTime);
									SunShafts.sunColor = Rays_day_color;
									SunShafts.sunTransform = SkyManager.SunObj.transform;
									SunShafts.useDepthTexture = true;
								}
								
							}else{
								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
								
								if(ImageEffectShafts){
									//apply moon to the shafts script at night and change color to white
									//if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
									if(is_after_79 ){
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
									}else{
										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
									}
									SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,0,0.4f*Time.deltaTime);
									SunShafts.sunColor = Rays_night_color;
									SunShafts.sunTransform = SkyManager.MoonObj.transform;
									SunShafts.useDepthTexture = true;
								}
							}
						}
						//SkyFog.startDistance = 20;
						if(!Application.isPlaying | Init){
							SkyFog.startDistance = 20;
						}else{
							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,20,Time.deltaTime*speed);
						}
						
						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,1500,Time.deltaTime*Trans_speed_sky*speed));//1500
						if(!Application.isPlaying | Init){
							SkyFog.GradientBounds = new Vector2(0,1500);//1500
							SkyFog.luminance = 0.9f;
							SkyFog.ScatterFac = 24f;
							SkyFog.turbidity =  61.41f;
							SkyFog.ClearSkyFac = 2f;
						}
						
						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance, 0.9f,Time.deltaTime*Trans_speed_sky*speed);//20
						SkyFog.lumFac =0.7f;
						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,24f,Time.deltaTime*Trans_speed_sky);//
						SkyFog.TurbFac=0.02f;
						SkyFog.HorizFac =0.4f;
						SkyFog.turbidity = Mathf.Lerp(SkyFog.turbidity,61.41f,Time.deltaTime);//
						SkyFog.reileigh=303.24f;
						SkyFog.mieCoefficient=0.074f;
						SkyFog.mieDirectionalG=0.88f;
						SkyFog.bias =0.62f;
						SkyFog.contrast=3.45f;//1.05f
						//SkyFog.Sun
						SkyFog.FogSky = true; //SkyFog.FogSky = true;
						SkyFog.TintColor = new Vector3(81.8f,155,26);
						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,2f,Time.deltaTime*Trans_speed_sky); 
					}

					//PRESET HAZE 15
					if(FogPreset == 15){ //v3.2 foggy - epic
						
							SkyFog.distanceFog = false;
							SkyFog.useRadialDistance = false;
							SkyFog.heightFog = true;
							//	SkyFog.height = Mathf.Lerp(SkyFog.height,505.69f,Time.deltaTime);//
							//SkyFog.heightDensity = 0.0065f;
							FogheightOffset = Mathf.Lerp(FogheightOffset,113 + AddFogHeightOffset, Time.deltaTime * SpeedFactor * 20);//v3.0
							FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
							//lower at night
							if(SkyManager != null){
								//if(SkyManager.Current_Time > 8 & SkyManager.Current_Time <=21.5f){
								//if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
								if(is_DayLight){
									SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f + FogdensityOffset,Time.deltaTime);//1.49f
									SkyFog.height = Mathf.Lerp(SkyFog.height,155.69f,Time.deltaTime*0.1f);//
									SkyFog.luminance = Mathf.Lerp(SkyFog.luminance, 1.9f,Time.deltaTime*Trans_speed_sky*speed);
									SkyFog.bias = Mathf.Lerp(SkyFog.bias, 1.66f,Time.deltaTime*Trans_speed_sky*speed);//v3.3
									SkyFog.contrast = Mathf.Lerp(SkyFog.contrast, 6.52f,Time.deltaTime*Trans_speed_sky*speed);//v3.3
									SkyFog.mieCoefficient = Mathf.Lerp(SkyFog.mieCoefficient, 0.13f,Time.deltaTime*Trans_speed_sky*speed);//v3.3
									SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,180f,Time.deltaTime*Trans_speed_sky);//
									SkyFog.mieDirectionalG =Mathf.Lerp(SkyFog.mieDirectionalG,0.93f,Time.deltaTime*Trans_speed_sky);//
									SkyFog.TurbFac =Mathf.Lerp(SkyFog.TurbFac,320f,Time.deltaTime*Trans_speed_sky);//
									SkyFog.HorizFac =Mathf.Lerp(SkyFog.HorizFac,421f,Time.deltaTime*Trans_speed_sky);//
									SkyFog.lumFac =Mathf.Lerp(SkyFog.lumFac,0.21f,Time.deltaTime*Trans_speed_sky);//
									SkyFog.turbidity = Mathf.Lerp(SkyFog.turbidity,9.7f,Time.deltaTime*Trans_speed_sky);//
									SkyFog.reileigh = Mathf.Lerp(SkyFog.reileigh,0.8f,Time.deltaTime*Trans_speed_sky);//
									if(!Application.isPlaying | Init){
										SkyFog.heightDensity = 0.0015f + AddFogDensityOffset + FogUnityOffset;
										SkyFog.height = 155.69f;//
										SkyFog.luminance = 1.9f;
										SkyFog.bias =1.66f;//v3.3
										SkyFog.contrast=6.52f;//1.05f
										SkyFog.mieCoefficient=0.13f;
										SkyFog.ScatterFac = 180f;
										SkyFog.mieDirectionalG=0.93f;
										SkyFog.TurbFac=320f;
										SkyFog.HorizFac =421f;
										SkyFog.lumFac =0.21f;
										SkyFog.turbidity = 9.7f;
										SkyFog.reileigh=0.8f;
									}//0.012
									SkyFog.Sun = SkyManager.SunObj.transform;

								}else{
									SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0045f + FogdensityOffset,Time.deltaTime*Trans_speed_sky*speed);//v3.3 - 0.0015 to 0.0075
									SkyFog.height = Mathf.Lerp(SkyFog.height,155.69f,Time.deltaTime);//
									SkyFog.luminance = Mathf.Lerp(SkyFog.luminance, 3.04f,Time.deltaTime*Trans_speed_sky*speed*1);//v3.3 - 2.6 to 2.98
									SkyFog.bias = Mathf.Lerp(SkyFog.bias, 1.3f,Time.deltaTime*Trans_speed_sky*speed);//v3.3
									//SkyFog.contrast = Mathf.Lerp(SkyFog.contrast, 2.0f,Time.deltaTime*Trans_speed_sky*speed*1);//v3.3
									SkyFog.mieCoefficient = Mathf.Lerp(SkyFog.mieCoefficient, 0.3f,Time.deltaTime*Trans_speed_sky*speed);//v3.3
									SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,10f,Time.deltaTime*Trans_speed_sky*1);//
									SkyFog.mieDirectionalG =Mathf.Lerp(SkyFog.mieDirectionalG,0.7f,Time.deltaTime*Trans_speed_sky);//
									SkyFog.TurbFac =Mathf.Lerp(SkyFog.TurbFac,2f,Time.deltaTime*Trans_speed_sky*1);//
									SkyFog.HorizFac =Mathf.Lerp(SkyFog.HorizFac,10f,Time.deltaTime*Trans_speed_sky*1);//
									SkyFog.lumFac =Mathf.Lerp(SkyFog.lumFac,0.4f,Time.deltaTime*Trans_speed_sky);//
									SkyFog.turbidity = Mathf.Lerp(SkyFog.turbidity,0.0f,Time.deltaTime*Trans_speed_sky*speed*4);//
									SkyFog.reileigh = Mathf.Lerp(SkyFog.reileigh,0.01f,Time.deltaTime*Trans_speed_sky);//
									if(!Application.isPlaying | Init){
										SkyFog.heightDensity = 0.0045f + AddFogDensityOffset + FogUnityOffset;
										SkyFog.height = 155.69f;//
										SkyFog.luminance = 3.04f;
										SkyFog.bias =1.3f;//v3.3
										SkyFog.contrast=1.0f;//1.05f
										SkyFog.mieCoefficient=0.3f;
										SkyFog.ScatterFac = 10f;
										SkyFog.mieDirectionalG=0.7f;
										SkyFog.TurbFac=2;
										SkyFog.HorizFac =10;
										SkyFog.lumFac =0.4f;
										SkyFog.turbidity = 0.0f;
										SkyFog.reileigh=0.01f;
									}
									if (SkyManager.Rot_Sun_X < -11) {
										SkyFog.Sun = SkyManager.MoonObj.transform;
										SkyFog.contrast = Mathf.Lerp(SkyFog.contrast, 0.7f,Time.deltaTime*Trans_speed_sky*speed*0.2f);//v3.3
									} else {
										SkyFog.Sun = SkyManager.SunObj.transform;
										SkyFog.contrast = Mathf.Lerp(SkyFog.contrast, 2.5f,Time.deltaTime*Trans_speed_sky*speed*0.1f);//v3.3
									}

								}
								//if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){	

								if(is_DayLight){
									if(ImageEffectShafts){
										//apply moon to the shafts script at night and change color to white
										//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
										if(is_after_216 ){
											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
										}else{
											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Shafts_intensity,0.4f*Time.deltaTime);
										}
										SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.4f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
										SunShafts.sunColor = Rays_day_color;
										SunShafts.sunTransform = SkyManager.SunObj.transform;
										SunShafts.useDepthTexture = true;
									}

								}else{
									//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
									//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}

									if(ImageEffectShafts){
										//apply moon to the shafts script at night and change color to white
										//if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
										if(is_after_79 ){
											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
										}else{
											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,Moon_Shafts_intensity,0.4f*Time.deltaTime);
										}
										SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,5.4f + ShaftBlurRadiusOffset,0.4f*Time.deltaTime);
										SunShafts.sunColor = Rays_night_color;
										SunShafts.sunTransform = SkyManager.MoonObj.transform;
										SunShafts.useDepthTexture = true;
									}
								}

//								if(is_DayLight){
//									if(ImageEffectShafts){
//										//apply moon to the shafts script at night and change color to white
//										//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
//										if(is_after_216 ){
//											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
//										}else{
//											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
//										}
//										SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,0,0.4f*Time.deltaTime);
//										SunShafts.sunColor = Rays_day_color;
//										SunShafts.sunTransform = SkyManager.SunObj.transform;
//										SunShafts.useDepthTexture = true;
//									}
//
//								}else{
//									//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
//									//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
//
//									if(ImageEffectShafts){
//										//apply moon to the shafts script at night and change color to white
//										//if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
//										if(is_after_79 ){
//											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
//										}else{
//											SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
//										}
//										SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,0,0.4f*Time.deltaTime);
//										SunShafts.sunColor = Rays_night_color;
//										SunShafts.sunTransform = SkyManager.MoonObj.transform;
//										SunShafts.useDepthTexture = true;
//									}
//								}
							}
							//SkyFog.startDistance = 20;
							if(!Application.isPlaying | Init){
								SkyFog.startDistance = 20;
							}else{
								SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,20,Time.deltaTime*speed);
							}

							SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,2000,Time.deltaTime*Trans_speed_sky*speed));//1500
							if(!Application.isPlaying | Init){
								SkyFog.GradientBounds = new Vector2(0,1000);//1500
								//SkyFog.luminance = 2.6f;
								//	SkyFog.ScatterFac = 180f;
								SkyFog.turbidity =  9.7f;
								SkyFog.ClearSkyFac = 2f;
							}

							//SkyFog.luminance = Mathf.Lerp(SkyFog.luminance, 2.6f,Time.deltaTime*Trans_speed_sky*speed);//20
							//	SkyFog.lumFac =0.21f;
							//	SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,180f,Time.deltaTime*Trans_speed_sky);//v3.3
							//	SkyFog.TurbFac=320f;
							//	SkyFog.HorizFac =421f;
							//	SkyFog.turbidity = Mathf.Lerp(SkyFog.turbidity,9.7f,Time.deltaTime);//
							//	SkyFog.reileigh=0.8f;
							//	SkyFog.mieCoefficient=0.13f;//v3.3
							//	SkyFog.mieDirectionalG=0.93f;
							//	SkyFog.bias =1.66f;
							//	SkyFog.contrast=6.52f;//1.05f //v3.3
							//SkyFog.Sun
							SkyFog.FogSky = true; //SkyFog.FogSky = true;
							SkyFog.TintColor = new Vector3(65.4f,155,345);
							SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,6.25f,Time.deltaTime*Trans_speed_sky); 
						

//						SkyFog.distanceFog = false;
//						SkyFog.useRadialDistance = false;
//						SkyFog.heightFog = true;
//						//	SkyFog.height = Mathf.Lerp(SkyFog.height,505.69f,Time.deltaTime);//
//						//SkyFog.heightDensity = 0.0065f;
//						FogheightOffset = Mathf.Lerp(FogheightOffset,113 + AddFogHeightOffset, Time.deltaTime * SpeedFactor * 20);//v3.0
//						FogdensityOffset = Mathf.Lerp(FogdensityOffset,0 + AddFogDensityOffset + FogUnityOffset, Time.deltaTime * SpeedFactor);//v3.0
//						//lower at night
//						if(SkyManager != null){
//							//if(SkyManager.Current_Time > 8 & SkyManager.Current_Time <=21.5f){
//							//if(SkyManager.Current_Time > (8+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.5f+ SkyManager.Shift_dawn)){
//							if(is_DayLightA){
//								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f + FogdensityOffset,Time.deltaTime);//1.49f
//								SkyFog.height = Mathf.Lerp(SkyFog.height,155.69f,Time.deltaTime*0.1f);//
//								if(!Application.isPlaying | Init){
//									SkyFog.heightDensity = 0.0015f + AddFogDensityOffset + FogUnityOffset;
//									SkyFog.height = 155.69f;//
//								}//0.012
//
//							}else{
//								SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.05f + FogdensityOffset,Time.deltaTime);
//								SkyFog.height = Mathf.Lerp(SkyFog.height,15.69f,Time.deltaTime);//
//								if(!Application.isPlaying | Init){
//									SkyFog.heightDensity = 0.05f + AddFogDensityOffset + FogUnityOffset;
//									SkyFog.height = 15.69f;//
//								}
//
//							}
//							//if(SkyManager.Current_Time > (9+ SkyManager.Shift_dawn) & SkyManager.Current_Time <=(21.9f+ SkyManager.Shift_dawn)){	
//							if(is_DayLight){
//								if(ImageEffectShafts){
//									//apply moon to the shafts script at night and change color to white
//									//if(SkyManager.Current_Time > (21.6f+ SkyManager.Shift_dawn)){
//									if(is_after_216 ){
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
//									}else{
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
//									}
//									SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,0,0.4f*Time.deltaTime);
//									SunShafts.sunColor = Rays_day_color;
//									SunShafts.sunTransform = SkyManager.SunObj.transform;
//									SunShafts.useDepthTexture = true;
//								}
//
//							}else{
//								//SkyFog.heightDensity = Mathf.Lerp(SkyFog.heightDensity,0.0015f,Time.deltaTime);
//								//if(!Application.isPlaying){SkyFog.heightDensity = 0.0015f;}
//
//								if(ImageEffectShafts){
//									//apply moon to the shafts script at night and change color to white
//									//if(SkyManager.Current_Time > (7.9f+ SkyManager.Shift_dawn)){
//									if(is_after_79 ){
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.1f*Time.deltaTime);
//									}else{
//										SunShafts.sunShaftIntensity = Mathf.Lerp(SunShafts.sunShaftIntensity,0,0.4f*Time.deltaTime);
//									}
//									SunShafts.sunShaftBlurRadius = Mathf.Lerp(SunShafts.sunShaftBlurRadius,0,0.4f*Time.deltaTime);
//									SunShafts.sunColor = Rays_night_color;
//									SunShafts.sunTransform = SkyManager.MoonObj.transform;
//									SunShafts.useDepthTexture = true;
//								}
//							}
//						}
//						//SkyFog.startDistance = 20;
//						if(!Application.isPlaying | Init){
//							SkyFog.startDistance = 20;
//						}else{
//							SkyFog.startDistance = Mathf.Lerp(SkyFog.startDistance,20,Time.deltaTime*speed);
//						}
//
//						SkyFog.GradientBounds = new Vector2(0,Mathf.Lerp(SkyFog.GradientBounds.y,1000,Time.deltaTime*Trans_speed_sky*speed));//1500
//						if(!Application.isPlaying | Init){
//							SkyFog.GradientBounds = new Vector2(0,1000);//1500
//							SkyFog.luminance = 2.6f;
//							SkyFog.ScatterFac = 180f;
//							SkyFog.turbidity =  9.7f;
//							SkyFog.ClearSkyFac = 2f;
//						}
//
//						SkyFog.luminance = Mathf.Lerp(SkyFog.luminance, 2.6f,Time.deltaTime*Trans_speed_sky*speed);//20
//						SkyFog.lumFac =0.21f;
//						SkyFog.ScatterFac =Mathf.Lerp(SkyFog.ScatterFac,180f,Time.deltaTime*Trans_speed_sky);//
//						SkyFog.TurbFac=320f;
//						SkyFog.HorizFac =421f;
//						SkyFog.turbidity = Mathf.Lerp(SkyFog.turbidity,9.7f,Time.deltaTime);//
//						SkyFog.reileigh=0.8f;
//						SkyFog.mieCoefficient=0.13f;
//						SkyFog.mieDirectionalG=0.93f;
//						SkyFog.bias =1.66f;
//						SkyFog.contrast=6.52f;//1.05f
//						//SkyFog.Sun
//						SkyFog.FogSky = true; //SkyFog.FogSky = true;
//						SkyFog.TintColor = new Vector3(65.4f,155,345);
//						SkyFog.ClearSkyFac = Mathf.Lerp(SkyFog.ClearSkyFac,6.25f,Time.deltaTime*Trans_speed_sky); 
					}
					
					//OVVERIDE FOG HEIGHT - v3.0
//					if(FogHeightByTerrain){
//						if(Mesh_Terrain){
//							SkyFog.height = this.transform.position.y + 25 + FogheightOffset;// + AddFogHeightOffset;
//						}
//						if(Terrain.activeTerrain != null){
//							SkyFog.height = this.transform.position.y + 25 + FogheightOffset;// + AddFogHeightOffset;
//						}
//					}
					
					
					//v2.1 - Add gradient if one defined and is different than current
					if(GradientHolders.Count > 0){
						if(FogPreset < GradientHolders.Count){
							if(GradientHolders[FogPreset] != null){
								if(GradientHolders[FogPreset].DistGradient != SkyFog.DistGradient){
									//v3.0 lerp gradients
									if(Lerp_gradient){
										//make sure they have same key number
										int Key_diff = Mathf.Abs(SkyFog.DistGradient.colorKeys.Length -  GradientHolders[FogPreset].DistGradient.colorKeys.Length);
										//										for(int i=0;i<Key_diff;i++){
										//											SkyFog.DistGradient.SetKeys
										//										}
										//										if(Key_diff > 0){
										//											if(SkyFog.DistGradient.colorKeys.Length < GradientHolders[FogPreset].DistGradient.colorKeys.Length){
										//												SkyFog.DistGradient.colorKeys.
										//											}
										//										}
										//Debug.Log("b="+Key_diff);
										//Debug.Log("b1="+SkyFog.DistGradient.colorKeys.Length);
										//Debug.Log("b2="+GradientHolders[FogPreset].DistGradient.colorKeys.Length);
										//Key_diff = Mathf.Abs(SkyFog.DistGradient.colorKeys.Length -  GradientHolders[FogPreset].DistGradient.colorKeys.Length);
										if(Key_diff == 0){
											//Debug.Log(Key_diff);
											GradientColorKey[] Keys = new GradientColorKey[8]; 
											for(int i=0;i<GradientHolders[FogPreset].DistGradient.colorKeys.Length;i++){
												Keys[i] = new GradientColorKey(Color.Lerp(SkyFog.DistGradient.colorKeys[i].color,GradientHolders[FogPreset].DistGradient.colorKeys[i].color,Time.deltaTime*1.4f),
												                               Mathf.Lerp(SkyFog.DistGradient.colorKeys[i].time,GradientHolders[FogPreset].DistGradient.colorKeys[i].time,Time.deltaTime*0.4f));
												//GradientHolders[FogPreset].DistGradient.colorKeys[i].time);
												//SkyFog.DistGradient.colorKeys[i].color = Color.Lerp(SkyFog.DistGradient.colorKeys[i].color,GradientHolders[FogPreset].DistGradient.colorKeys[i].color,Time.deltaTime); 
												//SkyFog.DistGradient.colorKeys[i].time = Mathf.Lerp(SkyFog.DistGradient.colorKeys[i].time,GradientHolders[FogPreset].DistGradient.colorKeys[i].time,Time.deltaTime);
											}
											SkyFog.DistGradient.SetKeys(Keys,SkyFog.DistGradient.alphaKeys);
											//Debug.Log(Key_diff);
										}else{
											//SkyFog.DistGradient = GradientHolders[FogPreset].DistGradient; //this should only happen at game start, then volume fog gradient will have 8 keys
											//Debug.Log(Key_diff);
											//Debug.Log("A="+SkyFog.DistGradient.colorKeys.Length);
											//Debug.Log("A1="+GradientHolders[FogPreset].DistGradient.colorKeys.Length);
											SkyFog.DistGradient.SetKeys(GradientHolders[FogPreset].DistGradient.colorKeys,GradientHolders[FogPreset].DistGradient.alphaKeys);
										}
									}else{
										//SkyFog.DistGradient = GradientHolders[FogPreset].DistGradient; //replaces gradient, if one is defined
										//Debug.Log("in2");
										SkyFog.DistGradient.SetKeys(GradientHolders[FogPreset].DistGradient.colorKeys,GradientHolders[FogPreset].DistGradient.alphaKeys);
									}
								}
							}
						}
					}
					
					// disable unity fog in winter and foggy case
					// and change to appropriate preset
					if(SkyManager != null){
						if(!Use_both_fogs){//v2.0.1
							if(SkyManager.Weather == SkyMasterManager.Weather_types.Foggy
							   | SkyManager.Weather == SkyMasterManager.Weather_types.HeavyFog
							   | SkyManager.Season == 4 ){
								SkyManager.Use_fog = false;
								RenderSettings.fog = false;
								RenderSettings.fogDensity = 0.0005f;
								FogPreset = 1;
							}else{
								//FogPreset = 0;
								RenderSettings.fogDensity = 0.00005f;
							}
						}
					}

					//v3.3e
					if(UseFogCurves){
						float calcPoint = SkyManager.calcColorTime;
						//SkyFog.height = heightOffsetFogCurve.Evaluate(calcPoint);
						//		AddFogHeightOffset = heightOffsetFogCurve.Evaluate(calcPoint);
						FogheightOffset = heightOffsetFogCurve.Evaluate(calcPoint);
						AddFogHeightOffset = 0;

						if (!FogHeightByTerrain) {
							SkyFog.height = FogheightOffset;
						}

						SkyFog.FogSky = SkyFogOn;
						SkyFog.heightFog = HeightFogOn;
						SkyFog.distanceFog = DistanceFogOn;
						//if (DistanceFogOn) {
						SkyFog.startDistance = VFogDistance;//if DistanceFogOn
						//}
						//AddFogHeightOffset = 0;
						//FogheightOffset = heightOffsetFogCurve.Evaluate(calcPoint);
						SkyFog.luminance = luminanceVFogCurve.Evaluate(calcPoint);
						SkyFog.lumFac = lumFactorFogCurve.Evaluate (calcPoint);
						SkyFog.ScatterFac = scatterFacFogCurve.Evaluate (calcPoint);
						SkyFog.turbidity = turbidityFogCurve.Evaluate (calcPoint);
						SkyFog.TurbFac = turbFacFogCurve.Evaluate (calcPoint);
						SkyFog.HorizFac = horizonFogCurve.Evaluate (calcPoint);
						SkyFog.contrast = contrastFogCurve.Evaluate (calcPoint);
					}

					//OVVERIDE FOG HEIGHT - v3.0
					if(FogHeightByTerrain){
						if(Mesh_Terrain){
							SkyFog.height = this.transform.position.y + 25 + FogheightOffset;// + AddFogHeightOffset;
						}
						if(Terrain.activeTerrain != null){
							SkyFog.height = this.transform.position.y + 25 + FogheightOffset;// + AddFogHeightOffset;
						}
					}

					//v3.3e
//					if(UseFogCurves){
//						float calcPoint = SkyManager.calcColorTime;
//						//SkyFog.height = heightOffsetFogCurve.Evaluate(calcPoint);
//						AddFogHeightOffset = heightOffsetFogCurve.Evaluate(calcPoint);
//						//AddFogHeightOffset = 0;
//						//FogheightOffset = heightOffsetFogCurve.Evaluate(calcPoint);
//						SkyFog.luminance = luminanceVFogCurve.Evaluate(calcPoint);
//						SkyFog.lumFac = lumFactorFogCurve.Evaluate (calcPoint);
//						SkyFog.ScatterFac = scatterFacFogCurve.Evaluate (calcPoint);
//						SkyFog.turbidity = turbidityFogCurve.Evaluate (calcPoint);
//						SkyFog.TurbFac = turbFacFogCurve.Evaluate (calcPoint);
//						SkyFog.HorizFac = horizonFogCurve.Evaluate (calcPoint);
//						SkyFog.contrast = contrastFogCurve.Evaluate (calcPoint);
//					}

				}
			}
			
		}
		//END preset handle function

 }
}
