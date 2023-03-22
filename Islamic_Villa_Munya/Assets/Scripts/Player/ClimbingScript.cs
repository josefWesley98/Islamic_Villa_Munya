using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClimbingScript : MonoBehaviour
{
    PlayerControls playercontrols;
    InputAction jump;
    InputAction climb;
    InputAction stopClimb;
    InputAction movement;
    [SerializeField] private Rigidbody rb;

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

    private Vector3 playerLerpStart = Vector3.zero;
    private Vector3 endLerpPoint =  Vector3.zero;
    private float interpolateAmount = 0.0f;
    private bool startClimb = false;
    private bool arrived = false;
    private bool holding = false;
    private float climbOffset = 0.0f;
    private bool[] lookDirection = new bool[4]{false, false, false, false};
    private bool isConnectedToWall = false;
    private bool detach = true;
    private float yRotForWall = 0.0f;
    private float interpolateWallRot = 0.0f;
    private bool startWallRot = true;
    private Vector3 direction = Vector3.zero;
    private Vector3 target = Vector3.zero;
    //private Quaternion slerpStart = Quaternion.identity;
    //private Quaternion lookRotation = Quaternion.identity;
    private void Awake() => playercontrols = new PlayerControls();
    

    // rotate to wall variables
    float slerpPercent = 0.0f;
    Quaternion lookRotation = Quaternion.identity;
    Vector3 rotationDirection = Vector3.zero;
    Quaternion slerpStart = Quaternion.identity;
    float slerpSpeed = 25.0f;
    bool doRotate = false;
    bool startedRotation = false;

    private void OnEnable()
    {
        playercontrols.Enable();
    
        movement = playercontrols.Player.Move;
        movement.Enable();

        climb = playercontrols.Player.DoClimb;
        climb.Enable();
        climb.started += DoClimbing;

        stopClimb = playercontrols.Player.Drop;
        stopClimb.Enable();
        stopClimb.started += StopClimb;
    }

    private void OnDisable()
    {
        playercontrols.Disable();
        climb.Disable();
        stopClimb.Disable();
    }
    // Update is called once per frame
    void Update()
    {
        if(startClimb)
        {
            Vector3 direction = upwardClimbing.GetWallPosition() - transform.localPosition;
        
            // Set the y component to 0 to ensure only rotation around the y-axis
            direction.y = 0;
            
            // Calculate the target rotation using Quaternion.LookRotation
            Quaternion targetRotation = Quaternion.LookRotation(-direction.normalized);

            if(startClimb && transform.localRotation != targetRotation)
            {
                transform.localRotation = targetRotation;
                Debug.Log("rotating.");
            }
            else if(startClimb && transform.localRotation == targetRotation)
            {
                Debug.Log("is not rotating.");
            }
        }

        // forward
        if(transform.eulerAngles.y >= 315 || transform.eulerAngles.y < 45f)
        {
            lookDirection[0] = true;
            lookDirection[1] = false;
            lookDirection[2] = false;
            lookDirection[3] = false;
            climbOffset = -0.2f;
            upwardClimbing.SetDetectionRadius(new Vector3(1f,1f,0.5f));
            downwardClimbing.SetDetectionRadius(new Vector3(1f,1f,0.5f));
        }
        // right
        if(transform.eulerAngles.y >= 45f && transform.eulerAngles.y < 135f )
        {
            climbOffset = -0.2f;
            lookDirection[0] = false;
            lookDirection[1] = true;
            lookDirection[2] = false;
            lookDirection[3] = false;
            upwardClimbing.SetDetectionRadius(new Vector3(0.5f,1f,1f));
            downwardClimbing.SetDetectionRadius(new Vector3(0.5f,1f,1f));
        }
        // backwards
        if(transform.eulerAngles.y >= 135f && transform.eulerAngles.y < 225f )
        {
            climbOffset = 0.2f;
            lookDirection[0] = false;
            lookDirection[1] = false;
            lookDirection[2] = true;
            lookDirection[3] = false;
            upwardClimbing.SetDetectionRadius(new Vector3(1f,1f,0.5f));
            downwardClimbing.SetDetectionRadius(new Vector3(1f,1f,0.5f));
        }
        // left
        if(transform.eulerAngles.y >= 225 && transform.eulerAngles.y < 315)
        {
            climbOffset = 0.2f;
            lookDirection[0] = false;
            lookDirection[1] = false;
            lookDirection[2] = false;
            lookDirection[3] = true;
            upwardClimbing.SetDetectionRadius(new Vector3(0.5f,1f,1f));
            downwardClimbing.SetDetectionRadius(new Vector3(0.5f,1f,1f));
        }
        
        if(!upwardClimbing.GetCanClimb())
        {
            startClimb = false;
            moveController.SetIsClimbing(false);
        }
        if(upwardClimbing.GetCanClimb() && startClimb)
        {
            moveController.SetIsClimbing(true);
            LerpFunction();
        }

        if(!startClimb)
        {
            rb.useGravity = true;
        }

        if(arrived && startClimb )
        {
            arrived = false;
            GetNextClimbSpot();
        }  
    }    

    private void StopClimb(InputAction.CallbackContext context)
    {
        if(startClimb)
        {
            rb.AddForce(-transform.forward * 100, ForceMode.Impulse);
        }
        moveController.enabled = true;
        startClimb = false;
        rb.useGravity = true;
        isConnectedToWall = false;
        detach = true;
        moveController.SetIsClimbing(false);
        
    }
    private void DoClimbing(InputAction.CallbackContext context)
    {
        if(upwardClimbing.GetCanClimb())
        {  
            isConnectedToWall = true;
            //Debug.Log("starting the climb.");
            startClimb = true;
            arrived = false;
            GetNextClimbSpot();
            rb.useGravity = false;
            upwardClimbing.SetMoveRightArm(true);
            detach = false;
        }
    }
    
    private void GetNextClimbSpot()
    {
       
        playerLerpStart = transform.position;

        if(upwardClimbing.GetMovingDownwards() || upwardClimbing.GetMovingDirecionally())
        {
            Vector3 newCentrePos = Vector3.zero;
            newCentrePos = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
            playerLerpStart = newCentrePos;
        }
       
        endLerpPoint = upwardClimbing.GetNewMiddleSpot();
        
        if(lookDirection[0] || lookDirection[2])
        {
            endLerpPoint = new Vector3(endLerpPoint.x, endLerpPoint.y-0.3f, endLerpPoint.z + climbOffset);
            //Debug.Log(" looking forward or back");
            if(upwardClimbing.GetMovingDownwards())
            {
                endLerpPoint = new Vector3(endLerpPoint.x, endLerpPoint.y -0.6f, endLerpPoint.z + climbOffset);
                
            }
            if(upwardClimbing.GetMovingDirecionally())
            {
                endLerpPoint = new Vector3(endLerpPoint.x, endLerpPoint.y -0.15f, endLerpPoint.z + climbOffset);
               
            }
            //Debug.Log(" looking forward or back");
        }
        if(lookDirection[1] || lookDirection[3])
        {
             endLerpPoint = new Vector3(endLerpPoint.x + climbOffset, endLerpPoint.y-0.3f, endLerpPoint.z);
            //Debug.Log(" looking left or right");
           if(upwardClimbing.GetMovingDownwards())
            {
                endLerpPoint = new Vector3(endLerpPoint.x, endLerpPoint.y -0.6f, endLerpPoint.z + climbOffset);
            
            }
            if(upwardClimbing.GetMovingDirecionally())
            {
                endLerpPoint = new Vector3(endLerpPoint.x, endLerpPoint.y -0.2f, endLerpPoint.z + climbOffset);

            }
            //Debug.Log(" looking left or right");
        }
       
    }
    private void LerpFunction()
    {
        if(startClimb && !arrived)
        {
            

            if(upwardClimbing.GetMovementDirection().y != 0 || upwardClimbing.GetMovementDirection().x != 0)
            {
                if(!upwardClimbing.GetMovingDirecionally())
                {
                    interpolateAmount = (interpolateAmount + Time.deltaTime * 0.55f);
                }
                if(upwardClimbing.GetMovingDirecionally())
                {
                    interpolateAmount = (interpolateAmount + Time.deltaTime * 0.75f);
                }
            }
            
            transform.position = Vector3.Lerp(playerLerpStart, endLerpPoint, interpolateAmount);
          
            if(interpolateAmount >= 1.0f)
            {
                interpolateAmount = 0.0f;
                arrived  = true;
            }
        }
    }
    public void SetNewMovement(Vector3 _newEndPoint)
	{

        playerLerpStart = transform.position;
        endLerpPoint = _newEndPoint;

        if(lookDirection[0] || lookDirection[2])
        {
            endLerpPoint = new Vector3(endLerpPoint.x, endLerpPoint.y-0.3f, endLerpPoint.z + climbOffset);
            //Debug.Log(" looking forward or back");
            if(upwardClimbing.GetMovingDownwards())
            {
                endLerpPoint = new Vector3(endLerpPoint.x, endLerpPoint.y -0.4f, endLerpPoint.z + climbOffset);
            }
            if(upwardClimbing.GetMovingDirecionally())
            {
                endLerpPoint = new Vector3(endLerpPoint.x, endLerpPoint.y -0.2f, endLerpPoint.z + climbOffset);
            }
        }

        if(lookDirection[1] || lookDirection[3])
        {
            endLerpPoint = new Vector3(endLerpPoint.x + climbOffset, endLerpPoint.y-0.3f, endLerpPoint.z);
            //Debug.Log(" looking left or right");
           if(upwardClimbing.GetMovingDownwards())
            {
                endLerpPoint = new Vector3(endLerpPoint.x, endLerpPoint.y -0.4f, endLerpPoint.z + climbOffset);
            
            }
            if(upwardClimbing.GetMovingDirecionally())
            {
                endLerpPoint = new Vector3(endLerpPoint.x, endLerpPoint.y -0.2f, endLerpPoint.z + climbOffset);
            }
        }
        //endLerpPoint.position = new Vector3(endLerpPoint.position.x - 0.15f , endLerpPoint.position.y , endLerpPoint.position.z);
        interpolateAmount = 0.0f;
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
    public bool GetDetach()
    {
        return detach;
    }
}
