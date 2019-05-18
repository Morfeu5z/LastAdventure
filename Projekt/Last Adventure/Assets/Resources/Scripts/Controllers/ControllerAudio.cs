using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerAudio : MonoBehaviour {

    private AudioSource audioSrc;
    public float Volume = 0.6f;
    public float Jump = 0.005f;
    private bool nextaudioSrc = false;
    private AudioClip newClip;

	// Use this for initialization
	void Start () {
        audioSrc = transform.GetComponent<AudioSource>();
        audioSrc.volume = 0;
        Volume = GameObject.Find("Config").GetComponent<Config>().config.SoundtrackVolume;
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if (Volume < -0.01) Volume = -0.01f;
        else if (Volume > 1.01) Volume = 1.01f;

        if (Math.Abs(audioSrc.volume - Volume) > 0.001)
        {
            if (audioSrc.volume < Volume) audioSrc.volume += Jump;
            else if (audioSrc.volume > Volume) audioSrc.volume -= Jump;
        }

        // audioSrc change
        if (nextaudioSrc)
        {
            if(audioSrc.volume < 0.01)
            {
                audioSrc.volume = 0;
                audioSrc.clip = newClip;
                nextaudioSrc = false;
                Turn("on");
                if(audioSrc.isPlaying == false) audioSrc.Play();
            }
        }
	}

    public void Turn(string switcher = "on")
    {
        if(switcher == "off") Volume = 0;
        else Volume = GameObject.Find("Config").GetComponent<Config>().config.SoundtrackVolume;
    }

    public void changeAudio(AudioClip clip)
    {
        if(clip != audioSrc.clip)
        {
            newClip = clip;
            Turn("off");
            nextaudioSrc = true;
        }
    }
}
