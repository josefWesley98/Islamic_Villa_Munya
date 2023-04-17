using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TriggerSlow : MonoBehaviour
{
    private bool freeze = false;
    [SerializeField] GameObject Notification1;
    [SerializeField] GameObject Notification;
    [SerializeField] GameObject Collider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space") && freeze == true)
        {
            Debug.Log("pRESSED");
            Notification1.SetActive(false);
            Time.timeScale = 1f;
            freeze = false;
            Collider.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Notification.SetActive(false);
            Notification1.SetActive(true);
            Time.timeScale = 0.0f;
            freeze = true;
            
        }
    }
}
