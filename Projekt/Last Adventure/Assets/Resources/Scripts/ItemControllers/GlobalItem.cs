using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GlobalItem : MonoBehaviour {

    private ControllerDialogManager CDM;

    public string ItemDestroyID;
    public string file;

    private Config config;

    private string path;
    private string itemString;
    public itemClass item;

    public int HorizontalScan = 4;
    public int VerticalScan = 8;
    private GameObject audioNext;

    private List<string> DialogList = new List<string>();

    private void Start()
    {
        config = GameObject.Find("Config").GetComponent<Config>();
        path = config.getLangPath() + file + ".json";

        if(File.Exists(path))
        {
            itemString = File.ReadAllText(path);
            item = JsonUtility.FromJson<itemClass>(itemString);
        }
        else Debug.Log("Nie znaleziono pliku.");

        if (config.checkDestroyItemOnMap(ItemDestroyID)) Destroy(gameObject);

        DialogList.Add("Person|"+item.ObjectName+":"+item.ItemName+"|item|");
        DialogList.Add("|||>> " + item.Action + ": "+ item.ItemName +" <<");
        DialogList.Add("Add|"+ item.ObjectName +":"+ item.Type +"|Jump|");
        DialogList.Add("Destroy|||");

        CDM = new ControllerDialogManager(
           gameObject,
           DialogList,
           HorizontalScan,
           VerticalScan
           );
        audioNext = (GameObject)Resources.Load("Audio/Effects/Prefab/Prefab_AudioNext", typeof(GameObject));
    }


    private void Update()
    {
        switch (CDM.DialogController())
        {
            case "Destroy":
                config.AddItemToDestroyOnMap(ItemDestroyID);
                Destroy(gameObject);
                break;
            case "Play":
                Destroy(Instantiate(audioNext, transform, false), 0.3f);
                break;
        }
    }
}

