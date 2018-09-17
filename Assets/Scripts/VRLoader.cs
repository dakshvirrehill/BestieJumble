using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class VRLoader : MonoBehaviour {
	public GameObject infycanvas;
	// Use this for initialization
	void Start () {
		infycanvas = Object.Instantiate (infycanvas, new Vector3 (7.6f, 20f, -2.93f), Quaternion.Euler (0f, 0f, 0f));
		if (SaveData.control.VRWon) {
			for (int i = 0; i < 4; i++) {
				infycanvas.transform.GetChild (i).GetChild (0).GetChild (3).gameObject.GetComponent<TextMeshProUGUI> ().text = "Congratulations!!!! \n\nYou Have Won!!! \n\nLoading Final Scene... Please Wait...";
			}
			StartCoroutine (LoadingScene ("FinalScene"));
		} else {
			int n = 0;
			if (SaveData.control.cubeTex.width > 1024 || SaveData.control.cubeTex.height > 1024) {
				n = 9;
			} else {
				n = 6;
			}
			for (int i = 0; i < 4; i++) {
				infycanvas.transform.GetChild (i).GetChild (0).GetChild (3).gameObject.GetComponent<TextMeshProUGUI> ().text = "1. The Puzzle Cubes are being hidden in the world. \n2. You need to find them and place them in the grid. \n3. " + n + " cubes can be collected at once and you can change their positions even after placing them on the grid. \n4. You win after all cubes are at the correct position.";
			}
			StartCoroutine (LoadingScene ("3DPuzzleScene"));
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
	IEnumerator LoadingScene(string scenename) {
		yield return new WaitForSeconds (4f);
		AsyncOperation SceneLoad = SceneManager.LoadSceneAsync (scenename);
		while (!SceneLoad.isDone) {
			yield return null;
		}
	}
}
