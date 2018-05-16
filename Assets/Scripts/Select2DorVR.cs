using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
public class Select2DorVR : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	IEnumerator SwitchToVR() {
		string desiredDevice = "cardboard";
		XRSettings.LoadDeviceByName (desiredDevice);
		StartCoroutine (waitforframe ());
		yield return 0;
	}
	IEnumerator waitforframe () {
		yield return 0;
		XRSettings.enabled = true;
	}
	// Update is called once per frame
	void Update () {
		if (XRSettings.loadedDeviceName.Equals("cardboard")) {
			SceneManager.LoadSceneAsync ("3DPuzzleScene");
		}
	}
	public void changeSelected(string selection) {
		GameObject.Find ("Home Screen").SetActive (false);
		Object.Instantiate(SaveData.control.PreLoader);
		if (selection.Equals ("2D")) {
			SceneManager.LoadSceneAsync ("2DPuzzleScene");
		} else {
			StartCoroutine (SwitchToVR());
		}
	}
}
