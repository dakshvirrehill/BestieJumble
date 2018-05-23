﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class VRPuzzleLogic : MonoBehaviour {
	public GameObject parent;
	public Shader shader;
	public GameObject NGLParent;
	private GameObject PuzzlePanel;
	private GameObject PuzzleCube;
	private GameObject[,] PuzzleCubes;
	private GameObject selector;
	private Vector3[,] PuzzleCubePosition;
	private GameObject[] NonGridLocations;
	private Vector3 firstCubeLocation;
	private int n;
	private GameObject[] selected;
	// Use this for initialization
	void Start () {
		SetNGLArray ();
		selected = new GameObject[n];
		parent.SetActive (true);
		PuzzleCube = parent.transform.GetChild (2).gameObject;
		selector = parent.transform.GetChild (3).gameObject;
		selector.SetActive (false);
		float z = 31f;
		float y = 8f;
		if (SaveData.control.cubeTex.width > 1024 || SaveData.control.cubeTex.height > 1024) {
			n = 9;
			selected = new GameObject[n];
			firstCubeLocation = new Vector3 (-16.8f, 26.2f, 36.3f);
			PuzzlePanel = parent.transform.GetChild (1).gameObject;
			Destroy (parent.transform.GetChild (0).gameObject);
			z = 37f;
			y = 3f;
		} else {
			n = 6;
			selected = new GameObject[n];
			firstCubeLocation = new Vector3 (-10.7f, 18.7f, 30.3f);
			PuzzlePanel = parent.transform.GetChild (0).gameObject;
			Destroy (parent.transform.GetChild (1).gameObject);
		}
		parent.transform.DetachChildren ();
		Destroy (parent);
		PuzzlePanel.transform.position=new Vector3(0,y,z);
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
		StartCoroutine (JumblerStarter ());
	}
	IEnumerator JumblerStarter() {
		yield return StartCoroutine(MoveToCenter());
		yield return StartCoroutine (JumblePuzzle ());
	}
	IEnumerator MoveToCenter() {
		for (int i = 0; i < n; i++) {
			for (int j = 0; j < n; j++) {
				iTween.MoveTo (PuzzleCubes [i, j], iTween.Hash ("x", PuzzlePanel.transform.position.x, "y", PuzzlePanel.transform.position.y, "z", PuzzlePanel.transform.position.z - 3, "time", 3f));
			}
		}
		yield return new WaitForSeconds (1f);
	}
	IEnumerator JumblePuzzle() {
		System.Random r = new System.Random ();
		for (int i = 0; i < n; i++) {
			for (int j = 0; j < n; j++) {
				int ngv = r.Next (0, 81);
				while (NonGridLocations [ngv].GetComponent<NonGridIsUsed> ().isUsed) {
					ngv=(ngv+1)%81;
				}
				NonGridLocations [ngv].GetComponent<NonGridIsUsed> ().isUsed = true;
				PuzzleCubes [i, j].GetComponent<PuzzleCube2D> ().ngv = ngv;
				EventTrigger.Entry entry = new EventTrigger.Entry ();
				entry.eventID = EventTriggerType.PointerClick;
				entry.callback.AddListener ((data) => {
			    	MoveCubeToSelectorUI ((PointerEventData)data, (GameObject)PuzzleCubes [i, j]);
				});
				PuzzleCubes [i, j].GetComponent<EventTrigger> ().triggers.Add (entry);
				iTween.MoveTo (PuzzleCubes [i, j], iTween.Hash("position",NonGridLocations [ngv].transform.position,"time", 5f));
				selector.SetActive (true);
				GameObject Sel = Object.Instantiate (selector, PuzzlePanel.transform, false);
				Sel.name = "Selector " + i + " " + (j + 1);
				Sel.GetComponent<VRGridSelector> ().pos = new Vector2 (i, j);
				selector.SetActive (false);
				Sel.transform.localPosition = PuzzleCubePosition [i, j];
				EventTrigger.Entry entree = new EventTrigger.Entry ();
				entree.eventID = EventTriggerType.PointerClick;
				entree.callback.AddListener ((data) => {
					ActivateSelectorUI ((PointerEventData)data, (GameObject)Sel);
				});
				Sel.GetComponent<EventTrigger> ().triggers.Add (entree);
			}
		}
		yield return new WaitForSeconds (1f);
	}
	public void ActivateSelectorUI(PointerEventData eventData, GameObject Sel) {
		
	}
	public void MoveCubeToSelectorUI(PointerEventData eventData, GameObject PC) {
		
	}
	// Update is called once per frame
	void Update () {
		
	}
}
