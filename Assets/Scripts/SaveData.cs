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
	private Texture defaultTex;
	public Vector2?[,] Puzzle2DCubePositions;
	public Vector2?[,] PuzzleVRCubePositions;
	public int?[] PuzzleVRNonGridPositions;
	public GameObject PreLoader;
	// Use this for initialization
	void Awake() {
		if (control == null) {
			DontDestroyOnLoad (gameObject);
			control = this;
			//control.username = "";
			control.defaultTex = control.cubeTex;
			control.Puzzle2DCubePositions = null;
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
		SaveDataHolder data = new SaveDataHolder (username,tex.EncodeToJPG(),Puzzle2DCubePositions,PuzzleVRCubePositions,PuzzleVRNonGridPositions);
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
			if (data.Puzzle2DCubePositions.GetLength (0) == 6) {
				Puzzle2DCubePositions = new Vector2?[data.Puzzle2DCubePositions.GetLength (0), data.Puzzle2DCubePositions.GetLength (1)];
				for (int i = 0; i < Puzzle2DCubePositions.GetLength (0); i++) {
					for (int j = 0; j < Puzzle2DCubePositions.GetLength (1); j++) {
						Puzzle2DCubePositions [i, j] = (Vector2?)data.Puzzle2DCubePositions [i, j];
					}
				}
			}
			if (data.PuzzleVRCubePositions.GetLength (0) >= 6) {
				PuzzleVRCubePositions = new Vector2?[data.PuzzleVRCubePositions.GetLength (0), data.PuzzleVRCubePositions.GetLength (1)];
				for (int i = 0; i < Puzzle2DCubePositions.GetLength (0); i++) {
					for (int j = 0; j < PuzzleVRCubePositions.GetLength (1); j++) {
						PuzzleVRCubePositions [i, j] = (Vector2?)data.PuzzleVRCubePositions [i, j];
					}
				}
			}
			if (data.PuzzleVRNonGridPositions.Length >= 36) {
				PuzzleVRNonGridPositions = new int?[data.PuzzleVRNonGridPositions.Length];
				for (int i = 0; i < PuzzleVRNonGridPositions.Length; i++) {
						PuzzleVRNonGridPositions [i] = (int?)data.PuzzleVRNonGridPositions [i];
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
		DefaultEverything ();
	}
	public void DefaultEverything() {
		SaveData.control.username = "";
		SaveData.control.cubeTex = defaultTex;
		SaveData.control.Puzzle2DCubePositions = null;
		SaveData.control.PuzzleVRCubePositions = null;
		SaveData.control.PuzzleVRNonGridPositions = null;
	}
}
[Serializable]
class SaveDataHolder {
	public string username;
	public Byte[] cubeTex;
	public Vector2[,] Puzzle2DCubePositions;
	public Vector2[,] PuzzleVRCubePositions;
	public int[] PuzzleVRNonGridPositions;
	public SaveDataHolder(string user,Byte[] ct, Vector2?[,] Puzzle2DCubePositions, Vector2?[,] PuzzleVRCubePositions, int?[] PuzzleVRNonGridPositions) {
		this.username = user;
		this.cubeTex = ct;
		if (Puzzle2DCubePositions != null) { 
			this.Puzzle2DCubePositions = new Vector2[Puzzle2DCubePositions.GetLength (0), Puzzle2DCubePositions.GetLength (1)];
			for (int i = 0; i < Puzzle2DCubePositions.GetLength (0); i++) {
				for (int j = 0; j < Puzzle2DCubePositions.GetLength (1); j++) {
					this.Puzzle2DCubePositions[i,j] = Puzzle2DCubePositions[i,j].Value;
				}
			}
		} else {
			this.Puzzle2DCubePositions = new Vector2[1, 1];
			this.Puzzle2DCubePositions [0, 0] = new Vector2 (-1, -1);
		}
		if (PuzzleVRCubePositions != null) {
			this.PuzzleVRCubePositions = new Vector2[PuzzleVRCubePositions.GetLength (0), PuzzleVRCubePositions.GetLength (1)];
			for (int i = 0; i < PuzzleVRCubePositions.GetLength (0); i++) {
				for (int j = 0; j < PuzzleVRCubePositions.GetLength (1); j++) {
					this.PuzzleVRCubePositions[i,j] = PuzzleVRCubePositions[i,j].Value;
				}
			}
		} else {
			this.PuzzleVRCubePositions = new Vector2[1, 1];
			this.PuzzleVRCubePositions [0, 0] = new Vector2 (-1, -1);
		}
		if (PuzzleVRNonGridPositions != null) {
			this.PuzzleVRNonGridPositions = new int[PuzzleVRNonGridPositions.Length];
			for (int i = 0; i < PuzzleVRNonGridPositions.Length; i++) {
				this.PuzzleVRNonGridPositions[i] = PuzzleVRNonGridPositions[i].Value;
			}
		} else {
			this.PuzzleVRNonGridPositions = new int[1];
			this.PuzzleVRNonGridPositions [0] = -1;
		}
	}
}