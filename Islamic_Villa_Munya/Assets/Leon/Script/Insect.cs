using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Insect : MonoBehaviour
{
    //butterfly brain script. controls everything for this butterfly instance, including flight, animations, collisions etc
    public float dirForceMult = 2; // force that is applied during flight

    public float changeDirTimeMin = 1f; //minimum time until change of direction
    public float changeDirTimeMax = 4f;//maximum time until change of direction
    public float landedTimeMin = 5f; //minimum time to spend landed
    public float landedTimeMax = 12f;//maximum time to spend landed
    public float landCooldownTime = 5f; //after taking off, how long until can land again
    bool canLand = false; //whether we can currently land or not

    float flightTimer = 0f; //time in air
    float switchFlightTime; // timer for when to switch to flying
    float stateTimer = 0f; //time in current state
    float switchStateTime;//timer for when to switch state

    Rigidbody rb; //rigidbody, butterflies are physics based
    Collider col; //collider
    Animator anim; // animator

    GameObject lastObjectLandedOn;

    public LayerMask landLayers; //what layers can be landed on. player ground walls etc

    public Collider pointOfInterest; // optional object for butterfly to be attracted to (bowl of fruit)
    bool visitedPOI = false; // flag for already visited point of interest
    float minVisitedDistance = 1f; //minimum distance to count as visited
    int poiCounter = 0; // counter for times visited POI
    public int poiCountAmount = 10; //how many times to visit before losing interest
    float forgetPOITimer = 0f; //timer for reset POI interest
    public float poiMemoryMax = 20f; //max time to spend not interested in POI
    float poiMemory; //memory time
    Transform rotHelper; //rotation helped for butterfly model
    float rotSpeed = 0.004f; //speed to interpolate rotations
    bool setUpComplete = false; //check everything is ready
    Rigidbody playerRB; // the player body for player interaction

    //on enabled, subscribe to scene loaded event
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    //unsubscribe if this object disabled
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //work around for script not working properly when scene is loaded. waits 3 seconds before starting set up
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Invoke(nameof(SetUp), 3f);
    }

    //function for setting up various parameters
    private void SetUp()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        rotHelper = transform.Find("RotHelper");
        rotHelper.parent = null;
        rotHelper.gameObject.hideFlags = HideFlags.HideInHierarchy;

        if (pointOfInterest == null)
            visitedPOI = true;

        poiMemory = Random.Range(poiMemoryMax / 2, poiMemoryMax);
        //take off initially
        TakeOff();
        setUpComplete = true;
    }
    //the various states of the buttefly
    enum ButterflyState
    {
        Flying,
        Landed,
        VisitingPOI,
    }
    //default to flying
    ButterflyState state = ButterflyState.Flying;

    //butterflies work off of the physics update as they are rigidbodies
    void FixedUpdate()
    {
        //dont run if not set up
        if (!setUpComplete)
            return;

        //always interpolate to intended rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, rotHelper.rotation, Time.time * rotSpeed);

        //Butterfly State Machine
        switch (state)
        {
            //flying state
            case ButterflyState.Flying:

                if(!visitedPOI)//if we havent visited our POI (Point of Interest), switch to that state
                    ChangeState(ButterflyState.VisitingPOI);
                else if (pointOfInterest != null) //if POI exists
                {
                    forgetPOITimer += Time.deltaTime; //count up the forget timer
                    //if we forgot about the POI, reset the flag that we have been there already
                    if(forgetPOITimer > poiMemory)
                        visitedPOI = false;
                }
                //count up the state timer
                stateTimer += Time.deltaTime;

                //we can land if we are past the landing cooldown
                if (stateTimer > landCooldownTime)
                    canLand = true;
                //count up the flight timer
                flightTimer += Time.deltaTime;
                
                //if we are over the max flight time
                if (flightTimer > switchFlightTime)
                {
                    //add a new force in a random direction to change our flight path
                    rb.AddForce(Random.onUnitSphere * dirForceMult);
                    //randomise next switch flight directoin time
                    switchFlightTime = Random.Range(changeDirTimeMin, changeDirTimeMax);
                    //reset flight timer
                    flightTimer = 0f;
                }

                break;
            case ButterflyState.Landed:
                //count up the state timer
                stateTimer += Time.deltaTime;
                //if player exists
                if(playerRB != null)
                {
                    //if player is moving too fast, break out of the landed state by making the state timer over its limit. we cannot stay landed on a fast moving player
                    if (playerRB.velocity.magnitude > 2)
                        stateTimer = 999f;
                }
                // swap to flying state if state timer is over switch state time
                if (stateTimer > switchStateTime)
                {
                    TakeOff();
                    ChangeState(ButterflyState.Flying);
                }

                break;
            case ButterflyState.VisitingPOI:
                //count up state timer
                stateTimer += Time.deltaTime;
                //check if we can land yet
                if (stateTimer > landCooldownTime)
                    canLand = true;
                //count up flight timer
                flightTimer += Time.deltaTime;
                //if over flight timer limit
                if (flightTimer > switchFlightTime)
                {
                    //change direction
                    rb.AddForce(Random.onUnitSphere * dirForceMult);
                    //if not visited Point of Interest, add some force towards the direction of the POI
                    if (!visitedPOI)
                        rb.AddForce((pointOfInterest.transform.position - transform.position).normalized * dirForceMult);
                    //randomise next switch flight time
                    switchFlightTime = Random.Range(changeDirTimeMin, changeDirTimeMax);
                    //reset flight timer
                    flightTimer = 0f;

                    //if at POI, +1 to the POI counter
                    if (Vector3.Distance(pointOfInterest.transform.position, transform.position) < minVisitedDistance)
                    {
                        poiCounter++;         
                    }
                    //if we visited past the amount we want, reset and change state back to regular flying
                    if(poiCounter > poiCountAmount)
                    {
                        poiCounter = 0;
                        visitedPOI = true;
                        ChangeState(ButterflyState.Flying);
                    }
                }

                break;
        }
    }
    //function for reseting values when a state is changed
    void ChangeState(ButterflyState _state, float _minTimeInState = 0, float _maxTimeInState = 0)
    {
        stateTimer = 0;
        //pick a random time from a range to change state
        switchStateTime = Random.Range(_minTimeInState, _maxTimeInState);
        //change the state
        state = _state;
    }

    //collision detection for if butterfly should land
    private void OnCollisionEnter(Collision c)
    {
        //if we cant land on this layer or the previous landing was on this object, return
        if (c.gameObject.tag == "Player")
            playerRB = c.gameObject.GetComponent<Rigidbody>();
        else
            playerRB = null;
        //check the candidate object is on a valid layer
        if (!IsInLayerMask(c.gameObject, landLayers) || lastObjectLandedOn == c.gameObject || !canLand)
            return;
        //if all checks passed, change state to landed
        ChangeState(ButterflyState.Landed, landedTimeMin, landedTimeMax);
        //do the landing
        Land(c);
    }

    //set all values for landing. freezes the rigidbody, disables collisions, switch animations to landed, set transform related variables based on the landed on surface
    void Land(Collision c)
    {
        canLand = false;
        lastObjectLandedOn = c.gameObject;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.None;
        col.enabled = false;
        transform.parent = c.transform;
        transform.position += c.contacts[0].normal * -0.05f;
        rotHelper.rotation = Quaternion.LookRotation(c.contacts[0].point - transform.position, Vector3.up);
        rotHelper.Rotate(new Vector3(0, 0, Random.Range(-180f, 180f)));
        anim.SetBool("grounded", true);
        anim.speed = Random.Range(0.1f, 1.5f);
        switchStateTime = Random.Range(landedTimeMin, landedTimeMax);
    }

    //reset all values to be ready for flight. unfreezes the rigidbody, enables collisions,starts flight animations, resets transform related variables
    void TakeOff()
    {
        transform.parent = null;
        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        col.enabled = true;
        rotHelper.rotation = Quaternion.identity;
        rotHelper.Rotate(new Vector3(50, Random.Range(-180f, 180f), 0));
        anim.speed = 1;
        anim.SetBool("grounded", false);
        transform.localScale = Vector3.one;
    }

    //function for checking if the specified object is on the specified layer
    public bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return ((layerMask.value & (1 << obj.layer)) > 0);
    }
}
