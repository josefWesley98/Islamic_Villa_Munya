using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothSoundTest : MonoBehaviour
{
    Cloth c;
    void Start()
    {
        c = GetComponent<Cloth>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //print("wahoo");
    }
}
