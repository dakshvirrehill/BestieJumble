using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftFunction : MonoBehaviour {
	public GameObject player;
	private GameObject parentMover;
	private GameObject eventSystem;
	// Use this for initialization
	void Start () {
		eventSystem = GameObject.Find ("GvrEventSystem");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void MoveLift() {
		parentMover = new GameObject ();
		parentMover.transform.position = transform.position;
		player.transform.SetParent (parentMover.transform);
		gameObject.transform.SetParent (parentMover.transform);
		if (transform.position.y >= 17.9f && transform.position.y <= 18.1f) {
			StartCoroutine(GoingDown ());
		} else {
			StartCoroutine(GoingUp ());
		}
	}
	IEnumerator GoingDown() {
		eventSystem.SetActive (false);
		yield return StartCoroutine (GoDown ());
		yield return StartCoroutine (OpenGate ());
		yield return StartCoroutine (MovePlayer ());
		yield return StartCoroutine (CloseGate ());
		yield return StartCoroutine (DestroyParent ());
	}
	IEnumerator GoingUp() {
		eventSystem.SetActive (false);
		yield return StartCoroutine (OpenGate ());
		yield return StartCoroutine (MovePlayer ());
		yield return StartCoroutine (CloseGate ());
		yield return StartCoroutine (GoUp ());
		yield return StartCoroutine (DestroyParent ());
	}
	IEnumerator OpenGate() {
		iTween.MoveTo (transform.GetChild (0).gameObject, transform.GetChild (0).position + new Vector3 (0f, 3f, 0f), 3f);
		yield return new WaitForSeconds(3f);
	}
	IEnumerator CloseGate() {
		iTween.MoveTo (transform.GetChild (0).gameObject, transform.GetChild (0).position + new Vector3 (0f, -3f, 0f), 3f);
		yield return new WaitForSeconds(3f);
	}
	IEnumerator MovePlayer() {
		if (player.transform.position.y < 7.9f) {
			iTween.MoveTo (player, new Vector3 (7.19f, 8f, 0f), 5f);
		} else {
			iTween.MoveTo (player, player.transform.position + new Vector3 (0f, -0.32f, -3.86f), 5f);
		}
		yield return new WaitForSeconds (5f);
	}
	IEnumerator GoUp() {
		iTween.MoveTo (parentMover, new Vector3 (parentMover.transform.position.x, 18f, parentMover.transform.position.z), 5f);
		yield return new WaitForSeconds (5f);
	}
	IEnumerator GoDown() {
		iTween.MoveTo (parentMover, new Vector3 (parentMover.transform.position.x, 6.32f, parentMover.transform.position.z), 5f);
		yield return new WaitForSeconds (5f);
	}
	IEnumerator DestroyParent() {
		parentMover.transform.DetachChildren ();
		Destroy (parentMover);
		eventSystem.SetActive (true);
		yield return new WaitForSeconds (0f);
	}
}
