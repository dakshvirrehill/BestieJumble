using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class PuzzleMainLogic : MonoBehaviour {
	public string OrientationAndScale;
	private GameObject[,] PuzzleCubes;
	private Vector2[,] PuzzleCubePosition;
	private GameObject selected1,selected2; 
	// Use this for initialization
	void Start () {
		selected1 = selected2 = null;
		int n=0,m=0;
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
		PuzzleCubes= new GameObject[n,m];
		PuzzleCubePosition = new Vector2[n,m];
		int count = 0;
		//Debug.Log ("Starting PuzzleCubePositions");
		for (int i = 0; i < n; i++) {
			for (int j = 0; j < m; j++) {
				PuzzleCubes [i,j] = gameObject.transform.GetChild (count).gameObject;
				count++;
				PuzzleCubes [i, j].GetComponent<RawImage> ().texture = SaveData.control.cubeTex;
				PuzzleCubes [i, j].GetComponent<PuzzleCube2D> ().actualPos = new Vector2 (i, j);
				PuzzleCubes [i, j].GetComponent<PuzzleCube2D> ().currentPos = new Vector2 (i, j);
				PuzzleCubePosition [i,j] = new Vector2(PuzzleCubes [i,j].GetComponent<RectTransform> ().localPosition.x,PuzzleCubes [i,j].GetComponent<RectTransform> ().localPosition.y);
			//	Debug.Log ("Value for i=" + i + " and j=" + j);
			//	Debug.Log (PuzzleCubePosition [i, j]);
			}
		}
		JumblePuzzle();
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
					Vector2 actualposij = PuzzleCubes [i, j].GetComponent<PuzzleCube2D> ().actualPos;
					Vector2 currentposmn = PuzzleCubes [n, m].GetComponent<PuzzleCube2D> ().currentPos;
					if (actualposij.x == currentposmn.x) {
						n = (n + 1) % row;
					}
					if (actualposij.x == currentposmn.x) {
						m = (m + 1) % column;
					}
					PuzzleCubes [i, j].GetComponent<PuzzleCube2D> ().currentPos = PuzzleCubes [n, m].GetComponent<PuzzleCube2D> ().currentPos;
					PuzzleCubes [n, m].GetComponent<PuzzleCube2D> ().currentPos = currentpos;
					PuzzleCubes [i, j].GetComponent<RectTransform> ().localPosition = PuzzleCubePosition [n, m];
					PuzzleCubes [n, m].GetComponent<RectTransform> ().localPosition = PuzzleCubePosition [i, j];
					PuzzleCubePosition [i, j] = PuzzleCubes [i, j].GetComponent<RectTransform> ().localPosition;
					PuzzleCubePosition [n, m] = PuzzleCubes [n, m].GetComponent<RectTransform> ().localPosition;
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
		if (selected1 == null) {
			selected1 = selected;
			iTween.ScaleTo (selected, new Vector3 (0.8f, 0.8f, 1f), 0f);
		} else if (selected2 == null) {
			selected2 = selected;
			iTween.ScaleTo (selected, new Vector3 (0.8f, 0.8f, 1f), 0f);
			MoveTheCubes ();
		} else if (selected1 == selected) {
			selected1 = null;
			iTween.ScaleTo (selected, new Vector3 (1f, 1f, 1f), 0f);
		} else if (selected2 == selected) {
			selected2 = null;
			iTween.ScaleTo (selected, new Vector3 (1f, 1f, 1f), 0f);
		} else {
			//play sound here
		}
	}
	void MoveTheCubes() {
		EventSystem.current.gameObject.SetActive (false);
	}
}
