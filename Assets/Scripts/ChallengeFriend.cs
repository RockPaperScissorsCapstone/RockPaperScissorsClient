using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.IO;
using System.Text;
using ServerManager;

public class ChallengeFriend: MonoBehaviour {

    public GameObject Challenge_Item;
    public GameObject Friend_name;
    string userId;
	// Use this for initialization
	void Start () {
        string data = File.ReadAllText(Application.dataPath + "/MyInfo.json");
        UserInfo playerinfo = JsonUtility.FromJson<UserInfo>(data);
        string username = playerinfo.getUsername();
        userId = playerinfo.getUserId();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void test()
    {
        Debug.Log(userId);
        string friendusername = Friend_name.GetComponent<Text>().text;
        Debug.Log(friendusername);
    }
}
