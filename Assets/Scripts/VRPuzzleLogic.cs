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
	public Shader shader;
	public GameObject NGLParent;
	public GameObject SelectorUICanvasPrefab;
	public GameObject SelectorUIPos;
	public GameObject PoofPrefab;
	public GameObject InformationCanvasPrefab;
	public GameObject MainMenuUICanvasPrefab;
	public GameObject MainMenuPos;
	public GameObject MainMenuPrompt;
	private GameObject PuzzlePanel;
	private GameObject PuzzleCube;
	private GameObject[,] PuzzleCubes;
	private GameObject selector;
	private Vector3[,] PuzzleCubePosition;
	private GameObject[] NonGridLocations;
	private Vector3 firstCubeLocation;
	private int n;
	private GameObject[] selected;
	private int selectedsize;
	private GameObject eventSystem;
	private GameObject infycanva;
	// Use this for initialization
	void Start () {
		eventSystem = GameObject.Find ("GvrEventSystem");
		SetNGLArray ();
		selectedsize = 0;
		parent.SetActive (true);
		PuzzleCube = parent.transform.GetChild (2).gameObject;
		selector = parent.transform.GetChild (3).gameObject;
		selector.SetActive (false);
		float z = 26.45f;
		float y = 19.6f;
		if (SaveData.control.cubeTex.width > 1024 || SaveData.control.cubeTex.height > 1024) {
			n = 9;
			selected = new GameObject[n];
			firstCubeLocation = new Vector3 (-9.61f, 37.6f, 30.3f);
			PuzzlePanel = parent.transform.GetChild (1).gameObject;
			Destroy (parent.transform.GetChild (0).gameObject);
			z = 31f;
			y = 14.4f;
		} else {
			n = 6;
			selected = new GameObject[n];
			firstCubeLocation = new Vector3 (-3.51f, 30.3f, 25.75f);
			PuzzlePanel = parent.transform.GetChild (0).gameObject;
			Destroy (parent.transform.GetChild (1).gameObject);
		}
		parent.transform.DetachChildren ();
		Destroy (parent);
		PuzzlePanel.transform.position=new Vector3(7.19f,y,z);
		PuzzlePanel.transform.rotation = Quaternion.Euler (0, 0, 0);
		PuzzleCube.transform.position = firstCubeLocation;
		PuzzleCube.transform.rotation = Quaternion.Euler (0, 0, 0);
		PuzzleCube.transform.SetParent (PuzzlePanel.transform, true);
		firstCubeLocation = PuzzleCube.transform.localPosition;
		MakePuzzle ();
	}
	void SetNGLArray() {
		NonGridLocations = new GameObject[81];
		for (int i = 0; i < 81; i++) {
			NonGridLocations [i] = NGLParent.transform.GetChild (i).gameObject;
			NonGridLocations [i].GetComponent<NonGridIsUsed> ().actInd = i;
		}
	}
	void MakePuzzle() {
		Vector2 scale = new Vector2 (1 / (float)n, 1 / (float)n);
		PuzzleCubes = new GameObject[n, n];
		PuzzleCubePosition = new Vector3[n, n];
		scale = new Vector2 (scale.x, 1);
		PuzzleCubes [0, 0] = PuzzleCube;
		PuzzleCubePosition [0, 0] = firstCubeLocation;
		for (int i = 0; i < n; i++) {
			for (int j = 0; j < n; j++) {
				if (!(i == 0 && j == 0)) {
					PuzzleCubes [i, j] = Object.Instantiate (PuzzleCube, PuzzlePanel.transform, false);
					PuzzleCubes [i, j].tag = "PuzzleCube";
					firstCubeLocation = firstCubeLocation + new Vector3 (4.2f, 0, 0);
					PuzzleCubes [i, j].transform.localPosition = firstCubeLocation;
					PuzzleCubePosition [i, j] = firstCubeLocation;
				}
				Material pc = new Material (shader);
				pc.mainTexture = SaveData.control.cubeTex;
				pc.mainTextureOffset = scale;
				pc.mainTextureScale = new Vector2 (1.0f / n, 1.0f / n);
				PuzzleCubes [i, j].GetComponent<Renderer> ().material = pc;
				scale = scale + new Vector2 (1 / (float)n, 0f);
			}
			scale = new Vector2 (1/(float)n, scale.y - 1 / (float)n);
			firstCubeLocation = firstCubeLocation + new Vector3 (-n * 4.2f, -4.2f, 0);
		}
		GameObject[] ShiftingRow = new GameObject[n];
		for (int j = 0; j < n; j++) {
			ShiftingRow [j] = PuzzleCubes [0, j];
		}
		for (int i = 1; i < n; i++) {
			for(int j = 0; j < n; j++) {
				PuzzleCubes [i, j].transform.localPosition = PuzzleCubePosition [i - 1, j];
				PuzzleCubes [i - 1, j] = PuzzleCubes [i, j];
			}
		}
		for (int j = 0; j < n; j++) {
			PuzzleCubes [n - 1, j] = ShiftingRow [j];
			PuzzleCubes [n - 1, j].transform.localPosition = PuzzleCubePosition [n - 1, j];
		}
		ShiftingRow = new GameObject[n];
		for (int i = 0; i < n; i++) {
			ShiftingRow [i] = PuzzleCubes [i, n - 1];
		}
		for (int j = n - 2; j >= 0; j--) {
			for (int i = 0; i < n; i++) {
				PuzzleCubes [i, j].transform.localPosition = PuzzleCubePosition [i, j + 1];
				PuzzleCubes [i, j + 1] = PuzzleCubes [i, j];
				PuzzleCubes [i, j + 1].GetComponent<PuzzleCube2D> ().actualPos = new Vector2 (i, j + 1);
				PuzzleCubes [i, j + 1].GetComponent<PuzzleCube2D> ().currentPos = new Vector2 (-1, -1);
				PuzzleCubes [i, j + 1].name = " " + i + " " + (j+1);
			}
		}
		for (int i = 0; i < n; i++) {
			PuzzleCubes [i, 0] = ShiftingRow [i];
			PuzzleCubes [i, 0].transform.localPosition = PuzzleCubePosition [i, 0];
			PuzzleCubes [i, 0].GetComponent<PuzzleCube2D> ().actualPos = new Vector2 (i, 0);
			PuzzleCubes [i, 0].GetComponent<PuzzleCube2D> ().currentPos = new Vector2 (-1, -1);
			PuzzleCubes [i, 0].name = " " + i + " " + 0;
		}
		ShiftingRow = null;
		//infycanva = Object.Instantiate (InformationCanvasPrefab, new Vector3 (17.19f, 20f, 23.81f), Quaternion.Euler (0f, 0f, 0f));
		//infycanva.transform.GetChild (0).GetChild (3).gameObject.GetComponent<TextMeshProUGUI> ().text = "1. The Puzzle Cubes are being hidden in the world behind you. \n2. You need to find them and place them in the grid. \n3. "+n+" cubes can be collected at once and you can change their positions even after placing them on the grid. \n4. You win after all cubes are at the correct position.";
		//StartCoroutine (JumblePuzzle ());
		//Destroy (infycanva);
		JumblePuzzle();
	}
	void CreateSelector(int i,int j) {
		selector.SetActive (true);
		GameObject Sel = Object.Instantiate (selector, PuzzlePanel.transform, false);
		Sel.name = "Selector " + i + " " + (j + 1);
		Sel.GetComponent<VRGridSelector> ().pos = new Vector2 (i, j);
		selector.SetActive (false);
		Sel.transform.localPosition = PuzzleCubePosition [i, j];
		/*EventTrigger.Entry entree = new EventTrigger.Entry ();
		entree.eventID = EventTriggerType.PointerClick;
		entree.callback.AddListener ((data) => {
			ActivateSelectorUI ((PointerEventData)data, (GameObject)Sel);
		});
		Sel.GetComponent<EventTrigger> ().triggers.Add (entree);*/
	}
	void JumblePuzzle() {
		System.Random r = new System.Random ();
		for (int i = 80; i > 0; i--) {
			int j = r.Next (0, i);
			GameObject temp = NonGridLocations [i];
			NonGridLocations [i] = NonGridLocations [j];
			NonGridLocations [j] = temp;
		}
		int ngv=0;
		for (int i = 0; i < n; i++) {
			for (int j = 0; j < n; j++) {
				NonGridLocations [ngv].GetComponent<NonGridIsUsed> ().isUsed = true;
				PuzzleCubes [i, j].GetComponent<PuzzleCube2D> ().ngv = NonGridLocations[ngv].GetComponent<NonGridIsUsed>().actInd;
				/*GameObject pc = PuzzleCubes [i, j];
				EventTrigger.Entry entry = new EventTrigger.Entry ();
				entry.eventID = EventTriggerType.PointerClick;
				entry.callback.AddListener ((data) => {
			    	MoveCubeToSelectorUI ((PointerEventData)data, (GameObject)pc);
				});
				PuzzleCubes [i, j].GetComponent<EventTrigger> ().triggers.Add (entry);*/
				PuzzleCubes [i, j].transform.localScale = new Vector3 (0.2f, 0.2f, 0.2f);
				PuzzleCubes [i, j].transform.position = NonGridLocations [ngv].transform.position;
				CreateSelector (i, j);
				ngv++;
			}
		}
	}
	public void ActivateSelectorUI(PointerEventData eventData, GameObject Sel) {
		if (selectedsize==0) {
			
		} else {
			if (GameObject.Find ("SelectorUICanvas") != null) {
				Destroy(GameObject.Find("SelectorUICanvas"));
				CreateSelector ((int)selected [0].GetComponent<PuzzleCube2D> ().currentPos.x, (int)selected [0].GetComponent<PuzzleCube2D> ().currentPos.y);
			}
			eventSystem.SetActive (false);
			iTween.MoveTo (Sel, iTween.Hash ("position", SelectorUIPos.transform.position, "time", 2f, "oncomplete", "RestOfCode", "oncompletetarget", gameObject, "oncompleteparams", Sel));
		} 
	}
	public void MoveCubeToGrid(int pos) {
		GameObject shifter = selected [pos];
		selected [pos] = null;
		for (int i = pos; i < (selectedsize-1); i++) {
			selected [i] = selected [i + 1];
			selected [i].GetComponent<PuzzleCube2D> ().currentPos = new Vector2(-1,-1);
		}
		selectedsize--;
		shifter.SetActive (true);
		Destroy(GameObject.Find ("SelectorUICanvas"));
		iTween.ScaleTo (shifter, new Vector3 (1, 1, 1), 2f);
		iTween.MoveTo (shifter, iTween.Hash("position",PuzzleCubePosition [(int)shifter.GetComponent<PuzzleCube2D> ().currentPos.x, (int)shifter.GetComponent<PuzzleCube2D> ().currentPos.y], "time",3f,"islocal",true));
		shifter.GetComponent<PuzzleCube2D> ().UpdateIsCorrect ();
		StartCoroutine (checkAll ());
	}
	public void MoveCubeToSelectorUI(PointerEventData eventData, GameObject PC) {
		if (selectedsize == n) {
			
		} else {
			if (!(PC.GetComponent<PuzzleCube2D> ().currentPos == new Vector2 (-1, -1))) {
				CreateSelector ((int)PC.GetComponent<PuzzleCube2D> ().currentPos.x, (int)PC.GetComponent<PuzzleCube2D> ().currentPos.y);
				PC.GetComponent<PuzzleCube2D> ().currentPos = new Vector2 (-1, -1);
				PC.GetComponent<PuzzleCube2D> ().isCorrect = false;
			} else {
				Object.Instantiate (PoofPrefab, PC.transform.position, Quaternion.Euler (-90f, 0f, 0f));
			}
			selected [selectedsize] = PC;
			selectedsize++;
			PC.SetActive (false);
		}
	}
	void RestOfCode(GameObject sel) {
		Vector2 GridValue = sel.GetComponent<VRGridSelector> ().pos;
		Destroy (sel);
		GameObject selectorUIPanel = Object.Instantiate (SelectorUICanvasPrefab, SelectorUIPos.transform, false);
		selectorUIPanel.name = "SelectorUICanvas";
		selectorUIPanel = selectorUIPanel.transform.GetChild (0).gameObject;
		int i;
		for (i = 0; i < selectedsize; i++) {
			selected [i].GetComponent<PuzzleCube2D> ().currentPos = GridValue;
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
		while (i != n) {
			GameObject rm = selectorUIPanel.transform.GetChild (i).gameObject;
			rm.GetComponent<Button> ().interactable = false;
			i++;
		}
		eventSystem.SetActive (true);
	}
	IEnumerator checkAll() {
		bool complete = true;
		for (int i = 0; i < n; i++) {
			for (int j = 0; j < n; j++) {
				complete = PuzzleCubes [i, j].GetComponent<PuzzleCube2D> ().isCorrect;
				if (!complete)
					break;
			}
			if (!complete)
				break;
		}
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
		Debug.Log ("Saving..");
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
		yield return StartCoroutine (SwitchToTwoD ());
		yield return StartCoroutine (SwitchScene ());
	}
	IEnumerator SwitchScene() {
		SceneManager.LoadSceneAsync ("MainScene");
	}
	IEnumerator SwitchToTwoD() {
		XRSettings.LoadDeviceByName ("");
		yield return null;
		ResetCameras ();
	}
	void ResetCameras() {
		for (int i = 0; i < Camera.allCameras.Length; i++) {
			Camera cam = Camera.allCameras[i];
			if (cam.enabled && cam.stereoTargetEye != StereoTargetEyeMask.None) {
				cam.transform.localPosition = Vector3.zero;
				cam.transform.localRotation = Quaternion.identity;
			}
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
