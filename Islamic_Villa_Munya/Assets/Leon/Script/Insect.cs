using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Insect : MonoBehaviour
{
    //public float hoverRadius = 4f;
    public float changeTargetTimeMin = 1f;
    public float changeTargetTimeMax = 4f;
    //public float moveSpeedMult = 1f;
    float changeTargetTime;

    float time = 0f;
    Vector3 targetPos = Vector3.zero;

    Transform insect;

    //public float rotationSpeed;
    public float forceMult = 100;
    //values for internal use
    private Quaternion _lookRotation;
    private Vector3 _direction;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //insect = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if(time > changeTargetTime)
        {
            rb.AddForce(Random.onUnitSphere * forceMult);
            rb.AddTorque(Vector3.up * Random.Range(-0.1f, 0.1f));
            //time = 0f;

            //targetPos = transform.position + Random.insideUnitSphere * hoverRadius;

            changeTargetTime = Random.Range(changeTargetTimeMin, changeTargetTimeMax);
        }
        rb.velocity *= 0.999f;
        rb.angularVelocity *= 0.99f;
        //insect.transform.position =  Vector3.Lerp(insect.transform.position, targetPos, Time.deltaTime * moveSpeedMult);


        //find the vector pointing from our position to the target
        //_direction = (targetPos - insect.transform.position).normalized;

        //create the rotation we need to be in to look at the target
        //_lookRotation = Quaternion.LookRotation(_direction);

        //rotate us over time according to speed until we are in the required rotation
        //insect.transform.rotation = Quaternion.Slerp(insect.transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, hoverRadius);
    }
}
