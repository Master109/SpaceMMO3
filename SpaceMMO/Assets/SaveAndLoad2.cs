using UnityEngine;
using System.Collections;

public class SaveAndLoad2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnApplicationQuit ()
	{
		LevelSerializer.SavedGames.Clear();
		LevelSerializer.SaveGame("SpaceMMO");
	}
}
