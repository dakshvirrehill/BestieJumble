using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class MainSceenLogic : MonoBehaviour {
	public GameObject LoadGamePrefab;
	public GameObject LoadGameNameButton;
	public GameObject mainUI;
	private GameObject[] openpuzzlebuttons;
	private bool uponce;
	private GameObject username;
	// Use this for initialization
	void Start () {
		openpuzzlebuttons = new GameObject[2];
		openpuzzlebuttons [0] = mainUI.transform.GetChild (0).GetChild (2).gameObject;
		openpuzzlebuttons [1] = mainUI.transform.GetChild (0).GetChild (3).gameObject;
		uponce = true;
		username = mainUI.transform.GetChild (0).GetChild (0).gameObject;
		if (SaveData.control.cubeTex != null) {
			mainUI.transform.GetChild (0).GetChild (8).gameObject.GetComponent<RawImage> ().texture = SaveData.control.cubeTex;
		} 
		if (SaveData.control.username.Equals ("")) {
			openpuzzlebuttons [0].SetActive (false);
			openpuzzlebuttons [1].SetActive (false);
		} else {
			username.GetComponent<TextMeshProUGUI> ().text = "Player: "+SaveData.control.username;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(uponce&&!username.GetComponent<TextMeshProUGUI> ().text.Equals("")) {
			uponce = false;
			openpuzzlebuttons[0].SetActive (true);
			openpuzzlebuttons[1].SetActive (true);
		}
	}
	public void SetNamesForLoad(GameObject inpfield) {
		string name = inpfield.GetComponent<TMP_InputField> ().text;
		if (!name.Equals ("")) {
			string allNames = PlayerPrefs.GetString ("BestieJumbleFriendNames");
			string fallNames = allNames + ", " + name;
			PlayerPrefs.SetString ("BestieJumbleFriendNames", fallNames);
			inpfield.GetComponent<TMP_InputField> ().DeactivateInputField ();
			SaveData.control.username = name;
			username.GetComponent<TextMeshProUGUI> ().text = "Player: "+name;
		}
	}
	public void QuitGame() {
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
			GameObject content = LoadGameUI.GetComponent<ScrollRect> ().content.gameObject;
			string[] names = allNames.Split (',');
			float height = 60f + 60f * (names.Length - 1);
			content.GetComponent<RectTransform> ().sizeDelta = new Vector2 (770f, height);
			Vector3 firstpos = new Vector3 (-273f, (height / 2) -25f, 0f);
			foreach (string name in names) {
				GameObject lgnb = Instantiate (LoadGameNameButton, content.transform,false);
				lgnb.GetComponent<RectTransform> ().sizeDelta = new Vector2 (200f, 50f);
				lgnb.GetComponent<RectTransform> ().localPosition = firstpos;
				firstpos = firstpos + new Vector3 (0f, -60f, 0f);
				lgnb.GetComponent<TextMeshProUGUI> ().SetText (name);
				lgnb.GetComponent<Button> ().onClick.AddListener (() => LoadTextClicked (name));
			}
		}
	}
	public void LoadTextClicked(string name) {
		SaveData.control.username = name;
		//SaveData.control.Load (name);
		Destroy(GameObject.Find ("LoadGameUI"));
		username.GetComponent<TextMeshProUGUI> ().text = "Player: "+SaveData.control.username;
		mainUI.transform.GetChild (0).GetChild (8).gameObject.GetComponent<RawImage> ().texture = SaveData.control.cubeTex;
	}
	public void DeleteAllPlayers() {
		string[] allNames = PlayerPrefs.GetString ("BestieJumbleFriendNames").Split(',');
		PlayerPrefs.DeleteKey ("BestieJumbleFriendNames");
		Destroy(GameObject.Find ("LoadGameUI"));
		//SaveData.control.DeleteAllSaveData (allNames);
	}
	public void SelectImage() {
		if (NativeGallery.IsMediaPickerBusy ())
			return;
		else {
			NativeGallery.Permission p=NativeGallery.GetImageFromGallery( ( path ) =>
				{
					Debug.Log( "Image path: " + path );
					if( path != null )
					{
						Texture2D texture = NativeGallery.LoadImageAtPath( path, 1280 );
						if( texture == null )
						{
							Debug.Log( "Couldn't load texture from " + path );
							return;
						}
						SaveData.control.cubeTex=texture;
						mainUI.transform.GetChild (0).GetChild (8).gameObject.GetComponent<RawImage> ().texture=texture;
						//Destroy(texture);
					}
				}, "Select a JPG image", "image/jpg", 1280 );

			Debug.Log( "Permission result: " + p );
		}
	}
}
