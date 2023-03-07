using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkleManager : MonoBehaviour
{
    [SerializeField] private GameObject sparkles;
    [SerializeField] private GameObject sparkles2;
    [SerializeField] private bool sparklesOneActive = true;
    [SerializeField] private bool sparklesTwoActive = false;
    [SerializeField] private bool leavingPuzzle = false;
    [SerializeField] private int id;
    // Start is called before the first frame update
  

    // Update is called once per frame
    void Update()
    {
        if(sparkles != null)
        {
            if(sparklesOneActive)
            {
                sparkles.gameObject.SetActive(true);
            }
            else
            {
                sparkles.gameObject.SetActive(false);
            }
        }
        if(GameManager.GetArtefactCollected() && leavingPuzzle)
        {
            sparklesOneActive = true;
            sparkles.gameObject.SetActive(true);
           
        }
        if(id == 10 && GameManager.GetArtefactCollected())
        {
            sparklesOneActive = false;
            sparkles.gameObject.SetActive(false);
        }
        // if(sparkles2 != null)
        // {
        //     if(sparklesTwoActive)
        //     {
        //         sparkles2.gameObject.SetActive(true);
        //     }
        //     else
        //     {
        //         sparkles2.gameObject.SetActive(false);
        //     }
        // }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(sparkles != null)
            {
                sparklesOneActive = false;
            }
            // if(sparkles2 != null)
            // {
            //     sparklesTwoActive = true;
            // }
        }
    }


}
