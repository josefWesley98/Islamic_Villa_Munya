using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    [Header("Areas")]
    [SerializeField] private GameObject areaOne;
    private string A1 = "Area_One";
    [SerializeField] private GameObject areaTwo;
    private string A2 = "Area_Two";
    [SerializeField] private GameObject areaThree;
    private string A3 = "Area_Three";
    [SerializeField] private GameObject areaFour;
    private string A4 = "Area_Four";
    [SerializeField] private GameObject areaFive;
    private string A5 = "Area_Five";
    [SerializeField] private GameObject areaSix;
    private string A6 = "Area_Six";
    [SerializeField] private GameObject areaSeven;
    private string A7 = "Area_Seven";
    private int locationCount = 7;
    //dining room
    [Header("Area One")]

        [Header("Walking")]
    [SerializeField] private Transform[] walkingWaypointsA1;
    [SerializeField] private bool[] walkingPointsA1Occupied;
    [SerializeField] private int[] AI_ID_A1_WP;

        [Header("Social")]
    [SerializeField] private Transform[] socialAreasA1;
    [SerializeField] private bool[] socialAreasA1Occupied;
    [SerializeField] private int[] AI_ID_A1_Social;

        [Header("Seating")]
    [SerializeField] private Transform[] seatingAreasA1;
    [SerializeField] private bool[] seatingAreasA1Occupied;
    [SerializeField] private int[] AI_ID_A1_Seating;

        [Header("Idling")]
    [SerializeField] private Transform[] idleAreasA1;
    [SerializeField] private bool[] idleAreasA1Occupied;
    [SerializeField] private int[] AI_ID_A1_Idle;

        [Header("Inspecting")]
    [SerializeField] private Transform[] inspectionAreasA1;
    [SerializeField] private bool[] inspectionAreasA1Occupied;
    [SerializeField] private int[] AI_ID_A1_Inspection;

    [Header("Area Two")]

        [Header("Walking")]
    [SerializeField] private Transform[] walkingWaypointsA2;
    [SerializeField] private bool[] walkingPointsA2Occupied;
    [SerializeField] private int[] AI_ID_A2_WP;

        [Header("Social")]
    [SerializeField] private Transform[] socialAreasA2;
    [SerializeField] private bool[] socialAreasA2Occupied;
    [SerializeField] private int[] AI_ID_A2_Social;
    
        [Header("Seating")]
    [SerializeField] private Transform[] seatingAreasA2;
    [SerializeField] private bool[] seatingAreasA2Occupied;
    [SerializeField] private int[] AI_ID_A2_Seating;

        [Header("Idling")]
    [SerializeField] private Transform[] idleAreasA2;
    [SerializeField] private bool[] idleAreasA2Occupied;
    [SerializeField] private int[] AI_ID_A2_Idle;

        [Header("Inspecting")]
    [SerializeField] private Transform[] inspectionAreasA2;
    [SerializeField] private bool[] inspectionAreasA2Occupied;
    [SerializeField] private int[] AI_ID_A2_Inspection;
 
    [Header("Area Three")]

        [Header("Walking")]
    [SerializeField] private Transform[] walkingWaypointsA3;
    [SerializeField] private bool[] walkingPointsA3Occupied;
    [SerializeField] private int[] AI_ID_A3_WP;

        [Header("Social")]
    [SerializeField] private Transform[] socialAreasA3;
    [SerializeField] private bool[] socialAreasA3Occupied;
    [SerializeField] private int[] AI_ID_A3_Social;
    
        [Header("Seating")]
    [SerializeField] private Transform[] seatingAreasA3;
    [SerializeField] private bool[] seatingAreasA3Occupied;
    [SerializeField] private int[] AI_ID_A3_Seating;

        [Header("Idling")]
    [SerializeField] private Transform[] idleAreasA3;
    [SerializeField] private bool[] idleAreasA3Occupied;
    [SerializeField] private int[] AI_ID_A3_Idle;

        [Header("Inspecting")]
    [SerializeField] private Transform[] inspectionAreasA3;
    [SerializeField] private bool[] inspectionAreasA3Occupied;
    [SerializeField] private int[] AI_ID_A3_Inspection;
    
    [Header("Area Four")]

        [Header("Walking")]
    [SerializeField] private Transform[] walkingWaypointsA4;
    [SerializeField] private bool[] walkingPointsA4Occupied;
    [SerializeField] private int[] AI_ID_A4_WP;

        [Header("Social")]
    [SerializeField] private Transform[] socialAreasA4;
    [SerializeField] private bool[] socialAreasA4Occupied;
    [SerializeField] private int[] AI_ID_A4_Social;
    
        [Header("Seating")]
    [SerializeField] private Transform[] seatingAreasA4;
    [SerializeField] private bool[] seatingAreasA4Occupied;
    [SerializeField] private int[] AI_ID_A4_Seating;

        [Header("Idling")]
    [SerializeField] private Transform[] idleAreasA4;
    [SerializeField] private bool[] idleAreasA4Occupied;
    [SerializeField] private int[] AI_ID_A4_Idle;

        [Header("Inspecting")]
    [SerializeField] private Transform[] inspectionAreasA4;
    [SerializeField] private bool[] inspectionAreasA4Occupied;
    [SerializeField] private int[] AI_ID_A4_Inspection;

    [Header("Area Five")]

        [Header("Walking")]
    [SerializeField] private Transform[] walkingWaypointsA5;
    [SerializeField] private bool[] walkingPointsA5Occupied;
    [SerializeField] private int[] AI_ID_A5_WP;

        [Header("Social")]
    [SerializeField] private Transform[] socialAreasA5;
    [SerializeField] private bool[] socialAreasA5Occupied;
    [SerializeField] private int[] AI_ID_A5_Social;
    
        [Header("Seating")]
    [SerializeField] private Transform[] seatingAreasA5;
    [SerializeField] private bool[] seatingAreasA5Occupied;
    [SerializeField] private int[] AI_ID_A5_Seating;

        [Header("Idling")]
    [SerializeField] private Transform[] idleAreasA5;
    [SerializeField] private bool[] idleAreasA5Occupied;
    [SerializeField] private int[] AI_ID_A5_Idle;

        [Header("Inspecting")]
    [SerializeField] private Transform[] inspectionAreasA5;
    [SerializeField] private bool[] inspectionAreasA5Occupied;
    [SerializeField] private int[] AI_ID_A5_Inspection;

    [Header("Area Six")]

        [Header("Walking")]
    [SerializeField] private Transform[] walkingWaypointsA6;
    [SerializeField] private bool[] walkingPointsA6Occupied;
    [SerializeField] private int[] AI_ID_A6_WP;

        [Header("Social")]
    [SerializeField] private Transform[] socialAreasA6;
    [SerializeField] private bool[] socialAreasA6Occupied;
    [SerializeField] private int[] AI_ID_A6_Social;
    
        [Header("Seating")]
    [SerializeField] private Transform[] seatingAreasA6;
    [SerializeField] private bool[] seatingAreasA6Occupied;
    [SerializeField] private int[] AI_ID_A6_Seating;

        [Header("Idling")]
    [SerializeField] private Transform[] idleAreasA6;
    [SerializeField] private bool[] idleAreasA6Occupied;
    [SerializeField] private int[] AI_ID_A6_Idle;

        [Header("Inspecting")]
    [SerializeField] private Transform[] inspectionAreasA6;
    [SerializeField] private bool[] inspectionAreasA6Occupied;
    [SerializeField] private int[] AI_ID_A6_Inspection;

    
    [Header("Area Seven")]

        [Header("Walking")]
    [SerializeField] private Transform[] walkingWaypointsA7;
    [SerializeField] private bool[] walkingPointsA7Occupied;
    [SerializeField] private int[] AI_ID_A7_WP;

        [Header("Social")]
    [SerializeField] private Transform[] socialAreasA7;
    [SerializeField] private bool[] socialAreasA7Occupied;
    [SerializeField] private int[] AI_ID_A7_Social;
    
        [Header("Seating")]
    [SerializeField] private Transform[] seatingAreasA7;
    [SerializeField] private bool[] seatingAreasA7Occupied;
    [SerializeField] private int[] AI_ID_A7_Seating;

        [Header("Idling")]
    [SerializeField] private Transform[] idleAreasA7;
    [SerializeField] private bool[] idleAreasA7Occupied;
    [SerializeField] private int[] AI_ID_A7_Idle;

        [Header("Inspecting")]
    [SerializeField] private Transform[] inspectionAreasA7;
    [SerializeField] private bool[] inspectionAreasA7Occupied;
    [SerializeField] private int[] AI_ID_A7_Inspection;

    [Header("AI")]

    [SerializeField] private GameObject[] AI;
    private bool[] AIWalking;
    private bool[] AIIdling;
    private bool[] AISocialising;
    private bool[] AISitting;
    private bool[] AIInspecting;

    private Vector3 DefaultPos = new Vector3(1000.0f,1000.0f,1000.0f);
    
    void Awake()
    {
        
        for(int i = 0; i < AI.Length; i++)
        {
            if(AI[i].GetComponent<AIController2>() != null)
            {
                AI[i].GetComponent<AIController2>().SetID(i);
            }
        }
    }
    // Start is called before the first frame upA1te
    void Start()
    {
        
        // initalise dining area.
        if(areaOne != null)
        {
            int childrenWayP = areaOne.transform.GetChild(0).transform.childCount;
            for(int i = 0; i < childrenWayP; i++)
            {
                walkingWaypointsA1[i] = areaOne.transform.GetChild(0).transform.GetChild(i).transform;
                AI_ID_A1_WP[i] = -1;
                walkingPointsA1Occupied[i] = false;
            }

            int childrenSocialP = areaOne.transform.GetChild(1).transform.childCount;
            for(int i = 0; i < childrenSocialP; i++)
            {
                socialAreasA1[i] = areaOne.transform.GetChild(1).transform.GetChild(i).transform;
                AI_ID_A1_Social[i] = -1;
                socialAreasA1Occupied[i] = false;
            }

            int childrenSeatingP = areaOne.transform.GetChild(2).transform.childCount;
            for(int i = 0; i < childrenSeatingP; i++)
            {
                idleAreasA1[i] = areaOne.transform.GetChild(2).transform.GetChild(i).transform;
                AI_ID_A1_Idle[i] = -1;
                idleAreasA1Occupied[i] = false;
            }
            
            int childrenIdleP = areaOne.transform.GetChild(3).transform.childCount;
            for(int i = 0; i < childrenIdleP; i++)
            {
                seatingAreasA1[i] = areaOne.transform.GetChild(3).transform.GetChild(i).transform;
                AI_ID_A1_Seating[i] = -1;
                seatingAreasA1Occupied[i] = false;
            }

            int childrenInspectingP = areaOne.transform.GetChild(4).transform.childCount;
            for(int i = 0; i < childrenInspectingP; i++)
            {
                inspectionAreasA1[i] = areaOne.transform.GetChild(4).transform.GetChild(i).transform;
                AI_ID_A1_Inspection[i] = -1;
                inspectionAreasA1Occupied[i] = false;
            }
        }
        else
        {
            areaOne.transform.position = DefaultPos;
        }
        // initalise relaxation area left.
        if(areaTwo != null)
        {
           int childrenWayP = areaTwo.transform.GetChild(0).transform.childCount;
            for(int i = 0; i < childrenWayP; i++)
            {
                walkingWaypointsA2[i] = areaTwo.transform.GetChild(0).transform.GetChild(i).transform;
                AI_ID_A2_WP[i] = -1;
                walkingPointsA2Occupied[i] = false;
            }

            int childrenSocialP = areaTwo.transform.GetChild(1).transform.childCount;
            for(int i = 0; i < childrenSocialP; i++)
            {
                socialAreasA2[i] = areaTwo.transform.GetChild(1).transform.GetChild(i).transform;
                AI_ID_A2_Social[i] = -1;
                socialAreasA2Occupied[i] = false;
            }

            int childrenSeatingP = areaTwo.transform.GetChild(2).transform.childCount;
            for(int i = 0; i < childrenSeatingP; i++)
            {
                idleAreasA2[i] = areaTwo.transform.GetChild(2).transform.GetChild(i).transform;
                AI_ID_A2_Idle[i] = -1;
                idleAreasA2Occupied[i] = false;
            }
            
            int childrenIdleP = areaTwo.transform.GetChild(3).transform.childCount;
            for(int i = 0; i < childrenIdleP; i++)
            {
                seatingAreasA2[i] = areaTwo.transform.GetChild(3).transform.GetChild(i).transform;
                AI_ID_A2_Seating[i] = -1;
                seatingAreasA2Occupied[i] = false;
            }

            int childrenInspectingP = areaTwo.transform.GetChild(4).transform.childCount;
            for(int i = 0; i < childrenInspectingP; i++)
            {
                inspectionAreasA2[i] = areaTwo.transform.GetChild(4).transform.GetChild(i).transform;
                AI_ID_A2_Inspection[i] = -1;
                inspectionAreasA2Occupied[i] = false;
            }
        }
        else
        {
            areaTwo.transform.position = DefaultPos;
        }
        // initalise relaxation area right.
        if(areaThree != null)
        {
            int childrenWayP = areaThree.transform.GetChild(0).transform.childCount;
            for(int i = 0; i < childrenWayP; i++)
            {
                walkingWaypointsA3[i] = areaThree.transform.GetChild(0).transform.GetChild(i).transform;
                AI_ID_A3_WP[i] = -1;
                walkingPointsA3Occupied[i] = false;
            }

            int childrenSocialP = areaThree.transform.GetChild(1).transform.childCount;
            for(int i = 0; i < childrenSocialP; i++)
            {
                socialAreasA3[i] = areaThree.transform.GetChild(1).transform.GetChild(i).transform;
                AI_ID_A3_Social[i] = -1;
                socialAreasA3Occupied[i] = false;
            }
             int childrenSeatingP = areaThree.transform.GetChild(2).transform.childCount;
            for(int i = 0; i < childrenSeatingP; i++)
            {
                idleAreasA3[i] = areaThree.transform.GetChild(2).transform.GetChild(i).transform;
                AI_ID_A3_Idle[i] = -1;
                idleAreasA3Occupied[i] = false;
            }
            
            int childrenIdleP = areaThree.transform.GetChild(3).transform.childCount;
            for(int i = 0; i < childrenIdleP; i++)
            {
                seatingAreasA3[i] = areaThree.transform.GetChild(3).transform.GetChild(i).transform;
                AI_ID_A3_Seating[i] = -1;
                seatingAreasA3Occupied[i] = false;
            }

            int childrenInspectingP = areaThree.transform.GetChild(4).transform.childCount;
            for(int i = 0; i < childrenInspectingP; i++)
            {
                inspectionAreasA3[i] = areaThree.transform.GetChild(4).transform.GetChild(i).transform;
                AI_ID_A3_Inspection[i] = -1;
                inspectionAreasA3Occupied[i] = false;
            }
        }
        else
        {
            areaThree.transform.position = DefaultPos;
        }
        // initalise left room.
        if(areaFour != null)
        {
            int childrenWayP = areaFour.transform.GetChild(0).transform.childCount;
            for(int i = 0; i < childrenWayP; i++)
            {
                walkingWaypointsA4[i] = areaFour.transform.GetChild(0).transform.GetChild(i).transform;
                AI_ID_A4_WP[i] = -1;
                walkingPointsA4Occupied[i] = false;
            }

            int childrenSocialP = areaFour.transform.GetChild(1).transform.childCount;
            for(int i = 0; i < childrenSocialP; i++)
            {
                socialAreasA4[i] = areaFour.transform.GetChild(1).transform.GetChild(i).transform;
                AI_ID_A4_Social[i] = -1;
                socialAreasA4Occupied[i] = false;
            }
             int childrenSeatingP = areaFour.transform.GetChild(2).transform.childCount;
            for(int i = 0; i < childrenSeatingP; i++)
            {
                idleAreasA4[i] = areaFour.transform.GetChild(2).transform.GetChild(i).transform;
                AI_ID_A4_Idle[i] = -1;
                idleAreasA4Occupied[i] = false;
            }
            
            int childrenIdleP = areaFour.transform.GetChild(3).transform.childCount;
            for(int i = 0; i < childrenIdleP; i++)
            {
                seatingAreasA4[i] = areaFour.transform.GetChild(3).transform.GetChild(i).transform;
                AI_ID_A4_Seating[i] = -1;
                seatingAreasA4Occupied[i] = false;
            }

            int childrenInspectingP = areaFour.transform.GetChild(4).transform.childCount;
            for(int i = 0; i < childrenInspectingP; i++)
            {
                inspectionAreasA4[i] = areaFour.transform.GetChild(4).transform.GetChild(i).transform;
                AI_ID_A4_Inspection[i] = -1;
                inspectionAreasA4Occupied[i] = false;
            }
        }
        else
        {
            areaFour.transform.position = DefaultPos;
        }
        // initialise right room.
        if(areaFive != null)
        {
            int childrenWayP = areaFive.transform.GetChild(0).transform.childCount;
            for(int i = 0; i < childrenWayP; i++)
            {
                walkingWaypointsA5[i] = areaFive.transform.GetChild(0).transform.GetChild(i).transform;
                AI_ID_A5_WP[i] = -1;
                walkingPointsA5Occupied[i] = false;
            }

            int childrenSocialP = areaFive.transform.GetChild(1).transform.childCount;
            for(int i = 0; i < childrenSocialP; i++)
            {
                socialAreasA5[i] = areaFive.transform.GetChild(1).transform.GetChild(i).transform;
                AI_ID_A5_Social[i] = -1;
                socialAreasA5Occupied[i] = false;
            }
             int childrenSeatingP = areaFive.transform.GetChild(2).transform.childCount;
            for(int i = 0; i < childrenSeatingP; i++)
            {
                idleAreasA5[i] = areaFive.transform.GetChild(2).transform.GetChild(i).transform;
                AI_ID_A5_Idle[i] = -1;
                idleAreasA5Occupied[i] = false;
            }
            
            int childrenIdleP = areaFive.transform.GetChild(3).transform.childCount;
            for(int i = 0; i < childrenIdleP; i++)
            {
                seatingAreasA5[i] = areaFive.transform.GetChild(3).transform.GetChild(i).transform;
                AI_ID_A5_Seating[i] = -1;
                seatingAreasA5Occupied[i] = false;
            }

            int childrenInspectingP = areaFive.transform.GetChild(4).transform.childCount;
            for(int i = 0; i < childrenInspectingP; i++)
            {
                inspectionAreasA5[i] = areaFive.transform.GetChild(4).transform.GetChild(i).transform;
                AI_ID_A5_Inspection[i] = -1;
                inspectionAreasA5Occupied[i] = false;
            }
        }
         else
        {
            areaFive.transform.position = DefaultPos;
        }
        // initalise upstairs area 1
        if(areaSix != null)
        {
            int childrenWayP = areaSix.transform.GetChild(0).transform.childCount;
            for(int i = 0; i < childrenWayP; i++)
            {
                walkingWaypointsA6[i] = areaSix.transform.GetChild(0).transform.GetChild(i).transform;
                AI_ID_A6_WP[i] = -1;
                walkingPointsA6Occupied[i] = false;
            }

            int childrenSocialP = areaSix.transform.GetChild(1).transform.childCount;
            for(int i = 0; i < childrenSocialP; i++)
            {
                socialAreasA6[i] = areaSix.transform.GetChild(1).transform.GetChild(i).transform;
                AI_ID_A6_Social[i] = -1;
                socialAreasA6Occupied[i] = false;
            }
             int childrenSeatingP = areaSix.transform.GetChild(2).transform.childCount;
            for(int i = 0; i < childrenSeatingP; i++)
            {
                idleAreasA6[i] = areaSix.transform.GetChild(2).transform.GetChild(i).transform;
                AI_ID_A6_Idle[i] = -1;
                idleAreasA6Occupied[i] = false;
            }
            
            int childrenIdleP = areaSix.transform.GetChild(3).transform.childCount;
            for(int i = 0; i < childrenIdleP; i++)
            {
                seatingAreasA6[i] = areaSix.transform.GetChild(3).transform.GetChild(i).transform;
                AI_ID_A6_Seating[i] = -1;
                seatingAreasA6Occupied[i] = false;
            }

            int childrenInspectingP = areaSix.transform.GetChild(4).transform.childCount;
            for(int i = 0; i < childrenInspectingP; i++)
            {
                inspectionAreasA6[i] = areaSix.transform.GetChild(4).transform.GetChild(i).transform;
                AI_ID_A6_Inspection[i] = -1;
                inspectionAreasA6Occupied[i] = false;
            }
        }
         else
        {
            areaSix.transform.position = DefaultPos;
        }
        // initalise upstairs area 2
        if(areaSeven != null)
        {
            int childrenWayP = areaSeven.transform.GetChild(0).transform.childCount;
            for(int i = 0; i < childrenWayP; i++)
            {
                walkingWaypointsA7[i] = areaSeven.transform.GetChild(0).transform.GetChild(i).transform;
                AI_ID_A7_WP[i] = -1;
                walkingPointsA7Occupied[i] = false;
            }

            int childrenSocialP = areaSeven.transform.GetChild(1).transform.childCount;
            for(int i = 0; i < childrenSocialP; i++)
            {
                socialAreasA7[i] = areaSeven.transform.GetChild(1).transform.GetChild(i).transform;
                AI_ID_A7_Social[i] = -1;
                socialAreasA7Occupied[i] = false;
            }
            int childrenSeatingP = areaSeven.transform.GetChild(2).transform.childCount;
            for(int i = 0; i < childrenSeatingP; i++)
            {
                idleAreasA7[i] = areaSeven.transform.GetChild(2).transform.GetChild(i).transform;
                AI_ID_A7_Idle[i] = -1;
                idleAreasA7Occupied[i] = false;
            }
            
            int childrenIdleP = areaSeven.transform.GetChild(3).transform.childCount;
            for(int i = 0; i < childrenIdleP; i++)
            {
                seatingAreasA7[i] = areaSeven.transform.GetChild(3).transform.GetChild(i).transform;
                AI_ID_A7_Seating[i] = -1;
                seatingAreasA7Occupied[i] = false;
            }

            int childrenInspectingP = areaSeven.transform.GetChild(4).transform.childCount;
            for(int i = 0; i < childrenInspectingP; i++)
            {
                inspectionAreasA7[i] = areaSeven.transform.GetChild(4).transform.GetChild(i).transform;
                AI_ID_A7_Inspection[i] = -1;
                inspectionAreasA7Occupied[i] = false;
            }
        }
         else
        {
            areaSeven.transform.position = DefaultPos;
        }
       
    }
    // done 
    public string FindNearestLocation(int id)
    {  

        string thePosition = "null";
        float[] distances = new float[7];
        int i = 0;
        int area = 0;
        float smallest  = 999.0f;

        distances[i] = Vector3.Distance(areaOne.transform.position, AI[id].transform.position);
        i++;
        distances[i] = Vector3.Distance(areaTwo.transform.position, AI[id].transform.position);
        i++;
        distances[i] = Vector3.Distance(areaThree.transform.position, AI[id].transform.position);
        i++;
        distances[i] = Vector3.Distance(areaFour.transform.position, AI[id].transform.position);
        i++;
        distances[i] = Vector3.Distance(areaFive.transform.position, AI[id].transform.position);
        i++;
        distances[i] = Vector3.Distance(areaSix.transform.position, AI[id].transform.position);
        i++;
        distances[i] = Vector3.Distance(areaSeven.transform.position, AI[id].transform.position);
    
        for(int j = 0; j < distances.Length; j++)
        {
            if(distances[j] < smallest)
            {
                distances[j] = smallest;
                area = j;
            }
        }

        if(area == 0)
        {
            thePosition = A1;
        }
        if(area == 1)
        {
            thePosition = A2;
        }
        if(area == 2)
        {
            thePosition = A3;
        }
        if(area == 3)
        {
            thePosition = A4;
        }
        if(area == 4)
        {
            thePosition = A5;
        }
        if(area == 5)
        {
            thePosition = A6;
        }
        if(area == 6)
        {
            thePosition = A7;
        }

        return thePosition;
    }

    // done
    public Vector3 FindNewDestination(int id, string area, int _lastJobIterator, string lastActivity)
    {
        Vector3 newDestination = Vector3.zero;
        string currentLocation  = area;
        int wayPointNowInUse = -1;
        int lastJobIterator = _lastJobIterator;
        
        if(AI[id].GetComponent<AIController2>().GetCurrentLocation() == "null")
        {
            currentLocation = FindNearestLocation(id);
        }
       

        if(currentLocation == A1)
        {
            int rand = Random.Range(0, walkingPointsA1Occupied.Length);
            for(int i = 0; ; i++)
            {
                if(!walkingPointsA1Occupied[rand])
                {
                    walkingPointsA1Occupied[rand] = true;
                    newDestination = walkingWaypointsA1[rand].position;
                    AI_ID_A1_WP[rand] = id;
                    wayPointNowInUse = rand;

                    if(lastJobIterator != -1)
                    {
                        if(lastActivity == "Walking")
                        {
                            AI_ID_A1_WP[lastJobIterator] = -1;
                            walkingPointsA1Occupied[lastJobIterator] = false;
                        }
                        if(lastActivity == "Sitting")
                        {
                            AI_ID_A1_Seating[lastJobIterator] = -1;
                            seatingAreasA1Occupied[lastJobIterator] = false;
                        }
                        if(lastActivity == "Idling")
                        {
                            AI_ID_A1_Idle[lastJobIterator] = -1;
                            idleAreasA1Occupied[lastJobIterator] = false;
                        }
                        if(lastActivity == "Inspecting")
                        {
                            AI_ID_A1_Inspection[lastJobIterator] = -1;
                            inspectionAreasA1Occupied[lastJobIterator] = false;
                        }
                        if(lastActivity == "Socialising")
                        {
                            AI_ID_A1_Social[lastJobIterator] = -1;
                            socialAreasA1Occupied[lastJobIterator] = false;
                        }
                    }
                }
            }
        }
        if(currentLocation == A2)
        {
            int rand = Random.Range(0, walkingPointsA2Occupied.Length);
            for(int i = 0; ; i++)
            {
                if(!walkingPointsA2Occupied[rand])
                {
                    wayPointNowInUse = rand;
                    walkingPointsA2Occupied[rand] = true;
                    newDestination = walkingWaypointsA2[rand].position;
                    AI_ID_A2_WP[rand] = id;
                    if(lastJobIterator != -1)
                    {
                        if(lastActivity == "Walking")
                        {
                            AI_ID_A2_WP[lastJobIterator] = -1;
                            walkingPointsA2Occupied[lastJobIterator] = false;
                        }
                        if(lastActivity == "Sitting")
                        {
                            AI_ID_A2_Seating[lastJobIterator] = -1;
                            seatingAreasA2Occupied[lastJobIterator] = false;
                        }
                        if(lastActivity == "Idling")
                        {
                            AI_ID_A2_Idle[lastJobIterator] = -1;
                            idleAreasA2Occupied[lastJobIterator] = false;
                        }
                        if(lastActivity == "Inspecting")
                        {
                            AI_ID_A2_Inspection[lastJobIterator] = -1;
                            inspectionAreasA2Occupied[lastJobIterator] = false;
                        }
                        if(lastActivity == "Socialising")
                        {
                            AI_ID_A2_Social[lastJobIterator] = -1;
                            socialAreasA2Occupied[lastJobIterator] = false;
                        }
                    }
                }
            }
        }
        if(currentLocation == A3)
        {
            int rand = Random.Range(0, walkingPointsA3Occupied.Length);
            for(int i = 0; ; i++)
            {
                if(!walkingPointsA3Occupied[rand])
                {
                    wayPointNowInUse = rand;
                    walkingPointsA3Occupied[rand] = true;
                    newDestination = walkingWaypointsA3[rand].position;
                    AI_ID_A3_WP[rand] = id;
                    if(lastJobIterator != -1)
                    {
                        if(lastActivity == "Walking")
                        {
                            AI_ID_A3_WP[lastJobIterator] = -1;
                            walkingPointsA3Occupied[lastJobIterator] = false;
                        }
                        if(lastActivity == "Sitting")
                        {
                            AI_ID_A3_Seating[lastJobIterator] = -1;
                            seatingAreasA3Occupied[lastJobIterator] = false;
                        }
                        if(lastActivity == "Idling")
                        {
                            AI_ID_A3_Idle[lastJobIterator] = -1;
                            idleAreasA3Occupied[lastJobIterator] = false;
                        }
                        if(lastActivity == "Inspecting")
                        {
                            AI_ID_A3_Inspection[lastJobIterator] = -1;
                            inspectionAreasA3Occupied[lastJobIterator] = false;
                        }
                        if(lastActivity == "Socialising")
                        {
                            AI_ID_A3_Social[lastJobIterator] = -1;
                            socialAreasA3Occupied[lastJobIterator] = false;
                        }
                    }
                }
            }
        }
        if(currentLocation == A4)
        {
            int rand = Random.Range(0, walkingPointsA4Occupied.Length);
            for(int i = 0; ; i++)
            {
                if(!walkingPointsA4Occupied[rand])
                {
                    wayPointNowInUse = rand;
                    walkingPointsA4Occupied[rand] = true;
                    newDestination = walkingWaypointsA4[rand].position;
                    AI_ID_A4_WP[rand] = id;
                    if(lastJobIterator != -1)
                    {
                        if(lastActivity == "Walking")
                        {
                            AI_ID_A4_WP[lastJobIterator] = -1;
                            walkingPointsA4Occupied[lastJobIterator] = false;
                        }
                        if(lastActivity == "Sitting")
                        {
                            AI_ID_A4_Seating[lastJobIterator] = -1;
                            seatingAreasA4Occupied[lastJobIterator] = false;
                        }
                        if(lastActivity == "Idling")
                        {
                            AI_ID_A4_Idle[lastJobIterator] = -1;
                            idleAreasA4Occupied[lastJobIterator] = false;
                        }
                        if(lastActivity == "Inspecting")
                        {
                            AI_ID_A4_Inspection[lastJobIterator] = -1;
                            inspectionAreasA4Occupied[lastJobIterator] = false;
                        }
                        if(lastActivity == "Socialising")
                        {
                            AI_ID_A4_Social[lastJobIterator] = -1;
                            socialAreasA4Occupied[lastJobIterator] = false;
                        }
                    }
                }
            }
        }
        if(currentLocation == A5)
        {
            int rand = Random.Range(0, walkingPointsA5Occupied.Length);
            for(int i = 0; ; i++)
            {
                if(!walkingPointsA5Occupied[rand])
                {
                    wayPointNowInUse = rand;
                    walkingPointsA5Occupied[rand] = true;
                    newDestination = walkingWaypointsA5[rand].position;
                    AI_ID_A5_WP[rand] = id;
                    if(lastJobIterator != -1)
                    {
                        if(lastActivity == "Walking")
                        {
                            AI_ID_A5_WP[lastJobIterator] = -1;
                            walkingPointsA5Occupied[lastJobIterator] = false;
                        }
                        if(lastActivity == "Sitting")
                        {
                            AI_ID_A5_Seating[lastJobIterator] = -1;
                            seatingAreasA5Occupied[lastJobIterator] = false;
                        }
                        if(lastActivity == "Idling")
                        {
                            AI_ID_A5_Idle[lastJobIterator] = -1;
                            idleAreasA5Occupied[lastJobIterator] = false;
                        }
                        if(lastActivity == "Inspecting")
                        {
                            AI_ID_A5_Inspection[lastJobIterator] = -1;
                            inspectionAreasA5Occupied[lastJobIterator] = false;
                        }
                        if(lastActivity == "Socialising")
                        {
                            AI_ID_A5_Social[lastJobIterator] = -1;
                            socialAreasA5Occupied[lastJobIterator] = false;
                        }
                    }
                }
            }
        }
        if(currentLocation == A6)
        {
            int rand = Random.Range(0, walkingPointsA6Occupied.Length);
            for(int i = 0; ; i++)
            {
                if(!walkingPointsA6Occupied[rand])
                {
                    wayPointNowInUse = rand;
                    walkingPointsA6Occupied[rand] = true;
                    newDestination = walkingWaypointsA6[rand].position;
                    AI_ID_A6_WP[rand] = id;
                    if(lastJobIterator != -1)
                    {
                        if(lastActivity == "Walking")
                        {
                            AI_ID_A6_WP[lastJobIterator] = -1;
                            walkingPointsA6Occupied[lastJobIterator] = false;
                        }
                        if(lastActivity == "Sitting")
                        {
                            AI_ID_A6_Seating[lastJobIterator] = -1;
                            seatingAreasA6Occupied[lastJobIterator] = false;
                        }
                        if(lastActivity == "Idling")
                        {
                            AI_ID_A6_Idle[lastJobIterator] = -1;
                            idleAreasA6Occupied[lastJobIterator] = false;
                        }
                        if(lastActivity == "Inspecting")
                        {
                            AI_ID_A6_Inspection[lastJobIterator] = -1;
                            inspectionAreasA6Occupied[lastJobIterator] = false;
                        }
                        if(lastActivity == "Socialising")
                        {
                            AI_ID_A6_Social[lastJobIterator] = -1;
                            socialAreasA6Occupied[lastJobIterator] = false;
                        }
                    }
                }
            }
        }
        if(currentLocation == A7)
        {
            int rand = Random.Range(0, walkingPointsA7Occupied.Length);
            for(int i = 0; ; i++)
            {
                if(!walkingPointsA7Occupied[rand])
                {
                    wayPointNowInUse = rand;
                    walkingPointsA7Occupied[rand] = true;
                    newDestination = walkingWaypointsA1[rand].position;
                    AI_ID_A7_WP[rand] = id;
                    if(lastJobIterator != -1)
                    {
                        if(lastActivity == "Walking")
                        {
                            AI_ID_A7_WP[lastJobIterator] = -1;
                            walkingPointsA7Occupied[lastJobIterator] = false;
                        }
                        if(lastActivity == "Sitting")
                        {
                            AI_ID_A7_Seating[lastJobIterator] = -1;
                            seatingAreasA7Occupied[lastJobIterator] = false;
                        }
                        if(lastActivity == "Idling")
                        {
                            AI_ID_A7_Idle[lastJobIterator] = -1;
                            idleAreasA7Occupied[lastJobIterator] = false;
                        }
                        if(lastActivity == "Inspecting")
                        {
                            AI_ID_A7_Inspection[lastJobIterator] = -1;
                            inspectionAreasA7Occupied[lastJobIterator] = false;
                        }
                        if(lastActivity == "Socialising")
                        {
                            AI_ID_A7_Social[lastJobIterator] = -1;
                            socialAreasA7Occupied[lastJobIterator] = false;
                        }
                    }
                }
            }
        }

        AI[id].GetComponent<AIController2>().SetCurrentWayPointPos(wayPointNowInUse);
        return newDestination;
    }

    // needs started.
    public Vector3 FindSocialSpot(int id, string area, int lastJobIterator, string currentActivity)
    {
        Vector3 newDestinationSocial = Vector3.zero;

        return newDestinationSocial;
    }

    //needs to be started.
    public void FindIdleAIInAreaToSocialise(int id, string area)
    {
        for(int i = 0; i < AI.Length; i++)
        {
            if(AI[i].GetComponent<AIController2>().GetCanDoSocialise())
            {

            }
        }
    }
    //needs to be started.
    public Vector3 FindSeat(int id, string area, int lastJobIterator, string currentActivity)
    {
        Vector3 newSeatDestination = Vector3.zero;
        return newSeatDestination;
    }
    //needs to be started.
    public Vector3 FindPointToInspect(int id, string area, int lastJobIterator, string currentActivity)
    {
        Vector3 newInspectDestination = Vector3.zero;
        return newInspectDestination;
    }
    //needs to be started.
    public Vector3 FindIdleSpot(int id, string area, int lastJobIterator, string currentActivity)
    {
        Vector3 newIdleSpot = Vector3.zero;
        return newIdleSpot;
    }
    //needs to be started.
    public bool CheckIfHasSocialPartner(int id, int currentPos)
    {
        bool hasPartner = false;
        return hasPartner;
        // check positions if odd check position below if even check above unless 0
    }
    void Update()
    {
        
    }
}
