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

	//this method takes a string sceneName and tells Unity to Load the matching SceneName
	//For this method to work, both main menu scene and profile page scene must be loaded
	//in the build settings.
    public void loadPlayerProfileScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
