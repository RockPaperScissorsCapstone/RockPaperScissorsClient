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
        string[] param = new string[3];
        string[] responses = new string[4];
        usc = new ShortCuts();

        data = File.ReadAllText(Application.persistentDataPath + "/MyInfo.json");
        playerinfo = JsonUtility.FromJson<UserInfo>(data);
        param[0] = playerinfo.getUserId();
        Debug.Log(param[0]);
        param[1] = usc.InputValue("usernameFriend");
        Debug.Log(param[1]);
        param[2] = "Friend Request";
        Debug.Log(param[2]);
        ConnectionManager CM = new ConnectionManager();
            if(CM.StartClient() == 1){
                responses = CM.ChallengeFriend(param);
                Debug.Log(responses[3]);
            }
        }
    }

