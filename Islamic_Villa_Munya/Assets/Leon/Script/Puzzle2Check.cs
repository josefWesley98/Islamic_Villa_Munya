using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle2Check : MonoBehaviour
{
    void Update()
    {
        if (GameManager.GetArtefactCollected())
        {
            Destroy(gameObject);
        }
    }
}
