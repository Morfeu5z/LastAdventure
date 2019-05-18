using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCanvas : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.GetComponent<Canvas>().worldCamera = GameObject.Find("MainCamera/Camera").GetComponent<Camera>();
        GameObject.Find("MainCamera").GetComponent<ControllerCamera>().UpdateBorder = true;
    }
}
