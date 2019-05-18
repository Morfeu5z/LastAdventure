using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinScript : MonoBehaviour {

    public float speed       = 0.1f;
    private float speedcpy   = 0.1f;
    private float stopScroll = 0.1f;
    GameObject Credits;
    GameObject Title;
    bool Scroll = false;
    bool Load   = false;


	// Use this for initialization
	void Start () {
        Credits     = GameObject.Find("Canvas/Credits");
        Title       = GameObject.Find("Canvas/Title");
        speedcpy    = speed;
        stopScroll  = (Credits.transform.position.y * -1) / 1.5f;
    }
	
	// Update is called once per frame
	void Update () {
        if (Scroll)
        {
            Credits.transform.position = new Vector3(Credits.transform.position.x, Credits.transform.position.y + speed, 0);
            Title.transform.position = new Vector3(Title.transform.position.x, Title.transform.position.y + speed, 0);
            if (Credits.transform.position.y > stopScroll) ReloadScene();
        }

        if (Input.GetButton("Accept")) speed = speedcpy * 10;
        else speed = speedcpy;
    }

    // Call when animation end
    public void Scrolling()
    {
        Scroll = true;
    }

    public void ReloadScene()
    {
        if (Load == false)
        {
            Load = true;
            GameObject.Find("Config").GetComponent<Config>().Fin = false;
            GameObject.Find("Config").GetComponent<Config>().Esc = true;
            GameObject.Find("Canvas/LoadScreen").GetComponent<Animator>().SetBool("LoadOn", true);
            GameObject.Find("SoundTrack").GetComponent<ControllerAudio>().Turn("off");
        }
    }
}
