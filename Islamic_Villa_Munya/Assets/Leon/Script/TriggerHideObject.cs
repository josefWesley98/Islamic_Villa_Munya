using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHideObject : MonoBehaviour
{
    public GameObject objectToHide;

    private void OnTriggerEnter(Collider c)
    {
        if(c.gameObject.tag == "Player")
            objectToHide.SetActive(false);
    }
    private void OnTriggerExit(Collider c)
    {
        if (c.gameObject.tag == "Player")
            objectToHide.SetActive(true);
    }
}
