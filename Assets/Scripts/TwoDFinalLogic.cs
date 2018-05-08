using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class TwoDFinalLogic : MonoBehaviour {
	public GameObject winningPanel;
	// Use this for initialization
	void Start () {
		//winningPanel.transform.GetChild (2).gameObject.GetComponent<RawImage> ().texture = SaveData.control.cubeTex;
		//winningPanel.transform.GetChild (3).gameObject.GetComponent<TextMeshProUGUI> ().text = SaveData.control.username;
		winningPanel.transform.GetChild (6).gameObject.GetComponent<Button> ().onClick.AddListener (() => BackToMain ());
		winningPanel.transform.GetChild (7).gameObject.GetComponent<Button> ().onClick.AddListener (() => Printed ());
		winningPanel.transform.GetChild (8).gameObject.GetComponent<Button> ().onClick.AddListener (() => new MainSceenLogic ().QuitGame ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void Printed() {
		//Screenshot Logic
	}
	public void BackToMain() {
		SceneManager.LoadSceneAsync ("MainScene");
	}
}