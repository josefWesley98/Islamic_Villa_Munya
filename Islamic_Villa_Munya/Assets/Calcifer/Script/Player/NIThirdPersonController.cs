using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*Cal's script starts here */

public class NIThirdPersonController : MonoBehaviour
{
    [SerializeField] private AnimationStateController animator_ref;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider capsule;
    [SerializeField] private PlayerControls controls;
    [SerializeField] private ThirdPersonCam main_cam_ref;
    [SerializeField] private Transform hand_hold;
    private GameObject pop_obj;
    Keyboard kb;
    InputAction hide_cursor;
    InputAction show_cursor;
    InputAction movement;
    InputAction jumping;
    InputAction running;
    InputAction crouching;
    InputAction push_or_pull;

    [SerializeField] private Transform orientation;
    [SerializeField] private Transform feet_pos;
    [SerializeField] private Transform head_pos;
    [SerializeField] private Transform wall_pos;
    [SerializeField] private Transform push_pos;

    private Vector3 input;
    private Vector3 move_dir;
    private Vector2 move_input;

    [SerializeField] private LayerMask is_ground;
    [SerializeField] private LayerMask is_ceiling;
    [SerializeField] private LayerMask is_a_wall;
    [SerializeField] private LayerMask is_a_pushable;

    private bool grounded;
    private bool is_roof;
    private bool is_wall;
    private bool is_pushable;
    private bool push;
    private bool is_running = false;
    private bool is_crouching = false;
    private bool is_jumping = false;
    private bool is_run_jump_ready = true;
    private bool is_standing_jump = false;
    private bool is_stand_jump_ready = true;
    private bool input_disabled = false;
    private bool hard_landing = false;
    private bool allow_jump = true;
    private bool artefact_collected;
    private bool pushing = false;
    private bool currently_pushing = false;
    private bool ready_to_push = false;

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
    [SerializeField] private bool isClimbing = false;

    private float stand_jump_delay = 0.5f;  //Change depending on the animation used
    private float run_jump_delay;
    private float disabled_input_delay;
    private int score = 0;


    void Awake() 
    {
        //Assign the new input controller
        controls = new PlayerControls();
        kb = InputSystem.GetDevice<Keyboard>();
    }
    private void Start()
    {
        //Initialise necessary variables here for player startup

        //Set the capsule collider to variables that will change when crouching
        capsule.height = capsule_height;
        capsule.radius = capsule_radius;
        capsule.center = new Vector3(0f, capsule_centre, 0f);

        //Freeze the rotation of the player indefinitely.
        rb.constraints = RigidbodyConstraints.FreezeRotation;
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
        
        if(!isClimbing)
        {
            jumping = controls.Player.Jump;
            jumping.Enable();
            
            movement = controls.Player.Move;
            movement.Enable();

            running = controls.Player.ToggleRun;
            running.Enable();

            crouching = controls.Player.Crouch;
            crouching.Enable();

            //push_or_pull = controls.Player.PushPull;
           // push_or_pull.Enable();

            jumping.started += _ => Jump();

            running.started += _ => ToggleRunning(_);
            running.canceled += _ => ToggleRunning(_);

            crouching.started += _ => ToggleCrouch(_);
            crouching.canceled += _ => ToggleCrouch(_);

            //push_or_pull.performed += _ => PushOrPull(_);
            //push_or_pull.canceled += _ => PushOrPull(_);
        }

        //Use input actions to call necessary functions for player functionality

        hide_cursor.started += _ => HideCursor();
        show_cursor.started += _ => ShowCursor();
    }

    private void OnDisable() 
    {
        controls.Disable();
        movement.Disable();
        jumping.Disable();
        running.Disable();
    }

    // Update is called once per frame
    private void Update()
    {
        PlayerInput();
        Debug.Log("Getttttting input no matter what");

        if (!isClimbing)
        {
            //Function to hide cursor when playing
            HideCursor();

            hard_landing = animator_ref.GetHardLanding();

            //Check to see if the player is on the ground
            bool floor = Physics.CheckSphere(feet_pos.position, 0.2f, is_ground);
            bool roof = Physics.CheckSphere(head_pos.position, 0.1f, is_ceiling);
            bool wall = Physics.CheckSphere(wall_pos.position, 0.2f, is_a_wall);
            //push = Physics.CheckSphere(push_pos.position, 0.1f, is_a_pushable);

            //If the player is on the floor then set grounded to true and allow for movement and jumping
            if (floor)
            {
                grounded = true;

                //if (push)
                //{
                //   is_pushable = true;
                //   SetPushingCollider();
                //}
                //else if (!push)
                //{
                //   is_pushable = false;
                //   //ResetCapsuleCollider();
                //   ResetPushCollider();
                //   //rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
                //}
            }
            else
            {
                grounded = false;
            }

            //Player will still be on ground but crouched, so allow for movement, but prevent the player from standing again.
            if(roof)
            {
                //grounded = true;
                is_roof = true;
            }
            else if(!roof)
            {
                //grounded = false;
                is_roof = false;

                //Reset the capsule to it's usual parameters when player is standing
                if(is_crouching)
                {
                    bool force_stop_crouch = kb.leftCtrlKey.isPressed;
                    if(!force_stop_crouch)
                    {
                        //ResetCapsuleCollider();
                        Debug.Log("First freeze");
                    }
                }
            }
            
            if(wall)
            {
                //StopPlayer();
                is_wall = true;
                
            }
            else if(!wall)
            {
                is_wall = false;
            }
            
            //Handle drag
            if (grounded && !hard_landing)
            {
                //If the player speed is above the maximum velocity then call this function
                ControlPlayerVel();

                //Prevent moving in the air by preventing player input updates during a jump
                rb.drag = ground_drag;
                //is_stand_jump_ready = true;
                
                //Deal with pushing and pulling of objects only on the floor
                //if(pushing)
                //{
                //    if(push_or_pull.triggered)
                //    {
                //        pushing = false;
                //        if(pop_obj != null)
                //        {
                //            //Unparent the object from the player when not pushing
                //            pop_obj.GetComponent<PushOrPull>().Push(pushing, gameObject);
                //        }
                //    }
                //}
                //else
                //{
                //    if(push_or_pull.triggered)
                //    {
                //        pushing = true;
                //    }
                //}
            }
            else
            {
                rb.drag = 0f;
                //is_stand_jump_ready = false;
            }

            //If the player falls and has a hard landing, disable the input
            if(hard_landing)
            {
                float disable_input = animator_ref.GetHardLandAnimTime();
                StartCoroutine(DisableInput(disable_input));

                //HardLandCapsuleCollider();
            }
        }
        else
        {
            Debug.Log("Cancelled climbing");
        }

    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.tag == "Pickup")
        {
            GameManager.SetArtefactCollected(true);
            GameManager.SetHUBTravel(true);
            Destroy(other.gameObject);
        }
    }

    //private void OnCollisionStay(Collision other)
    //{
    //    ready_to_push = false;

    //    if (other.gameObject.GetComponent<PushOrPull>() != null && push)
    //    {
    //        ready_to_push = true;
    //        //Call the game object function to set it's transform as that of the child of the player
    //        Vector3 direction = other.transform.position - transform.position;

    //        // Set the y component to 0 to ensure only rotation around the y-axis
    //        direction.y = 0;

    //        // Calculate the target rotation using Quaternion.LookRotation
    //        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
    //        transform.localRotation = targetRotation;

    //        pop_obj = other.gameObject;
    //        pop_obj.GetComponent<PushOrPull>().Push(pushing, gameObject);
    //        currently_pushing = pop_obj.GetComponent<PushOrPull>().GetIsParented();

    //        /*Joe's Code*/
    //        //Change the collider properties if the player is PoPing
    //        if(currently_pushing)
    //        {

    //            /*Joe's Code ends*/

    //            SetPushingCollider();
    //        }
    //    }
    //}

    private void FixedUpdate()
    {
        //Call player movement
        //if(!input_disabled)
        //{
            PlayerMovement();
        //}
    }

    private void PlayerInput()
    {
        //if(grounded && !input_disabled)
        //{
            //Newer input system reading input values from the player
            move_input = movement.ReadValue<Vector2>();
        //}
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
            if((is_running && is_crouching))
            {
                rb.AddForce(move_dir.normalized * walk_speed * 10, ForceMode.Force);
            }
        }

        //else if(grounded && !currently_pushing)
        //{
        //    //Player is pushing or pulling (Doesn't work with just if pushing and running for some reason)
        //    rb.AddForce(move_dir.normalized * walk_speed * 10, ForceMode.Force);
        //}
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
        if(!is_crouching)
        {
            rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
        }

        #region original_jump
        //if (grounded &&!is_crouching && allow_jump)
        //{
        //    is_jumping = true;
        //    bool apply_delay = false;

        //    //If the player magnitude is < 0.1, then apply delay for standing jump
        //    if(rb.velocity.magnitude < 0.1f)
        //    {
        //        //Get delay duration for disabling input to be that of the stand animation duration
        //        disabled_input_delay = animator_ref.GetStandJumpAnimTime() - 0.3f;

        //        apply_delay = true;
        //        StartCoroutine(DisableInput(disabled_input_delay));
        //    }

        //    if ((apply_delay && is_stand_jump_ready) || (apply_delay && is_stand_jump_ready && is_wall))
        //    {
        //        //Apply the delay before allowing the player to jump
        //        is_stand_jump_ready = false;

        //        Invoke("DelayedJump", stand_jump_delay);
        //    }

        //    //Run jump
        //    else if(rb.velocity.magnitude > 0.1f && !is_wall)
        //    {
        //        //Get delay from the animator
        //        run_jump_delay = animator_ref.GetRunJumpAnimTime() - 0.2f;

        //        //Reset the player y-velocity to 0 so they player jumps to the same height every time
        //        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //        //Apply the force once using impulse
        //        rb.AddForce(transform.up * moving_jump_force, ForceMode.Impulse);

        //        allow_jump = false;
        //        StartCoroutine(DelayRunJump(run_jump_delay));
        //    }

        //    Invoke("NoRotationDelay", 1f);
        //}
        #endregion

        //if (!is_crouching && allow_jump)
        //{
        //    is_jumping = true;
        //    bool apply_delay = false;

        //    //If the player magnitude is < 0.1, then apply delay for standing jump
        //    if (rb.velocity.magnitude < 0.1f)
        //    {
        //        //Get delay duration for disabling input to be that of the stand animation duration
        //        disabled_input_delay = animator_ref.GetStandJumpAnimTime() - 0.3f;

        //        apply_delay = true;
        //        //StartCoroutine(DisableInput(disabled_input_delay));
        //    }

        //    if ((apply_delay && is_stand_jump_ready) || (apply_delay && is_stand_jump_ready && is_wall))
        //    {
        //        //Apply the delay before allowing the player to jump
        //        is_stand_jump_ready = false;

        //        //Invoke("DelayedJump", stand_jump_delay);
        //        rb.AddForce(0f, stand_jump_force, 0f, ForceMode.Impulse);
        //        //rb.AddForce(move_dir.normalized * walk_speed * 10, ForceMode.Force);
        //    }

        //    //Run jump
        //    else if (rb.velocity.magnitude > 0.1f && !is_wall)
        //    {
        //        //Get delay from the animator
        //        run_jump_delay = animator_ref.GetRunJumpAnimTime() - 0.2f;

        //        //Reset the player y-velocity to 0 so they player jumps to the same height every time
        //        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //        //Apply the force once using impulse
        //        //rb.AddForce(transform.up * moving_jump_force, ForceMode.Impulse);
        //        rb.AddForce(0f, moving_jump_force, 0f, ForceMode.Impulse);
        //        allow_jump = false;
        //        StartCoroutine(DelayRunJump(run_jump_delay));
        //    }

        //Invoke("NoRotationDelay", 1f);
        //}

        if(grounded && !is_crouching && allow_jump)
        {
            //Get delay from the animator
            run_jump_delay = animator_ref.GetRunJumpAnimTime() - 0.2f;

            //Reset the player y-velocity to 0 so they player jumps to the same height every time
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //Apply the force once using impulse
            //rb.AddForce(transform.up * moving_jump_force, ForceMode.Impulse);
            rb.AddForce(0f, moving_jump_force, 0f, ForceMode.Impulse);
            allow_jump = false;
            StartCoroutine(DelayRunJump(run_jump_delay));
        }
    }

    //Prevents the player jumping immediately again after landing
    private IEnumerator DelayRunJump(float duration)
    {
        yield return new WaitForSeconds(duration);
        allow_jump = true;
    }

    private void DelayedJump()
    {
        if(grounded)
        {
            rb.AddForce(transform.up * stand_jump_force, ForceMode.Impulse);
            is_stand_jump_ready = true;
        }
    }

    private IEnumerator DisableInput(float duration)
    {
        //input_disabled = true;
        allow_jump = false;

        if (hard_landing)
        {
            animator_ref.SetHardLanding(false);
        }

        rb.velocity = Vector3.zero;;

        float prev_mag = rb.velocity.magnitude;

        yield return new WaitForSeconds(duration);

        while(rb.velocity.magnitude > 0.1f)
        {
            yield return null;
            if(rb.velocity.magnitude > prev_mag)
            {
                //Player is still moving so reset the timer
                prev_mag = rb.velocity.magnitude;
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                //Player is slowing down or is stationary
                prev_mag = rb.velocity.magnitude;
                yield return null;
            }

        }
        
        //Reset capsule properties after landing has finished
        if(hard_landing)
        {
            //ResetCapsuleCollider();
            Debug.Log("Second Freeze");
        }
        //input_disabled = false;
        allow_jump = true;
    }

    //Hide cursor function
    public void HideCursor()
    {
        if (!GameManager.GetPauseCursor())
        {
            //Hides cursor while playing
            //Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    //Show the cursor
    public void ShowCursor()
    {
        if (GameManager.GetPauseCursor())
        {
            //Shows cursor while playing
            //Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    //Function for pushing or pulling object
    //private void PushOrPull(InputAction.CallbackContext pop)
    //{
    //    //Set the transform of the object to be the child of the player.
    //    //If the bool is true or false then the player class will call the objects function to set itself as a child
    //    //object of the player, effectively joining them
    //    if(ready_to_push && !pushing && !is_crouching && !is_jumping)
    //    {
    //        //We want to lock the player position axes here
    //        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    //    }
    //    else if(!ready_to_push)
    //    {
    //        rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
    //    }
    //}

    //Function for determining whether the player wants to jump or not
    private void ToggleRunning(InputAction.CallbackContext run)
    {
        if(grounded)
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
    }

    //Function for determining whether the player wants to crouch or not
    private void ToggleCrouch(InputAction.CallbackContext crouch)
    {
        #region ToggleCrouchOG
        //if (grounded && !input_disabled)
        //{
        //    if(crouch.started)
        //    {
        //        is_crouching = true;
        //        //capsule.height = capsule_height / 2;
        //        //capsule.center = new Vector3(0f, capsule_centre / 2, 0f);
        //        //capsule.radius = capsule_radius * 2;
        //    }
        //    if(crouch.canceled && !is_roof)
        //    {
        //        is_crouching = false;
        //        //capsule.height = capsule_height;
        //        //capsule.center = new Vector3(0f, capsule_centre, 0f);
        //        //capsule.radius = capsule_radius;
        //    }
        //}
        #endregion

        if (grounded)
        {
            if (crouch.started)
            {
                is_crouching = true;
                //capsule.height = capsule_height / 2;
                //capsule.center = new Vector3(0f, capsule_centre / 2, 0f);
                //capsule.radius = capsule_radius * 2;
            }
            if (crouch.canceled && !is_roof)
            {
                is_crouching = false;
                //capsule.height = capsule_height;
                //capsule.center = new Vector3(0f, capsule_centre, 0f);
                //capsule.radius = capsule_radius;
            }
        }
    }

    //private void SetPushingCollider()
    //{
    //    capsule.height = 1f;
    //    capsule.center = new Vector3(0f, 0.3f, 0.3f);
    //    capsule.radius = 0.3f;
    //}

    private void ResetCapsuleCollider()
    {

        is_crouching = false;
        capsule.height = capsule_height;
        capsule.center = new Vector3(0f, capsule_centre, 0f);
        capsule.radius = capsule_radius;
        
        //Use bit mask to stop the player pinging into the air when moving the centre of gravity
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }

    //private void ResetPushCollider()
    //{
    //    capsule.height = capsule_height;
    //    capsule.center = new Vector3(0f, capsule_centre, 0f);
    //    capsule.radius = capsule_radius;
    //}

    private void HardLandCapsuleCollider()
    {
        capsule.height = capsule_height / 2;
        capsule.center = new Vector3(0f, capsule_centre / 2, 0f);
        capsule.radius = capsule_radius * 2;
    }

    //Getters relevant for passing variables into the animator controller
    public Vector3 GetRBVelocity(Vector3 vel)
    {
        vel = rb.velocity;
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

    public bool GetRunningJumping()
    {
        if(!grounded)
        {
            return true;
        }

        return false;
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

    public bool GetPushOrPull()
    {
        return currently_pushing;
    }

    public bool GetRunning()
    {
        return is_running;
    }

    public bool GetAllowJump()
    {
        return allow_jump;
    }

    public bool GetIsClimbing()
    {
        return isClimbing;
    }

    public bool GetArtefactCollected()
    {
        return artefact_collected;
    }

    public Transform GetHold()
    {
        return hand_hold;
    }

    //Setters
    public void SetArtefactCollected(bool val)
    {
        artefact_collected = val;
    }
    public void SetIsClimbing(bool change)
    {
        isClimbing = change;
    }
}

/*Cal's script ends here*/
