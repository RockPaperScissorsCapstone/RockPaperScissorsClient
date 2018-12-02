using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.IO;
using System.Text;
using ServerManager;

public class PopulateStore : MonoBehaviour {

    // Use this for initialization
    public GameObject SkinObject;
    public ScrollRect scrollView;
    public GameObject ScrollViewContent;
    public SkinList skinlist = new SkinList();
	void Start () {
         ConnectionManager CM = new ConnectionManager();
         if(CM.StartClient() == 1)
         {

            //test string
            string response = "{\r\n\t\"Skin\": [{\r\n\t\t\t\"id\": 1,\r\n\t\t\t\"name\": \"Krishna\",\r\n\t\t\t\"price\": \"100\"\r\n\t\t},\r\n\t\t{\r\n\t\t\t\"id\": 2,\r\n\t\t\t\"name\": \"Sam\",\r\n\t\t\t\"price\": \"200\"\r\n\t\t}\r\n\t]\r\n}";
            // string response = CM.getSkinList();
            skinlist = JsonUtility.FromJson<SkinList>(response);

            populateskins();
        } else
        {
            print("Connection manager failed on client.");
        }
    }
    public void populateskins()
    {
        foreach(Skin skin in skinlist.Skin)
        {
            GameObject newskin = Instantiate(SkinObject);
            newskin.transform.SetParent(ScrollViewContent.transform, false);
            newskin.transform.Find("Skin_Name").gameObject.GetComponent<Text>().text = skin.name;
            newskin.transform.Find("Skin_Cost").gameObject.GetComponent<Text>().text = skin.price;
            newskin.transform.Find("Skin_Image_Rock").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Graphics/Hand-Rock");
            newskin.transform.Find("Skin_Image_Paper").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Graphics/Hand-Paper");
            newskin.transform.Find("Skin_Image_Scissors").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Graphics/Hand-Scissors");
            newskin.tag = "skin";
        }
    }
}
