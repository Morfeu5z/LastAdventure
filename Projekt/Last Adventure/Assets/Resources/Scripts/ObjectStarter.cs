using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStarter : MonoBehaviour {

    public string ActiveEvent = "";

	// Use this for initialization
	void Start () {
        if (GameObject.Find("Config").GetComponent<Config>().CheckExistActivetedEvent(ActiveEvent)) transform.GetComponent<Animator>().SetBool("ok", true);
    }
    private void OnEnable()
    {
        if (GameObject.Find("Config").GetComponent<Config>().CheckExistActivetedEvent(ActiveEvent)) transform.GetComponent<Animator>().SetBool("ok", true);
    }
}
