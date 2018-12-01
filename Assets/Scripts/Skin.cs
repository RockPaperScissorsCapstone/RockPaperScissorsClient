﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Skin {

    public static string path = "Res/Graphics/";
    public static string rock = "Hand-Rock";
    public static string paper = "Hand-Paper";
    public static string scissors = "Hand-Scissors";


    private string skinTag;

    public Skin(string tag)
    {
        this.skinTag = tag;
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
        else tmpPath += ".png"; 

        return tmpPath;
    }

    //returns the path to the skin image resource from the Assets folder
    public string getRockPath(){ return buildPath(rock); }
    public string getPaperPath(){ return buildPath(paper); }
    public string getScissorsPath() { return buildPath(scissors); }


    public LinkedList<Skin> getAllSkins()
    {
        LinkedList<Skin> skinList = new LinkedList<Skin>();

        char[] trimArray = {'H', 'a', 'n', 'd', '-', 'R', 'o', 'c', 'k'};

        foreach (string file in Directory.EnumerateFiles(Application.persistentDataPath + path, "Hand-Roc*"))
        {
            string skinName = file.Remove(0, 8);
            skinName = skinName.Substring(skinName.Length - 3);
            if (skinName.Length == 0) { skinList.AddLast(new Skin("")); }
            else
            {
                skinList.AddLast(new Skin(skinName));
            }
        }
        return skinList;
    }
    
}