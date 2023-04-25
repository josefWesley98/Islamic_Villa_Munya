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

    [Header("Scripts and Rigidbody")]
    [SerializeField] private NIThirdPersonController moveController;
    [SerializeField] private UpwardClimbing upwardClimbing;
    [SerializeField] private DownwardClimbing downwardClimbing;
    [SerializeField] private Rigidbody rb;
    
    private Vector3 centre = Vector3.zero;
    private Vector3 playerLerpStart = Vector3.zero;
    private Vector3 endLerpPoint =  Vector3.zero;
    private Vector3 direction = Vector3.zero;
    private Vector3 target = Vector3.zero;
    
    private Quaternion lookRotation = Quaternion.identity;
    private Quaternion targetRotation = Quaternion.identity;

    private float radius;
    private float interpolateAmount = 0.0f;
    private float climbOffset = 0.0f;
    private float yRotForWall = 0.0f;
    private float interpolateWallRot = 0.0f;
    private float rotateCooldown = 1.0f;

    private bool[] lookDirection = new bool[4]{false, false, false, false};
    private bool isClimbing = false;
    private bool isJumping = false;
    private bool startClimb = false;
    private bool arrived = false;
    private bool holding = false;
    private bool isConnectedToWall = false;
    private bool detach = true;
    private bool startWallRot = true;
    private bool startRotating = false;

    private void Awake() => playercontrols = new PlayerControls();

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

        jump = playercontrols.Player.Jump;
        jump.Enable();

    }
    private void OnDisable()
    {
        playercontrols.Disable();
        climb.Disable();
        stopClimb.Disable();
        jump.Disable();
    }
    void Update()
    {
        DoJumpCheck();
        
        CanDoRotationCheck();

        CalculateDirection();

        ClimbingChecks();
    }    
    private void ClimbingChecks()
    {
        // checks if can do climb and if hand holds are available.
        if(!upwardClimbing.GetCanClimb())
        {
            startClimb = false;
            moveController.SetIsClimbing(false);
        }
        // starts the climbing.
        if(upwardClimbing.GetCanClimb() && startClimb)
        {
            moveController.SetIsClimbing(true);
            LerpFunction();
        }
        //resets gravity if climbing ends (redundency)
        if(!startClimb)
        {
            rb.useGravity = true;
        }
        //when arriving at current lerp target, get another.
        if(arrived && startClimb )
        {
            arrived = false;
            GetNextClimbSpot();
        }  
    }
    private void DoJumpCheck()
    {
        // checks if your jumping, if yes then you cant start climbing. (stops cheating in level to avoid puzzle.)
        if(jump.ReadValue<float>() > 0)
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }
    }
    private void CalculateDirection()
    {
        // forward
        // checks based on the rotation of the player what direction they are facing.
        // this will be the only commented if nest as they are all the same but with variable changes.
        if(transform.eulerAngles.y >= 315 || transform.eulerAngles.y < 45f)
        {
            if(!startClimb)
            {
                // sets the offset from the wall so they are face to face with the surface to make things more natural.
                climbOffset = -0.165f;
                // assign the direction and make all the others false.
                lookDirection[0] = true;
                lookDirection[1] = false;
                lookDirection[2] = false;
                lookDirection[3] = false;
                // change the detection radius of the box.
                upwardClimbing.SetDetectionRadius(new Vector3(1f,0.55f,0.5f));
                downwardClimbing.SetDetectionRadius(new Vector3(1f,0.55f,0.5f));
            }
        }
        // right
        if(transform.eulerAngles.y >= 45f && transform.eulerAngles.y < 135f )
        {
            if(!startClimb)
            {
                climbOffset = -0.165f;
                lookDirection[0] = false;
                lookDirection[1] = true;
                lookDirection[2] = false;
                lookDirection[3] = false;
                upwardClimbing.SetDetectionRadius(new Vector3(0.5f,0.55f,1f));
                downwardClimbing.SetDetectionRadius(new Vector3(0.5f,0.55f,1f));
            }
        }
        // backwards
        if(transform.eulerAngles.y >= 135f && transform.eulerAngles.y < 225f )
        {
            if(!startClimb)
            {
                climbOffset = 0.165f;
                lookDirection[0] = false;
                lookDirection[1] = false;
                lookDirection[2] = true;
                lookDirection[3] = false;
                upwardClimbing.SetDetectionRadius(new Vector3(1f,0.55f,0.5f));
                downwardClimbing.SetDetectionRadius(new Vector3(1f,0.55f,0.5f));
            }

        }
        // left
        if(transform.eulerAngles.y >= 225 && transform.eulerAngles.y < 315)
        {
            if(!startClimb)
            {
                climbOffset = 0.165f;
                lookDirection[0] = false;
                lookDirection[1] = false;
                lookDirection[2] = false;
                lookDirection[3] = true;
                upwardClimbing.SetDetectionRadius(new Vector3(0.5f,0.55f,1f));
                downwardClimbing.SetDetectionRadius(new Vector3(0.5f,0.55f,1f));
            }
        }
        
    }
    private void calculateRotationToWall()
    {
        //checks if the rotation cooldown is less than 0 so it does jitter by rotating 60+ times a second, aswell as if the player is infact climbing.
        // there is a version of these for each direction to help create an accurate rotation.
        // again only commenting this if nest becase they are all the same.
        if(startClimb && lookDirection[0] && rotateCooldown <= 0)
        {
            // gets the hand middle position and takes the current transform from that to get the direction.
            Vector3 direction = upwardClimbing.GetWallPosition() - transform.position;
            // make y = 0 so it only rotates on that axis.
            direction.y = 0;
            //use the look rotation function to use our vector3 direction to find the Quaternion equivelant.
            targetRotation = Quaternion.LookRotation(direction.normalized);

            //finally rotate the player to that angle and reset the cooldown.
            if(startClimb && transform.localRotation != targetRotation && !upwardClimbing.GetIsClimbingPaused())
            {
                transform.localRotation = targetRotation;
                rotateCooldown = 1.0f;
            }
            
        }
        if(startClimb && lookDirection[1] && rotateCooldown <= 0)
        {
            Vector3 direction = upwardClimbing.GetWallPosition() - transform.position;
            direction.y = 0;
            targetRotation = Quaternion.LookRotation(direction.normalized);

            if(startClimb && transform.localRotation != targetRotation && !upwardClimbing.GetIsClimbingPaused())
            {
                transform.localRotation = targetRotation;
                rotateCooldown = 1.0f;
            }
        }
        if(startClimb && lookDirection[2] && rotateCooldown <= 0)
        {
            Vector3 direction = upwardClimbing.GetWallPosition() - transform.position;
            direction.y = 0;
            targetRotation = Quaternion.LookRotation(direction.normalized);

            if(startClimb && transform.localRotation != targetRotation && !upwardClimbing.GetIsClimbingPaused())
            {
                transform.localRotation = targetRotation;
                rotateCooldown = 1.0f;
            }
        }
        if(startClimb && lookDirection[3] && rotateCooldown <= 0)
        {
            Vector3 direction = upwardClimbing.GetWallPosition() - transform.position;
            direction.y = 0;
            targetRotation = Quaternion.LookRotation(direction.normalized);

            if(startClimb && transform.localRotation != targetRotation && !upwardClimbing.GetIsClimbingPaused())
            {
                transform.localRotation = targetRotation;
                rotateCooldown = 1.0f;
            }
        }
    }
    private void CanDoRotationCheck()
    {
        // handles the cooldown function and rotation.
        rotateCooldown -= Time.deltaTime;

        if(startRotating)
        {
            calculateRotationToWall();
        }
    }
    private void StopClimb(InputAction.CallbackContext context)
    {
        // ends the climbing and resets values and reactivates scripts.
        moveController.enabled = true;
        startClimb = false;
        rb.useGravity = true;
        isConnectedToWall = false;
        detach = true;
        moveController.SetIsClimbing(false);
        
    }
    private void DoClimbing(InputAction.CallbackContext context)
    {
        // starts climbing if conditions are met.
        if(upwardClimbing.GetCanClimb() && !startClimb && !isJumping)
        {  
            // resets the current hand holds to account for previous climbing spots.
            upwardClimbing.ResetCurrentHandHolds();
            downwardClimbing.ResetCurrentFootHolds();
            startRotating = false;
            // does a quick calculation for rotation to line the player up.
            calculateRotationToWall();
            // does setup to prepare for the player to be lerped.
            isConnectedToWall = true;
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

        //sets up the offsets and lerp start points for moving down or directionally.
        if(upwardClimbing.GetMovingDownwards())
        {
            Vector3 newCentrePos = Vector3.zero;
            newCentrePos = new Vector3(transform.position.x, transform.position.y - 0.6f, transform.position.z);
            playerLerpStart = newCentrePos;
        }
        else if(upwardClimbing.GetMovingDirecionally())
        {
            Vector3 newCentrePos = Vector3.zero;
            newCentrePos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            playerLerpStart = newCentrePos;
        }
       
        endLerpPoint = upwardClimbing.GetNewMiddleSpot();
        
        // sets up the end point offsets based on direction and movement direction.
        if(lookDirection[0] || lookDirection[2])
        {
            endLerpPoint = new Vector3(endLerpPoint.x, endLerpPoint.y-0.3f, endLerpPoint.z + climbOffset);
            if(upwardClimbing.GetMovingDownwards())
            {
                endLerpPoint = new Vector3(endLerpPoint.x, endLerpPoint.y -0.6f, endLerpPoint.z + climbOffset);
                
            }
            if(upwardClimbing.GetMovingDirecionally())
            {
                endLerpPoint = new Vector3(endLerpPoint.x, endLerpPoint.y, endLerpPoint.z + climbOffset);
               
            }
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
                endLerpPoint = new Vector3(endLerpPoint.x, endLerpPoint.y, endLerpPoint.z + climbOffset);

            }
        }
       
    }
    private void LerpFunction()
    {
        if(startClimb && !arrived)
        {
            
            // only occurs if player is directly pressing movement input.
            if(upwardClimbing.GetMovementDirection().y != 0 || upwardClimbing.GetMovementDirection().x != 0)
            {
                // different interpolate amounts based on direction for smooth gameplay.
                if(!upwardClimbing.GetMovingDirecionally() && !upwardClimbing.GetMovingDownwards())
                {
                    interpolateAmount = (interpolateAmount + Time.deltaTime * 0.55f);
                }
                if(upwardClimbing.GetMovingDownwards())
                {
                    interpolateAmount = (interpolateAmount + Time.deltaTime * 0.85f);
                }
                else if(upwardClimbing.GetMovingDirecionally())
                {
                    interpolateAmount = (interpolateAmount + Time.deltaTime * 1.25f);
                }
            }
            // the actual lerping.
            transform.position = Vector3.Lerp(playerLerpStart, endLerpPoint, interpolateAmount);

            // reset on arrival at lerp destination.
            if(interpolateAmount >= 1.0f)
            {
                interpolateAmount = 0.0f;
                arrived  = true;
            }
        }
    }
    public void SetNewMovement(Vector3 _newEndPoint)
	{
        // initalises the first attach to the wall.
        playerLerpStart = transform.position;
        endLerpPoint = _newEndPoint;

        // very similar to the GetNextClimbing spot code refer to that.
        if(lookDirection[0] || lookDirection[2])
        {
            endLerpPoint = new Vector3(endLerpPoint.x, endLerpPoint.y-0.3f, endLerpPoint.z + climbOffset);
            if(upwardClimbing.GetMovingDownwards())
            {
                endLerpPoint = new Vector3(endLerpPoint.x, endLerpPoint.y -0.5f, endLerpPoint.z + climbOffset);
            }
            if(upwardClimbing.GetMovingDirecionally())
            {
                endLerpPoint = new Vector3(endLerpPoint.x, endLerpPoint.y -0.25f, endLerpPoint.z + climbOffset);
            }
        }

        if(lookDirection[1] || lookDirection[3])
        {
            endLerpPoint = new Vector3(endLerpPoint.x + climbOffset, endLerpPoint.y-0.3f, endLerpPoint.z);
            //Debug.Log(" looking left or right");
           if(upwardClimbing.GetMovingDownwards())
            {
                endLerpPoint = new Vector3(endLerpPoint.x, endLerpPoint.y -0.5f, endLerpPoint.z + climbOffset);
            
            }
            if(upwardClimbing.GetMovingDirecionally())
            {
                endLerpPoint = new Vector3(endLerpPoint.x, endLerpPoint.y -0.25f, endLerpPoint.z + climbOffset);
            }
        }
        //endLerpPoint.position = new Vector3(endLerpPoint.position.x - 0.15f , endLerpPoint.position.y , endLerpPoint.position.z);
        interpolateAmount = 0.0f;
    }

    // getters and setters
    public float GetMovementDirectionY()
    {
        Vector2 moveDirection = movement.ReadValue<Vector2>();
        return moveDirection.y;
    }
     public float GetMovementDirectionX()
    {
        Vector2 moveDirection = movement.ReadValue<Vector2>();
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
    public void SetDoRotatingToTrue()
    {
        startRotating = true;
    }
}
