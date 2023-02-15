using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    //Global variables
    public float turn_speed = 4f;
    public float sensitivity = 0.8f;
    public Transform player;

    private Vector2 offset;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        offset.x += Input.GetAxis("Mouse X") * sensitivity;
        offset.y = Input.GetAxis("Mouse Y") * sensitivity;
        transform.localRotation = Quaternion.Euler(-offset.y, offset.x, 0);
    }
}
