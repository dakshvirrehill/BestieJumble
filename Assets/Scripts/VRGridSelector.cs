using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class VRGridSelector : MonoBehaviour {
	public int pos;
	// Use this for initialization
	void Start () {
		EventTrigger.Entry entree = new EventTrigger.Entry ();
		entree.eventID = EventTriggerType.PointerClick;
		entree.callback.AddListener ((data) => {
			GameObject.Find("GameLogic").GetComponent<VRPuzzleLogic> ().ActivateSelectorUI ((PointerEventData)data, (GameObject)gameObject);
		});
		gameObject.GetComponent<EventTrigger> ().triggers.Add (entree);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
