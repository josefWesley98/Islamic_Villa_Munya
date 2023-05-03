using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cal's code starts here*/
public class SparkleController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < gameObject.transform.childCount; i++)
        {
            if(GameManager.GetArtefactCollected(i))
            {
                Debug.Log("Collected");
                gameObject.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}
/*Cal's code ends here*/
