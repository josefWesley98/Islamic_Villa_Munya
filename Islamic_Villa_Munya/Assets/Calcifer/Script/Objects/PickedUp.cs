using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cal's script starts here*/

//Basic picking up mechanic
public class PickedUp : MonoBehaviour
{
    bool is_picked = false;
    public NIThirdPersonController player_ref;

    //Test if object has been picked up
    private void OnCollisionEnter(Collision other) 
    {
            if(other.gameObject.tag == "Player")
            {
                //Item is picked up by the player
                is_picked = true;

                //Set the player collected bool to true
                GameManager.SetArtefactCollected(true);
                GameManager.SetHUBTravel(true);
                //Destroy the game object when picked up
                Destroy(gameObject);
            }
    }

    public bool GetPickedUp()
    {
        return is_picked;
    }
}
/*Cal's script starts here*/
