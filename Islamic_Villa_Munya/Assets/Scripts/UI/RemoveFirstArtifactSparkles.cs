using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveFirstArtifactSparkles : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject[] sparkles;
   
    // Update is called once per frame
    void Update()
    {
        // destroys all the sparkles if they key is held.
        for(int i = 0; i < sparkles.Length; i++)
        {
            if(sparkles[i] != null)
            {
                if(GameManager.GetHaveKey())
                {
                    Destroy(sparkles[i]);
                }
            }
        }
    }
}
