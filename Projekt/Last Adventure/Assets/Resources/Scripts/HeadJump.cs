using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadJump : MonoBehaviour {

    public float JumpForce = 2;
    private GameObject audioPuh;

	// Use this for initialization
	void Start () {
        audioPuh = (GameObject)Resources.Load("Audio/Effects/Prefab/Prefab_AudioSlimeJump", typeof(GameObject));
}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ResetSpeed()
    {
        //
    }

    public void HeadJumpEffect()
    {
        Destroy(Instantiate(audioPuh, transform, false), 0.5f);
        GameObject.Find("MainCamera/Camera").GetComponent<Animator>().SetTrigger("shake");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<ControllerPlayer>().playerCollision(gameObject, JumpForce);
            HeadJumpEffect();
        }
    }
}
