using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*Cal's code starts here*/

public class NIAnimationStateController : MonoBehaviour
{
    Animator animator;
    public NIThirdPersonController controller_ref;
    float velocityZ = 0f;
    float velocityX = 0f;
    private float acceleration = 7f;
    private float deceleration = 5f;
    public float max_walk_vel = 0.5f;
    public float max_run_vel = 2f;
    int velZ_hash;
    int velX_hash;
    int rb_velX_hash;
    int rb_velZ_hash;
    private int crouch_layer_index;
    private int jump_layer_index;
    private int base_layer_index;
    private float crouch_weight = 10f;
    private float weight = 0f;
    private float stand_jump_time = 0f;
    private float run_jump_time = 0f;
    private float hard_land_time = 0f;
    private float j_timer = 0f;
    private float land_timer = 0f;
    private bool is_jumping = false;
    private bool run_jump = false;
    private bool stand_jump = false;
    private bool hard_land = false;
    private Vector3 rb_vel;
    private bool input_disabled = false;

    private float time_since_fallen = 0f;
    private float hard_landing_threshold = 0.1f;

    private float falling_delay_time = 0.1f;

    Keyboard kb;


    // Start is called before the first frame update
    void Start()
    {
        //For player input
        kb = InputSystem.GetDevice<Keyboard>();

        //Search for the animator component attached to this object
        animator = GetComponent<Animator>();

        //Use hash so it is less expensive for changing these float variables in the animator controller
        velZ_hash = Animator.StringToHash("VelocityZ");
        velX_hash = Animator.StringToHash("VelocityX");
        rb_velX_hash = Animator.StringToHash("RB_velX");
        rb_velZ_hash = Animator.StringToHash("RB_velZ");

        //Referencing the layer for crouching
        crouch_layer_index = animator.GetLayerIndex("Crouching");
        jump_layer_index = animator.GetLayerIndex("Jumping");
        base_layer_index = animator.GetLayerIndex("Base Layer");

        //Get the animation times for specific animations
        UpdateAnimTimes();
    }

    //Handles acceleration and deceleration
    //void ChangeVel(bool forward_pressed, bool backward_pressed, bool left_pressed, bool right_pressed, bool run_pressed, bool crouch_pressed, float current_max_vel)
    //{
    //    //2D Blend trees example begins here
    //    //If player presses forward, increase vel in z direction
    //    if (forward_pressed && velocityZ < current_max_vel)
    //    {
    //        velocityZ += Time.deltaTime * acceleration;
    //    }

    //    //If the player presses walk backwards, decrease the vel in the z direction
    //    if (backward_pressed && velocityZ < current_max_vel)
    //    {
    //        velocityZ -= Time.deltaTime * acceleration;
    //    }

    //    //If the player presses left, increase vel in left dir
    //    if (left_pressed && velocityX > -current_max_vel)
    //    {
    //        velocityX -= Time.deltaTime * acceleration;
    //    }

    //    //If player moves right, increase vel in right dir
    //    if (right_pressed && velocityX < current_max_vel)
    //    {
    //        velocityX += Time.deltaTime * acceleration;
    //    }

    //    //Decrease velocityZ
    //    if (!forward_pressed && velocityZ > 0f)
    //    {
    //        velocityZ -= Time.deltaTime * deceleration;
    //    }

    //    //Increase the velocity when not moving backwards to 0
    //    if (!backward_pressed && velocityZ < 0f)
    //    {
    //        velocityX += Time.deltaTime * deceleration;
    //    }

    //    //Increase velocityX if left is not pressed and velocityX < 0
    //    if (!left_pressed && velocityX < 0f)
    //    {
    //        velocityX += Time.deltaTime * deceleration;
    //    }

    //    //Decrease velocityX if right is not pressed and velocityX > 0
    //    if (!right_pressed && velocityX > 0f)
    //    {
    //        velocityX -= Time.deltaTime * deceleration;
    //    }

    //    //Reset the crouch weight to 0
    //    if (crouch_pressed && weight > 1f)
    //    {
    //        weight = 1f;
    //    }

    //    if (!crouch_pressed && weight < 0f)
    //    {
    //        weight = 0f;
    //    }
    //}

    void ChangeVel(Vector3 rb_vel, bool jump_pressed, bool crouch_pressed, float current_max_vel)
    {
        //Newer functionality
        //If rb moving forward, increase vel in z direction
        if((rb_vel.z > 0.1f) && (velocityZ < current_max_vel))
        {
            velocityZ += Time.deltaTime * acceleration;
        }

        //If rb is moving backwards, decrease vel in z direction
        if((rb_vel.z < -0.1f) && velocityZ < -current_max_vel)
        {
            velocityZ -= Time.deltaTime * acceleration;
        }
        
        //If the player moves to the left, increase the vel in left direction
        if((rb_vel.x < -0.1f) && (velocityX > -current_max_vel))
        {
            velocityX -= Time.deltaTime * acceleration;
        }

        //If the player moves to the right, increase the vel in the right direction
        if((rb_vel.x > 0.1f) && (velocityX < current_max_vel))
        {
            velocityX += Time.deltaTime * acceleration;
        }

        //Reset the crouch weight to 0
        if (crouch_pressed && weight > 1f)
        {
            weight = 1f;
        }

        if (!crouch_pressed && weight < 0f)
        {
            weight = 0f;
        }
    }

    //void LockOrResetVel(bool forward_pressed, bool backward_pressed, bool left_pressed, bool right_pressed, bool run_pressed, bool crouch_pressed, float current_max_vel)
    //{
    //    //Reset velocityZ
    //    if(!forward_pressed && !backward_pressed && velocityZ < 0f)
    //    {
    //        velocityZ = 0f;
    //    }

    //    //Reset velocityX
    //    if(!left_pressed && !right_pressed && velocityX != 0f && (velocityX > -0.5f && velocityX < 0.05f))
    //    {
    //        velocityX = 0f;
    //    }

    //    //Lock forward
    //    if(forward_pressed && run_pressed && velocityZ > current_max_vel)
    //    {
    //        velocityZ = current_max_vel;
    //    }
    //    //Decelerate to the maximum walk vel
    //    else if(forward_pressed && velocityZ > current_max_vel)
    //    {
    //        velocityZ -= Time.deltaTime * deceleration;

    //        //Round to the current max vel if within offset
    //        if(velocityZ > current_max_vel && velocityZ < (current_max_vel + 0.05f))
    //        {
    //            velocityZ = current_max_vel;
    //        }
    //    }
    //    else if(forward_pressed && velocityZ < current_max_vel && velocityZ > (current_max_vel - 0.05f))
    //    {
    //        velocityZ = current_max_vel;
    //    }

    //    //Lock backwards
    //    if(backward_pressed && run_pressed && velocityZ < -current_max_vel)
    //    {
    //        velocityZ = -current_max_vel;
    //    }
    //    //Decelerate to the maximum walk vel
    //    else if(backward_pressed && velocityZ < -current_max_vel)
    //    {
    //        velocityZ += Time.deltaTime * deceleration;

    //        //Round to the current max vel if within offset
    //        if(velocityZ < -current_max_vel && velocityZ < (-current_max_vel - 0.05f))
    //        {
    //            velocityZ = -current_max_vel;
    //        }
    //    }
    //    else if(backward_pressed && velocityZ > -current_max_vel && velocityZ > (-current_max_vel + 0.05f))
    //    {
    //        velocityZ = -current_max_vel;
    //    }

    //      //Lock left
    //    if(left_pressed && run_pressed && velocityX < -current_max_vel)
    //    {
    //        velocityX = -current_max_vel;
    //    }
    //    //Decelerate to the maximum walk vel
    //    else if(left_pressed && velocityX < -current_max_vel)
    //    {
    //        velocityX += Time.deltaTime * deceleration;

    //        //Round to the current max vel if within offset
    //        if(velocityX < -current_max_vel && velocityX < (-current_max_vel - 0.05f))
    //        {
    //            velocityX = -current_max_vel;
    //        }
    //    }
    //    else if(left_pressed && velocityX > -current_max_vel && velocityX > (-current_max_vel + 0.05f))
    //    {
    //        velocityX = -current_max_vel;
    //    }

    //    //Lock right
    //    if(right_pressed && run_pressed && velocityX > current_max_vel)
    //    {
    //        velocityX = current_max_vel;
    //    }
    //    //Decelerate to the maximum walk vel
    //    else if(right_pressed && velocityX > current_max_vel)
    //    {
    //        velocityX -= Time.deltaTime * deceleration;

    //        //Round to the current max vel if within offset
    //        if(velocityX > current_max_vel && velocityX < (current_max_vel + 0.05f))
    //        {
    //            velocityX = current_max_vel;
    //        }
    //    }
    //    else if(right_pressed && velocityX < current_max_vel && velocityX > (current_max_vel - 0.05f))
    //    {
    //        velocityX = current_max_vel;
    //    }
    //}

    //Function to get the anim times for certain animations to play through to completion without interruption

    void LockOrResetVel(Vector3 rb_vel, bool run_pressed, bool crouch_pressed, float current_max_vel)
    {
        //Newer functionality
        //Reset the velocityZ
        if((rb_vel.z == 0) && (velocityZ < 0f || velocityZ > 0f))
        {
            velocityZ = 0f;
        }

        //Reset velocityX
        if((rb_vel.x == 0) && (velocityX < 0f || velocityX > 0f))
        {
            velocityX = 0f;
        }

        //Lock forward so the velocity z component cannot go above threshold
        if(rb_vel.z > 0f && run_pressed && velocityZ > current_max_vel)
        {
            velocityZ = current_max_vel;
        }
        //Decelrate to the maximum walk vel
        else if(rb_vel.z > 0f && velocityZ > current_max_vel)
        {
            velocityZ -= Time.deltaTime * deceleration;

            //Round to the current max vel if within range
            if(velocityZ > current_max_vel && velocityZ < (current_max_vel + 0.05f))
            {
                velocityZ = current_max_vel;
            }
        }
        else if(rb_vel.z > 0f && velocityZ < current_max_vel && velocityZ > (current_max_vel - 0.05f))
        {
            velocityZ = current_max_vel;
        }

        //Lock backwards
        if (rb_vel.z < 0f && run_pressed && velocityZ < -current_max_vel)
        {
            velocityZ = -current_max_vel;
        }
        //Decelerate to the maximum walk vel
        else if (rb_vel.z < 0f && velocityZ < -current_max_vel)
        {
            velocityZ += Time.deltaTime * deceleration;

            //Round to the current max vel if within offset
            if (velocityZ < -current_max_vel && velocityZ < (-current_max_vel - 0.05f))
            {
                velocityZ = -current_max_vel;
            }
        }
        else if (rb_vel.z < 0f && velocityZ > -current_max_vel && velocityZ > (-current_max_vel + 0.05f))
        {
            velocityZ = -current_max_vel;
        }

        //Lock left
        if (rb_vel.x < 0f && run_pressed && velocityX < -current_max_vel)
        {
            velocityX = -current_max_vel;
        }
        //Decelerate to the maximum walk vel
        else if (rb_vel.x < 0f && velocityX < -current_max_vel)
        {
            velocityX += Time.deltaTime * deceleration;

            //Round to the current max vel if within offset
            if (velocityX < -current_max_vel && velocityX < (-current_max_vel - 0.05f))
            {
                velocityX = -current_max_vel;
            }
        }
        else if (rb_vel.x < 0f && velocityX > -current_max_vel && velocityX > (-current_max_vel + 0.05f))
        {
            velocityX = -current_max_vel;
        }

        //Lock right
        if (rb_vel.x > 0f && run_pressed && velocityX > current_max_vel)
        {
            velocityX = current_max_vel;
        }
        //Decelerate to the maximum walk vel
        else if (rb_vel.x > 0f && velocityX > current_max_vel)
        {
            velocityX -= Time.deltaTime * deceleration;

            //Round to the current max vel if within offset
            if (velocityX > current_max_vel && velocityX < (current_max_vel + 0.05f))
            {
                velocityX = current_max_vel;
            }
        }
        else if (rb_vel.x > 0 && velocityX < current_max_vel && velocityX > (current_max_vel - 0.05f))
        {
            velocityX = current_max_vel;
        }

        Debug.Log(rb_vel);
    }

    void UpdateAnimTimes()
    {
        //Create an array to store the player animations which we will loop through
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        //Foreach loop to loop through the anims until we get to jumping
        foreach(AnimationClip clip in clips)
        {
            //If we reach the jumping animation, get the length and store this in the relative time variable
            if(clip.name == "H_Jumping2")
            {
                stand_jump_time = clip.length;
            }

            if(clip.name == "H_Jump3")
            {
                run_jump_time = clip.length;
            }

            if(clip.name == "H_HardLanding")
            {
                hard_land_time = clip.length;
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        //Variables - Player Control
        //Get key input
        //GET THE INPUT FROM THE PLAYER CONTROLLER CLASS INSTEAD OF USING BUTTON PRESSES
        //bool forward_pressed = Input.GetKey(KeyCode.W);
        //bool left_pressed = Input.GetKey(KeyCode.A);
        //bool right_pressed = Input.GetKey(KeyCode.D);
        //bool run_pressed = Input.GetKey(KeyCode.LeftShift);
        //bool backward_pressed = Input.GetKey(KeyCode.S);
        //bool crouch_pressed = Input.GetKey(KeyCode.LeftControl);
        //bool jump_pressed = Input.GetKey(KeyCode.Space);

        //Variables to be gotten from the player controller class
        //Use bools for player actions
        //Use rigid body velocity to determine the WASD direction movement
        rb_vel = controller_ref.GetRBVelocity(rb_vel);
        bool jumping_pressed = controller_ref.GetJumping();
        bool crouch_pressed = controller_ref.GetCrouching();
        bool running_pressed = controller_ref.GetRunning();


        //Set current max_vel
        float current_max_vel = running_pressed ? max_run_vel : max_walk_vel;

        //Handle changes in velocity
        //ChangeVel(forward_pressed, backward_pressed, left_pressed, right_pressed, running_pressed, crouch_pressed, current_max_vel);
        ChangeVel(rb_vel, jumping_pressed, crouch_pressed, current_max_vel);
        //LockOrResetVel(forward_pressed, backward_pressed, left_pressed, right_pressed, run_pressed, crouch_pressed, current_max_vel);
        LockOrResetVel(rb_vel, jumping_pressed, crouch_pressed, current_max_vel);

        //When the player presses crouch, set the layer weight to 1        
        if(crouch_pressed)
        {
            //Increment the weight of the crouch
            if(weight >= -0.5)
            {
                weight += Time.deltaTime * crouch_weight;
            }
            animator.SetLayerWeight(crouch_layer_index, weight);
        }

        //Return the layer weight to 0 when the player releases the crouch button
        if(!crouch_pressed)
        {
            //Decrement the weight of the crouch
            if(weight > 0f)
            {
                weight -= Time.deltaTime * crouch_weight;
            }
            animator.SetLayerWeight(crouch_layer_index, weight);
        }

        //Debug.Log(rb_vel);
        //if(!crouch_pressed && jump_pressed && ((rb_vel.x > 0.1 || rb_vel.x < -0.1) || 
        //(rb_vel.z > 0.1 || rb_vel.z < -0.1)))
        //{
        //    animator.SetBool("isJumping", true);

        //    run_jump = true;

        //    j_timer = run_jump_time - 0.5f;

        //    stand_jump = false;
        //}
        //else if(!crouch_pressed && jump_pressed && ((rb_vel.x <= 0.1 && rb_vel.x >= -0.1 ) || 
        //(rb_vel.z <= 0.1 && rb_vel.z >= -0.1)))
        //{
        //    animator.SetBool("isJumping", true);

        //    stand_jump = true;

        //    j_timer = stand_jump_time - 0.3f;

        //    run_jump = false;
        //}

        if (run_jump)
        {
            j_timer -= Time.deltaTime;
            if(j_timer <= 0)
            {
                animator.SetBool("isJumping", false);
                run_jump = false;
            }
        }
        else if (stand_jump)
        {
            j_timer -= Time.deltaTime;
            //Debug.Log(j_timer);
            if(j_timer <= 0)
            {
                animator.SetBool("isJumping", false);
                stand_jump = false;
            }
        }

        bool is_grounded = controller_ref.GetGrounded();

        //Is the player falling? Play animation if they are
        if(!stand_jump && !run_jump && !is_grounded)
        {
            StartCoroutine(DelayFallingAnim());
            time_since_fallen += Time.deltaTime;
        }
        else
        {
            //Set the falling animation to stop playing
            animator.SetBool("IsFalling", false);

            if(time_since_fallen > hard_landing_threshold)
            {
                //land_timer = hard_land_time;
                //animator.SetBool("HardLand", true);
                //hard_land = true;
                StartCoroutine(HardLandAnimation());
                time_since_fallen = 0f;
            }

            // if(hard_land)
            // {
            //     land_timer -= Time.deltaTime;
            //     Debug.Log(land_timer);
            //     if(land_timer <= 0)
            //     {
            //         animator.SetBool("HardLand", false);
            //         hard_land = false;
            //         Debug.Log(hard_land);
            //         time_since_fallen = 0f;
            //     }
            // }
        }


        animator.SetFloat(velZ_hash, velocityZ);
        animator.SetFloat(velX_hash, velocityX);
        animator.SetFloat(rb_velX_hash, rb_vel.x);
        animator.SetFloat(rb_velZ_hash, rb_vel.z);
    }

    IEnumerator DelayFallingAnim()
    {
        yield return new WaitForSeconds(falling_delay_time);
        animator.SetBool("IsFalling", true);
    }

    IEnumerator HardLandAnimation()
    {
        StartCoroutine(DisableInput(hard_land_time));
        animator.SetBool("HardLand", true);
        yield return new WaitForSeconds(hard_land_time - 0.3f);
        animator.SetBool("HardLand", false);
    }

    private IEnumerator DisableInput(float duration)
    {
        input_disabled = true;
        yield return new WaitForSeconds(duration);
        input_disabled = false;
    }
}

/*Cal's code ends here*/
