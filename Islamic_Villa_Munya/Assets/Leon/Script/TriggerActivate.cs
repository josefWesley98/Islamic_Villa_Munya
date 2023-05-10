using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActivate : MonoBehaviour
{
    public string sendMethodName = "";

    public GameObject targetObject;

    public bool doOnceOnly = true;
    bool done = false;

    void OnTriggerEnter(Collider c)
    {
        if (doOnceOnly && done)
            return;

        if (c.tag == "Player" && !c.isTrigger)
        {
            targetObject.SendMessage(sendMethodName);
            done = true;
        }
    }
}
