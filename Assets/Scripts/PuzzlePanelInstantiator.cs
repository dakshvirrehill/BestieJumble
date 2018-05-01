using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePanelInstantiator : MonoBehaviour {
	public GameObject[] PuzzlePanelPrefabs;
	public GameObject PuzzleCanvas;
	// Use this for initialization
	void Start () {
		GameObject pp;
		if (SaveData.control.cubeTex.width > SaveData.control.cubeTex.height) {
			pp = Object.Instantiate (PuzzlePanelPrefabs [0], PuzzleCanvas.transform, false);
		} else if (SaveData.control.cubeTex.width < SaveData.control.cubeTex.height) {
			pp = Object.Instantiate (PuzzlePanelPrefabs [1], PuzzleCanvas.transform, false);
		} else {
			pp = Object.Instantiate (PuzzlePanelPrefabs [2], PuzzleCanvas.transform, false);
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
