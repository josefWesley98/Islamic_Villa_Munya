using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cal's script starts here*/
public class PushOrPull : MonoBehaviour
{
    private bool is_parented = false;
    private Rigidbody rb;
    //Setter for the transform to be a child if the player is pushing or not
    public void Push(bool pop, GameObject obj)
    {
        if (pop)
        {
            transform.parent = obj.transform;
            rb = GetComponent<Rigidbody>();
            Destroy(rb);
            Debug.Log("Destroy");
            is_parented = true;
        }
        else if (!pop)
        {
            Debug.Log("Create");

            rb = gameObject.AddComponent<Rigidbody>();
            transform.parent = null;
            //transform.eulerAngles = new Vector3(0f, 0f, 0f);
            is_parented = false;
        }
    }

    public bool GetIsParented()
    {
        return is_parented;
    }
}

/*Cal's script ends here*/
