using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class TriggerTransition : MonoBehaviour
{
    //This script directs everything concerning the transition doors and portal which connect the museum to the villa

    //animator that controls the doors
    Animator anim;
    //portal particles 
    public ParticleSystem p, p2;
    //sounds
    public AudioClip doorSound, portalSound;
    //sound sources
    AudioSource doorSource, portalSource;

    AudioMixer mixer;

    bool open = false;

    //the instance of the transition trigger can specify if the first artefact needs to be found (to lock the player into the puzzle 1 "tutorial")
    public bool requireArtefact1 = false;

    //invisible barrier to set active or not, to prevent unwanted portal entrance
    public GameObject colliderClosed;

    //specify a second trigger to be entered before being active. For when the player must be a certain distance from the doors for them to open initially
    //this mainly just improves visuals
    public bool requireSecondTrigger = false;
    bool secondTriggerSprung = false;

    void Start()
    {
        //get mixer from file
        mixer = Resources.Load("NewAudioMixer") as AudioMixer;
        //door audio setup
        doorSource = gameObject.AddComponent<AudioSource>();
        doorSource.clip = doorSound;
        doorSource.spatialBlend = 1f;
        doorSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        //portal audio setup
        portalSource = gameObject.AddComponent<AudioSource>();
        portalSource.clip = portalSound;
        portalSource.spatialBlend = 1f;
        portalSource.loop = true;
        portalSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        //get anim and particles
        anim = GetComponent<Animator>();
        p = transform.GetChild(0).GetComponent<ParticleSystem>();
        p2 = transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player" && !c.isTrigger)
        {
            if (requireArtefact1)
            {
                if (!GameManager.GetArtefactCollected(0))
                {
                    return;
                }
            }
            if (requireSecondTrigger)
            {
                if (!secondTriggerSprung)
                    return;
            }

            //if player in trigger and conditions met, start the activation process for the transition effects
            Activate();
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.tag == "Player" && !c.isTrigger)
        {
            // if (ignoreFirstEnter)
            // {
            //     return;
            // }
            
            //close the transition if the player leaves the trigger
            Deactivate();
        }
    }

    void Activate()
    {
        //open the doors, play the particles and audio, disable invisible barrier
        open = true;
        p.gameObject.SetActive(open);
        p.Play();
        p2.Play();
        doorSource.Play();
        portalSource.Play();
        colliderClosed.SetActive(false);
        Anim();
    }

    void Deactivate()
    {
        //close the doors, cancel the particles and audio, enable invisible barrier
        open = false;
        p.Stop();
        p2.Stop();
        p.gameObject.SetActive(open);
        doorSource.Stop();
        portalSource.Stop();
        colliderClosed.SetActive(true);
        Anim();
    }

    //tell the animator to play/stop
    void Anim()
    {
        anim.SetBool("Open", open);
    }

    //function for recieving confirmation from second trigger that the player entered it. only used for a distance check to make sure player isnt nearby
    public void ActivateSecondTrigger() => secondTriggerSprung = true;
}
