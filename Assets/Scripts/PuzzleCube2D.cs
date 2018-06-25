using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class PuzzleCube2D : MonoBehaviour {
	public Vector2 currentPos;
	public Vector2 actualPos;
	public int ngv;
	public bool isCorrect;
	public int selectedPos;
	// Use this for initialization
	void Start () {
		if (SceneManager.GetActiveScene ().name == "3DPuzzleScene") {
			EventTrigger.Entry entry = new EventTrigger.Entry ();
			entry.eventID = EventTriggerType.PointerClick;
			entry.callback.AddListener ((data) => {
				GameObject.Find("GameLogic").GetComponent<VRPuzzleLogic> ().MoveCubeToSelectorUI ((PointerEventData)data, (GameObject)gameObject);
			});
			gameObject.GetComponent<EventTrigger> ().triggers.Add (entry);
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
	public void UpdateIsCorrect() {
		if (currentPos == actualPos) {
			isCorrect = true;
		} else {
			isCorrect = false;
		}
	}
}
