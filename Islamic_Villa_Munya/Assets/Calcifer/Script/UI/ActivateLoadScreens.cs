using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cal's script starts here*/
public class ActivateLoadScreens : MonoBehaviour
{
    [SerializeField] private GameObject loading_ref;

    // Start is called before the first frame update
    void Start()
    {
        loading_ref.SetActive(true);
    }
}
/*Cal's script ends*/
