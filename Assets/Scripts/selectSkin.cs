using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class selectSkin : MonoBehaviour {

    public void setSkin(string skintag)
    {
        string data = File.ReadAllText(Application.dataPath + "/MyInfo.json");
        UserInfo playerinfo = JsonUtility.FromJson<UserInfo>(data);
        playerinfo.setSkinTag(skintag);
        data = JsonUtility.ToJson(playerinfo);
        StreamWriter sw = File.CreateText(Application.dataPath + "/MyInfo.json");
        sw.Close();
        File.WriteAllText(Application.dataPath + "/MyInfo.json", data);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
