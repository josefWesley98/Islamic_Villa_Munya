using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpwardClimbing : MonoBehaviour
{
    [SerializeField] private LayerMask climableLayer;
    // 0 Left, 1 Right.
    [SerializeField] private Transform[] shoulders;
    private Vector3 boxSize;
    private bool canClimb = false;
    bool m_Started;
    Quaternion boxRotation = Quaternion.identity;
    private Transform[] grabablePositionsLeftHand;
    private Transform[] grabablePositionsRightHand;
    private Transform targetSpotLeftHand;
    private Transform targetSpotRightHand;
    private Transform currentHandSpotLeft;
    private Transform currentHandSpotRight;
    private Vector2 movementDirection;

    void Start()
    {
        boxSize = new Vector3(1.0f,1.45f,0.61f);
        m_Started = true;
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
        Collider[] climableSpots = Physics.OverlapBox(transform.position, boxSize, transform.rotation, climableLayer);
        for(int i = 0; i < climableSpots.Length; i++)
        {
            grabablePositionsLeftHand[i] = climableSpots[i].gameObject.transform;
            grabablePositionsRightHand[i] = climableSpots[i].gameObject.transform;
        }

        if(climableSpots.Length == 0)
        {
            canClimb = true;
        }
        else
        {
            canClimb = true;
        }

        System.Array.Sort(grabablePositionsLeftHand, (x, y) =>
            {
                float distanceX = Vector3.Distance(x.transform.position, shoulders[0].position);
                float distanceY = Vector3.Distance(y.transform.position, shoulders[0].position);
                return distanceX.CompareTo(distanceY);
            });

        for(int i = 0; i < grabablePositionsLeftHand.Length; i++)
        {

        }

    }

    public Transform GetTargetSpotLeftHand()
    {
        return targetSpotLeftHand;
    }
    public Transform GetTargetSpotRightHand()
    {
        return targetSpotRightHand;
    }
    public Transform GetCurrentSpotLeftHand()
    {
        return currentHandSpotLeft;
    }
    public Transform GetCurrentSpotRightHand()
    {
        return currentHandSpotRight;
    }
    public void SetMovementDirection(Vector2 _moveDirection)
    {
        movementDirection = _moveDirection;
    }
}
