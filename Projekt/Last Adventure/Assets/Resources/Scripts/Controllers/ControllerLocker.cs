using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerLocker : MonoBehaviour
{

    private GameObject config;
    private GameObject hero;
    private GameObject CataDoor;
    private int DialogLine;
    private int distanceX = 1;
    private int distanceY = 20;
    private int onTriggerExit = 0;
    private string[] Dialoconfiganager;
    private string locker;
    private bool stop = false;
    private bool open = false;

    private List<string> DialogList = new List<string>();

    // Use this for initialization
    void Start()
    {
        CataDoor = GameObject.Find("FindCatacumbsDoor");
        config = GameObject.Find("Config").gameObject;
        hero = GameObject.Find("Hero").gameObject;
        locker = "Locker";

        DialogLine = 0;
        DialogList.Add("Skip|wasActive:Sally-PowerGenerator|1|Wygląda na to, że nie ma zasilania.");
        DialogList.Add("Back|||");
        DialogList.Add("Jump||1|");
        DialogList.Add("Active|Lab-Door||");
        DialogList.Add("Skip|Item:KeyCardBlue|2|Wymagana karta dostępu poziomu niebieskiego.");
        DialogList.Add("Back|||");
        DialogList.Add("Jump||2|");
        DialogList.Add("Open|||");
        DialogList.Add("Skip||2|Przyznano dostęp.");
        DialogList.Add("Back|||");
        Dialoconfiganager = DialogList[DialogLine].Split('|');
    }


    private void Update()
    {
        if (Mathf.Abs(transform.position.x - hero.transform.position.x) < distanceX && Mathf.Abs(transform.position.y - hero.transform.position.y) < distanceY)
        {
            onTriggerExit = 0;

            if(open == false) hero.GetComponent<ControllerPlayer>().MovmentBlocker(-1);

            switch (Dialoconfiganager[0])
            {
                case "Skip":
                    string[] mode = Dialoconfiganager[1].Split(':');
                    if (mode[0] == "wasActive")
                    {
                        if (config.GetComponent<Config>().CheckExistActivetedEvent(mode[1]))
                        {
                            locker = "LockerOn";
                            skipDialog();
                        }
                    }
                    break;

                case "Back":
                    BackLine();
                    break;

                case "Active":
                    if(Dialoconfiganager[1] == "Lab-Door")
                    {
                        if (config.GetComponent<Config>().CheckExistActivetedEvent("Sally-PowerGenerator"))
                        {
                            if (!config.GetComponent<Config>().CheckExistActivetedEvent("Lab-Door")) config.GetComponent<Config>().SetActivetedEvent("Lab-Door");
                        }
                         transform.GetComponent<SpriteRenderer>().enabled = true;
                        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
                        CataDoor.GetComponent<Animator>().SetBool("Locker", true);
                    }
                    NextLine();
                    break;

                case "Jump":
                    NextLine();
                    break;

                case "Open":
                    CataDoor.GetComponent<Animator>().SetBool("Open", true);
                    open = true;
                    NextLine();
                    break;
            }

            if (stop == false) config.GetComponent<Config>().MessageBox(true, Dialoconfiganager[3], "Zamek Cyfrowy", "Sprites/Items/Elements/" + locker);
            if (Input.GetButtonDown("Submit") && stop == false)
            {
                DialogLine++;
                Dialoconfiganager = DialogList[DialogLine].Split('|');
            }
        }
        else
        {
            onTriggerExit++;
            if (onTriggerExit == 1)
            {
                config.GetComponent<Config>().MessageBox(false);
                CataDoor.GetComponent<Animator>().SetBool("Open", false);
                hero.GetComponent<ControllerPlayer>().MovmentBlocker();
            }
        }
    }

    public void NextLine()
    {
        DialogLine++;
        Dialoconfiganager = DialogList[DialogLine].Split('|');
    }

    public void BackLine()
    {
        DialogLine--;
        Dialoconfiganager = DialogList[DialogLine].Split('|');
    }

    public void skipDialog()
    {
        for (int i = 0; i < DialogList.Count; i++)
        {
            string[] tmpDialog = DialogList[i].Split('|');
            if (tmpDialog[2] == Dialoconfiganager[2])
            {
                DialogLine = i;
                Dialoconfiganager = DialogList[DialogLine].Split('|');
            }
        }
    }
}
  