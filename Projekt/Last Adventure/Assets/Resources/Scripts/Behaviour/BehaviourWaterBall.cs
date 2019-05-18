using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourWaterBall : MonoBehaviour {

    public float Speed = 0.01f;
    public float MinHeight = 1;
    public float maxHeight = 10;
    private GameObject hero;
    private GameObject body;
    private GameObject PrefExplo;
    private string switcher = "Wait";
    private float timer = 6;
    private float RandomHeigh;

    // Use this for initialization
    void Start () {
        hero = GameObject.Find("Hero/Feet");
        transform.GetComponent<Animator>().enabled = false;
        transform.GetComponent<CircleCollider2D>().enabled = false;
        body = transform.parent.gameObject;
        RandomHeigh = Random.Range(MinHeight, maxHeight);
        timer = Random.Range(4, 8);
        Speed = Random.Range(Speed, Speed*2);
    }

    private void FixedUpdate()
    {
        switch (switcher)
        {
            case "Active":
                transform.GetComponent<Animator>().enabled = true;
                transform.GetComponent<BoxCollider2D>().enabled = false;
                RandomHeigh = Random.Range(MinHeight, maxHeight);
                switcher = "Up";
                break;
            case "Up":
                if (body.transform.localPosition.y < RandomHeigh)
                {
                    body.transform.localPosition = new Vector3(body.transform.localPosition.x, body.transform.localPosition.y + 0.02f, 0);
                }
                else
                {
                    switcher = "ActiveDemage";
                }
                break;
            case "ActiveDemage":
                transform.GetComponent<CircleCollider2D>().enabled = true;
                transform.gameObject.tag = "Demage";
                if (body.transform.position.x > hero.transform.position.x) Speed *= -1;
                switcher = "Follow";
                break;
            case "Follow":
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }else
                {
                    switcher = "Explode";
                }
                body.transform.localPosition = new Vector3(body.transform.localPosition.x + Speed, body.transform.localPosition.y, 0);
                break;
            case "Explode":
                body.transform.localPosition = new Vector3(body.transform.localPosition.x, body.transform.localPosition.y - 0.1f, 0);
                transform.GetComponent<CircleCollider2D>().enabled = false;
                transform.GetComponent<Animator>().SetBool("Explode", true);
                break;
        }
    }
    
    public void DestroyMe()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if(switcher == "Wait") switcher = "Active";
            if(switcher == "Follow") switcher = "Explode";
        }
        else if (collision.tag == "Demage")
        {
            if (collision.name.Substring(0, 6) == "Bullet")
            {
                switcher = "Explode";
            }
        }
    }
}
