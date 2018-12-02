using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.IO;
using System.Text;
using ServerManager;
using UnityEngine.EventSystems;

public class PopulateStore : MonoBehaviour {

    // Use this for initialization
    public GameObject SkinObject;
    public ScrollRect scrollView;
    public GameObject ScrollViewContent;
    public GameObject skinDisplay;
    public LinkedList<Skin> skinList;
    public SkinList skinlist = new SkinList();
	void Start () {
        /*ConnectionManager CM = new ConnectionManager();
        if(CM.StartClient() == 1)
        {

       //test string
       //string response = "{\r\n\t\"Skin\": [{\r\n\t\t\t\"id\": 1,\r\n\t\t\t\"name\": \"Krishna\",\r\n\t\t\t\"price\": \"100\"\r\n\t\t},\r\n\t\t{\r\n\t\t\t\"id\": 2,\r\n\t\t\t\"name\": \"Sam\",\r\n\t\t\t\"price\": \"200\"\r\n\t\t}\r\n\t]\r\n}";
           string response = CM.getSkinList();
           skinlist = JsonUtility.FromJson<SkinList>(response);

           populateLocalSkins();
       } else
       {
           print("Connection manager failed on client.");
       } */
        skinDisplay = GameObject.FindGameObjectWithTag("currentSkinDisplay");
        skinDisplay.GetComponent<Text>().text = Skin.getCurrentSkinFromJson();

        populateLocalSkins();
    }

    public void populateLocalSkins()
    {
        skinList = Skin.getAllSkins();
        Debug.Log(skinList.Count);

        ScrollViewContent = GameObject.FindGameObjectWithTag("leaderboardContent");

        foreach (Skin skin in skinList)
        {
            Debug.Log("new Skin");
            GameObject newSkin = (GameObject)Instantiate(Resources.Load("Prefabs/Shop_Item"));

            GameObject newSkinName = newSkin.transform.Find("Skin_Name").gameObject;
            GameObject newSkinCost = newSkin.transform.Find("Skin_Cost").gameObject;
            GameObject newSkinRock = newSkin.transform.Find("Skin_Image_Rock").gameObject;
            GameObject newSkinPaper = newSkin.transform.Find("Skin_Image_Paper").gameObject;
            GameObject newSkinScissors = newSkin.transform.Find("Skin_Image_Scissors").gameObject;

            newSkinName.GetComponent<Text>().text = skin.getSkinTag();
            newSkinCost.GetComponent<Text>().text = skin.price;
            Skin.setImageSkin(newSkinRock.GetComponent<Image>(), newSkinScissors.GetComponent<Image>(), newSkinPaper.GetComponent<Image>(), skin);

            //add onclick listener to set the skin
            newSkin.AddComponent(typeof(EventTrigger));
            EventTrigger trigger = newSkin.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) => 
            {
                Skin.writeSkinToJson(skin);
                skinDisplay.GetComponent<Text>().text = skin.getSkinTag();
            });
            trigger.triggers.Add(entry);

            newSkin.transform.parent = ScrollViewContent.transform;
            newSkin.transform.localScale = new Vector3(1, 1, 1);
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
