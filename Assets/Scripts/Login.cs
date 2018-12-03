using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using ServerManager;
using UnityShortCuts;
using Navigator;

public class Login : MonoBehaviour {

	public void login() {
		//this is the parameter sent to the server to verify credentials for login
		string[] param = new string[2];
		string[] responses = new string[4];
		ShortCuts usc = new ShortCuts();

		//get the input field value from Login Scene
		param[0] = usc.InputValue("loginUserName");
		Debug.Log(param[0]);
		param[1] = usc.InputValue("loginPassword");
		Debug.Log(param[1]);
		if(!param[0].Contains(" ") && !param[1].Contains(" "))
		{
		//start the Connections Manager
			ConnectionManager CM = new ConnectionManager();
			if (CM.StartClient() == 1) 
			{
				responses = CM.SubmitLogin(param);
				if (responses.Length > 0) 
				{ //there is good response!
					for (int i = 0; i < 4; i++) 
					{
						Debug.Log(responses[i]);
					}
					UserInfo accountInfo = JsonUtility.FromJson<UserInfo>(responses[3]);
					SaveInfo(accountInfo);
					SceneNavigator navi = new SceneNavigator();
					navi.GoToScene("MainMenu");
				}
			} 
			else 
			{
				Debug.Log("Failed to start ConnectionsManager Client");
			}
		}
	}

	private void SaveInfo(UserInfo playerinfo)
    {
        string json = JsonUtility.ToJson(playerinfo);
        StreamWriter sw = File.CreateText(Application.persistentDataPath + "/MyInfo.json");
        sw.Close();
        File.WriteAllText(Application.persistentDataPath + "/MyInfo.json", json);

    }

}
