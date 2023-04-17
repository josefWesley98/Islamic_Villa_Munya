using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _HowTo : MonoBehaviour
{
    [SerializeField] GameObject Collider;
    [SerializeField] GameObject HowTo;

  

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
        yield return new WaitForSeconds(10);
        HowTo.SetActive(false);
        Collider.SetActive(false);
    }
}
