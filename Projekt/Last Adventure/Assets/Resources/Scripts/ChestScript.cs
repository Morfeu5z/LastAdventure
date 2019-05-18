using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour {

	public bool isEmpty = false;
    public GameObject EffectPrefab;
    private bool openChest = false;
    private bool particle = false;
    private bool playerCollision = false;

    // timer
    private float StartTime = 0;

    private void FixedUpdate()
    {
        if (Input.GetButtonDown("Submit") && playerCollision)
        {
            transform.GetComponent<Animator>().SetTrigger("OpenChest");
            createTreasure();
            GameObject.Find("Config").GetComponent<Config>().MessageBox(false);
        }

        if (openChest)
        {
            if (StartTime == 0) StartTime = Time.time;
            if (Time.time - StartTime > 0.6f && !particle)
            {
                particle = true;
                Vector3 pos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
                GameObject tmp = Instantiate(EffectPrefab, transform, false);
                tmp.transform.position = pos;
                Destroy(tmp, 5);
            }
            if (Time.time - StartTime > 4f)
            {
                StartTime = 0;
                openChest = false;
                isEmpty = true;
                transform.GetComponent<Animator>().SetBool("EmptyChest", true);
            }
        }
    }

    public void createTreasure()
    {
        openChest = true;
        transform.GetComponent<BoxCollider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.tag == "Player")
        {
            GameObject.Find("Config").GetComponent<Config>().MessageBox(true, "Otwórz skrzynię wciskając \"F\"");
            playerCollision = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isEmpty)
        {
            GameObject.Find("Config").GetComponent<Config>().MessageBox(false);
            playerCollision = true;
        }
    }
}
