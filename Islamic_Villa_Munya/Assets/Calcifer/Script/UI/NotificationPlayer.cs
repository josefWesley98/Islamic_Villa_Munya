using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*Modifed by Cal*/
public class NotificationPlayer : MonoBehaviour
{
    private bool freeze = false;
    private bool active = true;
    [SerializeField] GameObject Notification;
    private GameObject player_ref;
    private Collider col;
    Keyboard kb;
    private bool player_confirmed = false;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();
        kb = InputSystem.GetDevice<Keyboard>();
    }

    // Update is called once per frame
    void Update()
    {
        player_confirmed = kb.spaceKey.isPressed;

        if(player_confirmed && freeze == true)
        {
            //Activate player input again

            Debug.Log("pRESSED");
            Notification.SetActive(false);
            Time.timeScale = 1f;
            freeze = false;
            Destroy(col);
            
            player_ref.GetComponent<NIThirdPersonController>().SetInput(false);
        }
    }

    /*Modified by Cal*/
    private void OnTriggerExit(Collider other)
    {
        //Only want to show the 'You're free to explore' message once then no more after that so check if the second artefact has been found. If it has then never show the message 
        if (other.gameObject.tag == "Player" && GameManager.GetArtefactCollected(0) && !GameManager.GetArtefactCollected(1) && active)
        {
            //Disable player input
            player_ref = other.gameObject;
            
            player_ref.GetComponent<NIThirdPersonController>().SetInput(true);

            Notification.SetActive(true);
            Time.timeScale = 0.0f;
            freeze = true;
            active = false;
        }
    }
}
