using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicMixer : MonoBehaviour
{
    public AudioMixerSnapshot Area1;
    public AudioMixerSnapshot Area2;
    //public AudioMixerSnapshot Area3;

    private void OnTriggerEnter(Collider other)   //compares Gameobject tag that collided with the player
    {
        if (other.CompareTag("Area1"))  
        {
            Debug.Log("Area1");
            Area1.TransitionTo(5);          //transitions to Audio for Area 1 in a space of 5 seconds
        }

        if (other.CompareTag("Area2"))
        {
            Debug.Log("Area2");
            Area2.TransitionTo(2);          //transitions to Audio for Area 2 in a space of 2 seconds
        }

    }
}