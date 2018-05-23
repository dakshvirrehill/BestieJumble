using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class MainSceenLogic : MonoBehaviour {
	public GameObject LoadGamePrefab;
	public GameObject LoadGameNameButton;
	public GameObject SelectImagePrompt;
	public GameObject mainUI;
	private GameObject[] openpuzzlebuttons;
	private GameObject username;
	// Use this for initialization
	void Start () {
		openpuzzlebuttons = new GameObject[3];
		openpuzzlebuttons [2] = mainUI.transform.GetChild (0).GetChild (1).gameObject;
		openpuzzlebuttons [0] = mainUI.transform.GetChild (0).GetChild (2).gameObject;
		openpuzzlebuttons [1] = mainUI.transform.GetChild (0).GetChild (3).gameObject;
		username = mainUI.transform.GetChild (0).GetChild (0).gameObject;
		ActiveNewGameButton ();
		if (SaveData.control.cubeTex != null) {
			mainUI.transform.GetChild (0).GetChild (8).gameObject.GetComponent<RawImage> ().texture = SaveData.control.cubeTex;
		} 
		if (SaveData.control.username.Equals ("")) {
			openpuzzlebuttons [0].SetActive (false);
			openpuzzlebuttons [1].SetActive (false);
			openpuzzlebuttons [2].SetActive (false);
		} else {
			username.GetComponent<TextMeshProUGUI> ().text = "Player: "+SaveData.control.username;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!username.GetComponent<TextMeshProUGUI> ().text.Equals ("")) {
			openpuzzlebuttons [0].SetActive (true);
			openpuzzlebuttons [1].SetActive (true);
			openpuzzlebuttons [2].SetActive (true);
		} else {
			openpuzzlebuttons [0].SetActive (false);
			openpuzzlebuttons [1].SetActive (false);
			openpuzzlebuttons [2].SetActive (false);
		}
	}
	public void NewGame() {
		mainUI.transform.GetChild (0).GetChild (4).gameObject.SetActive (true);
		mainUI.transform.GetChild (0).GetChild (5).gameObject.SetActive (true);
		mainUI.transform.GetChild (0).GetChild (10).gameObject.SetActive (false);
		mainUI.transform.GetChild (0).GetChild (1).gameObject.SetActive (false);
		SaveData.control.DefaultEverything ();
		DefaultEverything ();
	}
	public void ActiveNewGameButton() {
		mainUI.transform.GetChild (0).GetChild (4).gameObject.SetActive (false);
		mainUI.transform.GetChild (0).GetChild (5).gameObject.SetActive (false);
		mainUI.transform.GetChild (0).GetChild (10).gameObject.SetActive (true);
		mainUI.transform.GetChild (0).GetChild (1).gameObject.SetActive (true);
	}
	public void SetNamesForLoad(GameObject inpfield) {
		string name = inpfield.GetComponent<TMP_InputField> ().text;
		if (!name.Equals ("")) {
			string[] allNames = PlayerPrefs.GetString ("BestieJumbleFriendNames").Split (',');
			bool already = false;
			for (int i = 0; i < allNames.Length; i++) {
				if (allNames [i].ToLower ().Equals (name.ToLower ())) {
					already = true;
					break;
				}
			}
			if (already) {
				LoadTextClicked (name);
				inpfield.GetComponent<TMP_InputField> ().DeactivateInputField ();
			} else {
				string fallNames = "";
				foreach (string names in allNames) {
					fallNames = fallNames+names + ",";
				}
				fallNames = fallNames+name;
				PlayerPrefs.SetString ("BestieJumbleFriendNames", fallNames);
				SaveData.control.username = name;
				username.GetComponent<TextMeshProUGUI> ().text = "Player: " + name;
				SaveData.control.Save (name);
				ActiveNewGameButton ();
			}
		}
	}
	public void QuitGame() {
		if (!SaveData.control.username.Equals ("")) {
			SaveData.control.Save (SaveData.control.username);
		}
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
	public void LoadGame() {
		string allNames = PlayerPrefs.GetString ("BestieJumbleFriendNames");
		if (!allNames.Equals ("")) {
			GameObject LoadGameUI = Instantiate (LoadGamePrefab, mainUI.transform, false);
			LoadGameUI.name = "LoadGameUI";
			LoadGameUI.GetComponent<ScrollRect>().viewport.GetChild(0).gameObject.GetComponent<Button> ().onClick.AddListener (() => DeleteAllPlayers ());
			LoadGameUI.GetComponent<ScrollRect>().viewport.GetChild(1).gameObject.GetComponent<Button> ().onClick.AddListener (() => BackToScreen ());
			GameObject content = LoadGameUI.GetComponent<ScrollRect> ().content.gameObject;
			string[] names = allNames.Split (',');
			float height = 90f + 90f * (names.Length - 1);
			content.GetComponent<RectTransform> ().sizeDelta = new Vector2 (1250f, height);
			Vector3 firstpos = new Vector3 (-241f, (height / 2) -40f, 0f);
			foreach (string name in names) {
				GameObject lgnb = Instantiate (LoadGameNameButton, content.transform,false);
				lgnb.GetComponent<RectTransform> ().sizeDelta = new Vector2 (720f, 80f);
				lgnb.GetComponent<RectTransform> ().localPosition = firstpos;
				firstpos = firstpos + new Vector3 (0f, -90f, 0f);
				lgnb.GetComponent<TextMeshProUGUI> ().SetText (name);
				lgnb.GetComponent<Button> ().onClick.AddListener (() => LoadTextClicked (name));
			}
		}
	}
	public void LoadTextClicked(string name) {
		SaveData.control.username = name;
		SaveData.control.Load (name);
		if (GameObject.Find ("LoadGameUI") != null) {
			Destroy (GameObject.Find ("LoadGameUI"));
		}
		username.GetComponent<TextMeshProUGUI> ().text = "Player: "+SaveData.control.username;
		mainUI.transform.GetChild (0).GetChild (8).gameObject.GetComponent<RawImage> ().texture = SaveData.control.cubeTex;
		ActiveNewGameButton ();
	}
	public void DeleteAllPlayers() {
		string[] allNames = PlayerPrefs.GetString ("BestieJumbleFriendNames").Split(',');
		PlayerPrefs.DeleteKey ("BestieJumbleFriendNames");
		Destroy(GameObject.Find ("LoadGameUI"));
		SaveData.control.DeleteAllSaveData (allNames);
		DefaultEverything ();
	}
	private void DefaultEverything() {
		username.GetComponent<TextMeshProUGUI> ().text = "";
		mainUI.transform.GetChild (0).GetChild (8).gameObject.GetComponent<RawImage> ().texture = SaveData.control.cubeTex;
	}
	public void BackToScreen() {
		Destroy(GameObject.Find("LoadGameUI"));
	}
	public void SelectImage() {
		if (SaveData.control.Puzzle2DCubePositions != null) {
			GameObject SelectImageUI = Object.Instantiate (SelectImagePrompt, mainUI.transform, false);
			SelectImageUI.transform.GetChild (0).GetChild (1).gameObject.GetComponent<Button> ().onClick.AddListener (() => ChangeImage (true));
			SelectImageUI.transform.GetChild (0).GetChild (2).gameObject.GetComponent<Button> ().onClick.AddListener (() => ChangeImage (false));
			SelectImageUI.name = "SelectImageUI";
		} else {
			ImageSelector ();
		}
	}
	public void ChangeImage(bool choice) {
		if (choice) {
			SaveData.control.Puzzle2DCubePositions = null;
			ImageSelector ();
		}
		Destroy (GameObject.Find ("SelectImageUI"));
	}
	void ImageSelector() {
		if (Application.platform == RuntimePlatform.Android) {
			if (NativeGallery.IsMediaPickerBusy ())
				return;
			else {
				NativeGallery.Permission p = NativeGallery.GetImageFromGallery (( path) => {
					Debug.Log ("Image path: " + path);
					if (path != null) {
						Texture2D texture = NativeGallery.LoadImageAtPath (path, -1 , false, true, false);
						if (texture == null) {
							Debug.Log ("Couldn't load texture from " + path);
							return;
						}
						SaveData.control.cubeTex = texture;
						mainUI.transform.GetChild (0).GetChild (8).gameObject.GetComponent<RawImage> ().texture = texture;
						//Destroy(texture);
					}
				}, "Select a JPG image", "image/jpg", 1280);
				SaveData.control.Save (SaveData.control.username);
				Debug.Log ("Permission result: " + p);
			}
		} else {
			WWW w = new WWW ("file:///F://workspaceVR//Little Rebel//Assets//Photos//IMG-20160816-WA0008.jpg");
			SaveData.control.cubeTex = w.texture;
			mainUI.transform.GetChild (0).GetChild (8).gameObject.GetComponent<RawImage> ().texture = w.texture;
			SaveData.control.Save (SaveData.control.username);
		}
	}
}
