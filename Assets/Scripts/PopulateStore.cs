using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;
using ServerManager;
using UnityEngine.EventSystems;

public class PopulateStore : MonoBehaviour {

    public enum SkinElements { Name, Cost, Rock, Paper, Scissors};

    // Use this for initialization
    public GameObject SkinObject;
    public ScrollRect scrollView;
    public GameObject ScrollViewContent;
    public GameObject skinDisplay;
    public LinkedList<Skin> skinList;
    public SkinList skinlist = new SkinList();

    string playerCurrency = "";
    string playerId = "";
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

        string data = File.ReadAllText(Application.persistentDataPath + "/MyInfo.json");
		UserInfo playerinfo = JsonUtility.FromJson<UserInfo>(data);
        GameObject currencyObject = GameObject.Find("Money_Display");
        // playerCurrency = playerinfo.getCurrency();
        playerCurrency = "500";
        playerId = playerinfo.getUserId();
        currencyObject.GetComponent<Text>().text = playerCurrency;
        skinDisplay = GameObject.FindGameObjectWithTag("currentSkinDisplay");
        skinDisplay.GetComponent<Text>().text = Skin.getCurrentSkinFromJson();

        populateServerSkins();
    }

    private GameObject[] getElements(GameObject newSkinPrefab)
    {
        GameObject[] elements = new GameObject[5];

        elements[(int)SkinElements.Name] = newSkinPrefab.transform.Find("Skin_Name").gameObject;
        elements[(int)SkinElements.Cost] = newSkinPrefab.transform.Find("Skin_Cost").gameObject;
        elements[(int)SkinElements.Rock] = newSkinPrefab.transform.Find("Skin_Image_Rock").gameObject;
        elements[(int)SkinElements.Paper] = newSkinPrefab.transform.Find("Skin_Image_Paper").gameObject;
        elements[(int)SkinElements.Scissors] = newSkinPrefab.transform.Find("Skin_Image_Scissors").gameObject;
        return elements;
    }
    private void setObjectText(GameObject nameObject, string nameString)
    {
        nameObject.GetComponent<Text>().text = nameString;
    }
    private GameObject getSkinPrefab()
    {
        return (GameObject)Instantiate(Resources.Load("Prefabs/Shop_Item"));
    }
    private void setOnClickListener(GameObject skinPrefab, Skin skin)
    {
        //add onclick listener to set the skin
        skinPrefab.AddComponent(typeof(EventTrigger));
        EventTrigger trigger = skinPrefab.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) =>
        {
            Debug.Log(eventData);
            GameObject selectedSkin = ((PointerEventData) eventData).pointerPress; //get the selected gameobject
            string selectedSkinCost = selectedSkin.transform.Find("Skin_Cost").GetComponent<Text>().text;//get the cost
            string selectedSkinName = selectedSkin.transform.Find("Skin_Name").GetComponent<Text>().text;
            string selectedSkinId = selectedSkin.transform.tag;

            Debug.Log(selectedSkinCost); 
            if (selectedSkinCost.Equals("Bought")) { //already bought, then just set the skin
                Skin.writeSkinToJson(skin);
                skinDisplay.GetComponent<Text>().text = skin.getSkinTag();
            } else { //skin not bought 
                int playercurrency = Convert.ToInt32(playerCurrency);
                int selectedskincost = Convert.ToInt32(selectedSkinCost);
                if (playercurrency < selectedskincost) { //can't buy skin
                    GameObject.Find("Help_Text").GetComponent<Text>().text = "You do not have enough money to buy this skin!";
                } else { //can buy skin
                    ConnectionManager CM = new ConnectionManager();
                    if (CM.StartClient() == 1) {
                        string[] param = new string[2];
                        param[0] = playerId;
                        param[1] = selectedSkinId;
                        string response = CM.buySkin(param);
                        Debug.Log(response);
                        selectedSkin.transform.Find("Skin_Cost").GetComponent<Text>().text = "Bought";
                        GameObject.Find("Help_Text").GetComponent<Text>().text = "Bought Skin!";
                        
                        //update currency
                        string data = File.ReadAllText(Application.persistentDataPath + "/MyInfo.json");
		                UserInfo playerinfo = JsonUtility.FromJson<UserInfo>(data);
                        playercurrency -= selectedskincost;
                        playerinfo.setCurrency(playercurrency.ToString());

                        GameObject currencyObject = GameObject.Find("Money_Display");
                        currencyObject.GetComponent<Text>().text = playercurrency.ToString();
                    }
                }
            }
            
        });
        trigger.triggers.Add(entry);
    }

    public void populateLocalSkins()
    {
        skinList = Skin.getAllSkins();
        Debug.Log(skinList.Count);

        ScrollViewContent = GameObject.FindGameObjectWithTag("leaderboardContent");

        foreach (Skin skin in skinList)
        {
            Debug.Log("new Skin");
            GameObject newSkin = getSkinPrefab();

            GameObject[] elements = getElements(newSkin);

            setObjectText(elements[(int)SkinElements.Name], skin.getSkinTag());
            setObjectText(elements[(int)SkinElements.Cost], skin.price);
            Skin.setImageSkin(elements, skin);

            setOnClickListener(newSkin, skin);

            newSkin.transform.parent = ScrollViewContent.transform;
            newSkin.transform.localScale = new Vector3(1, 1, 1);
        }

    }

    public void populateServerSkins()
    {
        ScrollViewContent = GameObject.FindGameObjectWithTag("leaderboardContent");

        ConnectionManager CM = new ConnectionManager();
        if (CM.StartClient() == 1)
        {
            string data = File.ReadAllText(Application.dataPath + "/MyInfo.json");
            Debug.Log(data);
            UserInfo playerinfo = JsonUtility.FromJson<UserInfo>(data);

            string[] skinsData = CM.getSkinsList().TrimEnd(';').Split(';');
            string[] skinsPurchased = CM.getSkinsPurchased(playerinfo.getUserId()).Split(';');
            // int count = 0;

            foreach (string skin in skinsData)
            {
                string[] skinDetails = skin.Split(',');
                string name = skinDetails[0];
                string tag = skinDetails[1];
                string cost = skinDetails[2];
                Skin skinObject = new Skin(tag);

                // if (tag.Equals(skinsPurchased[count]))
                // {
                //     cost = "0";
                //     count++;
                // }

                foreach (var purchased in skinsPurchased) {
                    if (tag.Equals(purchased)) {
                        cost = "Bought";
                    }
                }

                GameObject newSkin = getSkinPrefab();
                GameObject[] elements = getElements(newSkin);
                setObjectText(elements[(int)SkinElements.Name], name);
                setObjectText(elements[(int)SkinElements.Cost], cost);
                Skin.setImageSkin(elements, skinObject);

                setOnClickListener(newSkin, skinObject);

                newSkin.transform.SetParent(ScrollViewContent.transform, false);
                newSkin.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else
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
