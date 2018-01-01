using UnityEngine;
using System.Collections;
using Artngame.GIPROXY;

namespace Artngame.SKYMASTER {

public class SKYMASTER_DemoV17 : MonoBehaviour {

	#pragma warning disable 414

	void Start () {

		if(SKYMASTER_OBJ!=null){
				SUNMASTER = SKYMASTER_OBJ.GetComponent(typeof(SkyMasterManager)) as SkyMasterManager;
		}
		SPEED = SUNMASTER.SPEED;

			SUNMASTER.Seasonal_change_auto = false;

			//v1.7
			HORIZON = SUNMASTER.Horizon_adj;
			DuskLightG = SUNMASTER.Dusk_Sun_Color.g;
			DuskLightB = SUNMASTER.Dusk_Sun_Color.b;

			//v1.7
			if(TerrainHandle == null){
				TerrainObj.SetActive(true);
				TerrainHandle = TerrainObj.GetComponentInChildren(typeof(SeasonalTerrainSKYMASTER)) as SeasonalTerrainSKYMASTER;
				TerrainObj.SetActive(false);
			}
			if(MeshTerrainHandle == null){
				MeshTerrainObj.SetActive(true);
				MeshTerrainHandle = MeshTerrainObj.GetComponentInChildren(typeof(SeasonalTerrainSKYMASTER)) as SeasonalTerrainSKYMASTER;
			}

			TOD = SUNMASTER.Current_Time;

			GI_controller = SUNMASTER.SUN_LIGHT.GetComponent(typeof(ControlGIPROXY_ATTRIUM_SM)) as ControlGIPROXY_ATTRIUM_SM;
			Gi_proxy = SUNMASTER.SUN_LIGHT.GetComponent(typeof(LightCollisionsPDM)) as LightCollisionsPDM;

			if(GI_controller!=null){
				GI_controller.enabled = false;
			}
	}

		ControlGIPROXY_ATTRIUM_SM GI_controller;
		LightCollisionsPDM Gi_proxy;
		public GameObject GI_ITEMS;
	//v1.7
		public ColorCorrectionCurvesSkyMaster Colorizer;
		public AntialiasingSkyMaster AntiAlising;
		public ContrastStretchSkyMaster ContrastFilter;

		public GameObject Freeze_POOL;
		public GameObject Freezer;

		public GameObject Rain1;
		public GameObject Rain2;

		public GameObject Bats;
		public GameObject Leaves;

		public GameObject Floor;
		public GameObject Floor_stripes;
		public GameObject Floor_collider;
		bool Special_effects=false;

		bool Sky_effects=false;
		public GameObject SkyDOME;

		public GameObject Typhoon;
		public GameObject ChainLightning;

	public float Sun_time_start = 14.43f;	//at this time, rotation of sunsystem must be 62 (14, -1.525879e-05, -1.525879e-05 WORKS !!)

	public GameObject SKYMASTER_OBJ;

	SkyMasterManager SUNMASTER;

	public GameObject SUN;
	public GameObject TREES;

	public bool HUD_ON=true;

	float HDR=0.8f;
	float Esun=22;
	float Kr=0.0025f;
	float Km=0.0015f;
	float GE=-0.96f;
	float SPEED = 0.01f;

		//v1.7
		float HORIZON = 96f;
		float DuskLightG; 
		float DuskLightB;

	float fSamples = 3;
	float fScaleDepth = 0.5f;

	bool set_sun_start=false;
	float Ring_factor=0;

	Vector3 CURRENT_Force_color = new Vector3(0.65f,0.52f,0.475f);
	float Coloration = 0.28f;
	Vector4 TintColor = new Vector4(0,0,0,0);

	public bool enable_controls=false;
	//public bool enable_hud=true;

	public GameObject Clouds_top;
	public GameObject Clouds_bottom;
	public GameObject Flat_Clouds_top;
	public GameObject Flat_Clouds_bottom;
	public GameObject Cloud_Dome;
	public GameObject Cloud_Rays;
	public GameObject Cloud_Static;

	float Dome_rot = 0;

	public GameObject Tornado1;
	public GameObject Tornado2;

		public GameObject Butterflies;
		public GameObject FreezeEffect;
		public GameObject LightningStorm;

		public bool Auto_Season_Cycle=false;
		float Cycle_speed = 1500;
		bool GI_controls_on = false;

		//v1.7
		int current_ground = 0; //0 horizon, 1 terrain, 2 water, 3 sky
		int current_haze = 0; 
		int current_sky = 0; 
		SeasonalTerrainSKYMASTER TerrainHandle;
		SeasonalTerrainSKYMASTER MeshTerrainHandle;
		public GameObject TerrainObj;
		public GameObject HorizonObj;
		public GameObject MeshTerrainObj;//enable in all cases besides Terrain
		public GameObject Water;
		public GameObject CloudHanlder;

		float Camera_up;

		bool Rain1_on;
		bool Rain2_on;
		bool Leaves_on;
		bool Bats_on;
		bool Freeze_on;
		bool Colorcorrect_on;
		bool Contrast_on;
		bool AntiAliasng_on;

		bool SkyDome_on;
		bool MoonMesh_on;
		float TOD;

		bool Typhoon_on;
		bool Chain_on;

		void Disable_all(){
			if(Leaves.activeInHierarchy){
				Leaves.SetActive(false);
			}
			if(Freezer.activeInHierarchy){
				Freezer.SetActive(false);
			}
			if(Freeze_POOL.activeInHierarchy){
				Freeze_POOL.SetActive(false);
			}
			if(Bats.activeInHierarchy){
				Bats.SetActive(false);
			}
			if(Rain1.activeInHierarchy){
				Rain1.SetActive(false);
			}
			if(Rain2.activeInHierarchy){
				Rain2.SetActive(false);
			}

			if(Typhoon.activeInHierarchy){
				Typhoon.SetActive(false);
			}
			if(ChainLightning.activeInHierarchy){
				ChainLightning.SetActive(false);
			}
		}

	void OnGUI() {

			float BOX_WIDTH = 100;float BOX_HEIGHT = 30;

			//CAMERA 
			GUI.TextArea( new Rect(3*BOX_WIDTH+10, BOX_HEIGHT-30, BOX_WIDTH+0, 20),"Camera height");

			float min_height = 100;
			if(current_ground == 1){
				min_height = 285;
			}

			Camera_up = GUI.HorizontalSlider(new Rect(3*BOX_WIDTH+10, BOX_HEIGHT-11, BOX_WIDTH+0, 30  ),Camera.main.transform.position.y ,min_height,1560);
			
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,Camera_up,Camera.main.transform.position.z);

			//v1.7 - special effects
			if(Special_effects){
				if(current_ground == 2 | Typhoon_on){ //if ocean or typhoon, remove floor
					//Floor
					if(Floor.activeInHierarchy){
						Floor.SetActive(false);
					}
					if(Floor_stripes.activeInHierarchy){
						Floor_stripes.SetActive(false);
					}
					if(!Floor_collider.activeInHierarchy){
						Floor_collider.SetActive(true);
					}
				}else{
					if(!Floor.activeInHierarchy){
						Floor.SetActive(true);
					}
					if(!Floor_stripes.activeInHierarchy){
						Floor_stripes.SetActive(true);
					}
					if(!Floor_collider.activeInHierarchy){
						Floor_collider.SetActive(true);
					}
				}
			}else{
				if(Floor.activeInHierarchy){
					Floor.SetActive(false);
				}
				if(Floor_stripes.activeInHierarchy){
					Floor_stripes.SetActive(false);
				}
				if(Leaves.activeInHierarchy){
					Leaves.SetActive(false);
				}
				if(Freezer.activeInHierarchy){
					Freezer.SetActive(false);
				}
				if(Freeze_POOL.activeInHierarchy){
					Freeze_POOL.SetActive(false);
				}
				if(Bats.activeInHierarchy){
					Bats.SetActive(false);
				}
				if(Rain1.activeInHierarchy){
					Rain1.SetActive(false);
				}
				if(Rain2.activeInHierarchy){
					Rain2.SetActive(false);
				}
				if(Floor_collider.activeInHierarchy){
					Floor_collider.SetActive(false);
				}
			}

			string HUD_gui = "Disable HUD";
			if(!HUD_ON){
				HUD_gui = "Enable HUD";
			}
			if (GUI.Button(new Rect(5, 0*BOX_HEIGHT, BOX_WIDTH+5, 20), HUD_gui)){				
				if(HUD_ON){
					HUD_ON = false;
					if(CloudHanlder.activeInHierarchy){
						CloudHanlder.SetActive(false);
					}

					if(GI_controller.enabled){
						//GI_controls_on = false;
					}

				}else{
					HUD_ON = true;
					if(!CloudHanlder.activeInHierarchy){
						CloudHanlder.SetActive(true);
					}
				}				
			}

			if(HUD_ON){


				string toggle_sky_effects = "On";
				if(Sky_effects){
					toggle_sky_effects = "Off";
				}
				
				//v1.7 - sky effects
				if(!Special_effects){
					if (GUI.Button(new Rect(3*BOX_WIDTH+10, BOX_HEIGHT+60+30+30, BOX_WIDTH, 30), "Sky FX "+toggle_sky_effects)){
						if(Sky_effects){
							Sky_effects = false;
							
						}else{
							Sky_effects = true;
							Special_effects = false;
						}
					}
				}
				if(Sky_effects){ // special effects GUI
					
					//Enable Sky Dome
					string skyDome_toggle = "On";
					if(SkyDome_on){
						skyDome_toggle = "Off";
					}
					if (GUI.Button(new Rect(25+3*BOX_WIDTH+10, BOX_HEIGHT+60+30 +30+30, BOX_WIDTH, 20), "Sky Dome "+skyDome_toggle)){
						if(SkyDome_on){
							SkyDome_on = false;
							SUNMASTER.Preset = 0;
							if(SkyDOME.activeInHierarchy){
								SkyDOME.SetActive(false);
							}
						}else{
							SkyDome_on = true;
							SUNMASTER.Preset = 5;
							if(!SkyDOME.activeInHierarchy){
								SkyDOME.SetActive(true);
							}
						}
					}
					//Enable Moon Mesh
					string moon_mesh_toggle = "On";
					if(MoonMesh_on){
						moon_mesh_toggle = "Off";
					}
					if (GUI.Button(new Rect(25+3*BOX_WIDTH+10, BOX_HEIGHT+60+30 +30+30 +20, BOX_WIDTH, 20), "Moon Mesh "+moon_mesh_toggle)){
						if(MoonMesh_on){
							MoonMesh_on = false;
							//SUNMASTER.Preset = 0;
							if(SUNMASTER.MoonObj.GetComponent<Renderer>().enabled){
								SUNMASTER.MoonObj.GetComponent<Renderer>().enabled = false;
							}
						}else{
							MoonMesh_on = true;
							//SUNMASTER.Preset = 5;
							if(!SUNMASTER.MoonObj.GetComponent<Renderer>().enabled){
								SUNMASTER.MoonObj.GetComponent<Renderer>().enabled = true;
							}
						}
					}
					//time of day
					GUI.TextArea( new Rect(25+3*BOX_WIDTH+10, BOX_HEIGHT+60+30 +30+30 +20+20+2, BOX_WIDTH, 20),"Time of Day");
					TOD = GUI.HorizontalSlider(new Rect(25+3*BOX_WIDTH+10, BOX_HEIGHT+60+30 +30+30 +20+40, BOX_WIDTH, 20),SUNMASTER.Current_Time,0f,24);
					SUNMASTER.Current_Time = TOD;
				}

				if(SPEED < 5){
					SPEED =0;
				}


				string toggle_effects = "On";
				if(Special_effects){
					toggle_effects = "Off";
				}
			
				//v1.7 - special effects
				if (GUI.Button(new Rect(3*BOX_WIDTH+10, BOX_HEIGHT+60+30, BOX_WIDTH, 30), "Special FX "+toggle_effects)){
					if(Special_effects){
						Special_effects = false;
						 
					}else{
						Special_effects = true;
						Sky_effects = false;
					}
				}
				if(Special_effects){ // special effects GUI

					//Anti Aliasing
					string AntiAliasng_toggle = "On";
					if(AntiAliasng_on){
						AntiAliasng_toggle = "Off";
					}
					if (GUI.Button(new Rect(25+3*BOX_WIDTH+10, BOX_HEIGHT+60+30 +30, BOX_WIDTH, 20), "AntiAlising "+AntiAliasng_toggle)){
						if(AntiAliasng_on){
							AntiAliasng_on = false;
							AntiAlising.enabled = false;
						}else{
							AntiAliasng_on = true;
							AntiAlising.enabled = true;
						}
					}

					//Color correct
					string ColorCorrect_toggle = "On";
					if(Colorcorrect_on){
						ColorCorrect_toggle = "Off";
					}
					if (GUI.Button(new Rect(25+3*BOX_WIDTH+10, BOX_HEIGHT+60+30 +20+30, BOX_WIDTH, 20), "Color FX "+ColorCorrect_toggle)){
						if(Colorcorrect_on){
							Colorcorrect_on = false;
							Colorizer.enabled = false;
						}else{
							Colorcorrect_on = true;
							Colorizer.enabled = true;
						}
					}

					//Contrast
					string Contrast_toggle = "On";
					if(Contrast_on){
						Contrast_toggle = "Off";
					}
					if (GUI.Button(new Rect(25+3*BOX_WIDTH+10, BOX_HEIGHT+60+30 +(2*20)+30, BOX_WIDTH, 20), "Contrast "+Contrast_toggle)){
						if(Contrast_on){
							Contrast_on = false;
							ContrastFilter.enabled = false;
						}else{
							Contrast_on = true;
							ContrastFilter.enabled = true;
						}
					}

					//RAIN 1
					string toggle_rain1 = "On";
					if(Rain1_on){
						toggle_rain1 = "Off";
					}
					if (GUI.Button(new Rect(25+3*BOX_WIDTH+10, BOX_HEIGHT+60+30 +(3*20)+30, BOX_WIDTH, 20), "Rain "+toggle_rain1)){
						if(Rain1_on){
							Rain1_on = false;
							if(Rain1.activeInHierarchy){
								Rain1.SetActive(false);
							}
						}else{
							Rain1_on = true;
							Disable_all();
							if(!Rain1.activeInHierarchy){
								Rain1.SetActive(true);
							}

						}
					}

					//RAIN 2
					string toggle_rain2 = "On";
					if(Rain2_on){
						toggle_rain2 = "Off";
					}
					if (GUI.Button(new Rect(25+3*BOX_WIDTH+10, BOX_HEIGHT+60+30 +(4*20)+30, BOX_WIDTH, 20), "Heavy Rain "+toggle_rain2)){
						if(Rain2_on){
							Rain2_on = false;
							if(Rain2.activeInHierarchy){
								Rain2.SetActive(false);
							}
						}else{
							Rain2_on = true;
							Disable_all();
							if(!Rain2.activeInHierarchy){
								Rain2.SetActive(true);
							}
							
						}
					}
					//Leaves
					string toggle_leaves = "On";
					if(Leaves_on){
						toggle_leaves = "Off";
					}
					if (GUI.Button(new Rect(25+3*BOX_WIDTH+10, BOX_HEIGHT+60+30 +(5*20)+30, BOX_WIDTH, 20), "Leaves "+toggle_leaves)){
						if(Leaves_on){
							Leaves_on = false;
							if(Leaves.activeInHierarchy){
								Leaves.SetActive(false);
							}
						}else{
							Leaves_on = true;
							Disable_all();
							if(!Leaves.activeInHierarchy){
								Leaves.SetActive(true);
							}
							
						}
					}
					//Bats
					string toggle_bats = "On";
					if(Bats_on){
						toggle_bats = "Off";
					}
					if (GUI.Button(new Rect(25+3*BOX_WIDTH+10, BOX_HEIGHT+60+30 +(6*20)+30, BOX_WIDTH, 20), "Bats "+toggle_bats)){
						if(Bats_on){
							Bats_on = false;
							if(Bats.activeInHierarchy){
								Bats.SetActive(false);
							}
						}else{
							Bats_on = true;
							Disable_all();
							if(!Bats.activeInHierarchy){
								Bats.SetActive(true);
							}
							
						}
					}
					//Freeze
					string toggle_ice_decals = "On";
					if(Freeze_on){
						toggle_ice_decals = "Off";
					}
					if (GUI.Button(new Rect(25+3*BOX_WIDTH+10, BOX_HEIGHT+60+30 +(7*20)+30, BOX_WIDTH, 20), "Ice decals "+toggle_ice_decals)){
						if(Freeze_on){
							Freeze_on = false;
							if(Freezer.activeInHierarchy){
								Freezer.SetActive(false);
							}
							if(Freeze_POOL.activeInHierarchy){
								Freeze_POOL.SetActive(false);
							}
						}else{
							Freeze_on = true;
							Disable_all();
							if(!Freezer.activeInHierarchy){
								Freezer.SetActive(true);
							}
							if(!Freeze_POOL.activeInHierarchy){
								Freeze_POOL.SetActive(true);
							}
						}
					}

					//TYPHOON
					string toggle_typhoon = "On";
					if(Typhoon_on){
						toggle_typhoon = "Off";
					}
					if (GUI.Button(new Rect(25+3*BOX_WIDTH+10, BOX_HEIGHT+60+30 +(8*20)+30, BOX_WIDTH, 20), "Tornados "+toggle_typhoon)){
						if(Typhoon_on){
							Typhoon_on = false;
							if(Typhoon.activeInHierarchy){
								Typhoon.SetActive(false);
							}
						}else{
							Typhoon_on = true;
							Disable_all();
							if(!Typhoon.activeInHierarchy){
								Typhoon.SetActive(true);
							}
							
						}
					}

					//Chain Lightning
					string toggle_chain = "On";
					if(Chain_on){
						toggle_chain = "Off";
					}
					if (GUI.Button(new Rect(25+3*BOX_WIDTH+10, BOX_HEIGHT+60+30 +(9*20)+30, BOX_WIDTH, 20), "Lightning "+toggle_chain)){
						if(Chain_on){
							Chain_on = false;
							if(ChainLightning.activeInHierarchy){
								ChainLightning.SetActive(false);
							}
						}else{
							Chain_on = true;
							Disable_all();
							if(!ChainLightning.activeInHierarchy){
								ChainLightning.SetActive(true);
							}
							
						}
					}


				}// END Special EFFECTS handle GUI



		if(SUNMASTER.Current_Time!=Sun_time_start & !set_sun_start){
				//SUNMASTER.Current_Time=Sun_time_start;
			set_sun_start=true;
		}

			if(SUNMASTER.Season == 1){
				if(!Butterflies.activeInHierarchy){
					Butterflies.SetActive(true);
				}
			}else{
				if(Butterflies.activeInHierarchy){
					Butterflies.SetActive(false);
				}
			} 

			//GI_controls_on = false;

				//// GI PROXY controls
				/// 
				if(GI_controller!=null){
					if (GUI.Button(new Rect(1*BOX_WIDTH+(2*(BOX_WIDTH/3))-25, BOX_HEIGHT+60+10, BOX_WIDTH/3+35, 20), "GI Proxy")){
						
						if(!GI_controller.enabled){
							GI_controls_on = true;
							GI_controller.enabled = true;
							if(!GI_ITEMS.activeInHierarchy){
								GI_ITEMS.SetActive(true);
							}
							Gi_proxy.enabled = true;
						}else{
							GI_controls_on = false;
							GI_controller.enabled = false;
							if(GI_ITEMS.activeInHierarchy){
								GI_ITEMS.SetActive(false);
							}
							Gi_proxy.enabled = false;
						}
					}
				}


		if(GI_controls_on){
				//let GI controller take over GUI
		}
		else{
					
				if(!enable_controls){
					if (GUI.Button(new Rect(1*BOX_WIDTH+10, BOX_HEIGHT+60+30+30, BOX_WIDTH, 30), "Cycle Seasons")){
						if(!Auto_Season_Cycle){
							Auto_Season_Cycle = true;
							SUNMASTER.SPEED = 2431.818f;
							SUNMASTER.Seasonal_change_auto = true;
						}else{
							Auto_Season_Cycle = false;
							SUNMASTER.SPEED = 35;
							SUNMASTER.Seasonal_change_auto = false;
						}
					}
				}
				if(Auto_Season_Cycle){
					Cycle_speed = GUI.HorizontalSlider(new Rect(5*BOX_WIDTH+10, BOX_HEIGHT+80+30, BOX_WIDTH, 30),Cycle_speed,50,2431.818f);
					SUNMASTER.SPEED = Cycle_speed;
				}

				//Auto_Season_Cycle=false;
			if(!Auto_Season_Cycle){

					if(!enable_controls){
						if (GUI.Button(new Rect(1*BOX_WIDTH+10, BOX_HEIGHT+60+30, BOX_WIDTH, 30), "Sunny")){
							SUNMASTER.Weather = SkyMasterManager.Weather_types.Sunny;
							SUNMASTER.On_demand = true;
						}
					}
		
					if (GUI.Button(new Rect(2*BOX_WIDTH+10, BOX_HEIGHT-0, BOX_WIDTH, 30), "Cloudy")){
						SUNMASTER.Weather = SkyMasterManager.Weather_types.Cloudy;
						SUNMASTER.On_demand = true;
					}

					if (GUI.Button(new Rect(2*BOX_WIDTH+10, BOX_HEIGHT+30, BOX_WIDTH, 30), "Heavy Rain")){
						SUNMASTER.Weather = SkyMasterManager.Weather_types.FlatClouds;
						SUNMASTER.On_demand = true;
					}

		string Season_current= "Spring";
			if(SUNMASTER.Season == 2){
				Season_current= "Summer";
			}
			if(SUNMASTER.Season == 3){
				Season_current= "Autumn";
			}
			if(SUNMASTER.Season == 4){
				Season_current= "Winter";
			}
		GUI.TextArea(new Rect(2*BOX_WIDTH+10-(0/2), BOX_HEIGHT+60+30, BOX_WIDTH/1, 25),Season_current);
		if (GUI.Button(new Rect(2*BOX_WIDTH+10, BOX_HEIGHT+60, BOX_WIDTH, 30), "Cycle Season")){

				if(SUNMASTER.Season ==0){
					SUNMASTER.Season = 2;
				}else{
					SUNMASTER.Season = SUNMASTER.Season+1;
				}
				if(SUNMASTER.Season > 4){
					SUNMASTER.Season = 1;
				}
		}
					if (GUI.Button(new Rect(2*BOX_WIDTH+10, BOX_HEIGHT+60+30+30, BOX_WIDTH, 30), "Foggy")){
						SUNMASTER.Weather = SkyMasterManager.Weather_types.HeavyFog;
						SUNMASTER.On_demand = true;
					}


					//v1.7 - Choose ground type - sky and haze presets
					if (GUI.Button(new Rect(2*BOX_WIDTH+10, 2*BOX_HEIGHT+60+30+30, BOX_WIDTH, 20), "Toggle Ground")){
						current_ground++;
						if(current_ground > 3){
							current_ground=0;
						}
					}
					if(current_ground == 0){ // horizon
						if(Water.activeInHierarchy){
							Water.SetActive(false);
						} 
						if(TerrainObj.activeInHierarchy){
							TerrainObj.SetActive(false);
						} 
						if(!MeshTerrainObj.activeInHierarchy){ //enable haze handler
							MeshTerrainObj.SetActive(true);
						} 
						if(!HorizonObj.activeInHierarchy){
							HorizonObj.SetActive(true);
						}
					}
					if(current_ground == 1){ //terrain
						if(Water.activeInHierarchy){
							Water.SetActive(false);
						} 
						if(!TerrainObj.activeInHierarchy){
							TerrainObj.SetActive(true);
						} 
						if(MeshTerrainObj.activeInHierarchy){ //enable haze handler
							MeshTerrainObj.SetActive(false);
						} 
						if(HorizonObj.activeInHierarchy){
							HorizonObj.SetActive(false);
						}
					}
					if(current_ground == 2){ //water
						if(!Water.activeInHierarchy){
							Water.SetActive(true);
						} 
						if(TerrainObj.activeInHierarchy){
							TerrainObj.SetActive(false);
						} 
						if(!MeshTerrainObj.activeInHierarchy){ //enable haze handler
							MeshTerrainObj.SetActive(true);
						} 
						if(HorizonObj.activeInHierarchy){
							HorizonObj.SetActive(false);
						}
					}
					if(current_ground == 3){ //sky
						if(Water.activeInHierarchy){
							Water.SetActive(false);
						} 
						if(TerrainObj.activeInHierarchy){
							TerrainObj.SetActive(false);
						} 
						if(!MeshTerrainObj.activeInHierarchy){ //enable haze handler
							MeshTerrainObj.SetActive(true);
						} 
						if(HorizonObj.activeInHierarchy){
							HorizonObj.SetActive(false);
						}
					}
					//0 horiz(ON), 1 terrain, 2 water, 3 sky
					if (GUI.Button(new Rect(2*BOX_WIDTH+10,-10+ 3*BOX_HEIGHT+60+30+30, BOX_WIDTH, 20), "Toggle Haze")){
						current_haze++;
						if(current_haze >5){
							current_haze = 0;
						}
						if(current_ground == 1){
							TerrainHandle.FogPreset = current_haze;
						}else{
							MeshTerrainHandle.FogPreset = current_haze;
						}
					}
					if (GUI.Button(new Rect(2*BOX_WIDTH+10,-20+ 4*BOX_HEIGHT+60+30+30, BOX_WIDTH, 20), "Toggle Sky")){
						current_sky++;
						if(current_sky >4){
							current_sky = 0;
						}
						SUNMASTER.Preset = current_sky;

							SPEED = 5;
					}

						//Show currents
						GUI.TextArea( new Rect(3*BOX_WIDTH+10, 2*BOX_HEIGHT+60+30+30, 15, 19),(current_ground+1).ToString());
						GUI.TextArea( new Rect(3*BOX_WIDTH+10,-10+ 3*BOX_HEIGHT+60+30+30, 15, 19),(current_haze+1).ToString());
						GUI.TextArea( new Rect(3*BOX_WIDTH+10,-20+ 4*BOX_HEIGHT+60+30+30, 15, 19),(current_sky+1).ToString());


					if (GUI.Button(new Rect(3*BOX_WIDTH+10, BOX_HEIGHT-0, BOX_WIDTH, 30), "Snow storm")){
						SUNMASTER.Weather = SkyMasterManager.Weather_types.FreezeStorm;
						SUNMASTER.On_demand = true;
					}

					if (GUI.Button(new Rect(2*BOX_WIDTH+10, BOX_HEIGHT-30, BOX_WIDTH, 30), "Volcano")){
						//SUNMASTER.Weather = SkyMasterManager.Weather_types.HeavyStorm;
							SUNMASTER.Weather = SkyMasterManager.Weather_types.VolcanoErupt;
						SUNMASTER.On_demand = true;

							current_ground = 1;
					}

					if (GUI.Button(new Rect(3*BOX_WIDTH+10, 2*BOX_HEIGHT+0, BOX_WIDTH, 30), "Dark Storm")){
						SUNMASTER.Weather = SkyMasterManager.Weather_types.HeavyStormDark;
						SUNMASTER.On_demand = true;
					}

					if (GUI.Button(new Rect(4*BOX_WIDTH+10, BOX_HEIGHT-30, BOX_WIDTH, 30), "Lightning")){
						SUNMASTER.Weather = SkyMasterManager.Weather_types.LightningStorm;
						SUNMASTER.On_demand = true;
					}

					if(!enable_controls){

						if (GUI.Button(new Rect(1*BOX_WIDTH+10, BOX_HEIGHT-30, BOX_WIDTH, 30), "Volume Fog")){
							SUNMASTER.Weather = SkyMasterManager.Weather_types.RollingFog;
							SUNMASTER.On_demand = true;
						}

						if (GUI.Button(new Rect(1*BOX_WIDTH+10, BOX_HEIGHT+0, BOX_WIDTH+0, 30), "Tornado")){
							SUNMASTER.Weather = SkyMasterManager.Weather_types.Tornado;
							SUNMASTER.On_demand = true;
						}

					}

//					if (GUI.Button(new Rect(6*BOX_WIDTH+10, BOX_HEIGHT+0, BOX_WIDTH+10, 30), "Volcano Erupt")){
//						SUNMASTER.Weather = SkyMasterManager.Weather_types.VolcanoErupt;
//						SUNMASTER.On_demand = true;
//					}
	
			//DOME CONTROL
					if(!enable_controls){
			GUI.TextArea( new Rect(1*BOX_WIDTH+10, BOX_HEIGHT+30, BOX_WIDTH+0, 25),"SkyDome rot");
			Dome_rot = GUI.HorizontalSlider(new Rect(1*BOX_WIDTH+10, BOX_HEIGHT+30+30, BOX_WIDTH+0, 30),Dome_rot,0,10);
			SUNMASTER.Horizontal_factor = Dome_rot;
					}

			if (GUI.Button(new Rect(5, 0*BOX_HEIGHT+20, BOX_WIDTH+5, 20), "Enable Controls")){
				if(enable_controls){
					enable_controls = false;
					SUNMASTER.Auto_Cycle_Sky = true;
				}else{
					enable_controls = true;
					SUNMASTER.Auto_Cycle_Sky = false;
				}			
			}

		BOX_HEIGHT = BOX_HEIGHT+20;		
		float BOX_offset = 50;

		GUI.TextArea( new Rect(5, 5*BOX_HEIGHT+BOX_offset, 100, 20),"Speed");
		SPEED = GUI.HorizontalSlider(new Rect(10, 5*BOX_HEIGHT+BOX_offset+30, 100, 30),SPEED,0.01f,70f);
		SUNMASTER.SPEED = SPEED;

					//v1.7
					GUI.TextArea( new Rect(11+100, 5*BOX_HEIGHT+BOX_offset, 50, 20),"Horizon");
					HORIZON = GUI.HorizontalSlider(new Rect(11+100, 5*BOX_HEIGHT+BOX_offset+30, 50, 30),HORIZON,90f,170f);
					SUNMASTER.Horizon_adj = HORIZON;

					GUI.TextArea( new Rect(11+100, 6*BOX_HEIGHT+BOX_offset, 50, 20),"DuskG");
					DuskLightG = GUI.HorizontalSlider(new Rect(11+100, 6*BOX_HEIGHT+BOX_offset+30, 50, 30),DuskLightG,0f,0.5f);
					SUNMASTER.Dusk_Sun_Color.g = DuskLightG;

					GUI.TextArea( new Rect(11+100, 7*BOX_HEIGHT+BOX_offset, 50, 20),"DuskB");
					DuskLightB = GUI.HorizontalSlider(new Rect(11+100, 7*BOX_HEIGHT+BOX_offset+30, 50, 30),DuskLightB,0f,0.5f);
					SUNMASTER.Dusk_Sun_Color.b = DuskLightB;


		if(enable_controls){

			GUI.TextArea( new Rect(5, 0*BOX_HEIGHT+BOX_offset+5, 130, 20),"Increase HDR brightness");
			HDR = GUI.HorizontalSlider(new Rect(10, 0*BOX_HEIGHT+BOX_offset+30, 150, 30),HDR,0.05f,3f);
			SUNMASTER.m_fExposure = HDR;

			GUI.TextArea( new Rect(5, 1*BOX_HEIGHT+BOX_offset, 130, 20),"Esun");
			Esun = GUI.HorizontalSlider(new Rect(10, 1*BOX_HEIGHT+BOX_offset+30, 150, 30),Esun,0.9f,80f);
			SUNMASTER.m_ESun = Esun;

			GUI.TextArea( new Rect(5, 2*BOX_HEIGHT+BOX_offset, 130, 20),"Kr - White to Red factor");
			Kr = GUI.HorizontalSlider(new Rect(10, 2*BOX_HEIGHT+BOX_offset+30, 150, 30),Kr,0.0001f,0.014f);
			SUNMASTER.m_Kr = Kr;

			GUI.TextArea( new Rect(5, 3*BOX_HEIGHT+BOX_offset, 130, 20),"Km - Vertical effect factor");
			Km = GUI.HorizontalSlider(new Rect(10, 3*BOX_HEIGHT+BOX_offset+30, 150, 30),Km,0.0003f,0.1195f);
			SUNMASTER.m_Km = Km;

			GUI.TextArea( new Rect(5, 4*BOX_HEIGHT+BOX_offset, 130, 20),"G - Focus factor");
			GE = GUI.HorizontalSlider(new Rect(10, 4*BOX_HEIGHT+BOX_offset+30, 150, 30),GE,-0.69f,-0.9999f);
			SUNMASTER.m_g = GE;

			GUI.TextArea( new Rect(5, 6*BOX_HEIGHT+BOX_offset, 100, 20),"Sun Ring factor");
			Ring_factor = GUI.HorizontalSlider(new Rect(10, 6*BOX_HEIGHT+BOX_offset+30, 100, 30),Ring_factor,0f,0.15f);
			SUNMASTER.Sun_ring_factor = Ring_factor;

			GUI.TextArea( new Rect(5, 7*BOX_HEIGHT+BOX_offset, 100, 20),"Samples");
			fSamples = GUI.HorizontalSlider(new Rect(10, 7*BOX_HEIGHT+BOX_offset+30, 100, 30),fSamples,1,4);
			SUNMASTER.m_fSamples = fSamples;

			GUI.TextArea( new Rect(5, 8*BOX_HEIGHT+BOX_offset, 130, 20),"Scale depth");
			fScaleDepth = GUI.HorizontalSlider(new Rect(10, 8*BOX_HEIGHT+BOX_offset+30, 150, 30),fScaleDepth,0.1f,2);
			SUNMASTER.m_fRayleighScaleDepth = fScaleDepth;

			///////////// COLORS	
			float BASE_X = 50;
			float BASE1= 50;
			GUI.TextArea( new Rect(BASE_X+130, BASE1+(3.5f*40)-20+70, 180, 20),"Tint: "+CURRENT_Force_color);
			SUNMASTER.m_fWaveLength.x = GUI.HorizontalSlider(new Rect(BASE_X+130, BASE1+(3.5f*40)+70, 100, 30),SUNMASTER.m_fWaveLength.x,0,1);
			CURRENT_Force_color.x = SUNMASTER.m_fWaveLength.x;
			SUNMASTER.m_fWaveLength.y = GUI.HorizontalSlider(new Rect(BASE_X+130, BASE1+(4.3f*40)+70, 100, 30),SUNMASTER.m_fWaveLength.y,0,1);
			CURRENT_Force_color.y = SUNMASTER.m_fWaveLength.y;
			SUNMASTER.m_fWaveLength.z = GUI.HorizontalSlider(new Rect(BASE_X+130, BASE1+(5.1f*40)+70, 100, 30),SUNMASTER.m_fWaveLength.z,0,1);
			CURRENT_Force_color.z = SUNMASTER.m_fWaveLength.z;

			BASE_X = 50;
			BASE1= 50+100;
			GUI.TextArea( new Rect(BASE_X+130, BASE1+(3.5f*40)-20+70, 180, 20),"Global tint: "+TintColor);
			SUNMASTER.m_TintColor.r = GUI.HorizontalSlider(new Rect(BASE_X+130, BASE1+(3.5f*40)+70, 100, 30),SUNMASTER.m_TintColor.r,0,1);
			TintColor.x = SUNMASTER.m_TintColor.r;
			SUNMASTER.m_TintColor.g = GUI.HorizontalSlider(new Rect(BASE_X+130, BASE1+(4.3f*40)+70, 100, 30),SUNMASTER.m_TintColor.g,0,1);
			TintColor.y = SUNMASTER.m_TintColor.g;
			SUNMASTER.m_TintColor.b = GUI.HorizontalSlider(new Rect(BASE_X+130, BASE1+(5.1f*40)+70, 100, 30),SUNMASTER.m_TintColor.b,0,1);
			TintColor.z = SUNMASTER.m_TintColor.b;

			GUI.TextArea(new Rect(BASE_X+130, BASE1+(5.1f*40)+70+30, 100, 30),"Coloration");
			Coloration = GUI.HorizontalSlider(new Rect(BASE_X+130, BASE1+(5.1f*40)+70+30+30, 100, 30),Coloration,0.1f,2);
			SUNMASTER.m_Coloration = Coloration;
		}	
	}else{ // IF AUTO SEASON CYCLE
			string Season_current1 = "Spring";
			if(SUNMASTER.Season == 2){
				Season_current1= "Summer";
			}
			if(SUNMASTER.Season == 3){
				Season_current1= "Autumn";
			}
			if(SUNMASTER.Season == 4){
				Season_current1= "Winter";
			}
				GUI.TextArea(new Rect(5*BOX_WIDTH+10, BOX_HEIGHT+30+30, BOX_WIDTH, 30),Season_current1);
	  

	}
			}//END GI CHECK
		}
  }// END OnGUI	
}
}