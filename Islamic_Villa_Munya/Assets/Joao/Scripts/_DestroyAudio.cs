using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _DestroyAudio : MonoBehaviour
{
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            Destroy(gameObject);
            // When audio is Finished, Destroy audio prefab attached to this script.
        }
    }
}
