using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogMaker : MonoBehaviour {

    private ControllerDialogManager CDM;

    public int HorizontalScan = 5;
    public int VerticalScan = 10;
    private GameObject audioNext;
    private string[] dialog;
    public string file;
    private string txt;
    private string path;

    //private string txt;
    public List<string> DialogList = new List<string>();

    // Use this for initialization
    void Start()
    {
        path = GameObject.Find("Config").GetComponent<Config>().getLangPath() + file + ".txt";

        StreamReader reader = new StreamReader(path);
        txt = reader.ReadToEnd();
        reader.Close();

        dialog = txt.Split(
            new[] { "\r\n", "\r", "\n" },
            StringSplitOptions.None
        );

        foreach (var dialogLine in dialog) DialogList.Add(dialogLine);

        CDM = new ControllerDialogManager(
            gameObject,
            DialogList,
            HorizontalScan,
            VerticalScan);
        audioNext = (GameObject)Resources.Load("Audio/Effects/Prefab/Prefab_AudioNext", typeof(GameObject));
    }


    private void Update()
    {
        switch (CDM.DialogController())
        {
            case "Destroy":
                Destroy(gameObject);
                break;
            case "Play":
                Destroy(Instantiate(audioNext, transform, false), 0.3f);
                break;
        }
    }
}