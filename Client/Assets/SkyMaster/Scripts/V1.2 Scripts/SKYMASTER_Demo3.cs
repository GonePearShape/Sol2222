using UnityEngine;
using System.Collections;
using Artngame.GIPROXY;

namespace Artngame.SKYMASTER {

public class SKYMASTER_Demo3 : MonoBehaviour {

	#pragma warning disable 414

		public Transform target ;
		public float damping = 6.0f;
		public bool smooth = true;
		bool enable_lookat=true;

	void Start () {
			Sun_rotator = SUN.GetComponent(typeof(Circle_Around_ParticleSKYMASTER)) as Circle_Around_ParticleSKYMASTER;
			Cam_rotator = Camera.main.gameObject.GetComponent(typeof(Circle_Around_ParticleSKYMASTER)) as Circle_Around_ParticleSKYMASTER;
			MouseLOOK = Camera.main.gameObject.GetComponent(typeof(MouseLookSKYMASTER)) as MouseLookSKYMASTER;
			Cloud_instance = (GameObject)Instantiate(Clouds_top.gameObject);

			Cloud_instance.SetActive(true);
			Clouds_ORIGIN = Clouds_top.gameObject.GetComponent(typeof(VolumeClouds_SM)) as VolumeClouds_SM;
			//Emitter_ORIGIN =  Clouds_top.gameObject.GetComponent(typeof(ParticleEmitter)) as ParticleEmitter;
			Cloud_instance.SetActive(false);

			Cloud_instance.SetActive(true);

			if(Cloud_instance!=null){
				Clouds = Cloud_instance.GetComponent(typeof(VolumeClouds_SM)) as VolumeClouds_SM;

			}else{
				Debug.Log ("AAA");
			}
		}
		Circle_Around_ParticleSKYMASTER Sun_rotator;
		Circle_Around_ParticleSKYMASTER Cam_rotator;
		MouseLookSKYMASTER MouseLOOK;
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

			if(MouseLOOK.enabled){

				enable_lookat = false;

			}else{
				enable_lookat = true;
			}

			if (target & enable_lookat) {
				if (smooth)
				{
					// Look at and dampen the rotation
					Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
					transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
				}
				else
				{
					// Just lookat
					transform.LookAt(target);
				}
			}

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

		int Cloud_divider;
		float Cloud_bed_height;
		float Cloud_bed_width;
		GameObject Cloud_instance;
		float cloud_max_scale;
		int cloud_max_particle_size;

		bool Rot_clouds = false;

	void OnGUI() {
			SunLight = SUN.GetComponent<Light>();
			float BOX_WIDTH = 100;float BOX_HEIGHT = 30;

			///////////// COLORS	
			float BASE_X = 0;
			float BASE1= 10;
			float widthS = Screen.currentResolution.width;

			widthS = Camera.main.pixelWidth;

			//RESTART EFFECT !!!!
			if (GUI.Button(new Rect(0*BOX_WIDTH+10, BOX_HEIGHT-30, BOX_WIDTH+20, 30), "Recreate")){
				Destroy (Cloud_instance);
				//Object INSTA = Instantiate(Clouds_top);

				Clouds_ORIGIN.max_bed_corner = Cloud_bed_width;
				Clouds_ORIGIN.min_bed_corner = -Cloud_bed_width;
				Clouds_ORIGIN.cloud_bed_heigh = Cloud_bed_height;
				Clouds_ORIGIN.divider = Cloud_divider;
				Clouds_ORIGIN.cloud_max_scale = cloud_max_scale;
				//Emitter_ORIGIN.maxSize = cloud_max_particle_size;
				
				Cloud_instance = (GameObject)Instantiate(Clouds_top.gameObject);
				Cloud_instance.SetActive(true);
				Clouds = Cloud_instance.GetComponent(typeof(VolumeClouds_SM)) as VolumeClouds_SM;

			}

			//Destroy (
			GUI.TextArea( new Rect(widthS - (BASE_X+11*15), 0*BASE1+(3.5f*40)+7-30, 180, 30),"Cloud Centers = "+Clouds_ORIGIN.divider);
			Cloud_divider = (int)GUI.HorizontalSlider(new Rect(widthS - (BASE_X+11*15), 0*BASE1+(3.5f*40)+7, 100, 30),Clouds_ORIGIN.divider,2,45);

			GUI.TextArea( new Rect(widthS - (BASE_X+11*15), 0*BASE1+(3.5f*40)+7-30-60, 180, 30),"Cloud Bed size = "+Clouds_ORIGIN.max_bed_corner);
			Cloud_bed_width = GUI.HorizontalSlider(new Rect(widthS - (BASE_X+11*15), 0*BASE1+(3.5f*40)+7-60, 100, 30),Clouds_ORIGIN.max_bed_corner,300,1500);

			GUI.TextArea( new Rect(widthS - (BASE_X+11*15), 0*BASE1+(3.5f*40)+7-30-60-60, 180, 30),"Cloud Bed Height = "+Clouds_ORIGIN.cloud_bed_heigh);
			Cloud_bed_height = GUI.HorizontalSlider(new Rect(widthS - (BASE_X+11*15), 0*BASE1+(3.5f*40)+7-60-60, 100, 30),Clouds_ORIGIN.cloud_bed_heigh,500,1000);

			GUI.TextArea( new Rect(widthS - (BASE_X+11*15), 0*BASE1+(3.5f*40)+7+30, 180, 30),"Cloud Max size = "+Clouds_ORIGIN.cloud_max_scale);
			cloud_max_scale = GUI.HorizontalSlider(new Rect(widthS - (BASE_X+11*15), 0*BASE1+(3.5f*40)+7+30+30, 100, 30),Clouds_ORIGIN.cloud_max_scale,1,4);


			//GUI.TextArea( new Rect(widthS - (BASE_X+11*15), 0*BASE1+(3.5f*40)+7+30+30+30, 180, 30),"Cloud Particle size = "+Emitter_ORIGIN.maxSize);
			//cloud_max_particle_size = (int)GUI.HorizontalSlider(new Rect(widthS - (BASE_X+11*15), 0*BASE1+(3.5f*40)+7+30+30+30+30, 100, 30),Emitter_ORIGIN.maxSize,200,300);

			GUI.TextArea( new Rect(widthS - (BASE_X+11*15), 0*BASE1+(3.5f*40)+7+30+30+30+30+30, 170, 40),"Recreate button will renew the clouds");

			if (GUI.Button(new Rect(widthS - (BASE_X+11*15), 0*BASE1+(3.5f*40)+7+30+30+30+40+60, 170, 40), "Rotate Clouds")){
				if(Rot_clouds){
					Rot_clouds = false;
				}else{
					Rot_clouds = true;
				}
			}			

			Clouds_ORIGIN.max_bed_corner = Cloud_bed_width;
			Clouds_ORIGIN.min_bed_corner = -Cloud_bed_width;
			Clouds_ORIGIN.cloud_bed_heigh = Cloud_bed_height;
			Clouds_ORIGIN.divider = Cloud_divider;
			Clouds_ORIGIN.cloud_max_scale = cloud_max_scale;
			//Emitter_ORIGIN.maxSize = cloud_max_particle_size;
			
			if (GUI.Button(new Rect(0*BOX_WIDTH+10, BOX_HEIGHT-0, BOX_WIDTH+20, 30), "Rotate Camera")){
				if(!Cam_rotator.enabled){
					Cam_rotator.enabled = true;
				}else{
					Cam_rotator.enabled = false;
				}
			}

			if (GUI.Button(new Rect(0*BOX_WIDTH+10, BOX_HEIGHT+30, BOX_WIDTH+20, 30), "Rotate Sun")){
				if(!Sun_rotator.enabled){
					Sun_rotator.enabled = true;
				}else{
					Sun_rotator.enabled = false;
				}
			}
	
			//CAMERA 
			GUI.TextArea( new Rect(0*BOX_WIDTH+10, 2*BOX_HEIGHT+30, BOX_WIDTH+10, 25),"Camera up/down");

			Camera_up = GUI.VerticalSlider(new Rect(0*BOX_WIDTH+10, 2*BOX_HEIGHT+30+30, 30, BOX_WIDTH+10 ),Camera.main.transform.position.y ,-1300,1360);

			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,Camera_up,Camera.main.transform.position.z);

			//SUN
			GUI.TextArea( new Rect(0*BOX_WIDTH+10, 7*BOX_HEIGHT+30, BOX_WIDTH+10, 25),"Sun up/down");
			Sun_up = GUI.VerticalSlider(new Rect(1*BOX_WIDTH+10, 2*BOX_HEIGHT+30+30, 30, BOX_WIDTH+10 ),SUN.position.y ,-1500,1500);
			SUN.position = new Vector3(SUN.position.x,Sun_up,SUN.position.z);	  

  }// END OnGUI	
}
}