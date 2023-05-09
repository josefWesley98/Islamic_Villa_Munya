using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioPush : MonoBehaviour
{
    Rigidbody rb;
    public float speedMinimum = 0.5f;

    public AudioClip loopClip;
    public float volume = 1.0f;
    public float fadeSpeed = 10f;
    AudioSource a;
    AudioMixer mixer;
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        mixer = Resources.Load("NewAudioMixer") as AudioMixer;
        a = gameObject.AddComponent<AudioSource>();
        a.clip = loopClip;
        a.spatialBlend = 1f;
        a.volume = volume;
        a.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        a.loop = true;
        a.volume = 0;
        a.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.magnitude > speedMinimum)
        {
            a.volume = Mathf.Lerp(a.volume, volume, Time.deltaTime * fadeSpeed);
        }
        else
        {
            a.volume = Mathf.Lerp(a.volume, 0, Time.deltaTime * fadeSpeed);
        }

    }
}