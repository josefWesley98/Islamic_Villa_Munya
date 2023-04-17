using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cal's code starts here*/
public class MessagePopup : MonoBehaviour
{
    [SerializeField] private GameObject msg;


    // Update is called once per frame
    void Start()
    {
        msg.SetActive(false);
    }
    
    private void OnTriggerEnter(Collider other) 
    {
        //So when the player returns to the HUB, we don't show the tutorial again
        if(other.gameObject.tag == "Player" && !GameManager.GetArtefactCollected())
        {
            msg.SetActive(true);
        }    
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            msg.SetActive(false);
        }    
    }

}

/*Cal's code ends here*/