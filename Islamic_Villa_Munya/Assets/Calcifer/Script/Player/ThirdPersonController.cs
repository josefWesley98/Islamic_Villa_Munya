using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    public CharacterController player_controller;
    public Transform cam;

    public float speed = 7f;

    public float smooth_time = 0.1f;
    float turn_smooth_vel;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //get the direction
        Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;

        if(dir.magnitude >= 0.1f)
        {
            //returns angle from x axis and a vector starting at 0 and ending at x,y
            float target_angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target_angle, ref turn_smooth_vel, smooth_time);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 move_dir = Quaternion.Euler(0f, target_angle, 0f) * Vector3.forward;
            player_controller.Move(move_dir.normalized * speed * Time.deltaTime);
        }
    }
}
