using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioArea : MonoBehaviour {

    public AudioClip audioClip;
    private GameObject audioSource;

    private void Start()
    {
        audioSource = transform.parent.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            audioSource.GetComponent<ControllerAudio>().changeAudio(audioClip);
        }
    }
}
