using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class FinalSceneInitializer : MonoBehaviour {
	public GameObject[] WinningScenePrefabs;
	private GameObject ToBeDestroyed;
	// Use this for initialization
	void Start () {
		SceneStarter ();
	}
	public void SceneStarter() {
		if (XRSettings.loadedDeviceName.Equals ("cardboard")) {
			ToBeDestroyed = Object.Instantiate (WinningScenePrefabs [0], new Vector3 (0f, 0f, 0f), new Quaternion (0f, 0f, 0f, 0f));
		} else {
			ToBeDestroyed = Object.Instantiate (WinningScenePrefabs [1], new Vector3 (0f, 0f, 0f), new Quaternion (0f, 0f, 0f, 0f));
			ToBeDestroyed.transform.DetachChildren ();
			Destroy (ToBeDestroyed);
			//ToBeDestroyed.transform.GetChild(6).gameObject.GetComponent<TwoDFinalLogic>().winningPanel=ToBeDestroyed.transform.GetChild (1).GetChild (1);
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
