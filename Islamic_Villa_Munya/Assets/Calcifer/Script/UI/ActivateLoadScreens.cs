using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cal's script starts here*/
public class ActivateLoadScreens : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(1).gameObject.SetActive(true);
    }
}
/*Cal's script ends*/
