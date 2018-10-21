using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Text;
using ConnectionManager;
public class GetAccountInfo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ConnectionsManager CM = new ConnectionsManager();
		int result = CM.StartClient();
		Debug.Log(result);
		string[] playerInfo = CM.getAccoutInformation();
		for(int i = 0; i<3; i++){
			Debug.Log("playerInfo[" + i + "]: " + playerInfo[i]);
		}

		showUserInfo(playerInfo);

	}

	private void showUserInfo(string[] playerInfo){
		GameObject userNameField = GameObject.FindGameObjectWithTag("profileName");
		InputField userNameIput = userNameField.GetComponent<InputField>();
		userNameIput.text = playerInfo[2];
	}

	// Update is called once per frame
	void Update () {
		
	}
}
