using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
using TMPro;
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
		if (selection.Equals ("2D")) {
			SceneManager.LoadSceneAsync ("2DPuzzleScene");
		} else {
			StartCoroutine (SwitchToVR());
		}
	}
	public void SetNamesForLoad(GameObject inpfield) {
		string name = inpfield.transform.GetChild(0).GetChild(1).gameObject.GetComponent<TextMeshProUGUI> ().text;
		if (!name.Equals ("")) {
			string allNames = PlayerPrefs.GetString ("BestieJumbleFriendNames");
			string fallNames = allNames + ", " + name;
			PlayerPrefs.SetString ("BestieJumbleFriendNames", fallNames);
			inpfield.GetComponent<TMP_InputField> ().DeactivateInputField ();
		}
	}
}
