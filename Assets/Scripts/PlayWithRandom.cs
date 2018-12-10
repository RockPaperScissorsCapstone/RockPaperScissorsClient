﻿using System.Collections;
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
    Skin skin;

    public GameObject PlayerSprite;//Current Sprite, Yours or Opponents
    public GameObject OpponentSprite; //Your Opponent's sprite
    public GameObject RockSkin;
    public GameObject PaperSkin;
    public GameObject ScissorsSkin;

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
                string skintag = userInfo.getSkintag();
                skin = new Skin(skintag);
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
                Skin.setButtonSkin(Rock_Button, Paper_Button, Scissors_Button, skin);
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
    public void Run(string Move, string WINLOSS)
    {

        SetSprites(Move, WINLOSS);
    }

    public void SetSprites(string Move, string WinLossSituation)
    {

        if (WinLossSituation == "W")//You Won
        {

            if (Move == "1")//Set your sprite to Rock, opponent to Scissors
            {

                SetPlayerSpriteToObjectSprite(RockSkin);

                SetOpponentSpriteToObjectSprite(ScissorsSkin);
            }
            else if (Move == "2")//You picked Paper, set your sprite to paper and enemies to rock
            {

                SetPlayerSpriteToObjectSprite(PaperSkin);//Set Sprite Paper

                SetOpponentSpriteToObjectSprite(RockSkin);//Set Sprite Rock
            }
            else//You Won with Scissors, set sprites
            {

                SetPlayerSpriteToObjectSprite(ScissorsSkin);//Set Sprite Scissors

                SetOpponentSpriteToObjectSprite(PaperSkin);//Set Sprite Paper
            }
        }
        else if (WinLossSituation == "L")
        {
            if (Move == "1")//Set your sprite to Rock, opponent to Paper
            {

                SetPlayerSpriteToObjectSprite(RockSkin);

                SetOpponentSpriteToObjectSprite(PaperSkin);
            }
            else if (Move == "2")//You picked Paper, set your sprite to paper and enemies to Scissors
            {

                SetPlayerSpriteToObjectSprite(PaperSkin);//Set Sprite Paper

                SetOpponentSpriteToObjectSprite(ScissorsSkin);//Set Sprite Scissors
            }
            else//You Lost with Scissors, set sprites
            {

                SetPlayerSpriteToObjectSprite(ScissorsSkin);//Set Sprite Scissors

                SetOpponentSpriteToObjectSprite(RockSkin);//Set Sprite Rock
            }
        }
        else//Tie, change both sprites to the same thing
        {


            if (Move == "1")//Set your sprite to Rock, opponent to Rock
            {

                SetPlayerSpriteToObjectSprite(RockSkin);

                SetOpponentSpriteToObjectSprite(RockSkin);
            }
            else if (Move == "2")//You picked Paper, set your sprite to paper and enemies to Paper
            {

                SetPlayerSpriteToObjectSprite(PaperSkin);//Set Sprite Paper

                SetOpponentSpriteToObjectSprite(PaperSkin);//Set Sprite Paper
            }
            else//You Tie with Scissors, set sprites
            {

                SetPlayerSpriteToObjectSprite(ScissorsSkin);//Set Sprite Scissors

                SetOpponentSpriteToObjectSprite(ScissorsSkin);//Set Sprite Scissors
            }
        }

    }

    public void SetPlayerSpriteToObjectSprite(GameObject GameObjectImage)//Sets PlayerSprite to gameobject's image
    {
        Image ChangedImage = PlayerSprite.GetComponent<Image>();
        Image TheImage = GameObjectImage.GetComponent<Image>();
        ChangedImage.sprite = TheImage.sprite;
    }
    public void SetOpponentSpriteToObjectSprite(GameObject GameObjectImage)//Sets Opponent's sprite to gameobject's image
    {

        Image ChangedImage = OpponentSprite.GetComponent<Image>();
        Image TheImage = GameObjectImage.GetComponent<Image>();
        ChangedImage.sprite = TheImage.sprite;
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
            this.Run(move, "T");
        } else if (sessionResponse == 1) { //a win
            localPlayer1Win++;
            Player1_Number_Text.text = localPlayer1Win.ToString();
            this.Run(move, "W");
            if (localPlayer1Win == 2) {
                EndGame();
            } else {
                matchNumber++;
                Match_Number_Text.text = matchNumber.ToString();
            }
        } else if (sessionResponse == -1) { //a loss
            localPlayer2Win++;
            Player2_Number_Text.text = localPlayer2Win.ToString();
            this.Run(move, "L");
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

            //update currency
            string[] currencyParam = new string[2];
            currencyParam[0] = player1Id;
            currencyParam[1] = player2Id;

            ConnectionManager CM = new ConnectionManager();
            CM.StartClient();
            Debug.Log("trying to update currency");
            string updatedCurrency = CM.UpdateCurrency(currencyParam); //updates and receives updated currency as response
            Debug.Log(updatedCurrency);
            Debug.Log("setting currency");
            userInfo.setCurrency(updatedCurrency);
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

        
        connectionManager = new ConnectionManager();
        connectionManager.StartClient();
        // //update to the DB Win and Loss
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
