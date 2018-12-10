using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerManager;
using Navigator;
using System;
using System.IO;
using UnityEngine.UI;

public class PlayWithAI_Random : MonoBehaviour {
	public Button Rock_Button, Paper_Button, Scissors_Button;
	public Text Match_Number_Text, Help_Text, Human_Number_Text, AI_Number_Text, Player_Name;
	string userId = "";
    string wins = "";
    string losses = "";
    string skintag = "";
    string username = "";
    int matchNumber = 1;
    int sessionResponse = 2;
    int localAiWin = 0;
    int localHumanWin = 0;
	public GameObject PlayerSprite;//Current Sprite, Yours or Opponents
    public GameObject OpponentSprite; //Your Opponent's sprite
    public GameObject RockSkin;
    public GameObject PaperSkin;
    public GameObject ScissorsSkin;
	ConnectionManager connectionManager;
    UserInfo userInfo;
	Skin skin;

	// Use this for initialization
	void Start () {
		//initially, store the necessary info (user_id) into local variable to be ready to pass to playWithAI()
        try {
            using (StreamReader streamReader = new StreamReader(Application.persistentDataPath + "/MyInfo.json")){
                String line = streamReader.ReadToEnd();
                streamReader.Close();
                userInfo = JsonUtility.FromJson<UserInfo>(line);
                userId = userInfo.getUserId();
                wins = userInfo.getWins();
                losses = userInfo.getLosses();
                skintag = userInfo.getSkintag();
                username = userInfo.getUsername();
				skin = new Skin(skintag);
				Skin.setButtonSkin(Rock_Button, Paper_Button, Scissors_Button, skin);

                Player_Name.text = username;

                connectionManager = new ConnectionManager();
                if (connectionManager.StartClient() == 1) //successful start of client
                {
                    string sessionStartResponse = connectionManager.startGameSession();
                    Debug.Log(sessionStartResponse);
                } else //failed start of client
                {
                    Debug.Log("Failed to start ConnectionManager Client");
                }
            }
        } catch(Exception e) {
            Debug.Log(e.Message, gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
