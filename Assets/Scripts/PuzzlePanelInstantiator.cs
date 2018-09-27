using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePanelInstantiator : MonoBehaviour {
	public GameObject[] PuzzlePanelPrefabs; //2D puzzle panels for landscape portrait and square depending on uploaded image
	public GameObject PuzzleCanvas; //Puzzle canvas position
	public GameObject PuzzlePanel; //current puzzle panel object
	// Use this for initialization
	void Start () {
		if (SaveData.control.cubeTex.width > SaveData.control.cubeTex.height) {
			PuzzlePanel = Object.Instantiate (PuzzlePanelPrefabs [0], PuzzleCanvas.transform, false);
		} else if (SaveData.control.cubeTex.width < SaveData.control.cubeTex.height) {
			PuzzlePanel = Object.Instantiate (PuzzlePanelPrefabs [1], PuzzleCanvas.transform, false);
		} else {
			PuzzlePanel = Object.Instantiate (PuzzlePanelPrefabs [2], PuzzleCanvas.transform, false);
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
