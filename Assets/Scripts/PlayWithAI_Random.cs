using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerManager;
using Navigator;
using System;
using System.IO;
using UnityEngine.UI;

public class PlayWithAI_Random : MonoBehaviour {
    bool play = false;
	public Button Rock_Button, Paper_Button, Scissors_Button, Play_Button;
	public Text Match_Number_Text, Help_Text, Human_Number_Text, AI_Number_Text, Player_Name;
    public Sprite playImage;
    public Sprite stopImage;
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
    private IEnumerator keepSendingMoves;
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

                Play_Button.onClick.AddListener(delegate {PlayRandomAgainstAI();});

                connectionManager = new ConnectionManager();
                if (connectionManager.StartClient() == 1) //successful start of client
                {
                    string sessionStartResponse = connectionManager.startPlayWithAIRandom();
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

    public void PlayRandomAgainstAI() {
        if (play) { //stop the simulation
            play = false;
            Play_Button.GetComponent<Image>().sprite = null;
            Play_Button.enabled = false;
        } else { //start the simulation
            play = true;
            Play_Button.GetComponent<Image>().sprite = stopImage;
            connectionManager.sendMove("1"); //this starts the while loop in server

            keepSendingMoves = keepSending();
            StartCoroutine(keepSendingMoves);
        }
    }

    private IEnumerator keepSending() {
        bool stopNOW = true;
        while (stopNOW) {
            yield return new WaitForSeconds(1f);
            Debug.Log("Play: " + play.ToString());
            connectionManager.sendUserId(userId);
            System.Random rnd = new System.Random();
            string nextMove = rnd.Next(1, 4).ToString();
            Debug.Log("Next Move: " + nextMove);
            connectionManager.sendMove(nextMove);
            matchNumber++;
            Match_Number_Text.text = matchNumber.ToString();
            string playerWinResponse = connectionManager.getResponse();
            string AIWinResponse = connectionManager.getResponse();
            localHumanWin = Convert.ToInt32(playerWinResponse);
            localAiWin = Convert.ToInt32(AIWinResponse);

            int previousHumanWin = Convert.ToInt32(Human_Number_Text.text);
            int previousAIWin = Convert.ToInt32(AI_Number_Text.text);

            if (localHumanWin > previousHumanWin) {
                this.Run(nextMove, "W");
            }
            else if (localAiWin > previousAIWin)
            {
                this.Run(nextMove, "L");
            }
            else
            {
                this.Run(nextMove, "T");
            }

            Human_Number_Text.text = playerWinResponse;
            AI_Number_Text.text = AIWinResponse;

            if (play == false) {
                stopNOW = false;
            } else {
                //this is the 'code' in server
            connectionManager.sendMove("1");
            }
        }
        connectionManager.sendMove("0");
        string response = connectionManager.getResponse();
        // connectionManager.LogOff();
        Debug.Log(response);
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
}
