using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleCheck : MonoBehaviour
{
    [SerializeField] private MeshRenderer renderer;

    private void OnTriggerExit(Collider other)
    {
        if(!other.gameObject.GetComponent<PlayerInteract>())
        {
            if(!renderer.enabled)
            {
                renderer.enabled = true;
                Debug.Log("happening boop boop boop.");
            }
        }
    }
    
}
