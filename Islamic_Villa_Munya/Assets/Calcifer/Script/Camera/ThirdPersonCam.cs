using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    public Transform orientation;
    public Transform player;
    public Transform player_obj;
    public Rigidbody rb;
    public ThirdPersonController player_controller;

    public float ground_rot_speed;
    public float air_rot_speed;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        //Rotate orientation of camera
        Vector3 view_dir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = view_dir.normalized;

        //Rotate the player
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 input_dir = orientation.forward * vertical + orientation.right * horizontal;

        if (input_dir != Vector3.zero && player_controller.GetIsStandingJump() == false)
        {
            if (player_controller.GetGrounded() == false)
            {
                player_obj.forward = Vector3.Slerp(player_obj.forward, input_dir.normalized, Time.deltaTime * air_rot_speed);
            }
            else if (player_controller.GetGrounded() == true)
            {
                player_obj.forward = Vector3.Slerp(player_obj.forward, input_dir.normalized, Time.deltaTime * ground_rot_speed);
            }
        }

    }

}