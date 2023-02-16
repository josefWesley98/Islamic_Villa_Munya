using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private Transform[] grabbablePositionsRightHand;
    [SerializeField] private Transform[] grabbablePositionsLeftHand;
   [SerializeField] private Material newMaterialRefR;
   [SerializeField] private Material newMaterialRefL;
    private Transform targetSpotLeftHand;
    private Transform targetSpotRightHand;
    private Vector3 currentHandSpotLeft = Vector3.zero;
    private Vector3 currentHandSpotRight = Vector3.zero;
    private Vector2 movementDirection;
    private int arrayPosLeftHand = 0;
    private int arrayPosRightHand = 0;

    [Range(-1.0f, 1.0f)]
    [SerializeField] private float movingX = 0.0f;
    [Range(-1.0f, 1.0f)]
    [SerializeField] private float movingY = 0.0f;
    [SerializeField] private bool movingLeftHand = false;
    [SerializeField] private bool movingRightHand = false;

    void Start()
    {
        m_Started = true;
        for(int i = 0; i < 50; i++)
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
        FindClimbingPositions();
        LerpRightHandToTarget();
        LerpLeftHandToTarget();
        movementDirection.x = movingX;
        movementDirection.y = movingY;
    }
    private void FindClimbingPositions()
    {
        Collider[] climableSpots = Physics.OverlapBox(transform.position, transform.localScale, transform.rotation, climableLayer);
        
        for(int i = 0; i < climableSpots.Length; i++)
        {
            grabbablePositionsRightHand[i] = climableSpots[i].gameObject.transform;
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
            arrayPosRightHand = 0;


        if(movementDirection.y > 0)//Up
        {
            if(movementDirection.x > 0)//Up Right
            {
                chosenReference = 5;
                Debug.Log("moving up and to the left");
            }
            if(movementDirection.x < 0)//Up Left
            {
                chosenReference = 4;
                Debug.Log("moving Up and to the Right");
            }
            if(movementDirection.x == 0) // Up
            {
                chosenReference = 0;
                Debug.Log("just moving up");
            }
        }   
        if(movementDirection.y < 0)//Down
        {
            if(movementDirection.x > 0)//Down Right
            {
                chosenReference = 7;
                Debug.Log("moving down and to the left");
            }
            if(movementDirection.x < 0)//Down Left
            {
                 chosenReference = 6;
                 Debug.Log("moving down and to the right");
            }
            if(movementDirection.x == 0)// down
            {
               chosenReference = 1;
               Debug.Log("just moving down");
            }
        }     

        if(movementDirection.x > 0 && movementDirection.y == 0)
        {
            chosenReference = 2;
            Debug.Log("Just Moving Left");
        }
        if(movementDirection.x < 0 && movementDirection.y == 0)
        {
            chosenReference = 3;
            Debug.Log("just moving right");
        }
       

            System.Array.Sort(grabbablePositionsLeftHand, (x, y) =>
            {
                float distanceX = Vector3.Distance(x.transform.position, leftGrabPointReference[chosenReference].position);
                float distanceY = Vector3.Distance(y.transform.position, leftGrabPointReference[chosenReference].position);
                return distanceX.CompareTo(distanceY);
            });
            
            System.Array.Sort(grabbablePositionsRightHand, (x, y) =>
            {
                float distanceX = Vector3.Distance(x.transform.position, rightGrabPointReference[chosenReference].position);
                float distanceY = Vector3.Distance(y.transform.position, rightGrabPointReference[chosenReference].position);
                return distanceX.CompareTo(distanceY);
            });
            
            if(grabbablePositionsLeftHand[0].position == currentHandSpotLeft && grabbablePositionsLeftHand.Length > 1)
            {
                arrayPosLeftHand++;
            }
            
            if(grabbablePositionsRightHand[0].position == currentHandSpotRight && grabbablePositionsRightHand.Length > 1)
            {
                arrayPosRightHand++;
            }
         
            targetSpotLeftHand = grabbablePositionsLeftHand[arrayPosLeftHand];
            targetSpotRightHand = grabbablePositionsRightHand[arrayPosRightHand];

           
            targetSpotLeftHand.gameObject.GetComponent<Renderer>().material = newMaterialRefL;
            targetSpotRightHand.gameObject.GetComponent<Renderer>().material = newMaterialRefR;
            
            middlePoint = GetMiddlePoint(targetSpotLeftHand.position, targetSpotRightHand.position);
        }
    }
    private void LerpRightHandToTarget()
    {

    }
    private void LerpLeftHandToTarget()
    {

    }
    private Vector3 GetMiddlePoint(Vector3 leftHandPos, Vector3 rightHandPos)
    {
        float x = (leftHandPos.x + rightHandPos.x) / 2;
        float y = (leftHandPos.y + rightHandPos.y) / 2;
        float z = (leftHandPos.z + rightHandPos.z) / 2;
        Vector3 newPos = new Vector3(x,y,z);

        return newPos;
    }
    public Transform GetTargetSpotLeftHand()
    {
        return targetSpotLeftHand;
    }
    public Transform GetTargetSpotRightHand()
    {
        return targetSpotRightHand;
    }
    public Vector3 GetCurrentSpotLeftHand()
    {
        return currentHandSpotLeft;
    }
    public void SetCurrentSpotLeftHand(Vector3 _newCurrentSpotLeft)
    {
        currentHandSpotLeft = _newCurrentSpotLeft;
    }
    public void SetCurrentSpotRightHand(Vector3 _newCurrentSpotRight)
    {
        currentHandSpotRight = _newCurrentSpotRight;
    }
    public Vector3 GetCurrentSpotRightHand()
    {
        return currentHandSpotRight;
    }
    public void SetMovementDirection(Vector2 _moveDirection)
    {
        movementDirection = _moveDirection;
    }
    public Vector3 GetMiddlePoint()
    {
        currentHandSpotLeft = targetSpotLeftHand.position;
        currentHandSpotRight = targetSpotRightHand.position; 
        downwardClimbing.SetCurrentFootSpot();
        return middlePoint;
    }
    public bool GetCanClimb()
    {
        return canClimb;
    }
    public Vector2 GetMovementDirection()
    {
        return movementDirection;
    }
    
}
