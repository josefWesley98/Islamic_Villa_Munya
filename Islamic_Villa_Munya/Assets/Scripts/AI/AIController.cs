using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIController : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 target;
    private Animator animator;
    [SerializeField] private Transform groundFloorObj;
    [SerializeField] private Transform[] groundFloorPositions;
    [SerializeField] private Transform firstFloorObj;
    [SerializeField] private Transform[] islandOnePositions;
    [SerializeField] private Transform secondFloorObj;
    [SerializeField] private Transform[] islandTwoPositions;
    [SerializeField] private Transform thirdFloorObj;
    [SerializeField] private Transform[] islandThreePositions;
    [SerializeField] private Transform forthObj;
    [SerializeField] private Transform[] islandFourPositions;
    [SerializeField] private int seed;
    float timerForNewFloor = 0.0f;
    float timeForNewFloor = 10.0f;
    int chanceForNewFloor = 65;
    int newPositionRandomiser = 0;
    
    //int chanceToWaitAtLocation = 50;
    float waitTimeMax = 3.0f;
    float waitTimeMin = 1.0f;
    float waitingTime = 0.0f;
    float waitingTimer = 0.0f;
    bool isWaiting = false;
    [SerializeField] private bool arrivedAtTarget = false;
    private NavMeshAgent agent;
    [SerializeField] bool onGroundFloor = false;
    [SerializeField] bool onFirstIsland = false;
    [SerializeField] bool onSecondIsland = false;
    [SerializeField] bool onThirdIsland = false;
    [SerializeField] bool onFourthIsland = false;
    bool goToGroundFloor = false;
    bool goToFirstIsland = false;
    bool goToSecondIsland = false;
    bool goToThirdIsland = false;
    bool goToFourthIsland = false;
    void Start()
    {
        //Random.InitState(seed);
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

    //     Transform[] allChildren0 = groundFloorObj.GetComponentsInChildren<Transform>();
    //     for(int i = 0; i < allChildren0.Length; i++)
    //     {
    //         groundFloorPositions[0] = allChildren0[0].transform;
    //     }
    //     Transform[] allChildren1 = firstFloorObj.GetComponentsInChildren<Transform>();
    //     for(int i = 0; i < allChildren1.Length; i++)
    //     {
    //         islandOnePositions[0] = allChildren1[0].transform;
    //     }
    //     Transform[] allChildren2 = secondFloorObj.GetComponentsInChildren<Transform>();
    //    for(int i = 0; i < allChildren2.Length; i++)
    //     {
    //         islandTwoPositions[0] = allChildren2[0].transform;
    //     }
    //     Transform[] allChildren3 = thirdFloorObj.GetComponentsInChildren<Transform>();
    //      for(int i = 0; i < allChildren3.Length; i++)
    //     {
    //         islandThreePositions[0] = allChildren3[0].transform;
    //     }
    //     Transform[] allChildren4 = forthObj.GetComponentsInChildren<Transform>();
    //     for(int i = 0; i < allChildren4.Length; i++)
    //     {
    //         islandFourPositions[0] = allChildren4[0].transform;
    //     }
        SetNewTargetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("IsWaiting", isWaiting);

        SetNewGoTo();
        if(transform.position != target && agent.destination != null)
        {
            agent.destination = target;
        }
        
        if(transform.position.x == target.x && transform.position.z == target.z)
        {
            int roll = Random.Range(0,1);
            if(roll == 0)
            {
                isWaiting = true;
                waitingTime = Random.Range(waitTimeMin, waitTimeMax);
            }
            else
            {
                arrivedAtTarget = true;
            }
        }

        if(isWaiting)
        {
            waitingTimer += Time.deltaTime;
            if(waitingTimer >= waitingTime)
            {
                waitingTimer = 0.0f;
                isWaiting = false;
                arrivedAtTarget = true;
               
            }
        }
        else
        {

        }

       
        if(arrivedAtTarget)
        {
            SetNewTargetPosition();    
        }
    }
    void SetNewGoTo()
    {
        timerForNewFloor += Time.deltaTime;
        if(timerForNewFloor >= timeForNewFloor)
        {
            timerForNewFloor = 0;
            int rand = Random.Range(0, 100);

            if(rand >= chanceForNewFloor)
            {
                if(onGroundFloor)
                {
                    goToFirstIsland = true;
                }
                if(onFirstIsland)
                {
                    int chance = Random.Range(0,1);
                    if(chance == 0)
                    {
                        goToGroundFloor = true;
                    }
                    else
                    {
                        goToSecondIsland = true;
                    }
                }
                if(onSecondIsland)
                {
                    int chance = Random.Range(0,2);
                    if(chance == 0)
                    {
                        goToFirstIsland = true;
                    }
                    else
                    {
                        goToThirdIsland = true;
                    }
                }
                if(onThirdIsland)
                {
                    int chance = Random.Range(0,1);
                    if(chance == 0)
                    {
                        goToSecondIsland = true;
                    }
                    else
                    {
                        goToFourthIsland = true;
                    }
                }
                if(onFourthIsland)
                {
                    int chance = Random.Range(0,2);
                    if(chance == 0)
                    {
                        goToFirstIsland = true; 
                    }
                    else
                    {
                        goToThirdIsland = true;
                    }
                }
            }
        }

    }
    void SetNewTargetPosition()
    {
        newPositionRandomiser = 0;
        
        if(goToGroundFloor)
        {
            onGroundFloor = true;
            onFirstIsland = false;
            onSecondIsland = false;
            onThirdIsland = false;
            onFourthIsland = false;
            goToGroundFloor = false;
        }
        if(goToFirstIsland)
        {
            onGroundFloor = false;
            onFirstIsland = true;
            onSecondIsland = false;
            onThirdIsland = false;
            onFourthIsland = false;
            goToFirstIsland = false;
        }
        if(goToSecondIsland)
        {
            onGroundFloor = false;
            onFirstIsland = false;
            onSecondIsland = true;
            onThirdIsland = false;
            onFourthIsland = false;
            goToSecondIsland = false;
        }
        if(goToThirdIsland)
        {
            onGroundFloor = false;
            onFirstIsland = false;
            onSecondIsland = false;
            onThirdIsland = true;
            onFourthIsland = false;
            goToThirdIsland = false;
        }
        if(goToFourthIsland)
        {
            onGroundFloor = false;
            onFirstIsland = false;
            onSecondIsland = false;
            onThirdIsland = false;
            onFourthIsland = true;
            goToFourthIsland = false;
        }
        if(onGroundFloor)
        {
            newPositionRandomiser = Random.Range(0, groundFloorPositions.Length);
            target = groundFloorPositions[newPositionRandomiser].position;
            agent.destination = target;
        }
        if(onFirstIsland)
        {
            newPositionRandomiser = Random.Range(0, islandOnePositions.Length);
            target = islandOnePositions[newPositionRandomiser].position;
            agent.destination = target;
        }
        if(onSecondIsland)
        {
            newPositionRandomiser = Random.Range(0, islandTwoPositions.Length);
            target = islandTwoPositions[newPositionRandomiser].position;
            agent.destination = target;
        }
        if(onThirdIsland)
        {
            newPositionRandomiser = Random.Range(0, islandThreePositions.Length);
            target = islandThreePositions[newPositionRandomiser].position;
            agent.destination = target;
        }
        if(onFourthIsland)
        {
            newPositionRandomiser = Random.Range(0, islandFourPositions.Length);
            target = islandFourPositions[newPositionRandomiser].position;
            agent.destination = target;
        }
        arrivedAtTarget = false;
    }
    public void SetOnGroundFloor(bool _floor)
    {
        onGroundFloor = _floor;
    }
    public void SetOnFirstIsland(bool _floor)
    {
        onFirstIsland = _floor;
    }
    public void SetOnSecondIsland(bool _floor)
    {
        onSecondIsland = _floor;
    }
    public void SetOnThreeIsland(bool _floor)
    {
        onThirdIsland = _floor;
    }
    public void SetOnFourIsland(bool _floor)
    {
        onFourthIsland = _floor;
    }
}
