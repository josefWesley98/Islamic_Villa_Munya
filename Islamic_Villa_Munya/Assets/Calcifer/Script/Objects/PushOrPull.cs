using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cal's script starts here*/
public class PushOrPull : MonoBehaviour
{
    private bool is_parented = false;
    private Rigidbody rb;
    private bool destroy_rb = false;

    //Setter for the transform to be a child if the player is pushing or not
    public void Push(bool pop, GameObject obj)
    {
        
        if (pop)
        {
            if(!destroy_rb)
            {
                destroyRB();
                destroy_rb = true;
                Debug.Log("destroyed rb");
            }

            //rb.rotation = Quaternion.identity;
            is_parented = true;
            transform.parent = obj.transform;
           // rb.mass = 10f;
        }
        else if (!pop)
        {
           // rb.rotation = Quaternion.identity;
            is_parented = false;
            transform.parent = null;
           // rb.mass = 1000f;

            if(destroy_rb)
            {
                addRigidBody();
                destroy_rb = false;
                Debug.Log("created rb");
            }
        }
    }

    public bool GetIsParented()
    {
        return is_parented;
    }

    private void addRigidBody()
    {
        rb = gameObject.AddComponent<Rigidbody>();
        rb.mass = 1000f;
        gameObject.GetComponent<Rigidbody>();
    }

    //Collision function that unparents the object if not touching the ground
    // private void OnCollisionExit(Collision other) 
    // {
    //     if(other.gameObject.tag == "Player")
    //     {
    //         is_parented = false;
    //     }
    // }

    private void destroyRB()
    {
        Destroy(gameObject.GetComponent<Rigidbody>());
    }
}

/*Cal's script ends here*/
