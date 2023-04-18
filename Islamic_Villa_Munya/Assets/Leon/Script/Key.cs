using Unity.VisualScripting;
using UnityEngine;

public class Key : MonoBehaviour
{
    public Door doorToUnlock;
    float startY;
    bool triggered = false;
    public AudioClip pickupAudio;
    AudioSource pickupAudioSource;
    private void Start()
    {
        startY = transform.position.y;

        pickupAudioSource = transform.AddComponent<AudioSource>();
        pickupAudioSource.clip = pickupAudio;
        pickupAudioSource.spatialBlend = 1f;
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
        gameObject.SetActive(false);

        //Destroy(gameObject, 2f);

        /*Cal's code ends here*/
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, 60 * Time.deltaTime, Space.World);
        transform.position = new Vector3(transform.position.x, startY + Mathf.Sin(Time.time * 1.5f) * 0.15f, transform.position.z);
    }

    /*Cal's code starts here*/

    /*Cal's code ends here*/
}
