using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class VRPuzzleLogic : MonoBehaviour {
	public GameObject AllPuzzlesAndCubePrefabs;
	public Shader shader;
	private GameObject PuzzlePanel;
	private GameObject PuzzleCube;
	private GameObject[,] PuzzleCubes;
	private GameObject selector;
	private Vector2[,] PuzzleCubePosition;
	private Vector3 firstCubeLocation;
	private int n;
	private int m;
	// Use this for initialization
	void Start () {
		GameObject parent=Object.Instantiate(AllPuzzlesAndCubePrefabs,new Vector3(0,0,0),Quaternion.Euler(0,0,0));
		PuzzleCube = parent.transform.GetChild (2).gameObject;
		selector = parent.transform.GetChild (3).gameObject;
		selector.SetActive (false);
		PuzzleCube.AddComponent<PuzzleCube2D> ();
		float z = 31f;
		float y = 8f;
		if (SaveData.control.cubeTex.width > SaveData.control.cubeTex.height) {
			n = 9;
			m = 9;
			firstCubeLocation = new Vector3 (-16.8f, 26.2f, 36.3f);
			PuzzlePanel = parent.transform.GetChild (1).gameObject;
			Destroy (parent.transform.GetChild (0).gameObject);
			z = 37f;
			y = 3f;
		} else /*if (SaveData.control.cubeTex.width < SaveData.control.cubeTex.height) {
			n = 6;
			m = 4;
			firstCubeLocation = new Vector3 (-6.5f, 18.7f, 30.3f);
			PuzzlePanel = parent.transform.GetChild (0).gameObject;
			Destroy (parent.transform.GetChild (1).gameObject);
			Destroy (parent.transform.GetChild (2).gameObject);
		} else*/ {
			n = 6;
			m = 6;
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
	void MakePuzzle() {
		Vector2 scale = new Vector2 (1 / (float)m, 1 / (float)n);
		PuzzleCubes = new GameObject[n, m];
		PuzzleCubePosition = new Vector2[n, m];
		scale = new Vector2 (scale.x, 1);
		PuzzleCubes [0, 0] = PuzzleCube;
		PuzzleCubePosition [0, 0] = firstCubeLocation;
		for (int i = 0; i < n; i++) {
			for (int j = 0; j < m; j++) {
				if (!(i == 0 && j == 0)) {
					PuzzleCubes [i, j] = Object.Instantiate (PuzzleCube, PuzzlePanel.transform, false);	
					firstCubeLocation = firstCubeLocation + new Vector3 (4.2f, 0, 0);
					PuzzleCubes [i, j].transform.localPosition = firstCubeLocation;
					PuzzleCubePosition [i, j] = firstCubeLocation;
				}
				Material pc = new Material (shader);
				pc.mainTexture = SaveData.control.cubeTex;
				pc.mainTextureOffset = scale;
				pc.mainTextureScale = new Vector2 (1.0f / n, 1.0f / m);
				PuzzleCubes [i, j].GetComponent<Renderer> ().material = pc;
				scale = scale + new Vector2 (1 / (float)m, 0f);
			}
			scale = new Vector2 (1/(float)m, scale.y - 1 / (float)n);
			firstCubeLocation = firstCubeLocation + new Vector3 (-m * 4.2f, -4.2f, 0);
		}
		GameObject[] ShiftingRow = new GameObject[m];
		for (int j = 0; j < m; j++) {
			ShiftingRow [j] = PuzzleCubes [0, j];
		}
		for (int i = 1; i < n; i++) {
			for(int j = 0; j < m; j++) {
				PuzzleCubes [i, j].transform.localPosition = PuzzleCubePosition [i - 1, j];
				PuzzleCubes [i - 1, j] = PuzzleCubes [i, j];
			}
		}
		for (int j = 0; j < m; j++) {
			PuzzleCubes [n - 1, j] = ShiftingRow [j];
			PuzzleCubes [n - 1, j].transform.localPosition = PuzzleCubePosition [n - 1, j];
		}
		ShiftingRow = new GameObject[n];
		for (int i = 0; i < n; i++) {
			ShiftingRow [i] = PuzzleCubes [i, m - 1];
		}
		for (int j = m - 2; j >= 0; j--) {
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
		JumblePuzzle ();
	}
	void JumblePuzzle() {
		
	}
	// Update is called once per frame
	void Update () {
		
	}
}
