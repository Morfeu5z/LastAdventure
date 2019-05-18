using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BehaviourSlimeBoss : MonoBehaviour {

    public float life = 30;
    public float maxlife = 30;
    public float tmpLife = 0;

    private bool IsDemage = false;
    public bool Agresive = false;
    private bool canMove = true;
    private GameObject toxicPrefab;
    private GameObject hero;

    private float positionZero;
    private float positionNext;
    private float Range = 10;

    private float moveTimer = 0;
    private float moveDirection = 1;
    private float space = 1;
    private Rigidbody2D body;
    private GameObject prefBomb;
    private GameObject prefFlyBomb;
    private GameObject audioAttack;
    private GameObject audioDeath;
    private GameObject audioPuh;
    public int BombSpawnCount = 5;
    public float BombBrakeTime = 5;
    private float bombSpam = 1;
    private float bombSpamBreake = 0;
    private int makeBombs = 0;
    private bool LoadLife = true;
    private bool dead = false;

    private GameObject bossHP;
    private GameObject bossFrame;
    private GameObject bossName;
    private Config config;

    Vector3 HPSCALE;

    public AudioClip audioClip;
    private AudioClip audioClipOrigin;
    private GameObject audioSource;
    private bool battleSound = false;

    private void Start()
    {
        toxicPrefab = (GameObject)Resources.Load("prefabs/toxicCloud", typeof(GameObject));
        body = transform.GetComponent<Rigidbody2D>();
        positionZero = transform.position.x;
        positionNext = transform.position.x;
        hero = GameObject.Find("Hero");
        audioSource = GameObject.Find("SoundTrack");
        prefBomb = (GameObject)Resources.Load("Prefabs/Enemys/SlimeBomb", typeof(GameObject));
        prefFlyBomb = (GameObject)Resources.Load("Prefabs/Enemys/GigaSlimeBomb", typeof(GameObject));
        audioAttack = (GameObject)Resources.Load("Audio/Effects/Prefab/Prefab_AudioSlimeAttack", typeof(GameObject));
        audioDeath = (GameObject)Resources.Load("Audio/Effects/Prefab/Prefab_AudioSlimeDeath", typeof(GameObject));
        audioPuh = (GameObject)Resources.Load("Audio/Effects/Prefab/Prefab_AudioSlimeJump", typeof(GameObject));

        bossFrame = GameObject.Find("Canvas/BossHP/Frame");
        bossHP = GameObject.Find("Canvas/BossHP/Frame/Bar");
        bossName = GameObject.Find("Canvas/BossHP/Frame/Text");
        config = GameObject.Find("Config").GetComponent<Config>();

        HPSCALE = bossHP.transform.localScale;

        if (config.CheckExistActivetedEvent("Slime-Death"))
        {
            BossIsDead();
            dead = true;
        }
    }

    private void FixedUpdate()
    {
        if (!dead)
        {
            if (canMove)
            {
                if (moveTimer < 0)
                {
                    int flip = (Random.Range(0, 1) == 0) ? -1 : 1;
                    positionNext = positionZero + (Random.Range(2, Range) * flip);
                    moveTimer = 5;
                }
                else
                {
                    moveTimer -= Time.deltaTime;
                }
                space = Mathf.Abs(transform.position.x - positionNext);
                if (space > 2)
                {
                    moveDirection = (transform.position.x < positionNext) ? 1 : -1;
                    body.velocity = new Vector2(moveDirection * 5, body.velocity.y);
                }
            }
            if (Mathf.Abs(hero.transform.position.x - transform.position.x) < 50 && Mathf.Abs(hero.transform.position.y - transform.position.y) < 50) Agresive = true;
            if (Mathf.Abs(hero.transform.position.x - transform.position.x) < 150 && Mathf.Abs(hero.transform.position.y - transform.position.y) > 51) Agresive = false;
            if (Agresive)
            {

                if (hero.GetComponent<ControllerPlayer>().equipActive == false)
                {
                    bossFrame.GetComponent<Animator>().SetBool("Show", true);
                    bossName.GetComponent<Text>().text = "Giga Slime";
                    lifeFrameUpdate();
                    BattleMusic(true);
                }
                else
                {
                    bossFrame.GetComponent<Animator>().SetBool("Show", false);
                    BattleMusic(false);
                }

                if (bombSpamBreake <= 0)
                {
                    if (makeBombs <= 0) makeBombs = BombSpawnCount;
                }
                else
                {
                    bombSpamBreake -= Time.deltaTime;
                }

                if (bombSpam <= 0)
                {
                    if (makeBombs > 0)
                    {
                        Vector3 newVec = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                        Instantiate(prefBomb, newVec, Quaternion.identity);
                        Instantiate(prefFlyBomb, newVec, Quaternion.identity);
                        if (makeBombs == 1) bombSpamBreake = BombBrakeTime;
                        makeBombs--;
                        bombSpam = 0.5f;
                    }
                }
                else
                {
                    bombSpam -= Time.deltaTime;
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

    public void DropLife(float demage = 1)
    {
            Agresive = true;
            if (life > 0)
            {
                life--;
                transform.GetComponent<Animator>().SetTrigger("Attack");
                Destroy(Instantiate(audioAttack, transform, false), 1f);
            }
            
            if(life <= 0){
                Agresive = false;
                canMove = false;
                dead = true;
                config.SetActivetedEvent("Slime-Death");
                bossFrame.GetComponent<Animator>().SetBool("Show", false);
                transform.GetComponent<Animator>().SetTrigger("Death");
                Destroy(Instantiate(audioDeath, transform, false), 1f);
                BattleMusic(false);
        }
            GameObject.Find("MainCamera/Camera").GetComponent<Animator>().SetTrigger("shake");
        
    }
    public void HeadJump()
    {
        Destroy(Instantiate(audioPuh, transform, false), 0.5f);
        GameObject.Find("MainCamera/Camera").GetComponent<Animator>().SetTrigger("shake");
    }

    public void makeToxic()
    {
        Vector3 newVec = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        GameObject tmp = Instantiate(toxicPrefab, newVec, Quaternion.identity);
        Destroy(tmp, 1.1f);
        tmp = Instantiate(toxicPrefab, newVec, Quaternion.identity);
        newVec = new Vector3(transform.position.x + 10, transform.position.y + 1f, transform.position.z);
        Destroy(tmp, 1.1f);
        tmp = Instantiate(toxicPrefab, newVec, Quaternion.identity);
        newVec = new Vector3(transform.position.x - 10, transform.position.y + 1f, transform.position.z);
        Destroy(tmp, 1.1f);
    }


    public void Death()
    {
        Destroy(gameObject);
    }

    public void DamageEnable()
    {
        IsDemage = false;
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

    public void BossIsDead()
    {
        Destroy(gameObject);
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Demage")
        {
            if (collision.name.Substring(0, 6) == "Bullet" && IsDemage == false)
            {
                IsDemage = true;
                DropLife();
            }
        }
        if (collision.tag == "Player")
        {
            collision.GetComponent<ControllerPlayer>().playerCollision(gameObject, 1.5f);
            HeadJump();
        }
    }
}