using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cal's script starts here*/
public class KeyScript : MonoBehaviour
{
    //Destroy the key object
    private void OnTriggerEnter(Collider other) 
    {
        GameManager.SetHasKey(true);
        
        if(other.gameObject.tag == "Player")
        {
            Debug.Log(GameManager.GetHaveKey());
            Destroy(gameObject);
        }
    }
}
/*Cal's script ends here*/
