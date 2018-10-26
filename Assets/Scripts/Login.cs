using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using ConnectionManager;

public class Login : MonoBehaviour {

	public void login() {
		//this is the parameter sent to the server to verify credentials for login
		string[] param = new string[2];
		string[] responses = new string[4];

		//get the input field value from Login Scene
		param[0] = InputValue("Username_Input");
		param[1] = InputValue("Password_Input");

		//start the Connections Manager
		ConnectionsManager CM = new ConnectionsManager();
		if (CM.StartClient() == 1) {
			responses = CM.SubmitLogin(param);
			if (responses.Length > 0) { //there is good response!
				for (int i = 0; i < 4; i++) {
					Debug.Log(responses[i]);
				}
			}
		} else {
			Debug.Log("Failed to start ConnectionsManager Client");
		}
	}

	//this helper function gets the string text value of the gameobject with type of InputField
	private string InputValue(string name){
		GameObject inputField = GameObject.Find(name);
		InputField iput = inputField.GetComponent<InputField>();
		return iput.text.ToString();
	}
}
