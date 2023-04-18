using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cal's script starts here*/
public class QuitGame : MonoBehaviour
{
    //Serialise any variables or scripts that are required for a reset
    [SerializeField] Key key; //key object
    [SerializeField] PickedUp artefacts; //artefact object(s)
    [SerializeField] Door door; //Complicated door object

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

        //Reset key for collection
        key.SetkeyActive(true);

        //Reset all artefacts
        artefacts.SetArtefactActive(true);

        //Reset the door
        door.SetDoorActive(true);
    }
}
/*Cal's script ends here*/
