using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownwardClimbing : MonoBehaviour
{
    [SerializeField] private LayerMask climableLayer;
    [SerializeField] private UpwardClimbing upwardClimbing; 
    [SerializeField] private Transform[] grabbablePositionsRightFoot;
    [SerializeField] private Transform[] grabbablePositionsLeftFoot;
    [SerializeField] private Transform[] rightGrabPointReference;
    [SerializeField] private Transform[] leftGrabPointReference;
    [SerializeField] private Material newMaterialRefL;
    [SerializeField] private Material newMaterialRefR;
    private bool m_Started;
    private Transform targetSpotLeftFoot;
    private Transform targetSpotRightFoot;
    private Vector3 currentFootSpotLeft = Vector3.zero;
    private Vector3 currentFootSpotRight = Vector3.zero;
    private int arrayPosLeftFoot = 0;
    private int arrayPosRightFoot = 0;
    private int chosenReference = 0;
    private Vector2 movementDirection;
    private bool gotFootHolds = false;
    // Start is called before the first frame update
    void Start()
    {
        m_Started = true; 
        for(int i = 0; i < 50; i++)
        {
            grabbablePositionsLeftFoot[i] = gameObject.transform;
            grabbablePositionsRightFoot[i] = gameObject.transform;
        }
    }
    void Update()
    {
       DoClimbing();
       movementDirection = upwardClimbing.GetMovementDirection();
    }

    private void DoClimbing()
    {
        Collider[] climableSpots = Physics.OverlapBox(transform.position, transform.localScale, transform.rotation, climableLayer);
        
        for(int i = 0; i < climableSpots.Length; i++)
        {
            grabbablePositionsRightFoot[i] = climableSpots[i].gameObject.transform;
            grabbablePositionsLeftFoot[i] = climableSpots[i].gameObject.transform;
        }

        if(climableSpots.Length == 0)
        {
            gotFootHolds = false;
            Debug.Log("no Foot holds");
        }
        else 
        {
            gotFootHolds = true;
        }

        if(upwardClimbing.GetCanClimb() && gotFootHolds)
        {
             Debug.Log("Foot holds Found");
            arrayPosLeftFoot = 0;
            arrayPosRightFoot = 0;

            if(movementDirection.y > 0)//Up
            {
                if(movementDirection.x > 0)//Up Right
                {
                    chosenReference = 5;
                }
                if(movementDirection.x < 0)//Up Left
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
                if(movementDirection.x > 0)//Down Right
                {
                    chosenReference = 7;
                }
                if(movementDirection.x < 0)//Down Left
                {
                    chosenReference = 6;
                }
                if(movementDirection.x == 0)// down
                {
                chosenReference = 1;
                }
            }     

            if(movementDirection.x > 0 && movementDirection.y == 0)
            {
                chosenReference = 2;
                Debug.Log("just moving right");
            }
            if(movementDirection.x < 0 && movementDirection.y == 0)
            {
                chosenReference = 3;
                Debug.Log("Just Moving Left");
            }
            System.Array.Sort(grabbablePositionsLeftFoot, (x, y) =>
                {
                    float distanceX = Vector3.Distance(x.transform.position, leftGrabPointReference[chosenReference].position);
                    float distanceY = Vector3.Distance(y.transform.position, leftGrabPointReference[chosenReference].position);
                    return distanceX.CompareTo(distanceY);
                });
            
            System.Array.Sort(grabbablePositionsRightFoot, (x, y) =>
                {
                    float distanceX = Vector3.Distance(x.transform.position, rightGrabPointReference[chosenReference].position);
                    float distanceY = Vector3.Distance(y.transform.position, rightGrabPointReference[chosenReference].position);
                    return distanceX.CompareTo(distanceY);
                });
                
            if(grabbablePositionsLeftFoot[0].position == currentFootSpotLeft && grabbablePositionsLeftFoot.Length > 1)
            {
                arrayPosLeftFoot++;
            }
            
            if(grabbablePositionsRightFoot[0].position == currentFootSpotRight && grabbablePositionsRightFoot.Length > 1)
            {
                arrayPosRightFoot++;
            }
            
            targetSpotLeftFoot = grabbablePositionsLeftFoot[arrayPosLeftFoot];
            targetSpotRightFoot = grabbablePositionsRightFoot[arrayPosRightFoot];

            targetSpotLeftFoot.gameObject.GetComponent<Renderer>().material = newMaterialRefL;
            targetSpotRightFoot.gameObject.GetComponent<Renderer>().material = newMaterialRefR;

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
        return currentFootSpotLeft;
    }
    public Vector3 GetCurrentSpotRightFoot()
    {
        return currentFootSpotRight;
    }
    public void SetMovementDirection(Vector2 _moveDirection)
    {
        movementDirection = _moveDirection;
    }
    public void SetCurrentFootSpot()
    {
        //currentFootSpotLeft = targetSpotLeftFoot.position;
        //currentFootSpotRight = targetSpotRightFoot.position;
    }
}
