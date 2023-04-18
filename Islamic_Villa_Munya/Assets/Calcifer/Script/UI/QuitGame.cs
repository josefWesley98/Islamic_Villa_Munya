using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cal's script starts here*/
public class QuitGame : MonoBehaviour
{
    //Reset variables in the GameManager
    public void QuitToMenu()
    {
        //Variables to reset
        GameManager.SetArtefactCollected(false);
        GameManager.SetHasKey(false);
        GameManager.SetBoy(false);
        GameManager.SetGirl(false);
        GameManager.SetEasy(false);
        GameManager.SetMedium(false);
        GameManager.SetHard(false);
    }
}
/*Cal's script ends here*/
