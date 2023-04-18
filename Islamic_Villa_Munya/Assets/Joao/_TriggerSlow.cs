using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TriggerSlow : MonoBehaviour
{
    private bool freeze = false;
    [SerializeField] GameObject Notification1;
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
            Notification1.SetActive(false);
            Debug.Log("Destroy");
            Time.timeScale = 1f;
            freeze = false;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !GameManager.GetArtefactCollected())
        {
            //Notification.SetActive(false);
            Notification1.SetActive(true);
            Time.timeScale = 0.0f;
            freeze = true;

        }
    }
}
