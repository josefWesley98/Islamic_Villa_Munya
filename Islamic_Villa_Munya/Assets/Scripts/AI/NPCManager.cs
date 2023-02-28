using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    [Header("Areas")]
    [SerializeField] private GameObject diningArea;
    private string DA = "Dining_Area";
    [SerializeField] private GameObject relaxationAreaLeft;
    private string RAL = "Relaxation_Area_Left";
    [SerializeField] private GameObject relaxationAreaRight;
    private string RAR = "Relation_Area_Right";
    [SerializeField] private GameObject roomLeft;
    private string RL = "Room_Left";
    [SerializeField] private GameObject roomRight;
    private string RR = "Room_Right";
    [SerializeField] private GameObject upstairsAreaOne;
    private string UAO = "Upstairs_Area_One";
    [SerializeField] private GameObject upstairsAreaTwo;
    private string UAT = "Upstairs_Area_Two";
    private int locationCount = 7;
    //dining room
    [Header("Dining Room")]
    [SerializeField] private Transform[] walkingWaypointsDA;
    [SerializeField] private bool[] walkingPointsDAOccupied;
    [SerializeField] private int[] AI_ID_DA_WP;
    [SerializeField] private Transform[] seatingDA;
    [SerializeField] private bool[] seatingDAOccupied;
    [SerializeField] private int[] AI_ID_DA_Seating;
    [SerializeField] private Transform[] socialAreasDA;
    [SerializeField] private bool[] socialAreasDAOccupied;
    [SerializeField] private int[] AI_ID_DA_social;

    //relaxation area left
    [Header("Relaxation Area Left")]
    [SerializeField] private Transform[] walkingWaypointsRAL;
    [SerializeField] private bool[] walkingPointsRALOccupied;
    [SerializeField] private int[] AI_ID_RAL_WP;
    [SerializeField] private Transform[] socialAreasRAL;
    [SerializeField] private bool[] socialAreasRALOccupied;
    [SerializeField] private int[] AI_ID_RAL_Social;
    
    //relaxation area right
    [Header("Relaxation Area Right")]
    [SerializeField] private Transform[] walkingWaypointsRAR;
    [SerializeField] private bool[] walkingPointsRAROccupied;
    [SerializeField] private int[] AI_ID_RAR_WP;
    [SerializeField] private Transform[] socialAreasRAR;
    [SerializeField] private bool[] socialAreasRAROccupied;
    [SerializeField] private int[] AI_ID_RAR_Social;

    //room left
    [Header("Room Left")]
    [SerializeField] private Transform[] walkingWaypointsRL;
    [SerializeField] private bool[] walkingPointsRLOccupied;
    [SerializeField] private int[] AI_ID_RL_WP;
    [SerializeField] private Transform[] socialAreasRL;
    [SerializeField] private bool[] socialAreasRLOccupied;
    [SerializeField] private int[] AI_ID_RL_Social;

    // room right
    [Header("Room Right")]
    [SerializeField] private Transform[] walkingWaypointsRR;
    [SerializeField] private bool[] walkingPointsRROccupied;
    [SerializeField] private int[] AI_ID_RR_WP;
    [SerializeField] private Transform[] socialAreasRR;
    [SerializeField] private bool[] socialAreasRROccupied;
    [SerializeField] private int[] AI_ID_RR_Social;

    //upper area one
    [Header("Upper Area One")]
    [SerializeField] private Transform[] walkingWaypointsUAO;
    [SerializeField] private bool[] walkingPointsUAOOccupied;
    [SerializeField] private int[] AI_ID_UAO_WP;
    [SerializeField] private Transform[] socialAreasUAO;
    [SerializeField] private bool[] socialAreasUAOOccupied;
    [SerializeField] private int[] AI_ID_UAO_Social;

    //upper area two
    [Header("Upper Area Two")]
    [SerializeField] private Transform[] walkingWaypointsUAT;
    [SerializeField] private bool[] walkingPointsUATOccupied;
    [SerializeField] private int[] AI_ID_UAT_WP;
    [SerializeField] private Transform[] socialAreasUAT;
    [SerializeField] private bool[] socialAreasUATOccupied;
    [SerializeField] private int[] AI_ID_UAT_Social;

    [Header("AI")]
    [SerializeField] private GameObject[] AI;
    private bool[] AIMoving;
    private bool[] AIIdle;
    private bool[] AIChatting;
    private bool[] AISitting;
    
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
    // Start is called before the first frame update
    void Start()
    {
        // initalise dining area.
        if(diningArea != null)
        {
            int childrenWayP = diningArea.transform.GetChild(0).transform.childCount;
            for(int i = 0; i < childrenWayP; i++)
            {
                walkingWaypointsDA[i] = diningArea.transform.GetChild(0).transform.GetChild(i).transform;
                AI_ID_DA_WP[i] = -1;
                walkingPointsDAOccupied[i] = false;
            }

            int childrenSocialP = diningArea.transform.GetChild(1).transform.childCount;
            for(int i = 0; i < childrenSocialP; i++)
            {
                socialAreasDA[i] = diningArea.transform.GetChild(1).transform.GetChild(i).transform;
                AI_ID_DA_social[i] = -1;
                socialAreasDAOccupied[i] = false;
            }

            int childrenSeatingP = diningArea.transform.GetChild(2).transform.childCount;
            for(int i = 0; i < childrenSeatingP; i++)
            {
                seatingDA[i] = diningArea.transform.GetChild(2).transform.GetChild(i).transform;
                AI_ID_DA_Seating[i] = -1;
                seatingDAOccupied[i] = false;
            }
        }
        // initalise relaxation area left.
        if(relaxationAreaLeft != null)
        {
            int childrenWayP = relaxationAreaLeft.transform.GetChild(0).transform.childCount;
            for(int i = 0; i < childrenWayP; i++)
            {
                walkingWaypointsRAL[i] = relaxationAreaLeft.transform.GetChild(0).transform.GetChild(i).transform;
                AI_ID_RAL_WP[i] = -1;
                walkingPointsRALOccupied[i] = false;
            }

            int childrenSocialP = relaxationAreaLeft.transform.GetChild(1).transform.childCount;
            for(int i = 0; i < childrenSocialP; i++)
            {
                socialAreasRAL[i] = diningArea.transform.GetChild(1).transform.GetChild(i).transform;
                AI_ID_RAL_Social[i] = -1;
                socialAreasRALOccupied[i] = false;
            }
        }
        // initalise relaxation area right.
        if(relaxationAreaRight != null)
        {
            int childrenWayP = relaxationAreaRight.transform.GetChild(0).transform.childCount;
            for(int i = 0; i < childrenWayP; i++)
            {
                walkingWaypointsRAR[i] = relaxationAreaRight.transform.GetChild(0).transform.GetChild(i).transform;
                AI_ID_RAR_WP[i] = -1;
                walkingPointsRAROccupied[i] = false;
            }

            int childrenSocialP = relaxationAreaRight.transform.GetChild(1).transform.childCount;
            for(int i = 0; i < childrenSocialP; i++)
            {
                socialAreasRAR[i] = relaxationAreaRight.transform.GetChild(1).transform.GetChild(i).transform;
                AI_ID_RAR_Social[i] = -1;
                socialAreasRAROccupied[i] = false;
            }
        }
        // initalise left room.
        if(roomLeft != null)
        {
            int childrenWayP = roomLeft.transform.GetChild(0).transform.childCount;
            for(int i = 0; i < childrenWayP; i++)
            {
                walkingWaypointsRL[i] = roomLeft.transform.GetChild(0).transform.GetChild(i).transform;
                AI_ID_RL_WP[i] = -1;
                walkingPointsRLOccupied[i] = false;
            }

            int childrenSocialP = roomLeft.transform.GetChild(1).transform.childCount;
            for(int i = 0; i < childrenSocialP; i++)
            {
                socialAreasRL[i] = roomLeft.transform.GetChild(1).transform.GetChild(i).transform;
                AI_ID_RL_Social[i] = -1;
                socialAreasRLOccupied[i] = false;
            }
        }
        // initialise right room.
        if(roomRight != null)
        {
            int childrenWayP = roomRight.transform.GetChild(0).transform.childCount;
            for(int i = 0; i < childrenWayP; i++)
            {
                walkingWaypointsRR[i] = roomRight.transform.GetChild(0).transform.GetChild(i).transform;
                AI_ID_RR_WP[i] = -1;
                walkingPointsRROccupied[i] = false;
            }

            int childrenSocialP = roomRight.transform.GetChild(1).transform.childCount;
            for(int i = 0; i < childrenSocialP; i++)
            {
                socialAreasRR[i] = roomRight.transform.GetChild(1).transform.GetChild(i).transform;
                AI_ID_RR_Social[i] = -1;
                socialAreasRROccupied[i] = false;
            }
        }
        // initalise upstairs area 1
        if(upstairsAreaOne != null)
        {
            int childrenWayP = upstairsAreaOne.transform.GetChild(0).transform.childCount;
            for(int i = 0; i < childrenWayP; i++)
            {
                walkingWaypointsUAO[i] = upstairsAreaOne.transform.GetChild(0).transform.GetChild(i).transform;
                AI_ID_UAO_WP[i] = -1;
                walkingPointsUAOOccupied[i] = false;
            }

            int childrenSocialP = upstairsAreaOne.transform.GetChild(1).transform.childCount;
            for(int i = 0; i < childrenSocialP; i++)
            {
                socialAreasUAO[i] = upstairsAreaOne.transform.GetChild(1).transform.GetChild(i).transform;
                AI_ID_UAO_Social[i] = -1;
                socialAreasUAOOccupied[i] = false;
            }
        }
        // initalise upstairs area 2
        if(upstairsAreaTwo != null)
        {
            int childrenWayP = upstairsAreaTwo.transform.GetChild(0).transform.childCount;
            for(int i = 0; i < childrenWayP; i++)
            {
                walkingWaypointsUAT[i] = upstairsAreaTwo.transform.GetChild(0).transform.GetChild(i).transform;
                AI_ID_UAT_WP[i] = -1;
                walkingPointsUATOccupied[i] = false;
            }

            int childrenSocialP = upstairsAreaTwo.transform.GetChild(1).transform.childCount;
            for(int i = 0; i < childrenSocialP; i++)
            {
                socialAreasUAT[i] = upstairsAreaTwo.transform.GetChild(1).transform.GetChild(i).transform;
                AI_ID_UAT_Social[i] = -1;
                socialAreasUATOccupied[i] = false;
            }
        }
       
    }
    public string FindNearestLocation(Transform currentPos)
    {
        string thePosition = "null";

        // find nearest locaton.

        return thePosition;
    }
    public Vector3 FindNewDestination(int id, string area, int currentWaypointPos)
    {
        Vector3 newDestination = Vector3.zero;
        string currentLocation = AI[id].GetComponent<AIController2>().GetCurrentLocation();
        for(int i = 0; i < locationCount; i++)
        {
            if(currentLocation == DA)
            {

            }
            if(currentLocation == RAL)
            {

            }
            if(currentLocation == RAR)
            {

            }
            if(currentLocation == RL)
            {

            }
            if(currentLocation == RR)
            {

            }
            if(currentLocation == UAO)
            {

            }
            if(currentLocation == UAT)
            {

            }
        }

        if(currentWaypointPos != -1)
        {
            SetLastWayPointToInactive(currentWaypointPos);
        }
        int newWayPointId = 0;

        // check for free way points in the area

        // do find new destination code here.

        // mark new way point as in use.

        AI[id].GetComponent<AIController2>().SetCurrentWayPointPos(newWayPointId);
        return newDestination;
    }
    public void SetLastWayPointToInactive(int wayPointId)
    {
        // reset way point to out of use.
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
