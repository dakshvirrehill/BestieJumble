using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelGatesOC : MonoBehaviour {
	public Transform Pos;
	public GameObject[] gates;
	private GameObject player;
	private GameObject eventSystem;
	// Use this for initialization
	void Start () {
		player = GameObject.Find ("GvrEditorEmulator");
		eventSystem = GameObject.Find ("GvrEventSystem");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void MoveIn() {
		eventSystem.SetActive (false);
		StartCoroutine (Sequence ());
	}
	IEnumerator Sequence() {
		yield return StartCoroutine (OpenGate ());
		yield return StartCoroutine (MovePlayer ());
		yield return StartCoroutine (CloseGate ());
		yield return StartCoroutine (ActivateEventSystem ());
	}
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
	IEnumerator MovePlayer() {
		iTween.MoveTo (player, Pos.position, 3f);
		yield return new WaitForSeconds (3f);
	}
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
	IEnumerator ActivateEventSystem() {
		eventSystem.SetActive (true);
		yield return new WaitForSeconds (0f);
	}
}
