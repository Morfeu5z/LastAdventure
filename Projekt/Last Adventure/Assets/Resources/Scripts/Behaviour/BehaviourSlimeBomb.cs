using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourSlimeBomb : MonoBehaviour {

    private GameObject hero;
    private GameObject PrefExplo;
    private Rigidbody2D body;
    private int dir;
    private float timer = 10;

	// Use this for initialization
	void Start () {
        hero = GameObject.Find("Hero");
        body = transform.GetComponent<Rigidbody2D>();
        PrefExplo = (GameObject)Resources.Load("Prefabs/Explode", typeof(GameObject));
    }

    private void FixedUpdate()
    {
		if(Mathf.Abs(hero.transform.position.x - transform.position.x) > 1)
        {
            dir = (hero.transform.position.x < transform.position.x) ? -1 : 1;
            body.velocity = new Vector2(dir * 10, body.velocity.y);
        }
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            Explode();
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
            //Debug.Log(collision.name);
        if(collision.tag == "Player")
        {
            Explode();
        }else if (collision.tag == "Demage")
        {
            if (collision.name.Substring(0, 6) == "Bullet")
            {
                Debug.Log(collision.name.Substring(0, 6));
                Explode();
            }
        }
    }

    public void Explode()
    {
        Vector3 newVec = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Destroy(Instantiate(PrefExplo, newVec, Quaternion.identity), 1f);
        Destroy(gameObject);
    }
}
