using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class Door : MonoBehaviour
{
    public bool TEMPDELAYDOORSETUP = true;

    public enum DoorState
    {
        Opening,
        Open,
        Closing,
        Closed
    }
    public DoorState doorState;
    [Header("Door Parameters")]
    [Range(0.1f, 10)]
    public float hingeDistanceOffset = 1f;
    [Range(-180, 180)]
    public float angleOffset = 90.0f;
    [Range(-180, 180)]
    public float angleOpen = 90.0f;
    public bool flipAngleValues = false;

    public float openSpeed = 1f;
    public float closeSpeed = 1f;
    public float openDelay = 0f;
    public float closeDelay = 0f;
    float timer = 0.0f;

    //public bool open = false;
    public bool locked = false;
    public bool unlockNextPress = false;
    public bool canCloseDoor = true;

    Vector3 doorToHingeVector = Vector3.forward;
    GameObject hinge, hingeOpenTarget, hingeClosedTarget;
    Vector3 hingePos;
    Vector3 openPos;
    Vector3 closedPos;
    bool destinationReached = false;

    Mesh mesh;
    [Header("Door Extras")]
    public Door puppetDoor;
    public UnityEvent doorReachOpen, doorReachClose;

    public Transform playerBoy, playerGirl;
    Transform actualPlayerBoy, actualPlayerGirl;
    float interactMinDistance = 3f;
    public Rigidbody lockRB;
    public AudioClip unlockAudio;
    AudioSource unlockAudioSource;

    public bool permanentlyUnlocked = false;

    public NamedDoors doorName = NamedDoors.Unnamed;
    void Start()
    {
        //temporaryPlayerReferenceDeleteLaterOk = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1);
        //store the open and close positions
        UpdatePositions();
        //on game start, make a hinge for this door object
        if (Application.isPlaying)
        {
            if (TEMPDELAYDOORSETUP)
                Invoke(nameof(CreateDoorParent), 10f);
            else
                Invoke(nameof(CreateDoorParent), 1f);
            //CreateDoorParent();
            //print("ok");
            //CreateDoorParent();
        }

        /*Cal's script starts here*/
        // if(GameManager.GetDoorUnlocked())
        // {
        //     doorState = DoorState.Open;
        //     Debug.Log("open");
        //     Destroy(gameObject);
        // }
        // else
        // {
        //     Debug.Log("Closed");
        //     doorState = DoorState.Closed;
        // }
        /*Cal's script ends here*/
    }

    void Update()
    {
        //dont do door stuff anymore if perma unlocked
        //if (permanentlyUnlocked)
            //return;

        //failsafe
        if (unlockNextPress && (actualPlayerBoy == null && actualPlayerGirl == null))
        {
            //Destroy(gameObject);

            /*Cal's script starts here*/
            if(!GameManager.GetDoorUnlocked(doorName))// if players not found and door not unlocked disable door
                gameObject.SetActive(false);
            /*Cal's script ends here*/
        }

        //print(Vector3.Distance(temporaryPlayerReferenceDeleteLaterOk.position, closedPos));
        //run only in edit mode
        if (!Application.isPlaying)
        {
            UpdatePositions();
            mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
        }
        else if (hinge != null)
        {
            if (Input.GetKeyDown(KeyCode.F) && ((Vector3.Distance(actualPlayerBoy.position, closedPos) < interactMinDistance) || (Vector3.Distance(actualPlayerGirl.position, closedPos) < interactMinDistance)))
            {
                //print("in range");
                if (unlockNextPress && locked)
                {
                    //print("toggling open");
                    //play unlock sound
                    unlockAudioSource.Play();
                    if(lockRB != null)
                    {
                        lockRB.transform.GetChild(0).transform.GetComponent<ParticleSystem>().Play();
                        lockRB.transform.parent = null;
                        lockRB.isKinematic = false;
                        lockRB.AddForce(lockRB.transform.right * 2.5f);
                    }


                    ToggleOpen();
                    locked = false;
                    unlockNextPress = false;
                    //return;
                }
                if (!locked)
                {
                    ToggleOpen();
                    GameManager.SetDoorUnlocked(doorName, true);
                }
            }

            //if (locked)
            //    open = false;

            //Door State Machine
            switch (doorState)
            {
                case DoorState.Open:
                    timer = 0;
                    destinationReached = false;
                    break;
                case DoorState.Closed:
                    timer = 0;
                    destinationReached = false;
                    break;
                case DoorState.Closing:

                    timer += Time.deltaTime;

                    if(timer > closeDelay || locked)
                        MoveDoor(false);

                    if (destinationReached)
                    {
                        doorReachClose.Invoke();                
                        //print("door closed");
                        doorState = DoorState.Closed;
                    }
                    break;
                case DoorState.Opening:

                    timer += Time.deltaTime;

                    if (timer > openDelay)
                        MoveDoor(true);

                    if (destinationReached)
                    {
                        doorReachOpen.Invoke();
                        //print("door open");
                        doorState = DoorState.Open;
                    }
                    break;
            }

            //if (locked)
            //    doorState = DoorState.Closing;
            //print(Vector3.Angle(hinge.transform.forward, hingeOpenTarget.transform.forward));

            if (puppetDoor != null)
            {
                puppetDoor.doorState = doorState;
                puppetDoor.locked = locked;
            }
        }
    }
    void UpdatePositions()
    {
        doorToHingeVector = AddAngleToVector(angleOffset, transform.forward);
        closedPos = transform.position;
        hingePos = closedPos + doorToHingeVector * hingeDistanceOffset;
        openPos = hingePos - AddAngleToVector(angleOpen, doorToHingeVector) * hingeDistanceOffset;
    }

    Vector3 AddAngleToVector(float angle, Vector3 vector)
    {
        return Quaternion.Euler(0, angle, 0) * vector;
    }

    //move the door to its open or not open position
    void MoveDoor(bool openDoor = true)
    {
        //stop moving the door if at destination angle
        if (destinationReached)// && open != lastDestWasOpen)
            return;
        //set appropriate values based on if door is to be open or closed
        Vector3 targetDirection = openDoor ? hingeOpenTarget.transform.forward : hingeClosedTarget.transform.forward;

        float speed = openDoor ? openSpeed : closeSpeed;
        // step size is equal to speed times frame time.
        float singleStep = speed * Time.deltaTime;
        // rotate forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(hinge.transform.forward, targetDirection, singleStep, 0.0f);
        // Calculate a rotation a step closer to the target and applies rotation to this object
        hinge.transform.rotation = Quaternion.LookRotation(newDirection);

        destinationReached = CheckAngleReached(hinge.transform.forward, targetDirection);

        //hinge.transform.rotation = Quaternion.LookRotation(hingePos - openPos) * Quaternion.Euler(0, -angleOffset, 0);
        //if(open)
        //  hinge.transform.rotation = null
    }

    //return true when angle between vectors in near zero
    bool CheckAngleReached(Vector3 from, Vector3 to)
    {
        //print(Vector3.Angle(from, to));
        return Vector3.Angle(from, to) < 1f;
    }

    //spawns an empty gameobject to serve as the door hinge point and parents this object (door mesh) to it
    void CreateDoorParent()
    {
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

        transform.parent = hinge.transform;

        unlockAudioSource = transform.AddComponent<AudioSource>();
        unlockAudioSource.clip = unlockAudio;
        unlockAudioSource.spatialBlend = 1f;

        actualPlayerBoy = playerBoy.GetChild(0);
        actualPlayerGirl = playerGirl.GetChild(0);

        if (!permanentlyUnlocked && GameManager.GetDoorUnlocked(doorName))
        {
            ToggleOpen();
            locked = false;
            unlockNextPress = false;
            permanentlyUnlocked = true;
            lockRB.gameObject.SetActive(false);
        }
    }

    //editor helpers
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
        Gizmos.DrawMesh(mesh, openPos, Quaternion.LookRotation(hingePos - openPos) * Quaternion.Euler(0, -angleOffset, 0), transform.localScale);
    }
    private void OnValidate()
    {
        if (flipAngleValues)
        {
            angleOffset *= -1;
            angleOpen *= -1;
            flipAngleValues = false;
        }
    }
    public void ToggleOpen()
    {
        if(doorState == DoorState.Closed)
            doorState = DoorState.Opening;
        if (doorState == DoorState.Open && canCloseDoor)
            doorState = DoorState.Closing;
        //open = !open;
        //return open;
    }
    public bool ToggleLocked()
    {
        locked = !locked;
        return locked;
    }

    /*Cal's script starts here*/
    public void SetDoorActive(bool val)
    {
        //gameObject.SetActive(true);
    }
    /*Cal's script ends here*/
}
