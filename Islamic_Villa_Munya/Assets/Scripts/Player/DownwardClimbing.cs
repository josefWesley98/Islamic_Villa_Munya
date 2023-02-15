using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownwardClimbing : MonoBehaviour
{
    [SerializeField] private LayerMask climableLayer;
    [SerializeField] private Transform boxCentre;
    private Transform nextSpotLeftFoot;
    private Transform nextSpotRightFoot;
    private Transform currentFootSpotLeft;
    private Transform currentFootSpotRight;
    private Vector2 movementDirection;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    void OnTriggerStay(Collider collision)
    {
        // if(collision.gameObject.layer == climableLayer)
        // {
        //     Transform[] climbingSpots = collision.gameObject.transform;
        //     Debug.Log(climbingSpots.Length);
        // }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform GetNextSpotLeftFoot()
    {
        return nextSpotLeftFoot;
    }
    public Transform GetNextSpotRightFoot()
    {
        return nextSpotRightFoot;
    }
    public Transform GetCurrentSpotLeftFoot()
    {
        return currentFootSpotLeft;
    }
    public Transform GetCurrentSpotRightFoot()
    {
        return currentFootSpotRight;
    }
    public void SetMovementDirection(Vector2 _moveDirection)
    {
        movementDirection = _moveDirection;
    }
}
