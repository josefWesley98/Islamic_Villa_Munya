using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cal's script starts here*/
public class SparkleID : MonoBehaviour
{
    [SerializeField] GameObject artefact_ref;
    private int sparkle_id;
    private bool do_until = true;

    // Start is called before the first frame update
    void Start()
    {
        sparkle_id = artefact_ref.GetComponent<PlaceArtefact>().artefactID();
    }

    // Check if the artefact has been placed and is collected then set to inactive
    void Update()
    {
        if (GameManager.GetArtefactCollected(sparkle_id) && GameManager.GetArtefactPlaced(sparkle_id))
        {
            gameObject.SetActive(false);
        }
    }
}
/*Cal's script ends here*/
