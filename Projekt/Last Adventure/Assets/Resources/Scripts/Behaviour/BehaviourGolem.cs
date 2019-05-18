using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class BehaviourGolem : MonoBehaviour
{

    public bool demaged         = false;
    public float life           = 100;
    public float maxlife       = 100;
    public float tmpLife       = 0;
    private bool sleep          = true;
    private bool wakeup         = false;

    public AudioClip audioClip;
    private AudioClip audioClipOrigin;
    private GameObject audioSource;
    private bool battleSound = false;

    private string mess        = " You shall not pass! ヽ(°◇° )ノ";

    public Rigidbody2D body;
    private GameObject ExplosionPrefab;
    private GameObject hero;
    private GameObject startPoint;
    private GameObject Golem;
    private Transform feet;
    private Config config;
    private GameObject bossHP;
    private GameObject bossFrame;
    private GameObject bossName;
    private GameObject audioWark;
    private GameObject audioAttakMark;

    private bool golemLookLeft   = true;
    private bool defenceMove     = false;
    private bool follow          = false;
    private bool agresive        = false;
    private bool attack_flag     = false;
    private bool LoadLife        = true;
    private bool heroGoLeft      = false;
    private bool rage            = false;
    public bool defeated         = false;
    public bool closeMessage     = false;

    private float HGSpace  = 0;
    private float PGSpace  = 0;

    public float CoolDownAttack         = 3;
    public float timerCoolDownattack   = -2;

    Vector3 HPSCALE;

    // Use this for initialization
    void Start()
    {
        hero        = GameObject.Find("Hero");
        bossFrame   = GameObject.Find("Canvas/BossHP/Frame");
        bossHP      = GameObject.Find("Canvas/BossHP/Frame/Bar");
        bossName    = GameObject.Find("Canvas/BossHP/Frame/Text");
        startPoint  = GameObject.Find("LocationGolem/StartGolem");
        feet        = GameObject.Find("LocationGolem/BossGolem/Animator/Feet").transform;
        Golem       = GameObject.Find("LocationGolem/BossGolem");
        body        = GameObject.Find("LocationGolem/BossGolem").GetComponent<Rigidbody2D>();
        config      = GameObject.Find("Config").GetComponent<Config>();
        audioSource = GameObject.Find("SoundTrack");
        audioWark   = (GameObject)Resources.Load("Audio/Effects/Prefab/Prefab_AudioGolemWark", typeof(GameObject));
        audioAttakMark = (GameObject)Resources.Load("Audio/Effects/Prefab/Prefab_AudioGolemAttakMark", typeof(GameObject));

        HPSCALE     = bossHP.transform.localScale;
        ExplosionPrefab = (GameObject)Resources.Load("prefabs/Boss/ExplodeGolem", typeof(GameObject));
        if (config.CheckExistActivetedEvent("DeadBossGoodBoss"))
        {
            life = 0;
            defeated = true;
        }
    }

    private void FixedUpdate()
    {
        if (!wakeup && Mathf.Abs(hero.transform.position.x - Golem.transform.position.x) < 25 && !defeated)
        {
            if (config.CheckExistActivetedEvent("Golem-Awaking") && config.CheckExistActivetedEvent("DeadBossGoodBoss") == false)
            {
                config.SetActivetedEvent("ThisIsBoss");
                wakeup = true;
                transform.GetComponent<Animator>().SetBool("WakeUp", true);
            }
        }
        if (!sleep && !defeated)
        {
            PGSpace = Mathf.Abs(startPoint.transform.position.x - Golem.transform.position.x);
            HGSpace = Mathf.Abs(hero.transform.position.x - Golem.transform.position.x);
            if (hero.transform.position.x > Golem.transform.position.x) demaged = true;
            FlipAndHeroDirection();
            if (demaged) agresive = true;
            if (Mathf.Abs(startPoint.transform.position.x - hero.transform.position.x) < 180 && hero.GetComponent<ControllerPlayer>().equipActive == false)
            {
                bossFrame.GetComponent<Animator>().SetBool("Show", true);
                bossName.GetComponent<Text>().text = "Golem";
                lifeFrameUpdate();
                if(config.CheckExistActivetedEvent("DeadBossGoodBoss") == false) BattleMusic(true);
            }
            else
            {
                bossFrame.GetComponent<Animator>().SetBool("Show", false);
                BattleMusic(false);
            }

            if (rage && config.LoadedData.life > 0 && life > 0)
            {
                follow = true;
            }
            else
            {
                if (HGSpace < 50 && agresive && life > 0) {
                    if (HGSpace < 10)
                    {
                        if (HGSpace < 5 && !attack_flag)
                        {
                            rage = true;
                        }
                        else
                        {
                            defenceMove = true;
                        }
                    }
                    else
                    {
                        if (heroGoLeft != golemLookLeft)
                        {
                            defenceMove = false;
                            rage = true;
                        }

                        if (Mathf.Abs(startPoint.transform.position.x - hero.transform.position.x) < 180
                            && Mathf.Abs(startPoint.transform.position.y - hero.transform.position.y) < 80
                            && !attack_flag && life > 0)
                        {
                            MessageUpdate();
                            attackController();
                        }
                        else
                        {
                            follow = false;
                            defenceMove = false;
                            if (agresive) agresive = false;
                            MessageUpdate();
                            if (config.LoadedData.life > 0) BackToPoint();
                        }
                    }
                }
                else
                {
                    if (!agresive && HGSpace < 20 && config.LoadedData.life <= 0 && life > 0)
                    {
                        defenceMove = true;
                    }
                    else
                    {
                        defenceMove = false;
                        BackToPoint();
                    }
                }
            }


            // Update const
            if (life > 0)
            {
                Attack();
                Follow();
            }
                Defence();
        }
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

    public void Follow()
    {
        if ((follow && !attack_flag && !defenceMove) || rage) {
            if (defenceMove) defenceMove = false;
            if (Mathf.Abs(hero.transform.position.x - Golem.transform.position.x) > 10)
            {
                if (rage && config.LoadedData.life > 0)
                {
                    if (!attack_flag)
                    {
                        defenceMove = false;
                        if (timerCoolDownattack <= -2) timerCoolDownattack = CoolDownAttack;
                        rage = false;
                    }
                    else
                    {
                        defenceMove = true;
                    }
                }
                if (hero.transform.position.x - Golem.transform.position.x > 0 && !defenceMove)
                {
                    body.velocity = new Vector2(20, body.velocity.y);
                }
                else if (hero.transform.position.x - Golem.transform.position.x < 0 && !defenceMove)
                {
                    body.velocity = new Vector2(-20, body.velocity.y);
                }
            }
            else
            {
                if (rage && config.LoadedData.life > 0)
                {
                    if (Mathf.Abs(hero.transform.position.x - Golem.transform.position.x) < 3)
                    {
                        if (!attack_flag)
                        {
                            defenceMove = false;
                            if (timerCoolDownattack <= -2) timerCoolDownattack = CoolDownAttack;
                            rage = false;
                        }
                        else
                        {
                            defenceMove = true;
                        }
                    }
                    else
                    {
                        defenceMove = true;
                    }
                    
                }
            }
        }
    }

    public void FlipAndHeroDirection()
    {
        // Hero move direction
        if (hero.transform.localScale.x < 0)
        {
            heroGoLeft = true;
        }
        else
        {
            heroGoLeft = false;
        }

        // Golem flip
        if (!attack_flag && defenceMove == false)
        {
            if (config.LoadedData.life > 0 && life > 0)
            {
                if (hero.transform.position.x > transform.position.x && golemLookLeft)
                {
                    golemLookLeft = false;
                    Flip();
                }
                else if (hero.transform.position.x < transform.position.x && golemLookLeft == false)
                {
                    golemLookLeft = true;
                    Flip();
                }
            }
            else
            {
                if (startPoint.transform.position.x > transform.position.x && golemLookLeft)
                {
                    golemLookLeft = false;
                    Flip();
                }
                else if (startPoint.transform.position.x < transform.position.x && golemLookLeft == false)
                {
                    golemLookLeft = true;
                    Flip();
                }
            }
        }
    }

    public void BackToPoint()
    {
        defenceMove = false;
        if (Mathf.Abs(startPoint.transform.position.x - Golem.transform.position.x) < 5 && life <= 0)
        {
            transform.GetComponent<Animator>().SetBool("WakeUp", false);
            bossFrame.GetComponent<Animator>().SetBool("Show", false);
            config.SetActivetedEvent("DeadBossGoodBoss");
            BattleMusic(false);
            closeMessage = true;
            MessageUpdate();
        }
        else if (Mathf.Abs(startPoint.transform.position.x - Golem.transform.position.x) > 5)
        {
            if (startPoint.transform.position.x - Golem.transform.position.x > 1)
            {
                body.velocity = new Vector2(10, body.velocity.y);
            }
            else if (startPoint.transform.position.x - Golem.transform.position.x < 1)
            {
                body.velocity = new Vector2(-10, body.velocity.y);
            }
        }
    }

    public void attackController()
    {
        if (defenceMove)
        {
            if (agresive && HGSpace >= 30) defenceMove = false;
            if (!agresive && HGSpace >= 15) defenceMove = false;
        }
        else if (agresive && HGSpace <= 15) {
            follow = false;
            if (attack_flag == false && timerCoolDownattack <= -2)
            {
                timerCoolDownattack = CoolDownAttack;
            }
            else
            {
                defenceMove = true;
            }
        }
        else if(agresive && HGSpace > 30)
        {
            follow = true;
        }
        else if (!agresive)
        {
            if (HGSpace <= 15)
            {
                defenceMove = true;
            }
            else
            {
                defenceMove = false;
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

    public void Attack()
    {
        if(timerCoolDownattack == CoolDownAttack && attack_flag == false)
        {
            transform.GetComponent<Animator>().SetTrigger("Rush");
            Destroy(Instantiate(audioAttakMark, transform, false), 1f);
            Vector3 newPos = new Vector3(feet.transform.position.x, feet.transform.position.y, feet.transform.position.z);
            GameObject tmp = Instantiate(ExplosionPrefab, newPos, Quaternion.identity);
            if (!golemLookLeft) {
                Vector3 newScale = tmp.transform.localScale;
                newScale.x *= -1;
                tmp.transform.localScale = newScale;
            }
            Destroy(tmp, 1.5f);
            attack_flag = true;
        }
        else if(timerCoolDownattack > -1 && timerCoolDownattack < 0)
        {
            timerCoolDownattack = -2;
            defenceMove = true;
            attack_flag = false;
        }
        else if (timerCoolDownattack > 0)
        {
            timerCoolDownattack -= Time.deltaTime;
        }
    }

    public void Flip()
    {
        Vector3 newVector = Golem.transform.localScale;
        newVector.x *= -1;
        Golem.transform.localScale = newVector;
    }

    public void Defence()
    {
        if (defenceMove)
        {
            defenceMove = true;
            transform.GetComponent<Animator>().SetBool("Def", true);
        }
        else
        {
            defenceMove = false;
            transform.GetComponent<Animator>().SetBool("Def", false);
        }
    }

    public void setAttackFlag()
    {
        attack_flag = true;
    }

    public void offAttackFlag()
    {
        attack_flag = false;
    }

    public void sleepOff()
    {
        sleep = false;
    }

    public void GolemAwaken()
    {
        wakeup = true;
        config.SetActivetedEvent("GolemAwaken");
    }

    public void MessageUpdate()
    {
        if (!defeated && HGSpace < 180 && !sleep) config.MessageBox(true, mess, "Strażnik", "Sprites/DialogPic/GolemPortret");
        if (life <= 0 && PGSpace < 10 && closeMessage) config.MessageBox(false);
    }

    public void DropLife(float drop)
    {
        if (!sleep && !defeated && life > 0)
        {
            if (life > 0)
            {
                life -= drop;
                bossFrame.GetComponent<Animator>().SetTrigger("Shake");
                if (demaged == false) demaged = true;
                rage = true;
            }
            if (life <= 0)
            {
                life = 0;
                transform.GetComponent<Animator>().SetTrigger("Defeated");
                string path = GameObject.Find("Config").GetComponent<Config>().getLangPath() + "DialogGolemLastWords.txt";
                StreamReader reader = new StreamReader(path);
                mess = reader.ReadToEnd();
                reader.Close();
                MessageUpdate();
                lifeFrameUpdate(); 
            }
        }
    }
    public void setDefeatedTrue()
    {
        defeated = true;
        Destroy(Instantiate(audioWark, transform, false), 3f);
    }
    public void setDefeatedFalse()
    {
        defeated = false;
    }
}
