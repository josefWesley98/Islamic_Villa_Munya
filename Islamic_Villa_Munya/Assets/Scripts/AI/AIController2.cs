using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController2 : MonoBehaviour
{
    [SerializeField] private NPCManager manager;
    [SerializeField] private int id = 0;
    [SerializeField] private bool isMovingArea = false;
    [SerializeField] private bool isIdle = false;
    [SerializeField] private bool isWandering = false;
    [SerializeField] private bool isSocialising = false;
    [SerializeField] private bool isMovingToSocialArea = false;
    [SerializeField] float maxSpeed = 3.5f;
    [SerializeField] float minSpeed = 2.0f;
    [SerializeField] float waitTimeMax = 7.0f;
    [SerializeField] float waitTimeMin = 1.0f;
    [SerializeField] bool arrivedAtDestination = false;
    private Vector3 destination = Vector3.zero;
    private UnityEngine.AI.NavMeshAgent agent;
    private Animator animator;
    private int currentWaypointPos = 0;

    [SerializeField] private string currentLocation = "null";
    private string[] locations = new string[] { "Dining_Room", "Relaxation_Area_Left", "Relaxation_Area_Right", "Room_Left", "Room_Right", "Upstairs_Area_One", "Upstairs_Area_Two"};
    [Dropdown("locations")]
    [SerializeField] private string goToLocation;
    
   
    [SerializeField] private string currentJob = "null";
    private string[] jobs = new string[] {"test job 1", "test job 2", "test job 3"};
    [Dropdown("jobs")]
    [SerializeField] private string newJob;
    // Start is called before the first frame update
    
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();

        if(currentLocation == "null")
        {
            currentLocation = manager.FindNearestLocation(transform);
            destination = manager.FindNewDestination(id, currentLocation, currentWaypointPos);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x == destination.x && transform.position.z == destination.z)
        {
            arrivedAtDestination = true;
        }
        if(arrivedAtDestination)
        {
            manager.FindNewDestination(id, currentLocation,currentWaypointPos);
        }
        if(currentLocation == "null")
        {
            manager.FindNearestLocation(transform);
        }
        if(currentLocation != goToLocation)
        {
            isMovingArea = true;
            isIdle = false;
            isSocialising = false;
            isMovingToSocialArea = false;
            isWandering = false;
        }
        else
        {
            isMovingArea = false;
        }
    }

    public void SetID(int _id)
    {
        id = _id;
    }

    public void SetGoToLocation(string _location)
    {
        for(int i = 0; i < locations.Length; i++)
        {
            if(locations[i] == _location)
            {
                goToLocation = locations[i];
            }
        }
    }
    public string GetCurrentLocation()
    {
        return currentLocation;
    }
    public void SetCurrentWayPointPos(int WP)
    {
        currentWaypointPos = WP;
    }
}
