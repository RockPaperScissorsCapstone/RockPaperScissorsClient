using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.IO;
using System.Text;
using ServerManager;

public class newNotifications : MonoBehaviour {
    public GameObject newNotices;
    public string userId;
    int breaktime = 1;
    // Use this for initialization
    void Start () {
        string data = File.ReadAllText(Application.persistentDataPath + "/MyInfo.json");
        UserInfo playerinfo = JsonUtility.FromJson<UserInfo>(data);
        userId = playerinfo.getUserId();

    }
    void Update()
    {

        if (breaktime == 1)
        {
            StartCoroutine("CheckMyNotices");
        }
    }
    public IEnumerator CheckMyNotices()
    {
        //Update method stopped for every five seconds.
        breaktime = 0;
        yield return new WaitForSeconds(2f);

        breaktime = Getnotices();
    }

    public int Getnotices()
    {
        ConnectionManager CM = new ConnectionManager();
        if(CM.StartClient() == 1)
        {
            string response = CM.CheckChallengesFriendRquestsMessages(userId);
            string[] mynotices = response.Split(',');
            int numberOfNotices = (int) (mynotices.Length / 2);
            newNotices.GetComponent<Text>().text = numberOfNotices.ToString();

        }
        else
        {
            print("Connection Manager Failed!");
        }
        return 1;
    }

}
