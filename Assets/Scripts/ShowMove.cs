using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowMove : MonoBehaviour {

    public GameObject PlayerSprite;//Current Sprite, Yours or Opponents
    public GameObject OpponentSprite; //Your Opponent's sprite
    public GameObject Panel; //The Panel
    public GameObject WinLossText; //Text To show if you win/lose/tie
    public GameObject CloseButton; //CLoeBUTTON
    public Button TheButton;


    public void showPanel() //Shows Panel
    {
        
        Panel.gameObject.SetActive(true);

    }
    public void hidePanel() //Hides the panel
    {
        Panel.gameObject.SetActive(false);
    }

    public void Run(string Move, string WINLOSS) 
    {

        showPanel();
        SetSprites(Move, WINLOSS);

        
        TheButton.onClick.AddListener(hidePanel);


    }

    public void SetSprites(string Move, string WinLossSituation)
    {

        if(WinLossSituation == "W")//You Won
        {
            Debug.Log("You won!");
            Text TEXTSITUATION = WinLossText.GetComponent<Text>();
            TEXTSITUATION.text = "You Won";
            if (Move == "1")//Set your sprite to Rock, opponent to Scissors
            {

                SetPlayerSpriteToObjectSprite(GameObject.Find("RockSKin"));

                SetOpponentSpriteToObjectSprite(GameObject.Find("ScissorsSKin"));
            }
            else if (Move == "2")//You picked Paper, set your sprite to paper and enemies to rock
            {

                SetPlayerSpriteToObjectSprite(GameObject.Find("PaperSkin"));//Set Sprite Paper

                SetOpponentSpriteToObjectSprite(GameObject.Find("RockSKin"));//Set Sprite Rock
            }
            else//You Won with Scissors, set sprites
            {

                SetPlayerSpriteToObjectSprite(GameObject.Find("ScissorsSKin"));//Set Sprite Scissors

                SetOpponentSpriteToObjectSprite(GameObject.Find("PaperSkin"));//Set Sprite Paper
            }
        }
        else if(WinLossSituation=="L"){
            Text TEXTSITUATION = WinLossText.GetComponent<Text>();
            TEXTSITUATION.text =  "You Lost";
            if (Move == "1")//Set your sprite to Rock, opponent to Paper
            {

                SetPlayerSpriteToObjectSprite(GameObject.Find("RockSKin"));

                SetOpponentSpriteToObjectSprite(GameObject.Find("PaperSkin"));
            }
            else if (Move == "2")//You picked Paper, set your sprite to paper and enemies to Scissors
            {

                SetPlayerSpriteToObjectSprite(GameObject.Find("PaperSkin"));//Set Sprite Paper

                SetOpponentSpriteToObjectSprite(GameObject.Find("ScissorsSKin"));//Set Sprite Scissors
            }
            else//You Lost with Scissors, set sprites
            {

                SetPlayerSpriteToObjectSprite(GameObject.Find("ScissorsSKin"));//Set Sprite Scissors

                SetOpponentSpriteToObjectSprite(GameObject.Find("RockSKin"));//Set Sprite Rock
            }
        }
        else//Tie, change both sprites to the same thing
        {
            Text TEXTSITUATION = WinLossText.GetComponent<Text>();
            TEXTSITUATION.text= "You Tied";
            if (Move == "1")//Set your sprite to Rock, opponent to Rock
            {

                SetPlayerSpriteToObjectSprite(GameObject.Find("RockSKin"));

                SetOpponentSpriteToObjectSprite(GameObject.Find("RockSKin"));
            }
            else if (Move == "2")//You picked Paper, set your sprite to paper and enemies to Paper
            {

                SetPlayerSpriteToObjectSprite(GameObject.Find("PaperSkin"));//Set Sprite Paper

                SetOpponentSpriteToObjectSprite(GameObject.Find("PaperSkin"));//Set Sprite Paper
            }
            else//You Tie with Scissors, set sprites
            {

                SetPlayerSpriteToObjectSprite(GameObject.Find("ScissorsSKin"));//Set Sprite Scissors

                SetOpponentSpriteToObjectSprite(GameObject.Find("ScissorsSKin"));//Set Sprite Scissors
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

        Image ChangedImage = PlayerSprite.GetComponent<Image>();
        Image TheImage = GameObjectImage.GetComponent<Image>();
        ChangedImage.sprite = TheImage.sprite;
    }

//     // Use this for initialization
//     //This will Toggle the move frame
    void Start () {

        Panel = GameObject.Find("DisplayMovePanel");
        PlayerSprite = GameObject.Find("Your_move");
        OpponentSprite = GameObject.Find("Opponent_Move");
        WinLossText = GameObject.Find("Result_Text_Show_move");
        CloseButton = GameObject.Find("Close_Button_DisplayMove");
        TheButton = CloseButton.GetComponent<Button>();

	}

// 	// Update is called once per frame
	void Update () {

	}
}
