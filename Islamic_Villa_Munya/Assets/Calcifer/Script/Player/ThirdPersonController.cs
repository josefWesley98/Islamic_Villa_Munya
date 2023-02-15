using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    public CharacterController player_controller;
    public Transform cam;

    //For RB
    public Rigidbody rb;

    public float ground_drag;
    public float player_height;
    public LayerMask is_ground;
    bool grounded;

    public float walk_speed = 1f;
    public float run_speed  =3f;
    public float smooth_time = 0.2f;
    float turn_smooth_vel;

    // Update is called once per frame
    void Update()
    {
        //grounded = Physics.Raycast(transform.position, Vector3.down, player_height * 0.5f + 0.2f, is_ground);


        ////RB code
        //Vector3 view_dir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        //orientation.forward = view_dir.normalized;

        ////Rotate player
        //float horizontal = Input.GetAxisRaw("Horizontal");
        //float vertical = Input.GetAxisRaw("Vertical");

        //Vector3 input_dir = orientation.forward * vertical + orientation.right * horizontal;

        //if(input_dir != Vector3.zero)
        //{
        //    player_obj.forward = Vector3.Slerp(player_obj.forward, input_dir.normalized, Time.deltaTime * smooth_time);
        //}

        //rb.AddForce(input_dir.normalized * walk_speed * 10f, ForceMode.Force);

        //Make cursor disappear while playing game
        if (Input.GetKeyDown(KeyCode.Escape))
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

        if (dir.magnitude >= 0.1f)
        {
            //Returns angle from x axis and a vector starting at 0 and ending at x,y
            float target_angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target_angle, ref turn_smooth_vel, smooth_time);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //If the player is walking
            if (!run_pressed)
            {
                Vector3 move_dir = Quaternion.Euler(0f, target_angle, 0f) * Vector3.forward;
                //player_controller.Move(move_dir.normalized * walk_speed * Time.deltaTime);
                rb.AddForce(move_dir.normalized * walk_speed * 10, ForceMode.Force);
            }

            //If the player is running, increase the speed
            if (run_pressed)
            {
                Vector3 move_dir = Quaternion.Euler(0f, target_angle, 0f) * Vector3.forward;
                //player_controller.Move(move_dir.normalized * run_speed * Time.deltaTime);
                rb.AddForce(move_dir.normalized * run_speed * 10, ForceMode.Force);
            }

            if (grounded)
            {
                rb.drag = ground_drag;
            }
            else
            {
                rb.drag = 0f;
            }
        }
    }
}
