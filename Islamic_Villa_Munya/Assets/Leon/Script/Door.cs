using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class Door : MonoBehaviour
{
    [Range(0.1f, 20)]
    public float hingeOffset = 1f;
    [Range(-179, 179)]
    public float angleOpen = 90.0f;
    public float offsetRot = 0f;
    public float openSpeed = 1f;
    public float closeSpeed = 1f;
    public bool open = false;
    //public bool destinationReached = false;
    //door holder is an empty object that serves as the hinge which this gameobject gets parented to
    GameObject doorHolder;
    Vector3 hingeLocationWorld;
    Vector3 openPos;
    Vector3 closedPos;

    //public UnityEvent doorOpen;

    void Start()
    {
        //on game start, make a hinge for this door
        if(Application.isPlaying)
            CreateDoorParent();
        //store the open and close positions
        openPos = hingeLocationWorld + Quaternion.Euler(0, angleOpen + offsetRot, 0) * transform.right * -Vector3.Distance(hingeLocationWorld, transform.position);
        closedPos = hingeLocationWorld + Quaternion.Euler(0, 0, 0) * transform.right * -Vector3.Distance(hingeLocationWorld, transform.position);
    }

    void Update()
    {
        //run only in edit mode
        if (!Application.isPlaying)
        {
            //update the position of the hinge and open position in real time in edit mode
            hingeLocationWorld = transform.position + transform.right * hingeOffset;
            openPos = hingeLocationWorld + Quaternion.Euler(0, angleOpen + offsetRot, 0) * transform.right * -Vector3.Distance(hingeLocationWorld, transform.position);

            //stuff breaks if door not kept vertical so z and x are locked for now
            Vector3 rot = transform.eulerAngles;
            rot.x = rot.z = 0;
            transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z); 
        }
        //update the door position if not at destination
        else if (doorHolder != null)// && !destinationReached)
            MoveDoor(open);
    }

    //move the door to its open or not open position
    void MoveDoor(bool open = true)
    {
        //set appropriate values based on open state
        Vector3 targetDirection = (open? openPos : closedPos) - doorHolder.transform.position;
        float speed = open ? openSpeed : closeSpeed;

        // The step size is equal to speed times frame time.
        float singleStep = speed * Time.deltaTime;
        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(doorHolder.transform.forward, targetDirection, singleStep, 0.0f);
        // Calculate a rotation a step closer to the target and applies rotation to this object
        doorHolder.transform.rotation = Quaternion.LookRotation(newDirection);

        //stop moving the door if at destination angle
        //if(CheckAngleReached(doorHolder.transform.forward, targetDirection))        
            //destinationReached = true;
    }

    //return true when angle between vectors in near zero
    bool CheckAngleReached(Vector3 from, Vector3 to)
    {
        return Vector3.Angle(from, to) < 1f;
    }

    //spawns an empty gameobject to serve as the door hinge point and parents this object (door mesh) to it
    void CreateDoorParent()
    {
        doorHolder = new GameObject("Door Holder");
        doorHolder.transform.parent = null;
        hingeLocationWorld = transform.position + transform.right * hingeOffset;
        doorHolder.transform.rotation = transform.rotation * Quaternion.Euler(0, -90, 0);
        doorHolder.transform.position = hingeLocationWorld;
        transform.parent = doorHolder.transform;
    }

    //editor helpers
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(hingeLocationWorld, 0.2f);
        Handles.Label(hingeLocationWorld + Vector3.up * 0.3f, "Hinge");
        Gizmos.color = Color.green;
        Gizmos.DrawLine(hingeLocationWorld, openPos);
        Gizmos.DrawSphere(openPos, 0.05f);
        Handles.Label(openPos + Vector3.up * 0.2f, "Open");
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, hingeLocationWorld);
        Gizmos.DrawSphere(transform.position, 0.05f);
        Handles.Label(transform.position + Vector3.up * 0.2f, "Closed");
    }
}
