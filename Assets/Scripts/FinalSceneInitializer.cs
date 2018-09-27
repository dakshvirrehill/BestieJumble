using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class FinalSceneInitializer : MonoBehaviour {
	public GameObject[] WinningScenePrefabs; //Prefabs for winning scene
	private GameObject ToBeDestroyed; //The parent of all winning scene game objects
	// Use this for initialization
	void Start () {
		SceneStarter ();
	}
	//Instantiate Winning Scene Prefabs and Destroy Parent only
	public void SceneStarter() {
		if (XRSettings.loadedDeviceName.Equals ("cardboard")) {
			ToBeDestroyed = Object.Instantiate (WinningScenePrefabs [0], new Vector3 (0f, 0f, 0f), new Quaternion (0f, 0f, 0f, 0f));
		} else {
			ToBeDestroyed = Object.Instantiate (WinningScenePrefabs [1], new Vector3 (0f, 0f, 0f), new Quaternion (0f, 0f, 0f, 0f));
		}
		ToBeDestroyed.transform.DetachChildren ();
		Destroy (ToBeDestroyed);
	}
	// Update is called once per frame
	void Update () {
		
	}
}
