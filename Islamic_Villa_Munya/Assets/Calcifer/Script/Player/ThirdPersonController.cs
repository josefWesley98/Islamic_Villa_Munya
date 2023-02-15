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
    public bool grounded;
    bool is_walking = false;
    bool is_running = false;

    public float jump_force;
    public float jump_cooldown;
    public float air_multiplier;
    bool is_jump_ready;

    public float walk_speed = 1f;
    public float run_speed  =3f;
    public float smooth_time = 0.2f;
    float turn_smooth_vel;

    private Vector3 input;
    private Vector3 move_dir;
    public Transform orientation;
    public Transform ground_pos;
    float horizontal_input;
    float vertical_input;

    private void Start()
    {
        is_jump_ready = true;
    }
    // Update is called once per frame
    void Update()
    {

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

        //Check to see if the player is on the ground
        //grounded = Physics.Raycast(transform.position, Vector3.down, player_height * 0.5f + 0.1f, is_ground);
        bool floor = Physics.CheckSphere(ground_pos.position, 0.2f, is_ground);

        if(floor)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        //Handle drag
        if (grounded)
        {
            //If the player speed is above the maximum, call this function
            ControlPlayerVel();

            PlayerInput();
            rb.drag = ground_drag;
        }
        else
        {
            rb.drag = 0f;
        }
    }

    private void FixedUpdate()
    {
        //Call player movement
        PlayerMovement();
    }

    private void PlayerInput()
    {
        if(grounded)
        {
            horizontal_input = Input.GetAxisRaw("Horizontal");
            vertical_input = Input.GetAxisRaw("Vertical");

            bool jump_pressed = Input.GetKey(KeyCode.Space);

            //When to jump
            if (jump_pressed && is_jump_ready && grounded)
            {
                is_jump_ready = false;
                Jump();

                Invoke(nameof(ResetJump), jump_cooldown);
            }
        }
    }

    private void PlayerMovement()
    {
        //Capture the user key input
        //Determine speed depending on whether run key is pressed
        bool run_pressed = Input.GetKey(KeyCode.LeftShift);

        // calculate movement direction
        move_dir = orientation.forward * vertical_input + orientation.right * horizontal_input;

        if (grounded)
        {
            //If the player is walking
            if (!run_pressed)
            {
                is_running = false;
                is_walking = true;

                rb.AddForce(move_dir.normalized * walk_speed * 10, ForceMode.Force);
            }

            //If the player is running, increase the speed
            if (run_pressed)
            {
                is_walking = false;
                is_running = true;

                rb.AddForce(move_dir.normalized * run_speed * 10, ForceMode.Force);
            }
        }
        else if (!grounded)
        {
            rb.AddForce(move_dir.normalized * walk_speed * 10f * air_multiplier, ForceMode.Force);
        }
    }

    private void ControlPlayerVel()
    {
        Vector3 current_vel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //Limit the velocity if above the threshold 
        if(is_walking && !is_running && current_vel.magnitude > walk_speed)
        {
            Vector3 limit_vel = current_vel.normalized * walk_speed;
            rb.velocity = new Vector3(limit_vel.x, rb.velocity.y, limit_vel.z);
        }
        if (is_running && !is_walking && current_vel.magnitude > run_speed)
        {
            Vector3 limit_vel = current_vel.normalized * run_speed;
            rb.velocity = new Vector3(limit_vel.x, rb.velocity.y, limit_vel.z);
        }
    }

    //Function for jumping
    private void Jump()
    {
        //Reset the player y-velocity to 0 so they player jumps to the same height every time
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //Apply the force once using impulse
        rb.AddForce(transform.up * jump_force, ForceMode.Impulse);
    }

    //Reset the ability for the player to jump here
    private void ResetJump()
    {
        is_jump_ready = true;
    }
}
