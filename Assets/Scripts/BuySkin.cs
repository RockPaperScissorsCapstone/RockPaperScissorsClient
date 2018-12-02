using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using static SkinStatic;

public class BuySkin : MonoBehaviour {
	public string currency;
	public GameObject selectedSkin;

	// Use this for initialization
	void Start () {
		string data = File.ReadAllText(Application.persistentDataPath + "/MyInfo.json");
		UserInfo playerinfo = JsonUtility.FromJson<UserInfo>(data);
		currency = playerinfo.getCurrency();

		selectedSkin = SkinStatic.SelectedSkin;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
