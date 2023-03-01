using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController2 : MonoBehaviour
{
    [SerializeField] private NPCManager manager;
    [SerializeField] private int id = 0;

    // job checks
    [SerializeField] private bool isInsepecting = false;
    [SerializeField] private bool isWalking = false;
    [SerializeField] private bool isSocialising = false;
    [SerializeField] private bool isSitting = false;
    [SerializeField] private bool isIdling = false;

    // can do job checks
    [SerializeField] private bool canDoInsepecting = true;
    [SerializeField] private bool canDoWalking = true;
    [SerializeField] private bool canDoSocialising = true;
    [SerializeField] private bool canDoSitting = true;
    [SerializeField] private bool canDoIdling = true;

    float doInspectingFor = 30.0f;
    float doWalkingFor = 30.0f;
    float doSocialisingFor = 30.0f;
    float doSittingFor = 30.0f;
    float doIdlingFor = 30.0f;

    float actionTimer = 0.0f;

    // cooldowns after doing a job
    bool findNewJob = false;

    bool doInspectingCd = false;
    bool doWalkingCd = false;
    bool doSocialisingCd = false;
    bool doSittingCd = false;
    bool doDoIdlingCd = false;

    float inspectionCdTimer = 0.0f;
    float walkingCdTimer = 0.0f;
    float socialisingCdTimer = 0.0f;
    float sittingCdTimer = 0.0f;
    float idlingCdTimer = 0.0f;

    float inspectionCd = 30.0f;
    float walkingCd = 30.0f;
    float socialisingCd = 30.0f;
    float sittingCd = 30.0f;
    float idlingCd = 30.0f;

    // npc variables.
    [SerializeField] float maxSpeed = 3.5f;
    [SerializeField] float minSpeed = 2.0f;
    [SerializeField] float waitTimeMax = 7.0f;
    [SerializeField] float waitTimeMin = 1.0f;
    [SerializeField] bool arrivedAtDestination = false;
    [SerializeField] bool hasSocialPartner = false;
    private Vector3 destination = Vector3.zero;
    private UnityEngine.AI.NavMeshAgent agent;
    private Animator animator;
    private int currentWaypointPos = 0;
   
    private bool readyToSocialise = false;
    private bool socialPartnerLeftEarly = false;

    private bool doSitAnim = false;
    private bool doSocialAnim = false;
    private bool doWalkingAnim = false;
    private bool doInspectingAnim = false;
    private bool doIdlingAnim = false;

    private float inspectionTime = 8.0f;
    private float inspectiontimer = 0.0f;
    private bool findNewArtifact = false;

    private bool correctRotation = false;

    private Quaternion rotationHolder = Quaternion.identity;

    [SerializeField] private string currentLocation = "null";

    private string[] locations = new string[] { "AreaOne",  "AreaTwo", "AreaThree", "AreaFour", "AreaFive", "AreaSix", "AreaSeven"};
    [Dropdown("locations")]
    [SerializeField] private string goToLocation;
    
   
    [SerializeField] private string currentJob = "null";

    private string[] jobs = new string[] {"Walking", "Inspecting", "Socialising", "Idling", "Sitting"};
    [Dropdown("jobs")]
    [SerializeField] private string newJob;
    // Start is called before the first frame update
    
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();

        if(currentLocation == "null")
        {
            currentLocation = manager.FindNearestLocation(id);
            destination = manager.FindNewDestination(id, currentLocation, currentWaypointPos, currentJob);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(animator != null)
        {
            // remember to add get up amim.
            animator.SetBool("Walking", doWalkingAnim);
            animator.SetBool("Sitting", doSitAnim);
            animator.SetBool("Inspecting", doInspectingAnim);
            animator.SetBool("Socialising", doSocialAnim);
            animator.SetBool("Idling", doIdlingAnim);
        }

        if(doSitAnim)
        {
            doWalkingAnim = false;
            doInspectingAnim = false;
            doSocialAnim = false;
            doIdlingAnim = false;

        }
        if(doWalkingAnim)
        {
            doSitAnim = false;
            doInspectingAnim = false;
            doSocialAnim = false;
            doIdlingAnim = false;
        }
        if(doInspectingAnim)
        { 
            doWalkingAnim = false;
            doSitAnim = false;
            doSocialAnim = false;
            doIdlingAnim = false;
        }
        if(doSocialAnim)
        {
            doWalkingAnim = false;
            doSitAnim = false;
            doInspectingAnim = false;
            doIdlingAnim = false;
        }
        if(doIdlingAnim)
        {
            doWalkingAnim = false;
            doSitAnim = false;
            doInspectingAnim = false;
            doSocialAnim = false;
        }
        if(!arrivedAtDestination)
        {
            doWalkingAnim = true;
        }
        else
        {
            doWalkingAnim = false;
        }

        if(transform.position.x == destination.x && transform.position.z == destination.z)
        {
            arrivedAtDestination = true;
        }

        if(currentLocation == "null")
        {
            manager.FindNearestLocation(id);
        }

        if(arrivedAtDestination && currentJob == "Walking" && isWalking)
        {
            manager.FindNewDestination(id, currentLocation,currentWaypointPos,currentJob);
        }
        
        if(arrivedAtDestination && currentJob == "Inspecting" && isInsepecting)
        {
            findNewArtifact = true;
        }

        if(arrivedAtDestination && currentJob == "Inspecting" || currentJob == "Socialising" && !correctRotation)
        {
            
            Vector3 targetDirection = destination - transform.position;

            float singleStep = 2.0f * Time.deltaTime;

            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            Quaternion LD = Quaternion.EulerAngles(newDirection);
            transform.rotation = Quaternion.LookRotation(newDirection);

            if(transform.rotation == LD)
            {
                correctRotation = true;
            }
        }

        if(findNewArtifact)
        {
            inspectionTime += Time.deltaTime;
            if(inspectiontimer >= inspectionTime)
            {
                inspectiontimer = 0.0f;
                findNewArtifact = false;
                arrivedAtDestination = false;
                destination = manager.FindPointToInspect(id, currentLocation, currentWaypointPos, currentJob);
            }
        }

        if(findNewJob)
        {
            for(int i = 0; ; i++)
            {
                int rand = Random.Range(1, 5);
                if(rand == 1 && canDoWalking)
                {
                    newJob = "Walking";
                }
                if(rand == 2 && canDoSocialising)
                {
                    newJob = "Socialising";
                }
                if(rand == 3 && canDoSitting)
                {
                    newJob = "Sitting";
                }
                if(rand == 4 && canDoInsepecting)
                {
                    newJob = "Inspecting";
                }
                if(rand == 5 && canDoIdling)
                {
                    newJob = "Idling";
                }
            }

            arrivedAtDestination = false;
            findNewJob = false;
        }
        // done
        if(currentJob != newJob)
        {
           NewJobSetup();
        }

        // socialising
        if(currentJob == "Socialising" && isSocialising)
        {
            if(!hasSocialPartner)
            {
                //temporary.
                hasSocialPartner = true;
                // ask ai if they have one
                // if yes has social partner is true.
                // if not the ai will find one for them
            }
            if(hasSocialPartner)
            {
                if(socialPartnerLeftEarly)
                {
                    actionTimer = doSocialisingFor;
                }

                actionTimer += Time.deltaTime;
                doSocialAnim = true;

                if(actionTimer >= doSocialisingFor)
                {
                    correctRotation = false;
                    isSocialising = false;
                    hasSocialPartner = false;
                    canDoSocialising = false;
                    actionTimer = 0.0f;
                    findNewJob = true;
                }
            }
        }
        if(!isSocialising && !canDoSocialising)
        {
            socialisingCdTimer += Time.deltaTime;
            
            if(socialisingCdTimer >= socialisingCd)
            {
                canDoSocialising = true;
                socialisingCdTimer = 0.0f;
            }
        }

        // walking
        if(currentJob == "Walking" && isWalking)
        {
            actionTimer += Time.deltaTime;
            doWalkingAnim = true;
            if(actionTimer >= doWalkingFor)
            {
                isWalking = false;
                canDoWalking = false;
                actionTimer = 0.0f;
                findNewJob = true;
            }
        }

        if(!isWalking && !canDoWalking)
        {
            walkingCdTimer += Time.deltaTime;
            
            if(walkingCdTimer >= walkingCd)
            {
                canDoWalking = true;
                walkingCdTimer = 0.0f;
            }
        }

        //sitting
        if(currentJob == "Sitting" && isSitting)
        {
            if(arrivedAtDestination)
            {
                actionTimer += Time.deltaTime;
                doSitAnim = true;

                if(actionTimer >= doSittingFor)
                {
                    isSitting = false;
                    canDoSitting = false;
                    actionTimer = 0.0f;
                    findNewJob = true;
                }
            }
        }
        if(!isSitting && !canDoSitting)
        {
            sittingCdTimer += Time.deltaTime;
            
            if(sittingCdTimer >= sittingCd)
            {
                canDoSitting = true;
                sittingCdTimer = 0.0f;
            }
        }

        //inspecting
        if(currentJob == "Inspecting" && isInsepecting)
        {
            if(arrivedAtDestination)
            {
                doInspectingAnim = true;
            }

            actionTimer += Time.deltaTime;

            if(actionTimer >= doInspectingFor)
            {
                correctRotation = false;
                findNewArtifact = true;
                isInsepecting = false;
                canDoInsepecting = false;
                actionTimer = 0.0f;
                findNewJob = true;
            }
        }
        if(!isInsepecting && !canDoInsepecting)
        {
            inspectionCdTimer += Time.deltaTime;
            
            if(inspectionCdTimer >= inspectionCd)
            {
                canDoInsepecting = true;
                inspectionCdTimer = 0.0f;
            }
        }

        //idling
        if(currentJob == "Idling" && isIdling)
        {
            if(arrivedAtDestination)
            {
                actionTimer += Time.deltaTime;
                doIdlingAnim = true;
                if(actionTimer >= doIdlingFor)
                {
                    isIdling = false;
                    canDoIdling = false;
                    actionTimer = 0.0f;
                    findNewJob = true;
                }

            }
        }
        if(!isIdling && !canDoIdling)
        {
            idlingCdTimer += Time.deltaTime;
            
            if(idlingCdTimer >= idlingCd)
            {
                canDoIdling = true;
                idlingCdTimer = 0.0f;
            }
        }
    }

    //done
    private void NewJobSetup()
    {
        currentJob = newJob;
        if(newJob == "Walking")
        {
            isWalking = true;
            canDoWalking = false;
            walkingCdTimer = 0.0f;
            actionTimer = 0.0f;
            doWalkingFor = ActionRandomTimer();
            walkingCd = ActionCdRandomTimer();
        }
        if(newJob == "Inspecting")
        {
            isIdling = true;
            canDoIdling = false;
            idlingCdTimer = 0.0f;
            actionTimer = 0.0f;
            doIdlingFor = ActionRandomTimer();
            idlingCd = ActionCdRandomTimer();
        }
        if(newJob == "Sitting")
        {
            isSitting = true;
            canDoSitting = false;
            sittingCdTimer = 0.0f;
            actionTimer = 0.0f;
            doSittingFor = ActionRandomTimer();
            sittingCd = ActionCdRandomTimer();
        }
        if(newJob == "Socialising")
        {
            isSocialising = true;
            canDoSocialising = false;
            socialisingCdTimer = 0.0f;
            actionTimer = 0.0f;
            doSocialisingFor = ActionRandomTimer();
            socialisingCd = ActionCdRandomTimer();
        }
        if(newJob == "Idling")
        {
            isIdling = true;
            canDoIdling = false;
            idlingCdTimer = 0.0f;
            actionTimer = 0.0f;
            doIdlingFor = ActionRandomTimer();
            idlingCd = ActionCdRandomTimer();
        }
    }
    //done
    private float ActionRandomTimer()
    {
        float result = Random.Range(15.0f, 60.0f);
        return result; 
    }
    //done
    private float ActionCdRandomTimer()
    {
        float result = Random.Range(45.0f, 90.0f);
        return result; 
    }
    public void SetID(int _id)
    {
        id = _id;
    }
    
    // currently not in use.
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
    public bool ReadyToSocialise()
    {
        return readyToSocialise;
    }
    public void SetNewJob(string jobChange)
    {
        newJob = jobChange;
    }
    public void SetSocialPartnerLeftEarly(bool change)
    {
        socialPartnerLeftEarly = change;
    }

    public bool GetCanDoSocialise()
    {
        return canDoSocialising;
    }
}
