using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using ConnectionManager;
using UnityShortCuts;

public class Submit : MonoBehaviour {

    UserInfo myinfo;

	public void sumbitIn(){
		UnityShortCuts.ShortCuts finder = new ShortCuts();
		string[] responses = new string[6];
		string[] param = new string[6];
		string passOne;
		string passTwo;
		int passWordCheck = 1;
		param[0] = finder.InputValue("usernameInput");
		Debug.Log(param[0]);

		param[1] = finder.InputValue("userEmailInput");
		Debug.Log(param[1]);

		param[2] = finder.InputValue("userFNameInput");
		Debug.Log(param[2]);

		param[3] = finder.InputValue("userLNameInput");
		Debug.Log(param[3]);

		passOne = finder.InputValue("userPasswordInputOne");
		passTwo = finder.InputValue("userPasswordInputTwo");
		if(string.Equals(passOne, passTwo)){
			param[4] = passOne;
            myinfo = new UserInfo(param[2], param[3], param[1], param[0]);
            SaveInfo(myinfo);
			Debug.Log(param[4]);
		}else{
			passWordCheck = 0;
		}

		//param[5] = InputValue("userAgeInput");
		//Debug.Log(param[5]);

		if(passWordCheck == 1){
			ConnectionsManager CM = new ConnectionsManager();
			int accepted = CM.StartClient();
			if(accepted == 1){
				responses = CM.SubmitRegisteration(param);
				if(responses.Length > 0){
					for(int i = 0; i < 7; i++){
						Debug.Log(responses[i]);
						if(responses[i].Equals("User added successfully")){
							Debug.Log("Catching that user was added and able to create an event here");
						}
					}
				}
			}
		}else{
			showError("passwordMatch");
		}

	}


	private void showError(string ErrorType){
		if(string.Equals(ErrorType,"passwordMatch")){
			GameObject inputPassFieldOne = GameObject.FindGameObjectWithTag("userPasswordInputOne");
			GameObject inputPassFieldTwo = GameObject.FindGameObjectWithTag("userPasswordInputTwo");
			GameObject errorPassMatchTextField = GameObject.FindGameObjectWithTag("errorPasswordMisMatch");
			InputField passOne = inputPassFieldOne.GetComponent<InputField>();
			InputField passTwo = inputPassFieldTwo.GetComponent<InputField>();
			Text errorText = errorPassMatchTextField.GetComponent<Text>();
			
			passOne.text = null;
			passTwo.text = null;
			errorText.enabled = true;


		}
	}

    public void SaveInfo(UserInfo playerinfo)
    {
        string json = JsonUtility.ToJson(playerinfo);
        StreamWriter sw = File.CreateText(Application.dataPath + "/MyInfo.json");
        sw.Close();
        File.WriteAllText(Application.dataPath + "/MyInfo.json", json);

    }
}