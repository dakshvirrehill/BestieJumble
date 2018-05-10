using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class TwoDFinalLogic : MonoBehaviour {
	public GameObject winningPanel;
	private GameObject BackButton;
	private GameObject PrintButton;
	// Use this for initialization
	void Start () {
		BackButton = winningPanel.transform.GetChild (6).gameObject;
		PrintButton = winningPanel.transform.GetChild (7).gameObject;
		winningPanel.transform.GetChild (2).gameObject.GetComponent<RawImage> ().texture = SaveData.control.cubeTex;
		winningPanel.transform.GetChild (3).gameObject.GetComponent<TextMeshProUGUI> ().text = SaveData.control.username;
		winningPanel.transform.GetChild (5).gameObject.GetComponent<TMP_InputField> ().onSelect.AddListener ((string arg0) => DeactivateButtons (arg0));
		winningPanel.transform.GetChild (5).gameObject.GetComponent<TMP_InputField> ().onDeselect.AddListener ((string arg0) => ActivateButtons (arg0));
		winningPanel.transform.GetChild (5).gameObject.GetComponent<TMP_InputField> ().onEndEdit.AddListener ((string arg0) => ActivateButtons (arg0));
		BackButton.GetComponent<Button> ().onClick.AddListener (() => BackToMain ());
		PrintButton.GetComponent<Button> ().onClick.AddListener (() => Printed ());
		DeactivateButtons ("default");
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
		//string path = Application.persistentDataPath + "/" + SaveData.control.username + ".png";
		string path = Application.dataPath+"/"+SaveData.control.username+".png";
		if (Application.platform == RuntimePlatform.Android) {
			AndroidJavaClass environment = new AndroidJavaClass ("android.os.Environment");
			AndroidJavaObject filepath = environment.CallStatic<AndroidJavaObject> ("getExternalStoragePublicDirectory", environment.GetStatic<string> ("DIRECTORY_PICTURES"));
			filepath.Call<bool> ("mkdirs");
			path = filepath.Call<string> ("getAbsolutePath") + "/" + SaveData.control.username + ".png";
		}
		System.IO.File.WriteAllBytes (path, tex1.EncodeToPNG());
		Destroy (tex1);
	}
	public void BackToMain() {
		SceneManager.LoadSceneAsync ("MainScene");
	}
	public void ActivateButtons(string defaulte) {
		BackButton.SetActive(true);
		PrintButton.SetActive(true);
	}
	public void DeactivateButtons(string defaulte) {
		BackButton.SetActive(false);
		PrintButton.SetActive(false);
	}
}