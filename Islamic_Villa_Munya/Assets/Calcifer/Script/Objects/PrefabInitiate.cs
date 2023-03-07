using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cal's script starts here*/
public class PrefabInitiate : MonoBehaviour
{
    private bool instantiate = false;

    public GameObject artefact;

    // Update is called once per frame
    void Update()
    {
        if(GameManager.GetArtefactCollected())
        {
            artefact.SetActive(true);
            //Instantiate the object at desired location with rotation

            //Set the instantiation to false
            //GameManager.SetArtefactCollected(false);
        }
    }


}

/*Cal's script ends here*/