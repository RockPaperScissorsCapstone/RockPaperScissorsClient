using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace UnityShortCuts{
	public class ShortCuts : MonoBehaviour {

		// Use this for initialization
		void Start () {
		
		}
	
		// Update is called once per frame
		void Update () {
		
		}

		public string InputValue(string tagName){
			GameObject inputField = GameObject.FindGameObjectWithTag(tagName);
			InputField iput = inputField.GetComponent<InputField>();
			return iput.text.ToString();
		}
	}
}
