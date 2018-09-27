using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBoat : MonoBehaviour {
	private Vector3 boatPos; //Boat actual position
	private Vector3 islandPos; //Boat island position
	private Vector3 toMove; //Where to move in itween
	private GameObject eventSystem; //Eventsystem for activating and deactivating purposes
	// Use this for initialization
	void Start () {
		boatPos = gameObject.transform.position;
		islandPos = new Vector3 (-77.3f, 0.344f, -8.55f);
		eventSystem = GameObject.Find ("GvrEventSystem");
	}
	
	// Update is called once per frame
	void Update () {
		
	} 
	//Function to start movement by moving player to boat
	public void MoveTheBoat(GameObject player) {
		eventSystem.SetActive (false);
		iTween.MoveTo (player, iTween.Hash ("position", gameObject.transform.position + new Vector3 (0f, 1f, 0f), "time", 3f, "oncomplete", "MoveForReal","oncompletetarget",gameObject,"oncompleteparams",player));
	}
	//Function to set up rotation and combine player and boat and start movement
	void MoveForReal(GameObject player) {
		if (gameObject.transform.position == boatPos) {
			iTween.RotateTo (gameObject, new Vector3 (0f, 133f, 0f), 15f);
			toMove = islandPos;
		} else {
			iTween.RotateTo (gameObject, new Vector3 (0f, -90f, 0f), 15f);
			toMove = boatPos;
		}
		player.transform.SetParent (gameObject.transform);
		iTween.MoveTo (gameObject, iTween.Hash ("position", toMove , "speed", 4f, "oncomplete", "RemoveChildren", "oncompletetarget", gameObject,"oncompleteparams",player));
	}
	//Function to remove player as boat child
	void RemoveChildren(GameObject player) {
		player.transform.parent=null;
		eventSystem.SetActive (true);
	}
}
