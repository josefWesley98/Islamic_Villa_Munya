using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class Door : MonoBehaviour
{
    [Header("Door Parameters")]
    [Range(0.1f, 10)]
    public float hingeDistanceOffset = 1f;
    [Range(-180, 180)]
    public float angleOffset = 90.0f;
    [Range(-180, 180)]
    public float angleOpen = 90.0f;

    public float openSpeed = 1f;
    public float closeSpeed = 1f;

    public bool open = false;
    public bool locked = false;

    Vector3 doorToHingeVector = Vector3.forward;
    GameObject hinge, hingeOpenTarget, hingeClosedTarget;
    Vector3 hingePos;
    Vector3 openPos;
    Vector3 closedPos;

    Mesh mesh;
    [Header("Door Extras")]
    public Door puppetDoor;
    public UnityEvent doorReachOpen, doorReachClose;


    void Start()
    {
        //store the open and close positions
        UpdatePositions();
        //on game start, make a hinge for this door object
        if (Application.isPlaying)
        {
            CreateDoorParent();
        }
    }

    void Update()
    {
        //run only in edit mode
        if (!Application.isPlaying)
        {
            UpdatePositions();
            mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
            //stuff breaks if door not kept vertical so z and x are locked for now
            Vector3 rot = transform.eulerAngles;
            rot.x = rot.z = 0;
            transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
        }
        else if (hinge != null)
        {
            if (Input.GetKeyDown(KeyCode.F))//TEMPORARY
            {
                ToggleOpen();
            }

            if (locked)
                open = false;

            MoveDoor(open);

            if (puppetDoor != null)
            {
                puppetDoor.open = open;
                puppetDoor.locked = locked;
            }
        }// && !destinationReached)
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
    void MoveDoor(bool open = true)
    {
        //set appropriate values based on if door is to be open or closed
        Vector3 targetDirection = open ? hingeOpenTarget.transform.forward : hingeClosedTarget.transform.forward;

        float speed = open ? openSpeed : closeSpeed;
        // step size is equal to speed times frame time.
        float singleStep = speed * Time.deltaTime;
        // rotate forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(hinge.transform.forward, targetDirection, singleStep, 0.0f);
        // Calculate a rotation a step closer to the target and applies rotation to this object
        hinge.transform.rotation = Quaternion.LookRotation(newDirection);

        //stop moving the door if at destination angle
        //if(CheckAngleReached(doorHolder.transform.forward, targetDirection))        
        //destinationReached = true;

        //hinge.transform.rotation = Quaternion.LookRotation(hingePos - openPos) * Quaternion.Euler(0, -angleOffset, 0);
        //if(open)
        //  hinge.transform.rotation = null
    }

    //return true when angle between vectors in near zero
    bool CheckAngleReached(Vector3 from, Vector3 to)
    {
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
    }

    //editor helpers
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

    private void OnDrawGizmos()
    {
        //Open Preview
        Color c = new Color(0, 100, 200, 0.3f);
        Gizmos.color = c;
        Gizmos.DrawMesh(mesh, openPos, Quaternion.LookRotation(hingePos - openPos) * Quaternion.Euler(0, -angleOffset, 0), transform.localScale);
    }

    public bool ToggleOpen()
    {
        open = !open;
        return open;
    }
    public bool ToggleLocked()
    {
        locked = !locked;
        return locked;
    }
}
