using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    //variables
    static protected bool player_artefact = false;

    public static void SetArtefactCollected(bool val)
    {
        player_artefact = val;
    }

    public static bool GetArtefactCollected()
    {
        return player_artefact;
    }
}
