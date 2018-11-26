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
    public GameObject Challenge_Item;
    public GameObject ChallengerUsername;
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
        Destroy(Challenge_Item);
        string[] param = new string[3];
        param[0] = userId;
        param[1] = ChallengerUsername.GetComponent<Text>().text;
        param[2] = "Challenge Denied";
        ConnectionManager CM = new ConnectionManager();

        string response = CM.ChallengeDenied(param);

        Debug.Log(response);
        

    }

    public void AcceptChallenge()
    {
        Destroy(Challenge_Item);
        string[] param = new string[3];
        param[0] = userId;
        param[1] = ChallengerUsername.GetComponent<Text>().text;
        param[2] = "Challenge Accepted";
        ConnectionManager CM = new ConnectionManager();

        string response = CM.ChallengeAccepted(param);

        Debug.Log(response);

    }
}
