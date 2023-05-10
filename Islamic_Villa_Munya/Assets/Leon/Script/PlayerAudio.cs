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

    public AudioClip[] jumpClips;
    List<AudioSource> jumpSources = new List<AudioSource>();
    public float jumpVolume = 0.25f;

    bool jumping = false;

    public AudioClip[] landClips;
    List<AudioSource> landSources = new List<AudioSource>();
    public float landVolume = 0.25f;

    bool landed = false;

    public AudioClip[]  climbClips;
    List<AudioSource> climbSources = new List<AudioSource>();
    public float climbVolume = 0.25f;

    public Transform playerBoy, playerGirl;
    Transform actualPlayer;
    Transform actualPlayerPosition;
    NIThirdPersonController pController;

    UpwardClimbing upClimbingScript;

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

       actualPlayerPosition = actualPlayer.GetChild(0);
        // = actualPlayer.GetChild(0).position;

        pController = actualPlayer.GetChild(0).GetComponentInChildren<NIThirdPersonController>();

        upClimbingScript = actualPlayer.GetComponentInChildren<UpwardClimbing>();
    }

    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).gameObject;
        Invoke(nameof(FindPlayer), 1f);
        Invoke(nameof(SetUp), 1f);
    }

    private void FixedUpdate()
    {
        if (!setupComplete)
            return;

        transform.position = actualPlayerPosition.position;

        //print(pController.GetJumping());

        if (pController.GetJumping())
        {
            landed = false;

            if(!jumping)
            {
                Jump();

                jumping = true;
            }
        }
        else
        {
            jumping = false;

            if (!landed)
            {
                Land();
                landed = true;
            }

        }
        
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, Vector3.down * 0.1f, out hit, Mathf.Infinity, groundLayers))
        {
            //if (gameObject == previousFloor)
            //    return;

            Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.yellow);
            //Debug.Log("Did Hit");

            //print(hit.transform.gameObject.GetComponent<MeshRenderer>().material.name);

            onTile =
            hit.transform.gameObject.GetComponent<MeshRenderer>().material.name != carpetMaterials[0] && hit.transform.gameObject.tag != "DoNotCollide";
            //hit.transform.gameObject.GetComponent<MeshRenderer>().material.name != carpetMaterials[1] &&
            //hit.transform.gameObject.GetComponent<MeshRenderer>().material.name != carpetMaterials[2] &&
            //hit.transform.gameObject.GetComponent<MeshRenderer>().material.name != carpetMaterials[3] &&
            //hit.transform.gameObject.GetComponent<MeshRenderer>().material.name != carpetMaterials[4] &&
            //hit.transform.gameObject.GetComponent<MeshRenderer>().material.name != carpetMaterials[5]
            //;


            previousFloor = gameObject;
        }
        else
        {
            //Debug.Log("Did not Hit");
            Debug.DrawRay(transform.position, Vector3.down * 1000, Color.white);
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

        if (footstepTimer > footStepTime && playerStepping && !jumping && !pController.GetIsClimbing() && !pController.GetHardLanding())
        {
            int rng = Random.Range(0, stepCarpetClips.Count());
            footstepMaster[Convert.ToInt32(onTile)][rng].pitch = (Random.Range(0.8f, 1.2f));
            footstepMaster[Convert.ToInt32(onTile)][rng].Play();
            footstepTimer = 0f;
            //print(rng);
        }


        if (upClimbingScript.GetDoAudio())
        {
            Climb();
            upClimbingScript.SetDoAudio(false);
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

        for (int i = 0; i < jumpClips.Count(); i++)
        {
            AudioSource a = transform.AddComponent<AudioSource>();
            a.clip = jumpClips[i];
            a.spatialBlend = 1f;
            a.volume = jumpVolume;
            a.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
            jumpSources.Add(a);
        }        
        
        for (int i = 0; i < landClips.Count(); i++)
        {
            AudioSource a = transform.AddComponent<AudioSource>();
            a.clip = landClips[i];
            a.spatialBlend = 1f;
            a.volume = landVolume;
            a.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
            landSources.Add(a);
        }

        for (int i = 0; i < climbClips.Count(); i++)
        {
            AudioSource a = transform.AddComponent<AudioSource>();
            a.clip = climbClips[i];
            a.spatialBlend = 1f;
            a.volume = climbVolume;
            a.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
            climbSources.Add(a);
        }

        setupComplete = true;
    }

    void Jump()
    {
        int rng = Random.Range(0, jumpClips.Count());
        jumpSources[rng].pitch = (Random.Range(0.8f, 1.2f));
        jumpSources[rng].Play();
    }

    void Land()
    {
        if (pController.GetIsClimbing())
            return;

        int rng = Random.Range(0, landClips.Count());
        landSources[rng].pitch = (Random.Range(0.8f, 1.2f));
        landSources[rng].Play();
    }

    void Climb()
    {
        int rng = Random.Range(0, climbClips.Count());
        climbSources[rng].pitch = (Random.Range(0.5f, 1.5f));
        climbSources[rng].Play();

        print("PLAYED CLIMBING SOUND");
    }

    //Leon fps counter
    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 100), (1.0f / Time.smoothDeltaTime).ToString());
    }
}
