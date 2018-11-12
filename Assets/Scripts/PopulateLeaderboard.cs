using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityShortCuts;
using ServerManager;
using UnityEngine.UI;

public class PopulateLeaderboard : MonoBehaviour {

    string data;
    string response;
    UserInfo playerinfo;

    enum user { userName, userPoints} //userPoints is 0, userName is 1

    // Use this for initialization
    void Start () {
        GameObject leaderboardContent = GameObject.FindGameObjectWithTag("leaderboardContent");


        //start the Connections Manager to get leaderboard data
        ConnectionManager CM = new ConnectionManager();
        if(CM.StartClient() == 1)
        {
            response = CM.GetLeaderboard();
        }

        //response = "500,player1;400,another user;300,player2"; // for testing
        int count = 0;
        string[] leaderboardPlayers = response.Split(';');
        foreach (string User in leaderboardPlayers) {
            string[] UserData = User.Split(',');
            ++count;
  
            GameObject leaderboardItem = (GameObject)Instantiate(Resources.Load("Prefabs/Leaderboard_Item"));
            GameObject place = leaderboardItem.transform.Find("Leaderboard_Place").gameObject;
            GameObject points = leaderboardItem.transform.Find("Leaderboard_Points").gameObject;
            GameObject name = leaderboardItem.transform.Find("Leaderboard_Name").gameObject;

            place.GetComponent<Text>().text = "" + count;
            points.GetComponent<Text>().text = UserData[(int)user.userPoints];
            name.GetComponent<Text>().text = UserData[(int)user.userName];

            leaderboardItem.transform.parent = leaderboardContent.transform;
            leaderboardItem.transform.localScale = new Vector3(1, 1, 1);
        }
        
        

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
