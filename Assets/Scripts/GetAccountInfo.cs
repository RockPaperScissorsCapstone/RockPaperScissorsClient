using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.IO;
using System.Text;
using ServerManager;
using UnityShortCuts;
public class GetAccountInfo : MonoBehaviour {

	// Use this for initialization
	string user_id;
	ShortCuts usc;
	string data;
	UserInfo playerinfo;
	void Start() 
	{
		ConnectionManager cm = new ConnectionManager();
		usc = new ShortCuts(); 
		data = File.ReadAllText(Application.persistentDataPath + "/MyInfo.json");
		Debug.Log(data);
		playerinfo = JsonUtility.FromJson<UserInfo>(data);
        // RefreshUserInfo(usc,playerinfo);
        Debug.Log(playerinfo.getFirstName());
		string param = playerinfo.getUsername();
		user_id = playerinfo.getUserId();
		int clientStart = 0;
        while(clientStart != 1){
            clientStart = cm.StartClient();
        }
		playerinfo.setScore(cm.GetScore(user_id));
		usc.updateInputValue("profileUserName", param);
		string[] tagNames = {"profileScore", "profileCurrency", "profileWins", "profileLosses"};
		string[] userParams = {playerinfo.getScore(), playerinfo.getCurrency(), playerinfo.getWins(), playerinfo.getLosses()};
		usc.updateTextValue(tagNames, userParams);
	}

    // public void RefreshUserInfo(ShortCuts usc, UserInfo playerinfo)
    // {
    //     string responses;
    //     ConnectionManager CM = new ConnectionManager();

    //     if (CM.StartClient() == 1)
    //     {
    //         responses = CM.GetAccountInfo(playerinfo.getUserId());
    //         if (responses.Length > 0)
    //         { //there is good response!
    //             for (int i = 0; i < 4; i++)
    //             {
    //                 Debug.Log(responses[i]);
    //             }
    //             UserInfo accountInfo = JsonUtility.FromJson<UserInfo>(responses);
    //             string json = JsonUtility.ToJson(accountInfo);
    //             StreamWriter sw = File.CreateText(Application.persistentDataPath + "/MyInfo.json");
    //             sw.Close();
    //             File.WriteAllText(Application.persistentDataPath + "/MyInfo.json", json);
    //         }
    //     }
    //     else
    //     {
    //         Debug.Log("Failed to start ConnectionsManager Client");
    //     }
    // }

	public void UpdateUserInfo () 
	{
		ConnectionManager CM = new ConnectionManager();
		int connectionResult = CM.StartClient();
		Debug.Log(connectionResult);
		string new_username = usc.InputValue("profileUserName");
		string[] param = {user_id, new_username};
		string updateResult = (CM.UpdateAccountInfo(param)).Trim();

		try
		{
			if(Convert.ToInt32(updateResult) == 1)
			{
				playerinfo.setUsername(new_username);
				Debug.Log(playerinfo.getUsername());
				string json = JsonUtility.ToJson(playerinfo);
        		StreamWriter sw = File.CreateText(Application.persistentDataPath + "/MyInfo.json");
        		sw.Close();
        		File.WriteAllText(Application.persistentDataPath + "/MyInfo.json", json);
			}
			else
			{
				Debug.Log(updateResult);
			}
		} 
		catch (OverflowException error) 
		{
			Debug.Log(updateResult);
		} 
		
	}

	// Update is called once per frame
	void Update () {
		
	}
}
