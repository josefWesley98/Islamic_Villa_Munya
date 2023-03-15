using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cal's script starts here*/
public class PushOrPull : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Setter for the transform to be a child if the player is pushing or not
    public void Push(bool pop, GameObject obj)
    {
        Debug.Log("In the object");
        if(pop)
        {
            transform.parent = obj.transform;
        }
        else if(!pop)
        {
            transform.parent = null;
        }
    }
}

/*Cal's script ends here*/
