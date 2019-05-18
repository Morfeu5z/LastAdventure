using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourSlime : MonoBehaviour {

    public int EnemyLifes = 3;
    public int PointsAdd = 10;
    public float JumpForce = 1.5f;
    public bool Angry = true;
    private int EnemyLifesCopy = 3;
    private bool IsDemage = false;
    private bool Agresive = false;
    private bool canMove = true;
    private GameObject toxicPrefab;
    private GameObject hero;

    private float positionZero;
    private float positionNext;
    private  float Range = 10;

    public float  respownTime = 20;
    private float respownSlime = -2;
    private float moveTimer = 0;
    private float moveDirection = 1;
    private float space = 1;
    private Rigidbody2D body;
    private GameObject prefBomb;
    private GameObject audioAttack;
    private GameObject audioDeath;
    private GameObject audioPuh;
    public int BombSpawnCount = 5;
    public float BombBrakeTime = 5;
    private float bombSpam = 1;
    private float bombSpamBreake = 0;
    private int makeBombs = 0;

    private void Start()
    {
        toxicPrefab = (GameObject)Resources.Load("prefabs/toxicCloud", typeof(GameObject));
        body = transform.GetComponent<Rigidbody2D>();
        positionZero = transform.position.x;
        positionNext = transform.position.x;
        hero = GameObject.Find("Hero");
        prefBomb = (GameObject)Resources.Load("Prefabs/Enemys/SlimeBomb", typeof(GameObject));
        audioAttack = (GameObject)Resources.Load("Audio/Effects/Prefab/Prefab_AudioSlimeAttack", typeof(GameObject));
        audioDeath = (GameObject)Resources.Load("Audio/Effects/Prefab/Prefab_AudioSlimeDeath", typeof(GameObject));
        audioPuh = (GameObject)Resources.Load("Audio/Effects/Prefab/Prefab_AudioSlimeJump", typeof(GameObject));
        EnemyLifesCopy = EnemyLifes;
    }

    private void FixedUpdate()
    {
        if (respownSlime > -1 && respownSlime < 0)
        {
            respownSlime = -2;
            transform.GetComponent<Animator>().SetBool("Death", false);
            EnemyLifes = EnemyLifesCopy;
            Agresive = false;
            canMove = true;
            IsDemage = false;
        }
        else if (respownSlime > 0) respownSlime -= Time.deltaTime;

        if (canMove) { 
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
        if (Mathf.Abs(hero.transform.position.x - transform.position.x) < 10 && Mathf.Abs(hero.transform.position.y - transform.position.y) < 10 && EnemyLifes > 0 && Angry) Agresive = true;
        if (Agresive)
        {
            if (Mathf.Abs(hero.transform.position.x - transform.position.x) > 80 || EnemyLifes <= 0) Agresive = false;
            if(bombSpamBreake <= 0 )
            {
                if(makeBombs <= 0) makeBombs = BombSpawnCount;
            }
            else{
                bombSpamBreake -= Time.deltaTime;
            }

            if (bombSpam <= 0)
            {
                if (makeBombs > 0)
                {
                    Vector3 newVec = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    Instantiate(prefBomb, newVec, Quaternion.identity);
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

    public void DropLife(float demage = 1)
    {
        if (!IsDemage)
        {
            IsDemage = true;
            EnemyLifes -= (int)demage;
            Agresive = true;
            if (EnemyLifes > 0)
            {
                transform.GetComponent<Animator>().SetTrigger("Attack");
                Destroy(Instantiate(audioAttack, transform, false), 1f);
            }
            else
            {
                Agresive = false;
                canMove = false;
                transform.GetComponent<Animator>().SetBool("Death", true);
                Destroy(Instantiate(audioDeath, transform, false), 1f);
            }
            GameObject.Find("MainCamera/Camera").GetComponent<Animator>().SetTrigger("shake");
        }
    }
    public void HeadJump()
    {
        if (canMove)
        {
            Destroy(Instantiate(audioPuh, transform, false), 0.5f);
            GameObject.Find("MainCamera/Camera").GetComponent<Animator>().SetTrigger("shake");
        }
    }

    public void makeToxic()
    {
        Vector3 newVec = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        GameObject tmp = Instantiate(toxicPrefab, newVec, Quaternion.identity);
        Destroy(tmp, 1.1f);
    }


    public void Death()
    {
        respownSlime = respownTime;
    }
    

    public void DamageEnable()
    {
        IsDemage = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<ControllerPlayer>().playerCollision(gameObject, JumpForce);
            HeadJump();
        }
    }
}
