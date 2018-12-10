using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PatternStatic;
using UnityShortCuts;
using Navigator;

public class PlayAgainstAI_Pattern_Option : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void pattern_go() {
		ShortCuts usc = new ShortCuts();
		string pattern = usc.InputValue("patternString");
		Debug.Log(pattern);
		PatternStatic.SelectedPattern = pattern;
		SceneNavigator navi = new SceneNavigator();
		navi.GoToScene("GameScreen_AI_Pattern");
	}
}
