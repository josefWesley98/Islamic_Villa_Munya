using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Audio;

public class TriggerAudio : MonoBehaviour
{
    //generic script to trigger audio when trigger entered
    AudioMixer mixer;
    // sound clip array to choose from
    public AudioClip[] sounds;
    //list to hold all the audio sources which the clips will be added to
    List<AudioSource> sources = new List<AudioSource>();
    public float volume = 0.25f;
    public bool onceOnly = false;
    bool canPlay = true;

    void Start()
    {
        //get the mixer
        mixer = Resources.Load("NewAudioMixer") as AudioMixer;

        // for every clip, set up an audio source on this object and add it to the source list and mixer
        for (int i = 0; i < sounds.Count(); i++)
        {
            AudioSource a = transform.AddComponent<AudioSource>();
            a.clip = sounds[i];
            a.spatialBlend = 1f;
            a.volume = volume;
            a.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
            sources.Add(a);
        }

    }
    //if something enters trigger, play a random sound from the collection with pitch variation
    private void OnTriggerEnter(Collider other)
    {
        if (!canPlay)
            return;
        int rng = Random.Range(0, sources.Count());
        sources[rng].pitch = (Random.Range(0.8f, 1.2f));
        sources[rng].Play();
        if (onceOnly)
            canPlay = false;
    }
    //if this is a collider and not a trigger, if collision happens play a random sound from the collection with pitch variation
    private void OnCollisionEnter(Collision collision)
    {
        if (!canPlay)
            return;
        int rng = Random.Range(0, sources.Count());
        sources[rng].pitch = (Random.Range(0.8f, 1.2f));
        sources[rng].Play();
        if (onceOnly)
            canPlay = false;
    }
}
