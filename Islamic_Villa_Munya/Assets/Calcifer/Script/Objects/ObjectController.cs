using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cal's code starts here*/
public class ObjectController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float mass; //Decide how heavy any object is
    [SerializeField] private float break_force = 0f;
    [SerializeField] private float break_torque = 0f;
    private bool is_created = false;
    [SerializeField] private float const_force = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void FixedUpdate() 
    {
        //Always have a slight force pressing down on the object.
        //Set this to always be enough force to create torque and so break the joint
        //if the object is being moved over an edge.
        Vector3 force = Vector3.down * const_force * Time.deltaTime;
        rb.AddForce(force, ForceMode.Impulse);
    }

    // Update is called once per frame
    public void ToggleJoint(bool should_attach, Rigidbody player_rb)
    {
        if(should_attach && !is_created)
        {
            is_created = true;
            //Create a fixed joint
            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.breakForce = break_force;
            joint.breakTorque = break_torque;
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
            rb.constraints &= ~RigidbodyConstraints.FreezeRotation;
        }
    }
}
/*Cal's code ends here*/
