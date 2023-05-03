using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Audio;
public class PlayerAudio : MonoBehaviour
{
    AudioMixer mixer;
    public AudioClip[] stepTileClips;
    List<AudioSource> stepTileSources = new List<AudioSource>();
    public float stepTileVolume = 0.25f;


    public AudioClip[] stepCarpetClips;
    List<AudioSource> stepCarpetSources = new List<AudioSource>();
    public float stepCarpetVolume = 0.25f;

    public Transform playerBoy, playerGirl;
    Transform actualPlayer;
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

    bool setupComplete = false;
    void FindPlayer()
    {
        if (playerBoy.gameObject.activeSelf)
            actualPlayer = playerBoy;
        else if (playerGirl.gameObject.activeSelf)
            actualPlayer = playerGirl;
        else
            Debug.LogError("No player assigned for player audio!");

        transform.parent = actualPlayer.GetChild(0);
        transform.position = actualPlayer.GetChild(0).position;

        pController = actualPlayer.GetChild(0).GetComponentInChildren<NIThirdPersonController>();
    }

    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).gameObject;
        Invoke(nameof(FindPlayer), 2f);
        Invoke(nameof(SetUp), 2f);
    }

    private void FixedUpdate()
    {
        if (!setupComplete)
         return;
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayers))
        {
            //if (gameObject == previousFloor)
            //    return;

            Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.yellow);
            //Debug.Log("Did Hit");


            //print(hit.transform.gameObject.GetComponent<MeshRenderer>().material.name);

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
        if (!setupComplete)
         return;

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

    void SetUp()
    {
        mixer = Resources.Load("NewAudioMixer") as AudioMixer;

        footstepMaster[0] = stepCarpetSources;
        footstepMaster[1] = stepTileSources;

        for (int i = 0; i < stepTileClips.Count(); i++)
        {
            AudioSource a = transform.AddComponent<AudioSource>();
            a.clip = stepTileClips[i];
            a.spatialBlend = 1f;
            a.volume = stepTileVolume;
            a.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
            stepTileSources.Add(a);
        }

        for (int i = 0; i < stepCarpetClips.Count(); i++)
        {
            AudioSource a = transform.AddComponent<AudioSource>();
            a.clip = stepCarpetClips[i];
            a.spatialBlend = 1f;
            a.volume = stepCarpetVolume;
            a.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
            stepCarpetSources.Add(a);
        }

        setupComplete = true;
    }
}
