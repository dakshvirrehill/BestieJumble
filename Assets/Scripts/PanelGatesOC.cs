using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelGatesOC : MonoBehaviour {
	public Transform Pos; //Position to move
	public GameObject[] gates; //Gates to open
	private GameObject player; //Player to move
	private GameObject eventSystem; //Event system to deactivate and activate
	// Use this for initialization
	void Start () {
		player = GameObject.Find ("GvrEditorEmulator");
		eventSystem = GameObject.Find ("GvrEventSystem");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	//Start the moving sequence
	public void MoveIn() {
		StartCoroutine (Sequence ());
	}
	//Sequence for movement
	IEnumerator Sequence() {
		yield return StartCoroutine (SetAndDeactivateEventSystem ());
		yield return StartCoroutine (OpenGate ());
		yield return StartCoroutine (MovePlayer ());
		yield return StartCoroutine (CloseGate ());
		yield return StartCoroutine (ActivateEventSystem ());
	}
	//Coroutine to deactivate event system
	IEnumerator SetAndDeactivateEventSystem() {
		eventSystem.SetActive (false);
		yield return new WaitForSeconds (0f);
	}
	//Open the gate/gates depending upon the number of gates
	IEnumerator OpenGate() {
		if (gates.Length == 1 || gates.Length == 3) {
			iTween.MoveTo (gates [0], gates [0].transform.position + new Vector3 (0f, 3f, 0f), 3f);
			if (gates.Length == 3) {
				iTween.MoveTo (gates [1], gates [1].transform.position + new Vector3 (-2f, 0f, 0f), 3f);
				iTween.MoveTo (gates [2], gates [2].transform.position + new Vector3 (2f, 0f, 0f), 3f);
			}
		} else {
			iTween.MoveTo (gates [0], gates [0].transform.position + new Vector3 (-1.2f, 0f, 0f), 3f);
			iTween.MoveTo (gates [1], gates [1].transform.position + new Vector3 (1.2f, 0f, 0f), 3f);
		} 
		yield return new WaitForSeconds (3f);
	}
	//Move the player to position
	IEnumerator MovePlayer() {
		iTween.MoveTo (player, Pos.position, 3f);
		yield return new WaitForSeconds (3f);
	}
	//close the gate/gates depending upon how many gates are there
	IEnumerator CloseGate() {
		if (gates.Length == 1 || gates.Length == 3) {
			iTween.MoveTo (gates [0], gates [0].transform.position - new Vector3 (0f, 3f, 0f), 3f);
			if (gates.Length == 3) {
				iTween.MoveTo (gates [1], gates [1].transform.position - new Vector3 (-2f, 0f, 0f), 3f);
				iTween.MoveTo (gates [2], gates [2].transform.position - new Vector3 (2f, 0f, 0f), 3f);
			}
		} else {
			iTween.MoveTo (gates [0], gates [0].transform.position - new Vector3 (-1.2f, 0f, 0f), 3f);
			iTween.MoveTo (gates [1], gates [1].transform.position - new Vector3 (1.2f, 0f, 0f), 3f);
		} 
		yield return new WaitForSeconds (3f);
	}
	//activate the event system
	IEnumerator ActivateEventSystem() {
		eventSystem.SetActive (true);
		yield return new WaitForSeconds (0f);
	}
}
