﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public class UpwardClimbing : MonoBehaviour
{
    [SerializeField] private LayerMask climableLayer;
    [SerializeField] private DownwardClimbing downwardClimbing;
    // 0 = up. 1 = down. 2 = left. 3 = right. 4 = up left. 5 = up right. 6 = down left. 7 = down right.
    [SerializeField] private Transform[] rightGrabPointReference;
    private int chosenReference = 0;
    [SerializeField] private Transform[] leftGrabPointReference;
    private Vector3 middlePoint = Vector3.zero;
    private bool canClimb = false;
    bool m_Started;
    [SerializeField] private ClimbingScript climbingScript;
    // rigging start
    [SerializeField] private Transform rightArmStartPosition;
    [SerializeField] private Transform rightRigAimPosition;
    [SerializeField] private float rigTargetWeightRightArm;
    [SerializeField] private Rig rightArmRig;
    [SerializeField] private Transform rightArmPos;
    private float interpolateAmountRightArm = 0.0f;
    private bool moveRightArm = true;


    [SerializeField] private Transform leftArmStartPosition;
    [SerializeField] private Transform leftRigAimPosition;
    [SerializeField] private float rigTargetWeightLeftArm;
    [SerializeField] private Rig leftArmRig;
    [SerializeField] private Transform leftArmPos;
    private float interpolateAmountLeftArm = 0.0f;
    private bool movingDirecionally = false;
    //rigging end.
    [SerializeField] private Transform[] grabbablePositionsRightHand;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform[] grabbablePositionsLeftHand;
    [SerializeField] private Material newMaterialRefR;
    [SerializeField] private Material newMaterialRefL;
    private bool isRotatedToWall = false;
    private Transform targetSpotLeftHand;
    private Transform targetSpotRightHand;
    private GameObject currentHandSpotLeft;
    private GameObject currentHandSpotRight;
    private Vector2 movementDirection;
    private int arrayPosLeftHand = 0;
    private int arrayPosRightHand = 0;
    private bool finishCurrentGrab = false;
    [Range(-1.0f, 1.0f)]
    [SerializeField] private float movingX = 0.0f;
    [Range(-1.0f, 1.0f)]
    [SerializeField] private float movingY = 0.0f;
    [SerializeField] private bool movingLeftHand = false;
    [SerializeField] private bool movingRightHand = false;
    [SerializeField]  private bool needNewLeftHandSpot = false;
    [SerializeField] private bool needNewRightHandSpot = false;
    [SerializeField] private bool[] direction = new bool[4] { false, false,false,false};
    private Vector3 objectToRotateTo = Vector3.zero;
    private bool needNewSpots = true;
    private bool stopLeft = false;
    private bool stopRight = false;
    private bool rotateToWall = false;
	private Vector3 wallPosition = Vector3.zero;

    void Start()
    {
        

        m_Started = true;
        for(int i = 0; i < 150; i++)
        {
            grabbablePositionsLeftHand[i] = gameObject.transform;
            grabbablePositionsRightHand[i] = gameObject.transform;
        }
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_Started)
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
    // Update is called once per frame
    void Update()
    {
        if(currentHandSpotLeft != null && currentHandSpotRight != null && !movingDirecionally)
        {
            wallPosition = GetMiddlePoint(currentHandSpotLeft.transform.position, currentHandSpotRight.transform.position);
        }

        for(int i = 0; i < 4; i++)
        {
            direction[i] = climbingScript.GetLookDirection(i);
        }

        if(movementDirection.x == 0 && movementDirection.y == 0 && climbingScript.GetIsConnectedToWall() && !climbingScript.GetDetach()) 
        {
           
            needNewSpots = false;
            finishCurrentGrab = true;
            if(stopLeft && stopRight)
            {
                animator.speed = 0.0f;
            }
        }
        else if(climbingScript.GetIsConnectedToWall())
        {
            finishCurrentGrab = false;
            animator.speed = 0.8f;
            needNewSpots = true;
            stopLeft = false;
            stopRight = false;
        }

        FindRightHandClimbingPositions();
        FindLeftHandClimbingPositions();
        
        if(!climbingScript.GetDetach())
        {
            LerpRightHandToTarget();
            LerpLeftHandToTarget();
        }
        
        RiggingWeightLerp();
        downwardClimbing.SetMovementDirection(movementDirection);

        movementDirection.x = climbingScript.GetMovementDirectionX();
        movementDirection.y = climbingScript.GetMovementDirectionY();
    }
    private void RiggingWeightLerp()
    {
        rightArmRig.weight = Mathf.Lerp(rightArmRig.weight, rigTargetWeightRightArm, Time.deltaTime*10);
        leftArmRig.weight = Mathf.Lerp(leftArmRig.weight, rigTargetWeightLeftArm, Time.deltaTime*10);
        
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
        Collider[] climableSpots = Physics.OverlapBox(transform.position, transform.localScale, transform.rotation, climableLayer);
        
        for(int i = 0; i < climableSpots.Length; i++)
        {
            grabbablePositionsRightHand[i] = climableSpots[i].gameObject.transform;
        }

        if(climableSpots.Length == 0)
        {
            canClimb = false;
        }
        else
        {
            canClimb = true;
            arrayPosRightHand = 0;
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
                else// down
                {
                    chosenReference = 1;
              
                }
            }     
            movingDirecionally = false;
            if(movementDirection.x > 0 && movementDirection.y == 0)
            {
                chosenReference = 3;  
                movingDirecionally = true;  
            }
            if(movementDirection.x < 0 && movementDirection.y == 0)
            {
                chosenReference = 2;
                movingDirecionally = true;
            }
                
                System.Array.Sort(grabbablePositionsRightHand, (x, y) =>
                { 
                    float distanceX = Vector3.Distance(x.transform.position, rightGrabPointReference[chosenReference].position);
                    float distanceY = Vector3.Distance(y.transform.position, rightGrabPointReference[chosenReference].position);
                    return distanceX.CompareTo(distanceY);
                });
                
                targetSpotRightHand = grabbablePositionsRightHand[arrayPosRightHand];

                
        if(needNewRightHandSpot && needNewSpots)
        {
            currentHandSpotRight = targetSpotRightHand.gameObject;
            needNewRightHandSpot = false;
	        //wallPosition = currentHandSpotRight.transform.parent.gameObject.transform.localPosition;

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
            if(currentHandSpotRight != null)
            {
                currentHandSpotRight.GetComponent<Renderer>().material = newMaterialRefR;
            }
            objectToRotateTo = currentHandSpotRight.transform.parent.transform.position;
        }
        
    }
    public Vector3 GetNewMiddleSpot()
    {
        needNewLeftHandSpot = true;
        movingLeftHand = true;
        middlePoint = GetMiddlePoint(targetSpotRightHand.position, targetSpotLeftHand.position);
        return middlePoint;
    }
    private void FindLeftHandClimbingPositions()
    {
        Collider[] climableSpots = Physics.OverlapBox(transform.position, transform.localScale, transform.rotation, climableLayer);
        
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
               
                else
                {
                    chosenReference = 1;
           
                }
            }     
            movingDirecionally = false;
            if(movementDirection.x < 0 && movementDirection.y == 0)
            {
                chosenReference = 2;
                movingDirecionally = true;
               
            }
            if(movementDirection.x > 0 && movementDirection.y == 0)
            {
                chosenReference = 3;
                movingDirecionally = true;
            }
        

            System.Array.Sort(grabbablePositionsLeftHand, (x, y) =>
            {
                float distanceX = Vector3.Distance(x.transform.position, leftGrabPointReference[chosenReference].position);
                float distanceY = Vector3.Distance(y.transform.position, leftGrabPointReference[chosenReference].position);
                return distanceX.CompareTo(distanceY);
            });
            
            // if(grabbablePositionsLeftHand[0].position == currentHandSpotLeft.transform.position && grabbablePositionsLeftHand.Length > 1)
            // {
            //     arrayPosLeftHand++;
            // }
        
            targetSpotLeftHand = grabbablePositionsLeftHand[arrayPosLeftHand];

        

        if(needNewLeftHandSpot && needNewSpots)
        {
            currentHandSpotLeft = targetSpotLeftHand.gameObject;
            needNewLeftHandSpot = false;
	        //wallPosition = currentHandSpotLeft.transform.parent.gameObject.transform.localPosition;
            if(chosenReference == 2)
            {
                //Debug.Log("getting a middle point to the left");
                middlePoint = GetMiddlePoint(downwardClimbing.GetCurrentSpotLeftFoot(), targetSpotLeftHand.position);
            }
            else if(chosenReference == 3)
            {
                //Debug.Log("getting a middle point to the right");
                 middlePoint = GetMiddlePoint(targetSpotRightHand.position, downwardClimbing.GetCurrentSpotRightFoot());
            }
            else if(chosenReference == 6 || chosenReference == 7 || chosenReference == 1)
            {
                //Debug.Log("getting a middle point down");
                middlePoint = downwardClimbing.GetMiddlePoint();
            }
            else
            {  
                //Debug.Log("getting a middle point above");
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
    public bool GetIsRotatedToWall()
    {
        return isRotatedToWall;
    }
    private void LerpRightHandToTarget()
    {
        if(rightArmStartPosition == null)
        {
            rightArmStartPosition.position = rightArmPos.position;
        }
        if(movingRightHand && currentHandSpotRight != null && !stopRight)
        {
            //Debug.Log("moving right hand.");
            //float spotZ = currentHandSpotRight.transform.position.z + 0.15f;
            // float newX = Mathf.Lerp(rightRigAimPosition.position.x, currentHandSpotRight.transform.position.x, interpolateAmountRightArm);
            // float newY = Mathf.Lerp(rightRigAimPosition.position.y, currentHandSpotRight.transform.position.y, interpolateAmountRightArm);
            // float newZ = Mathf.Lerp(rightRigAimPosition.position.z, spotZ, interpolateAmountRightArm);
            
            //rightRigAimPosition.position = new Vector3(newX, newY, newZ);

            Vector3 LHandPos =  new Vector3(currentHandSpotRight.transform.position.x, currentHandSpotRight.transform.position.y, currentHandSpotRight.transform.position.z);
            if(movingDirecionally)
            {
                 interpolateAmountRightArm += Time.deltaTime * 1.5f;
            }
            else
            {
                 interpolateAmountRightArm += Time.deltaTime * 1.25f;
            }
            //interpolateAmountRightArm += Time.deltaTime * 1.25f;

            rightRigAimPosition.position = Vector3.Slerp(rightRigAimPosition.position, LHandPos, interpolateAmountRightArm);

            if(interpolateAmountRightArm >= 1.0f)
            {
                movingLeftHand = true;
                movingRightHand = false;

                needNewLeftHandSpot = true;
                if(finishCurrentGrab)
                {
                    stopRight = true;
                }
                rightArmStartPosition.position = currentHandSpotRight.transform.position;
                interpolateAmountRightArm = 0.0f;
            }
        }
        else if(!movingRightHand && currentHandSpotRight != null)
        {
            rightRigAimPosition.position = currentHandSpotRight.transform.position;
        }

    }
    private void LerpLeftHandToTarget()
    {
        if(leftArmStartPosition == null)
        {
            leftArmStartPosition.position = leftArmPos.position;
        }
        if(movingLeftHand && currentHandSpotLeft != null && !stopLeft)
        {
            
            if(movingDirecionally)
            {
                interpolateAmountLeftArm  += Time.deltaTime * 1.5f;
            }
            else
            {
                interpolateAmountLeftArm  += Time.deltaTime * 1.25f;
            }
            Vector3 RHandPos =  new Vector3(currentHandSpotLeft.transform.position.x, currentHandSpotLeft.transform.position.y, currentHandSpotLeft.transform.position.z);
            
            leftRigAimPosition.position = Vector3.Slerp(leftRigAimPosition.position, RHandPos, interpolateAmountLeftArm);

            // float newX = Mathf.Lerp(leftRigAimPosition.position.x, currentHandSpotLeft.transform.position.x, interpolateAmountLeftArm);
            // float newY = Mathf.Lerp(leftRigAimPosition.position.y, currentHandSpotLeft.transform.position.y, interpolateAmountLeftArm);
            // float newZ = Mathf.Lerp(leftRigAimPosition.position.z, spotZ, interpolateAmountLeftArm);
            
            // leftRigAimPosition.position = new Vector3(newX, newY, newZ);

            if(interpolateAmountLeftArm >= 1.0f)
            {
                interpolateAmountLeftArm = 0.0f;

                movingLeftHand = false;
                movingRightHand = true;

                needNewRightHandSpot = true; 
                leftArmStartPosition.position = currentHandSpotLeft.transform.position;
                
                if(finishCurrentGrab)
                {
                    stopLeft = true;
                }
               
            }
        }
        else if(!movingLeftHand && currentHandSpotLeft != null)
        {
            leftRigAimPosition.position = currentHandSpotLeft.transform.position;
        }
    }
    private Vector3 GetMiddlePoint(Vector3 leftHandPos, Vector3 rightHandPos)
    {
        float x = (leftHandPos.x + rightHandPos.x) / 2;
        float y = (leftHandPos.y + rightHandPos.y) / 2;
        float z = (leftHandPos.z + rightHandPos.z) / 2;
        Vector3 newPos = new Vector3(x,y,z);

        return newPos;
    }
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
}
