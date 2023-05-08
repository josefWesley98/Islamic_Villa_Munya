using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class Key : MonoBehaviour
{
    public Door doorToUnlock;
    float startY;
    bool triggered = false;
    public AudioClip pickupAudio;
    AudioSource pickupAudioSource;

    AudioMixer mixer;
    private void Start()
    {
        if(GameManager.GetHaveKey())
        {
            Destroy(gameObject);
        }
        
        startY = transform.position.y;

        pickupAudioSource = transform.AddComponent<AudioSource>();
        pickupAudioSource.clip = pickupAudio;
        pickupAudioSource.spatialBlend = 1f;

        mixer = Resources.Load("NewAudioMixer") as AudioMixer;
        pickupAudioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;
        GameManager.SetHasKey(true);
        doorToUnlock.unlockNextPress = true;
        //Destroy(lockRB);

        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        transform.GetChild(1).transform.GetComponent<ParticleSystem>().Play();
        triggered = true;
        pickupAudioSource.Play();

        /*Cal's code starts here*/

        //Disable the key and collider
        //gameObject.SetActive(false);

        Destroy(gameObject, 1f);

        /*Cal's code ends here*/
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, 60 * Time.deltaTime, Space.World);
        transform.position = new Vector3(transform.position.x, startY + Mathf.Sin(Time.time * 1.5f) * 0.15f, transform.position.z);
    }

    /*Cal's code starts here*/
    public void SetkeyActive(bool val)
    {
        gameObject.SetActive(val);
    }
    /*Cal's code ends here*/
}
