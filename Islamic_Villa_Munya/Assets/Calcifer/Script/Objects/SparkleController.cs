using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cal's code starts here*/
public class SparkleController : MonoBehaviour
{
    [SerializeField] private GameObject[] sparkle_arr;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 1; i < sparkle_arr.Length; i++)
        {
            if(GameManager.GetArtefactCollected(i))
            {
                Debug.Log("Collected");
                sparkle_arr[i].gameObject.SetActive(true);
            }
        }
    }
}
/*Cal's code ends here*/
