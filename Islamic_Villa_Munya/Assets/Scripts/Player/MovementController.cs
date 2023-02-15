using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private Transform cam;

    [SerializeField] private float walk_speed = 1f;
    [SerializeField] private float run_speed  =3f;
    [SerializeField] private float smooth_time = 0.2f;
    [SerializeField] private float walk_multiplier = 2.0f;
    [SerializeField] private float run_multiplier = 4.0f;
    private float turn_smooth_vel;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {

        //Make cursor disappear while playing game
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        //Capture the user key input
        //Determine speed depending on whether run key is pressed
        bool run_pressed = Input.GetKey(KeyCode.LeftShift);

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //Get the direction
        Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;

        if(dir.magnitude >= 0.1f)
        {
            //Returns angle from x axis and a vector starting at 0 and ending at x,y
            float target_angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target_angle, ref turn_smooth_vel, smooth_time);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //If the player is walking
            if(!run_pressed)
            {
                Vector3 move_dir = Quaternion.Euler(0f, target_angle, 0f) * Vector3.forward;
                rb.AddForce(move_dir.normalized * walk_speed * walk_multiplier , ForceMode.Acceleration);
            }

            //If the player is running, increase the speed
            if(run_pressed)
            {
                Vector3 move_dir = Quaternion.Euler(0f, target_angle, 0f) * Vector3.forward;
                rb.AddForce(move_dir.normalized * run_speed * run_multiplier, ForceMode.Acceleration);
            }
        }
    }
}
