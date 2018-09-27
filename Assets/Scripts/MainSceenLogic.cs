using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class MainSceenLogic : MonoBehaviour {
	public GameObject LoadGamePrefab; //Prefab for game loading name screen
	public GameObject LoadGameNameButton; //Prefab for game loading name button on game loading name screen
	public GameObject SelectImagePrompt; //Prefab for prompt of overwriting the game
	public GameObject mainUI; //MainUI panel game object
	private GameObject[] openpuzzlebuttons; //Buttons for opening VR or 2D Puzzles and select image button
	private GameObject username; //Username of Player
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
		if (SaveData.control.username.Equals ("")) { //If name is not set deactivate all buttons
			openpuzzlebuttons [0].SetActive (false);
			openpuzzlebuttons [1].SetActive (false);
			openpuzzlebuttons [2].SetActive (false);
		} else {
			username.GetComponent<TextMeshProUGUI> ().text = "Player: "+SaveData.control.username;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!username.GetComponent<TextMeshProUGUI> ().text.Equals ("")) { //If name is set activate buttons
			openpuzzlebuttons [0].SetActive (true);
			openpuzzlebuttons [1].SetActive (true);
			openpuzzlebuttons [2].SetActive (true);
		} else { //Deactivate buttons if name not set
			openpuzzlebuttons [0].SetActive (false);
			openpuzzlebuttons [1].SetActive (false);
			openpuzzlebuttons [2].SetActive (false);
		}
	}
	//When new game button clicked, activate game play buttons and deactivate new game button remove persistant data as well
	public void NewGame() {
		mainUI.transform.GetChild (0).GetChild (4).gameObject.SetActive (true);
		mainUI.transform.GetChild (0).GetChild (5).gameObject.SetActive (true);
		mainUI.transform.GetChild (0).GetChild (10).gameObject.SetActive (false);
		mainUI.transform.GetChild (0).GetChild (1).gameObject.SetActive (false);
		SaveData.control.DefaultEverything ();
		DefaultEverything ();
	}
	//Activate new game button and deactivate game play buttons
	public void ActiveNewGameButton() {
		mainUI.transform.GetChild (0).GetChild (4).gameObject.SetActive (false);
		mainUI.transform.GetChild (0).GetChild (5).gameObject.SetActive (false);
		mainUI.transform.GetChild (0).GetChild (10).gameObject.SetActive (true);
		mainUI.transform.GetChild (0).GetChild (1).gameObject.SetActive (true);
	}
	//Set load game names stored in player prefs and load game if name already in player prefs
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
	//Quit Game
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
	//Initialize load game window with all saved games names and setup buttons dynamically
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
	//Load game by the save game name and set everything to save game values
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
	//Delete all saved games
	public void DeleteAllPlayers() {
		string[] allNames = PlayerPrefs.GetString ("BestieJumbleFriendNames").Split(',');
		PlayerPrefs.DeleteKey ("BestieJumbleFriendNames");
		Destroy(GameObject.Find ("LoadGameUI"));
		SaveData.control.DeleteAllSaveData (allNames);
		DefaultEverything ();
	}
	//Default everything in the current game
	private void DefaultEverything() {
		username.GetComponent<TextMeshProUGUI> ().text = "";
		mainUI.transform.GetChild (0).GetChild (8).gameObject.GetComponent<RawImage> ().texture = SaveData.control.cubeTex;
	}
	//Go back to Main Scene from load game scene
	public void BackToScreen() {
		Destroy(GameObject.Find("LoadGameUI"));
	}
	//Select image from gallery for setting up puzzle
	public void SelectImage() {
		if (SaveData.control.Puzzle2DCubePositions != null || SaveData.control.PuzzleVRCubePositions !=null) {
			GameObject SelectImageUI = Object.Instantiate (SelectImagePrompt, mainUI.transform, false);
			SelectImageUI.transform.GetChild (0).GetChild (1).gameObject.GetComponent<Button> ().onClick.AddListener (() => ChangeImage (true));
			SelectImageUI.transform.GetChild (0).GetChild (2).gameObject.GetComponent<Button> ().onClick.AddListener (() => ChangeImage (false));
			SelectImageUI.name = "SelectImageUI";
		} else {
			ImageSelector ();
		}
	}
	//If image already set and prompt opens to reset image, change image funciton is called nulling already set values
	public void ChangeImage(bool choice) {
		if (choice) {
			SaveData.control.Puzzle2DCubePositions = null;
			SaveData.control.PuzzleVRCubePositions = null;
			SaveData.control.PuzzleVRNonGridPositions = null;
			ImageSelector ();
		}
		Destroy (GameObject.Find ("SelectImageUI"));
	}
	//Calling native gallery plugin to pick image from android native gallery and making texture from the image
	void ImageSelector() {
		if (Application.platform == RuntimePlatform.Android) {
			if (NativeGallery.IsMediaPickerBusy ())
				return;
			else {
				NativeGallery.Permission p = NativeGallery.GetImageFromGallery (( path) => {
					if (path != null) {
						Texture2D texture = NativeGallery.LoadImageAtPath (path, -1 , false, true, false);
						if (texture == null) {
							return;
						}
						SaveData.control.cubeTex = texture;
						mainUI.transform.GetChild (0).GetChild (8).gameObject.GetComponent<RawImage> ().texture = texture;
					}
				}, "Select a JPG image", "image/jpg", 3000);
				SaveData.control.Save (SaveData.control.username);
			}
		} else { //For testing in editor
			WWW w = new WWW ("file:///F://workspaceVR//Little Rebel//Assets//Photos//IMG-20160816-WA0008.jpg");
			SaveData.control.cubeTex = w.texture;
			mainUI.transform.GetChild (0).GetChild (8).gameObject.GetComponent<RawImage> ().texture = w.texture;
			SaveData.control.Save (SaveData.control.username);
		}
	}
}
