using UnityEngine;
using System.Collections;
using Artngame.GIPROXY;

namespace Artngame.SKYMASTER {

public class SKYMASTER_Demo2 : MonoBehaviour {

	#pragma warning disable 414

	void Start () {

		if(SKYMASTER_OBJ!=null){
				SUNMASTER = SKYMASTER_OBJ.GetComponent(typeof(SkyMasterManager)) as SkyMasterManager;
		}
		SPEED = SUNMASTER.SPEED;

			SUNMASTER.Seasonal_change_auto = false;

			GI_controller = SUNMASTER.SUN_LIGHT.GetComponent(typeof(ControlGIPROXY_ATTRIUM_SM)) as ControlGIPROXY_ATTRIUM_SM;

			if(GI_controller!=null){
				GI_controller.enabled = false;
			}
	}

	ControlGIPROXY_ATTRIUM_SM GI_controller;

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

	float fSamples = 3;
	float fScaleDepth = 0.5f;

	bool set_sun_start=false;
	float Ring_factor=0;

	Vector3 CURRENT_Force_color = new Vector3(0.65f,0.52f,0.475f);
	float Coloration = 0.28f;
	Vector4 TintColor = new Vector4(0,0,0,0);

	bool enable_controls=false;

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

	void OnGUI() {

			float BOX_WIDTH = 100;float BOX_HEIGHT = 30;

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

			GI_controls_on = false;
		if(GI_controls_on){
				//let GI controller take over GUI
		}
		else{
					if (GUI.Button(new Rect(5*BOX_WIDTH+10, BOX_HEIGHT+0, BOX_WIDTH, 30), "Sunny")){
						SUNMASTER.Weather = SkyMasterManager.Weather_types.Sunny;
						SUNMASTER.On_demand = true;
					}

				Auto_Season_Cycle=false;
			if(!Auto_Season_Cycle){
		
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
					if (GUI.Button(new Rect(2*BOX_WIDTH+10, BOX_HEIGHT+60+30+25, BOX_WIDTH, 30), "Foggy")){
						SUNMASTER.Weather = SkyMasterManager.Weather_types.HeavyFog;
						SUNMASTER.On_demand = true;
					}

					if (GUI.Button(new Rect(3*BOX_WIDTH+10, BOX_HEIGHT-0, BOX_WIDTH, 30), "Snow storm")){
						SUNMASTER.Weather = SkyMasterManager.Weather_types.FreezeStorm;
						SUNMASTER.On_demand = true;
					}

					if (GUI.Button(new Rect(2*BOX_WIDTH+10, BOX_HEIGHT-30, BOX_WIDTH, 30), "Heavy Storm")){
						SUNMASTER.Weather = SkyMasterManager.Weather_types.HeavyStorm;
						SUNMASTER.On_demand = true;
					}

					if (GUI.Button(new Rect(3*BOX_WIDTH+10, BOX_HEIGHT-30, BOX_WIDTH, 30), "Dark Storm")){
						SUNMASTER.Weather = SkyMasterManager.Weather_types.HeavyStormDark;
						SUNMASTER.On_demand = true;
					}

					if (GUI.Button(new Rect(4*BOX_WIDTH+10, BOX_HEIGHT-30, BOX_WIDTH, 30), "Lightning")){
						SUNMASTER.Weather = SkyMasterManager.Weather_types.LightningStorm;
						SUNMASTER.On_demand = true;
					}

					if (GUI.Button(new Rect(5*BOX_WIDTH+10, BOX_HEIGHT-30, BOX_WIDTH, 30), "Volume Fog")){
						SUNMASTER.Weather = SkyMasterManager.Weather_types.RollingFog;
						SUNMASTER.On_demand = true;
					}

					if (GUI.Button(new Rect(6*BOX_WIDTH+10, BOX_HEIGHT-30, BOX_WIDTH+10, 30), "Tornado")){
						SUNMASTER.Weather = SkyMasterManager.Weather_types.Tornado;
						SUNMASTER.On_demand = true;
					}

					if (GUI.Button(new Rect(6*BOX_WIDTH+10, BOX_HEIGHT+0, BOX_WIDTH+10, 30), "Volcano Erupt")){
						SUNMASTER.Weather = SkyMasterManager.Weather_types.VolcanoErupt;
						SUNMASTER.On_demand = true;
					}
	
			//DOME CONTROL
			GUI.TextArea( new Rect(6*BOX_WIDTH+10, BOX_HEIGHT+30, BOX_WIDTH+10, 25),"SkyDome rot");
			Dome_rot = GUI.HorizontalSlider(new Rect(6*BOX_WIDTH+10, BOX_HEIGHT+30+30, BOX_WIDTH+10, 30),Dome_rot,0,10);
			SUNMASTER.Horizontal_factor = Dome_rot;


		if (GUI.Button(new Rect(5, 0*BOX_HEIGHT, 150, 20), "Enable Controls")){

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

		if(enable_controls){

			GUI.TextArea( new Rect(5, 0*BOX_HEIGHT+BOX_offset, 150, 20),"Increase HDR brightness");
			HDR = GUI.HorizontalSlider(new Rect(10, 0*BOX_HEIGHT+BOX_offset+30, 150, 30),HDR,0.05f,3f);
			SUNMASTER.m_fExposure = HDR;

			GUI.TextArea( new Rect(5, 1*BOX_HEIGHT+BOX_offset, 150, 20),"Esun");
			Esun = GUI.HorizontalSlider(new Rect(10, 1*BOX_HEIGHT+BOX_offset+30, 150, 30),Esun,0.9f,80f);
			SUNMASTER.m_ESun = Esun;

			GUI.TextArea( new Rect(5, 2*BOX_HEIGHT+BOX_offset, 150, 20),"Kr - White to Red factor");
			Kr = GUI.HorizontalSlider(new Rect(10, 2*BOX_HEIGHT+BOX_offset+30, 150, 30),Kr,0.0001f,0.014f);
			SUNMASTER.m_Kr = Kr;

			GUI.TextArea( new Rect(5, 3*BOX_HEIGHT+BOX_offset, 150, 20),"Km - Vertical effect factor");
			Km = GUI.HorizontalSlider(new Rect(10, 3*BOX_HEIGHT+BOX_offset+30, 150, 30),Km,0.0003f,0.1195f);
			SUNMASTER.m_Km = Km;

			GUI.TextArea( new Rect(5, 4*BOX_HEIGHT+BOX_offset, 150, 20),"G - Focus factor");
			GE = GUI.HorizontalSlider(new Rect(10, 4*BOX_HEIGHT+BOX_offset+30, 150, 30),GE,-0.69f,-0.9999f);
			SUNMASTER.m_g = GE;

			GUI.TextArea( new Rect(5, 6*BOX_HEIGHT+BOX_offset, 100, 20),"Sun Ring factor");
			Ring_factor = GUI.HorizontalSlider(new Rect(10, 6*BOX_HEIGHT+BOX_offset+30, 100, 30),Ring_factor,0f,0.15f);
			SUNMASTER.Sun_ring_factor = Ring_factor;

			GUI.TextArea( new Rect(5, 7*BOX_HEIGHT+BOX_offset, 150, 20),"Samples");
			fSamples = GUI.HorizontalSlider(new Rect(10, 7*BOX_HEIGHT+BOX_offset+30, 150, 30),fSamples,1,4);
			SUNMASTER.m_fSamples = fSamples;

			GUI.TextArea( new Rect(5, 8*BOX_HEIGHT+BOX_offset, 150, 20),"Scale depth");
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
				GUI.TextArea(new Rect(5*BOX_WIDTH+10, BOX_HEIGHT+30, BOX_WIDTH, 30),Season_current1);
	  

	}
			}//END GI CHECK
  }// END OnGUI	
}
}