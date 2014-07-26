using UnityEngine;
using System.Collections;

public class MenuControl : MonoBehaviour {
	public GUISkin menuSkin;
	private float buttonh = 50f;
	private float buttonw = 200f;
	public Texture2D parley;
	private int displayScene = 0;
	private Vector2 scrollpos;
	
	void OnGUI() {
		
		if (menuSkin!=null){
			GUI.skin=menuSkin;
		}
		float y = 0;
		
		GUI.DrawTexture(new Rect(Screen.width - Screen.width/4, Screen.height - Screen.height/5, Screen.width/4, Screen.height/5), parley);
		
		if(GUI.Button(new Rect((Screen.width/2) - (Screen.width/3) - (buttonw/2),(Screen.height/2) - (Screen.height/4) - (buttonh/2) + y,buttonw,buttonh),"Scene 1")){
			displayScene = 1;
		}
		y+=50;
		if(GUI.Button(new Rect((Screen.width/2) - (Screen.width/3) - (buttonw/2),(Screen.height/2) - (Screen.height/4) - (buttonh/2) + y,buttonw,buttonh),"Scene 2")){
			displayScene = 2;
		}
		
		if( Application.isWebPlayer ){
		}else{
			y+=50;
			if(GUI.Button(new Rect((Screen.width/2) - (Screen.width/3) - (buttonw/2),(Screen.height/2) - (Screen.height/4) - (buttonh/2) + y,buttonw,buttonh),"Quit")){
				Application.Quit();
			}
		}
		
		if(displayScene == 1){
			Rect windowRect = new Rect((Screen.width/2) - (Screen.width/5),(Screen.height/2) - (Screen.height/4) - (buttonh/2) - 25,Screen.width/2,Screen.height/2);
			GUILayout.BeginArea(windowRect,"Learning about Parley : Scene 1",GUI.skin.window);
			scrollpos = GUILayout.BeginScrollView(scrollpos);
			GUILayout.Label(
				"Section 1:\n" +
			 	"Basic dialog quest: see how dialogs and quests interact, and how quest events are handled. This consists of one dialog, two quests and a variaty of different interactions between GameEvents and Enviromental Information.",new GUILayoutOption[]{GUILayout.ExpandHeight(true),GUILayout.ExpandWidth(true)});
			GUILayout.Label(
				"Section 2:\n" +
				"Dialog interaction with story: see how dialogs and quests are used to create a simple story. Here you will see how two dialogs and a quest interact to create a story between two characters in the game.",new GUILayoutOption[]{GUILayout.ExpandHeight(true),GUILayout.ExpandWidth(true)});
			GUILayout.Label(
				"Section 3:\n" +
				"Multi person Dialogs: here a conversation between more than two characters is shown. You will see how the system switches between dialogs depending who is talking at any time.",new GUILayoutOption[]{GUILayout.ExpandHeight(true),GUILayout.ExpandWidth(true)});
			if(GUILayout.Button("Start Scene 1", new GUILayoutOption[]{})){
				Application.LoadLevel("Igg");
			}
			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}
		if(displayScene == 2){
			Rect windowRect = new Rect((Screen.width/2) - (Screen.width/5),(Screen.height/2) - (Screen.height/4) - (buttonh/2) - 25,Screen.width/2,Screen.height/2);
			GUILayout.BeginArea(windowRect,"Learning about Parley : Scene 2",GUI.skin.window);
			scrollpos = GUILayout.BeginScrollView(scrollpos);
			GUILayout.Label(
				"Section 1:\n" +
			 	"Quests integrated into Dialog: This demonstrates how quests can be tied directly into Conversations like most RPG's Youwill need to accept and turn each quest in at an NPC. This mode works fine along side the other modes of operation.",new GUILayoutOption[]{GUILayout.ExpandHeight(true),GUILayout.ExpandWidth(true)});
			if(GUILayout.Button("Start Scene ", new GUILayoutOption[]{})){
				Application.LoadLevel("IggMom");
			}
			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}
	}
}
