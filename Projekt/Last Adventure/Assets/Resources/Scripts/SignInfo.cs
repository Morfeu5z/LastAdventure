using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignInfo : MonoBehaviour {

    public string Message = "";
    public string Img = "";
    private Config config;

	// Use this for initialization
	void Start () {
        config = GameObject.Find("Config").GetComponent<Config>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            config.MessageBox(true, Message, "Znak", "Sprites/Items/Elements/" + Img);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        config.MessageBox(false);
    }
}
