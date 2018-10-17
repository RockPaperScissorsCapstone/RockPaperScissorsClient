using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using ConnectionManager;

public class Submit : MonoBehaviour {
	public void sumbitIn(){
		string[] responses = new string[6];
		string[] param = new string[6];
		string passOne;
		string passTwo;
		int passWordCheck = 1;
		param[0] = InputValue("usernameInput");
		Debug.Log(param[0]);

		param[1] = InputValue("userEmailInput");
		Debug.Log(param[1]);

		param[2] = InputValue("userFNameInput");
		Debug.Log(param[2]);

		param[3] = InputValue("userLNameInput");
		Debug.Log(param[3]);

		passOne = InputValue("userPasswordInputOne");
		passTwo = InputValue("userPasswordInputTwo");
		if(string.Equals(passOne, passTwo)){
			param[4] = passOne;
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
					for(int i = 0; i < 5; i++){
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

	private string InputValue(string tagName){
		GameObject inputField = GameObject.FindGameObjectWithTag(tagName);
		InputField iput = inputField.GetComponent<InputField>();
		return iput.text.ToString();
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
}