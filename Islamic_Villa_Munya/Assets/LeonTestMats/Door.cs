using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Vector3 relativeHingeLocation = new Vector3(-0.5f, 0, 0);

    public float relativeAngleOpen = 90.0f;

    public float openSpeed = 1f;

    Quaternion startRot;

    GameObject doorHolder;

    void Start()
    {
        //startRot = transform.rotation;
        CreateDoorParent();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, startRot + Quaternion.Euler(0, relativeAngleOpen, 0), openSpeed * Time.deltaTime);
    }

    void CreateDoorParent()
    {
        doorHolder = new GameObject("Door Holder");
        doorHolder.transform.parent = null;
        doorHolder.transform.position = transform.position + relativeHingeLocation;
        doorHolder.transform.parent = transform;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + relativeHingeLocation, 0.2f);
    }
}
