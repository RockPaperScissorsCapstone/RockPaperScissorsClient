using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerManager;
using System;
using System.IO;

public class NotificationSystem : MonoBehaviour {
	ConnectionManager connectionManager;
	// Use this for initialization
	void Start () {
		connectionManager = new ConnectionManager();
		if (connectionManager.StartClient() == 1) {
			StartCoroutine("notificationSystem"); //as soon as script is loaded, run the notification system
		} else {
			Debug.Log("How do you expect me to check for notification when I'm not connected to the server???!?!?!");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//this is the notification system that gets run from Start()
	IEnumerator notificationSystem() {
		while (true) { //never stop the notification system. it stops when the scene which this script is attached to is destroyed
			//put logics here

			yield return new WaitForSecondsRealtime(2); //this is like sleep for real seconds. then while loop goes again
		}
	}
}
