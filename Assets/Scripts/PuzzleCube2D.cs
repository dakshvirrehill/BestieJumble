using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCube2D : MonoBehaviour {
	public Vector2 currentPos;
	public Vector2 actualPos;
	public int ngv;
	public bool isCorrect;
	public int selectedPos;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
	public void UpdateIsCorrect() {
		if (currentPos == actualPos) {
			isCorrect = true;
		} else {
			isCorrect = false;
		}
	}
}
