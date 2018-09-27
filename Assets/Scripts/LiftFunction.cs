using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftFunction : MonoBehaviour {
	public GameObject player; //Player i.e. GVR Editor Emulator
	private GameObject eventSystem; //GVR Event System
	bool flag; //Flag to check whether game sound has been played or not
	// Use this for initialization
	void Start () {
		eventSystem = GameObject.Find ("GvrEventSystem");
		flag = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	//Function to start movement of lift
	public void MoveLift() {
		if (!flag) {
			GameObject.Find ("GameLogic").GetComponent<AudioSource> ().Play (); //Playing game sound
			flag = true;
		}
		player.transform.SetParent (gameObject.transform); //Combining lift and player for easy movement
		if (transform.position.y >= 17.9f && transform.position.y <= 18.1f) { //Checking whether the lift is at top or bottom
			StartCoroutine(GoingDown ());
		} else {
			StartCoroutine(GoingUp ());
		}
	}
	//Coroutine for bringing lift down
	IEnumerator GoingDown() {
		eventSystem.SetActive (false);
		yield return StartCoroutine (GoDown ());
		yield return StartCoroutine (OpenGate ());
		yield return StartCoroutine (MovePlayer ());
		yield return StartCoroutine (DestroyParent ());
		yield return StartCoroutine (CloseGate ());
	}
	//Coroutine for bringing lift up
	IEnumerator GoingUp() {
		eventSystem.SetActive (false);
		yield return StartCoroutine (OpenGate ());
		yield return StartCoroutine (MovePlayer ());
		yield return StartCoroutine (CloseGate ());
		yield return StartCoroutine (GoUp ());
		yield return StartCoroutine (DestroyParent ());
	}
	//Opening Gate of Lift Coroutine
	IEnumerator OpenGate() {
		iTween.MoveTo (transform.GetChild (0).gameObject, transform.GetChild (0).position + new Vector3 (0f, 3f, 0f), 3f);
		yield return new WaitForSeconds(3f);
	}
	//Closing Gate of Lift Coroutine
	IEnumerator CloseGate() {
		iTween.MoveTo (transform.GetChild (0).gameObject, transform.GetChild (0).position + new Vector3 (0f, -3f, 0f), 3f);
		yield return new WaitForSeconds(3f);
	}
	//Moving Player in or out of the lift
	IEnumerator MovePlayer() {
		if (player.transform.position.y < 7.9f) {
			iTween.MoveTo (player, new Vector3 (7.19f, 8f, 0f), 5f);
		} else {
			iTween.MoveTo (player, player.transform.position + new Vector3 (0f, -0.92f, -5f), 5f);
		}
		yield return new WaitForSeconds (5f);
	}
	//Moving Lift up
	IEnumerator GoUp() {
		iTween.MoveTo (gameObject, new Vector3 (gameObject.transform.position.x, 18f, gameObject.transform.position.z), 5f);
		yield return new WaitForSeconds (5f);
	}
	//Moving Lift Down
	IEnumerator GoDown() {
		iTween.MoveTo (gameObject, new Vector3 (gameObject.transform.position.x, 6.32f, gameObject.transform.position.z), 5f);
		yield return new WaitForSeconds (5f);
	}
	//Remove Combining of Player and Lift
	IEnumerator DestroyParent() {
		player.transform.parent=null;
		eventSystem.SetActive (true);
		yield return new WaitForSeconds (0f);
	}
}
