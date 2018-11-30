using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.IO;
using System.Text;
using ServerManager;

public class FriendChallenges : MonoBehaviour {

    // Use this for initialization
    int checkupdates = 1;
    string userId;
    public NotificationList notifications = new NotificationList();
  

    public GameObject Challenge_Item;
    public GameObject Friend_Request;
    public GameObject Response;
    public ScrollRect scrollView;
    public GameObject ScrollViewContent;
    
    void Start() {
        //get userID from json file
        Debug.Log("Started");

        string data = File.ReadAllText(Application.dataPath + "/MyInfo.json");
        UserInfo playerinfo = JsonUtility.FromJson<UserInfo>(data);
        userId = playerinfo.getUserId();
    
    }
	
	// Update is called once per frame
	void Update () {
        if (checkupdates == 1)
        {
            StartCoroutine("CheckMyUpdates");
        }
	}
    public IEnumerator CheckMyUpdates()
    {
        //Update method stopped for every five seconds.
        checkupdates = 0;
        yield return new WaitForSeconds(6f);
        checkupdates = getChallenges();
    }
    //Receives all challenging usernames from the backend.
    public int getChallenges()
    {
        //get data from the bacck end
        int returnvalue = 0;
        ConnectionManager CM = new ConnectionManager();
        if (CM.StartClient() == 1)
        {
            //the server spits back comma separated string of friends
            string response = CM.CheckChallengesFriendRquestsMessages(userId);

            //split the response by comma
            notifications = JsonUtility.FromJson<NotificationList>(response);
            
            //update UI method
            returnvalue = addNewChallengesUI();

            scrollView.verticalNormalizedPosition = 1;

            return returnvalue;
        }
        else
        {
            Debug.Log("Connection Manager start client failed.");
            return 1;
        }
        
    }
    //Updates UI with new sets of challenges received from backend.
    public int addNewChallengesUI()
    {
        
        foreach(Notification notification in notifications.Notification) {
                if(notification.messagetype.Equals("Game Challenge"))
            {
                ChallengeMaker(notification.username);
            } else if (notification.messagetype.Equals("Friend Request"))
            {
                FriendRequestMaker(notification.username);
            } else
            {
                ResponseMaker(notification.username, notification.messagetype);
            }
                
            }
        return 1;
    }

    public void ChallengeMaker(string username)
    {
        GameObject ChallengeObject = Instantiate(Challenge_Item);
        ChallengeObject.transform.SetParent(ScrollViewContent.transform, false);
        ChallengeObject.transform.Find("Friend_Username").gameObject.GetComponent<Text>().text = username;
    }
    public void FriendRequestMaker(string username)
    {
        GameObject FriendObject = Instantiate(Friend_Request);
        FriendObject.transform.SetParent(ScrollViewContent.transform, false);
        FriendObject.transform.Find("Friend_Username").gameObject.GetComponent<Text>().text = username;
    }
    public void ResponseMaker(string username, string message)
    {
        GameObject ResponseObject = Instantiate(Response);
        ResponseObject.transform.SetParent(ScrollViewContent.transform, false);
        ResponseObject.transform.Find("Friend_Username").gameObject.GetComponent<Text>().text = username;
        ResponseObject.transform.Find("ResponseMessage").gameObject.GetComponent<Text>().text = message;

    }
}
