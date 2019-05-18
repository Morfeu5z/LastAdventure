using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemInfo : MonoBehaviour {

    public string file;

    private Config config;
    private string txt;
    private string path;

    private string itemString;
    public itemClass item;

    private void Start()
    {
        config  = GameObject.Find("Config").GetComponent<Config>();
        path    = config.getLangPath() + file + ".json";

        if (File.Exists(path))
        {
            itemString = File.ReadAllText(path);
            item = JsonUtility.FromJson<itemClass>(itemString);
        }
        else Debug.Log("Nie znaleziono pliku.");
    }

    public void ItemUse(int eqIndex)
    {
        switch (item.Type)
        {
            case "Food":
                for(int i = 0; i < item.LifeResp; i++) if(config.LoadedData.life < config.MaxLife) config.AddLife();
                config.DropItem(eqIndex);
                Destroy(gameObject);
                break;
        }
    }
}

#region // Class for json item
[System.Serializable]
public class itemClass
{
    public string ObjectName;
    public string ItemName;
    public string Type;
    public string Description;
    public int LifeResp;
    public string Action;

    public itemClass(string obj = "", string name = "", string typ = "", string des = "", int life = 0, string act = "")
    {
        ObjectName = obj;
        ItemName = name;
        Type = typ;
        Description = des;
        LifeResp = life;
        Action = act;
    }
}
#endregion