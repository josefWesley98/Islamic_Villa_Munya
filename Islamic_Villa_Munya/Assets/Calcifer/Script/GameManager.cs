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

    Transform PLAYER;

    private void Start()
    {
        PLAYER = GameObject.FindGameObjectWithTag("Player").transform;
        if (PLAYER != null)
            print("Found " + PLAYER);
        else
            Debug.LogError("Player not found!");
    }
    private void Update()
    {
        print("AAAAAAAA");
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
}

/*Cal's script ends here*/
