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
        Destroy(notice_Item);
        string[] param = new string[3];
        param[0] = userId;
        param[1] = friendUsername.GetComponent<Text>().text;
        param[2] = "Challenge Rejected";
        ConnectionManager CM = new ConnectionManager();

        string response = CM.ChallengeDenied(param);

        Debug.Log(response);
        

    }

    public void RejectedFriendRequest()
    {
        Destroy(notice_Item);
        string[] param = new string[3];
        param[0] = userId;
        param[1] = friendUsername.GetComponent<Text>().text;
        param[2] = "Friend Request Rejected";
        ConnectionManager CM = new ConnectionManager();

        string response = CM.FriendRequestRejected(param);

        Debug.Log(response);


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
