using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cal's script starts here*/

//Basic picking up mechanic
public class PickedUp : MonoBehaviour
{
    bool is_picked = false;
    [SerializeField] private int id;
    public NIThirdPersonController player_ref;

    private void Start() 
    {
        //Could be implemented better but if the artefact has already been collected then destroy the object
        //when loading back into the scene
        if(GameManager.GetArtefactCollected(id))
        {
            Destroy(gameObject);
        }    
    }

    //Test if object has been picked up
    private void OnCollisionEnter(Collision other) 
    {
            if(other.gameObject.tag == "Player")
            {
                Debug.Log("Got artefact");
                //Item is picked up by the player
                is_picked = true;

                //Set the player collected bool to true
                GameManager.SetArtefactCollected(id, true);
                GameManager.SetCurrentArtefactCollected(true);
                GameManager.SetHUBTravel(true);
                //Destroy the game object when picked up
                //Destroy(gameObject);
                gameObject.SetActive(false);
            }
    }

    public bool GetPickedUp()
    {
        return is_picked;
    }

    public void SetArtefactActive(bool val)
    {
        gameObject.SetActive(val);
    }

    public int GetID()
    {
        return id;
    }
}
/*Cal's script starts here*/
