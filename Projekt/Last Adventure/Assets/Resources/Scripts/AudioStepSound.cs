using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioStepSound : MonoBehaviour {

    public AudioClip StepSound;
    private GameObject hero;

	// Use this for initialization
	void Start () {
        hero = GameObject.Find("Hero");
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            hero.GetComponent<ControllerPlayer>().stepSound(StepSound);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            hero.GetComponent<ControllerPlayer>().stepSoundOriginal();
        }
    }
}
