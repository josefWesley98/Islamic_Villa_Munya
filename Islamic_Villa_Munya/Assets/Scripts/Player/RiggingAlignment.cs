using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiggingAlignment : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform CurrentPosition;
    [SerializeField] private Transform TargetPosition;
    void Start()
    {
        CurrentPosition.position = TargetPosition.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
