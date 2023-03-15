using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Insect : MonoBehaviour
{
    public float dirForceMult = 2;

    public float changeDirTimeMin = 1f;
    public float changeDirTimeMax = 4f;
    public float landedTimeMin = 5f;
    public float landedTimeMax = 12f;
    public float landCooldownTime = 5f;
    bool canLand = false;

    float flightTimer = 0f;
    float switchFlightTime;
    float stateTimer = 0f;
    float switchStateTime;

    Rigidbody rb;
    Collider col;
    Animator anim;

    GameObject lastObjectLandedOn;

    public LayerMask landLayers;

    public Collider pointOfInterest;
    bool visitedPOI = false;
    float minVisitedDistance = 1f;
    int poiCounter = 0;
    public int poiCountAmount = 10;
    float forgetPOITimer = 0f;
    public float poiMemory = 10f;

    Transform rotHelper;
    float rotSpeed = 0.002f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        rotHelper = transform.Find("RotHelper");
        rotHelper.parent = null;
        rotHelper.gameObject.hideFlags = HideFlags.HideInHierarchy;

        if (pointOfInterest == null)
            visitedPOI = true;

        TakeOff();
    }

    enum ButterflyState
    {
        Flying,
        Landed,
        VisitingPOI,
    }

    ButterflyState state = ButterflyState.Flying;

    void FixedUpdate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, rotHelper.rotation, Time.time * rotSpeed);

        switch (state)
        {
            case ButterflyState.Flying:

                if(!visitedPOI)
                    ChangeState(ButterflyState.VisitingPOI);
                else if (pointOfInterest != null)
                {
                    forgetPOITimer += Time.deltaTime;
                    if(forgetPOITimer > poiMemory)
                        visitedPOI = false;
                    print("reset timer : " + forgetPOITimer);
                }

                stateTimer += Time.deltaTime;

                if (stateTimer > landCooldownTime)
                    canLand = true;

                flightTimer += Time.deltaTime;

                if (flightTimer > switchFlightTime)
                {
                    rb.AddForce(Random.onUnitSphere * dirForceMult);
                    switchFlightTime = Random.Range(changeDirTimeMin, changeDirTimeMax);
                    flightTimer = 0f;
                }

                break;
            case ButterflyState.Landed:

                stateTimer += Time.deltaTime;

                if (stateTimer > switchStateTime)
                {
                    TakeOff();
                    ChangeState(ButterflyState.Flying);
                }

                break;
            case ButterflyState.VisitingPOI:

                stateTimer += Time.deltaTime;

                if (stateTimer > landCooldownTime)
                    canLand = true;

                flightTimer += Time.deltaTime;

                if (flightTimer > switchFlightTime)
                {
                    rb.AddForce(Random.onUnitSphere * dirForceMult);

                    if (!visitedPOI)
                        rb.AddForce((pointOfInterest.transform.position - transform.position).normalized * dirForceMult);

                    switchFlightTime = Random.Range(changeDirTimeMin, changeDirTimeMax);
                    flightTimer = 0f;

                    if (Vector3.Distance(pointOfInterest.transform.position, transform.position) < minVisitedDistance)
                    {
                        poiCounter++;
                        //visitedPOI = true;
                        print("visited poi! + " + poiCounter);            
                    }

                    if(poiCounter > poiCountAmount)
                    {
                        poiCounter = 0;
                        visitedPOI = true;
                        ChangeState(ButterflyState.Flying);
                    }
                }

                break;

                //rb.AddTorque(Vector3.up * Random.Range(-0.01f, 0.01f));
        }

        //if (visitedPOIAlready)
        //{
        //    forgetPOITimer += Time.deltaTime;
        //    if (forgetPOITimer > memoryPOI)
        //    {
        //        forgetPOITimer = 0;
        //        visitedPOIAlready = false;
        //    }
        //}

        //transform.Rotate(new Vector3(0, 0, 1));
        //transform.rotation = Quaternion.LookRotation(pointOfInterest.transform.position - transform.position, Vector3.up);
    }

    void ChangeState(ButterflyState _state, float _minTimeInState = 0, float _maxTimeInState = 0)
    {
        stateTimer = 0;
        switchStateTime = Random.Range(_minTimeInState, _maxTimeInState);
        state = _state;
    }

    private void OnCollisionEnter(Collision c)
    {
        //if we cant land on this layer or the previous landing was on this object, return
        if (!IsInLayerMask(c.gameObject, landLayers) || lastObjectLandedOn == c.gameObject || !canLand)
            return;

        ChangeState(ButterflyState.Landed, landedTimeMin, landedTimeMax);
        Land(c);
    }

    void Land(Collision c)
    {
        //if (c.collider == pointOfInterest)
        //{
        //    if (visitedPOIAlready)
        //        return;
        //    else
        //        visitedPOIAlready = true;
        //}
        canLand = false;
        lastObjectLandedOn = c.gameObject;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.None;
        col.enabled = false;
        transform.parent = c.transform;
        transform.position += c.contacts[0].normal * -0.05f;
        rotHelper.rotation = Quaternion.LookRotation(c.contacts[0].point - transform.position, Vector3.up);
        rotHelper.Rotate(new Vector3(0, 0, Random.Range(-180f, 180f)));
        anim.SetBool("grounded", true);
        switchStateTime = Random.Range(landedTimeMin, landedTimeMax);
    }

    void TakeOff()
    {
        transform.parent = null;
        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        col.enabled = true;
        rotHelper.rotation = Quaternion.identity;
        rotHelper.Rotate(new Vector3(50, Random.Range(-180f, 180f), 0));
        anim.SetBool("grounded", false);
    }

    public bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return ((layerMask.value & (1 << obj.layer)) > 0);
    }
}
