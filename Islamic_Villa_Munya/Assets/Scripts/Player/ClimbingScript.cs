using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClimbingScript : MonoBehaviour
{
    PlayerControls playercontrols;
    InputAction jump;
    Rigidbody rb;

    [SerializeField] private UpwardClimbing upwardClimbing
     [SerializeField] private DownwardClimbing downwardClimbing;

    private bool isClimbing = false;
    private bool isJumping = false;
    private Vector3 climbPoint = new Vector3(0,0,0);
    
    [SerializeField] Vector3 centre;
    [SerializeField] float radius;
    [SerializeField] LayerMask lm;
    [SerializeField] float jumpForce;

    [SerializeField] private Transform playerLerpStart;
    [SerializeField] private Transform endLerpPoint;
    private float interpolateAmount = 0.0f;
    private bool startClimb = false;
    private bool arrived = false;
    private bool holding = false;

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
        jump.started += Jumping;
        jump.performed += Holding;
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
        LerpFunction();

        if(!arrived && !holding)
        {
            rb.useGravity = true;
        }
        if(jump.ReadValue<float>() == 0)
        {
            arrived = false;
            holding = false;
        }
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
            playerLerpStart.transform.position = transform.position;
            endLerpPoint.position = climbPoint;
            endLerpPoint.position = new Vector3(endLerpPoint.position.x, endLerpPoint.position.y - 0.85f, endLerpPoint.position.z-0.25f);
            startClimb = true;
            isClimbing = false;
            climbPoint = new Vector3(0,0,0);
            arrived = false;
            holding = false;
        }
    }

    private void LerpFunction()
    {
        if(startClimb)
        {
            interpolateAmount = (interpolateAmount + Time.deltaTime*1.5f);
            transform.position = Vector3.Lerp(playerLerpStart.position, endLerpPoint.position, interpolateAmount);
            if(interpolateAmount >= 1)
            {
                startClimb = false;
                interpolateAmount = 0.0f;
               
                arrived  = true;
            }
        }
    }
    private void Holding(InputAction.CallbackContext context)
    {
        if(arrived)
        {
            rb.useGravity = false;
            holding = true;
        }
    }

    private void CanceledHold(InputAction.CallbackContext context)
    {
        holding = false;
        arrived = false;
    }
    
}
