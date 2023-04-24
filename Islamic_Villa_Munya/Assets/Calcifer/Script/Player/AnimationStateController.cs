using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*Cal's code starts here*/
public class AnimationStateController : MonoBehaviour
{
    private Animator animator;
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
    private int movementLayerIndex;
    private int pushing_layer_index;
    private float crouch_weight = 10f;
    private float weight = 0f;
    private float stand_jump_time = 0f;
    private float run_jump_time = 0f;
    private float hard_land_time = 0f;
    private float j_timer_run = 0f;
    private float j_timer_stand = 0f;
    private float land_timer = 0f;
    private bool is_jumping = false;
    private bool run_jump = false;
    private bool stand_jump = false;
    private bool hard_land = false;
    private Vector3 rb_vel;
    private bool input_disabled = false;

    private float time_since_fallen = 0f;
    private float hard_landing_threshold = 0.6f;

    private float falling_delay_time = 0.2f;
    private bool currently_jumping = false;
    private int climbLayerIndex;
    AnimatorClipInfo[] info;

    Keyboard kb;

    // Start is called before the first frame update
    void Start()
    {
        //User input from keyboard
        kb = InputSystem.GetDevice<Keyboard>();

        animator = GetComponent<Animator>();

        //Use hash so it is less expensive for changing these float variables in the animator controller
        velZ_hash = Animator.StringToHash("VelocityZ");
        velX_hash = Animator.StringToHash("VelocityX");
        rb_velX_hash = Animator.StringToHash("RB_velX");
        rb_velZ_hash = Animator.StringToHash("RB_velZ");

        //Referencing the layer for crouching
        climbLayerIndex = animator.GetLayerIndex("Climbing");
        crouch_layer_index = animator.GetLayerIndex("Crouching");
        jump_layer_index = animator.GetLayerIndex("Jumping");
        movementLayerIndex = animator.GetLayerIndex("Movement");
        pushing_layer_index = animator.GetLayerIndex("Pushing");

        //Get the animation times for specific animations
        UpdateAnimTimes();
    }

    //Handles acceleration and deceleration
    #region Velocity Functions
    void ChangeVel(bool forward_pressed, bool backward_pressed, bool left_pressed, bool right_pressed, bool run_pressed, bool crouch_pressed, float current_max_vel)
    {
        //2D Blend trees example begins here
        //If player presses forward, increase vel in z direction
        if (forward_pressed && velocityZ < current_max_vel)
        {
            velocityZ += Time.deltaTime * acceleration;
        }

        //If the player presses walk backwards, decrease the vel in the z direction
        if (backward_pressed && velocityZ < current_max_vel)
        {
            velocityZ -= Time.deltaTime * acceleration;
        }

        //If the player presses left, increase vel in left dir
        if (left_pressed && velocityX > -current_max_vel)
        {
            velocityX -= Time.deltaTime * acceleration;
        }

        //If player moves right, increase vel in right dir
        if (right_pressed && velocityX < current_max_vel)
        {
            velocityX += Time.deltaTime * acceleration;
        }

        //Decrease velocityZ
        if (!forward_pressed && velocityZ > 0f)
        {
            velocityZ -= Time.deltaTime * deceleration;
        }

        //Increase the velocity when not moving backwards to 0
        if (!backward_pressed && velocityZ < 0f)
        {
            velocityX += Time.deltaTime * deceleration;
        }

        //Increase velocityX if left is not pressed and velocityX < 0
        if (!left_pressed && velocityX < 0f)
        {
            velocityX += Time.deltaTime * deceleration;
        }

        //Decrease velocityX if right is not pressed and velocityX > 0
        if (!right_pressed && velocityX > 0f)
        {
            velocityX -= Time.deltaTime * deceleration;
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



    void LockOrResetVel(bool forward_pressed, bool backward_pressed, bool left_pressed, bool right_pressed, bool run_pressed, bool crouch_pressed, float current_max_vel)
    {
        //Reset velocityZ
        if(!forward_pressed && !backward_pressed && velocityZ < 0f)
        {
            velocityZ = 0f;
        }

        //Reset velocityX
        if(!left_pressed && !right_pressed && velocityX != 0f && (velocityX > -0.5f && velocityX < 0.05f))
        {
            velocityX = 0f;
        }

        //Lock forward
        if(forward_pressed && run_pressed && velocityZ > current_max_vel)
        {
            velocityZ = current_max_vel;
        }
        //Decelerate to the maximum walk vel
        else if(forward_pressed && velocityZ > current_max_vel)
        {
            velocityZ -= Time.deltaTime * deceleration;

            //Round to the current max vel if within offset
            if(velocityZ > current_max_vel && velocityZ < (current_max_vel + 0.05f))
            {
                velocityZ = current_max_vel;
            }
        }
        else if(forward_pressed && velocityZ < current_max_vel && velocityZ > (current_max_vel - 0.05f))
        {
            velocityZ = current_max_vel;
        }

        //Lock backwards
        if(backward_pressed && run_pressed && velocityZ < -current_max_vel)
        {
            velocityZ = -current_max_vel;
        }
        //Decelerate to the maximum walk vel
        else if(backward_pressed && velocityZ < -current_max_vel)
        {
            velocityZ += Time.deltaTime * deceleration;

            //Round to the current max vel if within offset
            if(velocityZ < -current_max_vel && velocityZ < (-current_max_vel - 0.05f))
            {
                velocityZ = -current_max_vel;
            }
        }
        else if(backward_pressed && velocityZ > -current_max_vel && velocityZ > (-current_max_vel + 0.05f))
        {
            velocityZ = -current_max_vel;
        }

          //Lock left
        if(left_pressed && run_pressed && velocityX < -current_max_vel)
        {
            velocityX = -current_max_vel;
        }
        //Decelerate to the maximum walk vel
        else if(left_pressed && velocityX < -current_max_vel)
        {
            velocityX += Time.deltaTime * deceleration;

            //Round to the current max vel if within offset
            if(velocityX < -current_max_vel && velocityX < (-current_max_vel - 0.05f))
            {
                velocityX = -current_max_vel;
            }
        }
        else if(left_pressed && velocityX > -current_max_vel && velocityX > (-current_max_vel + 0.05f))
        {
            velocityX = -current_max_vel;
        }

        //Lock right
        if(right_pressed && run_pressed && velocityX > current_max_vel)
        {
            velocityX = current_max_vel;
        }
        //Decelerate to the maximum walk vel
        else if(right_pressed && velocityX > current_max_vel)
        {
            velocityX -= Time.deltaTime * deceleration;

            //Round to the current max vel if within offset
            if(velocityX > current_max_vel && velocityX < (current_max_vel + 0.05f))
            {
                velocityX = current_max_vel;
            }
        }
        else if(right_pressed && velocityX < current_max_vel && velocityX > (current_max_vel - 0.05f))
        {
            velocityX = current_max_vel;
        }
    }
    #endregion
    //Function to get the anim times for certain animations to play through to completion without interruption
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
    private void Update()
    {
        bool forward_pressed = kb.wKey.isPressed || kb.upArrowKey.isPressed;
        bool left_pressed = kb.aKey.isPressed || kb.leftArrowKey.isPressed;
        bool right_pressed = kb.dKey.isPressed || kb.rightArrowKey.isPressed;
        bool run_pressed = kb.leftShiftKey.isPressed;
        bool backward_pressed = kb.sKey.isPressed || kb.downArrowKey.isPressed;
        bool crouch_pressed = controller_ref.GetCrouching();
        bool jump_pressed = kb.spaceKey.isPressed;

        bool is_grounded = controller_ref.GetGrounded();
        rb_vel = controller_ref.GetRBVelocity(rb_vel);

        if(!controller_ref.GetInput())
        {

                if(controller_ref.GetPushOrPull())
                {
                    animator.SetLayerWeight(movementLayerIndex, 0.0f);
                    animator.SetLayerWeight(pushing_layer_index, 1f);
                    animator.SetLayerWeight(crouch_layer_index, 1f);
                }
                else
                {
                    animator.SetLayerWeight(pushing_layer_index, 0f);
                    animator.SetLayerWeight(crouch_layer_index, 0f);
                    animator.SetLayerWeight(movementLayerIndex, 1f);
                }

                if (controller_ref.GetIsClimbing())
                {
                    animator.SetLayerWeight(climbLayerIndex, 1.0f);
                    animator.SetLayerWeight(movementLayerIndex, 0.0f);
                
                }
                else 
                {

                    animator.SetLayerWeight(climbLayerIndex, 0.0f);
                    animator.SetLayerWeight(movementLayerIndex, 1.0f);
                }

                //Set current max_vel
                float current_max_vel = run_pressed ? max_run_vel : max_walk_vel;

                //Handle changes in velocity
                ChangeVel(forward_pressed, backward_pressed, left_pressed, right_pressed, run_pressed, crouch_pressed, current_max_vel);
                LockOrResetVel(forward_pressed, backward_pressed, left_pressed, right_pressed, run_pressed, crouch_pressed, current_max_vel);

                //When the player presses crouch, set the layer weight to 1        
                if(!controller_ref.GetPushOrPull())
                {
                    if (crouch_pressed)
                    {
                        //Increment the weight of the crouch
                        if (weight >= -0.5)
                        {
                            weight += Time.deltaTime * crouch_weight;
                        }
                        animator.SetLayerWeight(crouch_layer_index, weight);
                    }

                    //Return the layer weight to 0 when the player releases the crouch button
                    if (!crouch_pressed)
                    {
                        //Decrement the weight of the crouch
                        if (weight > 0f)
                        {
                            weight -= Time.deltaTime * crouch_weight;
                        }
                        animator.SetLayerWeight(crouch_layer_index, weight);
                    }
                }

                #region OG Jumping Anim Code
                // if (rb_vel.y > 0f)
                // {
                //    //If the player is able to jump then play the animations relevant to the velocity of the player
                //    if (!crouch_pressed && jump_pressed && ((rb_vel.x < -0.1 || rb_vel.x > 0.1) || (rb_vel.z < -0.1 || rb_vel.z > 0.1)) && !controller_ref.GetPushOrPull())
                //    {
                //        animator.SetBool("IsJumping", true);
                //        run_jump = true;
                //        stand_jump = false;

                //        j_timer_run = run_jump_time - 0.5f;

                //    }

                // }
                // else if(run_jump && is_grounded && ((rb_vel.x < -0.1 || rb_vel.x > 0.1) || (rb_vel.z < -0.1 || rb_vel.z > 0.1)))
                // {
                //    animator.SetBool("IsJumping", false);
                //    run_jump = false;
                // }
                
                // //Rigidody velocity will not be above the threshold
                // if(!crouch_pressed && jump_pressed && ((rb_vel.x >= -0.1 && rb_vel.x <= 0.1) || (rb_vel.z >= -0.1 && rb_vel.z <= 0.1)) && !controller_ref.GetPushOrPull())
                // {
                //    if(!currently_jumping)
                //    {
                //        currently_jumping = true;
                //        animator.SetBool("IsJumping", true);

                //        stand_jump = true;
                //        run_jump = false;

                //        j_timer_stand = stand_jump_time - 0.3f;
                //        StartCoroutine(StandJumpTimer());
                //    }
                // }

                // if (run_jump)
                // {
                //     stand_jump = false;

                //     j_timer_run -= Time.deltaTime;
                //     if(j_timer_run <= 0)
                //     {
                //         animator.SetBool("IsJumping", false);
                //         run_jump = false;
                //     }
                // }
                #endregion

                if(animator.GetBool("IsJumping"))
                {   
                    if(run_jump)
                    {
                        j_timer_run = run_jump_time - 0.5f;
                        run_jump = false;
                    }
                    
                    j_timer_run -= Time.deltaTime;
                    
                    if(j_timer_run <= 0)
                    {
                        animator.SetBool("IsJumping", false);
                        run_jump = false;

                        //We want the player to stop being able to jump further than what is realistic so after the animation ends, prevent further input from the player until they hit the ground
                    }
                }

                //Is the player falling? Play animation if they are
                if(!animator.GetBool("IsJumping") && !is_grounded)
                {
                    // StartCoroutine(DelayFallingAnim());
                    time_since_fallen += Time.deltaTime;
                    animator.SetBool("IsFalling", true);
                }
                else if(!animator.GetBool("IsJumping") && is_grounded)
                {
                    //Set the falling animation to stop playing
                    animator.SetBool("IsFalling", false);

                    if(time_since_fallen > hard_landing_threshold)
                    {
                        hard_land = true;
                        controller_ref.SetInput(true);
                        StartCoroutine(HardLandAnimation());
                    }

                    time_since_fallen = 0f;
                }

                //Pass in hash values, cheaper to use for updating the animator values
                animator.SetFloat(velZ_hash, velocityZ);
                animator.SetFloat(velX_hash, velocityX);
                animator.SetFloat(rb_velX_hash, rb_vel.x);
                animator.SetFloat(rb_velZ_hash, rb_vel.z);
        }
    }

    IEnumerator StandJumpTimer()
    {
        while(j_timer_stand > 0)
        {
            j_timer_stand -= Time.deltaTime;
            yield return null;
        }

        animator.SetBool("IsJumping", false);
        stand_jump = false;
        currently_jumping = false;
    }

    IEnumerator DelayFallingAnim()
    {
        yield return new WaitForSeconds(falling_delay_time * Time.deltaTime);
        animator.SetBool("IsFalling", true);
    }

    IEnumerator HardLandAnimation()
    {
        //StartCoroutine(DisableInput(hard_land_time));        
        animator.SetBool("HardLand", true);
        yield return new WaitForSeconds((hard_land_time / 1.5f) - 0.4f);
        animator.SetBool("HardLand", false);
        
        controller_ref.SetInput(false);
    }

    private IEnumerator DisableInput(float duration)
    {
        input_disabled = true;
        hard_land = true;

        yield return new WaitForSeconds(duration);
        input_disabled = false;
    }

    public void FreezeAnimation()
    {
        info = animator.GetCurrentAnimatorClipInfo(0);

        animator.speed = 0f;
    }

    public void UnfreezeAnimation()
    {
        animator.speed = 1f;
    }

    public bool GetHardLanding()
    {
        return hard_land;
    }

    public void SetHardLanding(bool land)
    {
        hard_land = land;
    }

    public void SetJump(bool val)
    {
        run_jump = val;
    }

    public float GetHardLandAnimTime()
    {
        return hard_land_time;
    }

    public float GetRunJumpAnimTime()
    {
        return run_jump_time;
    }

    public float GetStandJumpAnimTime()
    {
        return stand_jump_time;
    }
}

/*Cal's code ends here*/
