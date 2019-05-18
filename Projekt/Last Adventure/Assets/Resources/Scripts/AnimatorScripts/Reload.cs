using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reload : MonoBehaviour {

    private Config config;

    private void Start()
    {
        config = GameObject.Find("Config").GetComponent<Config>();
        config.LinkRefresh();
    }

    public void ReloadScene()
    {
        if (config.Fin){
            config.portal = false;
            SceneManager.LoadScene(config.FinScene);
        }
        else if (config.Esc){
            config.portal = false;
            config.Esc = false;
            SceneManager.LoadScene(0);
        }
        else if (config.portal){
            if (config.portalBuild == 0) config.portal = false;
            SceneManager.LoadScene(config.portalBuild);
        }
        else SceneManager.LoadScene(config.LoadedData.scene);
    }
}
