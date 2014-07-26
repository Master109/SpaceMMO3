using UnityEngine;
using System.Collections;

public class scoreBoard : MonoBehaviour
{
    /*  This file is part of the "Ultimate Unity networking project" by M2H (http://www.M2H.nl)
     * *  This project is available on the Unity Store. You are only allowed to use these
     * *  resources if you've bought them from the Unity Assets Store.
     * */

    public GUISkin skin;

    //static string newRecordMessage = "";

	private bool displayingHighscore = false;
	private GUIText highscoreText;
	
	private string playerName = "";
	
	private string scoreText = "Loading scores";
	
	private bool hidestats = true;
	private int scoreBoardHeight = 60;
	private GameSetup gameSetupScript;

	void Awake(){
		gameSetupScript = GetComponent<GameSetup>() as GameSetup;
		
		highscoreText = GetComponent<GUIText>() as GUIText;
		highscoreText.enabled=false;
		playerName= PlayerPrefs.GetString("playerName");
	}
	
	void OnGUI () {
		GUI.skin = skin;
		
		scoreText = "Scoreboard:\n";
		for(int i = 0; i < gameSetupScript.playerList.Count; i ++){
			FPSPlayerNode entry = gameSetupScript.playerList[i];
			scoreText += entry.playerName+" \t"+entry.kills+" kills "+entry.deaths+" deaths\n";		
		}
		
		GUILayout.BeginArea (new Rect ((Screen.width-185),20,175,scoreBoardHeight));
		GUILayout.Box(scoreText);
		GUILayout.EndArea ();
	}
	
	FPSPlayerNode GetPlayer(NetworkPlayer networkP){
		for (int i = 0; i < gameSetupScript.playerList.Count; i ++) {
			FPSPlayerNode playerInstance = gameSetupScript.playerList[i];
			if (networkP == playerInstance.networkPlayer) {
				return playerInstance;
			}
		}
		Debug.LogError("GetPlayer couldn't find player! "+networkP);
		return null;
	}
	
	public void LocalPlayerHasKilled(){
		
		FPSPlayerNode pNode = GetPlayer(Network.player);
		pNode.kills +=1;		
		
		//Overwrite the data of other players with the new correct score
		networkView.RPC("UpdateScore",RPCMode.All, Network.player, pNode.kills, pNode.deaths); 
	}
	
	public void LocalPlayerDied(){
		FPSPlayerNode pNode = GetPlayer(Network.player);
		pNode.deaths +=1;
		
		//Overwrite with new correct score
		networkView.RPC("UpdateScore",RPCMode.All, Network.player, pNode.kills, pNode.deaths); 
	}
	
	
	[RPC]
	void UpdateScore(NetworkPlayer player, int kills, int deaths){
		Debug.Log((Network.player==player)+"=local "+kills+"kills & deaths="+deaths);
		
		FPSPlayerNode pNode = GetPlayer(player);
		if(pNode!=null){
			pNode.kills=kills;
			pNode.deaths=deaths;		
		} else {
			Debug.LogError("Could not find network player "+player+" in the gamesetup playerlist!");
		}
		scoreBoardHeight = gameSetupScript.playerList.Count*15+40;		
	}
}

