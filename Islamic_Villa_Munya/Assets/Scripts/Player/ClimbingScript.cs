using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClimbingScript : MonoBehaviour
{
    PlayerControls playercontrols;
    InputAction jump;
    InputAction climb;
    InputAction movement;
    Rigidbody rb;
    private MovementController movementController;
    [SerializeField] private NIThirdPersonController moveController;
    [SerializeField] private UpwardClimbing upwardClimbing;
    [SerializeField] private DownwardClimbing downwardClimbing;
    [SerializeField] private Transform centreMass;
    [SerializeField] private Transform playerPos;
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
    private float climbOffset = 0.0f;
    private bool[] lookDirection = new bool[4]{false, false, false, false};
    private bool isConnectedToWall = false;
    private void Awake() => playercontrols = new PlayerControls();

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        movementController = GetComponent<MovementController>();
    }
    private void OnEnable()
    {
        playercontrols.Enable();
        
        jump = playercontrols.Player.Jump;
        jump.Enable();
        jump.started += Jumping;
        jump.performed += Holding;

        movement = playercontrols.Player.Move;
        movement.Enable();

        climb = playercontrols.Player.DoClimb;
        climb.Enable();
        climb.performed += Climb;
    }

    private void OnDisable()
    {
        playercontrols.Disable();
        jump.Disable();
        climb.Disable();
    }
    // Update is called once per frame
    void Update()
    {

        if(transform.eulerAngles.y >= 315 || transform.eulerAngles.y < 45f)
        {
            lookDirection[0] = true;
            lookDirection[1] = false;
            lookDirection[2] = false;
            lookDirection[3] = false;
            climbOffset = -0.2f;
        }
     
        if(transform.eulerAngles.y >= 45f && transform.eulerAngles.y < 135f )
        {
            climbOffset = -0.2f;
            lookDirection[0] = false;
            lookDirection[1] = true;
            lookDirection[2] = false;
            lookDirection[3] = false;
        }

        if(transform.eulerAngles.y >= 135f && transform.eulerAngles.y < 225f )
        {
            climbOffset = 0.2f;
            lookDirection[0] = false;
            lookDirection[1] = false;
            lookDirection[2] = true;
            lookDirection[3] = false;
        }

        if(transform.eulerAngles.y >= 225 && transform.eulerAngles.y < 315)
        {
            climbOffset = 0.2f;
            lookDirection[0] = false;
            lookDirection[1] = false;
            lookDirection[2] = false;
            lookDirection[3] = true;
        }

        isJumping = true;
        LerpFunction();

        if(!upwardClimbing.GetCanClimb())
        {
            startClimb = false;
            moveController.SetIsClimbing(false);
        }
        if(upwardClimbing.GetCanClimb() && startClimb)
        {
            moveController.SetIsClimbing(true);
        }
        if(!startClimb)
        {
            rb.useGravity = true;
            movementController.enabled = true;
        }
        if(arrived && startClimb )
        {
            arrived = false;
            GetNextClimbSpot();
        }  
    }    
    private void Jumping(InputAction.CallbackContext context)
    {
        if(isJumping)
        {
            Debug.Log("jumping");
            rb.AddForce(jumpForce * transform.up * 10.0f, ForceMode.Impulse);
        }
    }
    private void Climb(InputAction.CallbackContext context)
    {
        if(upwardClimbing.GetCanClimb())
        {
            isConnectedToWall = true;
            Debug.Log("starting the climb.");
            startClimb = true;
            arrived = false;
            GetNextClimbSpot();
            rb.useGravity = false;
            movementController.enabled = false;
            upwardClimbing.SetMoveRightArm(true);
        }
    }
    private void GetNextClimbSpot()
    {
        Debug.Log("finding new climbing spot.");
        playerLerpStart.position = centreMass.position;
        endLerpPoint.position = upwardClimbing.GetNewMiddleSpot();
        if(lookDirection[0] || lookDirection[2])
        {
            endLerpPoint.position = new Vector3(endLerpPoint.position.x, endLerpPoint.position.y -0.4f, endLerpPoint.position.z + climbOffset);
            Debug.Log(" looking forward or back");
        }
        if(lookDirection[1] || lookDirection[3])
        {
            endLerpPoint.position = new Vector3(endLerpPoint.position.x + climbOffset, endLerpPoint.position.y -0.4f, endLerpPoint.position.z);
            Debug.Log(" looking left or right");
        }
       
    }
    private void LerpFunction()
    {
        if(startClimb && !arrived)
        {
            if(upwardClimbing.GetMovementDirection().y != 0 || upwardClimbing.GetMovementDirection().x != 0)
            {
                interpolateAmount = (interpolateAmount + Time.deltaTime * 0.35f);
            }
            
            playerPos.position = Vector3.Lerp(playerLerpStart.position, endLerpPoint.position, interpolateAmount);
          
            if(interpolateAmount >= 1.0f)
            {
                interpolateAmount = 0.0f;
                arrived  = true;
            }
        }
    }
    public void SetNewMovement(Vector3 _newEndPoint)
    {
        playerLerpStart.position = centreMass.position;
        endLerpPoint.position = _newEndPoint;
        if(lookDirection[0] || lookDirection[2])
        {
            endLerpPoint.position = new Vector3(endLerpPoint.position.x, endLerpPoint.position.y -0.4f, endLerpPoint.position.z + climbOffset);
            Debug.Log(" looking forward or back");
        }
        if(lookDirection[1] || lookDirection[3])
        {
            endLerpPoint.position = new Vector3(endLerpPoint.position.x + climbOffset, endLerpPoint.position.y -0.4f, endLerpPoint.position.z);
            Debug.Log(" looking left or right");
        }
        //endLerpPoint.position = new Vector3(endLerpPoint.position.x - 0.15f , endLerpPoint.position.y , endLerpPoint.position.z);
        interpolateAmount = 0.0f;
    }
    private void Holding(InputAction.CallbackContext context)
    {
        if(arrived)
        {
            rb.useGravity = false;
            holding = true;
        }
    }
    public float GetMovementDirectionY()
    {
        Vector2 moveDirection = movement.ReadValue<Vector2>();
     //   Debug.Log("move direction y = " + moveDirection.y);
        return moveDirection.y;
    }
     public float GetMovementDirectionX()
    {
        Vector2 moveDirection = movement.ReadValue<Vector2>();
//        Debug.Log("move direction x = " + moveDirection.x);
        return moveDirection.x;
    }
    private void CanceledHold(InputAction.CallbackContext context)
    {
        holding = false;
        arrived = false;
    }
    public bool GetIsConnectedToWall()
    {
        return isConnectedToWall;
    }
    public bool GetLookDirection(int i)
    {
        return lookDirection[i];
    }
}
