using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cal's code starts here*/
public class ThirdPersonCam : MonoBehaviour
{
    public Transform orientation;
    public Transform player;
    public Transform player_obj;
    public Rigidbody rb;
    public NIThirdPersonController p;

    private bool lock_rotation = false;

    public float ground_rot_speed;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Rotate orientation of camera
        Vector3 view_dir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = view_dir.normalized;
            
        //Rotate the player
        if(p.GetIsClimbing())
        {
            ground_rot_speed = 0;
        }
        else if (!p.GetIsClimbing())
        {
            ground_rot_speed = 15;
        }

        if(p.GetPushOrPull())
        {
            ground_rot_speed = 0f;
        }
        else if(!p.GetPushOrPull())
        {
            ground_rot_speed = 15f;
        }        
        
        Vector2 look_at = p.GetPlayerInput();
        if(!lock_rotation)
        {

            Vector3 input_dir = orientation.forward * look_at.y + orientation.right * look_at.x;
            player_obj.forward = Vector3.Slerp(player_obj.forward, input_dir.normalized, Time.deltaTime * ground_rot_speed);
        }

    }

    public void SetRotation(float rot)
    {
        ground_rot_speed = rot;
    }

    public void ResetRotation()
    {
        ground_rot_speed = 15f;
    }

    public void setLockRotation(bool val)
    {
        lock_rotation = val;
    }
}

/*Cal's code ends here*/