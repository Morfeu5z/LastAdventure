using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BehaviourYamiko : MonoBehaviour {

    private string BN = "???";

    public AudioClip audioClip;
    private AudioClip audioClipOrigin;
    private GameObject audioSource;
    private bool battleSound = false;

    private bool yamiko = true;
    private float HeroDistance;
    private GameObject hero;
    private GameObject Yami;

    public float life = 50;
    public float maxlife = 50;
    public float tmpLife = 0;
    private Config config;
    private GameObject bossHP;
    private GameObject bossFrame;
    private GameObject bossName;
    private ControllerPlayer cHero;
    Vector3 HPSCALE;
    private bool LoadLife = true;
    private bool firstLook = false;
    private bool YamiLookLeft = true;

    // Use this for initialization
    void Start () {
        hero = GameObject.Find("Hero");
        if (transform.name == "YamikoSleep")
        {
            yamiko = false;
            Yami = GameObject.Find("Yamiko");
            Yami.SetActive(false);
        }

        config = GameObject.Find("Config").GetComponent<Config>();
        bossFrame = GameObject.Find("Canvas/BossHP/Frame");
        bossHP = GameObject.Find("Canvas/BossHP/Frame/Bar");
        bossName = GameObject.Find("Canvas/BossHP/Frame/Text");
        audioSource = GameObject.Find("SoundTrack");

        cHero = hero.GetComponent<ControllerPlayer>();
        HPSCALE = bossHP.transform.localScale;
	}

    private void FixedUpdate()
    {
        if (yamiko)
        {
            if (hero.transform.position.x > transform.position.x && YamiLookLeft)
            {
                YamiLookLeft = false;
                Flip();
            }
            else if (hero.transform.position.x < transform.position.x && YamiLookLeft == false)
            {
                YamiLookLeft = true;
                Flip();
            }

            if (Mathf.Abs(hero.transform.position.x - transform.position.x) < 15 && Mathf.Abs(hero.transform.position.y - transform.position.y) < 10)
            {
                if (firstLook == false)
                {
                    if (config.CheckExistActivetedEvent("Yamiko") == false) BattleMusic(true);
                    if (config.CheckExistActivetedEvent("YamikoFirstLook"))
                    {
                        firstLook = true;
                        transform.GetComponent<Animator>().SetBool("Wait", false);
                    }
                    else
                    {
                        transform.GetComponent<Animator>().SetBool("Wait", true);
                    }
                }
                if (BN == "???") {
                    if (config.CheckExistActivetedEvent("MyNameIsMiko"))
                    {
                        BN = "Miko";
                        BattleMusic(false);
                    }
                }else if(BN == "Miko")
                {
                    if (config.CheckExistActivetedEvent("MyNameIsYamiko"))
                    {
                        BN = "Yamiko";
                    }
                }
                if (hero.GetComponent<ControllerPlayer>().equipActive == false)
                {
                    bossFrame.GetComponent<Animator>().SetBool("Show", true);
                    bossName.GetComponent<Text>().text = BN;
                    lifeFrameUpdate();
                    if (config.CheckExistActivetedEvent("MyNameIsYamiko"))
                    { 
                        BattleMusic(false);
                    }
                }
            }
            else
            {
                bossFrame.GetComponent<Animator>().SetBool("Show", false);
                BattleMusic(false);
            }
        }
        else
        {
		    if(Mathf.Abs(hero.transform.position.x - transform.position.x) < 20 && Mathf.Abs(hero.transform.position.y - transform.position.y) < 20)
            {
                transform.GetComponent<Animator>().SetBool("Awake", true);
                if (hero.GetComponent<ControllerPlayer>().equipActive == false)
                {
                    cHero.keyboardOn = false;
                    bossFrame.GetComponent<Animator>().SetBool("Show", true);
                    bossName.GetComponent<Text>().text = BN;
                    lifeFrameUpdate();
                }
                else
                {
                    bossFrame.GetComponent<Animator>().SetBool("Show", false);
                }
            }
        }
	}


    public void BattleMusic(bool on = true)
    {
        if (!battleSound && on)
        {
            audioClipOrigin = audioSource.GetComponent<AudioSource>().clip;
            audioSource.GetComponent<ControllerAudio>().changeAudio(audioClip);
            battleSound = true;
        }
        else if (battleSound && !on)
        {
            audioSource.GetComponent<ControllerAudio>().changeAudio(audioClipOrigin);
            battleSound = false;
        }
    }

    public void Flip()
    {
        Vector3 newVector = transform.parent.transform.localScale;
        newVector.x *= -1;
        transform.parent.transform.localScale = newVector;
    }

    public void AwakeMe()
    {
#pragma warning disable CS0618 // Typ lub składowa jest przestarzała
        if (Yami.active == false) Yami.SetActive(true);
#pragma warning restore CS0618 // Typ lub składowa jest przestarzała
    }

    public void KillMe()
    {
        cHero.keyboardOn = true;
        Destroy(gameObject);
    }


    public void lifeFrameUpdate()
    {
        if (LoadLife)
        {
            if (tmpLife <= maxlife)
            {
                HPSCALE = bossHP.transform.localScale;
                HPSCALE.x = tmpLife / maxlife;
                bossHP.transform.localScale = HPSCALE;
                tmpLife++;
            }
            else
            {
                LoadLife = false;
            }
        }
        else
        {
            HPSCALE = bossHP.transform.localScale;
            HPSCALE.x = life / maxlife;
            bossHP.transform.localScale = HPSCALE;
        }
    }

    public void chName(string name)
    {
        BN = name;
    }
}
