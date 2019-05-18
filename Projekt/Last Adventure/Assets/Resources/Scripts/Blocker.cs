using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : MonoBehaviour {

    public string ObjectID = "CaveBlocker";

    private ControllerDialogManager CDM;

    public int HorizontalScan = 5;
    public int VerticalScan = 10;
    private GameObject audioNext;
    private List<string> DialogList = new List<string>();

    // Use this for initialization
    void Start()
    {
        if (ObjectID == "CaveBlocker")
        {
            DialogList.Add("Movement|1||");
            DialogList.Add("GOTO|EventWasActiveted:OldMan-YourNameIsStefan|Stefan|");
            DialogList.Add("Person|HeroPortret:Ja||");
            DialogList.Add("Jump|3||");

            DialogList.Add("Jump||Stefan|");
            DialogList.Add("Person|HeroPortret:Stefan||");

            DialogList.Add("GOTO|EventWasActiveted:OldMan-FirstGun|HaveGun|");

            DialogList.Add("|||To najwyraźniej wyjście z tej jaskini.");
            DialogList.Add("|||Impreza musiała być ostra, bo ni huhu nic nie pamiętam...");
            DialogList.Add("|||...lepiej pogadam ze staruszkiem.");
            DialogList.Add("Stop|||");

            DialogList.Add("Jump||HaveGun|");
            DialogList.Add("GOTO|EventWasActiveted:CaveBlocker-FirstQuest|Relax|");
            DialogList.Add("Keyboard|off||");
            DialogList.Add("|||Wiii pierwszy Quest!");
            DialogList.Add("|||Typowo na start zostałem kurierem. Tyle dobrego, że dał mi broń...");
            DialogList.Add("|||...pewnie czeka mnie wystrzałowa podróż. Ajjjć... Sucho.");
            DialogList.Add("|||Zanim wyjdę powinienem wypocząć, ledwo stoję na nogach.");
            DialogList.Add("ActiveEvent|CaveBlocker-FirstQuest||");
            DialogList.Add("Keyboard|on||");

            DialogList.Add("Jump||Relax|");
            DialogList.Add("GOTO|EventWasActiveted:Save-FirstSave|Open|");
            DialogList.Add("|||W sumie mógłbym już ruszać, ale nie mam pełnego zdrowia...");
            DialogList.Add("|||...może jakaś krótka drzemka przed wyprawą?");
            DialogList.Add("Stop|||");

            DialogList.Add("Jump||Open|");
            DialogList.Add("Destroy|||");
        }
        else if (ObjectID == "DarkWood")
        {
            DialogList.Add("Movement|1||");
            DialogList.Add("Person|HeroPortret:Stefan||");
            DialogList.Add("|||Wygląda na to, że ta lokacja jest jeszcze nie gotowa.");
            DialogList.Add("|||Wrócę innym razem ¯\\_( ͡° ͜ʖ ͡°)_/¯    ");
            DialogList.Add("Stop|||");
        }
        else
        {
            DialogList.Add("Destroy|||");
        }

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