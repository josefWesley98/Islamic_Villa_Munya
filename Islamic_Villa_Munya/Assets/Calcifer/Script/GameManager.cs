using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Leon code
public enum NamedDoors//names for doors that represent indexes in the door unlock array
{
    Unnamed = 0,
    Puzzle1Door = 1,
    MuseumEntranceL = 2,
}
//Leon end

/*Cal's script starts here*/
public class GameManager : MonoBehaviour
{
    private const int total_main_artefacts = 7;
    static protected int artefact_counter = 0;
    static protected bool current_artefact_collected = false;

    static protected bool[] artefactToBePlaced = new bool[7] {false, false, false, false, false, false, false};
    static protected bool[] doOnce = new bool[7] {true, true ,true, true, true, true, true};

    //variables
    static protected bool[] player_artefact = new bool[total_main_artefacts];
    static protected bool has_key = false;
    static protected bool door_unlocked = false;
    static protected bool hub_travel = false;
    static protected bool pause_cursor = false;
    static protected bool[] artefact_placed = new bool[total_main_artefacts];

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


//Leon code 
    //array of door unlock states (true or false for every door)
    static bool[] doorUnlockStates = new bool[3];
    //give a name to every index in the door unlock array instead of having to remember numbers

    private void Start()
    {
        doorUnlockStates[(int)NamedDoors.Unnamed] = true;
        doorUnlockStates[(int)NamedDoors.Puzzle1Door] = true;
        doorUnlockStates[(int)NamedDoors.MuseumEntranceL] = true;
    }

    public static void SetDoorUnlocked(NamedDoors doorName, bool value)
    {
        //print("index = " + doorName);
        if (doorName == NamedDoors.Unnamed)
            return;

        doorUnlockStates[(int)doorName] = value;

        //print("Set " + doorName.ToString() + " to " + value);
    }

    public static bool GetDoorUnlocked(NamedDoors doorName)
    {
        bool value = doorUnlockStates[(int)doorName];
        //print("Door " + doorName.ToString() + " is set to " + value);
        return value;
    }
//Leon end

    //Setters
    public static void SetArtefactPlaced(int idx, bool val)
    {
        artefact_placed[idx] = val;
    }

    public static void SetArtefactCollected(int artefact_idx, bool val)
    {
        player_artefact[artefact_idx] = val;
        artefact_counter += 1;

        if(!artefactToBePlaced[artefact_idx] && doOnce[artefact_idx])
        {
            artefactToBePlaced[artefact_idx] = true;
            doOnce[artefact_idx] = false;   
        }
      
    }
    public static void SetArtefactToBePlaced(bool val, int idx)
    {
        artefactToBePlaced[idx] = val;
    }

    public static void SetCurrentArtefactCollected(bool val)
    {
        current_artefact_collected = val;
    }
    public static bool GetArtefactToBePlaced(int idx)
    {
        return artefactToBePlaced[idx];
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

    // public static void SetDoorUnlocked(bool val)
    // {
    //     door_unlocked = val;
    // }

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
    public static bool GetArtefactPlaced(int idx)
    {
        return artefact_placed[idx];
    }

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

    // public static bool GetDoorUnlocked()
    // {
    //     return door_unlocked;
    // }

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
