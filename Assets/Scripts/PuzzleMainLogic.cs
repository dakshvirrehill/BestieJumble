using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;
public class PuzzleMainLogic : MonoBehaviour {
	public string OrientationAndScale;
	private GameObject[,] PuzzleCubes;
	private Vector2[,] PuzzleCubePosition;
	//private GameObject selected1,selected2;
	private Vector2 sel1ap, sel2ap;
	private GameObject eventsystem;
	// Use this for initialization
	void Start () {
			eventsystem = EventSystem.current.gameObject;
		    sel1ap = sel2ap = new Vector2(-1,-1);
			int n = 0, m = 0;
			if (OrientationAndScale.Equals ("Landscape")) {
				n = 6;
				m = 9;
			} else if (OrientationAndScale.Equals ("Portrait")) {
				n = 6;
				m = 4;
			} else {
				n = 6;
				m = 6;
			}
			PuzzleCubes = new GameObject[n, m];
			PuzzleCubePosition = new Vector2[n, m];
			int count = 0;
			//Debug.Log ("Starting PuzzleCubePositions");
			for (int i = 0; i < n; i++) {
				for (int j = 0; j < m; j++) {
					PuzzleCubes [i, j] = gameObject.transform.GetChild (count).gameObject;
					count++;
					if (SaveData.control.Puzzle2DPanel == null) {	
						PuzzleCubes [i, j].GetComponent<RawImage> ().texture = SaveData.control.cubeTex;
						PuzzleCubes [i, j].GetComponent<PuzzleCube2D> ().actualPos = new Vector2 (i, j);
						PuzzleCubes [i, j].GetComponent<PuzzleCube2D> ().currentPos = new Vector2 (i, j);
					}
					GameObject pctrial = PuzzleCubes [i, j];
					PuzzleCubes [i, j].GetComponent<Button> ().onClick.AddListener (() => SetSelected (pctrial));
					PuzzleCubePosition [i, j] = new Vector2 (PuzzleCubes [i, j].GetComponent<RectTransform> ().localPosition.x, PuzzleCubes [i, j].GetComponent<RectTransform> ().localPosition.y);
					//Debug.Log ("Value for i=" + i + " and j=" + j);
					//	Debug.Log (PuzzleCubePosition [i, j]);
				}
			}
			gameObject.transform.GetChild (count).gameObject.GetComponent<TextMeshProUGUI> ().SetText ("Player: " + SaveData.control.username);
		gameObject.transform.GetChild (count + 1).gameObject.GetComponent<Button> ().onClick.AddListener (() => Save());
			gameObject.transform.GetChild (count + 2).gameObject.GetComponent<Button> ().onClick.AddListener (() => BackToMainMenu ());
		gameObject.transform.GetChild (count + 4).gameObject.GetComponent<Button> ().onClick.AddListener (() => new MainSceenLogic ().QuitGame ());
		if (SaveData.control.Puzzle2DPanel == null) {	
			JumblePuzzle ();
		}
	}
	public void Save() {
		SaveData.control.Puzzle2DPanel = PrefabUtility.CreatePrefab("Assets/Prefabs/Puzzle2DPanel.prefab", gameObject, ReplacePrefabOptions.ReplaceNameBased);
		//SaveData.control.Save (SaveData.control.username);
	}
	// Update is called once per frame
	void Update () {
	}
	void JumblePuzzle() {
		System.Random r = new System.Random ();
		//Debug.Log ("Values in jumblepuzzle function");
		int row=PuzzleCubes.GetLength (0);
		int column = PuzzleCubes.GetLength (1);
		for (int k = 0; k < column; k++) {
			for (int i = row - 1; i >= 0; i--) {
				for (int j = column - 1; j >= 0; j--) {
					int n = r.Next (0, (i + 1));
					int m = r.Next (0, (j + 1));
					Vector2 currentpos = PuzzleCubes [i, j].GetComponent<PuzzleCube2D> ().currentPos;
					Vector2 actualpos = PuzzleCubes [n, m].GetComponent<PuzzleCube2D> ().actualPos;
					if (actualpos.x == currentpos.x) {
						n = (n + 1) % row;
					}
					if (actualpos.y == currentpos.y) {
						m = (m + 1) % column;
					}
					actualpos = PuzzleCubes [i, j].GetComponent<PuzzleCube2D> ().actualPos;
					currentpos = PuzzleCubes [n, m].GetComponent<PuzzleCube2D> ().currentPos;
					if (actualpos.x == currentpos.x) {
						n = (n + 1) % row;
					}
					if (actualpos.x == currentpos.x) {
						m = (m + 1) % column;
					}
					sel1ap = new Vector2 (i, j);
					sel2ap = new Vector2 (n, m);
					swappingcpnp ();
					//Debug.Log ("i=" + i + " j=" + j + " n=" + n + " m=" + m);
					//Debug.Log ("CurrentPos of ij= " + PuzzleCubes [i, j].GetComponent<PuzzleCube2D> ().currentPos);
					//Debug.Log ("CurrentPos of nm= " + PuzzleCubes [n, m].GetComponent<PuzzleCube2D> ().currentPos);
					//Debug.Log ("PuzzleCubePosition of ij  " + PuzzleCubePosition [i, j]);
					//Debug.Log ("PuzzleCubePosition of nm" + PuzzleCubePosition [n, m]);
				}
			}
		}
	}
	public void SetSelected(GameObject selected) {
		if (sel1ap.x == -1) {
			sel1ap = selected.GetComponent<PuzzleCube2D> ().actualPos;
			iTween.ScaleTo (selected, new Vector3 (0.8f, 0.8f, 1f), 0f);
		} else if (sel2ap.x == -1) {
			sel2ap = selected.GetComponent<PuzzleCube2D> ().actualPos;
			iTween.ScaleTo (selected, new Vector3 (0.8f, 0.8f, 1f), 0f);
			MoveTheCubes ();
		} else if (sel1ap == selected.GetComponent<PuzzleCube2D> ().actualPos) {
			sel1ap = new Vector2(-1,-1);
			iTween.ScaleTo (selected, new Vector3 (1f, 1f, 1f), 0f);
		} else if (sel2ap == selected.GetComponent<PuzzleCube2D> ().actualPos) {
			sel2ap = new Vector2(-1,-1);
			iTween.ScaleTo (selected, new Vector3 (1f, 1f, 1f), 0f);
		}
	}
	void MoveTheCubes() {
		eventsystem.SetActive (false);
		swappingcpnp ();
		checkAll ();
	}
	void swappingcpnp() {
		int i = (int)sel1ap.x;
		int j = (int)sel1ap.y;
		int n = (int)sel2ap.x;
		int m = (int)sel2ap.y;
		PuzzleCubes [i, j].GetComponent<RectTransform> ().localPosition = PuzzleCubePosition [n, m];
		PuzzleCubes [n, m].GetComponent<RectTransform> ().localPosition = PuzzleCubePosition [i, j];
		Vector2 currentpos = PuzzleCubes [i, j].GetComponent<PuzzleCube2D> ().currentPos;
		PuzzleCubes [i, j].GetComponent<PuzzleCube2D> ().currentPos = PuzzleCubes [n, m].GetComponent<PuzzleCube2D> ().currentPos;
		PuzzleCubes [n, m].GetComponent<PuzzleCube2D> ().currentPos = currentpos;
		PuzzleCubePosition [i, j] = PuzzleCubes [i, j].GetComponent<RectTransform> ().localPosition;
		PuzzleCubePosition [n, m] = PuzzleCubes [n, m].GetComponent<RectTransform> ().localPosition;
		iTween.ScaleTo (PuzzleCubes[i,j], new Vector3 (1f, 1f, 1f), 0f);
		iTween.ScaleTo (PuzzleCubes[n,m], new Vector3 (1f, 1f, 1f), 0f);
		sel1ap = sel2ap = new Vector2(-1,-1);
		eventsystem.SetActive (true);
	}
	public void BackToMainMenu() {
		Save ();
		SceneManager.LoadSceneAsync ("MainScene");
	}
	void checkAll() {
		bool complete = true;
		for (int i = 0; i < PuzzleCubes.GetLength (0); i++) {
			for (int j = 0; j < PuzzleCubes.GetLength (1); j++) {
				complete = PuzzleCubes [i, j].GetComponent<PuzzleCube2D> ().isCorrect;
				if (!complete) {
					break;
				}
			}
			if (!complete) {
				break;
			}
		}
		if (complete) {
			Save ();
			SceneManager.LoadSceneAsync ("FinalScene");
		}
	}
}
