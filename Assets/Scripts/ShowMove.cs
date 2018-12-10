using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowMove
{

    public GameObject PlayerSprite;//Current Sprite, Yours or Opponents
    public GameObject OpponentSprite; //Your Opponent's sprite
    public GameObject RockSkin;
    public GameObject PaperSkin;
    public GameObject ScissorsSkin;


    
    public void Run(string Move, string WINLOSS) 
    {
        PlayerSprite = GameObject.FindWithTag("UI_Show_move_YourSprite"); //Current Sprite, Yours or Opponents
        OpponentSprite = GameObject.FindWithTag("UI_Show_move_OpponentSprite");  //Your Opponent's sprite
        RockSkin = GameObject.FindWithTag("RockSKin");
        PaperSkin = GameObject.FindWithTag("PaperSkin");
        ScissorsSkin = GameObject.FindWithTag("ScissorSKin");

        SetSprites(Move, WINLOSS);
    }

    public void SetSprites(string Move, string WinLossSituation)
    {

        if(WinLossSituation == "W")//You Won
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
        else if(WinLossSituation=="L"){
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

        Image ChangedImage = PlayerSprite.GetComponent<Image>();
        Image TheImage = GameObjectImage.GetComponent<Image>();
        ChangedImage.sprite = TheImage.sprite;
    }

//     // Use this for initialization
//     //This will Toggle the move frame
    void Start () {

        PlayerSprite = GameObject.FindWithTag("UI_Show_move_YourSprite"); //Current Sprite, Yours or Opponents
        OpponentSprite = GameObject.FindWithTag("UI_Show_move_OpponentSprite");  //Your Opponent's sprite
        RockSkin = GameObject.FindWithTag("RockSKin"); 
        PaperSkin = GameObject.FindWithTag("PaperSkin"); 
        ScissorsSkin = GameObject.FindWithTag("ScissorSKin"); 


}

// 	// Update is called once per frame
	void Update () {



	}
}
