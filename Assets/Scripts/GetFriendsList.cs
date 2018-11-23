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
    public GameObject FriendUsername;
    public Button ChallengeButton;
    string userId;

    // Use this for initialization
    void Start () {
		string data = File.ReadAllText(Application.dataPath + "/MyInfo.json");
		UserInfo playerinfo = JsonUtility.FromJson<UserInfo>(data);
		string username = playerinfo.getUsername();
        userId = playerinfo.getUserId();
		getFriends(username);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//get the friends list data from the server
	public void getFriends(string username) {
		ConnectionManager CM = new ConnectionManager();
		if (CM.StartClient() == 1) {
			//the server spits back comma separated string of friends
			string response = CM.getFriendsList(username);
			
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

		//testing
		// string response = "Steve,Jason,Illya,Nick";
		// string[] friendsList = response.Split(',');
		// fillFriends(friendsList);
	}

	//update the UI with the friends list
	public void fillFriends(string[] friendsList) {
		foreach(var friend in friendsList) {
			Debug.Log(friend.Length);
			if (friend.Length > 0 & friend.Length < 46){
				Debug.Log(friend);

				//instantiate a Friend_Item prefab, set the parent to scrollview's content, and change the text to friend var from friendsList
				GameObject friendObject = Instantiate(Friend_Item);
				friendObject.transform.SetParent(ScrollViewContent.transform, false);
				friendObject.transform.Find("Friend_Name").gameObject.GetComponent<Text>().text = friend;
			}
		}
	}

    public void ChallengeFriend()
    {
        string[] param = new string[3];
        string friendUsername = FriendUsername.GetComponent<Text>().text;
        param[0] = userId;
        param[1] = friendUsername;
        param[3] = "Challenge Message";


        Debug.Log(friendUsername);
        ConnectionManager CM = new ConnectionManager();

        if (CM.StartClient() == 1)
        {
            string[] response = CM.ChallengeFriend(param);
            Debug.Log(response[4]);
        }
        
    }
}
