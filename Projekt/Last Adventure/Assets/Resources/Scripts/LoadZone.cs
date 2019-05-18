using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadZone : MonoBehaviour {

    public string Zone;
    private GameObject RealZone;

	// Use this for initialization
	void Start () {
        RealZone = GameObject.Find(Zone);
        RealZone.SetActive(false);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            RealZone.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
            if(collision.tag == "Player") RealZone.SetActive(false);
    }
}
