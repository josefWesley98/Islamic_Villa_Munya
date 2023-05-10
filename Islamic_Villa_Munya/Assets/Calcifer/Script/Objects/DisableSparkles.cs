using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cal's script starts here*/
public class DisableSparkles : MonoBehaviour
{
    [SerializeField] private GameObject sparkles;
    [SerializeField] private int id;
    private bool do_until = true;
    private bool is_placed = false;

    // Start is called before the first frame update
    void Start()
    {
        sparkles.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(do_until && !GameManager.GetArtefactPlaced(id))
        {
            if(GameManager.GetArtefactCollected(id))
            {
                sparkles.SetActive(true);
                do_until = false;
            }

        }

        if(!is_placed && !do_until)
        {
            if(GameManager.GetArtefactPlaced(id))
            {
                sparkles.SetActive(false);
                is_placed = true;
            }
        }
    }
}

/*Cal's script ends here*/
