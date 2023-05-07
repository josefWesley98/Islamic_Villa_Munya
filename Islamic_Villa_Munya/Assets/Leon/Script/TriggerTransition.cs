using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class TriggerTransition : MonoBehaviour
{
    public bool transitionActivated = false;
    Animator anim;

    ParticleSystem p, p2;

    Rigidbody rb;

    public AudioClip doorSound, portalSound;
    AudioSource doorSource, portalSource;

    AudioMixer mixer;
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
            transitionActivated = true;

            rb = c.GetComponentInChildren<Rigidbody>();
        }
    }

    void Update()
    {
        if (rb != null)
            rb.AddForce(Vector3.forward * 1000);
    }

    void Activate()
    {
        if(transitionActivated)
            return;

        p.Play();
        p2.Play();
        Invoke("Anim", 0.2f);

        doorSource.Play();
    }

    void Anim()
    {
        anim.SetTrigger("StartAnim");
    }
}
