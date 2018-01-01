using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Artngame.GIPROXY;

namespace Artngame.SKYMASTER {

public class SKYMASTER_Volume_Clouds_Demo125 : MonoBehaviour {

	#pragma warning disable 414

//		public Transform target ;
//		public float damping = 6.0f;
//		public bool smooth = true;
//		bool enable_lookat=true;

	void Start () {
//			Sun_rotator = SUN.GetComponent(typeof(Circle_Around_ParticleSKYMASTER)) as Circle_Around_ParticleSKYMASTER;
//			Cam_rotator = Camera.main.gameObject.GetComponent(typeof(Circle_Around_ParticleSKYMASTER)) as Circle_Around_ParticleSKYMASTER;
//			MouseLOOK = Camera.main.gameObject.GetComponent(typeof(MouseLookSKYMASTER)) as MouseLookSKYMASTER;
			Cloud_instance = (GameObject)Instantiate(Clouds_top.gameObject, Clouds_top.transform.position,Quaternion.identity);

			Cloud_instance.SetActive(true);
			Clouds_ORIGIN = Clouds_top.gameObject.GetComponent(typeof(VolumeClouds_SM)) as VolumeClouds_SM;

//			Clouds_ORIGIN.max_bed_corner = Cloud_bed_width;
//			Clouds_ORIGIN.min_bed_corner = -Cloud_bed_width;
//			Clouds_ORIGIN.cloud_bed_heigh = Cloud_bed_height;
			Clouds_ORIGIN.divider = Cloud_divider;
			Clouds_ORIGIN.cloud_max_scale = cloud_max_scale;


			//Emitter_ORIGIN =  Clouds_top.gameObject.GetComponent(typeof(ParticleEmitter)) as ParticleEmitter;
		//	Emitter_ORIGIN.maxSize = cloud_max_particle_size;

			Cloud_instance.SetActive(false);

			Cloud_instance.SetActive(true);

			if(Cloud_instance!=null){
				Clouds = Cloud_instance.GetComponent(typeof(VolumeClouds_SM)) as VolumeClouds_SM;

			}else{
				Debug.Log ("AAA");
			}

			if(Clouds != null){
				cloud_X_speed = Clouds.wind.x;
				cloud_Z_speed = Clouds.wind.z;
			}

		}
//		Circle_Around_ParticleSKYMASTER Sun_rotator;
//		Circle_Around_ParticleSKYMASTER Cam_rotator;
//		MouseLookSKYMASTER MouseLOOK;
		VolumeClouds_SM Clouds;
		VolumeClouds_SM Clouds_ORIGIN;
		//ParticleEmitter Emitter_ORIGIN; //v3.4.6

		Light SunLight;

	public float Sun_time_start = 14.43f;	//


	public Transform SUN;
	

	public bool HUD_ON=true;

	
		public Transform Clouds_top;
	
		float Dome_rot;

		float SunLight_r;
			float SunLight_g;
				float SunLight_b;
		float Camera_up;
		float Sun_up;

		float sun_rot_speed;
		float cam_rot_speed;

		void LateUpdate () {

//			if(MouseLOOK.enabled){
//
//				enable_lookat = false;
//
//			}else{
//				enable_lookat = true;
//			}

//			if (target & enable_lookat) {
//				if (smooth)
//				{
//					// Look at and dampen the rotation
//					Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
//					transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
//				}
//				else
//				{
//					// Just lookat
//					transform.LookAt(target);
//				}
//			}

			//ROT CLOUDS
			if(Rot_clouds){
				if(Clouds!=null){
					Clouds.wind.x = 2*Mathf.Cos(Time.fixedTime*0.1f);
					Clouds.wind.z = 2*Mathf.Sin(Time.fixedTime*0.1f);
					Clouds.speed = 2f;
					Clouds.multiplier = 2;
				}
			}else{
				Clouds.speed = 0.5f;
				Clouds.multiplier = 1;
			}



		}

		int Cloud_divider=45;
		float Cloud_bed_height;
		float Cloud_bed_width;
		GameObject Cloud_instance;
		float cloud_max_scale=4;
		int cloud_max_particle_size=300;

		//v1.7
		float cloud_X_speed;
		float cloud_Z_speed;
		bool Plus_hit = false;

		bool Rot_clouds = false;

		//v1.7
		public List<Transform> CloudPresets;
		int count_presets = 0;
		bool Alt_color = false;
		public SunShaftsSkyMaster Shafts;
		public GlobalFogSkyMaster Fog;


	void OnGUI() {
			//SunLight = SUN.light;
			//float BOX_WIDTH = 100;float BOX_HEIGHT = 30;

			///////////// COLORS	
			float BASE_X = 0;
			float BASE1= 10;
			float widthS = Screen.currentResolution.width;

			widthS = Camera.main.pixelWidth;

			//Choose cloud preset
			GUI.TextArea( new Rect(widthS - (BASE_X+11*15)-0,-20+ 0*BASE1+(3.5f*40)+7+30+30+30+40+60+40, 120, 20),"Cloud Type: "+(count_presets+1).ToString()+"/"+CloudPresets.Count.ToString());
			//if (GUI.Button(new Rect(widthS - (BASE_X+11*15)-20, 0*BASE1+(3.5f*40)+7+30+30+30+40+60+40, 20, 20), "+")){
			if (GUI.Button(new Rect(125+widthS - (BASE_X+11*15)-0,-20+ 0*BASE1+(3.5f*40)+7+30+30+30+40+60+40, 20, 20), "+")){
				count_presets++;
				if(count_presets > CloudPresets.Count -1){
					count_presets=0;
				}
				if(CloudPresets[count_presets] != Clouds_top){
					Clouds_top = CloudPresets[count_presets];

					Clouds_ORIGIN = Clouds_top.gameObject.GetComponent(typeof(VolumeClouds_SM)) as VolumeClouds_SM;
					Cloud_bed_width = Clouds_ORIGIN.max_bed_corner;
					Cloud_bed_height = Clouds_ORIGIN.cloud_bed_heigh;
					Cloud_divider = Clouds_ORIGIN.divider;
					cloud_max_scale = Clouds_ORIGIN.cloud_max_scale;
					//cloud_max_particle_size = (int)Emitter_ORIGIN.maxSize;
				}
				Plus_hit = true;
			}
			//if (GUI.Button(new Rect(widthS - (BASE_X+11*15)-20, 0*BASE1+(3.5f*40)+7+30+30+30+40+60+40 +20, 20, 20), "-")){
			if (GUI.Button(new Rect(20+125+widthS - (BASE_X+11*15)-0,-20+ 0*BASE1+(3.5f*40)+7+30+30+30+40+60+40, 20, 20), "-")){
				count_presets--;
				if(count_presets < 0){
					count_presets=CloudPresets.Count-1;
				}
				if(CloudPresets[count_presets] != Clouds_top){
					Clouds_top = CloudPresets[count_presets];

					Clouds_ORIGIN = Clouds_top.gameObject.GetComponent(typeof(VolumeClouds_SM)) as VolumeClouds_SM;
					Cloud_bed_width = Clouds_ORIGIN.max_bed_corner;
					Cloud_bed_height = Clouds_ORIGIN.cloud_bed_heigh;
					Cloud_divider = Clouds_ORIGIN.divider;
					cloud_max_scale = Clouds_ORIGIN.cloud_max_scale;
					//cloud_max_particle_size = (int)Emitter_ORIGIN.maxSize;
				}
				Plus_hit = true;
			}


			//v1.7
			GUI.TextArea( new Rect(widthS - (BASE_X+11*15), 0*BASE1+(3.5f*40)+7+30+30+30+30+10  +20, 50, 20),"Clear");			
			if (GUI.Button(new Rect(widthS - (BASE_X+11*15) + 50, 0*BASE1+(3.5f*40)+7+30+30+30+30+10  +20, 50, 20), "Clouds")){
				Destroy (Cloud_instance);
			}
			GUI.TextArea( new Rect(widthS - (BASE_X+11*15), 0*BASE1+(3.5f*40)+7+30+30+30+30+10, 50, 20),"Toggle");			
			if (GUI.Button(new Rect(widthS - (BASE_X+11*15) + 50, 0*BASE1+(3.5f*40)+7+30+30+30+30+10, 50, 20), "Haze")){
				if(Fog.enabled){
					Fog.enabled = false;
				}else{
					Fog.enabled = true;
				}
			}
			if (GUI.Button(new Rect(widthS - (BASE_X+11*15) + 50+50, 0*BASE1+(3.5f*40)+7+30+30+30+30+10, 50, 20), "Shafts")){
				if(Shafts.enabled){
					Shafts.enabled = false;
				}else{
					Shafts.enabled = true;
				}
			}


			//RESTART EFFECT !!!!
			if (GUI.Button(new Rect(widthS - (BASE_X+11*15), 0*BASE1+(3.5f*40)+7+30+30+30+40+60+40, 170, 40), "Recreate Clouds") | Plus_hit){
				Destroy (Cloud_instance);
				//Object INSTA = Instantiate(Clouds_top);

				Plus_hit = false;

				Clouds_ORIGIN = Clouds_top.gameObject.GetComponent(typeof(VolumeClouds_SM)) as VolumeClouds_SM;			
			
				//Clouds_ORIGIN.divider = Cloud_divider;
				//Clouds_ORIGIN.cloud_max_scale = cloud_max_scale;

				Clouds_ORIGIN.max_bed_corner = Cloud_bed_width;
				Clouds_ORIGIN.min_bed_corner = 0;
				Clouds_ORIGIN.cloud_bed_heigh = Cloud_bed_height;
				Clouds_ORIGIN.divider = Cloud_divider;
				Clouds_ORIGIN.cloud_max_scale = cloud_max_scale;
				//Emitter_ORIGIN.maxSize = cloud_max_particle_size;
				
				Cloud_instance = (GameObject)Instantiate(Clouds_top.gameObject, Clouds_top.transform.position,Quaternion.identity);
				Cloud_instance.SetActive(true);
				Clouds = Cloud_instance.GetComponent(typeof(VolumeClouds_SM)) as VolumeClouds_SM;

				if(Alt_color){
					Clouds.Override_init_color = true;
				}
			}

			//Destroy (
			GUI.TextArea( new Rect(widthS - (BASE_X+11*15), 0*BASE1+(3.5f*40)+7-30-15-15, 180, 25),"Cloud Centers = "+Clouds_ORIGIN.divider);
			Cloud_divider = (int)GUI.HorizontalSlider(new Rect(widthS - (BASE_X+11*15), 0*BASE1+(3.5f*40)+7-15-15, 100, 30),Clouds_ORIGIN.divider,2,85);

			GUI.TextArea( new Rect(widthS - (BASE_X+11*15), 0*BASE1+(3.5f*40)+7-30-60-15, 180, 25),"Cloud Bed size = "+Clouds_ORIGIN.max_bed_corner);
			Cloud_bed_width = GUI.HorizontalSlider(new Rect(widthS - (BASE_X+11*15), 0*BASE1+(3.5f*40)+7-60-15, 100, 30),Clouds_ORIGIN.max_bed_corner,300,8900);

			GUI.TextArea( new Rect(widthS - (BASE_X+11*15), 0*BASE1+(3.5f*40)+7-30-60-60, 180, 25),"Cloud Bed Height = "+Clouds_ORIGIN.cloud_bed_heigh);
			Cloud_bed_height = GUI.HorizontalSlider(new Rect(widthS - (BASE_X+11*15), 0*BASE1+(3.5f*40)+7-60-60-0, 100, 30),Clouds_ORIGIN.cloud_bed_heigh,50,1000);

			GUI.TextArea( new Rect(widthS - (BASE_X+11*15), 0*BASE1+(3.5f*40)+7+30-15-15-15, 180, 25),"Cloud Max size = "+Clouds_ORIGIN.cloud_max_scale);
			cloud_max_scale = GUI.HorizontalSlider(new Rect(widthS - (BASE_X+11*15), 0*BASE1+(3.5f*40)+7+30+30-15-15-15, 100, 30),Clouds_ORIGIN.cloud_max_scale,1,8);


		//	GUI.TextArea( new Rect(widthS - (BASE_X+11*15), 0*BASE1+(3.5f*40)+7+30+30+30-15-15-15-15, 180, 25),"Cloud Particle size = "+Emitter_ORIGIN.maxSize);
		//	cloud_max_particle_size = (int)GUI.HorizontalSlider(new Rect(widthS - (BASE_X+11*15), 0*BASE1+(3.5f*40)+7+30+30+30+30-15-15-15-15, 100, 30),Emitter_ORIGIN.maxSize,200,300);

			GUI.TextArea( new Rect(widthS - (BASE_X+11*15), 0*BASE1+(3.5f*40)+7+30+30+30-15-15-15-15 +20+22, 180, 22),"Cloud speed");
			cloud_X_speed = (int)GUI.HorizontalSlider(new Rect(widthS - (BASE_X+11*15), 0*BASE1+(3.5f*40)+7+30+30+30+30-15-15-15-15 +35+2, 100, 20),Clouds.wind.x,-70,70);
			Clouds.wind.x = cloud_X_speed;
			cloud_Z_speed = (int)GUI.HorizontalSlider(new Rect(widthS - (BASE_X+11*15), 0*BASE1+(3.5f*40)+7+30+30+30+30-15-15-15-15 +25+35, 100, 20),Clouds.wind.z,-70,70);
			Clouds.wind.z = cloud_Z_speed;


			GUI.TextArea( new Rect(widthS - (BASE_X+11*15), 0*BASE1+(3.5f*40)+7+30+30+30+30+30+20, 170, 20),"Recreate renews the clouds");

			if (GUI.Button(new Rect(widthS - (BASE_X+11*15), 0*BASE1+(3.5f*40)+7+30+30+30+40+60, 170, 20), "Rotate Clouds")){
				if(Rot_clouds){
					Rot_clouds = false;
				}else{
					Rot_clouds = true;
				}
			}			

			//v1.7 - Alternate base color
			string Alt_color1 = "Alt color";
			if(Alt_color){
				Alt_color1 = "Prev color";
			}
			if (GUI.Button(new Rect(widthS - (BASE_X+11*15), 8*BASE1+(3.5f*40)+7+30+30+30+40+60, 170, 40), Alt_color1)){
				if(Alt_color){
					Alt_color = false;
				}else{
					Alt_color = true;
				}
			}

			Clouds_ORIGIN.max_bed_corner = Cloud_bed_width;
			Clouds_ORIGIN.min_bed_corner = -Cloud_bed_width;
			Clouds_ORIGIN.cloud_bed_heigh = Cloud_bed_height;
			Clouds_ORIGIN.divider = Cloud_divider;
			Clouds_ORIGIN.cloud_max_scale = cloud_max_scale;
		//	Emitter_ORIGIN.maxSize = cloud_max_particle_size;
			
//			if (GUI.Button(new Rect(0*BOX_WIDTH+10, BOX_HEIGHT-0, BOX_WIDTH+20, 30), "Rotate Camera")){
//				if(!Cam_rotator.enabled){
//					Cam_rotator.enabled = true;
//				}else{
//					Cam_rotator.enabled = false;
//				}
//			}
//
//			if (GUI.Button(new Rect(0*BOX_WIDTH+10, BOX_HEIGHT+30, BOX_WIDTH+20, 30), "Rotate Sun")){
//				if(!Sun_rotator.enabled){
//					Sun_rotator.enabled = true;
//				}else{
//					Sun_rotator.enabled = false;
//				}
//			}
	
//			//CAMERA 
//			GUI.TextArea( new Rect(3*BOX_WIDTH+10, 2*BOX_HEIGHT+30, BOX_WIDTH+10, 25),"Camera up/down");
//
//			Camera_up = GUI.HorizontalSlider(new Rect(3*BOX_WIDTH+10, 2*BOX_HEIGHT+30+30, BOX_WIDTH+10, 30  ),Camera.main.transform.position.y ,100,1560);
//
//			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,Camera_up,Camera.main.transform.position.z);
//
//			//SUN
//			GUI.TextArea( new Rect(0*BOX_WIDTH+10, 7*BOX_HEIGHT+30, BOX_WIDTH+10, 25),"Sun up/down");
//			Sun_up = GUI.VerticalSlider(new Rect(1*BOX_WIDTH+10, 2*BOX_HEIGHT+30+30, 30, BOX_WIDTH+10 ),SUN.position.y ,-1500,1500);
//			SUN.position = new Vector3(SUN.position.x,Sun_up,SUN.position.z);	  

  }// END OnGUI	
}
}