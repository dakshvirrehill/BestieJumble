using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using TMPro;
public class VRWinningLogic : MonoBehaviour {
	public GameObject Cowboy;
	public GameObject Player;
	public GameObject Menu;
	public AudioClip[] clips;
	// Use this for initialization
	void Start () {
		Menu.SetActive (true);
		Menu.transform.GetChild (0).GetChild (1).gameObject.GetComponent<RawImage> ().texture = SaveData.control.cubeTex;
		Menu.transform.GetChild (0).GetChild (2).gameObject.GetComponent<TextMeshProUGUI> ().text = SaveData.control.username;
		Menu.transform.GetChild (0).GetChild (3).gameObject.GetComponent<Button> ().onClick.AddListener (() => ConvertTo2D ());
		Menu.transform.GetChild (0).GetChild (4).gameObject.GetComponent<Button> ().onClick.AddListener (() => QuitGame ());
		Menu.SetActive (false);
	}
	public void ConvertTo2D() {
		StartCoroutine (Convert2D ());
	}
	IEnumerator Convert2D() {
		yield return StartCoroutine (Conversion ());
		yield return StartCoroutine (RestOfCode ());
	}
	IEnumerator Conversion() {
		XRSettings.LoadDeviceByName("");
		yield return null;
	}
	IEnumerator RestOfCode() {
		SceneManager.LoadSceneAsync("FinalScene");
		yield return null;
	}
	public void QuitGame() {
		SaveData.control.Save (SaveData.control.username);
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
	// Update is called once per frame
	void Update () {
		if (Cowboy.GetComponent<Animator> ().GetAnimatorTransitionInfo (0).IsName ("Wave -> SpeakingIdle")) {
			Cowboy.GetComponent<AudioSource> ().clip = clips [0];
			Cowboy.GetComponent<AudioSource> ().Play ();
		}
		if (Cowboy.GetComponent<Animator> ().GetAnimatorTransitionInfo (0).IsName ("SpeakingIdle -> Remembering")) {
			Cowboy.GetComponent<AudioSource> ().clip = clips [1];
			Cowboy.GetComponent<AudioSource> ().Play ();
		}
		if (Cowboy.GetComponent<Animator> ().GetAnimatorTransitionInfo (0).IsName ("Remembering -> StartWalking")) {
			MovePlayer ();
			StartCoroutine (PlayClip ());
		}
		if (Cowboy.GetComponent<Animator> ().GetAnimatorTransitionInfo (0).IsName ("WalkForwardAndTurn -> SpeakingIdle")) {
			Cowboy.GetComponent<AudioSource> ().clip = clips [3];
			Cowboy.GetComponent<AudioSource> ().Play ();
			Cowboy.GetComponent<Animator> ().SetBool ("idle", true);
			Menu.SetActive (true);
		}
	}
	IEnumerator PlayClip() {
		yield return new WaitForSeconds (3f);
		yield return StartCoroutine (PlayClip2 ());
	}
	IEnumerator PlayClip2() {
		Cowboy.GetComponent<AudioSource> ().clip = clips [2];
		Cowboy.GetComponent<AudioSource> ().Play ();
		yield return new WaitForSeconds (0f);
	}
	void MovePlayer() {
		iTween.MoveTo (Player, iTween.Hash ("position",new Vector3(11.24f,1.5f,2.44f),"speed",1f,"easetype","Linear"));
	}
}
