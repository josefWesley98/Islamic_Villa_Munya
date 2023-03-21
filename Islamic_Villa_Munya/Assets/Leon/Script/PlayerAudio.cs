using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip[] stepTileClips;
    List<AudioSource> stepTileSources = new List<AudioSource>();

    public float stepTimeVolume = 0.25f;

    public AudioClip[] stepCarpetClips;
    List<AudioSource> stepCarpetSources = new List<AudioSource>();

    GameObject player;
    NIThirdPersonController pController;

    public float footstepTimeWalk = 0.3f;
    public float footstepTimeRun = 0.35f;

    float footStepTime;
    float footstepTimer = 0f;

    //master array of all footstepsound lists
    List<AudioSource>[] footstepMaster = new List<AudioSource>[2];

    public LayerMask groundLayers;

    public List<string> carpetMaterials = new List<string>();
    GameObject previousFloor;

    bool onTile = true;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        transform.parent = player.transform;
        transform.position = player.transform.position;

        pController = player.GetComponentInChildren<NIThirdPersonController>();

        footstepMaster[0] = stepCarpetSources;
        footstepMaster[1] = stepTileSources;

        for (int i = 0; i < stepTileClips.Count(); i++)
        {
            AudioSource a = transform.AddComponent<AudioSource>();
            a.clip = stepTileClips[i];
            a.spatialBlend = 1f;
            a.volume = stepTimeVolume;
            stepTileSources.Add(a);
        }

        for (int i = 0; i < stepCarpetClips.Count(); i++)
        {
            AudioSource a = transform.AddComponent<AudioSource>();
            a.clip = stepCarpetClips[i];
            a.spatialBlend = 1f;
            stepCarpetSources.Add(a);
        }

    }
    private void FixedUpdate()
    {

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayers))
        {
            //if (gameObject == previousFloor)
            //    return;

            Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.yellow);
            //Debug.Log("Did Hit");


            print(hit.transform.gameObject.GetComponent<MeshRenderer>().material.name);

            onTile = !(hit.transform.gameObject.GetComponent<MeshRenderer>().material.name == carpetMaterials[0]);

            previousFloor = gameObject;
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.down * 1000, Color.white);
            //Debug.Log("Did not Hit");
        }
    }
    // Update is called once per frame
    void Update()
    {
        bool playerStepping = pController.GetPlayerInput() != Vector2.zero;
        //print(pController.GetRunning());

        footStepTime = pController.GetRunning() ? footstepTimeRun : footstepTimeWalk;

        footstepTimer += Time.deltaTime;

        if (footstepTimer > footStepTime && playerStepping)
        {
            int rng = Random.Range(0, stepCarpetClips.Count());
            footstepMaster[Convert.ToInt32(onTile)][rng].pitch = (Random.Range(0.8f, 1.2f));
            footstepMaster[Convert.ToInt32(onTile)][rng].Play();
            footstepTimer = 0f;
            //print(rng);
        }
    }
}
