using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewUIScript : MonoBehaviour {

    public gameObject RockPreview;      //Rock Skin Preview Object
    public gameObject PaperPreview;     //Paper Skin Preview Object
    public gameObject ScissorsPreview;  //Scissors Skin Preview Object


    // Use this for initialization
    void Start () {
        //Gameobject Inizalization
        RockPreview.gameObject.find("RockPreview");             //Finds RockPreview Object and initalize
        PaperPreview.gameObject.find("PaperPreview");           //Finds PaperPreview Object and initalize
        ScissorsPreview.gameObject.find("ScissorsPreview");     //Finds ScissorsPreview and initalize

    }

    public void setRockPreviewSkin(gameObject Skin) //Sets the Rock sprite to gameobject's  
    {
        RockPreview.sprite = Skin.getComponent<image>();
    }
    public void setPaperPreviewSkin(gameObject Skin)//Sets The Paper sprite to gameobjects
    {

        PaperPreview.sprite = Skin.getComponent<image>();
    }
    public void setScissorsPreviewSkin(gameObject Skin)//Sets the Scissors Sprite to the gameobject's
    {

        ScissorsPreview.sprite = Skin.getComponent<image>();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
