using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Modified by Cal
public class _TriggerSlow : MonoBehaviour
{
    private bool freeze = false;
    [SerializeField] GameObject Notification1;
    [SerializeField] GameObject Notification;
    private GameObject player_ref;
    private Collider col;
    Keyboard kb;
    private bool player_confirmed = false;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();

        //User input from keyboard
        kb = InputSystem.GetDevice<Keyboard>();
    }

    // Update is called once per frame
    void Update()
    {
        //Would rather 
        player_confirmed = kb.spaceKey.isPressed;

        if(player_confirmed && freeze == true)
        {
            //Reactivate player input

            Debug.Log("pRESSED");
            Notification1.SetActive(false);
            Debug.Log("Destroy");
            Time.timeScale = 1f;
            freeze = false;
            Destroy(gameObject);
            
            player_ref.GetComponent<NIThirdPersonController>().SetInput(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !GameManager.GetArtefactCollected(0))
        {
            //Deactivate player input
            player_ref = other.gameObject;
            
            player_ref.GetComponent<NIThirdPersonController>().SetInput(true);

            //Notification.SetActive(false);
            Notification1.SetActive(true);
            Time.timeScale = 0.0f;
            freeze = true;

        }
    }
}
