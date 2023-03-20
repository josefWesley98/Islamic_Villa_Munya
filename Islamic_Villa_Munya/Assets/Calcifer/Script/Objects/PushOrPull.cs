using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cal's script starts here*/
public class PushOrPull : MonoBehaviour
{

    //Setter for the transform to be a child if the player is pushing or not
    public void Push(bool pop, GameObject obj)
    {
        if(pop)
        {
            transform.parent = obj.transform;
        }
        else if(!pop)
        {
            transform.parent = null;
            transform.eulerAngles = new Vector3(0f,0f,0f);
        }
    }
}

/*Cal's script ends here*/
