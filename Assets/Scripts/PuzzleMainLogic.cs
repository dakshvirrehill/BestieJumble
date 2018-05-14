using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
public class PuzzleMainLogic : MonoBehaviour {
	public string OrientationAndScale;
	private GameObject[,] PuzzleCubes;
	private Vector2[,] PuzzleCubePosition;
	//private GameObject selected1,selected2;
	private Vector2 sel1ap, sel2ap;
	private GameObject eventsystem;
	private bool won;
	// Use this for initialization
	void Start () {
		won = false;
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
		//Debug.Log ("n= " + n + " , m= " + m);
		PuzzleCubes = new GameObject[n, m];
		PuzzleCubePosition = new Vector2[n, m];
		int count = 0;
		//Debug.Log ("Starting PuzzleCubePositions");
		for (int i = 0; i < n; i++) {
			for (int j = 0; j < m; j++) {
				//Debug.Log ("count= "+count);
				PuzzleCubes [i, j] = gameObject.transform.GetChild (count).gameObject;
				//Debug.Log (PuzzleCubes [i, j].name);
				count++;
				//	Debug.Log ("in here");	
				PuzzleCubes [i, j].GetComponent<RawImage> ().texture = SaveData.control.cubeTex;
				PuzzleCubes [i, j].GetComponent<PuzzleCube2D> ().actualPos = new Vector2 (i, j);
				PuzzleCubes [i, j].GetComponent<PuzzleCube2D> ().currentPos = new Vector2 (i, j);
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
		gameObject.transform.GetChild (count + 4).gameObject.GetComponent<Button> ().onClick.AddListener (() => QuitGame ());
		if (SaveData.control.Puzzle2DCubePositions == null) {	
			JumblePuzzle ();
		} else {
			SavedPuzzle ();
		}
	}
	public void Save() {
		SaveData.control.Puzzle2DCubePositions = new Vector2?[PuzzleCubePosition.GetLength(0),PuzzleCubePosition.GetLength(1)];
		for (int i = 0; i < PuzzleCubePosition.GetLength (0); i++) {
			for (int j = 0; j < PuzzleCubePosition.GetLength (1); j++) {
				SaveData.control.Puzzle2DCubePositions [i, j] = (Vector2?)PuzzleCubes [i, j].GetComponent<PuzzleCube2D>().currentPos;
			}
		}
		SaveData.control.Save (SaveData.control.username);
	}
	// Update is called once per frame
	void Update () {
		if (won) {
			Save ();
			SceneManager.LoadSceneAsync ("FinalScene");
		}
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
					StartCoroutine(swappingcpnp ());
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
		StartCoroutine (CallSwapAndCheck ());
	}
	IEnumerator CallSwapAndCheck() {
		yield return StartCoroutine(swappingcpnp ());
		yield return StartCoroutine(checkAll ());
	}
	IEnumerator swappingcpnp() {
		int i = (int)sel1ap.x;
		int j = (int)sel1ap.y;
		int n = (int)sel2ap.x;
		int m = (int)sel2ap.y;
		PuzzleCubes [i, j].GetComponent<RectTransform> ().localPosition = PuzzleCubePosition [n, m];
		PuzzleCubes [n, m].GetComponent<RectTransform> ().localPosition = PuzzleCubePosition [i, j];
		Vector2 currentpos = PuzzleCubes [i, j].GetComponent<PuzzleCube2D> ().currentPos;
		PuzzleCubes [i, j].GetComponent<PuzzleCube2D> ().currentPos = PuzzleCubes [n, m].GetComponent<PuzzleCube2D> ().currentPos;
		PuzzleCubes [n, m].GetComponent<PuzzleCube2D> ().currentPos = currentpos;
		PuzzleCubes [i, j].GetComponent<PuzzleCube2D> ().UpdateIsCorrect ();
		PuzzleCubes [n, m].GetComponent<PuzzleCube2D> ().UpdateIsCorrect ();
		PuzzleCubePosition [i, j] = PuzzleCubes [i, j].GetComponent<RectTransform> ().localPosition;
		PuzzleCubePosition [n, m] = PuzzleCubes [n, m].GetComponent<RectTransform> ().localPosition;
		iTween.ScaleTo (PuzzleCubes[i,j], new Vector3 (1f, 1f, 1f), 0f);
		iTween.ScaleTo (PuzzleCubes[n,m], new Vector3 (1f, 1f, 1f), 0f);
		sel1ap = sel2ap = new Vector2(-1,-1);
		eventsystem.SetActive (true);
		yield return new WaitForSeconds (0f);
	}
	public void BackToMainMenu() {
		Save ();
		SceneManager.LoadSceneAsync ("MainScene");
	}
	IEnumerator checkAll() {
		bool complete = true;
		for (int i = 0; i < PuzzleCubes.GetLength (0); i++) {
			for (int j = 0; j < PuzzleCubes.GetLength (1); j++) {
				complete = PuzzleCubes [i, j].GetComponent<PuzzleCube2D> ().isCorrect;
				//Debug.Log (complete);
				if (!complete) {
					break;
				}
			}
			if (!complete) {
				break;
			}
		}
		won = complete;
		yield return new WaitForSeconds (0f);
	}
	public void QuitGame() {
		Save ();
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
	void SavedPuzzle() {
		int n = PuzzleCubes.GetLength (0);
		int m = PuzzleCubes.GetLength (1);
		for (int i = 0; i < n; i++) {
			for (int j = 0; j < m; j++) {
				Vector2 a = PuzzleCubes [i, j].GetComponent<PuzzleCube2D> ().currentPos = SaveData.control.Puzzle2DCubePositions [i, j].Value;
				PuzzleCubes [i, j].GetComponent<PuzzleCube2D> ().UpdateIsCorrect ();
				PuzzleCubes [i, j].GetComponent<RectTransform> ().localPosition = PuzzleCubePosition [(int)a.x,(int)a.y];
			}
		}
		for (int i = 0; i < n; i++) {
			for (int j = 0; j < m; j++) {
				PuzzleCubePosition[i,j]=PuzzleCubes [i, j].GetComponent<RectTransform> ().localPosition;
			}
		}
	}
}
