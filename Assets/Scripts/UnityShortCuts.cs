using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace UnityShortCuts{
	public class ShortCuts {
		public ShortCuts(){

		}

		/* 
			Helper function that uses the tag name associated with a game object of type InputField
			to find said game object and return the value of the input fields text in a string format. 
		*/
		public string InputValue(string tagName){
			GameObject inputObject = GameObject.FindGameObjectWithTag(tagName);
			InputField inputObjectComponent = inputObject.GetComponent<InputField>();
			return inputObjectComponent.text.ToString();
		}

		public void updateInputValue(string tagName, string param){
			GameObject inputObject = GameObject.FindGameObjectWithTag(tagName);
			InputField inputObjectComponent = inputObject.GetComponent<InputField>();
			inputObjectComponent.text = param;
		}

		public void updateInputValue(string[] tagName, string[] param){
			for(int i = 0; i < tagName.Length; i++){
				GameObject inputObject = GameObject.FindGameObjectWithTag(tagName[i]);
				InputField inputObjectComponent = inputObject.GetComponent<InputField>();
				inputObjectComponent.text = param[i];
			}
		}

		public void updateTextValue(string tagName, string param){
			GameObject textObject = GameObject.FindGameObjectWithTag(tagName);
			Text textObjectComponent = textObject.GetComponent<Text>();
			textObjectComponent.text = param;
		}

		public void updateTextValue(string[] tagName, string[] param){
			for(int i = 0; i < tagName.Length; i++){
				GameObject textObject = GameObject.FindGameObjectWithTag(tagName[i]);
				Text textObjectComponent = textObject.GetComponent<Text>();
				textObjectComponent.text = param[i];
			}
		}

	}
}
