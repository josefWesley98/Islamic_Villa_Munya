using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;

[ExecuteInEditMode]
public class Door : MonoBehaviour
{
    //Script for the projects doors. Used for the Puzzle 1 door, and the Museum entrance doors.

    //this bool activates a workaround for the door not setting up correctly on scene load
    public bool TEMPDELAYDOORSETUP = true;

    //the various states of the door. Opening and Closing are transitional states that lead to the open or closed state
    public enum DoorState
    {
        Opening,
        Open,
        Closing,
        Closed
    }
    public DoorState doorState;
    //the public parameters that user can change for the door
    [Header("Door Parameters")]
    [Range(0.1f, 10)]
    public float hingeDistanceOffset = 1f; //hinge distance from centre of door
    [Range(-180, 180)]
    public float angleOffset = 90.0f; // set a custom angle for the door to start at
    [Range(-180, 180)]
    public float angleOpen = 90.0f; //the door open angle
    public bool flipAngleValues = false;  //bool for flipping the door angle values. good for quickly creating a flipped version of a door

    public float openSpeed = 1f; //speed door takes to open
    public float closeSpeed = 1f; //speed door takes to close
    public float openDelay = 0f; //delay until door opens
    public float closeDelay = 0f;//delay until door closes
    float timer = 0.0f;


    public bool locked = false; //locked state
    public bool unlockNextPress = false; //used by corresponding door keys to make the door unlock on the next press
    public bool canCloseDoor = true; //specify if the door cant be closed

    Vector3 doorToHingeVector = Vector3.forward; //internally door rotations are done between vectors
    GameObject hinge, hingeOpenTarget, hingeClosedTarget; //behind the scenes objects which are used to rotate between.
    Vector3 hingePos;//position the hinge will spawn at
    Vector3 openPos;//the doors destination when open
    Vector3 closedPos;// the doors destination when closed
    bool destinationReached = false; // flag for door reaching its target position

    Mesh mesh; // the mesh of this door

    //other door values
    [Header("Door Extras")]
    public Door puppetDoor; //door to puppeteer, if this door is to control another.
    public UnityEvent doorReachOpen, doorReachClose; // events in case other scripts need functions to fire when door reaches a state

    public Transform playerBoy, playerGirl; // the player refernces 
    Transform actualPlayerBoy, actualPlayerGirl; // the actual player objects
    float interactMinDistance = 3f; // minimum distance to open door
    public Rigidbody lockRB; // rigidbody of physical lock object should 
    public AudioClip unlockAudio; //audio to play when unlocking
    public AudioClip openAudio; //audio to play when opening
    AudioSource unlockAudioSource, openAudioSource; //audio source for the audio clips
    public AudioMixer mixer;
    public bool permanentlyUnlocked = false; //sepcify if this door will be unlcoked forever. changed during runtime usually

    public NamedDoors doorName = NamedDoors.Unnamed; //the unique identifier for this door

    void Start()
    {
        //calculate the open and close positions of the door based on angle provided
        UpdatePositions();

        //on game start, make a hinge for this door object
        if (Application.isPlaying)
        {
            //workaround adding a delay to the door set up, as door would break if tried to set up immediately on scene load. does not affect gameplay
            if (TEMPDELAYDOORSETUP)
                Invoke(nameof(CreateDoorParent), 10f);
            else
                Invoke(nameof(CreateDoorParent), 1f);

            //CreateDoorParent();
        }
    }

    void Update()
    {
        //failsafe for if the door needs to be unlocked and player hasnt been set up properly
        if (unlockNextPress && (actualPlayerBoy == null && actualPlayerGirl == null))
        {
            Destroy(gameObject);
        }

        //if in editor mode, calculate door positions and grab the door mesh from this object
        if (!Application.isPlaying)
        {
            UpdatePositions();
            mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
        }
        else if (hinge != null) //if in play mode/game running and the hinge object exists
        {
            //if player in range and interact key pressed
            if (Input.GetKeyDown(KeyCode.F) && ((Vector3.Distance(actualPlayerBoy.position, closedPos) < interactMinDistance) || (Vector3.Distance(actualPlayerGirl.position, closedPos) < interactMinDistance)))
            {
                //if can unlock this key press and door is locked
                if (unlockNextPress && locked)
                {
                    //play unlock sound
                    unlockAudioSource.Play();

                    //if the lock object exists, make it fall to the ground
                    if(lockRB != null)
                    {
                        lockRB.transform.GetChild(0).transform.GetComponent<ParticleSystem>().Play();
                        lockRB.transform.parent = null;
                        lockRB.isKinematic = false;
                        lockRB.AddForce(lockRB.transform.right * 2.5f);
                    }

                    //toggle door open state
                    ToggleOpen();
                    locked = false;
                    unlockNextPress = false;
                }
                //if door not locked
                if (!locked)
                {
                    //toggle door open
                    ToggleOpen();
                    //set state of door in game manager
                    GameManager.SetDoorUnlocked(doorName, true);
                }
            }

            //Door State Machine
            switch (doorState)
            {
                case DoorState.Open:
                //reset timer and destination reached
                    timer = 0;
                    destinationReached = false;
                    break;
                case DoorState.Closed:
                //reset timer and destination reached
                    timer = 0;
                    destinationReached = false;
                    break;
                case DoorState.Closing:
                //increment start delay timer
                    timer += Time.deltaTime;
                    //if time reached or locked, close the door
                    if(timer > closeDelay || locked)
                        MoveDoor(false);
                    //if at destination
                    if (destinationReached)
                    {
                        //fire the door closed event
                        doorReachClose.Invoke();                
                        //set state closed
                        doorState = DoorState.Closed;
                    }
                    break;
                case DoorState.Opening:
                //increment start delay timer
                    timer += Time.deltaTime;
                    //if time reached, open the door
                    if (timer > openDelay)
                        MoveDoor(true);
                    //if at destination
                    if (destinationReached)
                    {
                        //fire the door open event
                        doorReachOpen.Invoke();
                        //set state open
                        doorState = DoorState.Open;
                    }
                    break;
            }

            //if a door to puppeteer is specified, override the values of that door with this door's
            if (puppetDoor != null)
            {
                puppetDoor.doorState = doorState;
                puppetDoor.locked = locked;
            }
        }
    }
    //function for calculating the positions needed for the door
    void UpdatePositions()
    {
        doorToHingeVector = AddAngleToVector(angleOffset, transform.forward);
        closedPos = transform.position;
        hingePos = closedPos + doorToHingeVector * hingeDistanceOffset;
        openPos = hingePos - AddAngleToVector(angleOpen, doorToHingeVector) * hingeDistanceOffset;
    }

    //function for returning a vector with an angle added to it
    Vector3 AddAngleToVector(float angle, Vector3 vector)
    {
        return Quaternion.Euler(0, angle, 0) * vector;
    }

    //function for moving the door to its open or closed position
    void MoveDoor(bool openDoor = true)
    {
        //stop moving the door if at destination angle
        if (destinationReached)
            return;
        //set appropriate values based on if door is to be open or closed
        Vector3 targetDirection = openDoor ? hingeOpenTarget.transform.forward : hingeClosedTarget.transform.forward;
        //choose speed based on if poening or closing
        float speed = openDoor ? openSpeed : closeSpeed;
        //make step size equal to speed times frame time.
        float singleStep = speed * Time.deltaTime;
        // rotate forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(hinge.transform.forward, targetDirection, singleStep, 0.0f);
        // Calculate a rotation a step closer to the target and applies rotation to this object
        hinge.transform.rotation = Quaternion.LookRotation(newDirection);
        //check if we are at destination angle
        destinationReached = CheckAngleReached(hinge.transform.forward, targetDirection);
    }

    //function that returns true when angle between vectors is near zero. used for checking if door at destination
    bool CheckAngleReached(Vector3 from, Vector3 to)
    {
;
        return Vector3.Angle(from, to) < 1f;
    }

    //Function that does all the automatic setup for the door
    void CreateDoorParent()
    {
        //spawns an empty gameobject to serve as the door hinge point and parents this object (door mesh) to it
        hinge = new GameObject(gameObject.name + " Hinge"); // The actual hinge
        hingeClosedTarget = new GameObject(gameObject.name + " Hinge Target Closed"); // reference point for closed position
        hinge.transform.position = hingeClosedTarget.transform.position = hingePos;
        //hinge should face door for easier rotations
        hinge.transform.rotation = hingeClosedTarget.transform.rotation = Quaternion.LookRotation(closedPos - hinge.transform.position);
        hingeClosedTarget.hideFlags = HideFlags.HideInHierarchy;

        //open position
        hingeOpenTarget = new GameObject(gameObject.name + " Hinge Target Open");
        hingeOpenTarget.transform.position = hingePos;
        hingeOpenTarget.transform.rotation = Quaternion.LookRotation(openPos - hinge.transform.position);
        hingeOpenTarget.hideFlags = HideFlags.HideInHierarchy;
        //attach this door mesh object to the new hinge to rotate around
        transform.parent = hinge.transform;

        //set up door audio
        unlockAudioSource = transform.AddComponent<AudioSource>();
        unlockAudioSource.clip = unlockAudio;
        unlockAudioSource.spatialBlend = 1f;
        //find audio mixer and apply
        mixer = Resources.Load("NewAudioMixer") as AudioMixer;
        unlockAudioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        //more door sfx
        openAudioSource = transform.AddComponent<AudioSource>();
        openAudioSource.clip = openAudio;
        openAudioSource.spatialBlend = 1f;
        openAudioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        //get the real player object
        actualPlayerBoy = playerBoy.GetChild(0);
        actualPlayerGirl = playerGirl.GetChild(0);
        //if this door is perma unlocked in the door manager, set it to be open forever.
        if (!permanentlyUnlocked && GameManager.GetDoorUnlocked(doorName))
        {
            ToggleOpen();
            locked = false;
            unlockNextPress = false;
            permanentlyUnlocked = true;
            lockRB.gameObject.SetActive(false);
        }
    }

    //editor visualisation of door positions and rotations, ditances etc
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        //Hinge Pos
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(hingePos, 0.2f);
        Handles.Label(hingePos + Vector3.up * 0.3f, "Hinge");
        //Open Pos
        Gizmos.color = Color.green;
        Gizmos.DrawLine(hingePos, openPos);
        Gizmos.DrawSphere(openPos, 0.05f);
        Handles.Label(openPos + Vector3.up * 0.2f, "Open");
        //Closed Pos
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(closedPos, hingePos);
        Gizmos.DrawSphere(closedPos, 0.05f);
        Handles.Label(closedPos + Vector3.up * 0.2f, "Closed");
        //Open Preview
        Color c = new Color(0, 100, 200, 0.5f);
        Gizmos.color = c;
        Gizmos.DrawWireMesh(mesh, openPos, Quaternion.LookRotation(hingePos - openPos) * Quaternion.Euler(0, -angleOffset, 0), transform.localScale);
    }
#endif
    private void OnDrawGizmos()
    {
        //Open Preview
        Color c = new Color(0, 100, 200, 0.3f);
        Gizmos.color = c;
        //render a semi transparent visual of the door at its destination
        Gizmos.DrawMesh(mesh, openPos, Quaternion.LookRotation(hingePos - openPos) * Quaternion.Euler(0, -angleOffset, 0), transform.localScale);
    }

    //hack to use the flipAngleValues bool like a button in the inspector
    private void OnValidate()
    {
        //if button pressed
        if (flipAngleValues)
        {
            //flip the door angles
            angleOffset *= -1;
            angleOpen *= -1;
            //unpress the button
            flipAngleValues = false;
        }
    }
    //toggle the state of the door
    public void ToggleOpen()
    {
        if(doorState == DoorState.Closed)
            doorState = DoorState.Opening;
            openAudioSource.Play();
        if (doorState == DoorState.Open && canCloseDoor)
            doorState = DoorState.Closing;
        //open = !open;
        //return open;
    }
    //toggle the locked state
    public bool ToggleLocked()
    {
        locked = !locked;
        return locked;
    }
}
