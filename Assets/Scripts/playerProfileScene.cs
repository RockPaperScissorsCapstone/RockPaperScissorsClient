using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerProfileScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void loadPlayerProfileScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("PlayerProfile");
    }
}
