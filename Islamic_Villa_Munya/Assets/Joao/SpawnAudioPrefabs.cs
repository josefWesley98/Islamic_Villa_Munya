using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAudioPrefabs : MonoBehaviour
{
    [SerializeField] private Transform[] spawnLocations;
    [SerializeField] private GameObject[] WhatToSpawn;


    public void spawnAudioPrefab(int Num, bool attachTo = false)
    {
        WhatToSpawn[0] = Instantiate(WhatToSpawn[0], spawnLocations[Num].transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        // spawns prefabs number

        if (attachTo)
        {
            WhatToSpawn[0].transform.parent = spawnLocations[Num].transform;
            // If TRUE, spawns object as child of spawn location
        }
    }
}
