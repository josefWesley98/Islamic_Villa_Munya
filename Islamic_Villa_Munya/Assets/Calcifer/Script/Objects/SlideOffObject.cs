using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cal's code begins here*/
public class SlideOffObject : MonoBehaviour
{
    [Header("Slide Off Object Properties")]
    [SerializeField] private string slide_tag = "Player";
    [SerializeField] private float slideForce;
    [SerializeField] private float Force_X;
    [SerializeField] private float Force_Y;
    [SerializeField] private float Force_Z;

    //This script will add a modifiable mechanic to any object the player is not to climb or stand on etc
    //Depending on the object orientation, these values will need to be altered to push the player back into the playable area.
    //This is a basic example of how we can prevent specific exploits from the player.

    //Best used for pushing player off in any desired direction from an object we don't want the player on.
    private void OnCollisionStay(Collision other) 
    {
        if(other.gameObject.tag == slide_tag)
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

            Vector3 slide_vec = new Vector3(Force_X, Force_Y, Force_Z) * slideForce;

            rb.AddForce(slide_vec, ForceMode.Impulse);
        }
    }
}

/*Cal's code ends here*/
