using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using static SkinStatic;

public class SelectSkin : MonoBehaviour, IPointerClickHandler {
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerPress);
		SkinStatic.SelectedSkin = eventData.pointerPress;
		SceneManager.LoadScene("BuySkin");
    }
}
