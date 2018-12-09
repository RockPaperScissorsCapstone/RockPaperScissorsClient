using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using ServerManager;
using UnityShortCuts;
using Navigator;
using System;

public class CreateAccount : MonoBehaviour
{

    UserInfo myinfo;
    UnityShortCuts.ShortCuts finder = new ShortCuts();
    public void submitBTN()
    {
        string[] responses = new string[6];
        string[] param = new string[5];
        string passOne;
        string passTwo;
        bool validation = true;
        param[0] = finder.InputValue("usernameInput");
        Debug.Log(param[0]);

        param[1] = finder.InputValue("userEmailInput");
        Debug.Log(param[1]);

        param[2] = finder.InputValue("userFNameInput");
        Debug.Log(param[2]);

        param[3] = finder.InputValue("userLNameInput");
        Debug.Log(param[3]);

        passOne = finder.InputValue("userPasswordInputOne");
        Debug.Log(passOne);
        passTwo = finder.InputValue("userPasswordInputTwo");
        Debug.Log(passTwo);

        if (string.Equals(passOne, passTwo))
        {
            param[4] = passOne;
            myinfo = new UserInfo(param[2], param[3], param[1], param[0]);
            //SaveInfo(myinfo);
            Debug.Log(param[4]);
        }
        else
        {
            param[4] = "";
        }

        if (finder.toggleValue("overThirteenFlag"))
        {
            finder.disableObject("AgeLimitError", "Text");
            validation = validatePackage(param);
            if (validation)
            {
                ConnectionManager CM = new ConnectionManager();
                SceneNavigator navi = new SceneNavigator();
                int accepted = CM.StartClient();
                if (accepted == 1)
                {
                    responses = CM.SubmitRegisteration(param);
                    if (responses.Length > 0)
                    {
                        int desiredResult = 0;
						int duplicateUsername = 1;
						int duplicateEmail = 2;
						try{
                        int result = System.Convert.ToInt32(responses[6]);
                        Debug.Log(result == desiredResult);
                        if (result == desiredResult)
                        {
                            navi.GoToScene("LoginScreen");
                        }
                        else if(result == duplicateUsername)
                        {
                            Debug.Log("Failed to create Account duplicate Username");
							showError("DuplicateUserName");
                        }
						else if(result == duplicateEmail)
						{
							Debug.Log("Failed to create Account duplicate Email");
							showError("DuplicateEmail");
						}
						}catch (Exception err){
							Debug.Log(responses[6]);
						}
                    }
                }
            }
        }
        else
        {
			validation = validatePackage(param);
            showError("AgeLimit");
        }

    }

    public bool validatePackage(string[] param)
    {
        bool response = true;
        string[] errorTypes = { "userNameError", "emailError", "fNameError", "lNameError", "errorPasswordMisMatch" };
        for (int i = 0; i < param.Length; i++)
        {
            if (param[i] == "")
            {
                Debug.Log("Showing errors");
                showError(errorTypes[i]);
                response = false;
            }
            else
            {
                finder.disableObject(errorTypes[i], "Text");
            }
        }

        return response;
    }


    private void showError(string ErrorType)
    {
        if (string.Equals(ErrorType, "errorPasswordMisMatch"))
        {
            string[] nullableValues = { "", "" };
            string[] inputTagNames = { "userPasswordInputOne", "userPasswordInputTwo" };
            finder.updateInputValue(inputTagNames, nullableValues);
            finder.updateTextValue("errorPasswordMisMatch", "* Enter Valid Password");
            finder.enableObject("errorPasswordMisMatch", "Text");
        }
        else if (string.Equals(ErrorType, "AgeLimit"))
        {
            finder.updateTextValue("AgeLimitError", "You Must Be 13 Years or Older to register for this game");
            finder.enableObject("AgeLimitError", "Text");
        }
        else if (string.Equals(ErrorType, "userNameError"))
        {
            finder.enableObject("userNameError", "Text");
        }
        else if (string.Equals(ErrorType, "emailError"))
        {
            finder.enableObject("emailError", "Text");
        }
        else if (string.Equals(ErrorType, "fNameError"))
        {
            finder.enableObject("fNameError", "Text");
        }
        else if (string.Equals(ErrorType, "lNameError"))
        {
            finder.enableObject("lNameError", "Text");
        }
		else if (string.Equals(ErrorType, "DuplicateUserName"))
        {
			finder.updateTextValue("userNameError", "* Username is not available");
            finder.enableObject("userNameError", "Text");
        }
		else if (string.Equals(ErrorType, "DuplicateEmail"))
        {
			finder.updateTextValue("emailError", "* Email already in use");
            finder.enableObject("emailError", "Text");
        }
    }
}