using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cal's script starts here*/
public class PostMessagePlayer : MonoBehaviour
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
        if(other.gameObject.tag == "Player" && GameManager.GetArtefactCollected(0))
        {
            msg.SetActive(true);
        }    
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            msg.SetActive(false);
            //Destroy(msg);
        }    
    }
}
/*Cal's script ends here*/
