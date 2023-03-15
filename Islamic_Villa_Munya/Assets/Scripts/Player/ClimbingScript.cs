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
    Rigidbody rb;

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
        jump.Disable();
        stopClimb.Disable();
    }
    // Update is called once per frame
    void Update()
    {
     
        //if(upwardClimbing.GetWallPosition() != Vector3.zero)
		//{
		//	Vector3 targetDir = upwardClimbing.GetWallPosition() - transform.position;
		//	Quaternion targetRotation = Quaternion.LookRotation(targetDir);
		//	transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 50.0f * Time.deltaTime);			
	    //}
		
	    // Get the target direction
	    // Vector3 targetDir = upwardClimbing.GetWallPosition() - transform.position;

	    // // Get the rotation to face the target direction
	    // Quaternion targetRot = Quaternion.LookRotation(targetDir);

	    // // Slerp the object's rotation towards the target rotation over time
	    // float rotateSpeed = 10.0f; // Adjust as needed
	    // transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
	    // Get the target direction
        // Vector3 targetDir = upwardClimbing.GetWallPosition() - transform.position;

        // // Calculate the target rotation
        // Quaternion targetRotation = Quaternion.LookRotation(targetDir);

        // // Keep the current x and z rotations, only update the y rotation
        // targetRotation.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, targetRotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        // // Slerp the rotation to the target rotation over time
        // float rotationSpeed = 20.0f;
        // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        // Get the position of the target object
        if(startClimb && doRotate)
        {
            if(!startedRotation)
            {
                slerpStart = gameObject.transform.rotation;
                startedRotation = true;
            }
            Debug.Log("i am rotating on wall");
            Vector3 rotationTarget = upwardClimbing.GetWallPosition();
            rotationTarget.y = transform.localPosition.y;

            rotationDirection = (rotationTarget - transform.position).normalized;
 
            lookRotation = Quaternion.LookRotation(rotationDirection);
            
            slerpPercent = Mathf.MoveTowards(slerpPercent, 1f, Time.deltaTime * slerpSpeed);

            transform.rotation = Quaternion.Slerp(slerpStart, lookRotation, slerpPercent);
            
            if(slerpPercent >= 1.0f)
            {
                doRotate = false;
                startedRotation = false;
                slerpPercent = 0f;
                direction = Vector3.zero;
                lookRotation = Quaternion.identity;
           
            }
            // Vector3 targetPosition = upwardClimbing.GetWallPosition();
            // targetPosition.y = transform.position.y;
            // // Set the target's y position to be the same as the current object's y position
            // Rotate towards the target position
            // // //transform.LookAt(targetPosition);
            // // Vector3 direction = upwardClimbing.GetWallPosition() - transform.position;
            // // //direction.y = 0f; // Set y to zero to only rotate on the y-axis
            // // Quaternion rotation = Quaternion.LookRotation(direction);
            // // transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 25.0f * Time.deltaTime);
        }

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
        //Debug.Log("finding new climbing spot.");
        Vector3 rotationTarget = upwardClimbing.GetWallPosition();
        rotationTarget.y = transform.localPosition.y;
        Vector3 targetDir = rotationTarget - transform.localPosition;
        float angle = Vector3.Angle(transform.forward, targetDir);
   
        if(angle < -25 || angle > 25)
        {
            doRotate = true;
            slerpPercent = 0.0f;
            startedRotation = false;
            direction = Vector3.zero;
            lookRotation = Quaternion.identity;
            Debug.Log("this happend cals face");
        }
        playerLerpStart = centreMass.position;
        endLerpPoint = upwardClimbing.GetNewMiddleSpot();
        if(lookDirection[0] || lookDirection[2])
        {
            endLerpPoint = new Vector3(endLerpPoint.x, endLerpPoint.y -0.4f, endLerpPoint.z + climbOffset);
            //Debug.Log(" looking forward or back");
        }
        if(lookDirection[1] || lookDirection[3])
        {
            endLerpPoint = new Vector3(endLerpPoint.x + climbOffset, endLerpPoint.y -0.4f, endLerpPoint.z);
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
                    interpolateAmount = (interpolateAmount + Time.deltaTime * 0.35f);
                }
                if(upwardClimbing.GetMovingDirecionally())
                {
                    interpolateAmount = (interpolateAmount + Time.deltaTime * 0.75f);
                }
            }
            
            playerPos.position = Vector3.Lerp(playerLerpStart, endLerpPoint, interpolateAmount);
          
            if(interpolateAmount >= 1.0f)
            {
                interpolateAmount = 0.0f;
                arrived  = true;
            }
        }
    }
    public void SetNewMovement(Vector3 _newEndPoint)
	{
    	
		// do check for if you are rotated to the wall.
		
		
		//transform.rotation = Quaternion.Euler()
		 Vector3 rotationTarget = upwardClimbing.GetWallPosition();
        rotationTarget.y = transform.localPosition.y;
        Vector3 targetDir = rotationTarget - transform.localPosition;
        float angle = Vector3.Angle(transform.forward, targetDir);

        if(angle < -25 || angle > 25)
        {
            doRotate = true;
            slerpPercent = 0.0f;
            startedRotation = false;
            direction = Vector3.zero;
            lookRotation = Quaternion.identity;
            Debug.Log("this happend 2 cals face");
        }
        playerLerpStart = centreMass.position;
        endLerpPoint = _newEndPoint;
        if(lookDirection[0] || lookDirection[2])
        {
            endLerpPoint = new Vector3(endLerpPoint.x, endLerpPoint.y, endLerpPoint.z + climbOffset);
            //Debug.Log(" looking forward or back");
        }
        if(lookDirection[1] || lookDirection[3])
        {
            endLerpPoint = new Vector3(endLerpPoint.x + climbOffset, endLerpPoint.y, endLerpPoint.z);
            //Debug.Log(" looking left or right");
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
    public bool GetDetach()
    {
        return detach;
    }
}
