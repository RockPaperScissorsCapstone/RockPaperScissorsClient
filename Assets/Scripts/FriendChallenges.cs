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
    string[] challengeusernames;

    public ScrollRect scrollView;
    public GameObject Challenge_Item;
    public GameObject ScrollViewContent;

    void Start () {
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
        Debug.Log(userId);
        checkupdates = getChallenges();
    }

    public int getChallenges()
    {
        //get data from the bacck end

        ConnectionManager CM = new ConnectionManager();
        if (CM.StartClient() == 1)
        {
            //the server spits back comma separated string of friends
            string response = CM.CheckChallengesUpdate(userId);

            //split the response by comma
            challengeusernames = response.Split(',');

            foreach (var challenge in challengeusernames)
            {
                Debug.Log(challenge);
            }

            //update UI method
            addNewChallengesUI();

            scrollView.verticalNormalizedPosition = 1;
        }
        else
        {
            Debug.Log("Connection Manager start client failed.");
        }
        
        return 1;
    }
    public void addNewChallengesUI()
    {
        foreach(var challenge in challengeusernames) {
                Debug.Log(challenge.Length);
                if (challenge.Length > 0 & challenge.Length < 45)
                {
                    Debug.Log(challenge);

                    //instantiate a Friend_Item prefab, set the parent to scrollview's content, and change the text to friend var from friendsList
                    GameObject ChallengeObject = Instantiate(Challenge_Item);
                ChallengeObject.transform.SetParent(ScrollViewContent.transform, false);
                ChallengeObject.transform.Find("Friend_Username").gameObject.GetComponent<Text>().text = challenge;
                }
            }
    }

    public void ChallengeFriend()
    {
        string[] param = new string[3];
        string friendUsername = "";
        param[0] = userId;
        param[1] = friendUsername;
        param[3] = "Challenge Message";

        ConnectionManager CM = new ConnectionManager();

        if (CM.StartClient() == 1)
        {
            string[] response = CM.ChallengeFriend(param);
            Debug.Log(response[4]);
        }

    }
}
