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

    public GameObject Challenge_Item;
    public ScrollRect scrollView;
    public GameObject ScrollViewContent;
    
    void Start() {
        //get userID from json file
        Debug.Log("Started");

        string data = File.ReadAllText(Application.dataPath + "/MyInfo.json");
        UserInfo playerinfo = JsonUtility.FromJson<UserInfo>(data);
        userId = playerinfo.getUserId();
       // string response = "Ram,Sam,Ham,Gam,Tam,Pam";
       //challengeusernames = response.Split(',');
       // addNewChallengesUI();
        
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
            string response = CM.CheckChallengesUpdate(userId);

            //split the response by comma
            challengeusernames = response.Split(',');

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
        
        foreach(var challenge in challengeusernames) {
                Debug.Log(challenge.Length);
                if (challenge.Length > 0 & challenge.Length < 45)
                {
                    Debug.Log(challenge);
                
                GameObject ChallengeObject = Instantiate(Challenge_Item);
                ChallengeObject.transform.SetParent(ScrollViewContent.transform, false);
                ChallengeObject.transform.Find("Friend_Username").gameObject.GetComponent<Text>().text = challenge;
                }
            
            }
        return 1;
    }
}
