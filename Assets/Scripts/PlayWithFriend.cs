using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ServerManager;
using Navigator;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using static SocketPasser;

public class PlayWithFriend : MonoBehaviour
{
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

    public GameObject PlayerSprite;//Current Sprite, Yours or Opponents
    public GameObject OpponentSprite; //Your Opponent's sprite
    public GameObject RockSkin;
    public GameObject PaperSkin;
    public GameObject ScissorsSkin;

    List<string> methodsToCall = new List<string>();


    // Use this for initialization
    void Start()
    {
        //initially, store the necessary info (user_id) into local variable to be ready to pass to playWithAI()
        Debug.Log("Starting the play with friends code");
        try
        {
            using (StreamReader streamReader = new StreamReader(Application.persistentDataPath + "/MyInfo.json"))
            {
                String line = streamReader.ReadToEnd();
                streamReader.Close();
                userInfo = JsonUtility.FromJson<UserInfo>(line);
                player1Id = userInfo.getUserId();
                player1Username = userInfo.getUsername();
                wins = userInfo.getWins();
                losses = userInfo.getLosses();
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message, gameObject);
        }

        Debug.Log("Changing Player1 Name");
        Player1_ID_Text.text = player1Username;

        Debug.Log("Changing Help Text to show server is finding a match");
        Help_Text.text = "Waiting for your friend...";

        var thread = new Thread(friendSession);
        thread.Start();
    }

    private void friendSession() {
        connectionManager = SocketPasser.getCM();

        //playWithRandom = connectionManager.ClientListener();

        //this follow the sequence of MultiplayerSession.py in the server
        //send player1ID
        connectionManager.getResponse();
        connectionManager.sendResponse("1");
        connectionManager.sendUserId(player1Id);
        //receive other player's ID
        player2Username = connectionManager.getResponse();

        //receive okay from the server to start game
        sessionResponse = int.Parse(connectionManager.getResponse());
        Debug.Log(sessionResponse);
        if (sessionResponse == 1)
        {
            methodsToCall.Add("readyToFriendPlay");
        }
        else
        {
            Debug.Log("REJECTED");
        }

        // player2Username = connectionManager.GetUsernameFromPlayerID(player2Id);
        // Player2_ID_Text.text = player2Username;
    }

    // Update is called once per frame
    void Update()
    {
        if (methodsToCall.Count > 0) {
            foreach (string s in methodsToCall) {
                Invoke(s, 0f);
            }
            methodsToCall.Clear();
        }
    }

    void readyToFriendPlay() {
        Debug.Log("Multiplayer Session Start Complete. User can choose moves now");
        Help_Text.text = "Choose your move!";
        Debug.Log("add listener");
        Rock_Button.onClick.AddListener(delegate { TaskWithParameters("1"); });
        Paper_Button.onClick.AddListener(delegate { TaskWithParameters("2"); });
        Scissors_Button.onClick.AddListener(delegate { TaskWithParameters("3"); });
        Debug.Log("done adding listener");
        Player2_ID_Text.text = player2Username;
    }

    public void TaskWithParameters(string move)
    {
        // ShowMove ShowMoveUI = new ShowMove();
        //send the move to server
        Debug.Log(move);
        connectionManager.sendMove(move);
        Debug.Log("Sent Move");

        //receive response
        string stringResponse = connectionManager.getResponse();
        sessionResponse = int.Parse(stringResponse);
        Debug.Log(sessionResponse);

        //handle response
        if (sessionResponse == 0)
        { //a tie
            matchNumber++;
            Match_Number_Text.text = matchNumber.ToString();
            this.Run(move, "T");
        }
        else if (sessionResponse == 1)
        { //a win
            localPlayer1Win++;
            Player1_Number_Text.text = localPlayer1Win.ToString();
            this.Run(move, "W");

            if (localPlayer1Win == 2)
            {
                EndGame(move);
            }
            else
            {
                matchNumber++;
                Match_Number_Text.text = matchNumber.ToString();
            }
        }
        else if (sessionResponse == -1)
        { //a loss
            localPlayer2Win++;
            Player2_Number_Text.text = localPlayer2Win.ToString();
            this.Run(move, "L");

            if (localPlayer2Win == 2)
            {
                EndGame(move);
            }
            else
            {
                matchNumber++;
                Match_Number_Text.text = matchNumber.ToString();
            }
        }
    }

    public void EndGame(string move)
    {
        sessionResponse = int.Parse(connectionManager.getResponse());
        connectionManager.LogOff();
        if (sessionResponse == 2)
        { //Player1 Won! Good ending.
            // localPlayer1Win++;
            Player1_Number_Text.text = localPlayer1Win.ToString();
            Help_Text.text = "You won!";
            this.Run(move, "W");
            //adjust local data
            int newWin = int.Parse(wins);
            newWin++;
            userInfo.setWins(newWin.ToString());
        }
        else if (sessionResponse == -2)
        { //Player2 Won! Bad ending.
            // localPlayer2Win++;
            Player2_Number_Text.text = localPlayer2Win.ToString();
            Help_Text.text = "You lost...";
            this.Run(move, "L");
            //adjust local data
            int newLoss = int.Parse(losses);
            newLoss++;
            userInfo.setLosses(newLoss.ToString());
        }
        else
        {
            Debug.Log("Something wrong");
        }
        

        //update to the DB
        connectionManager = new ConnectionManager();
        connectionManager.StartClient();
        string[] param = new string[4];
        param[0] = wins;
        param[1] = losses;
        param[2] = player1Id;
        string updateWinLossResponse = connectionManager.updateWinLoss(param);
        Debug.Log(updateWinLossResponse);

        //update client based on DB
        string userID = userInfo.getUserId();
        int clientStart = 0;
        while(clientStart != 1){
            clientStart = connectionManager.StartClient();
        }
        string updatedScore = connectionManager.GetScore(userID);
        userInfo.setScore(updatedScore);

        string json = JsonUtility.ToJson(userInfo);
        File.WriteAllText(Application.persistentDataPath + "/MyInfo.json", json);
        //disable buttons
        Rock_Button.onClick.RemoveListener(delegate { TaskWithParameters("1"); });
        Paper_Button.onClick.RemoveListener(delegate { TaskWithParameters("2"); });
        Scissors_Button.onClick.RemoveListener(delegate { TaskWithParameters("3"); });
    }


    //Show Move hard code sad life
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
}
