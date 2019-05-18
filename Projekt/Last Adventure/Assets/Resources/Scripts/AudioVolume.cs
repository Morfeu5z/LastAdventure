using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVolume : MonoBehaviour {

    private void Start()
    {
        transform.GetComponent<AudioSource>().volume = GameObject.Find("Config").GetComponent<Config>().config.EffectVolume;
    }
}
