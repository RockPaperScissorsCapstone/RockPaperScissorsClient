using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Skin {

    public static string path = "Graphics/";
    public static string rock = "Hand-Rock";
    public static string paper = "Hand-Paper";
    public static string scissors = "Hand-Scissors";

    public string id, name, price;


    private string skinTag;

    public Skin(string tag)
    {
        this.skinTag = tag;
        this.price = "0";
    }

    public string getSkinTag()
    {
        return this.skinTag;
    }

    private string buildPath(string move)
    {
        string tmpPath = path + move;
        if (getSkinTag().Length > 0)
            tmpPath += '-' + getSkinTag();

        return tmpPath;
    }

    //returns the path to the skin image resource from the Resources folder
    public string getRockPath() { return buildPath(rock); }
    public string getPaperPath() { return buildPath(paper); }
    public string getScissorsPath() { return buildPath(scissors); }

    public static void setButtonSkin(Button rock, Button paper, Button scissors, Skin skin)
    {
        setImageSprite(rock, skin.getRockPath());
        setImageSprite(paper, skin.getPaperPath());
        setImageSprite(scissors, skin.getScissorsPath());

    }
    public static void setImageSkin(Image rock, Image paper, Image scissors, Skin skin)
    {
        setImageSprite(rock, skin.getRockPath());
        setImageSprite(paper, skin.getPaperPath());
        setImageSprite(scissors, skin.getScissorsPath());
    }
    private static void setImageSprite(Button button, string url)
    {
        button.GetComponent<Image>().sprite = Resources.Load<Sprite>(url);
    }
    private static void setImageSprite(Image image, string url)
    {
        image.sprite = Resources.Load<Sprite>(url);
    }

    public static void writeSkinToJson(Skin skin)
    {
        //read userinfo from json and set the new skin tag
        string data = File.ReadAllText(Application.dataPath + "/MyInfo.json");
        UserInfo userInfo = JsonUtility.FromJson<UserInfo>(data);
        userInfo.setSkinTag(skin.getSkinTag());

        //save the modified userinfo
        data = JsonUtility.ToJson(userInfo);
        StreamWriter sw = File.CreateText(Application.dataPath + "MyInfo.json");
        sw.Close();
        File.WriteAllText(Application.dataPath + "/MyInfo.json", data);

    }
    public static string getCurrentSkinFromJson()
    {
        string data = File.ReadAllText(Application.dataPath + "/MyInfo.json");
        UserInfo userInfo = JsonUtility.FromJson<UserInfo>(data);
        return userInfo.getSkintag();
    }

    public static LinkedList<Skin> getAllSkins()
    {
        LinkedList<Skin> skinList = new LinkedList<Skin>();

        Sprite[] images = Resources.LoadAll<Sprite>(path);

        foreach(Sprite image in images)
        {
            if (image.name.Contains(rock))
            {
                int remove = 10;
                if (image.name.Length == 9) {
                    remove--;
                }

                string skinName = image.name.Remove(0, remove);

                skinList.AddLast(new Skin(skinName));
            }
            
        }
        return skinList;
    }

}
