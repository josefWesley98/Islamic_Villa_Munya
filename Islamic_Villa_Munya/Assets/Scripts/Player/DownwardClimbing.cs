using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public class DownwardClimbing : MonoBehaviour
{
    //scripts
    [Header("Scripts")]
    [SerializeField] private UpwardClimbing upwardClimbing; 
    [SerializeField] private ClimbingScript climbingScript;
    // transforms 
    [Header("Grab Point Lists")]
    [SerializeField] private Transform[] grabbablePositionsRightFoot;
    [SerializeField] private Transform[] grabbablePositionsLeftFoot;

    [Header("References for 8D Movement")]
    [SerializeField] private Transform[] rightGrabPointReference;
    [SerializeField] private Transform[] leftGrabPointReference;

    [Header("Left Foot References")]
    [SerializeField] private Transform leftFootStartPosition;
    [SerializeField] private Transform leftRigAimPosition;
    [SerializeField] private Transform leftFootPos;

    [Header("Right Foot References")]
    [SerializeField] private Transform rightFootStartPosition;
    [SerializeField] private Transform rightRigAimPosition;
    [SerializeField] private Transform rightFootPos;

    [Header("Rigs")]
    [SerializeField] private Rig leftFootRig;
    [SerializeField] private Rig rightFootRig;

    [Header("Rig Weighting")]
    [SerializeField] private float rigTargetWeightLeftFoot;
    [SerializeField] private float rigTargetWeightRightFoot;

    [Header("Materials")]
    [SerializeField] private Material newMaterialRefL;
    [SerializeField] private Material newMaterialRefR;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask climableLayer;

    private GameObject currentFootSpotLeft;
    private GameObject currentFootSpotRight;

    private Transform targetSpotLeftFoot;
    private Transform targetSpotRightFoot;

    private Vector3 detectionRadius = new Vector3(5f,5f,5f);
    private Vector3 middlePoint = Vector3.zero;
    private Vector2 movementDirection;
    
    private bool[] direction = new bool[4] { false, false,false,false};
    private bool needNewLeftFootSpot = false;
    private bool needNewRightFootSpot = false;
    private bool movingLeftFoot = false;
    private bool movingRightFoot= false;
    private bool gotFootHolds = false;
    private bool moveRightFoot = true;
    private bool moveLeftFoot = true;
    private bool paused = false;
    private bool m_Started;
    
    private int arrayPosRightFoot = 0;
    private int arrayPosLeftFoot = 0;
    private int chosenReference = 0;

    private float interpolateAmountRightFoot = 0.0f;
    private float interpolateAmountLeftFoot = 0.0f;

    void Start()
    {
        movingLeftFoot = true;
        needNewLeftFootSpot = true; 
        m_Started = true; 

        // initalise grabable positions array.
        for(int i = 0; i < 150; i++)
        {
            grabbablePositionsLeftFoot[i] = gameObject.transform;
            grabbablePositionsRightFoot[i] = gameObject.transform;
        }
    }
    
    void OnDrawGizmos()
    {
        // draws a box around the player for debugging that shows the detection radius for footholds.
        Gizmos.color = Color.blue;
        if (m_Started)
        {
            Vector3 colliderBoxPos = new Vector3(transform.position.x, transform.position.y +0.2f,transform.position.z);
            Gizmos.DrawWireCube(colliderBoxPos, detectionRadius);
        }
    }
    void Update()
    {
        // get directions.
        for(int i = 0; i < 4; i++)
        {
            direction[i] = climbingScript.GetLookDirection(i);
        }

        // pauses animation and lerping when not moving.
        Pausing();
        
        // finds new foot holds.
        if(!paused)
        {
            FindRightFootClimbingPositions();
            FindLeftFootClimbingPositions();
        }
        
        //Lerps feet positions.
        if(!climbingScript.GetDetach() && !paused)
        {
            LerpRightFootToTarget();
            LerpLeftFootToTarget();
        }

        // handles the changing of weight when attaching to the wall.
        RiggingWeightLerp();
       
    }
    private void Pausing()
    {
        if(movementDirection.x == 0 && movementDirection.y == 0)
        {
            paused = true;
        }
        else
        {
            paused = false;
        }
    }
    private void RiggingWeightLerp()
    {
        // lerps the weight to create clean transitions.
        rightFootRig.weight = Mathf.Lerp(rightFootRig.weight, rigTargetWeightRightFoot, Time.deltaTime*5);
        leftFootRig.weight = Mathf.Lerp(leftFootRig.weight, rigTargetWeightLeftFoot, Time.deltaTime*5);
        
        // checks if on the wall climbing and sets the target weight accordingly.
        if(gotFootHolds && climbingScript.GetIsConnectedToWall() && !climbingScript.GetDetach())
        {
            rigTargetWeightRightFoot = 1.0f;
            rigTargetWeightLeftFoot = 1.0f;
        }
        if(climbingScript.GetDetach() || !gotFootHolds || !climbingScript.GetIsConnectedToWall())
        {
            rigTargetWeightRightFoot = 0.0f;
            rigTargetWeightLeftFoot = 0.0f;
        }
    }
    private void FindRightFootClimbingPositions()
    {
        //moves the collider box up slightly.
        Vector3 colliderBoxPos = new Vector3(transform.position.x, transform.position.y +0.2f,transform.position.z);

        // please refer to the UpwardClimbing Script for a break down of this code and they are basically duplicates, the functions name: FindRightHandClimbingPositions().
        Collider[] climableSpots = Physics.OverlapBox(colliderBoxPos, detectionRadius, transform.rotation, climableLayer);
        

        for(int i = 0; i < climableSpots.Length; i++)
        {
            grabbablePositionsRightFoot[i] = climableSpots[i].gameObject.transform;
        }

        if(climableSpots.Length == 0)
        {
            gotFootHolds = false;
        }
        else
        {
            gotFootHolds = true;
            arrayPosRightFoot = 0;
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
                else
                {
                    chosenReference = 0;
                 
                }
            }   
            if(movementDirection.y < 0)//Down
            {
                if(movementDirection.x > 0)
                {
                    chosenReference = 7;
             
                }
                if(movementDirection.x < 0)
                {
                    chosenReference = 6;
                   
                }
                else // down
                {
                    chosenReference = 1;
              
                }
            }     

            if(movementDirection.x > 0 && movementDirection.y == 0)
            {
                chosenReference = 3;
    
            }
            if(movementDirection.x < 0 && movementDirection.y == 0)
            {
                chosenReference = 2;
     
            }
                
            System.Array.Sort(grabbablePositionsRightFoot, (x,y) =>
            {
                
                float distanceX = Vector3.Distance(x.transform.position, rightGrabPointReference[chosenReference].position);
                float distanceY = Vector3.Distance(y.transform.position, rightGrabPointReference[chosenReference].position);
                return distanceX.CompareTo(distanceY);
            });
            
            targetSpotRightFoot = grabbablePositionsRightFoot[arrayPosRightFoot];

                
        if(needNewRightFootSpot && upwardClimbing.GetNeedNewSpots())
        {
            currentFootSpotRight = targetSpotRightFoot.gameObject;
            needNewRightFootSpot = false;
            GetNewMiddleSpot();
            if(currentFootSpotRight != null && currentFootSpotRight.GetComponent<Renderer>()!= null)
            {
                currentFootSpotRight.GetComponent<Renderer>().material = newMaterialRefR;
            }
        }
        
    }
   
    private void FindLeftFootClimbingPositions()
    {
        // please refer to the UpwardClimbing Script for a break down of this code and they are basically duplicates, the functions name: FindLeftHandClimbingPositions().
        Vector3 colliderBoxPos = new Vector3(transform.position.x, transform.position.y +0.2f,transform.position.z);
        Collider[] climableSpots = Physics.OverlapBox(colliderBoxPos, detectionRadius, transform.rotation, climableLayer);
        
        for(int i = 0; i < climableSpots.Length; i++)
        {
            grabbablePositionsLeftFoot[i] = climableSpots[i].gameObject.transform;
        }

        if(climableSpots.Length == 0)
        {
            gotFootHolds = false;
        }
        else
        {
            gotFootHolds = true;
            arrayPosLeftFoot = 0;
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
                else // Up
                {
                    chosenReference = 0;
                }
            }   
            if(movementDirection.y < 0)//Down
            {
                if(movementDirection.x > 0)
                {
                    chosenReference = 7;
                }
                if(movementDirection.x < 0)
                {
                    chosenReference = 6;
                }
                else // down
                {
                    chosenReference = 1;
                }
            }     

            if(movementDirection.x > 0 && movementDirection.y == 0)
            {
                chosenReference = 3;
                
            }
            if(movementDirection.x < 0 && movementDirection.y == 0)
            {
                chosenReference = 2;
               
            }
        
            System.Array.Sort(grabbablePositionsLeftFoot, (x, y) =>
            {
                float distanceX = Vector3.Distance(x.transform.position, leftGrabPointReference[chosenReference].position);
                float distanceY = Vector3.Distance(y.transform.position, leftGrabPointReference[chosenReference].position);
                return distanceX.CompareTo(distanceY);
            });
            
            targetSpotLeftFoot = grabbablePositionsLeftFoot[arrayPosLeftFoot];

        if(needNewLeftFootSpot && upwardClimbing.GetNeedNewSpots())
        {
            GetNewMiddleSpot();
            currentFootSpotLeft = targetSpotLeftFoot.gameObject;
            needNewLeftFootSpot = false;
            if(currentFootSpotLeft != null && currentFootSpotLeft.GetComponent<Renderer>() != null)
            {
                currentFootSpotLeft.GetComponent<Renderer>().material = newMaterialRefL;
            }
        }   
    }
  
    private void LerpRightFootToTarget()
    {
        // please refer to the UpwardClimbing Script for a break down of this code and they are basically duplicates, the functions name: LerpRightHandToTarget().
        if(rightFootStartPosition == null)
        {
            rightFootStartPosition.position = rightFootPos.position;
        }
        if(movingRightFoot && currentFootSpotRight != null)
        {
            if(upwardClimbing.GetMovingDirecionally())
            {
                 interpolateAmountRightFoot += Time.deltaTime *1.75f;
            }
            else if(upwardClimbing.GetMovingDownwards())
            {
                interpolateAmountRightFoot += Time.deltaTime *1.1f;
            }
            else
            {
                 interpolateAmountRightFoot += Time.deltaTime *1.25f;
            }

            rightRigAimPosition.position = Vector3.Slerp(rightRigAimPosition.position,  currentFootSpotRight.transform.position, interpolateAmountRightFoot);
          

            if(interpolateAmountRightFoot >= 1.0f)
            {
                movingLeftFoot = true;
                movingRightFoot = false;

                rightFootStartPosition.position = currentFootSpotRight.transform.position;
                
                needNewLeftFootSpot = true;
                interpolateAmountRightFoot = 0.0f;
                
                
            }
        }
        else if(!movingRightFoot && currentFootSpotRight != null)
        {
            rightRigAimPosition.position = currentFootSpotRight.transform.position;
        }

    }
    private void LerpLeftFootToTarget()
    {
        // please refer to the UpwardClimbing Script for a break down of this code and they are basically duplicates, the functions name: LerpLeftHandToTarget().
        if(leftFootStartPosition == null)
        {
            leftFootStartPosition.position = leftFootPos.position;
        }
        if(movingLeftFoot && currentFootSpotLeft != null)
        {
            if(upwardClimbing.GetMovingDirecionally())
            {
                interpolateAmountLeftFoot += Time.deltaTime * 1.75f;
            }
            else if(upwardClimbing.GetMovingDownwards())
            {
                interpolateAmountLeftFoot += Time.deltaTime * 1.1f;
            }
            else
            {
                interpolateAmountLeftFoot += Time.deltaTime * 1.25f;
            }

            leftRigAimPosition.position = Vector3.Slerp(leftRigAimPosition.position, currentFootSpotLeft.transform.position, interpolateAmountLeftFoot);

            if(interpolateAmountLeftFoot >= 1.0f)
            {
                interpolateAmountLeftFoot = 0.0f;
                
                movingLeftFoot = false;
                movingRightFoot = true;

                needNewRightFootSpot = true; 
                leftFootStartPosition.position = currentFootSpotLeft.transform.position;
               
            }
        }
        else if(!movingLeftFoot && currentFootSpotLeft != null)
        {
            leftRigAimPosition.position = currentFootSpotLeft.transform.position;
        }
    }

    // getters and setters.
    public Vector3 GetNewMiddleSpot()
    {
        // returns middle point between the two feet.
        middlePoint = GetMiddlePoint(targetSpotRightFoot.position, targetSpotLeftFoot.position);
        return middlePoint;
    }
    private Vector3 GetMiddlePoint(Vector3 leftHandPos, Vector3 rightHandPos)
    {
        // gets middle point between 2 positions.
        float x = (leftHandPos.x + rightHandPos.x) / 2;
        float y = (leftHandPos.y + rightHandPos.y) / 2;
        float z = (leftHandPos.z + rightHandPos.z) / 2;
        Vector3 newPos = new Vector3(x,y,z);

        return newPos;
    }
    public Transform GetTargetSpotLeftFoot()
    {
        return targetSpotLeftFoot;
    }
    public Transform GetTargetSpotRightFoot()
    {
        return targetSpotRightFoot;
    }
    public Vector3 GetCurrentSpotLeftFoot()
    {
        if(currentFootSpotRight != null)
        {
            return currentFootSpotLeft.transform.position;
        }
        else
        {
            return Vector3.zero;
        }
    }
    public Vector3 GetCurrentSpotRightFoot()
    {
        if(currentFootSpotRight != null)
        {
            return currentFootSpotRight.transform.position;
        }
        else
        {
            return Vector3.zero;
        }
    }
    public void SetMovementDirection(Vector2 _moveDirection)
    {
        movementDirection = _moveDirection;
    }
    public void SetDetectionRadius(Vector3 _detectionRadius)
    {
        detectionRadius = _detectionRadius;
    }
    public Vector3 GetMiddlePoint()
    {
        return middlePoint;
    }
    public void ResetCurrentFootHolds()
    {
        needNewLeftFootSpot = true;
        needNewRightFootSpot = true;
        movingLeftFoot = true;
        movingRightFoot = false;
        currentFootSpotLeft = null;
        currentFootSpotRight = null;
    }
}
