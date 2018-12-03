// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class ShowMove : MonoBehaviour {

//     public GameObject PlayerSprite;//Current Sprite, Yours or Opponents
//     public GameObject OpponentSprite; //Your Opponent's sprite
//     public GameObject Panel; //The Panel
//     public GameObject WinLossText; //Text To show if you win/lose/tie
    

//     public void showPanel() //Shows Panel
//     {
//         Panel.gameObject.SetActive(true);
 
//     }
//     public void hidePanel() //Hides the panel
//     {
//         Panel.gameObject.SetActive(false);
//     }

//     public void SetSprites(string Move, string WinLossSituation)
//     {
        
//         if(WinLossSituation == "W")//You Won
//         {

//             WinLossText.text = "You Won";
//             if (Move == "1")//Set your sprite to Rock, opponent to Scissors
//             {

//                 SetPlayerSpriteToObjectSprite(GameObject.Find("Rock"));

//                 SetOpponentSpriteToObjectSprite(GameObject.Find("Scissors"));
//             }
//             else if (Move == "2")//You picked Paper, set your sprite to paper and enemies to rock
//             {

//                 SetPlayerSpriteToObjectSprite(GameObject.Find("Paper"));//Set Sprite Paper

//                 SetOpponentSpriteToObjectSprite(GameObject.Find("Rock"));//Set Sprite Rock
//             }
//             else//You Won with Scissors, set sprites
//             {

//                 SetPlayerSpriteToObjectSprite(GameObject.Find("Scissors"));//Set Sprite Scissors

//                 SetOpponentSpriteToObjectSprite(GameObject.Find("Paper"));//Set Sprite Paper
//             }
//         }
//         else if(WinLossSituation=="L"){

//             WinLossText.text = "You Lost";
//             if (Move == "1")//Set your sprite to Rock, opponent to Paper
//             {

//                 SetPlayerSpriteToObjectSprite(GameObject.Find("Rock"));

//                 SetOpponentSpriteToObjectSprite(GameObject.Find("Paper"));
//             }
//             else if (Move == "2")//You picked Paper, set your sprite to paper and enemies to Scissors
//             {

//                 SetPlayerSpriteToObjectSprite(GameObject.Find("Paper"));//Set Sprite Paper

//                 SetOpponentSpriteToObjectSprite(GameObject.Find("Scissors"));//Set Sprite Scissors
//             }
//             else//You Lost with Scissors, set sprites
//             {

//                 SetPlayerSpriteToObjectSprite(GameObject.Find("Scissors"));//Set Sprite Scissors

//                 SetOpponentSpriteToObjectSprite(GameObject.Find("Rock"));//Set Sprite Rock
//             }
//         }
//         else//Tie, change both sprites to the same thing
//         {
//             WinLossText.text = "You Tied";
//             if (Move == "1")//Set your sprite to Rock, opponent to Rock
//             {

//                 SetPlayerSpriteToObjectSprite(GameObject.Find("Rock"));

//                 SetOpponentSpriteToObjectSprite(GameObject.Find("Rock"));
//             }
//             else if (Move == "2")//You picked Paper, set your sprite to paper and enemies to Paper
//             {

//                 SetPlayerSpriteToObjectSprite(GameObject.Find("Paper"));//Set Sprite Paper

//                 SetOpponentSpriteToObjectSprite(GameObject.Find("Paper"));//Set Sprite Paper
//             }
//             else//You Tie with Scissors, set sprites
//             {

//                 SetPlayerSpriteToObjectSprite(GameObject.Find("Scissors"));//Set Sprite Scissors

//                 SetOpponentSpriteToObjectSprite(GameObject.Find("Scissors"));//Set Sprite Scissors
//             }
//         }

//     }

//     public  SetPlayerSpriteToObjectSprite(GameObject GameObjectImage)//Sets PlayerSprite to gameobject's image
//     {
//         PlayerSprite.sprite = GameObjectImage.getComponent<image>();
//     }
//     public SetOpponentSpriteToObjectSprite(GameObject GameObjectImage)//Sets Opponent's sprite to gameobject's image
//     {
//         OpponentSprite.sprite = GameObjectImage.getComponent<image>();
//     }

//     // Use this for initialization
//     //This will Toggle the move frame
//     void Start () {

//         Panel = GameObject.Find("MovePanel");
//         PlayerSprite = GameObject.Find("PlayerSprite");
//         OpponentSprite = GameObject.Find("OpponentSprite");
//         WinLossText = GameObject.Find("MoveWinLoss");
        

// 	}
	
// 	// Update is called once per frame
// 	void Update () {
		
// 	}
// }
