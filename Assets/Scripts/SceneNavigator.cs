using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Navigator{
	public class SceneNavigator {

		public SceneNavigator(){

		}
		public void GoToScene(string sceneName)
    	{
        	UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    	}
	}
}