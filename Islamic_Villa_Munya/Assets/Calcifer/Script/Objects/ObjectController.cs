using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cal's code starts here*/
public class ObjectController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float mass; //Decide how heavy any object is
    // [SerializeField] private float break_force = 0f;
    // [SerializeField] private float break_torque = 0f;
    private bool is_created = false;
    [SerializeField] private float const_force = 0f;
    private bool grounded = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void FixedUpdate() 
    {
        //Always have a slight force pressing down on the object.
        //Set this to always be enough force to create torque and so break the joint
        //if the object is being moved over an edge.
        // Vector3 force = Vector3.down * const_force * Time.deltaTime;
        // rb.AddForce(force, ForceMode.Impulse);

        //Check for the ground underneath. If there isn't one then break the joint
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 1f))
        {
            Debug.DrawRay(transform.position, Vector3.down, Color.cyan);
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    private void OnCollisionStay(Collision other) 
    {
        if(other.gameObject.tag == "Player")
        {
            //If not grounded then call player class and set to not pushing
            if(!grounded)
            {
                //The joint will be destroyed.
                other.gameObject.GetComponent<NIThirdPersonController>().setPushing(false);
            }   
        }
    }

    // Update is called once per frame
    public void ToggleJoint(bool should_attach, Rigidbody player_rb)
    {
        if(should_attach && !is_created)
        {
            is_created = true;
            //Create a fixed joint
            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.breakForce = Mathf.Infinity;
            joint.breakTorque = Mathf.Infinity;
            joint.connectedBody = player_rb;
            rb.mass = mass;
        }
        else if (!should_attach && is_created)
        {
            is_created = false;
            //Remove the fixed joint
            FixedJoint joint = GetComponent<FixedJoint>();
            Destroy(joint);
            rb.mass = 1000f;

            if(!grounded)
            {
                
            }
            //rb.constraints &= ~RigidbodyConstraints.FreezeRotation;
        }
    }

    public void ResetPosition(Transform new_pos)
    {
        gameObject.transform.position = new_pos.position;
    }
}
/*Cal's code ends here*/
