using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cal's script starts here*/
public class GameManager : MonoBehaviour
{
    private const int total_main_artefacts = 3;
    static protected int artefact_counter = 0;
    static protected bool current_artefact_collected = false;
    static protected bool artifactOneToBePlaced = false;
    static protected bool doOnce = true;
    //variables
    static protected bool[] player_artefact = new bool[total_main_artefacts];
    static protected bool has_key = false;
    static protected bool door_unlocked = false;
    static protected bool hub_travel = false;
    static protected bool pause_cursor = false;

    //Player selection
    static protected bool boy_select = false;
    static protected bool girl_select = false;

    //Difficulty Selection - Primary, secondary, university
    static protected bool easy_diff = false;
    static protected bool medium_diff = false;
    static protected bool hard_diff = false;

    // private void Start() 
    // {
    //     player_artefact = new bool[total_main_artefacts];
    //     Debug.Log("Call me");
    // }

    //Setters
    public static void SetArtefactCollected(int artefact_idx, bool val)
    {
        player_artefact[artefact_idx] = val;
        artefact_counter += 1;
        if(player_artefact[0] && doOnce)
        {
            artifactOneToBePlaced = true;
            doOnce = false;
        }
    }
    public static void SetArtifactOneToBePlaced(bool val)
    {
        artifactOneToBePlaced = false;
    }
    public static void SetCurrentArtefactCollected(bool val)
    {
        current_artefact_collected = val;
    }
    public static bool GetArtifactOneToBePlaced()
    {
        return artifactOneToBePlaced;
    }
    //When player quits the game, we reset every artefact to original state
    public static void ResetArtefacts(bool val)
    {
        for(int i = 0; i < total_main_artefacts; i++)
        {
            Debug.Log("resetting the artefacts");
            player_artefact[i] = val;
        }
    }

    public static void SetHasKey(bool val)
    {
        has_key = val;
    }

    public static void SetDoorUnlocked(bool val)
    {
        door_unlocked = val;
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
    public static bool GetArtefactCollected(int artefact_idx)
    {
        return player_artefact[artefact_idx];
    }

    public static bool GetCurrentArtefactCollected()
    {
        return current_artefact_collected;
    }

    public static bool GetHaveKey()
    {
        return has_key;
    }

    public static bool GetDoorUnlocked()
    {
        return door_unlocked;
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
    // lol
    public static bool GetHard()
    {
        return hard_diff;
    }

    public static int GetArtefactCounter()
    {
        return artefact_counter;
    }

    public static int GetTotalArtefacts()
    {
        return total_main_artefacts;
    }
}

/*Cal's script ends here*/
