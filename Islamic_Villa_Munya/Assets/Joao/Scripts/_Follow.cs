using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Follow : MonoBehaviour
{
    private bool freeze = false;
    [SerializeField] GameObject Controls1;
    [SerializeField] GameObject Controls2;
    [SerializeField] GameObject Collider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Controls1.SetActive(false);
            Controls2.SetActive(true);
            Collider.SetActive(false);
            //StartCoroutine(Execute());
            freeze = true;
        }
    }

    /*IEnumerator Execute()
    {
        yield return new WaitForSeconds(10);
        Controls2.SetActive(false);

    }*/
}
