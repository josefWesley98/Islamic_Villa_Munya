using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Door : MonoBehaviour
{
    enum DoorState
    {
        Closed,
        Open,
        Moving
    }

    public float hingeOffset = 1f;
    [Range(-179, 179)]
    public float angleOpen = 90.0f;
    public float openSpeed = 1f;
    public float closeSpeed = 1f;
    public bool open = false;

    bool destinationReached = false;
    GameObject doorHolder;
    Vector3 hingeLocationWorld;
    Vector3 openPos;

    void Start()
    {
        if(Application.isPlaying)
            CreateDoorParent();

        openPos = hingeLocationWorld + Quaternion.Euler(0, angleOpen, 0) * transform.right * -Vector3.Distance(hingeLocationWorld, transform.position);
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            hingeLocationWorld = transform.position + transform.right * hingeOffset;
            openPos = hingeLocationWorld + Quaternion.Euler(0, angleOpen, 0) * transform.right * -Vector3.Distance(hingeLocationWorld, transform.position);

            //stuff breaks if door not vertical so z and x are locked for now
            Vector3 rot = transform.eulerAngles;
            rot.x = rot.z = 0;
            transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z); 
        }
        else if (doorHolder != null && !destinationReached)
        {
            MoveDoor(open);
        }
    }

    void MoveDoor(bool open = true)
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = openPos - doorHolder.transform.position;

        // The step size is equal to speed times frame time.
        float singleStep = openSpeed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(doorHolder.transform.forward, targetDirection, singleStep, 0.0f);

        // Draw a ray pointing at our target in
        Debug.DrawRay(doorHolder.transform.position, newDirection, Color.red);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        doorHolder.transform.rotation = Quaternion.LookRotation(newDirection);

        if(CheckAngleReached(doorHolder.transform.forward, targetDirection))
        {
            destinationReached = true;
            return;
        }
        destinationReached = false;
    }

    //return true when angle between vectors in near zero
    bool CheckAngleReached(Vector3 from, Vector3 to)
    {
        return Vector3.Angle(from, to) < 1f;
    }

    //spawns an empty gameobject to serve as the door hinge point and parents this object to it
    void CreateDoorParent()
    {
        doorHolder = new GameObject("Door Holder");
        doorHolder.transform.parent = null;
        hingeLocationWorld = transform.position + transform.right * hingeOffset;
        doorHolder.transform.rotation = transform.rotation * Quaternion.Euler(0, -90, 0);
        doorHolder.transform.position = hingeLocationWorld;
        transform.parent = doorHolder.transform;
    }

    //editor visuals
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(hingeLocationWorld, 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(hingeLocationWorld, openPos);
        Gizmos.DrawSphere(openPos, 0.05f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, hingeLocationWorld);
        Gizmos.DrawSphere(transform.position, 0.05f);
    }
}
