using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnHead : MonoBehaviour {

    public float PositionIdleX = 0.078f;
    public float PositionIdleY = 0.715f;

    public float PositionEquipX = -0.017f;
    public float PositionEquipY = 0.545f;

    public float PositionCrouchX = 0.09f;
    public float PositionCrouchY = 0.538f;

    private ControllerPlayer hero;
    private Config config;
    private bool IdleOff = false;


	// Use this for initialization
	void Start () {
        hero = GameObject.Find("Hero").GetComponent<ControllerPlayer>();
        config = GameObject.Find("Config").GetComponent<Config>();
	}
	
	// Update is called once per frame
	void Update () {
        if (hero.equipActive)
        {
            IdleOff = true;
            transform.localPosition = new Vector3(PositionEquipX, PositionEquipY, 0);
            gameObject.GetComponent<Animator>().SetBool("Stop", true);
        }
        else if (hero.crouch)
        {
            IdleOff = true;
            transform.localPosition = new Vector3(PositionCrouchX, PositionCrouchY, 0);
            gameObject.GetComponent<Animator>().SetBool("Stop", true);
        }
        else if (IdleOff)
        {
            IdleOff = false;
            transform.localPosition = new Vector3(PositionIdleX, PositionIdleY, 0);
            gameObject.GetComponent<Animator>().SetBool("Stop", false);
        }
        if (config.LoadedData.life == 0 && config.CheckActiveItem("LifeCrystal") == false) Destroy(gameObject);
	}
}
