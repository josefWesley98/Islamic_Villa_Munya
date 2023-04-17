using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRBActive : MonoBehaviour
{
    public Rigidbody rigidbodyToSetKinematic;

    void Start()
    {
        rigidbodyToSetKinematic.isKinematic = false;
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag != "Player")
            return;

        print(c.gameObject.name + " ENTER");

        rigidbodyToSetKinematic.isKinematic = true;
    }

    private void OnTriggerExit(Collider c)
    {
        if (c.gameObject.tag != "Player")
            return;

        print(c.gameObject.name + " EXIT");

        rigidbodyToSetKinematic.isKinematic = false;
    }
}
