using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cal's script starts here*/
public class GameManager : MonoBehaviour
{

    //variables
    static protected bool player_artefact = false;
    static protected bool has_key = false;
    static protected bool hub_travel = false;
    static protected bool pause_cursor = false;

    //Player selection
    static protected bool boy_select = false;
    static protected bool girl_select = false;

    //Difficulty Selection - Primary, secondary, university
    static protected bool easy_diff = false;
    static protected bool medium_diff = false;
    static protected bool hard_diff = false;


    Transform PLAYER;

    private void Start()
    {
        PLAYER = GameObject.FindGameObjectWithTag("Player").transform;
        if (PLAYER != null)
            print("Found " + PLAYER);
        else
            Debug.LogError("Player not found!");
    }

    //Setters
    public static void SetArtefactCollected(bool val)
    {
        player_artefact = val;
        print("ARTEFACT");
    }

    public static void SetHasKey(bool val)
    {
        has_key = val;
    }

    public static void SetHUBTravel(bool val)
    {
        hub_travel = val;
    }

    public static void SetPauseCursor(bool val)
    {
        pause_cursor = val;
    }

    public static void SetBoy(bool val)
    {
        boy_select = val;
    }

    public static void SetGirl(bool val)
    {
        girl_select = val;
    }

    public static void SetEasy(bool val)
    {
        easy_diff = val;
    }

    public static void SetMedium(bool val)
    {
        medium_diff = val;
    }

    public static void SetHard(bool val)
    {
        hard_diff = val;
    }

    //Getters
    public static bool GetArtefactCollected()
    {
        return player_artefact;
    }

    public static bool GetHaveKey()
    {
        return has_key;
    }

    public static bool GetHUBTravel()
    {
        return hub_travel;
    }

    public static bool GetPauseCursor()
    {
        return pause_cursor;
    }

    public static bool GetBoy()
    {
        return boy_select;
    }

    public static bool GetGirl()
    {
        return girl_select;
    }

    public static bool GetEasy()
    {
        return easy_diff;
    }

    public static bool GetMedium()
    {
        return medium_diff;
    }

    public static bool GetHard()
    {
        return hard_diff;
    }
}

/*Cal's script ends here*/
