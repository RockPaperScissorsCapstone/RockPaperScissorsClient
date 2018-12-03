using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ServerManager;
using System;
using System.IO;

public class Logout : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//the user clicks the logout button on main menu to trigger this
	public void logout() {
		ConnectionManager connectionManager = new ConnectionManager();
		if (connectionManager.StartClient() == 1) { //successful connection to server
			//get username from stored user data
			string data = File.ReadAllText(Application.persistentDataPath + "/MyInfo.json");
			UserInfo playerinfo = JsonUtility.FromJson<UserInfo>(data);
			string username = playerinfo.getUsername();

			//tell server to logout
			string response = connectionManager.LogOut(username);
			Debug.Log(response);

			//clear stored MyInfo.json
			UserInfo emptyPlayer = new UserInfo();
			string emptyJSON = JsonUtility.ToJson(emptyPlayer);
			File.WriteAllText(Application.persistentDataPath + "/MyInfo.json", emptyJSON);

			//navigate back to LoginScreen
			SceneManager.LoadScene("LoginScreen");
		} else {
			Debug.Log("Connection Manager Failed to Start a Client");
		}
	}
}
