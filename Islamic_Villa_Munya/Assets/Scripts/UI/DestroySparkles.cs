using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySparkles : MonoBehaviour
{
    [SerializeField] private GameObject[] sparkles;

    void Update()
    {
        if(GameManager.GetHaveKey())
        {
            foreach(GameObject sparkle in sparkles)
            {
                Destroy(sparkle);
            }
        }
    }
}
