using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using ServerManager;
using UnityShortCuts;
using Navigator;

public class CreateAccount : MonoBehaviour 
{

    UserInfo myinfo;

	public void submitBTN()
	{
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
		if(string.Equals(passOne, passTwo))
		{
			param[4] = passOne;
            myinfo = new UserInfo(param[2], param[3], param[1], param[0]);
            //SaveInfo(myinfo);
			Debug.Log(param[4]);
		}
		else
		{
			passWordCheck = 0;
		}

		//param[5] = InputValue("userAgeInput");
		//Debug.Log(param[5]);

		if(passWordCheck == 1)
		{
			ConnectionManager CM = new ConnectionManager();
			SceneNavigator navi = new SceneNavigator();
			int accepted = CM.StartClient();
			if(accepted == 1)
			{
				responses = CM.SubmitRegisteration(param);
				if(responses.Length > 0)
				{
					/* for(int i = 0; i < 7; i++)
					{
						Debug.Log(responses[i]);
						string result = responses[i];
						Debug.Log(result.Trim());
						if(string.Equals(result, "1"))
						{
							Debug.Log("Catching that user was added and able to create an event here");
							navi.GoToScene("LoginScreen");
						}
						else
						{
							Debug.Log("Not catching success");
						}
					}*/
					int desiredResult = 1;
					int result = System.Convert.ToInt32(responses[6]);
					Debug.Log(result == desiredResult);
					if(result == desiredResult)
					{
						navi.GoToScene("LoginScreen");
					}
					else
					{
						Debug.Log("Failed to create Account");
					}
				}
			}
		}
		else
		{
			showError("passwordMatch");
		}

	}


	private void showError(string ErrorType)
	{
		if(string.Equals(ErrorType,"passwordMatch"))
		{
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