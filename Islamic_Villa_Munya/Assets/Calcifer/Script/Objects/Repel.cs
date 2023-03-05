using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cal's code begins here*/
public class Repel : MonoBehaviour
{
    [Header("Repel Objects Properties")]
    [SerializeField] private float repel_force;

    //This script will add a modifiable mechanic to any object so the player doesn't get stuck in the wall etc.
    //This is a basic method and most likely temporary. Should be useful for now though.
    //Will be implementing some further logic here to stop issues.
    //Best used for pushing player away from walls.
    private void OnCollisionStay(Collision other) 
    {
        Vector3 dir = (transform.position - other.transform.position).normalized;

        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        
        rb.AddForce(dir * (-repel_force), ForceMode.VelocityChange);
    }
}

/*Cal's code ends here*/
