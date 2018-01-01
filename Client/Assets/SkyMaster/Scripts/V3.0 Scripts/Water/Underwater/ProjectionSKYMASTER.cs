using UnityEngine;
using System.Collections;

public class ProjectionSKYMASTER : MonoBehaviour {

	Projector CausticProjector;
	public Texture2D[] Caustics;
	public float fps=30;
	float start_time;

	int currentFrame=0;

	// Use this for initialization
	void Start () {
		CausticProjector = GetComponent<Projector> ();
		start_time = Time.fixedTime;
	}
	
	// Update is called once per frame
	void Update () {

		if (Time.fixedTime - start_time > (1 / fps)) {

			CausticProjector.material.SetTexture ("_CausticTexture", Caustics [currentFrame]);
			currentFrame = currentFrame + 1;
			if(currentFrame > 30){
				currentFrame =0;
			}

			start_time = Time.fixedTime;
		}

	}
}
