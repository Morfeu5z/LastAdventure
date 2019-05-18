using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerHand : MonoBehaviour {

    private ControllerPlayer hero;
    // Use this for initialization

    void Start () {
        hero = GameObject.Find("Hero").GetComponent<ControllerPlayer>();
    }
	
	// Update is called once per frame
	void Update () {
        if (hero.grounded) GetComponent<Animator>().SetBool("Jump", false);
        else  GetComponent<Animator>().SetBool("Jump", true);

        if (hero.move != 0) GetComponent<Animator>().SetBool("Run", true);
        else GetComponent<Animator>().SetBool("Run", false);

        if (hero.equipActive) GetComponent<Animator>().SetBool("Equip", true);
        else GetComponent<Animator>().SetBool("Equip", false);

        if (Input.GetButton("Crouch")) GetComponent<Animator>().SetBool("Crouch", true);
        else GetComponent<Animator>().SetBool("Crouch", false);
    }
}
