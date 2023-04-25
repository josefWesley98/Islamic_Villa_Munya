using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public class UpwardClimbing : MonoBehaviour
{
    [Header("Scrips")]
    [SerializeField] private DownwardClimbing downwardClimbing;
    [SerializeField] private ClimbingScript climbingScript;
    [SerializeField] private Animator animator;

    [Header("References for 8D Movement")]
    [SerializeField] private Transform[] rightGrabPointReference;
    [SerializeField] private Transform[] leftGrabPointReference;

    [Header("Grab Point Lists")]
    [SerializeField] private Transform[] grabbablePositionsRightHand;
    [SerializeField] private Transform[] grabbablePositionsLeftHand;

    [Header("Right Arm References")]
    [SerializeField] private Transform rightArmStartPosition;
    [SerializeField] private Transform rightRigAimPosition;
    [SerializeField] private Transform rightArmPos;

    [Header("Left Arm References")]
    [SerializeField] private Transform leftArmStartPosition;
    [SerializeField] private Transform leftRigAimPosition;
    [SerializeField] private Transform leftArmPos;
    
    [Header("Rig References")]
    [SerializeField] private Rig rightArmRig;
    [SerializeField] private Rig leftArmRig;

    [Header("Rig Weighting")]
    [SerializeField] private float rigTargetWeightRightArm;
    [SerializeField] private float rigTargetWeightLeftArm;
    
    [Header("Materials")]
    [SerializeField] private Material newMaterialRefR;
    [SerializeField] private Material newMaterialRefL;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask climableLayer;


    private Vector3 detectionRadius = new Vector3(5, 5, 5);
    private Vector3 middlePoint = Vector3.zero;
    private Vector3 objectToRotateTo = Vector3.zero;
	private Vector3 wallPosition = Vector3.zero;
    private Vector3 idleHandPosL = Vector3.zero;
    private Vector3 idleHandPosR = Vector3.zero;
    private Vector2 movementDirection;


    private Transform targetSpotLeftHand;
    private Transform targetSpotRightHand;
    
    private GameObject currentHandSpotLeft;
    private GameObject currentHandSpotRight;
    
    private bool[] direction = new bool[4] { false, false,false,false};
    private bool movingLeftHand = false;
    private bool movingRightHand = false;
    private bool needNewLeftHandSpot = false;
    private bool needNewRightHandSpot = false;
    private bool movingDirecionally = false;
    private bool left = false;
    private bool right = false;
    private bool canClimb = false;
    private bool moveRightArm = true;
    private bool needNewSpots = true;
    private bool stopLeft = false;
    private bool stopRight = false;
    private bool rotateToWall = false;
    private bool isRotatedToWall = false;
    private bool m_Started;
    private bool finishCurrentGrab = false;
    private bool movingDown = false;
    private bool paused = false;
    
    // what number each of the directions of travel are assigned to.
    // 0 = up. 1 = down. 2 = left. 3 = right. 4 = up left. 5 = up right. 6 = down left. 7 = down right.
    private int chosenReference = 0;
    private int arrayPosLeftHand = 0;
    private int arrayPosRightHand = 0;

    private float interpolateAmountRightArm = 0.0f;
    private float interpolateAmountLeftArm = 0.0f;
    private float idleHandLInterpolate = 0.0f;
    private float idleHandRInterpolate = 0.0f;
    private float movingX = 0.0f;
    private float movingY = 0.0f;

    void Start()
    {
        m_Started = true;

        // initaise grabable spots.
        for(int i = 0; i < 150; i++)
        {
            grabbablePositionsLeftHand[i] = gameObject.transform;
            grabbablePositionsRightHand[i] = gameObject.transform;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (m_Started)
        {
            Gizmos.DrawWireCube(transform.position, detectionRadius);
        }
    }

    void Update()
    {
        // movement.
        movementDirection.x = climbingScript.GetMovementDirectionX();
        movementDirection.y = climbingScript.GetMovementDirectionY();

        // direction facing.
        for(int i = 0; i < 4; i++)
        {
            direction[i] = climbingScript.GetLookDirection(i);
        }

        //pausing to stop jitter.
        PausingAnimationAndLerping();
        
        // find new positions.
        if(!paused)
        {
            // finding new climbing spots.
            FindRightHandClimbingPositions();
            FindLeftHandClimbingPositions();
        }
        
        if(!climbingScript.GetDetach() && !paused)
        {
            // hand lerping to new positions.
            LerpRightHandToTarget();
            LerpLeftHandToTarget();

        }
        // calculates the point which the body should to.
        CalculateRotationPoint();

        // lerps the weights on the players limbs.
        RiggingWeightLerp();

        // tells downward climbing the direction player is moving.
        downwardClimbing.SetMovementDirection(movementDirection);
   
    }
    private void CalculateRotationPoint()
    {
        if(currentHandSpotLeft != null && currentHandSpotRight != null && downwardClimbing.GetCurrentSpotLeftFoot() != Vector3.zero && downwardClimbing.GetCurrentSpotRightFoot() != Vector3.zero)
        {
            // tells the climbing script the player is ready to rotate.
            climbingScript.SetDoRotatingToTrue();
            // calculates a position between 2 hand holds and foot holds to create an authentic body angle to the surface they are climbing on.
            wallPosition = GetMiddlePointForRotate(currentHandSpotLeft.transform.position, currentHandSpotRight.transform.position, downwardClimbing.GetCurrentSpotLeftFoot(), downwardClimbing.GetCurrentSpotRightFoot());
        }
    }
    private void PausingAnimationAndLerping()
    {
        // halts the animations and movement of the hands if the player isnt moving.
        if(movementDirection.x == 0 && movementDirection.y == 0)
        {
            paused = true;
            animator.speed = 0f;
        }
        else
        {
            paused = false;
            animator.speed = 0.65f;
        }
        if(climbingScript.GetDetach())
        {
            paused = false;
            animator.speed = 1.0f;
        }
        
    }
    private void RiggingWeightLerp()
    {
        // sets the rigs off and on based on if they are attached to the wall.
        rightArmRig.weight = Mathf.Lerp(rightArmRig.weight, rigTargetWeightRightArm, Time.deltaTime*5);
        leftArmRig.weight = Mathf.Lerp(leftArmRig.weight, rigTargetWeightLeftArm, Time.deltaTime*5);
        
        if(canClimb && climbingScript.GetIsConnectedToWall() && !climbingScript.GetDetach())
        {
            
            rigTargetWeightRightArm = 1.0f;
            rigTargetWeightLeftArm = 1.0f;
        }
        if(!canClimb || !climbingScript.GetIsConnectedToWall() || climbingScript.GetDetach())
        {
            rigTargetWeightRightArm = 0.0f;
            rigTargetWeightLeftArm = 0.0f;
        }
    }
    private void FindRightHandClimbingPositions()
    {
        // generates a box around the player that detects hand holds that are tagged with a specific layer.
        Collider[] climableSpots = Physics.OverlapBox(transform.position, detectionRadius, transform.rotation, climableLayer);
        
        for(int i = 0; i < climableSpots.Length; i++)
        {
            //creates a list of possible hand holds for the player.
            grabbablePositionsRightHand[i] = climableSpots[i].gameObject.transform;
        }

        if(climableSpots.Length == 0)
        {
            // if no spots then cant climb.
            canClimb = false;
        }
        else
        {
            // if spots the can climb
            canClimb = true;
            arrayPosRightHand = 0;
        }

            // checks what direction the player is moving on an 8D spectrum which is tagged by assignin a number between 0-7 for each direction.
            if(movementDirection.y > 0)//Up
            {
                if(movementDirection.x > 0)
                {
                    chosenReference = 5;
              
                }
                if(movementDirection.x < 0)
                {
                    chosenReference = 4;
        
                }
                if(movementDirection.x == 0) // Up
                {
                    chosenReference = 0;
                }
            }
            movingDown = false;   
            if(movementDirection.y < 0)//Down
            {
                movingDown = true;
                if(movementDirection.x > 0)
                {
                    chosenReference = 7;
                  
                }
                if(movementDirection.x < 0)
                {
                    chosenReference = 6;
                    
                }
                else// down
                {
                    chosenReference = 1;
              
                }
            }     
            movingDirecionally = false;
            left = false;
            right = false;
            if(movementDirection.x > 0 && movementDirection.y == 0)
            {
                right = true;
                chosenReference = 3;  
                movingDirecionally = true;  
            }
            if(movementDirection.x < 0 && movementDirection.y == 0)
            {
                left = true;
                chosenReference = 2;
                movingDirecionally = true;
            }
                // an array that orders from nearest to furthest from the to a gameobject attached to the player based on the 8 directions they might want to move.
                // there are 8 game objects that are assigned between 0-7 in an array that link up with the previously mentioned chosenReference vairables that was decied based
                // on user input.
                System.Array.Sort(grabbablePositionsRightHand, (x, y) =>
                { 
                    float distanceX = Vector3.Distance(x.transform.position, rightGrabPointReference[chosenReference].position);
                    float distanceY = Vector3.Distance(y.transform.position, rightGrabPointReference[chosenReference].position);
                    return distanceX.CompareTo(distanceY);
                });
                // the clostest hand hold to the to object in the direction they want to move is assigned as the target hand hold spot (the ideal spot to for moving in the direction they want to go.).
                targetSpotRightHand = grabbablePositionsRightHand[arrayPosRightHand];

        //changes the current hand hold to the target hand hold when its the right hands turn to move and disconect from the wall(this condition occurs when the left hand reaches destination.).
        if(needNewRightHandSpot && needNewSpots)
        {
            currentHandSpotRight = targetSpotRightHand.gameObject;
            needNewRightHandSpot = false;
            // the middle point Vector3 reference the middle point between either hands or feet depending on direction the player is moving to create a smooth transition between points.
            if(chosenReference == 2)
            {
                 middlePoint = GetMiddlePoint(targetSpotLeftHand.position, downwardClimbing.GetCurrentSpotLeftFoot());
                 Debug.Log("moving left");
                 
            }
            else if(chosenReference == 3)
            {
                 middlePoint = GetMiddlePoint(targetSpotRightHand.position, downwardClimbing.GetCurrentSpotRightFoot());
            }
            else if(chosenReference == 6 || chosenReference == 7 || chosenReference == 1)
            {
                middlePoint = downwardClimbing.GetMiddlePoint();
            }
            else
            {  
                middlePoint = GetMiddlePoint(targetSpotRightHand.position, targetSpotLeftHand.position);   
            }
              
            climbingScript.SetNewMovement(middlePoint);
            if(currentHandSpotRight != null)
            {
                currentHandSpotRight.GetComponent<Renderer>().material = newMaterialRefR;
            }
            objectToRotateTo = currentHandSpotRight.transform.parent.transform.position;
        }
        
    }
    public Vector3 GetNewMiddleSpot()
    {
        // sets a new middle point.
        middlePoint = GetMiddlePoint(targetSpotRightHand.position, targetSpotLeftHand.position);
        return middlePoint;
    }
    private void FindLeftHandClimbingPositions()
    {
        // this is all the same as the FindRightHandClimbingPositions so refer to the comments there.
        Collider[] climableSpots = Physics.OverlapBox(transform.position, detectionRadius, transform.rotation, climableLayer);
        
        for(int i = 0; i < climableSpots.Length; i++)
        {
            grabbablePositionsLeftHand[i] = climableSpots[i].gameObject.transform;
        }

        if(climableSpots.Length == 0)
        {
            canClimb = false;
        }
        else
        {
            canClimb = true;
            arrayPosLeftHand = 0;
        }
            if(movementDirection.y > 0)//Up
            {
                if(movementDirection.x > 0)
                {
                    chosenReference = 5;
                   
                }
                if(movementDirection.x < 0)
                {
                    chosenReference = 4;
             
                }
                if(movementDirection.x == 0) // Up
                {
                    chosenReference = 0;
           
                }
            }   
            movingDown = false;
            if(movementDirection.y < 0)//Down
            {
                movingDown = true;
                if(movementDirection.x > 0)
                {
                    chosenReference = 7;
          
                }
                if(movementDirection.x < 0)
                {
                    chosenReference = 6;
               
                }
               
                else
                {
                    chosenReference = 1;
           
                }
            }     
            movingDirecionally = false;
            left = false;
            right = false;
            if(movementDirection.x < 0 && movementDirection.y == 0)
            {
                left = true;
                chosenReference = 2;
                movingDirecionally = true;
               
            }
            if(movementDirection.x > 0 && movementDirection.y == 0)
            {
                right = true;
                chosenReference = 3;
                movingDirecionally = true;
            }
        

            System.Array.Sort(grabbablePositionsLeftHand, (x, y) =>
            {
                float distanceX = Vector3.Distance(x.transform.position, leftGrabPointReference[chosenReference].position);
                float distanceY = Vector3.Distance(y.transform.position, leftGrabPointReference[chosenReference].position);
                return distanceX.CompareTo(distanceY);
            });
            
        
            targetSpotLeftHand = grabbablePositionsLeftHand[arrayPosLeftHand];

        

        if(needNewLeftHandSpot && needNewSpots)
        {
            currentHandSpotLeft = targetSpotLeftHand.gameObject;
            needNewLeftHandSpot = false;

            if(chosenReference == 2)
            {
                middlePoint = GetMiddlePoint(downwardClimbing.GetCurrentSpotLeftFoot(), targetSpotLeftHand.position);
            }
            else if(chosenReference == 3)
            {
                 middlePoint = GetMiddlePoint(targetSpotRightHand.position, downwardClimbing.GetCurrentSpotRightFoot());
            }
            else if(chosenReference == 6 || chosenReference == 7 || chosenReference == 1)
            {
                middlePoint = downwardClimbing.GetMiddlePoint();
            }
            else
            {  
                middlePoint = GetMiddlePoint(targetSpotRightHand.position, targetSpotLeftHand.position);   
            }
              
            climbingScript.SetNewMovement(middlePoint);

            if(currentHandSpotLeft != null)
            {
                currentHandSpotLeft.GetComponent<Renderer>().material = newMaterialRefL;
            }
        
            objectToRotateTo = currentHandSpotLeft.transform.parent.transform.position;
        }
        
    }
 
    private void LerpRightHandToTarget()
    {
        if(rightArmStartPosition == null)
        {
            rightArmStartPosition.position = rightArmPos.position;
        }
        if(movingRightHand && currentHandSpotRight != null)
        {
            Vector3 LHandPos = Vector3.zero;

            if(climbingScript.GetLookDirection(0) || climbingScript.GetLookDirection(2))
            {//z
                LHandPos =  new Vector3(currentHandSpotRight.transform.position.x, currentHandSpotRight.transform.position.y, currentHandSpotRight.transform.position.z + 0.15f);
            }   
            if(climbingScript.GetLookDirection(1) || climbingScript.GetLookDirection(3))
            {//x
                LHandPos =  new Vector3(currentHandSpotRight.transform.position.x + 0.15f, currentHandSpotRight.transform.position.y, currentHandSpotRight.transform.position.z);
            }
            
            if(movingDirecionally)
            {
                interpolateAmountRightArm += Time.deltaTime * 1.75f;
            }
            else if(movingDown)
            {
                interpolateAmountRightArm += Time.deltaTime * 1.75f;
            }
            else
            {
                 interpolateAmountRightArm += Time.deltaTime * 1.25f;
            }

            rightRigAimPosition.position = Vector3.Slerp(rightRigAimPosition.position, LHandPos, interpolateAmountRightArm);

            if(interpolateAmountRightArm >= 1.0f)
            {
                movingLeftHand = true;
                movingRightHand = false;
                idleHandRInterpolate = 0.0f;
                idleHandPosR = rightRigAimPosition.position;
                needNewLeftHandSpot = true;
                rightArmStartPosition.position = currentHandSpotRight.transform.position;
                interpolateAmountRightArm = 0.0f;
            }
        }
        if(!movingRightHand && currentHandSpotRight != null)
        {
            idleHandRInterpolate += Time.deltaTime * 1.25f;
            rightRigAimPosition.position = Vector3.Lerp(idleHandPosR, currentHandSpotRight.transform.position, idleHandRInterpolate);
            if(idleHandLInterpolate >= 1.0f)
            {
                idleHandLInterpolate = 0.0f;
                idleHandPosL =  rightRigAimPosition.position;
            }
        }

    }
    private void LerpLeftHandToTarget()
    {
        // checks if the start point for the lerping isnt null as to avoid errors.
        if(leftArmStartPosition == null)
        {
            leftArmStartPosition.position = leftArmPos.position;
        }

        // checks if its the left hands turn to move and if there is a current hand hold to move to.
        if(movingLeftHand && currentHandSpotLeft != null)
        {
            // variable lerping speeds to make it look clean when the player moves.
            if(movingDirecionally)
            {
                interpolateAmountLeftArm  += Time.deltaTime * 1.75f;
            }
            else if(movingDown)
            {
                interpolateAmountLeftArm  += Time.deltaTime * 1.75f;
            }
            else
            {
                interpolateAmountLeftArm  += Time.deltaTime * 1.25f;
            }
            Vector3 RHandPos = Vector3.zero;

            // a slight offset based on the direction the player is look to get a better looking hold on objects.
            if(climbingScript.GetLookDirection(0) || climbingScript.GetLookDirection(2))
            {//z
                RHandPos =  new Vector3(currentHandSpotLeft.transform.position.x, currentHandSpotLeft.transform.position.y, currentHandSpotLeft.transform.position.z + 0.15f);
            }   
            if(climbingScript.GetLookDirection(1) || climbingScript.GetLookDirection(3))
            {//x
                RHandPos =  new Vector3(currentHandSpotLeft.transform.position.x + 0.15f, currentHandSpotLeft.transform.position.y, currentHandSpotLeft.transform.position.z);
            }

            // the actual slerping happens here, I know its named lerping function but slerping ended up looking a lot better.
            leftRigAimPosition.position = Vector3.Slerp(leftRigAimPosition.position, RHandPos, interpolateAmountLeftArm);

            // when the slerp is complete reset values and set the right hand to move now and tell the find hand hold function it needs a new hand hold for this hand.
            if(interpolateAmountLeftArm >= 1.0f)
            {
                interpolateAmountLeftArm = 0.0f;

                movingLeftHand = false;
                movingRightHand = true;
                idleHandPosL = leftRigAimPosition.position;
                idleHandLInterpolate = 0.0f;
                needNewRightHandSpot = true; 
                leftArmStartPosition.position = currentHandSpotLeft.transform.position;
        
               
            }
        }
        if(!movingLeftHand && currentHandSpotLeft != null)
        {
            // this continously keeps the hand on the spot its on while the body moves so it doesnt look disconnected from the hand hold, i found lerping this decreased jiter.
            idleHandLInterpolate += Time.deltaTime * 1.25f;
            leftRigAimPosition.position = Vector3.Lerp(idleHandPosL, currentHandSpotLeft.transform.position, idleHandLInterpolate);
            if(idleHandLInterpolate >= 1)
            {
                idleHandLInterpolate = 0.0f;
                idleHandPosL = leftRigAimPosition.position;
            }
        }
    }
    private Vector3 GetMiddlePoint(Vector3 leftHandPos, Vector3 rightHandPos)
    {
        // finds the middle point between the two points.
        float x = (leftHandPos.x + rightHandPos.x) / 2;
        float y = (leftHandPos.y + rightHandPos.y) / 2;
        float z = (leftHandPos.z + rightHandPos.z) / 2;
        Vector3 newPos = new Vector3(x,y,z);

        return newPos;
    }
    private Vector3 GetMiddlePointForRotate(Vector3 leftHandPos, Vector3 rightHandPos, Vector3 leftFootPos, Vector3 rightFootPos)
    {
        // finds the middle point between 4 points.
        float x = (leftHandPos.x + rightHandPos.x + leftFootPos.x + rightFootPos.x) / 4;
        float y = (leftHandPos.y + rightHandPos.y + leftFootPos.y + rightFootPos.y) / 4;
        float z = (leftHandPos.z + rightHandPos.z + leftFootPos.z + rightFootPos.z) / 4;
        Vector3 newPos = new Vector3(x,y,z);

        return newPos;
    }

    // everything below this point is getters and setters.
    public bool GetCanClimb()
    {
        return canClimb;
    }
    public Vector2 GetMovementDirection()
    {
        return movementDirection;
    }
    public void SetMoveRightArm(bool _moveRightArm)
    {
        rightArmStartPosition.position = rightArmPos.position;
        leftArmStartPosition.position = leftArmPos.position;
    }
    public bool GetNeedNewSpots()
    {
        return needNewSpots;
    }
    public bool GetRotateToWall()
    {
        return rotateToWall;
    }
	public Vector3 GetWallPosition()
    {
        return wallPosition;
    }
    public void SetRotateToWall(bool value)
    {
        rotateToWall = value;
    }
    public bool GetMovingDirecionally()
    {
        return movingDirecionally;
    }
    public bool GetMovingDownwards()
    {
        return movingDown;
    }
    public void SetDetectionRadius(Vector3 _detectionRadius)
    {
        detectionRadius = _detectionRadius;
    }
    public bool GetIsClimbingPaused()
    {
        return paused;
    }
    public bool GetIsRotatedToWall()
    {
        return isRotatedToWall;
    }
    public void ResetCurrentHandHolds()
    {
        needNewLeftHandSpot = true;
        needNewRightHandSpot = false;
        movingLeftHand = true;
        movingRightHand = false;
        currentHandSpotRight = null;
        currentHandSpotLeft = null;
    }

}
