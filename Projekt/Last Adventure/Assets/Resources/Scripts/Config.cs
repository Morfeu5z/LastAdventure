using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class Config : MonoBehaviour {
    
    private static Config instance;

    public DataSave LoadedData = null;

    public configJSON config = new configJSON();
    private string languaPath;
    private string configPath;

    private GameObject  MainMenu;
    private GameObject  SettingsMenu;
    private GameObject  ControlMenu;

    private menuJSON    menu;
    private string      jsonString;

    public bool portal      = false;
    public int  portalIndex = 0;
    [HideInInspector]
    public int  portalBuild = 0;

    public int  MaxLife = 3;

    //Equip
    public List<string> global_ItemList = new List<string>();
    public List<string> global_ItemActiveList = new List<string>();
    public List<string> global_ItemsToDestroy = new List<string>();
    public List<string> global_ActivetedEventList = new List<string>();

    // Message Box
    GameObject Info;
    GameObject Message;
    GameObject Person;
    GameObject Pic;
    GameObject leafPrefab;

    [HideInInspector]
    public GameObject LifeTree;
    public bool Esc = false;
    public bool Fin = false;
    public int  FinScene = 5;

    public bool saveSettings = false;

    // Use this for initialization
    void Start () {
        Application.targetFrameRate = 60;
        Cursor.visible = false;
        MainMenu = GameObject.Find("Canvas/Menu/main");
        SettingsMenu = GameObject.Find("Canvas/Menu/settings");
        ControlMenu = GameObject.Find("Canvas/Menu/control");
        leafPrefab = (GameObject)Resources.Load("prefabs/Leaf", typeof(GameObject));
        LoadedData = new DataSave(1,0,0,1);
    }

    public void LinkRefresh()
    {
        int sceneNow = SceneManager.GetActiveScene().buildIndex;
        if (sceneNow != 0 && sceneNow != FinScene)
        {
            LifeTree = GameObject.Find("Canvas/Life");
            LifeTree.GetComponent<Animator>().SetTrigger("LifeTree");
            Info = GameObject.Find("Canvas/Info");
            Message = GameObject.Find("Canvas/Info/Message/Text");
            Person = GameObject.Find("Canvas/Info/Person/Text");
            Pic = GameObject.Find("Canvas/Info/Person/Pic");
            RespawnLifes();
        }
    }
    

    // --------------------------------------------------------------- AWAKE ------------------

    void Awake()// Do not destroy this object
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            ReadConfig();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --------------------------------------------------------------- READ CONFIG ------------------


    public void refile()
    {
        //string path = Application.streamingAssetsPath + "/language/pl";
        //string[] files;
        //string[] tmp;
        //    files = Directory.GetFiles(path);
        //    foreach (var item in files)
        //    {
        //        string[] filepath = item.Split('\\');
        //        foreach (var it in filepath)
        //        {
        //            if (it.Substring(0, 4) == "Item")
        //            {
        //                tmp = it.Split('.');
        //                if (tmp[tmp.Length - 1] == "txt")
        //                {
        //                    if (File.Exists(item))
        //                    {
        //                    StreamReader reader = new StreamReader(path + "/" + it);
        //                    string txt = reader.ReadToEnd();
        //                    reader.Close();
        //                    string[] s = txt.Split('|');
        //                    itemClass itfil = new itemClass(s[0], s[1], s[2], s[3], s[4], "Weź");
        //                    string makejson = JsonUtility.ToJson(itfil);
        //                    string newpath = path + "/" + tmp[0] + ".json";
        //                    File.WriteAllText(newpath, makejson);
        //                    }
        //                }
        //            }
        //        }
        //    }
    }

    #region // Read config from file
    public void ReadConfig()
    {
        configPath = Application.streamingAssetsPath + "/config.json";
        if (!File.Exists(configPath))
        {
            config.Language = 0;
            config.LangList = new List<string>(new string[] { "pl", "en" });
            config.SoundtrackVolume = 0.5f;
            config.EffectVolume = 0.5f;
            config.fps = 0;
            saveConfig();
        }
        else
        {
            jsonString = File.ReadAllText(configPath);
            config = JsonUtility.FromJson<configJSON>(jsonString);
        }

        languaPath = Application.streamingAssetsPath + "/language/" + config.LangList[config.Language] + "/" + config.LangList[config.Language] + "-menu.json";
    }
    #endregion


    public string getLangPath() // Path to dialog
    {
        return Application.streamingAssetsPath + "/language/" + config.LangList[config.Language] + "/";
    }


    public void nextLang() // Change lenguage
    {
        if (config.Language + 1 > config.LangList.Count - 1)
        {
            config.Language = 0;
        }
        else
        {
            config.Language++;
        }

        languaPath = Application.streamingAssetsPath + "/language/" + config.LangList[config.Language] + "/" + config.LangList[config.Language] + "-menu.json";
        MainMenuConfig();
    }

    public void MainMenuConfig() // Configuration display text in main menu and settings menu
    {
        jsonString = File.ReadAllText(languaPath);
        menu = JsonUtility.FromJson<menuJSON>(jsonString);

        if (MainMenu == null) MainMenu = GameObject.Find("Canvas/Menu/main");
        if (SettingsMenu == null) SettingsMenu = GameObject.Find("Canvas/Menu/settings");
        if (ControlMenu == null) ControlMenu = GameObject.Find("Canvas/Menu/control");

        for (int i = 0; i < MainMenu.transform.childCount; i++)
        {
            switch (MainMenu.transform.GetChild(i).gameObject.transform.name)
            {
                case "continue":
                    MainMenu.transform.GetChild(i).GetComponent<Text>().text = menu.Conti;
                    break;

                case "newgame":
                    MainMenu.transform.GetChild(i).GetComponent<Text>().text = menu.NewGame;
                    break;

                case "settings":
                    MainMenu.transform.GetChild(i).GetComponent<Text>().text = menu.Sett;
                    break;

                case "control":
                    MainMenu.transform.GetChild(i).GetComponent<Text>().text = menu.Control;
                    break;

                case "exit":
                    MainMenu.transform.GetChild(i).GetComponent<Text>().text = menu.Exit;
                    break;
            }
        }

        for (int i = 0; i < SettingsMenu.transform.childCount; i++)
        {
            switch (SettingsMenu.transform.GetChild(i).gameObject.transform.name)
            {

                case "language":
                    SettingsMenu.transform.GetChild(i).GetComponent<Text>().text = menu.Lang;
                    break;

                case "sound":
                    SettingsMenu.GetComponent<MainMenu>().soundName = menu.Sound;
                    break;

                case "effect":
                    SettingsMenu.GetComponent<MainMenu>().effectName = menu.Effect;
                    break;

                case "back":
                    SettingsMenu.transform.GetChild(i).GetComponent<Text>().text = menu.Back;
                    break;
            }
        }
        ControlMenu.transform.GetChild(0).GetComponent<Text>().text = menu.Back;
    }

    public void saveConfig() // Save new configuration
    {
        string json = JsonUtility.ToJson(config);
        File.WriteAllText(configPath, json);
    }


  

    // --------------------------------------------------------------- ITEMS & EQUIP ------------------

    #region // Add item to destroy list
    public void AddItemToDestroyOnMap(string ItemID)
    {
        global_ItemsToDestroy.Add(ItemID);
    }
    #endregion

    #region // Check If Item Is Destroy On Map List
    public bool checkDestroyItemOnMap(string ItemID)
    {
        for (int i = 0; i < global_ItemsToDestroy.Count; i++)
        {
            if (global_ItemsToDestroy[i] == ItemID) return true;
        }
        return false;
    }
    #endregion

    #region // Check If Action was Activated
    public bool CheckExistActivetedEvent(string e_event) {
        for(int i = 0; i < global_ActivetedEventList.Count; i++)
        {
            if (global_ActivetedEventList[i] == e_event) return true;
        }
        return false;
    }
    #endregion

    #region // Add Activated Action
    public void SetActivetedEvent(string action)
    {
        if(CheckExistActivetedEvent(action) == false)global_ActivetedEventList.Add(action);
    }
    #endregion

    #region // Update Player Equip
    public void UpdateEquip()
    {
        if (GameObject.Find("Hero").GetComponent<ControllerPlayer>().equipActive)
        {
            GameObject equip = GameObject.Find("Hero/Equip").gameObject;
            ResetEquip();
            for (int i = 0; i < global_ItemActiveList.Count; i++)
            {
                try
                {
                    string itemName = global_ItemActiveList[i] + "_Item";
                    GameObject addItem = (GameObject)Resources.Load("prefabs/Items/" + itemName, typeof(GameObject));
                    Instantiate(addItem, equip.transform.GetChild(17 + i).gameObject.transform, false);
                }
                catch { Debug.Log("Cant load active item: " + global_ItemActiveList[i]); }
            }

            for (int i = 0; i < global_ItemList.Count; i++)
            {
                try
                {
                    string itemName = global_ItemList[i] + "_Item";
                    GameObject addItem = (GameObject)Resources.Load("prefabs/Items/" + itemName, typeof(GameObject));
                    Instantiate(addItem, equip.transform.GetChild(i + 1).gameObject.transform, false);
                } catch { Debug.Log("Cant load item"); }
            }
        }
    }
    #endregion

    #region // ResetEquip
    public void ResetEquip()
    {
        GameObject equip = GameObject.Find("Hero/Equip").gameObject;
        for (int i = 1; i < equip.transform.childCount; i++)
        {
            GameObject equ = equip.transform.GetChild(i).gameObject;
            if (equ.transform.childCount > 0) Destroy(equ.transform.GetChild(0).gameObject);
        }
    }
    #endregion

    public void AddItem(string name) // Add Item
    {
        global_ItemList.Add(name);
    }

    public void UseItem(GameObject item, int eqIndex)    // From ControllerPlayer
    {
        bool search = true;
        string name = item.GetComponent<ItemInfo>().item.ObjectName;
        if (item.GetComponent<ItemInfo>().item.Type == "Weapon" || item.GetComponent<ItemInfo>().item.Type == "Active")
        {
            if (eqIndex >= 17)
            {
                for (int i = 0; i < global_ItemActiveList.Count; i++)
                {
                    if (global_ItemActiveList[i] == name && search)
                    {
                        ItemStatus(false, global_ItemActiveList[i], eqIndex);
                        search = false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < global_ItemList.Count; i++)
                {
                    if (global_ItemList[i] == name && search)
                    {
                        ItemStatus(true, global_ItemList[i], eqIndex);
                        search = false;
                    }
                }
            }
        }
        else if (item.GetComponent<ItemInfo>().item.Type == "Food")
        {
            if (LoadedData.life < MaxLife)
            {
                item.GetComponent<ItemInfo>().ItemUse(eqIndex);
            }
        }
    }

    public void DropItemByName(string name) 
    {
        for(int i = 0; i < global_ItemList.Count; i++)
        {
            if(global_ItemList[i] == name)
            {
                global_ItemList.RemoveAt(i);
                i = global_ItemList.Count;
            }
        }
        UpdateEquip();
    }

    public void DropItem(int eqIndex)
    {
        global_ItemList.RemoveAt(eqIndex - 1);
        UpdateEquip();
    }

    public void DropActiveItem(string itemName)
    {
        int i;
        for (i = 0; i < 4; i++)
        {
            if (global_ItemActiveList[i] == itemName)
            {
                global_ItemActiveList.RemoveAt(i);
                i = 4;
            }
        }
        if(GameObject.Find("Hero").GetComponent<ControllerPlayer>().equipActive) UpdateEquip();
    }

    public void ItemStatus(bool active, string name, int eqIndex)
    {
        GameObject equip = GameObject.Find("Hero/Equip").gameObject;
        if (active)
        {
            if (global_ItemActiveList.Count < 4) {
                global_ItemActiveList.Add(name);
                global_ItemList.RemoveAt(eqIndex - 1);
                equip.GetComponent<ControllerEquip>().Active(active, name);
            }
        }
        else
        {
            global_ItemList.Add(name);
            global_ItemActiveList.RemoveAt((17 - eqIndex) * -1);
            equip.GetComponent<ControllerEquip>().Active(active, name);
        }

        UpdateEquip();
    }

    // Active elements
    public void ActiveEquip()
    {
        GameObject equip = GameObject.Find("Hero/Equip").gameObject;
        equip.GetComponent<ControllerEquip>().Active(true, "Hand");

        for (int i = 0; i < global_ItemActiveList.Count; i++)
        {
            equip.GetComponent<ControllerEquip>().Active(true, global_ItemActiveList[i]);
        }
    }

    public bool CheckExistItemInEquip(string item)
    {
        for (int i = 0; i < global_ItemActiveList.Count; i++)
        {
            if (global_ItemActiveList[i] == item) return true;
        }
        for (int i = 0; i < global_ItemList.Count; i++)
        {
            if (global_ItemList[i] == item) return true;
        }
        return false;
    }

    public bool CheckActiveItem(string item)
    {
        for (int i = 0; i < global_ItemActiveList.Count; i++)
        {
            if (global_ItemActiveList[i] == item) return true;
        }
        return false;
    }

    // --------------------------------------------------------------- AUDIO ------------------

    public void volume(string name, string direct) // Change effects and sound volume
    {
        if (name == "sound" && direct == "up")
        {
            if (config.SoundtrackVolume < 0.99) config.SoundtrackVolume += 0.01f;
        }
        else if (name == "sound" && direct == "down")
        {
            if (config.SoundtrackVolume > 0.01) config.SoundtrackVolume -= 0.01f;
        }
        else if (name == "effect" && direct == "up") {
            if(config.EffectVolume < 0.99) config.EffectVolume += 0.01f;
        }
        else if (name == "effect" && direct == "down"){
            if(config.EffectVolume > 0.01) config.EffectVolume -= 0.01f;
        }
        GameObject.Find("SoundTrack").transform.GetComponent<ControllerAudio>().Volume = config.SoundtrackVolume;
    }


    // --------------------------------------------------------------- SAVE SYSTEM ------------------

    public void SaveAndRespown()
    {
        SaveGame(3);
        SceneReload();
    }

    public void QLoad()
    {
        LoadGame();
        SceneReload();
    }

    public void QSave()
    {
        SaveGame(LoadedData.life);
    }

    #region Save progress
    public void SaveGame(int life = 1)
    {
        GameObject.Find("Canvas/SaveInfo").GetComponent<Animator>().SetTrigger("Save");
        string ActiveItems = "";
        string EquipItems = "";
        string toDestroy = "";
        string wasActive = "";

        if (global_ItemList.Count > 0)
        {
            for (int i = 0; i < global_ItemList.Count; i++)
            {
                if (EquipItems != "") EquipItems += "|";
                EquipItems += global_ItemList[i];
            }
        }
        if (global_ItemActiveList.Count > 0)
        {
            for (int i = 0; i < global_ItemActiveList.Count; i++)
            {
                if (ActiveItems != "") ActiveItems += "|";
                ActiveItems += global_ItemActiveList[i];
            }
        }
        if (global_ItemsToDestroy.Count > 0)
        {
            for (int i = 0; i < global_ItemsToDestroy.Count; i++)
            {
                if (toDestroy != "") toDestroy += "|";
                toDestroy += global_ItemsToDestroy[i];
            }
        }
        if (global_ActivetedEventList.Count > 0) {
            for (int i = 0; i < global_ActivetedEventList.Count; i++)
            {
                if (wasActive != "") wasActive += "|";
                wasActive += global_ActivetedEventList[i];
            }
        }

        DataSave data = new DataSave(
                    SceneManager.GetActiveScene().buildIndex,
                    GameObject.Find("Hero").transform.position.x, 
                    GameObject.Find("Hero").transform.position.y,
                    life,
                    EquipItems,
                    ActiveItems,
                    toDestroy,
                    wasActive
                    );

        LoadedData = data;
        SaveSystem.SaveGame(data);
    }
    #endregion

    #region Loading data
    public void LoadGame()
    {
        LoadedData = SaveSystem.LoadGame();

        global_ItemActiveList   = new List<string>();
        global_ItemList         = new List<string>();
        global_ItemsToDestroy   = new List<string>();
        global_ActivetedEventList        = new List<string>();

        if (LoadedData.equipItems != "")
        {
            string[] splitEquipItems = LoadedData.equipItems.Split('|');
            for (int i = 0; i < splitEquipItems.Length; i++)
            {
                if(splitEquipItems[i] != null) global_ItemList.Add(splitEquipItems[i]);
            }
        }
        if (LoadedData.activeItems != "")
        {
            string[] splitActiveItems = LoadedData.activeItems.Split('|');
            for (int i = 0; i < splitActiveItems.Length; i++)
            {
                if(splitActiveItems[i] != null) global_ItemActiveList.Add(splitActiveItems[i]);
            }
        }
        if (LoadedData.itemsToDestroy != "")
        {
            string[] splitToDestroy = LoadedData.itemsToDestroy.Split('|');
            for (int i = 0; i < splitToDestroy.Length; i++)
            {
                if(splitToDestroy[i] != null) global_ItemsToDestroy.Add(splitToDestroy[i]);
            }
        }
        if (LoadedData.wasActive != "")
        {
            string[] splitWasActive = LoadedData.wasActive.Split('|');
            for (int i = 0; i < splitWasActive.Length; i++)
            {
                if(splitWasActive[i] != null) global_ActivetedEventList.Add(splitWasActive[i]);
            }
        }
    }
    #endregion

    #region New Game
    public void NewGame()
    {
        global_ItemActiveList = new List<string>();
        global_ItemList = new List<string>();
        global_ItemsToDestroy = new List<string>();
        global_ActivetedEventList = new List<string>();
        LoadedData = SaveSystem.NewGame();
    }
    #endregion



// --------------------------------------------------------------- LIFE MANAGER ------------------

    public void RespawnLifes() // Respown leaf
    {
        int target = LoadedData.life;
        LoadedData.life = 0;
        for (int i = 0; i < target; i++) AddLife();
    }

    public void AddLife() // Add life
    {
        Vector3 newVec3 = new Vector3(0.47f + (LoadedData.life * 0.34f), -0.21f, LifeTree.transform.position.z);
        GameObject tmp = Instantiate(leafPrefab, LifeTree.transform, false);
        tmp.transform.localPosition = newVec3;
        LoadedData.life++;
    }

    public void DropLife(string mode = "") // Drop Life
    {
        if (LoadedData.life > 0)
        {
            LifeTree.transform.GetChild(LoadedData.life - 1).gameObject.GetComponent<Animator>().SetTrigger("Destroy");
            LoadedData.life--;
            if (LoadedData.life <= 0)
            {
                if (CheckActiveItem("LifeCrystal") && mode != "force")
                {
                    DropActiveItem("LifeCrystal");
                    LoadedData.life = MaxLife;
                    RespawnLifes();
                }else{
                    GameObject.Find("Hero").GetComponent<ControllerPlayer>().Death();
                }
            }
        }
    }

    public void OnePunchDeath() // One Punch Death
    {
        for (int i = 0; i < LoadedData.life; i++) DropLife("force");
    }

    public void MessageBox(bool onoff, string txt = "", string pers = "", string sprPath = "") // Info Box
    {
        if (onoff)
        {
            Info.SetActive(true);
            string[] sprPathCut = sprPath.Split('/');
            if (txt != Message.GetComponent<Text>().text || sprPathCut[sprPathCut.Length - 1] != Pic.GetComponent<Image>().sprite.name)
            {
                // redukcja znaków
                string[] stringSeparators = new string[] { "color" };
                string[] result = txt.Split(stringSeparators, StringSplitOptions.None);
                int txtLeng = txt.Length - ((result.Length - 1) * 12); // <color=#ffffff></color> 23 znaki

                // ilość liter * miejsce na jedną literę + szerokość startowa
                float sizeX = txtLeng * 15 + 150;
                float sizeY = 100;
                if (sizeX > 1330)
                {
                    sizeX = 1330;
                    sizeY = 200;
                }
                Message.transform.parent.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeX, sizeY);
                Message.GetComponent<Text>().text = txt;
                Person.GetComponent<Text>().text = pers;
                Sprite spr = (Sprite)Resources.Load("Sprites/DialogPic/unknow", typeof(Sprite));
                Pic.GetComponent<Image>().sprite = spr;
                if (sprPath != "")
                {
                    spr = (Sprite)Resources.Load(sprPath, typeof(Sprite));
                    if (spr) Pic.GetComponent<Image>().sprite = spr;
                    else
                    {
                        spr = (Sprite)Resources.Load("Sprites/DialogPic/unknow", typeof(Sprite));
                        Pic.GetComponent<Image>().sprite = spr;
                    }
                }
                else
                {
                    spr = (Sprite)Resources.Load("Sprites/DialogPic/unknow", typeof(Sprite));
                    Pic.GetComponent<Image>().sprite = spr;
                }
            }
        }
        else
        {
                try
                {
                    Sprite spr = (Sprite)Resources.Load("Sprites/DialogPic/unknow", typeof(Sprite));
                    Pic.GetComponent<Image>().sprite = spr;
                }
                catch { }

            Message.GetComponent<Text>().text = "";
            Person.GetComponent<Text>().text = "";
            Info.SetActive(false);
        }
    }

    public void SceneReload() // Scene reload
    {
        GameObject.Find("Canvas/LoadScreen").GetComponent<Animator>().SetBool("LoadOn", true);
        GameObject.Find("Hero").GetComponent<ControllerPlayer>().keyboardOn = false;
    }
}


// --------------------------------------------------------------- JSON CLASS ------------------


#region // Class for json config 
[System.Serializable]
public class configJSON
{
    public int Language;
    public List<string> LangList;
    public float SoundtrackVolume;
    public float EffectVolume;
    public int fps;
}
#endregion

#region // Class for menu language configuration
[System.Serializable]
public class menuJSON
{
    public string NewGame;
    public string Conti;
    public string Sett;
    public string Lang;
    public string Exit;
    public string Volume;
    public string Sound;
    public string Effect;
    public string Control;
    public string Back;
}
#endregion

#region // refile
//[System.Serializable]
//public class itemClass
//{
//    public string ObjectName;
//    public string ItemName;
//    public string Type;
//    public string Description;
//    public string LifeResp;
//    public string Action;

//    public itemClass(string obj = "", string name = "", string typ = "", string des = "", string life = "", string act = "")
//    {
//        ObjectName = obj;
//        ItemName = name;
//        Type = typ;
//        Description = des;
//        LifeResp = life;
//        Action = act;
//    }
//}
#endregion