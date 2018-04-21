using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
public class SaveData : MonoBehaviour {
	public static SaveData control;
	public string username;
	public Texture cubeTex;
	// Use this for initialization
	void Awake() {
		if (control == null) {
			DontDestroyOnLoad (gameObject);
			control = this;
		} else if (control != this) {
			Destroy (gameObject);
		}
	}
	public void Save(string name) {
		username = name;
		control.SaveToFile ();
	}
	public void SaveToFile() {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file;
		if (!File.Exists (Application.persistentDataPath + "/" + name + ".dat")) {
			file = File.Create (Application.persistentDataPath + "/" + name + ".dat");
		} else {
			file = File.Open (Application.persistentDataPath + "/" + name + ".dat", FileMode.Open);
		}
		SaveDataHolder data = new SaveDataHolder (username,cubeTex);
		bf.Serialize (file, data);
		file.Close ();
	}
	public void Load(string name) {
		BinaryFormatter bf = new BinaryFormatter ();
		if(File.Exists (Application.persistentDataPath + "/" + name + ".dat")) {
			FileStream file;
			file = File.Open (Application.persistentDataPath + "/" + name + ".dat",FileMode.Open);
			SaveDataHolder data = (SaveDataHolder)bf.Deserialize (file);
			file.Close ();
			username = data.username;
			cubeTex = data.cubeTex;
		} else {
			
		}
	}
}
class SaveDataHolder {
	public string username;
	public Texture cubeTex;
	public SaveDataHolder(string user,Texture ct) {
		this.username = user;
		this.cubeTex = ct;
	}
}