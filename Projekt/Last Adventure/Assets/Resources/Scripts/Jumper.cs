using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour {

    public float force = 6.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<ControllerPlayer>().jumper(gameObject, force);
            if (transform.name != "jumperoff")
            {
                if (GetComponent<AudioSource>().isPlaying == false) GetComponent<AudioSource>().Play();
            }
        }
    }
}
