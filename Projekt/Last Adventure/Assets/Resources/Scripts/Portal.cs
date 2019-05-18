using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour {

    public int BuildMapIndex;
    public int PortalIndex;
    private Config config;

    private void Start()
    {
        config = GameObject.Find("Config").GetComponent<Config>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Hero")
        {
            config.portal       = true;
            config.portalIndex  = PortalIndex;
            config.portal       = true;
            config.portalBuild  = BuildMapIndex;
            config.SceneReload();
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        config.MessageBox(false);
    }
}
