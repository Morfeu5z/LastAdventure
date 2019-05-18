using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDistance : MonoBehaviour {

    public float distance = 20;
    private float max;
    private GameObject hero;
    private AudioSource source;
    private float x = 0;
    // Use this for initialization
    void Start () {
        max = GameObject.Find("Config").GetComponent<Config>().config.EffectVolume;
        hero = GameObject.Find("Hero");
        source = transform.GetComponent<AudioSource>();
        x = max / 10;
        source.volume = 0;
    }

    private void FixedUpdate()
    {
        if(Mathf.Abs(hero.transform.position.x - transform.position.x) < distance + 1)
        {
            source.volume = (distance - Mathf.Abs(hero.transform.position.x - transform.position.x)) * x;
        }
    }
}
