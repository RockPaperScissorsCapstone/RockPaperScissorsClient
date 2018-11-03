using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerManager;
using System.IO;
using UnityShortCuts;
using Navigator;

public class AddFriend : MonoBehaviour {

    public GameObject addFriendPanel;
    ShortCuts usc;
    string data;
    UserInfo playerinfo;
    bool foo;

    public void Start()
    {
        if (!foo)
        {
            HideAddFriendDialogueBox();
        }
    }

    public void AddNewFriend()
    {
        string[] param = new string[2];
        usc = new ShortCuts();

        data = File.ReadAllText(Application.dataPath + "/MyInfo.json");
        playerinfo = JsonUtility.FromJson<UserInfo>(data);
        param[0] = playerinfo.getUsername();
        Debug.Log(param[0]);
        param[1] = usc.InputValue("usernameFriend");
        Debug.Log(param[1]);

        ConnectionManager CM = new ConnectionManager();
        if (CM.StartClient() == 1)
        {
           string response = CM.AddNewFriend(param);
           Debug.Log(response);
        }

    }

    public void HideAddFriendDialogueBox()
    {
        
        addFriendPanel.SetActive(false);
        foo = false;
    }

    public void ShowAddFriendDialogueBox()
    {
        addFriendPanel.SetActive(true);
        foo = true;
    }
}
