using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cal's script starts here*/
public class Activate : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int artefact_id;
    private bool do_until = true;
    [SerializeField] private GameObject child_ref;

    void Start()
    {
        child_ref.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(do_until)
        {
            if(GameManager.GetArtefactCollected(0))
            {
                child_ref.SetActive(true);
            }
        }
    }
}
/*Cal's script ends here*/
