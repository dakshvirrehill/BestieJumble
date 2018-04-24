using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class MainSceenLogic : MonoBehaviour {
	public GameObject LoadGamePrefab;
	public GameObject LoadGameNameButton;
	public GameObject mainUI;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
	public void SetNamesForLoad(GameObject inpfield) {
		string name = inpfield.GetComponent<TMP_InputField> ().text;
		if (!name.Equals ("")) {
			string allNames = PlayerPrefs.GetString ("BestieJumbleFriendNames");
			string fallNames = allNames + ", " + name;
			PlayerPrefs.SetString ("BestieJumbleFriendNames", fallNames);
			inpfield.GetComponent<TMP_InputField> ().DeactivateInputField ();
			SaveData.control.username = name;
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
			}
		}
	}
	public void LoadTextClicked() {
		
		Destroy(GameObject.Find ("LoadGameUI"));
	}
	public void DeleteAllPlayers() {
		string[] allNames = PlayerPrefs.GetString ("BestieJumbleFriendNames").Split(',');
		PlayerPrefs.DeleteKey ("BestieJumbleFriendNames");
		Destroy(GameObject.Find ("LoadGameUI"));
		//SaveData.control.DeleteAllSaveData (allNames);
	}
}
