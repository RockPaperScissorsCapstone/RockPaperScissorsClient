using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen_Random : MonoBehaviour {
	public Text Loading_Text;

	// Use this for initialization
	void Start () {
		StartCoroutine(LoadYourAsyncScene());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator LoadYourAsyncScene() {
		yield return null;
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScreen_Random");
		asyncLoad.allowSceneActivation = false;
		while (!asyncLoad.isDone) {
			Loading_Text.text = "Loading progress: " + (asyncLoad.progress * 100) + "%";
			if (asyncLoad.progress >= 0.9f)
            {
                //Change the Text to show the Scene is ready
                Loading_Text.text = "Press the space bar to continue";
                //Wait to you press the space key to activate the Scene
                if (Input.GetKeyDown(KeyCode.Space))
                    //Activate the Scene
                    asyncLoad.allowSceneActivation = true;
            }
			yield return null;
		}
	}
}
