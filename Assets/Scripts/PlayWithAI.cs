using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerManager;
using Navigator;
using System;
using System.IO;
using UnityEngine.UI;

public class PlayWithAI : MonoBehaviour {
    public Button Rock_Button, Paper_Button, Scissors_Button;
    public Text Match_Number_Text, Help_Text, Human_Number_Text, AI_Number_Text;
    string userId = "";
    string wins = "";
    string losses = "";
    int matchNumber = 1;
    int sessionResponse = 2;

    ConnectionManager connectionManager;
    UserInfo userInfo;

	// Use this for initialization
	void Start () {
        //initially, store the necessary info (user_id) into local variable to be ready to pass to playWithAI()
        try {
            using (StreamReader streamReader = new StreamReader(Application.dataPath + "/MyInfo.json")){
                String line = streamReader.ReadToEnd();
                userInfo = JsonUtility.FromJson<UserInfo>(line);
                userId = userInfo.getUserId();
                wins = userInfo.getWins();
                losses = userInfo.getLosses();

                Rock_Button.onClick.AddListener(delegate {TaskWithParameters("1");});
                Paper_Button.onClick.AddListener(delegate {TaskWithParameters("2");});
                Scissors_Button.onClick.AddListener(delegate {TaskWithParameters("3");});

                connectionManager = new ConnectionManager();
                if (connectionManager.StartClient() == 1) //successful start of client
                {
                    connectionManager.startGameSession();

                } else //failed start of client
                {
                    Debug.Log("Failed to start ConnectionManager Client");
                }
            }
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

        // string playerWinResponse = connectionManager.getResponse();
        // string AIWinResponse = connectionManager.getResponse();
        string stringResponse = connectionManager.getResponse();

        sessionResponse = int.Parse(stringResponse);

        // Debug.Log(playerWinResponse);
        // Debug.Log(AIWinResponse);
        Debug.Log(sessionResponse);

        if (sessionResponse == 2) {
            matchNumber++;
            Match_Number_Text.text = matchNumber.ToString();
            // Human_Number_Text.text = playerWinResponse;
            // AI_Number_Text.text = AIWinResponse;
        } else {
            Debug.Log(sessionResponse);
            EndGame();
        }
    }

    public void EndGame() {
        if (sessionResponse == 1) {
            Help_Text.text = "You won!";
            int newWin = int.Parse(wins);
            newWin++;
            userInfo.setWins(newWin.ToString());
            wins = newWin.ToString();
        } else if (sessionResponse == 0){
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
