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
		winningPanel.transform.GetChild (2).gameObject.GetComponent<RawImage> ().texture = SaveData.control.cubeTex;
		winningPanel.transform.GetChild (3).gameObject.GetComponent<TextMeshProUGUI> ().text = SaveData.control.username;
		winningPanel.transform.GetChild (6).gameObject.GetComponent<Button> ().onClick.AddListener (() => BackToMain ());
		winningPanel.transform.GetChild (7).gameObject.GetComponent<Button> ().onClick.AddListener (() => Printed ());
		winningPanel.transform.GetChild (8).gameObject.GetComponent<Button> ().onClick.AddListener (() => new MainSceenLogic ().QuitGame ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void Printed() {
		Texture2D tex=ScreenCapture.CaptureScreenshotAsTexture ();
		Debug.Log (tex.width + " w + " + tex.height + " h ");
		Color[] cols = tex.GetPixels ((int)(tex.width * 0.15625 + 1), 0, (int)(tex.width - tex.width * 0.3125), tex.height-1, 0);
		Texture2D tex1 = new Texture2D ((int)(tex.width - tex.width * 0.3125), tex.height - 1);
		Destroy (tex);
		tex1.SetPixels (cols);
		tex1.Apply ();
		System.IO.File.WriteAllBytes(Application.persistentDataPath + "/SavedScreen.png", tex1.EncodeToPNG());
		Destroy (tex1);
	}
	public void BackToMain() {
		SceneManager.LoadSceneAsync ("MainScene");
	}
}