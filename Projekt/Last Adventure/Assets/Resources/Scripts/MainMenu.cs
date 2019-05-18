using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;

public class MainMenu : MonoBehaviour {

    public GameObject SelectBtnSound;

    private int child = 0;
    private int childCount = 1;

    private bool move = false;

    private bool Loading = false;
    private bool block = false;
    public string soundName;
    public string effectName;

    private Config config;
    private GameObject mainmenu;
    private GameObject control;
    private GameObject settingsmenu;

    void Start ()
    {
        config = GameObject.Find("Config").GetComponent<Config>();
        control = GameObject.Find("Canvas/Menu/control");
        mainmenu = GameObject.Find("Canvas/Menu/main");
        settingsmenu = GameObject.Find("Canvas/Menu/settings");


        childCount = transform.childCount - 1;
        transform.GetChild(0).GetComponent<Animator>().SetBool("Select", true);

        if (transform.name == "main")
        {
            gameObject.SetActive(true);
            config.MainMenuConfig();
        }
        else gameObject.SetActive(false);
    }

    void Update ()
    {
        if (transform.name == "settings")
        {
            string SoundVolume = (config.config.SoundtrackVolume * 100).ToString("0");
            string txt = "< " + soundName + ": " + SoundVolume + " >";
            settingsmenu.transform.GetChild(1).gameObject.transform.GetComponent<Text>().text = txt;

            SoundVolume = (config.config.EffectVolume * 100).ToString("0");
            txt = "< " + effectName + ": " + SoundVolume + " >";
            settingsmenu.transform.GetChild(2).gameObject.transform.GetComponent<Text>().text = txt;
        }

        if(!block) menuMovement();

        loading();
    }

    private void OnEnable()
    {
        transform.GetChild(child).GetComponent<Animator>().SetBool("Select", true);
    }

    public void menuMovement()
    {
        if (Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0) move = true;
        if (move)
        {
            if (Input.GetAxisRaw("Vertical") > 0) Move("up");
            else if (Input.GetAxisRaw("Vertical") < 0) Move("down");
            else if (Input.GetAxisRaw("Horizontal") > 0) { if (transform.name == "settings") Move("right"); }
            else if (Input.GetAxisRaw("Horizontal") < 0) { if (transform.name == "settings") Move("left"); }
            else if (Input.GetButtonDown("Accept"))
            {
                Destroy(Instantiate(SelectBtnSound, transform, false), 0.1f);
                switch (transform.GetChild(child).gameObject.transform.name)
                {
                    case "continue":
                        config.LoadGame();
                        Loading = true;
                        block = true;
                        break;

                    case "newgame":
                        config.NewGame();
                        Loading = true;
                        block = true;
                        break;

                    case "settings":
                        gameObject.SetActive(false);
                        settingsmenu.SetActive(true);
                        config.saveSettings = true;
                        break;

                    case "language":
                        config.nextLang();
                        break;

                    case "control":
                        gameObject.SetActive(false);
                        control.SetActive(true);
                        break;

                    case "exit":
                        Application.Quit();
                        break;

                    case "back":
                        gameObject.SetActive(false);
                        mainmenu.SetActive(true);
                        if (config.saveSettings)
                        {
                            config.saveConfig();
                            config.saveSettings = false;
                        }
                            break;

                    default:
                        Debug.Log("Cos nie pyklo w menu :/");
                        break;
                }
            }
        }
    }

    public void Move(string go)
    {
        if (go == "up" || go == "down")
        {
            int add = 0;
            if (go == "up" && child > 0) add = -1;
            else if (go == "down" && child < childCount) add = 1;
            if (add != 0)
            {
                child += add;
                if (transform.GetChild(child).GetComponent<Animator>() != null)
                {
                    transform.GetChild(child - add).GetComponent<Animator>().SetBool("Select", false);
                    transform.GetChild(child).GetComponent<Animator>().SetBool("Select", true);
                    GetComponent<AudioSource>().Play();
                }
                else child -= add;
            }
        }else if(go == "left" || go == "right") {
            string name = transform.GetChild(child).gameObject.transform.name;
            string direction = (go == "left") ? "down" : "up" ;
            if (name == "sound" || name == "effect") config.volume(name, direction);
        }
        move = false;
    }
    
    public void loading()
    {
        if (Loading)
        {
            config.config.Language = 0;
            GameObject.Find("Canvas/LoadScreen").GetComponent<Animator>().SetBool("LoadOn", true);
            GameObject.Find("SoundTrack").GetComponent<ControllerAudio>().Turn("off");
            Loading = false;
        }
    }
}
