using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    public Transform orientation;
    public Transform player;
    public Transform player_obj;
    public Rigidbody rb;
    public ThirdPersonController p;

    public float ground_rot_speed;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        //Rotate orientation of camera
        Vector3 view_dir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = view_dir.normalized;

        //Rotate the player
        Vector2 look_at = p.GetPlayerInput();
        Vector3 input_dir = orientation.forward * look_at.y + orientation.right * look_at.x;

        player_obj.forward = Vector3.Slerp(player_obj.forward, input_dir.normalized, Time.deltaTime * ground_rot_speed);

    }

}