using System.Collections;
using System.Collections.Generic;
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
    public int i = 0;

    public GameObject Challenge_Item;
    public GameObject Friend_Request;
    public GameObject Response;
    public ScrollRect scrollView;
    public GameObject ScrollViewContent;
    
    
    void Start() {
        //get userID from json file
        Debug.Log("Started");

        string data = File.ReadAllText(Application.persistentDataPath + "/MyInfo.json");
        UserInfo playerinfo = JsonUtility.FromJson<UserInfo>(data);
        userId = playerinfo.getUserId();
    
    }
	
	// Update is called once per frame
	void Update () {
        // Debug.Log("Hello");
        if (checkupdates == 1)
        {
            i++;
            StartCoroutine("CheckMyUpdates");
        }
	}
    public IEnumerator CheckMyUpdates()
    {
        //Update method stopped for every five seconds.
        checkupdates = 0;
        yield return new WaitForSeconds(5f);
        checkupdates = getChallenges();
    }
    //Receives all challenging usernames from the backend.
    public int getChallenges()
    {
        print(i);
       // get data from the bacck end


      /*  string var = "{\r\n\t\"Notification\": [{\r\n\t\t\t\"username\": \"Krishna\",\r\n\t\t\t\"messagetype\":" +
            " \"Friend Request\"\r\n\t\t},\r\n\t\t{\r\n\t\t\t\"username\": " +
            "\"Sam\",\r\n\t\t\t\"messagetype\": \"Challenge Message\"" +
            "\r\n\t\t},\r\n\t\t{\r\n\t\t\t\"username\": \"Hari\",\r\n\t\t\t\"messagetype\"" +
            ": \"He declined your message\"\r\n\t\t}\r\n\t]\r\n}";
        notifications = JsonUtility.FromJson<NotificationList>(var);
        returnvalue = addNewChallengesUI();
    */
        ConnectionManager CM = new ConnectionManager();
         if (CM.StartClient() == 1)
         {
            // the server spits back comma separated string of friends
            string response = CM.CheckChallengesFriendRquestsMessages(userId);
            //Destroy(ScrollViewContent.);
            for (int i = 0; i < ScrollViewContent.transform.childCount; i++) {
                Destroy(ScrollViewContent.transform.GetChild(i).gameObject);
            }
            Debug.Log(response);

            string[] usernameMessageList = response.Split(',');
            for (int i = 0; i < usernameMessageList.Length-1; i+=2){
                string friendusername = usernameMessageList[i];
                string message = usernameMessageList[i+1];

                addNewChallengesUI(friendusername, message);
            }

            // split the response by comma
           // notifications = JsonUtility.FromJson<NotificationList>(response);

            // update UI method
            // returnvalue = addNewChallengesUI();

             scrollView.verticalNormalizedPosition = 1;

            return 1;
        }
        else
         {
            Debug.Log("Connection Manager start client failed.");
             return 1;
         }
    }
    //Updates UI with new sets of challenges received from backend.
    public void addNewChallengesUI(string username, string message)
    {

        
       // foreach(Notification notification in notifications.Notification) {
               // if(notification.messagetype.Equals("Challenge Message"))
        if(message.Equals("Challenge Message"))
            {
            ChallengeMaker(username, message);

              //  ChallengeMaker(notification.username);
        } else if  (message.Equals("Friend Request"))
            //(notification.messagetype.Equals("Friend Request"))
            {
            FriendRequestMaker(username, message);
           // FriendRequestMaker(notification.username);
            } else
            {
                ResponseMaker(username, message);
               // ResponseMaker(notification.username, notification.messagetype);
        }
                
          //  }
       // return 1;
    }

    public void ChallengeMaker(string username, string message)
    {
        GameObject ChallengeObject = Instantiate(Challenge_Item);
        ChallengeObject.transform.SetParent(ScrollViewContent.transform, false);
        ChallengeObject.transform.Find("Friend_Username").gameObject.GetComponent<Text>().text = username;
        ChallengeObject.transform.Find("ChallengeLabel").gameObject.GetComponent<Text>().text = message;
    }
    public void FriendRequestMaker(string username, string message)
    {
        GameObject FriendObject = Instantiate(Friend_Request);
        FriendObject.transform.SetParent(ScrollViewContent.transform, false);
        FriendObject.transform.Find("Friend_Username").gameObject.GetComponent<Text>().text = username;
        FriendObject.transform.Find("FriendRequestLabel").gameObject.GetComponent<Text>().text = message;
    }
    public void ResponseMaker(string username, string message)
    {
        GameObject ResponseObject = Instantiate(Response);
        ResponseObject.transform.SetParent(ScrollViewContent.transform, false);
        ResponseObject.transform.Find("Friend_Username").gameObject.GetComponent<Text>().text = username;
        ResponseObject.transform.Find("ResponseMessage").gameObject.GetComponent<Text>().text = message;

    }
}
