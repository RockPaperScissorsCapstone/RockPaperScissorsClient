using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerManager;
using Navigator;
using System;
using System.IO;
using UnityEngine.UI;
using UnityShortCuts;

public class PlayWithAI : MonoBehaviour {
    public Button Rock_Button, Paper_Button, Scissors_Button;
    public Text Match_Number_Text, Help_Text, Human_Number_Text, AI_Number_Text;
    string userId = "";
    string wins = "";
    string losses = "";
    int matchNumber = 1;
    int sessionResponse = 2;
    int playerWins = 0;
    int playerLosses = 0;
    int aiWins = 0;
    int aiLosses = 0;
    int winner = 0;

    ConnectionManager connectionManager;
    UserInfo userInfo;
    ShortCuts usc;

	// Use this for initialization
	void Start () {
        //initially, store the necessary info (user_id) into local variable to be ready to pass to playWithAI()
        try {
            Debug.Log("About access json file");
            string data = File.ReadAllText(Application.dataPath + "/MyInfo.json");
            //String line = streamReader.ReadToEnd();
            userInfo = JsonUtility.FromJson<UserInfo>(data);
            userId = userInfo.getUserId();
            wins = userInfo.getWins();
            losses = userInfo.getLosses();

            Debug.Log("About to add listners");
            /*Rock_Button.onClick.AddListener(delegate {TaskWithParameters("1");});
            Paper_Button.onClick.AddListener(delegate {TaskWithParameters("2");});
            Scissors_Button.onClick.AddListener(delegate {TaskWithParameters("3");});*/

            Debug.Log("About to create ConnectionManager");
            connectionManager = new ConnectionManager();
            if (connectionManager.StartClient() == 1) //successful start of client
            {
                Debug.Log("About to start Game Session");
                string response = connectionManager.startGameSession();

            } 
            else //failed start of client
            {
                Debug.Log("Failed to start ConnectionManager Client");
            }
            usc = new ShortCuts();
        } catch(Exception e) {
            Debug.Log("MyInfo.json could not be read.");
            Debug.Log(e.Message);
        }
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void TaskWithParameters(string move) {
        Debug.Log(move);
        connectionManager.sendUserId(userId);
        Debug.Log("Sent User Id");
        connectionManager.sendMove(move);
        Debug.Log("Sent Move");
        string computerMove = connectionManager.getResponse();
        Debug.Log(computerMove);

        // string playerWinResponse = connectionManager.getResponse();
        // string AIWinResponse = connectionManager.getResponse();

        string stringResponse = connectionManager.getResponse();


        sessionResponse = int.Parse(stringResponse);

        // Debug.Log(playerWinResponse);
        // Debug.Log(AIWinResponse);
        Debug.Log(sessionResponse);

        if (sessionResponse == 9) {
            stringResponse = connectionManager.getResponse();

            matchNumber++;
            Match_Number_Text.text = matchNumber.ToString();
            if(int.Parse(stringResponse) == 0)
            {
                aiWins++;
                aiLosses++;
                winner = 0;
                usc.updateTextValue("gameMatchResult", "AI Wins");
                usc.showTextObject("gameMatchResult");
            }
            else
            {
                playerWins++;
                playerLosses++;
                winner = 1;
                usc.updateTextValue("gameMatchResult", "Player Wins");
                usc.showTextObject("gameMatchResult");
            }
            Human_Number_Text.text = playerWins.ToString();
            AI_Number_Text.text = aiWins.ToString();
            EndGame(winner);

        } else {
            if(int.Parse(stringResponse) == 0)
            {
                aiWins++;
                aiLosses++;
                usc.updateTextValue("gameMatchResult", "AI Wins");
                usc.showTextObject("gameMatchResult");
            }
            else
            {
                playerWins++;
                playerLosses++;
                usc.updateTextValue("gameMatchResult", "Player Wins");
                usc.showTextObject("gameMatchResult");
            }
        }
    }

    public void EndGame(int winer) {
        string watedReceive = connectionManager.getResponse();
        if (winner == 1) {
            usc.hideTextObject("gameMatchResult");
            Help_Text.text = "You won!";
            int newWin = int.Parse(wins);
            newWin++;
            userInfo.setWins(newWin.ToString());
            wins = newWin.ToString();
        } else if (winner == 0){
            usc.hideTextObject("gameMatchResult");
            Help_Text.text = "AI won!";
            int newLosses = int.Parse(losses);
            newLosses++;
            userInfo.setLosses(newLosses.ToString());
            losses = newLosses.ToString();
        }
        string json = JsonUtility.ToJson(userInfo);
        File.WriteAllText(Application.dataPath + "/MyInfo.json", json);

        //update to the DB
        connectionManager = new ConnectionManager();
        connectionManager.StartClient();
        string[] param = new string[4];
        param[0] = wins;
        param[1] = losses;
        param[2] = userId;
        string updateWinLossResponse = connectionManager.updateWinLoss(param);
        Debug.Log(updateWinLossResponse);
    }
}
