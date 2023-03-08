using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NPCManager : MonoBehaviour
{
    [Header("Areas")]
    [SerializeField] private GameObject areaOne;
    private string A1 = "AreaOne";
    [SerializeField] private GameObject areaTwo;
    private string A2 = "AreaTwo";
    [SerializeField] private GameObject areaThree;
    private string A3 = "AreaThree";
    [SerializeField] private GameObject areaFour;
    private string A4 = "AreaFour";
    [SerializeField] private GameObject areaFive;
    private string A5 = "AreaFive";
    [SerializeField] private GameObject areaSix;
    private string A6 = "AreaSix";
    [SerializeField] private GameObject areaSeven;
    private string A7 = "AreaSeven";
    private int locationCount = 7;
    private int iterator = 0; 
    [Header("AI")]
    [SerializeField] private GameObject[] AI;
    
    private int[] evenNumbers = new int[20] {0,2,4,6,8,10,12,14,16,18,20,24,26,28,30,32,34,36,38,40};
    private int[] oddNumbers = new int[20] {1,3,5,7,9,11,13,15,17,19,21,23,25,27,29,31,33,35,37,39};
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
        // initalise Area One
        if(areaOne != null)
        {
            if(areaOne.transform.GetChild(0) != null)
            {
                int childrenWayP = areaOne.transform.GetChild(0).transform.childCount;
                for(int i = 0; i < childrenWayP; i++)
                {
                    walkingWaypointsA1[i] = areaOne.transform.GetChild(0).transform.GetChild(i).transform;
                    AI_ID_A1_WP[i] = -1;
                    walkingPointsA1Occupied[i] = false;
                }
                
            }
            if(areaOne.transform.GetChild(1) != null)
            {
                int childrenSocialP = areaOne.transform.GetChild(1).transform.childCount;
                for(int i = 0; i < childrenSocialP; i++)
                {
                    socialAreasA1[i] = areaOne.transform.GetChild(1).transform.GetChild(i).transform;
                    AI_ID_A1_Social[i] = -1;
                    socialAreasA1Occupied[i] = false;
                } 
            }
            if(areaOne.transform.GetChild(2) != null)
            {
                int childrenSeatingP = areaOne.transform.GetChild(2).transform.childCount;
                for(int i = 0; i < childrenSeatingP; i++)
                {
                    idleAreasA1[i] = areaOne.transform.GetChild(2).transform.GetChild(i).transform;
                    AI_ID_A1_Idle[i] = -1;
                    idleAreasA1Occupied[i] = false;
                } 
            }
            if(areaOne.transform.GetChild(3) != null)
            {
                int childrenIdleP = areaOne.transform.GetChild(3).transform.childCount;
                for(int i = 0; i < childrenIdleP; i++)
                {
                    seatingAreasA1[i] = areaOne.transform.GetChild(3).transform.GetChild(i).transform;
                    AI_ID_A1_Seating[i] = -1;
                    seatingAreasA1Occupied[i] = false;
                }
            }
            if(areaOne.transform.GetChild(4) != null)
            {
                int childrenInspectingP = areaOne.transform.GetChild(4).transform.childCount;
                for(int i = 0; i < childrenInspectingP; i++)
                {
                    inspectionAreasA1[i] = areaOne.transform.GetChild(4).transform.GetChild(i).transform;
                    AI_ID_A1_Inspection[i] = -1;
                    inspectionAreasA1Occupied[i] = false;
                }
            }
        }
      
        // initalise Area Two
        if(areaTwo != null)
        {
            if(areaTwo.transform.GetChild(0) != null)
            {
                int childrenWayP = areaTwo.transform.GetChild(0).transform.childCount;
                for(int i = 0; i < childrenWayP; i++)
                {
                    walkingWaypointsA2[i] = areaTwo.transform.GetChild(0).transform.GetChild(i).transform;
                    AI_ID_A2_WP[i] = -1;
                    walkingPointsA2Occupied[i] = false;
                }
            }
            if(areaTwo.transform.GetChild(1) != null)
            {
                int childrenSocialP = areaTwo.transform.GetChild(1).transform.childCount;
                for(int i = 0; i < childrenSocialP; i++)
                {
                    socialAreasA2[i] = areaTwo.transform.GetChild(1).transform.GetChild(i).transform;
                    AI_ID_A2_Social[i] = -1;
                    socialAreasA2Occupied[i] = false;
                }   
            }
            if(areaTwo.transform.GetChild(2) != null)
            {
                int childrenSeatingP = areaTwo.transform.GetChild(2).transform.childCount;
                for(int i = 0; i < childrenSeatingP; i++)
                {
                    idleAreasA2[i] = areaTwo.transform.GetChild(2).transform.GetChild(i).transform;
                    AI_ID_A2_Idle[i] = -1;
                    idleAreasA2Occupied[i] = false;
                }
            }
            if(areaTwo.transform.GetChild(3) != null)
            {
                int childrenIdleP = areaTwo.transform.GetChild(3).transform.childCount;
                for(int i = 0; i < childrenIdleP; i++)
                {
                    seatingAreasA2[i] = areaTwo.transform.GetChild(3).transform.GetChild(i).transform;
                    AI_ID_A2_Seating[i] = -1;
                    seatingAreasA2Occupied[i] = false;
                }
                
            }
            if(areaTwo.transform.GetChild(4) != null)
            {
                int childrenInspectingP = areaTwo.transform.GetChild(4).transform.childCount;
                for(int i = 0; i < childrenInspectingP; i++)
                {
                    inspectionAreasA2[i] = areaTwo.transform.GetChild(4).transform.GetChild(i).transform;
                    AI_ID_A2_Inspection[i] = -1;
                    inspectionAreasA2Occupied[i] = false;
                }
            }
        }
       
        // initalise Area Three
        if(areaThree != null)
        {
            if(areaThree.transform.GetChild(0) != null)
            {
                int childrenWayP = areaThree.transform.GetChild(0).transform.childCount;
                for(int i = 0; i < childrenWayP; i++)
                {
                    walkingWaypointsA3[i] = areaThree.transform.GetChild(0).transform.GetChild(i).transform;
                    AI_ID_A3_WP[i] = -1;
                    walkingPointsA3Occupied[i] = false;
                }   
            }
            if(areaThree.transform.GetChild(1) != null)
            {
                int childrenSocialP = areaThree.transform.GetChild(1).transform.childCount;
                for(int i = 0; i < childrenSocialP; i++)
                {
                    socialAreasA3[i] = areaThree.transform.GetChild(1).transform.GetChild(i).transform;
                    AI_ID_A3_Social[i] = -1;
                    socialAreasA3Occupied[i] = false;
                }
            }
            if(areaThree.transform.GetChild(2) != null)
            {
                int childrenSeatingP = areaThree.transform.GetChild(2).transform.childCount;
                for(int i = 0; i < childrenSeatingP; i++)
                {
                    idleAreasA3[i] = areaThree.transform.GetChild(2).transform.GetChild(i).transform;
                    AI_ID_A3_Idle[i] = -1;
                    idleAreasA3Occupied[i] = false;
                }
                
            }
            if(areaThree.transform.GetChild(3) != null)
            {
                int childrenIdleP = areaThree.transform.GetChild(3).transform.childCount;
                for(int i = 0; i < childrenIdleP; i++)
                {
                    seatingAreasA3[i] = areaThree.transform.GetChild(3).transform.GetChild(i).transform;
                    AI_ID_A3_Seating[i] = -1;
                    seatingAreasA3Occupied[i] = false;
                }   
            }
            if(areaThree.transform.GetChild(4) != null)
            {
                int childrenInspectingP = areaThree.transform.GetChild(4).transform.childCount;
                for(int i = 0; i < childrenInspectingP; i++)
                {
                    inspectionAreasA3[i] = areaThree.transform.GetChild(4).transform.GetChild(i).transform;
                    AI_ID_A3_Inspection[i] = -1;
                    inspectionAreasA3Occupied[i] = false;
                }            
            }
        }
      
        // initalise Area Four
        if(areaFour != null)
        {
            if(areaFour.transform.GetChild(0) != null)
            {
                int childrenWayP = areaFour.transform.GetChild(0).transform.childCount;
                for(int i = 0; i < childrenWayP; i++)
                {
                    walkingWaypointsA4[i] = areaFour.transform.GetChild(0).transform.GetChild(i).transform;
                    AI_ID_A4_WP[i] = -1;
                    walkingPointsA4Occupied[i] = false;
                }
            }
            if(areaFour.transform.GetChild(1) != null)
            {
                int childrenSocialP = areaFour.transform.GetChild(1).transform.childCount;
                for(int i = 0; i < childrenSocialP; i++)
                {
                    socialAreasA4[i] = areaFour.transform.GetChild(1).transform.GetChild(i).transform;
                    AI_ID_A4_Social[i] = -1;
                    socialAreasA4Occupied[i] = false;
                }
            }
            if(areaFour.transform.GetChild(2) != null)
            {
                int childrenSeatingP = areaFour.transform.GetChild(2).transform.childCount;
                for(int i = 0; i < childrenSeatingP; i++)
                {
                    idleAreasA4[i] = areaFour.transform.GetChild(2).transform.GetChild(i).transform;
                    AI_ID_A4_Idle[i] = -1;
                    idleAreasA4Occupied[i] = false;
                }
            }
            if(areaFour.transform.GetChild(3) != null)
            {
                int childrenIdleP = areaFour.transform.GetChild(3).transform.childCount;
                for(int i = 0; i < childrenIdleP; i++)
                {
                    seatingAreasA4[i] = areaFour.transform.GetChild(3).transform.GetChild(i).transform;
                    AI_ID_A4_Seating[i] = -1;
                    seatingAreasA4Occupied[i] = false;
                }
            }
            if(areaFour.transform.GetChild(4) != null)
            {
                int childrenInspectingP = areaFour.transform.GetChild(4).transform.childCount;
                for(int i = 0; i < childrenInspectingP; i++)
                {
                    inspectionAreasA4[i] = areaFour.transform.GetChild(4).transform.GetChild(i).transform;
                    AI_ID_A4_Inspection[i] = -1;
                    inspectionAreasA4Occupied[i] = false;
                }
            }
        }
       
        // initialise Area Five
        if(areaFive != null)
        {
            if(areaFive.transform.GetChild(0) != null)
            {
                int childrenWayP = areaFive.transform.GetChild(0).transform.childCount;
                for(int i = 0; i < childrenWayP; i++)
                {
                    walkingWaypointsA5[i] = areaFive.transform.GetChild(0).transform.GetChild(i).transform;
                    AI_ID_A5_WP[i] = -1;
                    walkingPointsA5Occupied[i] = false;
                }
            }
            if(areaFive.transform.GetChild(1) != null)
            {
                int childrenSocialP = areaFive.transform.GetChild(1).transform.childCount;
                for(int i = 0; i < childrenSocialP; i++)
                {
                    socialAreasA5[i] = areaFive.transform.GetChild(1).transform.GetChild(i).transform;
                    AI_ID_A5_Social[i] = -1;
                    socialAreasA5Occupied[i] = false;
                }
            }
            if(areaFive.transform.GetChild(2) != null)
            {
                int childrenSeatingP = areaFive.transform.GetChild(2).transform.childCount;
                for(int i = 0; i < childrenSeatingP; i++)
                {
                    idleAreasA5[i] = areaFive.transform.GetChild(2).transform.GetChild(i).transform;
                    AI_ID_A5_Idle[i] = -1;
                    idleAreasA5Occupied[i] = false;
                }
            }
            if(areaFive.transform.GetChild(3) != null)
            {
                int childrenIdleP = areaFive.transform.GetChild(3).transform.childCount;
                for(int i = 0; i < childrenIdleP; i++)
                {
                    seatingAreasA5[i] = areaFive.transform.GetChild(3).transform.GetChild(i).transform;
                    AI_ID_A5_Seating[i] = -1;
                    seatingAreasA5Occupied[i] = false;
                }
            }
            if(areaFive.transform.GetChild(4) != null)
            {
                int childrenInspectingP = areaFive.transform.GetChild(4).transform.childCount;
                for(int i = 0; i < childrenInspectingP; i++)
                {
                    inspectionAreasA5[i] = areaFive.transform.GetChild(4).transform.GetChild(i).transform;
                    AI_ID_A5_Inspection[i] = -1;
                    inspectionAreasA5Occupied[i] = false;
                }
            }
        }
       
        // initalise Area Six
        if(areaSix != null)
        {
            if(areaSix.transform.GetChild(0) != null)
            {
                int childrenWayP = areaSix.transform.GetChild(0).transform.childCount;
                for(int i = 0; i < childrenWayP; i++)
                {
                    walkingWaypointsA6[i] = areaSix.transform.GetChild(0).transform.GetChild(i).transform;
                    AI_ID_A6_WP[i] = -1;
                    walkingPointsA6Occupied[i] = false;
                }
            }
            if(areaSix.transform.GetChild(1) != null)
            {
                int childrenSocialP = areaSix.transform.GetChild(1).transform.childCount;
                for(int i = 0; i < childrenSocialP; i++)
                {
                    socialAreasA6[i] = areaSix.transform.GetChild(1).transform.GetChild(i).transform;
                    AI_ID_A6_Social[i] = -1;
                    socialAreasA6Occupied[i] = false;
                }
            }
            if(areaSix.transform.GetChild(2) != null)
            {
                int childrenSeatingP = areaSix.transform.GetChild(2).transform.childCount;
                for(int i = 0; i < childrenSeatingP; i++)
                {
                    idleAreasA6[i] = areaSix.transform.GetChild(2).transform.GetChild(i).transform;
                    AI_ID_A6_Idle[i] = -1;
                    idleAreasA6Occupied[i] = false;
                }
            }
            if(areaSix.transform.GetChild(3) != null)
            {
                int childrenIdleP = areaSix.transform.GetChild(3).transform.childCount;
                for(int i = 0; i < childrenIdleP; i++)
                {
                    seatingAreasA6[i] = areaSix.transform.GetChild(3).transform.GetChild(i).transform;
                    AI_ID_A6_Seating[i] = -1;
                    seatingAreasA6Occupied[i] = false;
                }
            }
            if(areaSix.transform.GetChild(4) != null)
            {
                int childrenInspectingP = areaSix.transform.GetChild(4).transform.childCount;
                for(int i = 0; i < childrenInspectingP; i++)
                {
                    inspectionAreasA6[i] = areaSix.transform.GetChild(4).transform.GetChild(i).transform;
                    AI_ID_A6_Inspection[i] = -1;
                    inspectionAreasA6Occupied[i] = false;
                }
            }
        }
      
        // initalise Area Seven
        if(areaSeven != null)
        {
            if(areaSeven.transform.GetChild(0) != null)
            {
                int childrenWayP = areaSeven.transform.GetChild(0).transform.childCount;
                for(int i = 0; i < childrenWayP; i++)
                {
                    walkingWaypointsA7[i] = areaSeven.transform.GetChild(0).transform.GetChild(i).transform;
                    AI_ID_A7_WP[i] = -1;
                    walkingPointsA7Occupied[i] = false;
                }
            }
            if(areaSeven.transform.GetChild(1) != null)
            {
                int childrenSocialP = areaSeven.transform.GetChild(1).transform.childCount;
                for(int i = 0; i < childrenSocialP; i++)
                {
                    socialAreasA7[i] = areaSeven.transform.GetChild(1).transform.GetChild(i).transform;
                    AI_ID_A7_Social[i] = -1;
                    socialAreasA7Occupied[i] = false;
                }
            }
            if(areaSeven.transform.GetChild(2) != null)
            {
                int childrenSeatingP = areaSeven.transform.GetChild(2).transform.childCount;
                for(int i = 0; i < childrenSeatingP; i++)
                {
                    idleAreasA7[i] = areaSeven.transform.GetChild(2).transform.GetChild(i).transform;
                    AI_ID_A7_Idle[i] = -1;
                    idleAreasA7Occupied[i] = false;
                }
            }
            if(areaSeven.transform.GetChild(3) != null)
            {
                int childrenIdleP = areaSeven.transform.GetChild(3).transform.childCount;
                for(int i = 0; i < childrenIdleP; i++)
                {
                    seatingAreasA7[i] = areaSeven.transform.GetChild(3).transform.GetChild(i).transform;
                    AI_ID_A7_Seating[i] = -1;
                    seatingAreasA7Occupied[i] = false;
                }
            }
            if(areaSeven.transform.GetChild(4) != null)
            {
                int childrenInspectingP = areaSeven.transform.GetChild(4).transform.childCount;
                for(int i = 0; i < childrenInspectingP; i++)
                {
                    inspectionAreasA7[i] = areaSeven.transform.GetChild(4).transform.GetChild(i).transform;
                    AI_ID_A7_Inspection[i] = -1;
                    inspectionAreasA7Occupied[i] = false;
                }
            }
        }
       
    }
    // done 
    public string FindNearestLocation(int id)
    {  
       
        string thePosition = "null";
        float[] distances = new float[7]{1000.0f,1000.0f,1000.0f,1000.0f,1000.0f,1000.0f,1000.0f};
        int area = 0;
        float smallest  = 999.0f;

        if(areaOne != null)
        {
            distances[0] = Vector3.Distance(areaOne.transform.position, AI[id].transform.position);
        }
        if(areaTwo != null)
        {
            distances[1] = Vector3.Distance(areaTwo.transform.position, AI[id].transform.position);
        }
        if(areaThree != null)
        {
            distances[2] = Vector3.Distance(areaThree.transform.position, AI[id].transform.position);
        }
        if(areaFour != null)
        {
            distances[3] = Vector3.Distance(areaFour.transform.position, AI[id].transform.position);
        }
        if(areaFive != null)
        {
             distances[4] = Vector3.Distance(areaFive.transform.position, AI[id].transform.position);
        }
        if(areaSix != null)
        {
            distances[5] = Vector3.Distance(areaSix.transform.position, AI[id].transform.position);
        }
        if(areaSeven != null)
        {
            distances[6] = Vector3.Distance(areaSeven.transform.position, AI[id].transform.position);
        }
    
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
        iterator = 0;
        if(AI[id].GetComponent<AIController2>().GetCurrentLocation() == "null")
        {
            currentLocation = FindNearestLocation(id);
        }
       

        if(currentLocation == A1)
        {
            
            for(int i = 0; ; i++)
            {
                iterator++;
                int rand = Random.Range(0, walkingWaypointsA1.Length);
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
                            //Debug.Log("position: " + lastJobIterator + " is now available.");
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
                    break;
                }
                if(iterator >= 100)
                {
                    AI[id].GetComponent<AIController2>().SetFindNewJob(true);
                    break;
                }
            }
        }
        if(currentLocation == A2)
        {
            for(int i = 0; ; i++)
            {
                int rand = Random.Range(0, walkingPointsA2Occupied.Length);
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
                    break;
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
                    break;
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
                    break;
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
                    break;
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
                    break;
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
                    break;
                }
            }
        }

        AI[id].GetComponent<AIController2>().SetCurrentWayPointPos(wayPointNowInUse);
        return newDestination;
    }

    public Vector3 FindSocialSpot(int id, string area, int _lastJobIterator, string lastActivity)
    {
        Vector3 newDestination = Vector3.zero;
        string currentLocation  = area;
        int wayPointNowInUse = -1;
        int lastJobIterator = _lastJobIterator;
        iterator = 0;

        if(AI[id].GetComponent<AIController2>().GetCurrentLocation() == "null")
        {
            currentLocation = FindNearestLocation(id);
        }
       
        if(currentLocation == A1)
        {
            for(int i = 0; ; i++)
            {
                iterator++;
                int rand = Random.Range(0, socialAreasA1.Length);
                if(!socialAreasA1Occupied[rand])
                {
                    socialAreasA1Occupied[rand] = true;
                    newDestination = socialAreasA1[rand].position;
                    AI_ID_A1_Social[rand] = id;
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
                    break;
                }
                if(iterator >= 100)
                {
                    AI[id].GetComponent<AIController2>().SetFindNewJob(true);
                    break;
                }
            }
        }
        if(currentLocation == A2)
        {
            for(int i = 0; ; i++)
            {
                int rand = Random.Range(0, socialAreasA2Occupied.Length);
                if(!socialAreasA2Occupied[rand])
                {
                    wayPointNowInUse = rand;
                    socialAreasA2Occupied[rand] = true;
                    newDestination = socialAreasA2[rand].position;
                    AI_ID_A2_Social[rand] = id;
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
                    break;
                }
            }
        }
        if(currentLocation == A3)
        {
            int rand = Random.Range(0, socialAreasA3Occupied.Length);
            for(int i = 0; ; i++)
            {
                if(!socialAreasA3Occupied[rand])
                {
                    wayPointNowInUse = rand;
                    socialAreasA3Occupied[rand] = true;
                    newDestination = socialAreasA3[rand].position;
                    AI_ID_A3_Social[rand] = id;
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
                    break;
                }
            }
        }
        if(currentLocation == A4)
        {
            int rand = Random.Range(0, socialAreasA4Occupied.Length);
            for(int i = 0; ; i++)
            {
                if(!socialAreasA4Occupied[rand])
                {
                    wayPointNowInUse = rand;
                    socialAreasA4Occupied[rand] = true;
                    newDestination = socialAreasA4[rand].position;
                    AI_ID_A4_Social[rand] = id;
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
                    break;
                }
            }
        }
        if(currentLocation == A5)
        {
            int rand = Random.Range(0, socialAreasA5Occupied.Length);
            for(int i = 0; ; i++)
            {
                if(!socialAreasA5Occupied[rand])
                {
                    wayPointNowInUse = rand;
                    socialAreasA5Occupied[rand] = true;
                    newDestination = socialAreasA5[rand].position;
                    AI_ID_A5_Social[rand] = id;
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
                    break;
                }
            }
        }
        if(currentLocation == A6)
        {
            int rand = Random.Range(0, socialAreasA6Occupied.Length);
            for(int i = 0; ; i++)
            {
                if(!socialAreasA6Occupied[rand])
                {
                    wayPointNowInUse = rand;
                    socialAreasA6Occupied[rand] = true;
                    newDestination = socialAreasA6[rand].position;
                    AI_ID_A6_Social[rand] = id;
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
                    break;
                }
            }
        }
        if(currentLocation == A7)
        {
            int rand = Random.Range(0, socialAreasA7Occupied.Length);
            for(int i = 0; ; i++)
            {
                if(!socialAreasA7Occupied[rand])
                {
                    wayPointNowInUse = rand;
                    socialAreasA7Occupied[rand] = true;
                    newDestination = socialAreasA7[rand].position;
                    AI_ID_A7_Idle[rand] = id;
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
                    break;
                }
            }
        }

        AI[id].GetComponent<AIController2>().SetCurrentWayPointPos(wayPointNowInUse);
        return newDestination;
    }
    // quick fix for check fixes check if partner has arrived at social point
    public bool CheckIfPartnerHasArrivedAtSocialDestination(int id, string area, int currentPosID)
    {
        bool partnerArrived = false;
        int socialPosID = 0;
        int partnerId = 0;
        if(area == A1)
        {
            for(int i = 0; i < socialAreasA1.Length ;i++)
            {
                if(evenNumbers[i] == currentPosID)
                {
                    socialPosID = evenNumbers[i] + 1;
                    partnerId = AI_ID_A1_Social[socialPosID];

                    if(AI[partnerId].GetComponent<AIController2>().GetDistToDestination() <= 0.1f)
                    {
                        partnerArrived = true;
                        AI[partnerId].GetComponent<AIController2>().SetSocialAudio(true);
                    }
                    else
                    {
                        partnerArrived = false;
                    }
                    return partnerArrived;
                }
                if(oddNumbers[i] == currentPosID)
                {
                    socialPosID = oddNumbers[i] - 1;
                    partnerId = AI_ID_A1_Social[socialPosID];

                    if(AI[partnerId].GetComponent<AIController2>().GetDistToDestination() <= 0.1f)
                    {
                        partnerArrived = true;
                        AI[partnerId].GetComponent<AIController2>().SetSocialAudio(true);
                    }
                    else
                    {
                        partnerArrived = false;
                    }
                    return partnerArrived;
                }
            }
        }
        if(area == A2)
        {
            for(int i = 0; i < socialAreasA2.Length ;i++)
            {
                if(evenNumbers[i] == currentPosID)
                {
                    socialPosID = evenNumbers[i] + 1;
                    partnerId = AI_ID_A2_Social[socialPosID];
                    if(AI[partnerId].transform.position.x == socialAreasA2[socialPosID].position.x && AI[partnerId].transform.position.z == socialAreasA2[socialPosID].position.z)
                    {
                        partnerArrived = true;
                    }
                    return partnerArrived;
                }
                if(oddNumbers[i] == currentPosID)
                {
                    socialPosID = oddNumbers[i] - 1;
                    partnerId = AI_ID_A2_Social[socialPosID];
                    if(AI[partnerId].transform.position.x == socialAreasA2[socialPosID].position.x && AI[partnerId].transform.position.z == socialAreasA2[socialPosID].position.z)
                    {
                        partnerArrived = true;
                    }
                    else
                    {
                    }
                    return partnerArrived;
                }
            }
        }
        if(area == A3)
        {
            for(int i = 0; i < socialAreasA3.Length ;i++)
            {
                if(evenNumbers[i] == currentPosID)
                {
                    socialPosID = evenNumbers[i] + 1;
                    partnerId = AI_ID_A3_Social[socialPosID];
                    if(AI[partnerId].transform.position.x == socialAreasA3[socialPosID].position.x && AI[partnerId].transform.position.z == socialAreasA3[socialPosID].position.z)
                    {
                        partnerArrived = true;
                    }
                    return partnerArrived;
                }
                if(oddNumbers[i] == currentPosID)
                {
                    socialPosID = oddNumbers[i] - 1;
                    partnerId = AI_ID_A3_Social[socialPosID];
                    if(AI[partnerId].transform.position.x == socialAreasA3[socialPosID].position.x && AI[partnerId].transform.position.z == socialAreasA3[socialPosID].position.z)
                    {
                        partnerArrived = true;
                    }
                    else
                    {
                    }
                    return partnerArrived;
                }
            }
        }
        if(area == A4)
        {
            for(int i = 0; i < socialAreasA4.Length ;i++)
            {
                if(evenNumbers[i] == currentPosID)
                {
                    socialPosID = evenNumbers[i] + 1;
                    partnerId = AI_ID_A4_Social[socialPosID];
                    if(AI[partnerId].transform.position.x == socialAreasA4[socialPosID].position.x && AI[partnerId].transform.position.z == socialAreasA4[socialPosID].position.z)
                    {
                        partnerArrived = true;
                    }
                    return partnerArrived;
                }
                if(oddNumbers[i] == currentPosID)
                {
                    socialPosID = oddNumbers[i] - 1;
                    partnerId = AI_ID_A4_Social[socialPosID];
                    if(AI[partnerId].transform.position.x == socialAreasA4[socialPosID].position.x && AI[partnerId].transform.position.z == socialAreasA4[socialPosID].position.z)
                    {
                        partnerArrived = true;
                    }
                    else
                    {
                        return partnerArrived;
                    }
                }
            }
        }
        if(area == A5)
        {
            for(int i = 0; i < socialAreasA5.Length ;i++)
            {
                if(evenNumbers[i] == currentPosID)
                {
                    socialPosID = evenNumbers[i] + 1;
                    partnerId = AI_ID_A5_Social[socialPosID];
                    if(AI[partnerId].transform.position.x == socialAreasA5[socialPosID].position.x && AI[partnerId].transform.position.z == socialAreasA5[socialPosID].position.z)
                    {
                        partnerArrived = true;
                    }
                    return partnerArrived;
                }
                if(oddNumbers[i] == currentPosID)
                {
                    socialPosID = oddNumbers[i] - 1;
                    partnerId = AI_ID_A5_Social[socialPosID];
                    if(AI[partnerId].transform.position.x == socialAreasA5[socialPosID].position.x && AI[partnerId].transform.position.z == socialAreasA5[socialPosID].position.z)
                    {
                        partnerArrived = true;
                    }
                    else
                    {
                    }
                    return partnerArrived;
                }
            }
        }
        if(area == A6)
        {
            for(int i = 0; i < socialAreasA6.Length ;i++)
            {
                if(evenNumbers[i] == currentPosID)
                {
                    socialPosID = evenNumbers[i] + 1;
                    partnerId = AI_ID_A6_Social[socialPosID];
                    if(AI[partnerId].transform.position.x == socialAreasA6[socialPosID].position.x && AI[partnerId].transform.position.z == socialAreasA6[socialPosID].position.z)
                    {
                        partnerArrived = true;
                    }
                    return partnerArrived;
                }
                if(oddNumbers[i] == currentPosID)
                {
                    socialPosID = oddNumbers[i] - 1;
                    partnerId = AI_ID_A6_Social[socialPosID];
                    if(AI[partnerId].transform.position.x == socialAreasA6[socialPosID].position.x && AI[partnerId].transform.position.z == socialAreasA6[socialPosID].position.z)
                    {
                        partnerArrived = true;
                    }
                    else
                    {
                    }
                    return partnerArrived;
                }
            }
        }
        if(area == A7)
        {
            for(int i = 0; i < socialAreasA7.Length ;i++)
            {
                if(evenNumbers[i] == currentPosID)
                {
                    socialPosID = evenNumbers[i] + 1;
                    partnerId = AI_ID_A7_Social[socialPosID];
                    if(AI[partnerId].transform.position.x == socialAreasA7[socialPosID].position.x && AI[partnerId].transform.position.z == socialAreasA7[socialPosID].position.z)
                    {
                        partnerArrived = true;
                    }
                    return partnerArrived;
                }
                if(oddNumbers[i] == currentPosID)
                {
                    socialPosID = oddNumbers[i] - 1;
                    partnerId = AI_ID_A7_Social[socialPosID];
                    if(AI[partnerId].transform.position.x == socialAreasA7[socialPosID].position.x && AI[partnerId].transform.position.z == socialAreasA7[socialPosID].position.z)
                    {
                        partnerArrived = true;
                    }
                    else
                    {
                    }
                    return partnerArrived;
                }
            }
        }
       
        return partnerArrived;
    }
    // quick fix here for AI
    public bool FindIdleAIInAreaToSocialise(int id, string area, int currentPosID)
    {
        Vector3 socialDestination = Vector3.zero;
        string currentJob = "null";
        int socialPosID = 0;
        int AILastPos = 0;
        if(area == A1)
        {
            for(int i = 0; i < AI.Length; i++)
            {
                int rand = Random.Range(0, AI.Length);
                if(AI[rand].GetComponent<AIController2>().GetCanDoSocialise() && !AI[rand].GetComponent<AIController2>().GetIsSocialising() && AI[rand].GetComponent<AIController2>().GetId() != id)
                {
                    
                    for(int j = 0; j < socialAreasA1.Length ;j++)
                    {
                        if(evenNumbers[j] == currentPosID)
                        {
                            socialPosID = evenNumbers[j] + 1;
                            socialDestination = socialAreasA1[socialPosID].position;
                            socialAreasA1Occupied[socialPosID] = true;
                            AI_ID_A1_Social[socialPosID] = rand;
                            AILastPos = AI[rand].GetComponent<AIController2>().GetCurrentWayPointPos();
                            currentJob = AI[rand].GetComponent<AIController2>().GetCurrentJob();

                            if(AILastPos != -1)
                            {
                                if(currentJob == "Walking")
                                {
                                    walkingPointsA1Occupied[AILastPos] = false;
                                    AI_ID_A1_WP[AILastPos] = -1;
                                }
                                if(currentJob == "Inspecting")
                                {
                                    inspectionAreasA1Occupied[AILastPos] = false;
                                    AI_ID_A1_Inspection[AILastPos] = -1;
                                }
                                if(currentJob == "Idling")
                                {
                                    idleAreasA1Occupied[AILastPos] = false;
                                    AI_ID_A1_Idle[AILastPos] = -1;
                                }
                                if(currentJob == "Sitting")
                                {
                                    seatingAreasA1Occupied[AILastPos] = false;
                                    AI_ID_A1_Seating[AILastPos] = -1;
                                }
                            }

                            AI[rand].GetComponent<AIController2>().SetNewJob("Socialising");
                            AI[rand].GetComponent<AIController2>().SetCurrentWayPointPos(socialPosID);
                            AI[rand].GetComponent<AIController2>().SetSocialDestination(socialDestination);

                            return true;
                        }

                        if(oddNumbers[j] == currentPosID)
                        {
                            socialPosID = oddNumbers[j] - 1;
                            socialDestination = socialAreasA1[socialPosID].position;
                            socialAreasA1Occupied[socialPosID] = true;
                            AI_ID_A1_Social[socialPosID] = rand;
                            AILastPos = AI[rand].GetComponent<AIController2>().GetCurrentWayPointPos();
                            currentJob = AI[rand].GetComponent<AIController2>().GetCurrentJob();

                            if(AILastPos != -1)
                            {
                                if(currentJob == "Walking")
                                {
                                    walkingPointsA1Occupied[AILastPos] = false;
                                    AI_ID_A1_WP[AILastPos] = -1;
                                }
                                if(currentJob == "Inspecting")
                                {
                                    inspectionAreasA1Occupied[AILastPos] = false;
                                    AI_ID_A1_Inspection[AILastPos] = -1;
                                }
                                if(currentJob == "Idling")
                                {
                                    idleAreasA1Occupied[AILastPos] = false;
                                    AI_ID_A1_Idle[AILastPos] = -1;
                                }
                                if(currentJob == "Sitting")
                                {
                                    seatingAreasA1Occupied[AILastPos] = false;
                                    AI_ID_A1_Seating[AILastPos] = -1;
                                }
                            }

                            AI[rand].GetComponent<AIController2>().SetNewJob("Socialising");
                            AI[rand].GetComponent<AIController2>().SetCurrentWayPointPos(socialPosID);
                            AI[rand].GetComponent<AIController2>().SetSocialDestination(socialDestination);

                            return true;
                        }
                    }
                }
            }  
        }
        if(area == A2)
        {
            for(int i = 0; i < AI.Length; i++)
            {
                int rand = Random.Range(0, AI.Length);
                if(AI[rand].GetComponent<AIController2>().GetCanDoSocialise() && !AI[rand].GetComponent<AIController2>().GetIsSocialising() && AI[rand].GetComponent<AIController2>().GetId() != id)
                {
                    
                    for(int j = 0; j < socialAreasA2.Length ;j++)
                    {
                        if(evenNumbers[j] == currentPosID)
                        {
                            socialPosID = evenNumbers[j] + 1;
                            socialDestination = socialAreasA2[socialPosID].position;
                            socialAreasA2Occupied[socialPosID] = true;
                            AI_ID_A2_Social[socialPosID] = rand;
                            break;
                        }
                        if(oddNumbers[j] == currentPosID)
                        {
                            socialPosID = oddNumbers[j] - 1;
                            socialDestination = socialAreasA2[socialPosID].position;
                            socialAreasA2Occupied[socialPosID] = true;
                            AI_ID_A2_Social[socialPosID] = rand;
                            break;
                        }
                    }
                    
                    currentJob = AI[rand].GetComponent<AIController2>().GetCurrentJob();
                    if(currentJob == "Walking")
                    {
                        walkingPointsA2Occupied[socialPosID] = false;
                        AI_ID_A2_WP[socialPosID] = -1;
                    }
                    if(currentJob == "Inspecting")
                    {
                        inspectionAreasA2Occupied[socialPosID] = false;
                        AI_ID_A2_Inspection[socialPosID] = -1;
                    }
                    if(currentJob == "Idling")
                    {
                        idleAreasA2Occupied[socialPosID] = false;
                        AI_ID_A2_Idle[socialPosID] = -1;
                    }
                    if(currentJob == "Sitting")
                    {
                        seatingAreasA1Occupied[socialPosID] = false;
                        AI_ID_A2_Seating[socialPosID] = -1;
                    }

                    AI[rand].GetComponent<AIController2>().SetNewJob("Socialising");
                    AI[rand].GetComponent<AIController2>().SetCurrentWayPointPos(socialPosID);
                    AI[rand].GetComponent<AIController2>().SetSocialDestination(socialDestination);
                    return true;
                }
            }
        }
        if(area == A3)
        {
            for(int i = 0; i < AI.Length; i++)
            {
                int rand = Random.Range(0, AI.Length);
                if(AI[rand].GetComponent<AIController2>().GetCanDoSocialise() && !AI[rand].GetComponent<AIController2>().GetIsSocialising() && AI[rand].GetComponent<AIController2>().GetId() != id)
                {
                    
                    for(int j = 0; j < socialAreasA3.Length ;j++)
                    {
                        if(evenNumbers[j] == currentPosID)
                        {
                            socialPosID = evenNumbers[j] + 1;
                            socialDestination = socialAreasA3[socialPosID].position;
                            socialAreasA3Occupied[socialPosID] = true;
                            AI_ID_A3_Social[socialPosID] = rand;
                            break;
                        }
                        if(oddNumbers[j] == currentPosID)
                        {
                            socialPosID = oddNumbers[j] - 1;
                            socialDestination = socialAreasA3[socialPosID].position;
                            socialAreasA3Occupied[socialPosID] = true;
                            AI_ID_A3_Social[socialPosID] = rand;
                            break;
                        }
                    }
                    
                    currentJob = AI[rand].GetComponent<AIController2>().GetCurrentJob();
                    if(currentJob == "Walking")
                    {
                        walkingPointsA3Occupied[socialPosID] = false;
                        AI_ID_A3_WP[socialPosID] = -1;
                    }
                    if(currentJob == "Inspecting")
                    {
                        inspectionAreasA1Occupied[socialPosID] = false;
                        AI_ID_A3_Inspection[socialPosID] = -1;
                    }
                    if(currentJob == "Idling")
                    {
                        idleAreasA1Occupied[socialPosID] = false;
                        AI_ID_A3_Idle[socialPosID] = -1;
                    }
                    if(currentJob == "Sitting")
                    {
                        seatingAreasA1Occupied[socialPosID] = false;
                        AI_ID_A3_Seating[socialPosID] = -1;
                    }

                    AI[rand].GetComponent<AIController2>().SetNewJob("Socialising");
                    AI[rand].GetComponent<AIController2>().SetCurrentWayPointPos(socialPosID);
                    AI[rand].GetComponent<AIController2>().SetSocialDestination(socialDestination);
                    return true;
                }
            }
        }
        if(area == A4)
        {
            for(int i = 0; i < AI.Length; i++)
            {
                int rand = Random.Range(0, AI.Length);
                if(AI[rand].GetComponent<AIController2>().GetCanDoSocialise() && !AI[rand].GetComponent<AIController2>().GetIsSocialising() && AI[rand].GetComponent<AIController2>().GetId() != id)
                {
                    
                    for(int j = 0; j < socialAreasA4.Length ;j++)
                    {
                        if(evenNumbers[j] == currentPosID)
                        {
                            socialPosID = evenNumbers[j] + 1;
                            socialDestination = socialAreasA4[socialPosID].position;
                            socialAreasA4Occupied[socialPosID] = true;
                            AI_ID_A4_Social[socialPosID] = rand;
                            break;
                        }
                        if(oddNumbers[j] == currentPosID)
                        {
                            socialPosID = oddNumbers[j] - 1;
                            socialDestination = socialAreasA4[socialPosID].position;
                            socialAreasA4Occupied[socialPosID] = true;
                            AI_ID_A4_Social[socialPosID] = rand;
                            break;
                        }
                    }
                    
                    currentJob = AI[rand].GetComponent<AIController2>().GetCurrentJob();
                    if(currentJob == "Walking")
                    {
                        walkingPointsA4Occupied[socialPosID] = false;
                        AI_ID_A4_WP[socialPosID] = -1;
                    }
                    if(currentJob == "Inspecting")
                    {
                        inspectionAreasA4Occupied[socialPosID] = false;
                        AI_ID_A4_Inspection[socialPosID] = -1;
                    }
                    if(currentJob == "Idling")
                    {
                        idleAreasA4Occupied[socialPosID] = false;
                        AI_ID_A4_Idle[socialPosID] = -1;
                    }
                    if(currentJob == "Sitting")
                    {
                        seatingAreasA4Occupied[socialPosID] = false;
                        AI_ID_A4_Seating[socialPosID] = -1;
                    }

                    AI[rand].GetComponent<AIController2>().SetNewJob("Socialising");
                    AI[rand].GetComponent<AIController2>().SetCurrentWayPointPos(socialPosID);
                    AI[rand].GetComponent<AIController2>().SetSocialDestination(socialDestination);
                    return true;
                }
            }
        }
        if(area == A5)
        {
            for(int i = 0; i < AI.Length; i++)
            {
                int rand = Random.Range(0, AI.Length);
                if(AI[rand].GetComponent<AIController2>().GetCanDoSocialise() && !AI[rand].GetComponent<AIController2>().GetIsSocialising() && AI[rand].GetComponent<AIController2>().GetId() != id)
                {
                    
                    for(int j = 0; j < socialAreasA5.Length ;j++)
                    {
                        if(evenNumbers[j] == currentPosID)
                        {
                            socialPosID = evenNumbers[j] + 1;
                            socialDestination = socialAreasA5[socialPosID].position;
                            socialAreasA5Occupied[socialPosID] = true;
                            AI_ID_A5_Social[socialPosID] = rand;
                            break;
                        }
                        if(oddNumbers[j] == currentPosID)
                        {
                            socialPosID = oddNumbers[j] - 1;
                            socialDestination = socialAreasA5[socialPosID].position;
                            socialAreasA5Occupied[socialPosID] = true;
                            AI_ID_A5_Social[socialPosID] = rand;
                            break;
                        }
                    }
                    
                    currentJob = AI[rand].GetComponent<AIController2>().GetCurrentJob();
                    if(currentJob == "Walking")
                    {
                        walkingPointsA5Occupied[socialPosID] = false;
                        AI_ID_A5_WP[socialPosID] = -1;
                    }
                    if(currentJob == "Inspecting")
                    {
                        inspectionAreasA5Occupied[socialPosID] = false;
                        AI_ID_A5_Inspection[socialPosID] = -1;
                    }
                    if(currentJob == "Idling")
                    {
                        idleAreasA5Occupied[socialPosID] = false;
                        AI_ID_A5_Idle[socialPosID] = -1;
                    }
                    if(currentJob == "Sitting")
                    {
                        seatingAreasA5Occupied[socialPosID] = false;
                        AI_ID_A5_Seating[socialPosID] = -1;
                    }

                    AI[rand].GetComponent<AIController2>().SetNewJob("Socialising");
                    AI[rand].GetComponent<AIController2>().SetCurrentWayPointPos(socialPosID);
                    AI[rand].GetComponent<AIController2>().SetSocialDestination(socialDestination);
                    return true;
                }
            }
        }
        if(area == A6)
        {
            for(int i = 0; i < AI.Length; i++)
            {
                int rand = Random.Range(0, AI.Length);
                if(AI[rand].GetComponent<AIController2>().GetCanDoSocialise() && !AI[rand].GetComponent<AIController2>().GetIsSocialising() && AI[rand].GetComponent<AIController2>().GetId() != id)
                {
                    
                    for(int j = 0; j < socialAreasA6.Length ;j++)
                    {
                        if(evenNumbers[j] == currentPosID)
                        {
                            socialPosID = evenNumbers[j] + 1;
                            socialDestination = socialAreasA6[socialPosID].position;
                            socialAreasA6Occupied[socialPosID] = true;
                            AI_ID_A6_Social[socialPosID] = rand;
                            break;
                        }
                        if(oddNumbers[j] == currentPosID)
                        {
                            socialPosID = oddNumbers[j] - 1;
                            socialDestination = socialAreasA6[socialPosID].position;
                            socialAreasA6Occupied[socialPosID] = true;
                            AI_ID_A6_Social[socialPosID] = rand;
                            break;
                        }
                    }
                    
                    currentJob = AI[rand].GetComponent<AIController2>().GetCurrentJob();
                    if(currentJob == "Walking")
                    {
                        walkingPointsA6Occupied[socialPosID] = false;
                        AI_ID_A6_WP[socialPosID] = -1;
                    }
                    if(currentJob == "Inspecting")
                    {
                        inspectionAreasA6Occupied[socialPosID] = false;
                        AI_ID_A6_Inspection[socialPosID] = -1;
                    }
                    if(currentJob == "Idling")
                    {
                        idleAreasA6Occupied[socialPosID] = false;
                        AI_ID_A6_Idle[socialPosID] = -1;
                    }
                    if(currentJob == "Sitting")
                    {
                        seatingAreasA6Occupied[socialPosID] = false;
                        AI_ID_A6_Seating[socialPosID] = -1;
                    }

                    AI[rand].GetComponent<AIController2>().SetNewJob("Socialising");
                    AI[rand].GetComponent<AIController2>().SetCurrentWayPointPos(socialPosID);
                    AI[rand].GetComponent<AIController2>().SetSocialDestination(socialDestination);
                    return true;
                }
            }
        }
        if(area == A7)
        {
            for(int i = 0; i < AI.Length; i++)
            {
                int rand = Random.Range(0, AI.Length);
                if(AI[rand].GetComponent<AIController2>().GetCanDoSocialise() && !AI[rand].GetComponent<AIController2>().GetIsSocialising() && AI[rand].GetComponent<AIController2>().GetId() != id)
                {
                    
                    for(int j = 0; j < socialAreasA7.Length ;j++)
                    {
                        if(evenNumbers[j] == currentPosID)
                        {
                            socialPosID = evenNumbers[j] + 1;
                            socialDestination = socialAreasA7[socialPosID].position;
                            socialAreasA7Occupied[socialPosID] = true;
                            AI_ID_A7_Social[socialPosID] = rand;
                            break;
                        }
                        if(oddNumbers[j] == currentPosID)
                        {
                            socialPosID = oddNumbers[j] - 1;
                            socialDestination = socialAreasA7[socialPosID].position;
                            socialAreasA7Occupied[socialPosID] = true;
                            AI_ID_A7_Social[socialPosID] = rand;
                            break;
                        }
                    }
                    
                    currentJob = AI[rand].GetComponent<AIController2>().GetCurrentJob();
                    if(currentJob == "Walking")
                    {
                        walkingPointsA7Occupied[socialPosID] = false;
                        AI_ID_A7_WP[socialPosID] = -1;
                    }
                    if(currentJob == "Inspecting")
                    {
                        inspectionAreasA7Occupied[socialPosID] = false;
                        AI_ID_A7_Inspection[socialPosID] = -1;
                    }
                    if(currentJob == "Idling")
                    {
                        idleAreasA7Occupied[socialPosID] = false;
                        AI_ID_A7_Idle[socialPosID] = -1;
                    }
                    if(currentJob == "Sitting")
                    {
                        seatingAreasA7Occupied[socialPosID] = false;
                        AI_ID_A7_Seating[socialPosID] = -1;
                    }

                    AI[rand].GetComponent<AIController2>().SetNewJob("Socialising");
                    AI[rand].GetComponent<AIController2>().SetCurrentWayPointPos(socialPosID);
                    AI[rand].GetComponent<AIController2>().SetSocialDestination(socialDestination);
                    return true;
                }
            }
        }
        
        return false;
    }
    public Vector3 FindSeat(int id, string area, int _lastJobIterator, string lastActivity)
    {
        Vector3 newDestination = Vector3.zero;
        string currentLocation  = area;
        int wayPointNowInUse = -1;
        int lastJobIterator = _lastJobIterator;
        iterator = 0;

        if(AI[id].GetComponent<AIController2>().GetCurrentLocation() == "null")
        {
            currentLocation = FindNearestLocation(id);
        }
       
        if(currentLocation == A1)
        {
            for(int i = 0; ; i++)
            {
                iterator++;
                int rand = Random.Range(0, seatingAreasA1.Length);
                if(!seatingAreasA1Occupied[rand])
                {
                    seatingAreasA1Occupied[rand] = true;
                    newDestination = seatingAreasA1[rand].position;
                    AI_ID_A1_Seating[rand] = id;
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
                    break;
                }
                if(iterator >=  100)
                {
                    AI[id].GetComponent<AIController2>().SetFindNewJob(true);
                    break;
                }
            }
        }
        if(currentLocation == A2)
        {
            for(int i = 0; ; i++)
            {
                int rand = Random.Range(0, seatingAreasA2Occupied.Length);
                if(!seatingAreasA2Occupied[rand])
                {
                    wayPointNowInUse = rand;
                    seatingAreasA2Occupied[rand] = true;
                    newDestination = seatingAreasA2[rand].position;
                    AI_ID_A2_Seating[rand] = id;
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
                    break;
                }
            }
        }
        if(currentLocation == A3)
        {
            int rand = Random.Range(0, seatingAreasA3Occupied.Length);
            for(int i = 0; ; i++)
            {
                if(!seatingAreasA3Occupied[rand])
                {
                    wayPointNowInUse = rand;
                    seatingAreasA3Occupied[rand] = true;
                    newDestination = seatingAreasA3[rand].position;
                    AI_ID_A3_Seating[rand] = id;
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
                    break;
                }
            }
        }
        if(currentLocation == A4)
        {
            int rand = Random.Range(0, seatingAreasA4Occupied.Length);
            for(int i = 0; ; i++)
            {
                if(!seatingAreasA4Occupied[rand])
                {
                    wayPointNowInUse = rand;
                    seatingAreasA4Occupied[rand] = true;
                    newDestination = seatingAreasA4[rand].position;
                    AI_ID_A4_Seating[rand] = id;
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
                    break;
                }
            }
        }
        if(currentLocation == A5)
        {
            int rand = Random.Range(0, seatingAreasA5Occupied.Length);
            for(int i = 0; ; i++)
            {
                if(!seatingAreasA5Occupied[rand])
                {
                    wayPointNowInUse = rand;
                    seatingAreasA5Occupied[rand] = true;
                    newDestination = seatingAreasA5[rand].position;
                    AI_ID_A5_Seating[rand] = id;
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
                    break;
                }
            }
        }
        if(currentLocation == A6)
        {
            int rand = Random.Range(0, seatingAreasA6Occupied.Length);
            for(int i = 0; ; i++)
            {
                if(!seatingAreasA6Occupied[rand])
                {
                    wayPointNowInUse = rand;
                    seatingAreasA6Occupied[rand] = true;
                    newDestination = seatingAreasA6[rand].position;
                    AI_ID_A6_Seating[rand] = id;
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
                    break;
                }
            }
        }
        if(currentLocation == A7)
        {
            int rand = Random.Range(0, seatingAreasA7Occupied.Length);
            for(int i = 0; ; i++)
            {
                if(!seatingAreasA7Occupied[rand])
                {
                    wayPointNowInUse = rand;
                    seatingAreasA7Occupied[rand] = true;
                    newDestination = seatingAreasA7[rand].position;
                    AI_ID_A7_Seating[rand] = id;
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
                    break;
                }
            }
        }
        
        AI[id].GetComponent<AIController2>().SetCurrentWayPointPos(wayPointNowInUse);
        return newDestination;
    }
    public Vector3 FindPointToInspect(int id, string area, int _lastJobIterator, string lastActivity)
    {
        Vector3 newDestination = Vector3.zero;
        string currentLocation  = area;
        int wayPointNowInUse = -1;
        int lastJobIterator = _lastJobIterator;
        iterator = 0;
        if(AI[id].GetComponent<AIController2>().GetCurrentLocation() == "null")
        {
            currentLocation = FindNearestLocation(id);
        }
       
        if(currentLocation == A1)
        {
            for(int i = 0; ; i++)
            {
                iterator++;
                int rand = Random.Range(0, inspectionAreasA1.Length);
                if(!inspectionAreasA1Occupied[rand])
                {
                    inspectionAreasA1Occupied[rand] = true;
                    newDestination = inspectionAreasA1[rand].position;
                    AI_ID_A1_Inspection[rand] = id;
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
                    break;
                }
                if(iterator >= 100)
                {
                    AI[id].GetComponent<AIController2>().SetFindNewJob(true);
                    break;
                }
            }
        }
        if(currentLocation == A2)
        {
            for(int i = 0; ; i++)
            {
                int rand = Random.Range(0, inspectionAreasA2Occupied.Length);
                if(!inspectionAreasA2Occupied[rand])
                {
                    wayPointNowInUse = rand;
                    inspectionAreasA2Occupied[rand] = true;
                    newDestination = inspectionAreasA2[rand].position;
                    AI_ID_A2_Inspection[rand] = id;
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
                    break;
                }
            }
        }
        if(currentLocation == A3)
        {
            int rand = Random.Range(0, inspectionAreasA3Occupied.Length);
            for(int i = 0; ; i++)
            {
                if(!inspectionAreasA3Occupied[rand])
                {
                    wayPointNowInUse = rand;
                    inspectionAreasA3Occupied[rand] = true;
                    newDestination = inspectionAreasA3[rand].position;
                    AI_ID_A3_Inspection[rand] = id;
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
                    break;
                }
            }
        }
        if(currentLocation == A4)
        {
            int rand = Random.Range(0, inspectionAreasA4Occupied.Length);
            for(int i = 0; ; i++)
            {
                if(!inspectionAreasA4Occupied[rand])
                {
                    wayPointNowInUse = rand;
                    inspectionAreasA4Occupied[rand] = true;
                    newDestination = inspectionAreasA4[rand].position;
                    AI_ID_A4_Inspection[rand] = id;
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
                    break;
                }
            }
        }
        if(currentLocation == A5)
        {
            int rand = Random.Range(0, inspectionAreasA5Occupied.Length);
            for(int i = 0; ; i++)
            {
                if(!inspectionAreasA5Occupied[rand])
                {
                    wayPointNowInUse = rand;
                    inspectionAreasA5Occupied[rand] = true;
                    newDestination = inspectionAreasA5[rand].position;
                    AI_ID_A5_Inspection[rand] = id;
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
                    break;
                }
            }
        }
        if(currentLocation == A6)
        {
            int rand = Random.Range(0, inspectionAreasA6Occupied.Length);
            for(int i = 0; ; i++)
            {
                if(!inspectionAreasA6Occupied[rand])
                {
                    wayPointNowInUse = rand;
                    inspectionAreasA6Occupied[rand] = true;
                    newDestination = inspectionAreasA6[rand].position;
                    AI_ID_A6_Inspection[rand] = id;
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
                    break;
                }
            }
        }
        if(currentLocation == A7)
        {
            int rand = Random.Range(0, inspectionAreasA7Occupied.Length);
            for(int i = 0; ; i++)
            {
                if(!inspectionAreasA7Occupied[rand])
                {
                    wayPointNowInUse = rand;
                    inspectionAreasA7Occupied[rand] = true;
                    newDestination = inspectionAreasA7[rand].position;
                    AI_ID_A7_Inspection[rand] = id;
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
                    break;
                }
            }
        }
        AI[id].GetComponent<AIController2>().SetCurrentWayPointPos(wayPointNowInUse);
        return newDestination;
    }
    public Vector3 FindIdleSpot(int id, string area, int _lastJobIterator, string lastActivity)
    {
        Vector3 newDestination = Vector3.zero;
        string currentLocation  = area;
        int wayPointNowInUse = -1;
        int lastJobIterator = _lastJobIterator;
        iterator = 0;
        if(AI[id].GetComponent<AIController2>().GetCurrentLocation() == "null")
        {
            currentLocation = FindNearestLocation(id);
        }
       
        if(currentLocation == A1)
        {
            for(int i = 0; ; i++)
            {
                iterator++;
                int rand = Random.Range(0, idleAreasA1.Length);
                if(!idleAreasA1Occupied[rand])
                {
                    idleAreasA1Occupied[rand] = true;
                    newDestination = idleAreasA1[rand].position;
                    AI_ID_A1_Idle[rand] = id;
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
                    break;
                }
                if(iterator >= 100)
                {
                    AI[id].GetComponent<AIController2>().SetFindNewJob(true);
                    break;
                }
            }
        }
        if(currentLocation == A2)
        {
            for(int i = 0; ; i++)
            {
                int rand = Random.Range(0, idleAreasA2Occupied.Length);
                if(!idleAreasA2Occupied[rand])
                {
                    wayPointNowInUse = rand;
                    idleAreasA2Occupied[rand] = true;
                    newDestination = idleAreasA2[rand].position;
                    AI_ID_A2_Idle[rand] = id;
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
                    break;
                }
            }
        }
        if(currentLocation == A3)
        {
            int rand = Random.Range(0, idleAreasA3Occupied.Length);
            for(int i = 0; ; i++)
            {
                if(!idleAreasA3Occupied[rand])
                {
                    wayPointNowInUse = rand;
                    idleAreasA3Occupied[rand] = true;
                    newDestination = idleAreasA3[rand].position;
                    AI_ID_A3_Idle[rand] = id;
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
                    break;
                }
            }
        }
        if(currentLocation == A4)
        {
            int rand = Random.Range(0, idleAreasA4Occupied.Length);
            for(int i = 0; ; i++)
            {
                if(!idleAreasA4Occupied[rand])
                {
                    wayPointNowInUse = rand;
                    idleAreasA4Occupied[rand] = true;
                    newDestination = idleAreasA4[rand].position;
                    AI_ID_A4_Idle[rand] = id;
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
                    break;
                }
            }
        }
        if(currentLocation == A5)
        {
            int rand = Random.Range(0, idleAreasA5Occupied.Length);
            for(int i = 0; ; i++)
            {
                if(!idleAreasA5Occupied[rand])
                {
                    wayPointNowInUse = rand;
                    idleAreasA5Occupied[rand] = true;
                    newDestination = idleAreasA5[rand].position;
                    AI_ID_A5_Idle[rand] = id;
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
                    break;
                }
            }
        }
        if(currentLocation == A6)
        {
            int rand = Random.Range(0, idleAreasA6Occupied.Length);
            for(int i = 0; ; i++)
            {
                if(!idleAreasA6Occupied[rand])
                {
                    wayPointNowInUse = rand;
                    idleAreasA6Occupied[rand] = true;
                    newDestination = idleAreasA6[rand].position;
                    AI_ID_A6_Idle[rand] = id;
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
                    break;
                }
            }
        }
        if(currentLocation == A7)
        {
            int rand = Random.Range(0, idleAreasA7Occupied.Length);
            for(int i = 0; ; i++)
            {
                if(!idleAreasA7Occupied[rand])
                {
                    wayPointNowInUse = rand;
                    idleAreasA7Occupied[rand] = true;
                    newDestination = idleAreasA7[rand].position;
                    AI_ID_A7_Idle[rand] = id;
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
                    break;
                }
            }
        }

        AI[id].GetComponent<AIController2>().SetCurrentWayPointPos(wayPointNowInUse);
        return newDestination;
    }
    public bool CheckIfHasSocialPartner(int id, string area, int currentPosID)
    {
        bool hasPartner = false;
        if(area == A1)
        {
            for(int j = 0; j < socialAreasA1.Length ;j++)
            {
                if(evenNumbers[j] == currentPosID)
                {
                    int altJ = evenNumbers[j] + 1;
                    if(socialAreasA1Occupied[altJ])
                    {
                        AI[altJ].GetComponent<AIController2>().SetSocialAudio(true);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                if(oddNumbers[j] == currentPosID)
                {
                    int altJ = oddNumbers[j] - 1;
                    if(socialAreasA1Occupied[altJ])
                    {
                        AI[altJ].GetComponent<AIController2>().SetSocialAudio(true);
                        return true;
                    }
                    else
                    {
                        return false;
                    }  
                }
            }
        }
        if(area == A2)
        {
            for(int j = 0; j < socialAreasA2.Length ;j++)
            {
                if(evenNumbers[j] == currentPosID)
                {
                    int altJ = evenNumbers[j] + 1;
                    if(socialAreasA2Occupied[altJ])
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                if(oddNumbers[j] == currentPosID)
                {
                    int altJ = oddNumbers[j] - 1;
                    if(socialAreasA2Occupied[altJ])
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }  
                }
            }
        }
        if(area == A3)
        {
            for(int j = 0; j < socialAreasA3.Length ;j++)
            {
                if(evenNumbers[j] == currentPosID)
                {
                    int altJ = evenNumbers[j] + 1;
                    if(socialAreasA3Occupied[altJ])
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                if(oddNumbers[j] == currentPosID)
                {
                    int altJ = oddNumbers[j] - 1;
                    if(socialAreasA3Occupied[altJ])
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }  
                }
            }
        }
        if(area == A4)
        {
            for(int j = 0; j < socialAreasA4.Length ;j++)
            {
                if(evenNumbers[j] == currentPosID)
                {
                    int altJ = evenNumbers[j] + 1;
                    if(socialAreasA4Occupied[altJ])
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                if(oddNumbers[j] == currentPosID)
                {
                    int altJ = oddNumbers[j] - 1;
                    if(socialAreasA4Occupied[altJ])
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }  
                }
            }
        }
        if(area == A5)
        {
            for(int j = 0; j < socialAreasA5.Length ;j++)
            {
                if(evenNumbers[j] == currentPosID)
                {
                    int altJ = evenNumbers[j] + 1;
                    if(socialAreasA5Occupied[altJ])
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                if(oddNumbers[j] == currentPosID)
                {
                    int altJ = oddNumbers[j] - 1;
                    if(socialAreasA5Occupied[altJ])
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }  
                }
            }
        }
        if(area == A6)
        {
            for(int j = 0; j < socialAreasA6.Length ;j++)
            {
                if(evenNumbers[j] == currentPosID)
                {
                    int altJ = evenNumbers[j] + 1;
                    if(socialAreasA6Occupied[altJ])
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                if(oddNumbers[j] == currentPosID)
                {
                    int altJ = oddNumbers[j] - 1;
                    if(socialAreasA6Occupied[altJ])
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }  
                }
            }
        }
        if(area == A7)
        {
            for(int j = 0; j < socialAreasA7.Length ;j++)
            {
                if(evenNumbers[j] == currentPosID)
                {
                    int altJ = evenNumbers[j] + 1;
                    if(socialAreasA7Occupied[altJ])
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                if(oddNumbers[j] == currentPosID)
                {
                    int altJ = oddNumbers[j] - 1;
                    if(socialAreasA7Occupied[altJ])
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }  
                }
            }
        }
        return hasPartner;
    }
   // quick fix for clearing the spot the social partner was in, still needs to be done for all other zones.
    public void TellSocialPartnerAILeft(int id, string area, int currentPosID)
    {
        int socialPosID = 0;

        if(area == A1)
        {
            for(int j = 0; j < socialAreasA1.Length ;j++)
            {
                if(evenNumbers[j] == currentPosID)
                {
                    socialPosID = evenNumbers[j] + 1;
                    if(AI_ID_A1_Social[socialPosID] != -1)
                    {   
                        int newId = AI_ID_A1_Social[socialPosID];
                        AI[newId].GetComponent<AIController2>().SetSocialPartnerLeftEarly(true);
                        AI_ID_A1_Social[newId] = -1;
                        socialAreasA1Occupied[newId] = false;
                        break;
                    }          
                }
                if(oddNumbers[j] == currentPosID)
                {
                    socialPosID = oddNumbers[j] - 1;   
                    if(AI_ID_A1_Social[socialPosID] != -1)
                    {   
                        int newId = AI_ID_A1_Social[socialPosID];
                        AI[newId].GetComponent<AIController2>().SetSocialPartnerLeftEarly(true);
                        AI_ID_A1_Social[newId] = -1;
                        socialAreasA1Occupied[newId] = false;
                        break;
                    }
                }
            }
        }
        if(area == A2)
        {
            for(int j = 0; j < socialAreasA2.Length ;j++)
            {
                if(evenNumbers[j] == currentPosID)
                {
                    socialPosID = evenNumbers[j] + 1;
                    if(AI_ID_A2_Social[socialPosID] != -1)
                    {   
                        int newId = AI_ID_A2_Social[socialPosID];
                        AI[newId].GetComponent<AIController2>().SetSocialPartnerLeftEarly(true);
                        break;
                    }          
                }
                if(oddNumbers[j] == currentPosID)
                {
                    socialPosID = oddNumbers[j] - 1;   
                    if(AI_ID_A2_Social[socialPosID] != -1)
                    {   
                        int newId = AI_ID_A2_Social[socialPosID];
                        AI[newId].GetComponent<AIController2>().SetSocialPartnerLeftEarly(true);
                        break;
                    }
                }
            }
        }
        if(area == A3)
        {
            for(int j = 0; j < socialAreasA3.Length ;j++)
            {
                if(evenNumbers[j] == currentPosID)
                {
                    socialPosID = evenNumbers[j] + 1;
                    if(AI_ID_A3_Social[socialPosID] != -1)
                    {   
                        int newId = AI_ID_A3_Social[socialPosID];
                        AI[newId].GetComponent<AIController2>().SetSocialPartnerLeftEarly(true);
                        break;
                    }          
                }
                if(oddNumbers[j] == currentPosID)
                {
                    socialPosID = oddNumbers[j] - 1;   
                    if(AI_ID_A3_Social[socialPosID] != -1)
                    {   
                        int newId = AI_ID_A3_Social[socialPosID];
                        AI[newId].GetComponent<AIController2>().SetSocialPartnerLeftEarly(true);
                        break;
                    }
                }
            }
        }
        if(area == A4)
        {
            for(int j = 0; j < socialAreasA4.Length ;j++)
            {
                if(evenNumbers[j] == currentPosID)
                {
                    socialPosID = evenNumbers[j] + 1;
                    if(AI_ID_A4_Social[socialPosID] != -1)
                    {   
                        int newId = AI_ID_A4_Social[socialPosID];
                        AI[newId].GetComponent<AIController2>().SetSocialPartnerLeftEarly(true);
                        break;
                    }          
                }
                if(oddNumbers[j] == currentPosID)
                {
                    socialPosID = oddNumbers[j] - 1;   
                    if(AI_ID_A4_Social[socialPosID] != -1)
                    {   
                        int newId = AI_ID_A4_Social[socialPosID];
                        AI[newId].GetComponent<AIController2>().SetSocialPartnerLeftEarly(true);
                        break;
                    }
                }
            }
        }
        if(area == A5)
        {
            for(int j = 0; j < socialAreasA5.Length ;j++)
            {
                if(evenNumbers[j] == currentPosID)
                {
                    socialPosID = evenNumbers[j] + 1;
                    if(AI_ID_A5_Social[socialPosID] != -1)
                    {   
                        int newId = AI_ID_A5_Social[socialPosID];
                        AI[newId].GetComponent<AIController2>().SetSocialPartnerLeftEarly(true);
                        break;
                    }          
                }
                if(oddNumbers[j] == currentPosID)
                {
                    socialPosID = oddNumbers[j] - 1;   
                    if(AI_ID_A5_Social[socialPosID] != -1)
                    {   
                        int newId = AI_ID_A5_Social[socialPosID];
                        AI[newId].GetComponent<AIController2>().SetSocialPartnerLeftEarly(true);
                        break;
                    }
                }
            }
        }
        if(area == A6)
        {
            for(int j = 0; j < socialAreasA6.Length ;j++)
            {
                if(evenNumbers[j] == currentPosID)
                {
                    socialPosID = evenNumbers[j] + 1;
                    if(AI_ID_A6_Social[socialPosID] != -1)
                    {   
                        int newId = AI_ID_A6_Social[socialPosID];
                        AI[newId].GetComponent<AIController2>().SetSocialPartnerLeftEarly(true);
                        break;
                    }          
                }
                if(oddNumbers[j] == currentPosID)
                {
                    socialPosID = oddNumbers[j] - 1;   
                    if(AI_ID_A6_Social[socialPosID] != -1)
                    {   
                        int newId = AI_ID_A6_Social[socialPosID];
                        AI[newId].GetComponent<AIController2>().SetSocialPartnerLeftEarly(true);
                        break;
                    }
                }
            }
        }
        if(area == A7)
        {
            for(int j = 0; j < socialAreasA7.Length ;j++)
            {
                if(evenNumbers[j] == currentPosID)
                {
                    socialPosID = evenNumbers[j] + 1;
                    if(AI_ID_A7_Social[socialPosID] != -1)
                    {   
                        int newId = AI_ID_A7_Social[socialPosID];
                        AI[newId].GetComponent<AIController2>().SetSocialPartnerLeftEarly(true);
                        break;
                    }          
                }
                if(oddNumbers[j] == currentPosID)
                {
                    socialPosID = oddNumbers[j] - 1;   
                    if(AI_ID_A7_Social[socialPosID] != -1)
                    {   
                        int newId = AI_ID_A7_Social[socialPosID];
                        AI[newId].GetComponent<AIController2>().SetSocialPartnerLeftEarly(true);
                        break;
                    }
                }
            }
        }
    }
    public Vector3 GetChildPositionOfInspectionPos(int id, string currentPos, int currentPosID)
    {
        Vector3 rotation = Vector3.zero;
        
        if(currentPos == A1)
        {
            rotation = inspectionAreasA1[currentPosID].GetChild(0).transform.position;
            return rotation;
        }
        if(currentPos == A2)
        {
            rotation = inspectionAreasA2[currentPosID].GetChild(0).transform.position;
            return rotation;
        }
        if(currentPos == A3)
        {
            rotation = inspectionAreasA3[currentPosID].GetChild(0).transform.position;
            return rotation;
        }
        if(currentPos == A4)
        {
            rotation = inspectionAreasA4[currentPosID].GetChild(0).transform.position;
            return rotation;
        }
        if(currentPos == A5)
        {
            rotation = inspectionAreasA5[currentPosID].GetChild(0).transform.position;
            return rotation;
        }
        if(currentPos == A6)
        {
            rotation = inspectionAreasA6[currentPosID].GetChild(0).transform.position;
            return rotation;
        }
        if(currentPos == A7)
        {
            rotation = inspectionAreasA7[currentPosID].GetChild(0).transform.position;
            return rotation;
        }

        return rotation;
    }
    public Vector3 GetChildPositionOfIdlePos(int id, string currentPos, int currentPosID)
    {
        Vector3 rotation = Vector3.zero;
        if(currentPos == A1)
        {
            rotation = idleAreasA1[currentPosID].GetChild(0).transform.position;
            return rotation;
        }
        if(currentPos == A2)
        {
            rotation = idleAreasA2[currentPosID].GetChild(0).transform.position;
            return rotation;
        }
        if(currentPos == A3)
        {
            rotation = idleAreasA3[currentPosID].GetChild(0).transform.position;
            return rotation;
        }
        if(currentPos == A4)
        {
            rotation = idleAreasA4[currentPosID].GetChild(0).transform.position;
            return rotation;
        }
        if(currentPos == A5)
        {
            rotation = idleAreasA5[currentPosID].GetChild(0).transform.position;
            return rotation;
        }
        if(currentPos == A6)
        {
            rotation = idleAreasA6[currentPosID].GetChild(0).transform.position;
            return rotation;
        }
        if(currentPos == A7)
        {
            rotation = idleAreasA7[currentPosID].GetChild(0).transform.position;
            return rotation;
        }
        return rotation;
    }  
    public Vector3 GetChildPositionOfSeatPos(int id, string currentPos, int currentPosID)
    {
        Vector3 rotation = Vector3.zero;
        if(currentPos == A1)
        {
            rotation = seatingAreasA1[currentPosID].GetChild(0).transform.position;
            return rotation;
        }
         if(currentPos == A2)
        {
            rotation = seatingAreasA2[currentPosID].GetChild(0).transform.position;
            return rotation;
        }
         if(currentPos == A3)
        {
            rotation = seatingAreasA3[currentPosID].GetChild(0).transform.position;
            return rotation;
        }
         if(currentPos == A4)
        {
            rotation = seatingAreasA4[currentPosID].GetChild(0).transform.position;
            return rotation;
        }
         if(currentPos == A5)
        {
            rotation = seatingAreasA5[currentPosID].GetChild(0).transform.position;
            return rotation;
        }
         if(currentPos == A6)
        {
            rotation = seatingAreasA6[currentPosID].GetChild(0).transform.position;
            return rotation;
        }
         if(currentPos == A7)
        {
            rotation = seatingAreasA7[currentPosID].GetChild(0).transform.position;
            return rotation;
        }
        return rotation;
    }  
    public void CheckIdleLean(int id, string currentPos, int currentPosID)
    {
        if(currentPos == A1)
        {
            if(idleAreasA1[currentPosID].tag == "IdleLeanBack")
            {
                AI[id].GetComponent<AIController2>().SetIdleLeaning(idleAreasA1[currentPosID].tag);
            }
            if(idleAreasA1[currentPosID].tag == "IdleLeanLeft")
            {
                AI[id].GetComponent<AIController2>().SetIdleLeaning(idleAreasA1[currentPosID].tag);
            }
            if(idleAreasA1[currentPosID].tag == "IdleLeanRight")
            {
                AI[id].GetComponent<AIController2>().SetIdleLeaning(idleAreasA1[currentPosID].tag);
            }
        }
        if(currentPos == A2)
        {
            if(idleAreasA2[currentPosID].tag == "IdleLeanBack")
            {
                AI[id].GetComponent<AIController2>().SetIdleLeaning(idleAreasA2[currentPosID].tag);
            }
            if(idleAreasA2[currentPosID].tag == "IdleLeanLeft")
            {
                AI[id].GetComponent<AIController2>().SetIdleLeaning(idleAreasA2[currentPosID].tag);
            }
            if(idleAreasA2[currentPosID].tag == "IdleLeanRight")
            {
                AI[id].GetComponent<AIController2>().SetIdleLeaning(idleAreasA2[currentPosID].tag);
            }
        }
        if(currentPos == A3)
        {
            if(idleAreasA3[currentPosID].tag == "IdleLeanBack")
            {
                AI[id].GetComponent<AIController2>().SetIdleLeaning(idleAreasA3[currentPosID].tag);
            }
            if(idleAreasA3[currentPosID].tag == "IdleLeanLeft")
            {
                AI[id].GetComponent<AIController2>().SetIdleLeaning(idleAreasA3[currentPosID].tag);
            }
            if(idleAreasA3[currentPosID].tag == "IdleLeanRight")
            {
                AI[id].GetComponent<AIController2>().SetIdleLeaning(idleAreasA3[currentPosID].tag);
            }
        }
        if(currentPos == A4)
        {
            if(idleAreasA4[currentPosID].tag == "IdleLeanBack")
            {
                AI[id].GetComponent<AIController2>().SetIdleLeaning(idleAreasA4[currentPosID].tag);
            }
            if(idleAreasA4[currentPosID].tag == "IdleLeanLeft")
            {
                AI[id].GetComponent<AIController2>().SetIdleLeaning(idleAreasA4[currentPosID].tag);
            }
            if(idleAreasA4[currentPosID].tag == "IdleLeanRight")
            {
                AI[id].GetComponent<AIController2>().SetIdleLeaning(idleAreasA4[currentPosID].tag);
            }
        }
        if(currentPos == A5)
        {
            if(idleAreasA5[currentPosID].tag == "IdleLeanBack")
            {
                AI[id].GetComponent<AIController2>().SetIdleLeaning(idleAreasA5[currentPosID].tag);
            }
            if(idleAreasA5[currentPosID].tag == "IdleLeanLeft")
            {
                AI[id].GetComponent<AIController2>().SetIdleLeaning(idleAreasA5[currentPosID].tag);
            }
            if(idleAreasA5[currentPosID].tag == "IdleLeanRight")
            {
                AI[id].GetComponent<AIController2>().SetIdleLeaning(idleAreasA5[currentPosID].tag);
            }
        }
        if(currentPos == A6)
        {
            if(idleAreasA6[currentPosID].tag == "IdleLeanBack")
            {
                AI[id].GetComponent<AIController2>().SetIdleLeaning(idleAreasA6[currentPosID].tag);
            }
            if(idleAreasA6[currentPosID].tag == "IdleLeanLeft")
            {
                AI[id].GetComponent<AIController2>().SetIdleLeaning(idleAreasA6[currentPosID].tag);
            }
            if(idleAreasA6[currentPosID].tag == "IdleLeanRight")
            {
                AI[id].GetComponent<AIController2>().SetIdleLeaning(idleAreasA6[currentPosID].tag);
            }
        }
        if(currentPos == A7)
        {
            if(idleAreasA7[currentPosID].tag == "IdleLeanBack")
            {
                AI[id].GetComponent<AIController2>().SetIdleLeaning(idleAreasA7[currentPosID].tag);
            }
            if(idleAreasA7[currentPosID].tag == "IdleLeanLeft")
            {
                AI[id].GetComponent<AIController2>().SetIdleLeaning(idleAreasA7[currentPosID].tag);
            }
            if(idleAreasA7[currentPosID].tag == "IdleLeanRight")
            {
                AI[id].GetComponent<AIController2>().SetIdleLeaning(idleAreasA7[currentPosID].tag);
            }
        }
    } 
    public Vector3 GetSocialPartnerPos(int id, string currentArea, int currentPosID)
    {
       Vector3 rotation = Vector3.zero;
        
        if(currentArea == A1)
        {
            rotation = socialAreasA1[currentPosID].GetChild(0).transform.position;
            return rotation;
        }
        if(currentArea == A2)
        {
            rotation = socialAreasA2[currentPosID].GetChild(0).transform.position;
            return rotation;
        }
        if(currentArea == A3)
        {
            rotation = socialAreasA3[currentPosID].GetChild(0).transform.position;
            return rotation;
        }
        if(currentArea == A4)
        {
            rotation = socialAreasA4[currentPosID].GetChild(0).transform.position;
            return rotation;
        }
        if(currentArea == A5)
        {
            rotation = socialAreasA5[currentPosID].GetChild(0).transform.position;
            return rotation;
        }
        if(currentArea == A6)
        {
            rotation = socialAreasA6[currentPosID].GetChild(0).transform.position;
            return rotation;
        }
        if(currentArea == A7)
        {
            rotation = socialAreasA7[currentPosID].GetChild(0).transform.position;
            return rotation;
        }
            
        return rotation;
    }
    public bool CheckForWalkingSpotsAvailable(string area)
    {
        if(area == A1)
        {
            for(int i = 0; i < walkingWaypointsA1.Length; i++)
            {
                if(!walkingPointsA1Occupied[i])
                {
                    return true;
                }
            }
        }
        if(area == A2)
        {
            for(int i = 0; i < walkingWaypointsA2.Length; i++)
            {
                if(!walkingPointsA2Occupied[i])
                {
                    return true;
                }
            }
        }
        if(area == A3)
        {
            for(int i = 0; i < walkingWaypointsA3.Length; i++)
            {
                if(!walkingPointsA3Occupied[i])
                {
                    return true;
                }
            }
        }
        if(area == A4)
        {
            for(int i = 0; i < walkingWaypointsA4.Length; i++)
            {
                if(!walkingPointsA4Occupied[i])
                {
                    return true;
                }
            }
        }
        if(area == A5)
        {
            for(int i = 0; i < walkingWaypointsA5.Length; i++)
            {
                if(!walkingPointsA5Occupied[i])
                {
                    return true;
                }
            }
        }
        if(area == A6)
        {
            for(int i = 0; i < walkingWaypointsA6.Length; i++)
            {
                if(!walkingPointsA6Occupied[i])
                {
                    return true;
                }
            }
        }
        if(area == A7)
        {
            for(int i = 0; i < walkingWaypointsA7.Length; i++)
            {
                if(!walkingPointsA7Occupied[i])
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool CheckForInspectionSpotsAvailable(string area)
    {
        if(area == A1)
        {
            for(int i = 0; i < inspectionAreasA1.Length; i++)
            {
                if(!inspectionAreasA1Occupied[i])
                {
                    return true;
                }
            }
        }
        if(area == A2)
        {
            for(int i = 0; i < inspectionAreasA2.Length; i++)
            {
                if(!inspectionAreasA2Occupied[i])
                {
                    return true;
                }
            }
        }
        if(area == A3)
        {
            for(int i = 0; i < inspectionAreasA3.Length; i++)
            {
                if(!inspectionAreasA3Occupied[i])
                {
                    return true;
                }
            }
        }
        if(area == A4)
        {
            for(int i = 0; i < inspectionAreasA4.Length; i++)
            {
                if(!inspectionAreasA4Occupied[i])
                {
                    return true;
                }
            }
        }
        if(area == A5)
        {
            for(int i = 0; i < inspectionAreasA5.Length; i++)
            {
                if(!inspectionAreasA5Occupied[i])
                {
                    return true;
                }
            }
        }
        if(area == A6)
        {
            for(int i = 0; i < inspectionAreasA6.Length; i++)
            {
                if(!inspectionAreasA6Occupied[i])
                {
                    return true;
                }
            }
        }
        if(area == A7)
        {
            for(int i = 0; i < inspectionAreasA7.Length; i++)
            {
                if(!inspectionAreasA7Occupied[i])
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool CheckForIdleSpotsAvailable(string area)
    {
        if(area == A1)
        {
            for(int i = 0; i < idleAreasA1.Length; i++)
            {
                if(!idleAreasA1Occupied[i])
                {
                    return true;
                }
            }
        }
        if(area == A2)
        {
            for(int i = 0; i < idleAreasA2.Length; i++)
            {
                if(!idleAreasA2Occupied[i])
                {
                    return true;
                }
            }
        }
        if(area == A3)
        {
            for(int i = 0; i < idleAreasA3.Length; i++)
            {
                if(!idleAreasA3Occupied[i])
                {
                    return true;
                }
            }
        }
        if(area == A4)
        {
            for(int i = 0; i < idleAreasA4.Length; i++)
            {
                if(!idleAreasA4Occupied[i])
                {
                    return true;
                }
            }
        }
        if(area == A5)
        {
            for(int i = 0; i < idleAreasA5.Length; i++)
            {
                if(!idleAreasA5Occupied[i])
                {
                    return true;
                }
            }
        }
        if(area == A6)
        {
            for(int i = 0; i < idleAreasA6.Length; i++)
            {
                if(!idleAreasA6Occupied[i])
                {
                    return true;
                }
            }
        }
        if(area == A7)
        {
            for(int i = 0; i < idleAreasA7.Length; i++)
            {
                if(!idleAreasA7Occupied[i])
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool CheckForSocialSpotsAvailable(string area)
    {
        if(area == A1)
        {
            for(int i = 0; i < socialAreasA1.Length; i++)
            {
                if(!socialAreasA1Occupied[i])
                {
                    return true;
                }
            }
        }
        if(area == A2)
        {
            for(int i = 0; i < socialAreasA2.Length; i++)
            {
                if(!socialAreasA2Occupied[i])
                {
                    return true;
                }
            }
        }
        if(area == A3)
        {
            for(int i = 0; i < socialAreasA3.Length; i++)
            {
                if(!socialAreasA3Occupied[i])
                {
                    return true;
                }
            }
        }
        if(area == A4)
        {
            for(int i = 0; i < socialAreasA4.Length; i++)
            {
                if(!socialAreasA4Occupied[i])
                {
                    return true;
                }
            }
        }
        if(area == A5)
        {
            for(int i = 0; i < socialAreasA5.Length; i++)
            {
                if(!socialAreasA5Occupied[i])
                {
                    return true;
                }
            }
        }
        if(area == A6)
        {
            for(int i = 0; i < socialAreasA6.Length; i++)
            {
                if(!socialAreasA6Occupied[i])
                {
                    return true;
                }
            }
        }
        if(area == A7)
        {
            for(int i = 0; i < socialAreasA7.Length; i++)
            {
                if(!socialAreasA7Occupied[i])
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool CheckForSeatingSpotsAvailable(string area)
    {
        if(area == A1)
        {
            for(int i = 0; i < seatingAreasA1.Length; i++)
            {
                if(!seatingAreasA1Occupied[i])
                {
                    return true;
                }
            }
        }
        if(area == A2)
        {
            for(int i = 0; i < seatingAreasA2.Length; i++)
            {
                 if(!seatingAreasA2Occupied[i])
                {
                    return true;
                }
            }
        }
        if(area == A3)
        {
            for(int i = 0; i < seatingAreasA3.Length; i++)
            {
                 if(!seatingAreasA3Occupied[i])
                {
                    return true;
                }
            }
        }
        if(area == A4)
        {
            for(int i = 0; i < seatingAreasA4.Length; i++)
            {
                 if(!seatingAreasA4Occupied[i])
                {
                    return true;
                }
            }
        }
        if(area == A5)
        {
            for(int i = 0; i < seatingAreasA5.Length; i++)
            {
                 if(!seatingAreasA5Occupied[i])
                {
                    return true;
                }
            }
        }
        if(area == A6)
        {
            for(int i = 0; i < seatingAreasA6.Length; i++)
            {
                 if(!seatingAreasA6Occupied[i])
                {
                    return true;
                }
            }
        }
        if(area == A7)
        {
            for(int i = 0; i < seatingAreasA7.Length; i++)
            {
                 if(!seatingAreasA7Occupied[i])
                {
                    return true;
                }
            }
        }
        return false;
    }
}
