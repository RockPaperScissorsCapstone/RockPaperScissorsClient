using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Navigator{
    public class MonoSceneNavigator : MonoBehaviour {
		public void GoToScene(string sceneName)
    	{
        	UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    	}
	}
}