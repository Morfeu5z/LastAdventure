using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDemage : MonoBehaviour {

    public string IfItemIsActive = "";
    private ControllerPlayer hero;
    private Config config;

	// Use this for initialization
	void Start () {
        hero    = GameObject.Find("Hero").GetComponent<ControllerPlayer>();
        config  = GameObject.Find("Config").GetComponent<Config>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(IfItemIsActive != "")
            {
                if (!config.CheckActiveItem(IfItemIsActive)){
                    hero.SetDemaged();
                }
            }
            else
            {
                    hero.SetDemaged();
            }
        }
    }
}
