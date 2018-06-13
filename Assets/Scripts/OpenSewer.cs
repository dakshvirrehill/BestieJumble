using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSewer : MonoBehaviour {
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OpenTheSewer() {
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
			iTween.MoveTo (gameObject, iTween.Hash ("position", new Vector3(12.62f,0f,6.93f), "time", 3f, "islocal", true, "oncompletetarget", gameObject, "oncomplete", "BringCubeUp", "oncompleteparams", hitColliders [i].gameObject));
		} else {
			
		}
	}
	public void BringCubeUp(GameObject PuzzleCube) {
		iTween.MoveTo(PuzzleCube,iTween.Hash("position",PuzzleCube.transform.position+new Vector3(0f,1.28f,0f),"time",3f,"oncompletetarget", gameObject, "oncomplete", "CloseSewer"));
	}
	public void CloseSewer() {
		iTween.MoveTo (gameObject, iTween.Hash ("position", new Vector3 (10.42f,0f,6.93f), "time", 3f, "islocal", true));
	}
}
