using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConnectionManager;

public class PlayWithAI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void playWithAI()
    {
        ConnectionsManager connectionsManager = new ConnectionsManager();
        if (connectionsManager.StartClient() == 1) //successful start of client
        {

        } else //failed start of client
        {
            Debug.Log("Failed to start ConnectionsManager Client");
        }
    }
}
