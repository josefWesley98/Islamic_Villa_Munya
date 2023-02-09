using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    int is_walking_hash;
    int is_running_hash;
    int is_walking_back_hash;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        is_walking_hash = Animator.StringToHash("IsWalking?");
        is_running_hash = Animator.StringToHash("IsRunning?");
        is_walking_back_hash = Animator.StringToHash("IsWalkingBack?");
    }

    // Update is called once per frame
    void Update()
    {
        //Variables - Player Control
        bool forward_pressed = Input.GetKey("w");
        bool left_pressed = Input.GetKey("a");
        bool right_pressed = Input.GetKey("d");
        bool run_pressed = Input.GetKey("left shift");
        bool backward_pressed = Input.GetKey("s");

        //Variables - Player Animation States
        bool is_walking = animator.GetBool(is_walking_hash);
        bool is_running = animator.GetBool(is_running_hash);
        bool is_walking_back = animator.GetBool(is_walking_back_hash);

        //Capture the player input
        //If the player is not walking and forward/left/right
        if(!is_walking && (forward_pressed || left_pressed || right_pressed || backward_pressed))
        {
            //Set the iswalking bool to true
            animator.SetBool(is_walking_hash, true);
        }

        //If the player is walking and not pressing forward/left/right
        if(is_walking && (!forward_pressed && !left_pressed && !right_pressed && !backward_pressed))
        {
            //Set the iswalking bool to true
            animator.SetBool(is_walking_hash, false);
        }

        //If the player is pressing forward and the run button
        if(!is_running && (forward_pressed && run_pressed))
        {
            //Set the running state to true
            animator.SetBool(is_running_hash, true);
        }

        //If the player is not pressing running or forward
        if(is_running && (!forward_pressed || !run_pressed))
        {
            //Set the running state to false
            animator.SetBool(is_running_hash, false);
        }

        ////If the player is pressing backwards
        // if(!is_walking_back && (backward_pressed))
        // {
        //     //Set the walking backwards state to being true
        //     animator.SetBool(is_walking_back_hash, true);
        // }

        // //If the player is not pressing backwards
        // if(is_walking_back && (!backward_pressed))
        // {
        //     //Set the walking backward state to false
        //     animator.SetBool(is_walking_back_hash, false);
        // }
    }
}
