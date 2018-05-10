using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCube2D : MonoBehaviour {
	public Vector2 currentPos;
	public Vector2 actualPos;
	public bool isCorrect;
	// Use this for initialization
	void Start () {
		isCorrect = false;
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
