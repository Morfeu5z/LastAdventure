using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerDialogManager{

    private Config config;
    private GameObject hero;
    private ControllerPlayer c_hero;
    private GameObject host;

    private int DialogLine;
    private int distanceX = 5;
    private int distanceXCpy = 5;
    private int distanceY = 2;
    private int onTriggerExit = 0;
    private string[] DialogAssistent;
    private string[] DialogPicture;
    private bool stop = false;
    private string callback = "";
    private string picPath = "Sprites/DialogPic/";
    private bool makeSave = false;
    private bool reload = false;

    private List<string> DialogList = new List<string>();


    public ControllerDialogManager(GameObject _Host, List<string> Dialogs, int x, int y)
    {
        config  = GameObject.Find("Config").GetComponent<Config>();
        hero    = GameObject.Find("Hero").gameObject;
        c_hero  = hero.GetComponent<ControllerPlayer>();
        host    = _Host;
        DialogList  = Dialogs;
        distanceX   = x;
        distanceY   = y;
        DialogLine  = 0;
        distanceXCpy    = x + 1;
        DialogAssistent = DialogList[DialogLine].Split('|');
        DialogPicture   = null;
    }

    public string DialogController()
    {
        callback = "";
        if (DialogLine == 0)
        {
            if (DialogAssistent[0] == "OneUse")
            {
                if (config.CheckExistActivetedEvent(DialogAssistent[1]))
                {
                    config.MessageBox(false);
                    KeyboardDisable(false);
                    MovmentDisable(false);
                    c_hero.RunDialog(false);
                    return "Destroy";
                }
                else NextLine();
            }
        }

        if (Mathf.Abs(host.transform.position.x - hero.transform.position.x) < distanceX 
            && Mathf.Abs(host.transform.position.y - hero.transform.position.y) < distanceY)
        {
            if (DialogLine == 0 && distanceXCpy < distanceX) distanceX += 1;
            onTriggerExit = 0;
            if (!c_hero.equipActive && stop == false)
            {
                switch (DialogAssistent[0])
                {
                    case "GOTO":
                        string[] mode = DialogAssistent[1].Split(':');

                        if (mode[0] == "ItemInEquip")
                        {
                            if (config.CheckExistItemInEquip(mode[1])) GoToDialog();
                            else if (DialogAssistent[3] == "") NextLine();
                        }
                        else if (mode[0] == "ItemInActive")
                        {
                            if (config.CheckActiveItem(mode[1])) GoToDialog();
                            else if (DialogAssistent[3] == "") NextLine();
                        }
                        else if (mode[0] == "EventWasActiveted")
                        {
                            string[] activated;
                            bool wasActiveBool = false;
                            try
                            {
                                activated = mode[1].Split(',');
                                for (int i = 0; i < activated.Length; i++)
                                {
                                    if (config.CheckExistActivetedEvent(activated[i])) wasActiveBool = true;
                                    else
                                    {
                                        wasActiveBool = false;
                                        i = activated.Length;
                                    }
                                }
                                if (wasActiveBool) GoToDialog();
                                else if (DialogAssistent[3] == "") NextLine();
                            }
                            catch
                            {
                                if (config.CheckExistActivetedEvent(mode[1])) GoToDialog();
                                else if (DialogAssistent[3] == "") NextLine();
                            }

                        }
                        else if (mode[0] == "EventWasNotActiveted")
                        {
                            if (config.CheckExistActivetedEvent(mode[1]))
                            {
                                if (DialogAssistent[3] == "") NextLine();
                            }
                            else GoToDialog();
                        }
                        else if (mode[0] == "BackIfDontActive")
                        {
                            if (config.CheckExistActivetedEvent(mode[1]))
                            {
                                if (DialogAssistent[3] == "") NextLine();
                            }
                            else GoToDialog("back");
                        }
                        else if (mode[0] == "LeftLife")
                        {
                            if (config.LoadedData.life == Int32.Parse(mode[1])) GoToDialog();
                            else if (DialogAssistent[3] == "") NextLine();
                        }
                        else if (mode[0] == "Back") GoToDialog("back");
                        break;

                    case "Add":
                        string[] item = DialogAssistent[1].Split(':');
                        if (DialogAssistent[3] == "")
                        {
                            config.AddItem(item[0]);
                            NextLine();
                        }
                        else if (Input.GetButtonDown("Submit")) config.AddItem(item[0]);
                        break;

                    case "Drop":
                        config.DropItemByName(DialogAssistent[1]);
                        NextLine();
                        break;

                    case "ActiveEvent":
                        if (!config.CheckExistActivetedEvent(DialogAssistent[1])) config.SetActivetedEvent(DialogAssistent[1]);
                        NextLine();
                        break;

                    case "SetOK":
                        GameObject.Find(DialogAssistent[1]).gameObject.GetComponent<Animator>().SetBool("ok", true);
                        NextLine();
                        break;

                    case "BackToBegin":
                        DialogLine = 0;
                        DialogAssistent = DialogList[DialogLine].Split('|');
                        break;

                    case "Back":
                        if (DialogAssistent[1] != "") DialogLine -= (Int32.Parse(DialogAssistent[1]));
                        else DialogLine--;
                        DialogAssistent = DialogList[DialogLine].Split('|');
                        break;

                    case "Jump":
                        {
                            if (DialogAssistent[1] != "") DialogLine += (Int32.Parse(DialogAssistent[1])) - 1;
                            NextLine();
                        }
                        break;

                    case "Keyboard":
                        {
                            if (DialogAssistent[1] == "off") KeyboardDisable(true);
                            else KeyboardDisable(false);
                            NextLine();
                        }
                        break;

                    case "Stop":
                        {
                            DialogAssistent = "|||".Split('|');
                            stop = true;
                            KeyboardDisable(false);
                            config.MessageBox(false);
                            c_hero.RunDialog(false);
                        }
                        break;

                    case "Movement":
                        {
                            c_hero.MovmentBlocker(Int32.Parse(DialogAssistent[1]));
                            NextLine();
                        }
                        break;

                    case "Person":
                        {
                            if (DialogAssistent[2] == "item") picPath = "Sprites/Items/Items/";
                            else picPath = "Sprites/DialogPic/";
                            DialogPicture = DialogAssistent[1].Split(':');
                            NextLine();
                        }
                        break;

                    case "Destroy":
                        {
                            DialogAssistent = "|||".Split('|');
                            config.MessageBox(false);
                            c_hero.RunDialog(false);
                            c_hero.MovmentBlocker();
                            stop = true;
                            return "Destroy";
                        }

                    case "AutoSave":
                        {
                            config.SaveGame(config.LoadedData.life);
                            NextLine();
                        }
                        break;

                    case "SavePoint":
                        {
                            if (makeSave == false)
                            {
                                makeSave = true;
                                config.SaveAndRespown();
                            }
                        }
                        break;
                        
                    case "GoToFin":
                        {
                            if (reload == false)
                            {
                                reload = true;
                                GameObject.Find("Config").GetComponent<Config>().Fin = true;
                                GameObject.Find("Config").GetComponent<Config>().SceneReload();
                            }
                        }
                        break;
                }
                if (DialogAssistent.Length == 4)
                {
                    if (DialogAssistent[3] != "")
                    {
                        config.MessageBox(true, DialogAssistent[3], DialogPicture[1], picPath + DialogPicture[0]);
                        c_hero.RunDialog(true);
                        if (Input.GetButtonDown("Submit") && stop == false)
                        {
                            callback = "Play";
                            NextLine();
                        }
                    }
                }
                else KeyboardDisable(false);
            }
        }
        else
        {
            onTriggerExit++;
            if (onTriggerExit == 1) ExitDialog();
        }
        return callback;
    }

    public void KeyboardDisable(bool on = false)
    {
        if (on)  c_hero.keyboardOn = false;
        else c_hero.keyboardOn = true;
    }

    public void MovmentDisable(bool on = true) {
        if (on)
        {
            c_hero.MovmentBlocker(-1);
            c_hero.MovmentBlocker(1);
        }
        else c_hero.MovmentBlocker(0);
    }

    public void ExitDialog()
    {
        stop = false;
        DialogPicture = null;
        DialogLine = 0;
        KeyboardDisable(false);
        MovmentDisable(false);
        distanceX = distanceXCpy;
        DialogAssistent = DialogList[DialogLine].Split('|');
        config.MessageBox(false);
        c_hero.RunDialog(false);
    }

    public void NextLine()
    {
        DialogLine++;
        if (DialogList[DialogLine].Length > 2)
        {
            if (DialogList[DialogLine].Substring(0, 2) == "//" || DialogList[DialogLine] == "") NextLine();
            else DialogAssistent = DialogList[DialogLine].Split('|');
        }
        else NextLine();
    }

    public void GoToDialog(string direction = "")
    {
        for (int i = 0; i < DialogList.Count; i++)
        {
            if (DialogList[i].Length > 2)
            {
                if (DialogList[i].Substring(0, 2) == "//" || DialogList[i] == "") {/* Debug.Log("Go to - Skip line: " + i); */ }
                else {
                    string[] tmpDialog = DialogList[i].Split('|');
                    if (tmpDialog[2] == DialogAssistent[2])
                    {
                        DialogLine = i;
                        DialogAssistent = DialogList[i].Split('|');
                        if (direction == "back") i = DialogList.Count;
                    }
                }
            }
        }
    }
}