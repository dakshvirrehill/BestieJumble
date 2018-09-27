using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
public class VRPuzzleLogic : MonoBehaviour {
	public GameObject parent;
	public GameObject SelectorUICanvasPrefab;
	public GameObject SelectorUIPos;
	public GameObject PoofPrefab;
	public GameObject InformationCanvasPrefab;
	public GameObject MainMenuUICanvasPrefab;
	public GameObject MainMenuPos;
	public GameObject MainMenuPrompt;
	private GameObject PuzzlePanel;
	private Vector3[] PuzzleCubePosition;
	public GameObject[] NonGridLocations;
	private int n;
	private GameObject[] selected;
	private int selectedsize;
	private GameObject eventSystem;
	private GameObject gvreditoremulator;
	// Use this for initialization
	void Awake() {
		eventSystem = GameObject.Find ("GvrEventSystem");
		gvreditoremulator = GameObject.Find ("GvrEditorEmulator");
		selectedsize = 0;
		selected = new GameObject[9];
		parent.SetActive (true);
		if (SaveData.control.cubeTex.width > SaveData.control.cubeTex.height) {
			n = 81;
			PuzzlePanel = parent.transform.GetChild (1).gameObject;
			PuzzlePanel.transform.transform.SetParent(null);
		} else {
			n = 36;
			PuzzlePanel = parent.transform.GetChild (0).gameObject;
			PuzzlePanel.transform.SetParent (null);
		}
		parent.SetActive(false);
	}
	void Start () {
		StartCoroutine(MakePuzzle ());
	}
	IEnumerator MakePuzzle() {
		PuzzleCubePosition = new Vector3[n];
		int k = 0;
		while(k<n) {
			PuzzlePanel.transform.GetChild(k).gameObject.GetComponent<PuzzleCube2D> ().actualPosVR = k;
			PuzzlePanel.transform.GetChild(k).gameObject.GetComponent<PuzzleCube2D> ().currentPosVR = -1;
			PuzzleCubePosition [k] = PuzzlePanel.transform.GetChild(k).localPosition;
			PuzzlePanel.transform.GetChild(k).gameObject.name = "PuzzleCube " + k;
			PuzzlePanel.transform.GetChild(k+n).gameObject.name = "Selector " + k;
			//PuzzlePanel.transform.GetChild (k + n).gameObject.GetComponent<VRGridSelector> ().pos = k;
			PuzzlePanel.transform.GetChild(k+n).gameObject.SetActive (false);
			k++;
		}
		if (SaveData.control.PuzzleVRCubePositions == null) {	
			JumblePuzzle ();
		} else {
			SavedPuzzle ();
		}
		yield return new WaitForSeconds (0f);
	}
	void SavedPuzzle () {
		int k = 0;
		System.Random r = new System.Random ();
		HashSet<int> hs = new HashSet<int> ();
		for (int i = 0; i < n; i++) {
			PuzzlePanel.transform.GetChild(k).gameObject.GetComponent<PuzzleCube2D> ().currentPosVR = SaveData.control.PuzzleVRCubePositions [i].Value;
			if (PuzzlePanel.transform.GetChild(k).gameObject.GetComponent<PuzzleCube2D> ().currentPosVR == -1) {
					int ngv = SaveData.control.PuzzleVRNonGridPositions [k].Value;
					if (ngv == -2) {
						ngv = r.Next (0, 81);
					}
					while (NonGridLocations [ngv].GetComponent<NonGridIsUsed> ().isUsed) {
						ngv=(ngv+1)%81;
					}
					PuzzlePanel.transform.GetChild(k).localScale = new Vector3 (0.2f, 0.2f, 0.2f);
					PuzzlePanel.transform.GetChild(k).gameObject.GetComponent<PuzzleCube2D> ().ngv = ngv;
					PuzzlePanel.transform.GetChild(k).position = NonGridLocations [ngv].transform.position;
					NonGridLocations [ngv].GetComponent<NonGridIsUsed> ().isUsed = true;
				} else {
					PuzzlePanel.transform.GetChild(k).localPosition = PuzzleCubePosition [PuzzlePanel.transform.GetChild(k).gameObject.GetComponent<PuzzleCube2D> ().currentPosVR];
					PuzzlePanel.transform.GetChild(k).gameObject.GetComponent<PuzzleCube2D> ().ngv = -2;
					PuzzlePanel.transform.GetChild(k).gameObject.GetComponent<PuzzleCube2D> ().UpdateIsCorrectVR ();
					hs.Add (PuzzlePanel.transform.GetChild(k).gameObject.GetComponent<PuzzleCube2D> ().currentPosVR);
				}
				k++;
				if (k == n) {
					StartCoroutine(checkAll ());
				}
		}
		for (int i = 0; i < n; i++) {
				if (!hs.Contains (i)) {
					CreateSelector (i);
				}
		}
	}
	void CreateSelector(int i) {
		PuzzlePanel.transform.GetChild(i+n).gameObject.SetActive (true);
	}
	void JumblePuzzle() {
		System.Random r = new System.Random ();
		for (int i = 80; i > 0; i--) {
			int j = r.Next (0, i);
			GameObject temp = NonGridLocations [i];
			NonGridLocations [i] = NonGridLocations [j];
			NonGridLocations [j] = temp;
		}
		for(int i=0;i<n;i++) {
			NonGridLocations [i].GetComponent<NonGridIsUsed> ().isUsed = true;
			PuzzlePanel.transform.GetChild(i).gameObject.GetComponent<PuzzleCube2D> ().ngv = NonGridLocations[i].GetComponent<NonGridIsUsed>().actInd;
			PuzzlePanel.transform.GetChild(i).localScale = new Vector3 (0.2f, 0.2f, 0.2f);
			PuzzlePanel.transform.GetChild(i).position = NonGridLocations [i].transform.position;
			CreateSelector (i);
			}
	}
	public void ActivateSelectorUI(PointerEventData eventData, GameObject Sel) {
		if (selectedsize==0) {
			
		} else {
			if (GameObject.Find ("SelectorUICanvas") != null) {
				Destroy(GameObject.Find("SelectorUICanvas"));
				CreateSelector (selected [0].GetComponent<PuzzleCube2D> ().currentPosVR);
			}
			eventSystem.SetActive (false);
			iTween.MoveTo (Sel, iTween.Hash ("position", SelectorUIPos.transform.position, "time", 2f, "oncomplete", "RestOfCode", "oncompletetarget", gameObject, "oncompleteparams", Sel));
		} 
	}
	public void MoveCubeToGrid(int pos) {
		GameObject shifter = selected [pos];
		selected [pos] = null;
		for (int i = 0; i < (selectedsize-1); i++) {
			if (i >= pos) {
				selected [i] = selected [i + 1];
			}
			selected [i].GetComponent<PuzzleCube2D> ().currentPosVR = -1;
		}
		selectedsize--;
		shifter.SetActive (true);
		Destroy(GameObject.Find ("SelectorUICanvas"));
		iTween.ScaleTo (shifter, new Vector3 (1, 1, 1), 2f);
		iTween.MoveTo (shifter, iTween.Hash("position",PuzzleCubePosition [shifter.GetComponent<PuzzleCube2D> ().currentPosVR], "time",3f,"islocal",true));
		shifter.GetComponent<PuzzleCube2D> ().UpdateIsCorrectVR ();
		StartCoroutine (checkAll ());
	}
	public void MoveCubeToSelectorUI(PointerEventData eventData, GameObject PC) {
		if (selectedsize == 9) {
			
		} else {
			if (!(PC.GetComponent<PuzzleCube2D> ().currentPosVR == -1)) {
				CreateSelector ((int)PC.GetComponent<PuzzleCube2D> ().currentPosVR);
				PC.GetComponent<PuzzleCube2D> ().currentPosVR =-1;
				PC.GetComponent<PuzzleCube2D> ().isCorrect = false;
			} else {
				if (selectedsize == 8) {
					gvreditoremulator.transform.position = new Vector3 (7.19f, 3.9f, -11f);
				} else {
					Object.Instantiate (PoofPrefab, PC.transform.position, Quaternion.Euler (-90f, 0f, 0f));
				}
			}
			selected [selectedsize] = PC;
			selectedsize++;
			PC.SetActive (false);
		}
	}
	void RestOfCode(GameObject sel) {
		int GridValue = sel.GetComponent<VRGridSelector> ().pos;
		sel.transform.localPosition = PuzzleCubePosition [GridValue];
		sel.SetActive (false);
		GameObject selectorUIPanel = Object.Instantiate (SelectorUICanvasPrefab, SelectorUIPos.transform, false);
		selectorUIPanel.name = "SelectorUICanvas";
		selectorUIPanel = selectorUIPanel.transform.GetChild (0).gameObject;
		int i;
		for (i = 0; i < selectedsize; i++) {
			selected [i].GetComponent<PuzzleCube2D> ().currentPosVR = GridValue;
			GameObject rm = selectorUIPanel.transform.GetChild (i).gameObject;
			rm.GetComponent<RawImage> ().color = Color.white;
			rm.GetComponent<RawImage> ().texture = SaveData.control.cubeTex;
			rm.GetComponent<RawImage> ().uvRect = new Rect (selected [i].GetComponent<Renderer> ().material.mainTextureOffset, selected [i].GetComponent<Renderer> ().material.mainTextureScale);
			rm.GetComponent<Button> ().interactable = true;
			selected [i].GetComponent<PuzzleCube2D> ().selectedPos = i;
			GameObject rk = selected [i];
			rm.GetComponent<Button> ().onClick.AddListener (() => MoveCubeToGrid (rk.GetComponent<PuzzleCube2D>().selectedPos));
			selected [i].transform.position = rm.transform.position;
		}
		while (i != 9) {
			GameObject rm = selectorUIPanel.transform.GetChild (i).gameObject;
			rm.GetComponent<Button> ().interactable = false;
			i++;
		}
		eventSystem.SetActive (true);
	}
	IEnumerator checkAll() {
		bool complete = true;
		for (int i = 0; i < n; i++) {
			complete = PuzzlePanel.transform.GetChild(i).gameObject.GetComponent<PuzzleCube2D> ().isCorrect;
			if (!complete)
				break;
		}
		SaveData.control.VRWon = complete;
		yield return new WaitForSeconds(0f);
	}
	public void DisplayMainMenu(GameObject MenuButton) {
		GameObject mainMenu = Object.Instantiate (MainMenuUICanvasPrefab, MainMenuPos.transform, false);
		mainMenu.name = "MainMenuUI";
		mainMenu.transform.GetChild (0).GetChild (0).gameObject.GetComponent<Button> ().onClick.AddListener (() => SaveGame());
		mainMenu.transform.GetChild (0).GetChild (1).gameObject.GetComponent<Button> ().onClick.AddListener (() => BackToMainMenu());
		mainMenu.transform.GetChild (0).GetChild (2).gameObject.GetComponent<Button> ().onClick.AddListener (() => DelMenu (MenuButton));
		mainMenu.transform.GetChild (0).GetChild (3).gameObject.GetComponent<Button> ().onClick.AddListener (() => ShowHideImage(true));
		mainMenu.transform.GetChild (0).GetChild (4).gameObject.GetComponent<Button> ().onClick.AddListener (() => ShowHideImage(false));
		mainMenu.transform.GetChild (0).GetChild (4).gameObject.GetComponent<RawImage> ().texture = SaveData.control.cubeTex;
		mainMenu.transform.GetChild (0).GetChild (4).gameObject.SetActive (false);
		MenuButton.SetActive (false);
	}
	void DelMenu(GameObject MenuButton) {
		MenuButton.SetActive (true);
		Destroy (GameObject.Find ("MainMenuUI"));
	}
	void SaveGame () {
		SaveData.control.PuzzleVRCubePositions = new int?[n];
		SaveData.control.PuzzleVRNonGridPositions = new int?[n];
		if (selectedsize != 0 && GameObject.Find ("SelectorUICanvas") != null) {
			CreateSelector (selected [0].GetComponent<PuzzleCube2D> ().currentPosVR);
			for (int i = 0; i < selectedsize; i++) {
				selected [i].GetComponent<PuzzleCube2D> ().currentPosVR = -1;
			}
			Destroy(GameObject.Find("SelectorUICanvas"));
		}
		for (int i = 0; i < n; i++) {
			SaveData.control.PuzzleVRCubePositions [i] = (int?)PuzzlePanel.transform.GetChild(i).gameObject.GetComponent<PuzzleCube2D>().currentPosVR;
			if (SaveData.control.PuzzleVRCubePositions [i] == -1) {
				SaveData.control.PuzzleVRNonGridPositions [i] = (int?)PuzzlePanel.transform.GetChild(i).gameObject.GetComponent<PuzzleCube2D> ().ngv;
				} else {
					SaveData.control.PuzzleVRNonGridPositions [i] = (int?)-1;
				}
		}
		SaveData.control.Save (SaveData.control.username);
	}
	void BackToMainMenu() {
		SaveGame ();
		Destroy (GameObject.Find ("MainMenuUI"));
		Object.Instantiate (MainMenuPrompt, MainMenuPos.transform, false);
		StartCoroutine (STD ());
	}
	void ShowHideImage(bool a) {
		GameObject mainMenu = GameObject.Find ("MainMenuUI");
		mainMenu.transform.GetChild (0).GetChild (4).gameObject.SetActive (a);
		mainMenu.transform.GetChild (0).GetChild (3).gameObject.SetActive (!a);
	}
	IEnumerator STD() {
		yield return new WaitForSeconds (2f);
		yield return StartCoroutine (SwitchToTwoD ());
		yield return StartCoroutine (SwitchScene ());
	}
	IEnumerator SwitchScene() {
		SceneManager.LoadSceneAsync ("MainScene");
		yield return new WaitForSeconds (0f);
	}
	IEnumerator SwitchToTwoD() {
		XRSettings.LoadDeviceByName ("");
		yield return null;
	}
	// Update is called once per frame
	void Update () {
		if (SaveData.control.VRWon) {
			SaveGame ();
			SceneManager.LoadSceneAsync ("VRLoadingScene");
		}
	}
}
