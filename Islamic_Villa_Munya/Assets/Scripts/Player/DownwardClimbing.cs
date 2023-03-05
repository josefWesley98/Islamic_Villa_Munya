using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public class DownwardClimbing : MonoBehaviour
{
    //scripts
    [SerializeField] private LayerMask climableLayer;
    [SerializeField] private UpwardClimbing upwardClimbing; 
    [SerializeField] private Transform centrePoint;
    // transforms 
    [SerializeField] private Transform[] grabbablePositionsRightFoot;
    [SerializeField] private Transform[] grabbablePositionsLeftFoot;
    [SerializeField] private Transform[] rightGrabPointReference;
    [SerializeField] private Transform[] leftGrabPointReference;
    [SerializeField] private bool movingLeftFoot = false;
    [SerializeField] private bool movingRightFoot= false;
    [SerializeField] private bool needNewLeftFootSpot = false;
    [SerializeField] private bool needNewRightFootSpot = false;
    //Mats
    [SerializeField] private Material newMaterialRefL;
    [SerializeField] private Material newMaterialRefR;

    // Left Foot Rig
    [SerializeField] private Transform leftFootStartPosition;
    [SerializeField] private Transform leftRigAimPosition;
    [SerializeField] private float rigTargetWeightLeftFoot;
    [SerializeField] private Rig leftFootRig;
    [SerializeField] private Transform leftFootPos;
    private float interpolateAmountLeftFoot = 0.0f;
     private bool moveLeftFoot = true;

    // Right Foot Rig
    [SerializeField] private Transform rightFootStartPosition;
    [SerializeField] private Transform rightRigAimPosition;
    [SerializeField] private float rigTargetWeightRightFoot;
    [SerializeField] private Rig rightFootRig;
    [SerializeField] private Transform rightFootPos;
    private float interpolateAmountRightFoot = 0.0f;
    private bool moveRightFoot = true;
    private Vector3 middlePoint = Vector3.zero;
    // general variables
    private bool m_Started;
    private Transform targetSpotLeftFoot;
    private Transform targetSpotRightFoot;
    private GameObject currentFootSpotLeft;
    private GameObject currentFootSpotRight;
    private int arrayPosLeftFoot = 0;
    private int arrayPosRightFoot = 0;
    private int chosenReference = 0;
    private Vector2 movementDirection;
    private bool gotFootHolds = false;
    private Vector3 leftFootPositionUp;
    private Vector3 leftFootPositionUpLeft;
    private Vector3 leftFootPositionUpRight;

    private Vector3 rightFootPositionUp;
    private Vector3 rightFootPositionUpLeft;
    private Vector3 rightFootPositionUpRight;

     private Vector3 leftFootPositionDown;
    private Vector3 leftFootPositionDownLeft;
    private Vector3 leftFootPositionDownRight;

    private Vector3 rightFootPositionDown;
    private Vector3 rightFootPositionDownLeft;
    private Vector3 rightFootPositionDownRight;
    
    // Start is called before the first frame update
    void Start()
    {
        // leftFootPositionUp;
        // leftFootPositionUpLeft;
        // leftFootPositionUpRight;

        // rightFootPositionUp;
        // rightFootPositionUpLeft;
        // rightFootPositionUpRight;

        // leftFootPositionDown;
        // leftFootPositionDownLeft;
        // leftFootPositionDownRight;

        // rightFootPositionDown;
        // rightFootPositionDownLeft;
        // rightFootPositionDownRight;

        movingLeftFoot = true;
        needNewLeftFootSpot = true; 
        m_Started = true; 
        for(int i = 0; i < 150; i++)
        {
            grabbablePositionsLeftFoot[i] = gameObject.transform;
            grabbablePositionsRightFoot[i] = gameObject.transform;
        }
    }
     void Update()
    {
        FindRightFootClimbingPositions();
        FindLeftFootClimbingPositions();
        
        LerpRightFootToTarget();
        LerpLeftFootToTarget();
        
        RiggingWeightLerp();
       
    }
    private void RiggingWeightLerp()
    {
        rightFootRig.weight = Mathf.Lerp(rightFootRig.weight, rigTargetWeightRightFoot, Time.deltaTime*10);
        leftFootRig.weight = Mathf.Lerp(leftFootRig.weight, rigTargetWeightLeftFoot, Time.deltaTime*10);
        
        if(gotFootHolds)
        {
            rigTargetWeightRightFoot = 1.0f;
            rigTargetWeightLeftFoot = 1.0f;
        }
        else
        {
            rigTargetWeightRightFoot = 0.0f;
            rigTargetWeightLeftFoot = 0.0f;
        }
    }
    private void FindRightFootClimbingPositions()
    {
        Collider[] climableSpots = Physics.OverlapBox(transform.position, transform.localScale, transform.rotation, climableLayer);
        
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
                    Debug.Log("moving down and to the left");
                }
                if(movementDirection.x < 0)
                {
                    chosenReference = 6;
                    Debug.Log("moving down and to the right");
                }
                else // down
                {
                    chosenReference = 1;
                    Debug.Log("just moving down");
                }
            }     

            if(movementDirection.x > 0 && movementDirection.y == 0)
            {
                chosenReference = 3;
                Debug.Log("Just Moving Left");
            }
            if(movementDirection.x < 0 && movementDirection.y == 0)
            {
                chosenReference = 2;
                Debug.Log("just moving right");
            }
                
            System.Array.Sort(grabbablePositionsRightFoot, (x,y) =>
            {
                
                float distanceX = Vector3.Distance(x.transform.position, rightGrabPointReference[chosenReference].position);
                float distanceY = Vector3.Distance(y.transform.position, rightGrabPointReference[chosenReference].position);
                return distanceX.CompareTo(distanceY);
            });
            
            targetSpotRightFoot = grabbablePositionsRightFoot[arrayPosRightFoot];

                
        if(needNewRightFootSpot)
        {
            currentFootSpotRight = targetSpotRightFoot.gameObject;
            needNewRightFootSpot = false;
            GetNewMiddleSpot();
            if(currentFootSpotRight != null)
            {
                currentFootSpotRight.GetComponent<Renderer>().material = newMaterialRefR;
            }
        }
        
    }
    public Vector3 GetMiddlePoint()
    {
        return middlePoint;
    }
    private void FindLeftFootClimbingPositions()
    {
        Collider[] climableSpots = Physics.OverlapBox(transform.position, transform.localScale, transform.rotation, climableLayer);
        
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
                    Debug.Log("moving down and to the left");
                }
                if(movementDirection.x < 0)
                {
                    chosenReference = 6;
                    Debug.Log("moving down and to the right");
                }
                else // down
                {
                    chosenReference = 1;
                    Debug.Log("just moving down");
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
            
            // if(grabbablePositionsLeftFoot[0].position == currentFootSpotLeft.transform.position && grabbablePositionsLeftFoot.Length > 1)
            // {
            //     arrayPosLeftFoot++;
            // }
        
            targetSpotLeftFoot = grabbablePositionsLeftFoot[arrayPosLeftFoot];

        

        if(needNewLeftFootSpot)
        {
            GetNewMiddleSpot();
            currentFootSpotLeft = targetSpotLeftFoot.gameObject;
            needNewLeftFootSpot = false;
            if(currentFootSpotLeft != null)
            {
                currentFootSpotLeft.GetComponent<Renderer>().material = newMaterialRefL;
            }
        }   
    }
    public Vector3 GetNewMiddleSpot()
    {
        middlePoint = GetMiddlePoint(targetSpotRightFoot.position, targetSpotLeftFoot.position);
        return middlePoint;
    }
    private Vector3 GetMiddlePoint(Vector3 leftHandPos, Vector3 rightHandPos)
    {
        float x = (leftHandPos.x + rightHandPos.x) / 2;
        float y = (leftHandPos.y + rightHandPos.y) / 2;
        float z = (leftHandPos.z + rightHandPos.z) / 2;
        Vector3 newPos = new Vector3(x,y,z);

        return newPos;
    }
    private void LerpRightFootToTarget()
    {
        if(rightFootStartPosition == null)
        {
            rightFootStartPosition.position = rightFootPos.position;
        }
        if(movingRightFoot && currentFootSpotRight != null)
        {
           //  rightFootRig.weight = Mathf.Lerp(rightFootRig.weight, rigTargetWeightRightFoot, Time.deltaTime*10);
            interpolateAmountRightFoot += Time.deltaTime *1.25f;
            rightRigAimPosition.position = Vector3.Slerp(rightRigAimPosition.position,  currentFootSpotRight.transform.position, interpolateAmountRightFoot);
            //  rightRigAimPosition.position = Vector3.Lerp(rightFootStartPosition.position, currentFootSpotRight.transform.position , interpolateAmountRightFoot);
            //  float newX = Mathf.Lerp(rightRigAimPosition.position.x, currentFootSpotRight.transform.position.x, interpolateAmountRightFoot);
            //  float newY = Mathf.Lerp(rightRigAimPosition.position.y, currentFootSpotRight.transform.position.y, interpolateAmountRightFoot);
            //  float newZ = Mathf.Lerp(rightRigAimPosition.position.z, currentFootSpotRight.transform.position.z, interpolateAmountRightFoot);
            
            // rightRigAimPosition.position = new Vector3(newX, newY, newZ);

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
        if(leftFootStartPosition == null)
        {
            leftFootStartPosition.position = leftFootPos.position;
        }
        if(movingLeftFoot && currentFootSpotLeft != null)
        {
           
            interpolateAmountLeftFoot += Time.deltaTime * 1.25f;
            leftRigAimPosition.position = Vector3.Slerp(leftRigAimPosition.position, currentFootSpotLeft.transform.position, interpolateAmountLeftFoot);
            // float newX = Mathf.Lerp(leftRigAimPosition.position.x, currentFootSpotLeft.transform.position.x, interpolateAmountLeftFoot);
            // float newY = Mathf.Lerp(leftRigAimPosition.position.y, currentFootSpotLeft.transform.position.y, interpolateAmountLeftFoot);
            // float newZ = Mathf.Lerp(leftRigAimPosition.position.z, currentFootSpotLeft.transform.position.z, interpolateAmountLeftFoot);

            // leftRigAimPosition.position = new Vector3(newX, newY, newZ);

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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_Started)
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(transform.position, transform.localScale);
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
        return currentFootSpotLeft.transform.position;
    }
    public Vector3 GetCurrentSpotRightFoot()
    {
        return currentFootSpotRight.transform.position;
    }
    public void SetMovementDirection(Vector2 _moveDirection)
    {
        movementDirection = _moveDirection;
    }
}
