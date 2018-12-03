using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerManager;
using Navigator;
using System;
using System.IO;
using UnityEngine.UI;

public class PlayWithAI : MonoBehaviour {
    public Button Rock_Button, Paper_Button, Scissors_Button;
    public Text Match_Number_Text, Help_Text, Human_Number_Text, AI_Number_Text;
    // public GameObject skinList;
    string userId = "";
    string wins = "";
    string losses = "";
    string skintag = "";
    int matchNumber = 1;
    int sessionResponse = 2;
    int localAiWin = 0;
    int localHumanWin = 0;
    private static int skinInScreenPosition = 276;
    private static int skinOutScreenPosition = 630;
    private bool skinsOnScreen = false;

    ConnectionManager connectionManager;
    UserInfo userInfo;
    Skin skin;

	// Use this for initialization
	void Start () {
        //initially, store the necessary info (user_id) into local variable to be ready to pass to playWithAI()
        try {
            using (StreamReader streamReader = new StreamReader(Application.persistentDataPath + "/MyInfo.json")){
                String line = streamReader.ReadToEnd();
                streamReader.Close();
                userInfo = JsonUtility.FromJson<UserInfo>(line);
                userId = userInfo.getUserId();
                wins = userInfo.getWins();
                losses = userInfo.getLosses();
                skintag = userInfo.getSkintag();
                skin = new Skin(skintag);

                Debug.Log("add listener");
                // Rock_Button = Rock_Button.GetComponent<Button>();
                // Paper_Button = Paper_Button.GetComponent<Button>();
                // Scissors_Button = Scissors_Button.GetComponent<Button>();

                // skinList = GameObject.FindGameObjectWithTag("skinList");

                Skin.setButtonSkin(Rock_Button, Paper_Button, Scissors_Button, skin);
                
                Rock_Button.onClick.AddListener(delegate {TaskWithParameters("1");});
                Paper_Button.onClick.AddListener(delegate {TaskWithParameters("2");});
                Scissors_Button.onClick.AddListener(delegate {TaskWithParameters("3");});
                Debug.Log("done adding listener");

                connectionManager = new ConnectionManager();
                if (connectionManager.StartClient() == 1) //successful start of client
                {
                    string sessionStartResponse = connectionManager.startGameSession();
                    Debug.Log(sessionStartResponse);
                } else //failed start of client
                {
                    Debug.Log("Failed to start ConnectionManager Client");
                }
            }
        } catch(Exception e) {
            Debug.Log(e.Message, gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void TaskWithParameters(string move) {
        Debug.Log(move);
        connectionManager.sendUserId(userId);
        Debug.Log("Sent User Id");
        connectionManager.sendMove(move);
        Debug.Log("Sent Move");

        
        string stringResponse = connectionManager.getOneResponse();
        Debug.Log(stringResponse);

        sessionResponse = Convert.ToInt32(stringResponse);

        // Debug.Log(playerWinResponse);
        // Debug.Log(AIWinResponse);
        Debug.Log(sessionResponse);

        if (sessionResponse == 2) {
            matchNumber++;
            Match_Number_Text.text = matchNumber.ToString();
            string playerWinResponse = connectionManager.getOneResponse();
            string AIWinResponse = connectionManager.getOneResponse();

            localHumanWin = Convert.ToInt32(playerWinResponse);
            localAiWin = Convert.ToInt32(AIWinResponse);

            Human_Number_Text.text = playerWinResponse;
            AI_Number_Text.text = AIWinResponse;
        } else {
            Debug.Log(sessionResponse);
            EndGame();
        }
    }

    public void EndGame() {
        if (sessionResponse == 1) {
            Help_Text.text = "You won!";
            int newWin = int.Parse(wins);
            newWin++;
            userInfo.setWins(newWin.ToString());
            wins = newWin.ToString();
            localHumanWin++;
            Human_Number_Text.text = (localHumanWin).ToString();
        } else if (sessionResponse == 0){
            Help_Text.text = "AI won!";
            int newLosses = int.Parse(losses);
            newLosses++;
            userInfo.setLosses(newLosses.ToString());
            losses = newLosses.ToString();
            localAiWin++;
            AI_Number_Text.text = (localAiWin).ToString();
        }
        string json = JsonUtility.ToJson(userInfo);
        File.WriteAllText(Application.persistentDataPath + "/MyInfo.json", json);

        //update to the DB
        connectionManager = new ConnectionManager();
        connectionManager.StartClient();
        string[] param = new string[4];
        param[0] = wins;
        param[1] = losses;
        param[2] = userId;
        string updateWinLossResponse = connectionManager.updateWinLoss(param);
        Debug.Log(updateWinLossResponse);

        //disable move buttons
    }
}
