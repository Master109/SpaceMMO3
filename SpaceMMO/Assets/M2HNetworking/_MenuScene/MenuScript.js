/*  This file is part of the "Ultimate Unity networking project" by M2H (http://www.M2H.nl)
 *  This project is available on the Unity Store. You are only allowed to use these
 *  resources if you've bought them from the Unity Assets Store.
 */
#pragma strict

function OnGUI () {
	
	GUILayout.BeginArea(Rect(Screen.width/2-225,0,450,Screen.height));
	
	GUILayout.FlexibleSpace();	
	
	GUILayout.BeginHorizontal();
	GUILayout.FlexibleSpace();	
	GUILayout.Label("Main Menu");
	GUILayout.FlexibleSpace();
	GUILayout.EndHorizontal();
	
	GUILayout.Space(10);
	
	GUILayout.BeginHorizontal();
	
	
	GUILayout.BeginVertical();
	if(GUILayout.Button("MMO (Coming in a long time)")){
		Application.LoadLevel(4);
	}
	if(GUILayout.Button("Matchmaking")){
		Application.LoadLevel(1);
	}
	if(GUILayout.Button("Survival (Singleplayer)")){
		Application.LoadLevel(3);
	}
	GUILayout.EndVertical();
	
	GUILayout.EndHorizontal();
	GUILayout.FlexibleSpace();
	GUILayout.EndArea();
	
}