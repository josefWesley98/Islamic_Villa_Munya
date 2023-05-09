using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _HowTo : MonoBehaviour
{
    Collider col;
    [SerializeField] GameObject HowTo;

    private void Start() 
    {
        col.GetComponent<Collider>();    
    }

    //Modified by Cal
    private void OnTriggerEnter(Collider other)
    {
        //Make sure that the message pops once so check if the first artefact has been collected. If it has then don't show message
        if (other.gameObject.tag == "Player" && !GameManager.GetArtefactCollected(0))
        {
            HowTo.SetActive(true);
            StartCoroutine(Execute());
        }
    }

    IEnumerator Execute()
    {
        yield return new WaitForSeconds(7);
        Destroy(HowTo);
    }
}
