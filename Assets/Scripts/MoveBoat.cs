using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBoat : MonoBehaviour {
	private Vector3 boatPos;
	private Vector3 islandPos;
	private Vector3[] toMove;
	private GameObject eventSystem;
	// Use this for initialization
	void Start () {
		toMove = new Vector3[2];
		toMove[0] = new Vector3(-36.87f,0.344f,-4.48f);
		boatPos = gameObject.transform.position;
		islandPos = new Vector3 (-77.3f, 0.344f, -8.55f);
		eventSystem = GameObject.Find ("GvrEventSystem");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void MoveTheBoat(GameObject player) {
		eventSystem.SetActive (false);
		iTween.MoveTo (player, iTween.Hash ("position", gameObject.transform.position + new Vector3 (0f, 1f, 0f), "time", 3f, "oncomplete", "MoveForReal","oncompletetarget",gameObject,"oncompleteparams",player));
	}
	void MoveForReal(GameObject player) {
		if (gameObject.transform.position == boatPos) {
			iTween.RotateTo (gameObject, new Vector3 (0f, 133f, 0f), 15f);
			toMove[1] = islandPos;
		} else {
			iTween.RotateTo (gameObject, new Vector3 (0f, -90f, 0f), 15f);
			toMove[1] = boatPos;
		}
		player.transform.SetParent (gameObject.transform);
		iTween.MoveTo (gameObject, iTween.Hash ("path", toMove , "time", 15f, "oncomplete", "RemoveChildren", "oncompletetarget", gameObject,"oncompleteparams",player));
	}
	void RemoveChildren(GameObject player) {
		player.transform.parent=null;
		eventSystem.SetActive (true);
	}
}
