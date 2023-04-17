using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TriggerSlow : MonoBehaviour
{
    private bool freeze = false;
    [SerializeField] GameObject Notification1;
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
            Collider.SetActive(false);
            Notification1.SetActive(false);
            Time.timeScale = 1f;
            freeze = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Notification1.SetActive(true);
            Time.timeScale = 0.001f;
            freeze = true;
        }
    }
}
