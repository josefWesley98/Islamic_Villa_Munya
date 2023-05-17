using UnityEngine;
using UnityEngine.Audio;
public class AudioAmbientArea : MonoBehaviour
{
    //script for ambient audio zones
    //one AudioAmbientArea script is used per one set of triggers. A set of triggers defines an area that should use a specific ambient track.
    public float fadeSpeed = 0.5f;

    //clip to loop
    public AudioClip loopClip;
    public float volume = 1.0f;

    bool playAudio = false;

    AudioSource a;
    AudioMixer mixer;

    public int triggerCounter = 0;

    void Start()
    {
        //grab the audio mixer
        mixer = Resources.Load("NewAudioMixer") as AudioMixer;
        //set various values
        a = gameObject.AddComponent<AudioSource>();
        a.clip = loopClip;
        a.spatialBlend = 0f;
        a.volume = volume;
        //add this sound to the mixer
        a.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        a.loop = true;
        a.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //if player is in this ambient area, interpolate volume towards target volume
        if (triggerCounter > 0)
        {
            a.volume = Mathf.Lerp(a.volume, volume, Time.deltaTime * fadeSpeed);
        }
        //if player is not in this ambient area, interpolate volume towards muted (zero)
        else
        {
            a.volume = Mathf.Lerp(a.volume, 0, Time.deltaTime * fadeSpeed);
        }
    }

    private void OnTriggerExit(Collider c)
    {
        if (c.tag == "Player" && !c.isTrigger)
        {
            //decrement the amount of triggers the player is in
            triggerCounter--;
        }
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player" && !c.isTrigger)
        {
            //increment a number instead of setting a bool, in case the player is in overlapping triggers
            triggerCounter++;
        }
    }
}