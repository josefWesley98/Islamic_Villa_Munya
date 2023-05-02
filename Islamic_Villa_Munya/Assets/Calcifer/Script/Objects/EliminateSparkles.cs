using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliminateSparkles : MonoBehaviour
{
    private bool do_until = true;
    [SerializeField] private int artefact_id;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(do_until)
        {
            if(GameManager.GetArtefactCollected(artefact_id) && GameManager.GetArtefactPlaced(artefact_id))
            {
                Destroy(gameObject);
                do_until = false;
            }
        }
    }
}
