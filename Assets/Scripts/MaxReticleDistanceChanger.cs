using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxReticleDistanceChanger : MonoBehaviour {
	private float angley; //Absolute value of rotation euler angles
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {//Setting reticle distance according to the rotation and position of camera
		if (transform.GetChild (0).rotation.eulerAngles.y > 180f) {
			angley = Mathf.Abs (transform.GetChild (0).rotation.eulerAngles.y - 360f);
		} else {
			angley = transform.GetChild (0).rotation.eulerAngles.y;
		}
		if (transform.position.y > 19f && (angley >= 0f && angley <= 50f)) {
			transform.GetChild (0).GetChild (0).gameObject.GetComponent<GvrReticlePointer> ().maxReticleDistance = 38f;
		} else {
			transform.GetChild (0).GetChild (0).gameObject.GetComponent<GvrReticlePointer> ().maxReticleDistance = 10f;
		}
	}
}
