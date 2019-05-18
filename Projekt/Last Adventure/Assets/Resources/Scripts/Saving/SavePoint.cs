using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{

    public string Img = "trunk";
    private Config config;
    private GameObject hero;
    private ControllerPlayer c_hero;
    private bool saving = false;
    private int distanceX = 1;
    private int distanceY = 2;
    private int onTriggerExit = 0;

    private void Start()
    {
        // Create new save if new save dont exist
        hero = GameObject.Find("Hero").gameObject;
        c_hero = GameObject.Find("Hero").GetComponent<ControllerPlayer>();
        config = GameObject.Find("Config").GetComponent<Config>();
    }

    private void Update()
    {
        if (Mathf.Abs(transform.position.x - hero.transform.position.x) < distanceX && Mathf.Abs(transform.position.y - hero.transform.position.y) < distanceY)
        {
            if (!c_hero.equipActive)
            {
                onTriggerExit = 0;
                if (config.CheckExistActivetedEvent("OldMan-FirstSavePoint"))
                {
                    config.MessageBox(true, ">> Odzyskaj siły <<", "Zapis Gry", "Sprites/Items/Elements/" + Img);
                    if (Input.GetButtonDown("Submit"))
                    {
                        if (!saving && !c_hero.equipActive)
                        {
                            if (!config.CheckExistActivetedEvent("Save-FirstSave")) config.SetActivetedEvent("Save-FirstSave");
                            saving = true;
                            config.SaveAndRespown();
                        }
                    }
                }
            }
        }
        else
        {
            onTriggerExit++;
            if (onTriggerExit == 1)
            {
                config.MessageBox(false);
            }
        }
    }
}
