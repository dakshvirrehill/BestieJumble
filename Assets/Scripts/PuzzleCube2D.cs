using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class PuzzleCube2D : MonoBehaviour {
	public Vector2 currentPos; //Curr Position in 2D Grid
	public Vector2 actualPos; //Actual Pos in 2D Grid
	public int currentPosVR; //VR curr pos
	public int actualPosVR; //VR act pos
	public int ngv; //Non grid value VR
	public bool isCorrect; //Bool to check correct position
	public int selectedPos; //Selected position in VR selector
	// Use this for initialization
	void Start () { //Set event trigger for 3D Puzzle Scene
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
	public void UpdateIsCorrect() {//Set is correct value
		if (currentPos == actualPos) {
			isCorrect = true;
			gameObject.GetComponent<AudioSource>().Play ();
		} else {
			isCorrect = false;
		}
	}
	public void UpdateIsCorrectVR() {//Set is correct VR value
		if (currentPosVR == actualPosVR) {
			isCorrect = true;
			gameObject.GetComponent<AudioSource>().Play ();
		} else {
			isCorrect = false;
		}
	}
}
