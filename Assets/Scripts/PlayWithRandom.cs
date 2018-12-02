using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ServerManager;
using Navigator;
using System;
using System.IO;
using System.Net.Sockets;

public class PlayWithRandom : MonoBehaviour {
	//Player 1 is the player running this instance.
	//Player 2 is the randomly matched user
	public Button Rock_Button, Paper_Button, Scissors_Button;
	public Text Match_Number_Text, Help_Text, Player1_ID_Text, Player1_Number_Text, Player2_ID_Text, Player2_Number_Text;
	string wins = ""; //local reference to wins
    string losses = ""; //local reference to losses
	string player1Id = "";
    string player1Username = "";
    string player2Username = "";
	string player2Id = "";
	int matchNumber = 1;
	int sessionResponse = 2;
	int localPlayer1Win = 0;
	int localPlayer2Win = 0;
	ConnectionManager connectionManager;
    UserInfo userInfo;

    Socket playWithRandom;

	// Use this for initialization
	void Start () {
		//initially, store the necessary info (user_id) into local variable to be ready to pass to playWithAI()
        try {
            using (StreamReader streamReader = new StreamReader(Application.persistentDataPath + "/MyInfo.json")){
                String line = streamReader.ReadToEnd();
                streamReader.Close();
                userInfo = JsonUtility.FromJson<UserInfo>(line);
                player1Id = userInfo.getUserId();
                wins = userInfo.getWins();
                losses = userInfo.getLosses();
            }
        } catch(Exception e) {
            Debug.Log(e.Message, gameObject);
        }

        Debug.Log("Changing Player1 Name");
        Player1_ID_Text.text = player1Id;

        Debug.Log("Changing Help Text to show server is finding a match");
        Help_Text.text = "Finding a Random Player...";

        connectionManager = new ConnectionManager();
        if (connectionManager.StartClient() == 1) //successful start of client
        {
            string multiplayerSessionStartResponse = connectionManager.startPlayerWithRandom();
            Debug.Log(multiplayerSessionStartResponse);

            //playWithRandom = connectionManager.ClientListener();

            //this follow the sequence of MultiplayerSession.py in the server
            //send player1ID
            connectionManager.getResponse();
            connectionManager.sendResponse("1");
            connectionManager.sendUserId(player1Id);
            //receive other player's ID
            player2Id = connectionManager.getResponse();
            Player2_ID_Text.text = player2Id;

            //receive okay from the server to start game
            sessionResponse = int.Parse(connectionManager.getResponse());
            Debug.Log(sessionResponse);
            if (sessionResponse == 1) {
                Debug.Log("Multiplayer Session Start Complete. User can choose moves now");
                Help_Text.text = "Choose your move!";
                Debug.Log("add listener");
                Rock_Button.onClick.AddListener(delegate {TaskWithParameters("1");});
                Paper_Button.onClick.AddListener(delegate {TaskWithParameters("2");});
                Scissors_Button.onClick.AddListener(delegate {TaskWithParameters("3");});
                Debug.Log("done adding listener");
            } else {
                Debug.Log("REJECTED");
            }
        } else //failed start of client
        {
            Debug.Log("Failed to start ConnectionManager Client");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TaskWithParameters(string move) {
        //send the move to server
        Debug.Log(move);
        connectionManager.sendMove(move);
        Debug.Log("Sent Move");

        //receive response
        string stringResponse = connectionManager.getResponse();
        sessionResponse = int.Parse(stringResponse);
        Debug.Log(sessionResponse);

        //handle response
        if (sessionResponse == 0) { //a tie
            matchNumber++;
            Match_Number_Text.text = matchNumber.ToString();
        } else if (sessionResponse == 1) { //a win
            localPlayer1Win++;
            Player1_Number_Text.text = localPlayer1Win.ToString();
            if (localPlayer1Win == 2) {
                EndGame();
            } else {
                matchNumber++;
                Match_Number_Text.text = matchNumber.ToString();
            }
        } else if (sessionResponse == -1) { //a loss
            localPlayer2Win++;
            Player2_Number_Text.text = localPlayer2Win.ToString();
            if (localPlayer2Win == 2) {
                EndGame();
            } else {
                matchNumber++;
                Match_Number_Text.text = matchNumber.ToString();
            }
        }
    }

    public void EndGame() {
        sessionResponse = int.Parse(connectionManager.getResponse());
        connectionManager.LogOff();
        if (sessionResponse == 2) { //Player1 Won! Good ending.
            // localPlayer1Win++;
            Player1_Number_Text.text = localPlayer1Win.ToString();
            Help_Text.text = "You won!";

            //adjust local data
            int newWin = int.Parse(wins);
            newWin++;
            userInfo.setWins(newWin.ToString());
        } else if (sessionResponse == -2) { //Player2 Won! Bad ending.
            // localPlayer2Win++;
            Player2_Number_Text.text = localPlayer2Win.ToString();
            Help_Text.text = "You lost...";

            //adjust local data
            int newLoss = int.Parse(losses);
            newLoss++;
            userInfo.setLosses(newLoss.ToString());
        } else {
            Debug.Log("Something wrong");
        }
        string json = JsonUtility.ToJson(userInfo);
        File.WriteAllText(Application.persistentDataPath + "/MyInfo.json", json);

        //update to the DB
        connectionManager = new ConnectionManager();
        connectionManager.StartClient();
        string[] param = new string[4];
        param[0] = wins;
        param[1] = losses;
        param[2] = player1Id;
        string updateWinLossResponse = connectionManager.updateWinLoss(param);
        Debug.Log(updateWinLossResponse);

        //disable buttons
        Rock_Button.onClick.RemoveListener(delegate {TaskWithParameters("1");});
        Paper_Button.onClick.RemoveListener(delegate {TaskWithParameters("2");});
        Scissors_Button.onClick.RemoveListener(delegate {TaskWithParameters("3");});
    }
}
