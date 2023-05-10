using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioAmbientArea : MonoBehaviour
{
    public float fadeSpeed = 0.5f;

    AudioClip loopClip;
    public float volume = 1.0f;

    bool playAudio = false;

    AudioSource a;
    AudioMixer mixer;

    public int triggerCounter = 0;

    void Start()
    {
        loopClip = Resources.Load("amb_villa 1") as AudioClip;
        mixer = Resources.Load("NewAudioMixer") as AudioMixer;
        a = gameObject.AddComponent<AudioSource>();
        a.clip = loopClip;
        a.spatialBlend = 0f;
        a.volume = volume;
        a.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        a.loop = true;
        a.volume = 0;
        a.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (triggerCounter > 0)
        {
            a.volume = Mathf.Lerp(a.volume, volume, Time.deltaTime * fadeSpeed);
        }
        else
        {
            a.volume = Mathf.Lerp(a.volume, 0, Time.deltaTime * fadeSpeed);
        }
    }

    private void OnTriggerExit(Collider c)
    {
        if (c.tag == "Player" && !c.isTrigger)
        {
            triggerCounter--;
            //playAudio = false;
        }
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player" && !c.isTrigger)
        {
            triggerCounter++;
            //playAudio = true;
        }
    }
}