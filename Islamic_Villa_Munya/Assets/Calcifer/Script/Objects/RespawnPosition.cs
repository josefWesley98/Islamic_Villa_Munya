using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPosition : MonoBehaviour
{
    [SerializeField] private Transform obj;
    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.GetHaveKey())
        {
            transform.position = obj.transform.position;
        }
    }
}
