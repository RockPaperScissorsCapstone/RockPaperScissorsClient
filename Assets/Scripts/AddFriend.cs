using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerManager;
using System.IO;
using UnityShortCuts;
using Navigator;

public class AddFriend : MonoBehaviour {

    string user_id;
    ShortCuts usc;
    string data;
    UserInfo playerinfo;

    public void AddNewFriend()
    {
        string[] param = new string[2];
        usc = new ShortCuts();
        
        data = File.ReadAllText(Application.dataPath + "/MyInfo.json");
        playerinfo = JsonUtility.FromJson<UserInfo>(data);
        param[0] = playerinfo.getUserId();
        Debug.Log(param[0]);

        param[1] = usc.InputValue("usernameFriend");
        Debug.Log(param[1]);


        Debug.Log(param);

    }
}
