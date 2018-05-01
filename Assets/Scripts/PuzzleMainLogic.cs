using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PuzzleMainLogic : MonoBehaviour {
	public string OrientationAndScale;
	private GameObject[,] PuzzleCubes;
	private Vector2[,] PuzzleCubePosition;
	// Use this for initialization
	void Start () {
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
		for (int i = 0; i < n; i++) {
			for (int j = 0; j < m; j++) {
				PuzzleCubes [i,j] = gameObject.transform.GetChild (count).gameObject;
				count++;
				PuzzleCubes [i, j].GetComponent<RawImage> ().texture = SaveData.control.cubeTex;
				PuzzleCubes [i, j].GetComponent<PuzzleCube2D> ().actualPos = new Vector2 (i, j);
				PuzzleCubePosition [i,j] = new Vector2(PuzzleCubes [i,j].GetComponent<RectTransform> ().localPosition.x,PuzzleCubes [i,j].GetComponent<RectTransform> ().localPosition.y);
				//Debug.Log (PuzzleCubePosition [i, j]);
			}
		}
		StartCoroutine (JumblePuzzle());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	IEnumerator JumblePuzzle() {
		
		yield return new WaitForSeconds (5.0f);
	}
}
