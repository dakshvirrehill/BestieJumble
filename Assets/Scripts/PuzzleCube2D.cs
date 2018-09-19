﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class PuzzleCube2D : MonoBehaviour {
	public Vector2 currentPos;
	public Vector2 actualPos;
	public int currentPosVR;
	public int actualPosVR;
	public int ngv;
	public bool isCorrect;
	public int selectedPos;
	AudioSource Winning;
	// Use this for initialization
	void Start () { 
		gameObject.AddComponent<AudioSource> ();
		Winning = gameObject.GetComponent<AudioSource> ();
		Winning.clip = Resources.Load ("collection") as AudioClip;
		if (SceneManager.GetActiveScene ().name == "3DPuzzleScene") {
			EventTrigger.Entry entry = new EventTrigger.Entry ();
			entry.eventID = EventTriggerType.PointerClick;
			entry.callback.AddListener ((data) => {
				GameObject.Find("GameLogic").GetComponent<VRPuzzleLogic> ().MoveCubeToSelectorUI ((PointerEventData)data, (GameObject)gameObject);
			});
			gameObject.GetComponent<EventTrigger> ().triggers.Add (entry);
			gameObject.GetComponent<Renderer> ().material.mainTexture=SaveData.control.cubeTex;
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
	public void UpdateIsCorrect() {
		if (currentPos == actualPos) {
			isCorrect = true;
			Winning.Play ();
		} else {
			isCorrect = false;
		}
	}
	public void UpdateIsCorrectVR() {
		if (currentPosVR == actualPosVR) {
			isCorrect = true;
			Winning.Play ();
		} else {
			isCorrect = false;
		}
	}
}
