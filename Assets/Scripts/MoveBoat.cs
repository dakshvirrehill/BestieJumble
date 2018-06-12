﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBoat : MonoBehaviour {
	private Vector3 boatPos;
	private Vector3 islandPos;
	// Use this for initialization
	void Start () {
		boatPos = gameObject.transform.position;
		islandPos = new Vector3 (-77.3f, 0.344f, -8.55f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void MoveTheBoat(GameObject player) {
		iTween.MoveTo (player, iTween.Hash ("position", gameObject.transform.position + new Vector3 (0f, 1f, 0f), "time", 3f, "oncomplete", "MoveForReal","oncompletetarget",gameObject,"oncompleteparams",player));
	}
	void MoveForReal(GameObject player) {
		GameObject parentMover = new GameObject ();
		parentMover.transform.position = gameObject.transform.position;
		player.transform.SetParent (parentMover.transform);
		gameObject.transform.SetParent (parentMover.transform);
		Vector3 toMove;
		if (gameObject.transform.position == boatPos) {
			toMove = islandPos;
		} else {
			toMove = boatPos;
		}
		iTween.MoveTo (parentMover, iTween.Hash ("position", toMove, "time", 15f, "oncomplete", "RemoveChildren", "oncompletetarget", gameObject, "oncompleteparams", parentMover,"orienttopath",true));
	}
	void RemoveChildren(GameObject parentMover) {
		parentMover.transform.DetachChildren ();
		Destroy (parentMover);
	}
}
