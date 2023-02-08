using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClimbingScript : MonoBehaviour
{
    PlayerControls playercontrols;
    InputAction jump;
    Rigidbody rb;

    private bool isClimbing = false;
    private bool isJumping = false;
    private Vector3 climbPoint = new Vector3(0,0,0);
    
    [SerializeField] Vector3 centre;
    [SerializeField] float radius;
    [SerializeField] LayerMask lm;
    [SerializeField] float jumpForce;

    private void Awake() => playercontrols = new PlayerControls();

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        playercontrols.Enable();
        
        jump = playercontrols.Player.Jump;
        jump.Enable();
        jump.performed += Jumping;
    }

    private void OnDisable()
    {
        playercontrols.Disable();
        jump.Disable();
    }
    // Update is called once per frame
    void Update()
    {
        Climb(centre, radius);
        //isClimbing = false;
        isJumping = true;
        
    }

    void Climb(Vector3 center, float radius)
    {
        Debug.Log("starting climb function");
        Collider[] climabeObjects = Physics.OverlapSphere(center, radius, lm);
        
        float[] order;

        if(climabeObjects != null)
        {
            isClimbing = true;
            isJumping = false;
            foreach(var climabeObject in climabeObjects)
            {
                // find the nearest spot that isnt currently in use.

                // make climbpoint = the nearest point.
                climbPoint = climabeObject.transform.position;
                Debug.Log("climbing detected");
            }
        }
        else
        {
            isJumping = true;
            Debug.Log("no climbing spots detected");
        }
        
    }
    
    private void Jumping(InputAction.CallbackContext context)
    {
        if(isJumping)
        {
            Debug.Log("jumping");
            rb.AddForce(jumpForce * transform.up * 10.0f, ForceMode.Impulse);
        }
        if(isClimbing && climbPoint != new Vector3(0,0,0))
        {
            Debug.Log("climbing");
            transform.position = climbPoint;
            isClimbing = false;
            climbPoint = new Vector3(0,0,0);
        }
    }
}
