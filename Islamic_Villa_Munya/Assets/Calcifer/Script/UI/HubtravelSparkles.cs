using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cal's script starts here*/
public class HubtravelSparkles : MonoBehaviour
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
            if(GameManager.GetArtefactCollected(0))
            {
                gameObject.SetActive(true);
                do_until = false;
            }
        }
    }
}
/*Cal's script ends here*/
