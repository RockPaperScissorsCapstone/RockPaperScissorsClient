using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerManager;
using System.IO;
using UnityShortCuts;
using Navigator;


public class AddFriend : MonoBehaviour {

    ShortCuts usc;
    string data;
    UserInfo playerinfo;

    public void AddMyNewFriend(){
        string[] param = new string[2];
        string[] responses = new string[4];
        usc = new ShortCuts();

        data = File.ReadAllText(Application.dataPath + "/MyInfo.json");
        playerinfo = JsonUtility.FromJson<UserInfo>(data);
        param[0] = playerinfo.getUsername();
        Debug.Log(param[0]);
        param[1] = usc.InputValue("usernameFriend");
        Debug.Log(param[1]);

        ConnectionManager CM = new ConnectionManager();
            if(CM.StartClient() == 1){
                responses = CM.AddNewFriend(param);
                Debug.Log(responses[3]);
            }
        }
    }

