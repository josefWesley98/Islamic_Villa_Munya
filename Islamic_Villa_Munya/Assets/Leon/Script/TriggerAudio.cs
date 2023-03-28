using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class TriggerAudio : MonoBehaviour
{
    public AudioClip[] sounds;
    List<AudioSource> sources = new List<AudioSource>();
    public float volume = 0.25f;
    public bool onceOnly = false;
    bool canPlay = true;

    void Start()
    {

        for (int i = 0; i < sounds.Count(); i++)
        {
            AudioSource a = transform.AddComponent<AudioSource>();
            a.clip = sounds[i];
            a.spatialBlend = 1f;
            a.volume = volume;
            sources.Add(a);
        }

    }
    //private void FixedUpdate()
    //{

    //    RaycastHit hit;
    //    // Does the ray intersect any objects excluding the player layer
    //    if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayers))
    //    {
    //        //if (gameObject == previousFloor)
    //        //    return;

    //        Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.yellow);
    //        //Debug.Log("Did Hit");


    //        print(hit.transform.gameObject.GetComponent<MeshRenderer>().material.name);

    //        onTile = !(hit.transform.gameObject.GetComponent<MeshRenderer>().material.name == carpetMaterials[0]);

    //        previousFloor = gameObject;
    //    }
    //    else
    //    {
    //        Debug.DrawRay(transform.position, Vector3.down * 1000, Color.white);
    //        //Debug.Log("Did not Hit");
    //    }
    //}
    // Update is called once per frame
    //void Update()
    //{
    //    bool playerStepping = pController.GetPlayerInput() != Vector2.zero;
    //    //print(pController.GetRunning());

    //    footStepTime = pController.GetRunning() ? footstepTimeRun : footstepTimeWalk;

    //    footstepTimer += Time.deltaTime;

    //    if (footstepTimer > footStepTime && playerStepping)
    //    {
    //        int rng = Random.Range(0, stepCarpetClips.Count());
    //        footstepMaster[Convert.ToInt32(onTile)][rng].pitch = (Random.Range(0.8f, 1.2f));
    //        footstepMaster[Convert.ToInt32(onTile)][rng].Play();
    //        footstepTimer = 0f;
    //        //print(rng);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (!canPlay)
            return;
        int rng = Random.Range(0, sources.Count());
        sources[rng].pitch = (Random.Range(0.8f, 1.2f));
        sources[rng].Play();
        if (onceOnly)
            canPlay = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!canPlay)
            return;
        int rng = Random.Range(0, sources.Count());
        sources[rng].pitch = (Random.Range(0.8f, 1.2f));
        sources[rng].Play();
        if (onceOnly)
            canPlay = false;
    }
}
