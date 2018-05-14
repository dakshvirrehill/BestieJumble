using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System;
public class SaveData : MonoBehaviour {
	public static SaveData control;
	public string username;
	public Texture cubeTex;
	public Vector2?[,] Puzzle2DCubePositions;
	// Use this for initialization
	void Awake() {
		//Debug.Log (Application.persistentDataPath);
		if (control == null) {
			DontDestroyOnLoad (gameObject);
			control = this;
		} else if (control != this) {
			Destroy (gameObject);
		}
	}
	public void Save(string named) {
		username = named;
		SaveToFile ();
	}
	public void SaveToFile() {
		BinaryFormatter bf = new BinaryFormatter ();
		SurrogateSelector surrogateSelector = new SurrogateSelector();
		surrogateSelector.AddSurrogate(typeof(Vector2),new StreamingContext(StreamingContextStates.All),new Vector2SurrogateSelector());
		bf.SurrogateSelector = surrogateSelector;
		FileStream file;
		if (!File.Exists (Application.persistentDataPath + "/" + username + ".dat")) {
			file = File.Create (Application.persistentDataPath + "/" + username + ".dat");
		} else {
			file = File.Open (Application.persistentDataPath + "/" + username + ".dat", FileMode.Open);
		}
		Texture2D tex1 = (Texture2D)cubeTex;
		Texture2D tex = new Texture2D (tex1.width, tex1.height, TextureFormat.RGB24, false);
		tex.SetPixels (0, 0, tex1.width, tex1.height, tex1.GetPixels ());
		tex.Apply ();
		SaveDataHolder data = new SaveDataHolder (username,tex.EncodeToJPG(),Puzzle2DCubePositions);
		bf.Serialize (file, data);
		//Destroy (tex);
		file.Close ();
	}
	public void Load(string named) {
		BinaryFormatter bf = new BinaryFormatter ();
		SurrogateSelector surrogateSelector = new SurrogateSelector();
		surrogateSelector.AddSurrogate(typeof(Vector2),new StreamingContext(StreamingContextStates.All),new Vector2SurrogateSelector());
		bf.SurrogateSelector = surrogateSelector;
		if(File.Exists (Application.persistentDataPath + "/" + named + ".dat")) {
			FileStream file;
			file = File.Open (Application.persistentDataPath + "/" + named + ".dat",FileMode.Open);
			SaveDataHolder data = (SaveDataHolder)bf.Deserialize (file);
			file.Close ();
			username = data.username;
			Texture2D tex = new Texture2D (0, 0);
			tex.LoadImage(data.cubeTex);
			cubeTex = tex;
			//Destroy (tex);
			Puzzle2DCubePositions=new Vector2?[data.Puzzle2DCubePositions.GetLength(0),data.Puzzle2DCubePositions.GetLength(1)];
			for (int i = 0; i < Puzzle2DCubePositions.GetLength (0); i++) {
				for (int j = 0; j < Puzzle2DCubePositions.GetLength (1); j++) {
					Puzzle2DCubePositions [i, j] = (Vector2?)data.Puzzle2DCubePositions [i, j];
				}
			}
		}
	}
	public void DeleteAllSaveData(string[] names) {
		foreach(string name in names) {
			if (File.Exists (Application.persistentDataPath + "/" + name + ".dat")) {
				File.Delete (Application.persistentDataPath + "/" + name + ".dat");
			}
		}
	}
}
[Serializable]
class SaveDataHolder {
	public string username;
	public Byte[] cubeTex;
	public Vector2[,] Puzzle2DCubePositions;
	public SaveDataHolder(string user,Byte[] ct, Vector2?[,] Puzzle2DCubePositions) {
		this.username = user;
		this.cubeTex = ct;
		this.Puzzle2DCubePositions = new Vector2[Puzzle2DCubePositions.GetLength (0), Puzzle2DCubePositions.GetLength (1)];
		for (int i = 0; i < Puzzle2DCubePositions.GetLength (0); i++) {
			for (int j = 0; j < Puzzle2DCubePositions.GetLength (1); j++) {
				this.Puzzle2DCubePositions[i,j] = Puzzle2DCubePositions[i,j].Value;
			}
		}
	}
}