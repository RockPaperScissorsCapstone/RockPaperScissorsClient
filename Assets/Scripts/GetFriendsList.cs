using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.IO;
using System.Text;
using ServerManager;

public class GetFriendsList : MonoBehaviour {
	public ScrollRect scrollView;
	public GameObject Friend_Item;
	public GameObject ScrollViewContent;

	// Use this for initialization
	void Start () {
		string data = File.ReadAllText(Application.dataPath + "/MyInfo.json");
		UserInfo playerinfo = JsonUtility.FromJson<UserInfo>(data);
		string user_id = playerinfo.getUserId();
		getFriends(user_id);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//get the friends list data from the server
	public void getFriends(string userId) {
		ConnectionManager CM = new ConnectionManager();
		if (CM.StartClient() == 1) {
			//the server spits back comma separated string of friends
			string response = CM.getFriendsList(userId);
			//split the response by comma
			string[] friendsList = response.Split(','); 
			
			foreach(var friend in friendsList) {
				Debug.Log(friend);
			}

			//update UI method
			fillFriends(friendsList);

			scrollView.verticalNormalizedPosition = 1;
		} else {
			Debug.Log("Connection Manager start client failed.");
		}
	}

	//update the UI with the friends list
	public void fillFriends(string[] friendsList) {
		foreach(var friend in friendsList) {
			Debug.Log(friend);

			//instantiate a Friend_Item prefab, set the parent to scrollview's content, and change the text to friend var from friendsList
			GameObject friendObject = Instantiate(Friend_Item);
			friendObject.transform.SetParent(ScrollViewContent.transform, false);
			friendObject.transform.Find("Friend_Name").gameObject.GetComponent<Text>().text = friend;
		}
	}
}
