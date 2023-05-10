using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class TriggerTransition : MonoBehaviour
{
    //public bool transitionActivated = false;
    Animator anim;

    public ParticleSystem p, p2;

    Rigidbody rb;

    public AudioClip doorSound, portalSound;
    AudioSource doorSource, portalSource;

    AudioMixer mixer;

    bool open = false;

    public bool ignoreFirstEnter = false;
    public bool requireArtefact1 = false;

    public GameObject colliderClosed;

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

        if(ignoreFirstEnter)
            Invoke(nameof(IgnoreFix), 3f);
    }

    void IgnoreFix()
    {
        ignoreFirstEnter = false;
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player" && !c.isTrigger)
        {
            //print(c.gameObject.transform.parent.name);

            if (requireArtefact1)
            {
                if (!GameManager.GetArtefactCollected(0))
                {
                    return;
                }
            }
            if (ignoreFirstEnter)
            {
                ignoreFirstEnter = false;
                return;
            }
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

            Deactivate();
        }
    }

    void Activate()
    {
        open = true;
        p.gameObject.SetActive(open);
        p.Play();
        p2.Play();
        //Invoke("Anim", 0.2f);

        doorSource.Play();
        portalSource.Play();
        colliderClosed.SetActive(false);
        Anim();
    }

    void Deactivate()
    {
        open = false;
        p.Stop();
        p2.Stop();
        p.gameObject.SetActive(open);
        //Invoke("Anim", 0.2f);

        doorSource.Stop();
        portalSource.Stop();
        colliderClosed.SetActive(true);
        Anim();
    }

    void Anim()
    {
        anim.SetBool("Open", open);
    }
}
