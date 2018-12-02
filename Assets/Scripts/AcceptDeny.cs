using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.IO;
using System.Text;
using ServerManager;


public class AcceptDeny : MonoBehaviour {

    // Use this for initialization
    public GameObject notice_Item;
    public GameObject friendUsername;
    public GameObject messageLabel;
    public Button Accept;
    public Button Deny;

    string userId;

    void Start () {
        string data = File.ReadAllText(Application.dataPath + "/MyInfo.json");
        UserInfo playerinfo = JsonUtility.FromJson<UserInfo>(data);
        userId = playerinfo.getUserId();
    }

    public void RejectChallenge()
    {
        //Rejects both challenges and friend requests and adds a message for sender.
        Destroy(notice_Item);
        string[] param = new string[3];
        param[0] = userId;
        Debug.Log(param[0]);
        param[1] = friendUsername.GetComponent<Text>().text;
        Debug.Log(param[1]);
        param[2] = messageLabel.GetComponent<Text>().text;
        Debug.Log(param[2]);

        // Rejecting a challenge deleting from messages
        ConnectionManager CM = new ConnectionManager();
        if (CM.StartClient() == 1)
        {
            string response = CM.ChallengeDenied(param);

            Debug.Log(response);

        } else {
            Debug.Log("You messed up again");
        }
        param[2] = "Challenge Rejected";
        Debug.Log(param[2]);
        if (CM.StartClient() == 1)
        {
            string[] responses = CM.ChallengeFriend(param);
        }
    }

    public void RejectedFriendRequest()
    {
        Destroy(notice_Item);
        string[] param = new string[3];
        param[0] = userId;
        Debug.Log(param[0]);
        param[1] = friendUsername.GetComponent<Text>().text;
        Debug.Log(param[1]);
        param[2] = messageLabel.GetComponent<Text>().text;
        Debug.Log(param[2]);

        // Rejecting a challenge deleting from messages
        ConnectionManager CM = new ConnectionManager();
        if (CM.StartClient() == 1)
        {
            string response = CM.ChallengeDenied(param);

            Debug.Log(response);


        }
        else
        {
            Debug.Log("You messed up again");
        }
        param[2] = "Friend Request Rejected";
        Debug.Log(param[2]);
        if (CM.StartClient() == 1)
        {
            string[] responses = CM.ChallengeFriend(param);
        }
    }

    public void AcceptChallenge()
    {
        Destroy(notice_Item);
        string[] param = new string[3];
        param[0] = userId;
        param[1] = friendUsername.GetComponent<Text>().text;
        param[2] = "Challenge Accepted";
        ConnectionManager CM = new ConnectionManager();

        string response = CM.ChallengeAccepted(param);

        Debug.Log(response);

    }

    public void AcceptFriendRequest()
    {
        Destroy(notice_Item);
        string[] param = new string[3];
        param[0] = userId;
        param[1] = friendUsername.GetComponent<Text>().text;
        param[2] = "Friend Request Accepted";
        ConnectionManager CM = new ConnectionManager();

        string response = CM.FriendRequestAccepted(param);

        Debug.Log(response);

    }

    public void DeleteResponse()
    {
        Destroy(notice_Item);
    }
}
