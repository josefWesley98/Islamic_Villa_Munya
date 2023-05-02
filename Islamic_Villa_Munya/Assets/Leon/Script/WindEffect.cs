using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEffect : MonoBehaviour
{
   public float forceTimeMin = 0.1f;
   public float forceTimeMax = 0.5f;
    public float forceAmountMin = 10f;

    public float forceAmountMax = 30f;

    float timer = 0.0f;

    bool doWind = false;

    Rigidbody rb;

    void Start()
    {
        Invoke(nameof(SetUp), 2f);
    }

    void SetUp()
    {
        rb = GetComponent<Rigidbody>();
        doWind = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!doWind)
            return;

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            rb.AddForce(Vector3.back * Random.Range(forceAmountMin, forceAmountMax) * 0.333f);

            ResetRandomTimer();
        }
    }

    void ResetRandomTimer()
    {
        timer = Random.Range(forceTimeMin, forceTimeMax);
    }
}
