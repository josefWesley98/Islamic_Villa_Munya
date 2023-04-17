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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
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
