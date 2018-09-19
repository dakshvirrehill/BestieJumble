using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTheCrate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OpenCrate() {
		Collider[] hitColliders = Physics.OverlapSphere (gameObject.transform.position, 0.65f);
		bool found = false;
		int i = 0;
		for (i = 0; i < hitColliders.Length; i++) {
			if (hitColliders [i].gameObject.tag == "PuzzleCube") {
				found = true;
				break;
			}
		}
		if (found) {
			iTween.RotateTo (transform.GetChild(1).gameObject, iTween.Hash ("rotation", new Vector3(90f,0f,0f), "time", 3f, "oncompletetarget", gameObject, "oncomplete", "BringCubeUp", "oncompleteparams", hitColliders [i].gameObject));
		} else {

		}
	}
	void BringCubeUp(GameObject PuzzleCube) {
		iTween.MoveTo(PuzzleCube,iTween.Hash("position",PuzzleCube.transform.position+new Vector3(0f,1.28f,0f),"time",3f,"oncompletetarget", gameObject, "oncomplete", "CloseCrate"));
	}
	void CloseCrate() {
		iTween.RotateTo (transform.GetChild(1).gameObject, new Vector3 (0f, 0f, 0f), 3f);
	}
}
