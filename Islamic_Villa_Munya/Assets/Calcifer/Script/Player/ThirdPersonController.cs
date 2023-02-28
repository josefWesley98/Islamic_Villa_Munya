using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    public CharacterController player_controller;
    public Transform cam;

    //Player controls - new system
    //public PlayerControls_Cal controls;

    //For RB
    public Rigidbody rb;

    public float ground_drag;
    public float player_height;
    public LayerMask is_ground;
    public bool grounded;
    bool is_walking = false;
    bool is_running = false;
    public bool is_standing_jump = false;

    public float stand_jump_force;
    public float moving_jump_force;
    public float jump_cooldown;
    public float air_multiplier;
    bool is_jump_ready;
    private bool input_disabled = false;
    private float disabled_input_delay = 1f;

    public float walk_speed;
    public float run_speed;
    public float smooth_time;
    float turn_smooth_vel;

    private Vector3 input;
    private Vector3 move_dir;
    public Transform orientation;
    public Transform ground_pos;
    Vector2 move_input;
    private float stand_jump_delay = 0f;
    InputAction movement;
    InputAction jumping;
    public PlayerControls_Cal controls;


    void Awake() 
    {
        controls = new PlayerControls_Cal();
    }
    private void Start()
    {
        is_jump_ready = true;
    }

    private void OnEnable() 
    {
        //Initialise the controls and assign to input actions
        controls.Enable();

        //Assign input actions here
        jumping = controls.Player.Jump;
        jumping.Enable();
        
        movement = controls.Player.Move;
        movement.Enable();
        
        //jumping.started += _ => Jump(true);
    }

    private void OnDisable() 
    {
        controls.Disable();
        movement.Disable();
        jumping.Disable();
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
        if(grounded && !input_disabled)
        {
            //horizontal_input = Input.GetAxisRaw("Horizontal");
            //vertical_input = Input.GetAxisRaw("Vertical");

            move_input = movement.ReadValue<Vector2>();

            bool jump_pressed = Input.GetKey(KeyCode.Space);
            bool  crouch_pressed = Input.GetKey(KeyCode.LeftControl);

            //When to jump
            if (jump_pressed && is_jump_ready && grounded && !is_standing_jump && !crouch_pressed)
            {
                is_jump_ready = false;
                
                //Check if the player is standing still
                if(rb.velocity.magnitude < 0.1f)
                {
                    is_standing_jump = true;
                    //Apply a delay before jumping
                    stand_jump_delay = 0.5f;
                    Invoke("JumpWithDelay", stand_jump_delay);
                }
                else
                {
                    //Jump without delay
                    Jump(false);
                }

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
        move_dir = orientation.forward * move_input.y + orientation.right * move_input.x;
        // Debug.Log(orientation.forward);
        // Debug.DrawRay(orientation.position, orientation.forward * 2, Color.blue);
        // Debug.DrawRay(orientation.position, orientation.right * 2, Color.red);
        // Debug.DrawRay(rb.position, move_dir, Color.green);
        //Debug.Log(rb.velocity);
        //Debug.Log(move_input);
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

    //Delay jump
    private void JumpWithDelay()
    {
        Jump(true);
    }

    //Stop the player rotating when standing jumping
    private void NoRotationDelay()
    {
        is_standing_jump = false;
    }

    //Function for jumping
    private void Jump(bool apply_delay)
    {

        if(apply_delay)
        {
            //Apply the delay before allowing the player to jump
            StartCoroutine(DisableInput(disabled_input_delay));
            rb.AddForce(transform.up * stand_jump_force, ForceMode.Impulse);
        }
        else
        {
            //Reset the player y-velocity to 0 so they player jumps to the same height every time
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            
            //Apply the force once using impulse
            rb.AddForce(transform.up * moving_jump_force, ForceMode.Impulse);
        }

        Invoke("NoRotationDelay", 1f);
    }

    private IEnumerator DisableInput(float duration)
    {
        input_disabled = true;
        yield return new WaitForSeconds(duration);
        input_disabled = false;
    }

    //Reset the ability for the player to jump here
    private void ResetJump()
    {
        is_jump_ready = true;
    }

    public Vector3 GetRBVelocity(Vector3 vel)
    {
        vel = rb.velocity;
        //Debug.Log(vel);
        return vel;
    }

    public bool GetGrounded()
    {
        return grounded;
    }

    public bool GetIsStandingJump()
    {
        return is_standing_jump;
    }

    public Vector2 GetPlayerInput()
    {
        return move_input;
    }
}
