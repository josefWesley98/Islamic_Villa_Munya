using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class TriggerTransition : MonoBehaviour
{
    //public bool transitionActivated = false;
    Animator anim;

    ParticleSystem p, p2;

    Rigidbody rb;

    public AudioClip doorSound, portalSound;
    AudioSource doorSource, portalSource;

    AudioMixer mixer;

    bool open = false;

    void Start()
    {
        mixer = Resources.Load("NewAudioMixer") as AudioMixer;

        doorSource = gameObject.AddComponent<AudioSource>();
        doorSource.clip = doorSound;
        doorSource.spatialBlend = 1f;
        doorSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];

        portalSource = gameObject.AddComponent<AudioSource>();
        portalSource.clip = portalSound;
        portalSource.spatialBlend = 1f;
        portalSource.loop = true;
        portalSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];

        anim = GetComponent<Animator>();
        p = transform.GetChild(0).GetComponent<ParticleSystem>();
        p2 = transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    void OnTriggerEnter(Collider c)
    {
        //print(c);

        if (c.tag == "Player")
        {
            Activate();
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.tag == "Player")
        {
            Deactivate();
        }
    }

    void Activate()
    {
        open = true;
        p.Play();
        p2.Play();
        p.gameObject.SetActive(open);
        Invoke("Anim", 0.2f);

        doorSource.Play();
        portalSource.Play();
    }

    void Deactivate()
    {
        open = false;
        p.Stop();
        p2.Stop();
        p.gameObject.SetActive(open);
        Invoke("Anim", 0.2f);

        doorSource.Stop();
        portalSource.Stop();
        Anim();
    }

    void Anim()
    {
        anim.SetBool("Open", open);
    }
}
