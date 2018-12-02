using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This is the Object class for a skin
//ALL VARIABLES
public string SkinName;   //Name of Skin
public string SkinID;     //ID of skin
public int SkinPrice;     //price of skin via currency
public bool Owned;        // If SKin is owned or not
public Image RockSkin;    //The image of the ROCK skin
public Image PaperSKin;   //The image of the PAPER skin
public Image ScissorsSkin;//The image of the SCISSORS skin

public class SkinObjectClass : MonoBehaviour {


    //ALL initalizing calls FUNCTIONS
    public SetSkinName(string name)                         //Sets The skin object's Name
    {
        SkinName = name;
    }

    public SetSkinID(string name)
    {
        SkinID = name;                                      //Sets SKINID to the name
    }
    public SetSkinPrice(int priceofskin)                    //Sets price
    {
        SkinPrice = priceofskin;
    }

    public SetRockSkin(gameObject RockObj)                  //Sets Rock Skin from a Object's Image
    {
        RockSkin = RockObj.getComponent<image>();

    }

    public SetPaperSkin(gameObject PaperObj)                //Sets Paper Skin from a Object's image
    {
        PaperSkin = PaperObj.getComponent<image>();
    }

    public SetScissorsSkin(gameObject ScissorsObj)          //Sets Scissors skin from a Object's image
    {
        ScissorsSkin = ScissorsObj.getComponent<image>();
    }

   



    //ALL RETURN/GET FUNCTIONS

    public getSkinName()
    {
        return SkinName;
    }

    public getSkinID()
    {
        return SkinID;
    }

    public getSkinPrice()
    {
        return SkinPrice;
    }

    public getRockSkin()                                    //Returns Rock Skin
    {
        return RockSkin;
    }

    public setPaperSkin()                                   //Returns Paper Skin
    {
        return PaperSkin;

    } 

    public setScissorsSkin()                                //Returns Scissors Skin
    {
        return PaperSkin;
    }

   

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
