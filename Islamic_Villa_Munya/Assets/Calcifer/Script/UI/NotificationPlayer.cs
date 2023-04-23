using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Modifed by Cal*/
public class NotificationPlayer : MonoBehaviour
{
    private bool freeze = false;
    private bool active = true;
    [SerializeField] GameObject Notification;
    private Collider col;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space") && freeze == true)
        {
            Debug.Log("pRESSED");
            Notification.SetActive(false);
            Time.timeScale = 1f;
            freeze = false;
            Destroy(col);
        }
    }

    /*Modified by Cal*/
    private void OnTriggerExit(Collider other)
    {
        //Only want to show the 'You're free to explore' message once then no more after that so check if the second artefact has been found. If it has then never show the message 
        if (other.gameObject.tag == "Player" && GameManager.GetArtefactCollected(0) && !GameManager.GetArtefactCollected(1) && active)
        {
            Notification.SetActive(true);
            Time.timeScale = 0.0f;
            freeze = true;
            active = false;
        }
    }
}
