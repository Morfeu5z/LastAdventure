using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour {

    public float zoomIn;
    public float zoomOut;
    public float speedIn;
    public float speedOut;
    ControllerCamera cam;

	// Use this for initialization
	void Start () {
        cam = GameObject.Find("MainCamera").GetComponent<ControllerCamera>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            cam.Focus(gameObject.transform.GetChild(0).gameObject, zoomIn, speedIn);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        cam.PlayerFocus(zoomOut, speedOut);
    }
}
