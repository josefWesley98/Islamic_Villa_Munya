using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*Cal's script starts here */

public class NIThirdPersonController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider capsule;
    [SerializeField] private PlayerControls_Cal controls;
    Keyboard kb;
    InputAction hide_cursor;
    InputAction show_cursor;
    InputAction movement;
    InputAction jumping;
    InputAction running;
    InputAction crouching;

    [SerializeField] private Transform cam;
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform ground_pos;

    private Vector3 input;
    private Vector3 move_dir;
    private Vector2 move_input;

    [SerializeField] private LayerMask is_ground;

    private bool grounded;
    private bool is_running = false;
    private bool is_crouching = false;
    private bool is_jumping = false;
    private bool is_standing_jump = false;
    private bool input_disabled = false;

    [Header("Movement")]
    [SerializeField] private float ground_drag;
    [SerializeField] private float walk_speed;
    [SerializeField] private float run_speed;
    [Header("Jumping")]
    [SerializeField] private float stand_jump_force;
    [SerializeField] private float moving_jump_force;
    [SerializeField] private float air_multiplier;
    [Header("Capsule Properties")]
    [SerializeField] private float capsule_radius;
    [SerializeField] private float capsule_height;
    [SerializeField] private float capsule_centre;
    private float stand_jump_delay = 0.5f;
    private float disabled_input_delay = 1.7f;


    void Awake() 
    {
        //Assign the new input controller
        controls = new PlayerControls_Cal();
        kb = InputSystem.GetDevice<Keyboard>();
    }
    private void Start()
    {
        //Initialise necessary variables here for player startup

        //Set the capsule collider to variables that will change when crouching
        capsule.height = capsule_height;
        capsule.radius = capsule_radius;
        capsule.center = new Vector3(0f, capsule_centre, 0f);
    }

    private void OnEnable() 
    {
        //Initialise the controls and assign to input actions
        controls.Enable();

        //Assign input actions here
        hide_cursor = controls.Player.HideCursor;
        hide_cursor.Enable();

        show_cursor = controls.Player.ShowCursor;
        show_cursor.Enable();

        jumping = controls.Player.Jump;
        jumping.Enable();
        
        movement = controls.Player.Move;
        movement.Enable();

        running = controls.Player.ToggleRun;
        running.Enable();

        crouching = controls.Player.Crouch;
        crouching.Enable();

        //Use input actions to call necessary functions for player functionality
        hide_cursor.started += _ => HideCursor();
        show_cursor.started += _ => ShowCursor();

        jumping.started += _ => Jump();

        running.started += _ => ToggleRunning(_);
        running.canceled += _ => ToggleRunning(_);

        crouching.started += _ => ToggleCrouch(_);
        crouching.canceled += _ => ToggleCrouch(_);
    }

    private void OnDisable() 
    {
        controls.Disable();
        movement.Disable();
        jumping.Disable();
        running.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        //Function to hide cursor when playing
        HideCursor();

        //Check to see if the player is on the ground
        bool floor = Physics.CheckSphere(ground_pos.position, 0.2f, is_ground);

        //If the player is on the floor then set grounded to true and allow for movement and jumping
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
            //If the player speed is above the maximum velocity then call this function
            ControlPlayerVel();

            //Prevent moving in the air by preventing player input updates during a jump
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
            //Newer input system reading input values from the player
            move_input = movement.ReadValue<Vector2>();
        }
    }

    private void PlayerMovement()
    {
        //Calculate movement direction
        move_dir = orientation.forward * move_input.y + orientation.right * move_input.x;
        
        //Debug rays for making sure the player moves in the correct direction
        Debug.DrawRay(orientation.position, orientation.forward * 2, Color.blue);
        Debug.DrawRay(orientation.position, orientation.right * 2, Color.red);
        Debug.DrawRay(rb.position, move_dir, Color.green);

        if (grounded)
        {
            //If the player is walking
            if (!is_running)
            {
                rb.AddForce(move_dir.normalized * walk_speed * 10, ForceMode.Force);
            }

            //If the player is running
            if (is_running && !is_crouching)
            {
                rb.AddForce(move_dir.normalized * run_speed * 10, ForceMode.Force);
            }

            //If the player is trying to run while crouching
            if(is_running && is_crouching)
            {
                rb.AddForce(move_dir.normalized * walk_speed * 10, ForceMode.Force);
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
        if(current_vel.magnitude > walk_speed)
        {
            Vector3 limit_vel = current_vel.normalized * walk_speed;
            rb.velocity = new Vector3(limit_vel.x, rb.velocity.y, limit_vel.z);
        }
        if (current_vel.magnitude > run_speed)
        {
            Vector3 limit_vel = current_vel.normalized * run_speed;
            rb.velocity = new Vector3(limit_vel.x, rb.velocity.y, limit_vel.z);
        }
    }

    //Stop the player rotating when standing jumping
    private void NoRotationDelay()
    {
        is_standing_jump = false;
    }

    //Function for jumping
    private void Jump()
    {
        if(grounded)
        {
            is_jumping = true;
            bool apply_delay = false;

            //If the player magnitude is < 0.1, then apply delay for standing jump
            if(rb.velocity.magnitude < 0.1f)
            {
                apply_delay = true;
            }

            if(apply_delay)
            {
                //Apply the delay before allowing the player to jump
                StartCoroutine(DisableInput(disabled_input_delay));
                Invoke("DelayedJump", stand_jump_delay);
                Debug.Log("delaaaaaaaayed");
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
    }

    private void DelayedJump()
    {
        rb.AddForce(transform.up * stand_jump_force, ForceMode.Impulse);
    }

    private IEnumerator DisableInput(float duration)
    {
        input_disabled = true;
        yield return new WaitForSeconds(duration);
        input_disabled = false;
    }

    //Hide cursor function
    private void HideCursor()
    {
        //Hides cursor while playing
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    //Show the cursor
    private void ShowCursor()
    {
        //Shows cursor while playing
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //Function for determining whether the player wants to jump or not
    private void ToggleRunning(InputAction.CallbackContext run)
    {
        //If the player has pressed the run button in addition to moving
        if(run.started)
        {
            is_running = true;
        }
        if(run.canceled)
        {
            is_running = false;
        }
    }

    //Function for determining whether the player wants to crouch or not
    private void ToggleCrouch(InputAction.CallbackContext crouch)
    {
        if(crouch.started)
        {
            is_crouching = true;
            capsule.height = capsule_height / 2;
            capsule.center = new Vector3(0f, capsule_centre / 2, 0f);
            capsule.radius = capsule_radius * 2;
        }
        if(crouch.canceled)
        {
            is_crouching = false;
            capsule.height = capsule_height;
            capsule.center = new Vector3(0f, capsule_centre, 0f);
            capsule.radius = capsule_radius;
        }
    }

    //Getters relevant for passing variables into the animator controller
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

    public bool GetJumping()
    {
        return is_jumping;
    }

    public Vector2 GetPlayerInput()
    {
        return move_input;
    }

    public bool GetCrouching()
    {
        return is_crouching;
    }

    public bool GetRunning()
    {
        return is_running;
    }

    public bool GetPlayerDirection()
    {
        bool dir = false;
        //Need to get the direction of the player
        return dir;
    }
}

/*Cal's script ends here*/
