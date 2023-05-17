using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Audio;

//the three types of floor for footstep sounds
public enum FloorType
{
    Carpet,
    Mud,
    Tile
}

public class PlayerAudio : MonoBehaviour
{
    //this script controls all the player related movement sounds

    AudioMixer mixer;
    //array of all tile step clips, list of sound sources and volumes
    public AudioClip[] stepTileClips;
    List<AudioSource> stepTileSources = new List<AudioSource>();
    public float stepTileVolume = 0.25f;

//array of all carpet step clips, list of sound sources and volumes
    public AudioClip[] stepCarpetClips;
    List<AudioSource> stepCarpetSources = new List<AudioSource>();
    public float stepCarpetVolume = 0.25f;

//array of all mud step clips, list of sound sources and volumes
    public AudioClip[] stepMudClips;
    List<AudioSource> stepMudSources = new List<AudioSource>();
    public float stepMudVolume = 0.25f;

//array of all jump clips, list of sound sources and volumes
    public AudioClip[] jumpClips;
    List<AudioSource> jumpSources = new List<AudioSource>();
    public float jumpVolume = 0.25f;

    bool jumping = false;

//array of all landing clips, list of sound sources and volumes
    public AudioClip[] landClips;
    List<AudioSource> landSources = new List<AudioSource>();
    public float landVolume = 0.25f;

    bool landed = false;

//array of all climbing clips, list of sound sources and volumes
    public AudioClip[]  climbClips;
    List<AudioSource> climbSources = new List<AudioSource>();
    public float climbVolume = 0.25f;

//plug in for the player objects
    public Transform playerBoy, playerGirl;
    Transform actualPlayer;
    Transform actualPlayerPosition;
//the player movement script
    NIThirdPersonController pController;
//the player climbing script
    UpwardClimbing upClimbingScript;

//the timings for the run and walk sounds to play
    public float footstepTimeWalk = 0.3f;
    public float footstepTimeRun = 0.35f;

    float footStepTime;
    float footstepTimer = 0f;

    //master array of all footstep sound lists. for switching between floor types
    List<AudioSource>[] footstepMaster = new List<AudioSource>[3];

    public LayerMask groundLayers;

    public List<string> carpetMaterials = new List<string>();
    GameObject previousFloor;

    //default floor type
    FloorType floor = FloorType.Tile;

    bool setupComplete = false;

    //funtction for detecting which player is used and grabbingthe corresponding scripts to check
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

    //after a short delay find up the player then set up the script
    void Start()
    {
        Invoke(nameof(FindPlayer), 1f);
        Invoke(nameof(SetUp), 1f);
    }

    private void FixedUpdate()
    {
        if (!setupComplete)
            return;

        //teleport the sound object to the players location
        transform.position = actualPlayerPosition.position;

        //print(pController.GetJumping());

        if (pController.GetJumping())
        {
            landed = false;

            if(!jumping)
            {
                //trigger a jump sound if the player is jumping
                Jump();

                jumping = true;
            }
        }
        else
        {
            jumping = false;

            if (!landed)
            {
                //trigger a land sound if the player is landing
                Land();
                landed = true;
            }

        }
        
        //raycast to the floor to detect the floor type
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down * 0.1f, out hit, Mathf.Infinity, groundLayers))
        {
            //if (gameObject == previousFloor)
            //    return;

            Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.yellow);

            if (hit.collider is TerrainCollider)//assume terrain requiers the mud footstep sounds
            {
                floor = FloorType.Mud;
            }
            else if (hit.transform.gameObject.GetComponent<MeshRenderer>().material.name == carpetMaterials[0]) //if the hit material is the carpet mat, carpet sounds
            {
                floor = FloorType.Carpet;
            }
            else if (hit.transform.gameObject.tag == "DoNotCollide") // rugs are on the DoNotCollide tag so use carpet sounds
            {
                floor = FloorType.Carpet;
            }
            else //otherwise just use the tile sounds for footsteps
                floor = FloorType.Tile;

            previousFloor = gameObject;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!setupComplete)
         return;

        //read the move input from the player script
        bool playerStepping = pController.GetPlayerInput() != Vector2.zero;
        //set the footstep timer based on if we are running
        footStepTime = pController.GetRunning() ? footstepTimeRun : footstepTimeWalk;
        //count up the timer
        footstepTimer += Time.deltaTime;

        //if timer is past the footstep time and player is moving and not jumping and not climbing and not impacting ground,
        // play random footstep sound with random pitch variation from the current footstep library (mud, tile or carpet)
        if (footstepTimer > footStepTime && playerStepping && !jumping && !pController.GetIsClimbing() && !pController.GetHardLanding())
        {
            //"floor" is the current floor type
            int rng = Random.Range(0, stepTileClips.Count());
            footstepMaster[Convert.ToInt32(floor)][rng].pitch = (Random.Range(0.8f, 1.2f));
            footstepMaster[Convert.ToInt32(floor)][rng].Play();
            footstepTimer = 0f;
            //print(rng);
        }

        //trigger the climbing sound if the climbing script requests it
        if (upClimbingScript.GetDoAudio())
        {
            Climb();
            upClimbingScript.SetDoAudio(false);
        }
    }

    void SetUp()
    {
        //load up the audio mixer
        mixer = Resources.Load("NewAudioMixer") as AudioMixer;

        //assign the footsteps to the master
        footstepMaster[0] = stepCarpetSources;
        footstepMaster[1] = stepMudSources;
        footstepMaster[2] = stepTileSources;


        //every for loop below adds an audio source with each clip from the array, sets the appropriate values and adds to the appropriate list and mixer.
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

        for (int i = 0; i < stepMudClips.Count(); i++)
        {
            AudioSource a = transform.AddComponent<AudioSource>();
            a.clip = stepMudClips[i];
            a.spatialBlend = 1f;
            a.volume = stepMudVolume;
            a.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
            stepMudSources.Add(a);
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
        //random jump sound and pitch
        int rng = Random.Range(0, jumpClips.Count());
        jumpSources[rng].pitch = (Random.Range(0.8f, 1.2f));
        jumpSources[rng].Play();
    }

    void Land()
    {
        //random landing sound and pitch
        if (pController.GetIsClimbing())
            return;

        int rng = Random.Range(0, landClips.Count());
        landSources[rng].pitch = (Random.Range(0.8f, 1.2f));
        landSources[rng].Play();
    }

    void Climb()
    {
        //random climbing sound and pitch
        int rng = Random.Range(0, climbClips.Count());
        climbSources[rng].pitch = (Random.Range(0.5f, 1.5f));
        climbSources[rng].Play();
    }
}
