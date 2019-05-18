using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasDeath : MonoBehaviour {

    public void ReloadAfterDeathText()
    {
        GameObject.Find("Config").GetComponent<Config>().QLoad();
    }

}
