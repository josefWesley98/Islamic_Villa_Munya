using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Insect : MonoBehaviour
{
    public float changeDirTimeMin = 1f;
    public float changeDirTimeMax = 4f;
    public float landedTimeMin = 5f;
    public float landedTimeMax = 12f;
    public float dirMult = 2;

    float switchDirectionTime;
    public float switchTimer = 0f;

    Rigidbody rb;
    Collider col;
    Animator anim;

    public bool flying = true;
    public LayerMask landLayers;
    public GameObject lastObjectLandedOn;

    public Collider pointOfInterest;
    public bool visitedPOIAlready = false;
    public float forgetPOITimer = 0f;
    public float memoryPOI = 10f;

    public float landCooldown = 5f;
    float canLandTimer = 0f;
    bool canLand = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        switchTimer += Time.deltaTime;

        if (switchTimer > switchDirectionTime)
        {
            if (flying)
            {
                rb.AddForce(Random.onUnitSphere * dirMult);
                rb.AddTorque(Vector3.up * Random.Range(-0.01f, 0.01f));

                if(pointOfInterest != null && !visitedPOIAlready)
                    //rb.AddForce((pointOfInterest.transform.position - transform.position).normalized * dirMult);

                switchDirectionTime = Random.Range(changeDirTimeMin, changeDirTimeMax);
                switchTimer = 0;
            }
            else
            {
                //TakeOff();
            }
        }

        if (visitedPOIAlready)
        {
            forgetPOITimer += Time.deltaTime;
            if (forgetPOITimer > memoryPOI)
            {
                forgetPOITimer = 0;
                visitedPOIAlready = false;
            }
        }
        //if(!canLand)
        //    canLandTimer += Time.deltaTime;
        //if (canLandTimer > landCooldown)
        //    canLand = true;

        //transform.Rotate(new Vector3(0, 0, 1));
        //transform.rotation = Quaternion.LookRotation(pointOfInterest.transform.position - transform.position, Vector3.up);
    }

    private void OnCollisionEnter(Collision c)
    {
        //if (!canLand)
        //    return;
        //if we can land on this layer and the previous landing wasnt on this object
        print(lastObjectLandedOn == c.gameObject);
        if (!IsInLayerMask(c.gameObject, landLayers) || lastObjectLandedOn == c.gameObject)
            return;

        Land(c);
    }

    void Land(Collision c)
    {
        if (!flying)
            return;
        if (c.collider == pointOfInterest)
        {
            if (visitedPOIAlready)
                return;
            else
                visitedPOIAlready = true;
        }
           
        lastObjectLandedOn = c.gameObject;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.None;
        col.enabled = false;
        transform.parent = c.transform;
        //transform.position += c.contacts[0].normal * -0.03f;
        transform.rotation = Quaternion.LookRotation(c.contacts[0].normal - transform.position, Vector3.up);
        //transform.Rotate(new Vector3(0, 0, Random.Range(-180f, 180f)));
        anim.SetBool("grounded", true);
        switchTimer = 0;

        switchDirectionTime = Random.Range(landedTimeMin, landedTimeMax);
        flying = false;
    }

    void TakeOff()
    {
        if (flying)
            return;
        transform.parent = null;
        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        col.enabled = true;
        transform.rotation = Quaternion.identity;
        transform.Rotate(new Vector3(90, 0, 0));
        anim.SetBool("grounded", false);
        switchTimer = 999;
        canLand = false;
        flying = true;
    }

    public bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return ((layerMask.value & (1 << obj.layer)) > 0);
    }
}
