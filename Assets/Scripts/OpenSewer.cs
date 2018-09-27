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
	//Check if puzzlecube is there and if it is found, move the sewer and start movement using itween
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
			iTween.MoveTo (gameObject, iTween.Hash ("position", new Vector3(-1284.13f,0f,-59.67f), "time", 3f, "islocal", true, "oncompletetarget", gameObject, "oncomplete", "BringCubeUp", "oncompleteparams", hitColliders [i].gameObject));
		} else {
			
		}
	}
	//Bring the cube up
	public void BringCubeUp(GameObject PuzzleCube) {
		iTween.MoveTo(PuzzleCube,iTween.Hash("position",PuzzleCube.transform.position+new Vector3(0f,1.28f,0f),"time",3f,"oncompletetarget", gameObject, "oncomplete", "CloseSewer"));
	}
	//Close the sewer
	public void CloseSewer() {
		iTween.MoveTo (gameObject, iTween.Hash ("position", new Vector3 (-1282.308f,0f,-59.67f), "time", 3f, "islocal", true));
	}
}
