using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class NewDoor : MonoBehaviour
{
    [Range(0.1f, 10)]
    public float hingeDistanceOffset = 1f;
    [Range(-180, 180)]
    public float angleOffset = 90.0f;
    [Range(-180, 180)]
    public float angleOpen = 90.0f;

    Vector3 doorToHingeVector = Vector3.forward;

    public float openSpeed = 1f;
    public float closeSpeed = 1f;
    public bool open = false;
    public bool locked = false;
    //public bool destinationReached = false;
    //door holder is an empty object that serves as the hinge which this gameobject gets parented to
    GameObject hinge;
    Vector3 hingePos;
    Vector3 openPos;
    Vector3 closedPos;

    Mesh mesh;
    //public UnityEvent doorOpen;
    public NewDoor syncroniseWithDoor;
    void Start()
    {
        //store the open and close positions
        UpdatePositions();
        //on game start, make a hinge for this door object
        if (Application.isPlaying)
            CreateDoorParent();
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
        //update the door position if not at destination
        else if (hinge != null)// && !destinationReached)
            MoveDoor(open);


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
        Vector3 targetDirection = (open ? openPos : closedPos) - hinge.transform.position;

        float speed = open ? openSpeed : closeSpeed;
        // step size is equal to speed times frame time.
        float singleStep = speed * Time.deltaTime;


        // rotate forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(hinge.transform.forward, targetDirection, singleStep, 0.0f);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        //hinge.transform.rotation = Quaternion.LookRotation(newDirection);

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
        hinge = new GameObject("Door Holder");
        hinge.transform.parent = null;
        hinge.transform.position = hingePos;
        hinge.transform.rotation = transform.rotation;

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
        Gizmos.DrawLine(transform.position, hingePos);
        Gizmos.DrawSphere(transform.position, 0.05f);
        Handles.Label(transform.position + Vector3.up * 0.2f, "Closed");
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
}
