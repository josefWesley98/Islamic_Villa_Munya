using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    float velocityZ = 0f;
    float velocityX = 0f;
    public float acceleration = 7f;
    public float deceleration = 5f;
    public float max_walk_vel = 0.5f;
    public float max_run_vel = 2f;
    int velZ_hash;
    int velX_hash;
    private int crouch_layer_index;
    private int jump_layer_index;
    private int base_layer_index;
    private float crouch_weight = 6f;
    private float weight = 0f;
    public float jump_time = 0f;
    private float j_timer = 0f;
    bool is_jumping = false;

    // Start is called before the first frame update
    void Start()
    {
        //Search for the animator component attached to this object
        animator = GetComponent<Animator>();
        velZ_hash = Animator.StringToHash("VelocityZ");
        velX_hash = Animator.StringToHash("VelocityX");

        //Referencing the layer for crouching
        crouch_layer_index = animator.GetLayerIndex("Crouching");
        jump_layer_index = animator.GetLayerIndex("Jumping");
        base_layer_index = animator.GetLayerIndex("Base Layer");

        UpdateAnimTimes();
    }

    //Handles acceleration and deceleration
    void ChangeVel(bool forward_pressed, bool backward_pressed, bool left_pressed, bool right_pressed, bool run_pressed, bool crouch_pressed, float current_max_vel)
    {
        //2D Blend trees example begins here
        //If player presses forward, increase vel in z direction
        if(forward_pressed && velocityZ < current_max_vel)
        {
            velocityZ += Time.deltaTime * acceleration;
        }

        //If the player presses walk backwards, decrease the vel in the z direction
        if(backward_pressed && velocityZ < current_max_vel)
        {
            velocityZ -= Time.deltaTime * acceleration;
        }

        //If the player presses left, increase vel in left dir
        if(left_pressed && velocityX > -current_max_vel)
        {
            velocityX -= Time.deltaTime * acceleration;
        }

        //If player moves right, increase vel in right dir
        if(right_pressed && velocityX < current_max_vel)
        {
            velocityX += Time.deltaTime * acceleration;
        }

        //Decrease velocityZ
        if(!forward_pressed && velocityZ > 0f)
        {
            velocityZ -= Time.deltaTime * deceleration;
        }

        //Increase the velocity when not moving backwards to 0
        if(!backward_pressed && velocityZ < 0f)
        {
            velocityX += Time.deltaTime * deceleration;
        }

        //Increase velocityX if left is not pressed and velocityX < 0
        if(!left_pressed && velocityX < 0f)
        {
            velocityX += Time.deltaTime * deceleration;
        }

        //Decrease velocityX if right is not pressed and velocityX > 0
        if(!right_pressed && velocityX > 0f)
        {
            velocityX -= Time.deltaTime * deceleration;
        }

        //Reset the crouch weight to 0
        if(crouch_pressed && weight > 1f)
        {
            weight = 1f;
        }

        if(!crouch_pressed && weight < 0f)
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

    //Function to get the anim times for certain animations to play through to completion without interruption
    void UpdateAnimTimes()
    {
        //Create an array to store the player animations which we will loop through
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        //Foreach loop to loop through the anims until we get to jumping
        foreach(AnimationClip clip in clips)
        {
            //If we reach the jumping animation, get the length and store this in the jump_time variable
            if(clip.name == "H_Jumping2")
            {
                jump_time = clip.length;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        //Variables - Player Control
        //Get key input
        bool forward_pressed = Input.GetKey(KeyCode.W);
        bool left_pressed = Input.GetKey(KeyCode.A);
        bool right_pressed = Input.GetKey(KeyCode.D);
        bool run_pressed = Input.GetKey(KeyCode.LeftShift);
        bool backward_pressed = Input.GetKey(KeyCode.S);
        bool crouch_pressed = Input.GetKey(KeyCode.LeftControl);
        bool jump_pressed = Input.GetKey(KeyCode.Space);

        //Set current max_vel
        float current_max_vel = run_pressed ? max_run_vel : max_walk_vel;

        //Handle changes in velocity
        ChangeVel(forward_pressed, backward_pressed, left_pressed, right_pressed, run_pressed, crouch_pressed, current_max_vel);
        LockOrResetVel(forward_pressed, backward_pressed, left_pressed, right_pressed, run_pressed, crouch_pressed, current_max_vel);

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

        if(!crouch_pressed && jump_pressed)
        {
            animator.SetBool("IsJumping", true);

            is_jumping = true;

            j_timer = jump_time - 0.3f;
        }

        if(is_jumping)
        {
            j_timer -= Time.deltaTime;

            if(j_timer <= 0)
            {
                animator.SetBool("IsJumping", false);
                is_jumping = false;
                j_timer = jump_time - 0.3f;
            }
        }

        animator.SetFloat(velZ_hash, velocityZ);
        animator.SetFloat(velX_hash, velocityX);
    }
}
