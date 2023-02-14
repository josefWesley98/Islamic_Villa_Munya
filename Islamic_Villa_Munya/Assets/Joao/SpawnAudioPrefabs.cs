using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAudioPrefabs : MonoBehaviour
{
    [SerializeField] private Transform[] spawnLocations;
    [SerializeField] private GameObject[] WhatToSpawn;


    public void spawnAudioPrefab(int Num, bool attachTo = false)
    {
        WhatToSpawn[Num] = Instantiate(WhatToSpawn[Num], spawnLocations[Num].transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        // spawns prefabs number

        if (attachTo)
        {
            WhatToSpawn[Num].transform.parent = spawnLocations[Num].transform;
            // If TRUE, spawns object as child of spawn location
        }
    }
}
