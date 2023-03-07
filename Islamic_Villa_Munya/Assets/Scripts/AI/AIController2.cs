using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIController2 : MonoBehaviour
{
    [SerializeField] private NPCManager manager;
    [SerializeField] private int id = 0;
    [SerializeField] private SpawnAudioPrefabs spawnAudioPrefabs;
    // job checks
    private bool isInspecting = false;
    private bool isWalking = false;
    private bool isSocialising = false;
    private bool isSitting = false;
    private bool isIdling = false;

    // can do job checks
    private bool canDoInspecting = true;
    private bool canDoWalking = true;
    private bool canDoSocialising = true;
    private bool canDoSitting = true;
    private bool canDoIdling = true;

    float doInspectingFor = 30.0f;
    float doWalkingFor = 30.0f;
    float doSocialisingFor = 30.0f;
    float doSittingFor = 30.0f;
    float doIdlingFor = 30.0f;

    float actionTimer = 0.0f;

    // cooldowns after doing a job
    [SerializeField] private bool findNewJob = false;

    // bool doInspectingCd = false;
    // bool doWalkingCd = false;
    // bool doSocialisingCd = false;
    // bool doSittingCd = false;
    // bool doDoIdlingCd = false;

    float inspectionCdTimer = 0.0f;
    float walkingCdTimer = 0.0f;
    float socialisingCdTimer = 0.0f;
    float sittingCdTimer = 0.0f;
    float idlingCdTimer = 0.0f;
    float viewArtifactTimer = 0.0f;

    float inspectionCd = 30.0f;
    float walkingCd = 30.0f;
    float socialisingCd = 30.0f;
    float sittingCd = 30.0f;
    float idlingCd = 30.0f;
    float viewingArtifactCd = 7.0f;
    // // npc variables.
    [Header("Movement Speed")]
    [SerializeField] float maxSpeed = 1.5f;
    [SerializeField] float minSpeed = 2.5f;

    bool arrivedAtDestination = false;
    bool hasSocialPartner = false;
    private Vector3 destination =  new Vector3(1111,111,111);
    private NavMeshAgent agent;
    private Animator animator;
    private int currentWaypointPos = -1;
    private bool readyToSocialise = false;
    private bool socialPartnerLeftEarly = false;
    private bool doSitAnim = false;
    private bool doSocialAnim = false;
    private bool doWalkingAnim = false;
    private bool doInspectingAnim = false;
    private bool doIdlingAnim = false;
    private float inspectionTime = 8.0f;
    private float inspectionTimer = 0.0f;
    private bool findNewArtifact = false;
    private bool correctRotation = false;
    private bool rotatingLeft = false;
    private bool rotatingRight = false;
    private bool viewingArtifact = false;
    private bool doSocialAudio = false;
    private Vector3 socialDestination = Vector3.zero;
    private float slerpSpeed = 2.0f;
    private Vector3 direction = Vector3.zero;
    private Quaternion lookRotation = Quaternion.identity;
    private bool partnerHasArrived = false;
    private float slerpPercent = 0.0f;
    private Quaternion slerpStart = Quaternion.identity;
    private bool startedRotation = false;
    private string[] locations = new string[] { "AreaOne",  "AreaTwo", "AreaThree", "AreaFour", "AreaFive", "AreaSix", "AreaSeven"};
    [Dropdown("locations")]
    [Header("Location Options")]
    [SerializeField] private string goToLocation;
    [SerializeField] private string currentLocation = "null";

    [Header("Job Settings")]
    private string[] jobs = new string[] {"Walking", "Inspecting", "Socialising", "Idling", "Sitting"};
    [Dropdown("jobs")]
    [Header("Job Options")]
    [SerializeField] private string newJob;
    [SerializeField] private string currentJob = "null";

    [Header("Allowed Jobs")]
    [SerializeField] private bool allowWalkingAbout = true;
    [SerializeField] private bool allowInspecting = true;
    [SerializeField] private bool allowSocialising = true;
    [SerializeField] private bool allowIdling = true;
    [SerializeField] private bool allowSitting = true;
    private bool doIdleLean = false;
    private bool idleLeanLeft = false;
    private bool idleLeanRight = false;
    private bool idleLeanBack = false;
    private bool doSitDown = false;
    private bool doSitUp = false;
    private bool doingSitUp = false;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if(currentLocation == "null")
        {
            currentLocation = manager.FindNearestLocation(id);
            agent.avoidancePriority = 50;
        }
    }
    private void AnimationManagement()
    {
        if(animator != null)
        {
            int rand = Random.Range(0,3);
            animator.SetInteger("SetAnimStartNumber", rand);
            animator.SetBool("Walking", doWalkingAnim);
            animator.SetBool("SittingUp", doSitUp);
            animator.SetBool("SittingDown", doSitDown);
            animator.SetBool("Inspecting", doInspectingAnim);
            animator.SetBool("Socialising", doSocialAnim);
            animator.SetBool("Idling", doIdlingAnim);
            // animator.SetBool("TurningLeft", rotatingLeft);
            // animator.SetBool("TurningRight", rotatingRight);
            animator.SetBool("IdleLeanLeft", idleLeanLeft);
            animator.SetBool("IdleLeanRight", idleLeanRight);
            animator.SetBool("IdleLeanBack", idleLeanBack);
            animator.SetBool("Inspecting", doInspectingAnim);
        }
        if(doInspectingAnim)
        {
            idleLeanLeft = false;
            idleLeanRight = false;
            doWalkingAnim = false;
            doInspectingAnim = false;
            doSocialAnim = false;
            rotatingLeft = false;
            rotatingRight = false;
            idleLeanBack = true;
            doSitDown = false;
            doSitUp = false;
        }
        if(doSitUp && arrivedAtDestination)
        {
            idleLeanLeft= false;
            idleLeanRight = false;
            doWalkingAnim = false;
            doInspectingAnim = false;
            doSocialAnim = false;
            rotatingLeft = false;
            rotatingRight = false;
            idleLeanBack = true;
            doSitDown = false;
            doInspectingAnim = false;
        }
       
        if(doSitDown && arrivedAtDestination)
        {
            idleLeanLeft= false;
            idleLeanRight = false;
            doWalkingAnim = false;
            doInspectingAnim = false;
            doSocialAnim = false;
            rotatingLeft = false;
            rotatingRight = false;
            idleLeanBack = true;
            doSitUp = false;
            doInspectingAnim = false;
        }
        if(idleLeanBack && arrivedAtDestination)
        {
            idleLeanLeft= false;
            idleLeanRight = false;
            doWalkingAnim = false;
            doInspectingAnim = false;
            doSocialAnim = false;
            rotatingLeft = false;
            rotatingRight = false;
            doSitAnim = false;
            doSitDown = false;
            doSitUp = false;
            doInspectingAnim = false;
        }
        if(idleLeanLeft && arrivedAtDestination)
        {
            idleLeanBack = false;
            idleLeanRight = false;
            doWalkingAnim = false;
            doInspectingAnim = false;
            doSocialAnim = false;
            rotatingLeft = false;
            rotatingRight = false;
            doSitAnim = false;
            doSitDown = false;
            doSitUp = false;
            doInspectingAnim = false;
        }
        if(idleLeanRight && arrivedAtDestination)
        {
            idleLeanBack = false;
            idleLeanLeft = false;
            doWalkingAnim = false;
            doInspectingAnim = false;
            doSocialAnim = false;
            rotatingLeft = false;
            rotatingRight = false;
            doSitAnim = false;
            doSitDown = false;
            doSitUp = false;
            doInspectingAnim = false;
        }

        if(doWalkingAnim && !doingSitUp)
        {
            agent.avoidancePriority = 50;
            agent.speed = Random.Range(minSpeed, maxSpeed);
            idleLeanBack = false;
            idleLeanLeft= false;
            idleLeanRight = false;
            doSitAnim = false;
            doInspectingAnim = false;
            doSocialAnim = false;
            doIdlingAnim = false;
            rotatingLeft = false;
            rotatingRight = false;
            doSitDown = false;
            doSitUp = false;
            doInspectingAnim = false;
        }
        else
        {
            agent.speed = 0.0f;
            agent.avoidancePriority = 10;
        }

        if(doInspectingAnim)
        { 
            idleLeanBack = false;
            idleLeanLeft= false;
            idleLeanRight = false;
            rotatingLeft = false;
            rotatingRight = false;
            doWalkingAnim = false;
            doSitAnim = false;
            doSocialAnim = false;
            doIdlingAnim = false;
            doSitDown = false;
            doSitUp = false;
            doInspectingAnim = false;
        }
        if(doSocialAnim)
        {
          
              idleLeanBack = false;
            idleLeanLeft= false;
            idleLeanRight = false;
            rotatingLeft = false;
            rotatingRight = false;
            doWalkingAnim = false;
            doSitAnim = false;
            doInspectingAnim = false;
            doIdlingAnim = false;
            doSitDown = false;
            doSitUp = false;
            doInspectingAnim = false;
        }
        if(doIdlingAnim)
        {
            rotatingLeft = false;
            rotatingRight = false;
            doWalkingAnim = false;
            doSitAnim = false;
            doInspectingAnim = false;
            doSocialAnim = false;
            doSitDown = false;
            doSitUp = false;
            doInspectingAnim = false;
        }

        if(!arrivedAtDestination)
        {
            doWalkingAnim = true;
        }
        else
        {
            doWalkingAnim = false;
        }

    }    
    void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("SitUp"))
        {
            agent.speed = 0.0f;
            agent.avoidancePriority = 10;
            doingSitUp = true;
        }
        else
        {
            doSitUp = false;
            doingSitUp = false;
        }
        
        if(currentLocation == "null")
        {
            manager.FindNearestLocation(id);
            Debug.Log("finding current location");
        }
        if(currentJob == null || currentJob == "null")
        {
            findNewJob = true;
        }
       
        if(agent.remainingDistance > 0.1)
        {
            arrivedAtDestination = false;
        }
        if(currentJob != "null" && agent.remainingDistance < 0.1 )
        {
           arrivedAtDestination = true; 
        }
        // if(transform.position.z == destination.z)// && transform.position.z == destination.z)
        // {
        //     transform.position.x == destination.x
            //arrivedAtDestination = true; 
        //     
        // }
        AnimationManagement();
        if(socialPartnerLeftEarly)
        {
            socialPartnerLeftEarly = false;
            isSocialising = false;
            canDoSocialising = false;
            actionTimer = 0.0f;
            findNewJob = true;
        }

        if(arrivedAtDestination && currentJob == "Walking" && currentLocation != "null" && isWalking)
        {
            destination = manager.FindNewDestination(id, currentLocation, currentWaypointPos, currentJob);
            agent.destination = destination;
            arrivedAtDestination = false;
        }
        
        if(arrivedAtDestination && currentJob == "Inspecting" && currentLocation != "null" && isInspecting && correctRotation && !viewingArtifact)
        {
            viewingArtifact = true;  
        }

      
       
        if(arrivedAtDestination && currentJob == "Inspecting" && !correctRotation || arrivedAtDestination &&  currentJob == "Socialising" && !correctRotation || arrivedAtDestination && currentJob == "Idling" && !correctRotation || arrivedAtDestination && currentJob == "Sitting" && !correctRotation)
        {
             Vector3 target = Vector3.zero;

            if(currentJob == "Inspecting")
            {
               target =  manager.GetChildPositionOfInspectionPos(id, currentLocation, currentWaypointPos);
            }
            if(currentJob == "Socialising")
            {
                target = manager.GetSocialPartnerPos(id, currentLocation, currentWaypointPos);
            }
            if(currentJob == "Idling")
            {
                target =  manager.GetChildPositionOfIdlePos(id, currentLocation, currentWaypointPos);
            }
            if(currentJob == "Sitting")
            {
                target = manager.GetChildPositionOfSeatPos(id, currentLocation, currentWaypointPos);
            }

            DoRotation(target);
        }

        // find new artifact to look at.
        if(findNewArtifact)
        {
            DoFindNewArtifact();
        }

        if(findNewJob && currentLocation != "null")
        {
           NewRandomJob();
        }
        // done
        if(currentJob != newJob)
        {
           NewJobSetup();
        }

        // socialising
        DoSocialising();

        // walking
        DoWalking();

        //sitting
        DoSitting();

        //inspecting
        DoInspecting();

        //idling
        DoIdling();
    }
    private void DoFindNewArtifact()
    {
        findNewArtifact = false;
        arrivedAtDestination = false;
        destination = manager.FindPointToInspect(id, currentLocation, currentWaypointPos, currentJob);
        agent.destination = destination;
    }
    private void DoSocialising()
    {
        if(currentJob == "Socialising")
        {

            if(!hasSocialPartner && arrivedAtDestination && !partnerHasArrived)
            {
                
                bool check = manager.CheckIfHasSocialPartner(id, currentLocation, currentWaypointPos);
                if(check)
                {
                    hasSocialPartner = true;
                }
                else
                {
                    
                    bool check2 = manager.FindIdleAIInAreaToSocialise(id, currentLocation, currentWaypointPos);
                    if(check2)
                    {
                        hasSocialPartner = true;
                    }
                    else
                    {
                        findNewJob = true;
                    }
                }

                doIdlingAnim = true;
            }
            if(hasSocialPartner && arrivedAtDestination && correctRotation && !partnerHasArrived)
            {
                partnerHasArrived = manager.CheckIfPartnerHasArrivedAtSocialDestination(id, currentLocation, currentWaypointPos);
                doIdlingAnim = true;
            }
            if(hasSocialPartner && arrivedAtDestination && correctRotation && partnerHasArrived)
            {
                isSocialising = true;
                actionTimer += Time.deltaTime;
                doSocialAnim = true;
                // do audio.
                if(doSocialAudio)
                {
                    int rand = Random.Range(0,2);
                    spawnAudioPrefabs.spawnAudioPrefab(id,rand ,true);
                    doSocialAudio = false;
                }

                if(actionTimer >= doSocialisingFor)
                {
                    manager.TellSocialPartnerAILeft(id, currentLocation, currentWaypointPos);
                    socialPartnerLeftEarly = false;
                    isSocialising = false;
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
    }
    private void DoWalking()
    {
        if(currentJob == "Walking" )
        {
            
            isWalking = true;
            actionTimer += Time.deltaTime;
            doWalkingAnim = true;

            if(actionTimer >= doWalkingFor)
            {
                isWalking = false;
                //canDoWalking = false;
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
    }
    private void DoIdling()
    {
        if(currentJob == "Idling" && isIdling)
        {
            if(arrivedAtDestination && correctRotation)
            {
                actionTimer += Time.deltaTime;

                doIdlingAnim = true;
                manager.CheckIdleLean(id, currentLocation, currentWaypointPos);
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
    private void DoSitting()
    {
        if(currentJob == "Sitting" && isSitting)
        {
            if(arrivedAtDestination && correctRotation)
            {

                if(!doSitDown)
                {
                    doSitDown = true;
                }
                actionTimer += Time.deltaTime;
                
                if(actionTimer >= doSittingFor)
                {
                    doSitDown = false;
                    doSitUp = true;
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
    }
    private void DoInspecting()
    {
        if(currentJob == "Inspecting" && isInspecting)
        {
            if(viewingArtifact)
            { 
                doInspectingAnim = true;
                viewArtifactTimer += Time.deltaTime;
                if(viewArtifactTimer >= viewingArtifactCd)
                {
                    viewArtifactTimer = 0.0f;
                    viewingArtifact = false;
                    findNewArtifact = true; 
                }
            }
            actionTimer += Time.deltaTime;

            if(actionTimer >= doInspectingFor)
            {
                findNewArtifact = false;
                isInspecting = false;
                canDoInspecting = false;
                actionTimer = 0.0f;
                findNewJob = true;
            }
        }
        if(!isInspecting && !canDoInspecting)
        {
            inspectionCdTimer += Time.deltaTime;
            
            if(inspectionCdTimer >= inspectionCd)
            {
                
                canDoInspecting = true;
                inspectionCdTimer = 0.0f;
            }
        }
    }
    private void DoRotation(Vector3 target)
    {
        if(!startedRotation)
        {
            slerpStart = gameObject.transform.rotation;
            startedRotation = true;

            // if(transform.position.z > target.z && transform.position.x > target.x)
            // {
            //     rotatingLeft = true;
            // }
            // if(transform.position.z < target.z && transform.position.x < target.x)
            // {
            //     rotatingRight = true;
            // }
            // if(transform.position.z > target.z && transform.position.x < target.x)
            // {
            //     rotatingRight = true;
            // }
            // if(transform.position.z < target.z && transform.position.x > target.x)
            // {
            //     rotatingLeft = true;
            //}
        }
        direction = (target - transform.position).normalized;
 
        lookRotation = Quaternion.LookRotation(direction);
        
        slerpPercent = Mathf.MoveTowards(slerpPercent, 1f, Time.deltaTime * slerpSpeed);

        transform.rotation = Quaternion.Slerp(slerpStart, lookRotation, slerpPercent);
        
        if(slerpPercent >= 1.0f)
        {
            correctRotation = true;
            startedRotation = false;
            slerpPercent = 0f;
            direction = Vector3.zero;
            lookRotation = Quaternion.identity;
            
        }
    }
    private void NewRandomJob()
    {
         findNewJob = false;

        for(int i = 0; ; i++)
        {
            int rand = Random.Range(1, 12);

            if(rand == 1 && canDoWalking && allowWalkingAbout && manager.CheckForWalkingSpotsAvailable(currentLocation))
            {
                newJob = "Walking";
                break;
            }
            if(rand >= 2 && rand <= 3 && canDoSocialising && allowSocialising && manager.CheckForSocialSpotsAvailable(currentLocation))
            {
                newJob = "Socialising";
                break;
            }
            if(rand >= 4 && rand <= 5 && canDoSitting && allowSitting && manager.CheckForSeatingSpotsAvailable(currentLocation))
            {
                newJob = "Sitting"; 
                break;
            }
            if(rand >= 6 && rand <= 9 && canDoInspecting && allowInspecting && manager.CheckForInspectionSpotsAvailable(currentLocation))
            {
                newJob = "Inspecting";
                break;
            }
            if(rand == 10 && rand <= 12 && canDoIdling && allowIdling && manager.CheckForIdleSpotsAvailable(currentLocation))
            {
                newJob = "Idling";
                break;
            }
        }
    }
    private void NewJobSetup()
    {
        if(newJob == "Walking" && allowWalkingAbout && manager.CheckForWalkingSpotsAvailable(currentLocation))
        {
            correctRotation = false;
            arrivedAtDestination = true;
            isIdling = false;
            isSocialising = false;
            isInspecting = false;
            isSitting = false;
            isWalking = true;
            //canDoWalking = false;
            walkingCdTimer = 0.0f;
            actionTimer = 0.0f;
            doWalkingFor = ActionRandomTimer();
            walkingCd = ActionCdRandomTimer();
            Debug.Log("walking job setup done");
        }
        else if(newJob == "Walking" && !allowWalkingAbout || newJob == "Walking" && !manager.CheckForWalkingSpotsAvailable(currentLocation))
        {
            findNewJob = true;
        }
        if(newJob == "Inspecting" && allowInspecting && manager.CheckForInspectionSpotsAvailable(currentLocation))
        {
            correctRotation = false;
            arrivedAtDestination = false;
            isIdling = false;
            isSocialising = false;
            isSitting = false;
            isWalking = false;
            isInspecting = true;
            canDoIdling = false;
            idlingCdTimer = 0.0f;
            actionTimer = 0.0f;
            destination = manager.FindPointToInspect(id, currentLocation, currentWaypointPos, currentJob);
            agent.destination = destination;
            doIdlingFor = ActionRandomTimer();
            idlingCd = ActionCdRandomTimer();
        }
        else if(newJob == "Inspecting" && !allowInspecting || newJob == "Inspecting" && !manager.CheckForInspectionSpotsAvailable(currentLocation))
        {
            findNewJob = true;
        }
        if(newJob == "Sitting" && allowSitting && manager.CheckForSeatingSpotsAvailable(currentLocation))
        {
            correctRotation = false;
            arrivedAtDestination = false;
            isIdling = false;
            isSocialising = false;
            isInspecting = false;
            isWalking = false;
            isSitting = true;
            canDoSitting = false;
            sittingCdTimer = 0.0f;
            actionTimer = 0.0f;
            destination = manager.FindSeat(id, currentLocation, currentWaypointPos, currentJob);
            agent.destination = destination;
            doSittingFor = ActionRandomTimer();
            sittingCd = ActionCdRandomTimer();
        }
        else if(newJob == "Sitting" && allowSitting || newJob == "Sitting" && !manager.CheckForSeatingSpotsAvailable(currentLocation))
        {
            findNewJob = true;
        }
        if(newJob == "Socialising" && allowSocialising && manager.CheckForSocialSpotsAvailable(currentLocation))
        {
            correctRotation = false;
            isIdling = false;
            isInspecting = false;
            isSitting  = false;
            arrivedAtDestination = false;
            isWalking = false;
            isSocialising = true;
            canDoSocialising = false;
            socialisingCdTimer = 0.0f;
            actionTimer = 0.0f;
            doSocialisingFor = ActionRandomTimer();
            socialisingCd = ActionCdRandomTimer();
            doSocialAudio = true;
            if(socialDestination != Vector3.zero)
            {
                hasSocialPartner = true;
                destination = socialDestination;
                agent.destination = destination;
                partnerHasArrived = true;
            }
            if(socialDestination == Vector3.zero)
            {           
                destination = manager.FindSocialSpot(id, currentLocation, currentWaypointPos, currentJob);
                agent.destination = destination;
                hasSocialPartner = false;
                partnerHasArrived = false;
            }
        }
        else if(newJob == "Socialising" && !allowSocialising || newJob == "Socialising" && !manager.CheckForSocialSpotsAvailable(currentLocation))
        {
            findNewJob = true;
        }

        if(newJob == "Idling"  && allowIdling && manager.CheckForIdleSpotsAvailable(currentLocation))
        { 
            correctRotation = false;
            arrivedAtDestination = false;
            isSocialising = false;
            isInspecting = false;
            isSitting = false;
            isWalking = false;
            isIdling = true;
            canDoIdling = false;
            idlingCdTimer = 0.0f;
            actionTimer = 0.0f;
            destination = manager.FindIdleSpot(id, currentLocation, currentWaypointPos, currentJob);
            agent.destination = destination;
            doIdlingFor = ActionRandomTimer();
            idlingCd = ActionCdRandomTimer();
            
        }
        else if(newJob == "Idling" && !allowIdling || newJob == "Idling" && !manager.CheckForIdleSpotsAvailable(currentLocation))
        {
            findNewJob = true;
        }

        socialDestination = Vector3.zero;
        if(!findNewJob)
        {
            currentJob = newJob;
        }
    }
    private float ActionRandomTimer()
    {
        float result = Random.Range(15.0f, 60.0f);
        return result; 
    }    
    private float ActionCdRandomTimer()
    {
        float result = Random.Range(45.0f, 90.0f);
        return result; 
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
    public int GetCurrentWayPointPos()
    {
        return currentWaypointPos;
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
    public void SetSocialDestination(Vector3 newDest)
    {
        socialDestination = newDest;
    }
    public bool GetIsSocialising()
    {
        return isSocialising;
    }
    public int GetId()
    {
        return id;
    }
    public string GetCurrentJob()
    {
        return currentJob;
    }
    public void SetPartnerHasArrived(bool change)
    {
        partnerHasArrived = change;
    }
    public bool GetHasArrivedAtDestination()
    {
        return arrivedAtDestination;
    }
    public void SetIdleLeaning(string idleLeanType)
    {
        if(idleLeanType == "IdleLeanLeft")
        {
            idleLeanLeft = true;
        }
        if(idleLeanType == "IdleLeanRight")
        {
            idleLeanRight = true;
        }
        if(idleLeanType == "IdleLeanBack")
        {     
            idleLeanBack = true;
        }
        doIdleLean = true;
    }
}
