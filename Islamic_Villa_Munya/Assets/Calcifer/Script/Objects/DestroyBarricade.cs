using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cal's script starts here*/
public class DestroyBarricade : MonoBehaviour
{
    private bool do_until = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(do_until)
        {
            //If the first artefact is collected then destroy barricades and allow player to be free
            if(GameManager.GetArtefactCollected(0))
            {
                gameObject.SetActive(false);
                do_until = false;
            }
        }
    }
}
/*Cal's script ends here*/
