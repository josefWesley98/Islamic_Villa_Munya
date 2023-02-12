using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    public CharacterController player_controller;
    public Transform cam;

    public float walk_speed = 1f;
    public float run_speed  =3f;
    public float smooth_time = 0.15f;
    float turn_smooth_vel;

    // Update is called once per frame
    void Update()
    {

        //Make cursor disappear while playing game
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        //Capture the user key input
        //Determine speed depending on whether run key is pressed
        bool run_pressed = Input.GetKey(KeyCode.LeftShift);

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //Get the direction
        Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;

        if(dir.magnitude >= 0.1f)
        {
            //Returns angle from x axis and a vector starting at 0 and ending at x,y
            float target_angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target_angle, ref turn_smooth_vel, smooth_time);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //If the player is walking
            if(!run_pressed)
            {
                Vector3 move_dir = Quaternion.Euler(0f, target_angle, 0f) * Vector3.forward;
                player_controller.Move(move_dir.normalized * walk_speed * Time.deltaTime);
            }

            //If the player is running, increase the speed
            if(run_pressed)
            {
                Vector3 move_dir = Quaternion.Euler(0f, target_angle, 0f) * Vector3.forward;
                player_controller.Move(move_dir.normalized * run_speed * Time.deltaTime);
            }
        }
    }
}
