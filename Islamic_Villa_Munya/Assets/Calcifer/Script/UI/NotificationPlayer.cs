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

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && GameManager.GetArtefactCollected() && active)
        {
            Notification.SetActive(true);
            Time.timeScale = 0.0f;
            freeze = true;
            active = false;
        }
    }
}
