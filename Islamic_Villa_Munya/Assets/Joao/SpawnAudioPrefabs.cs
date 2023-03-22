using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAudioPrefabs : MonoBehaviour
{
    [SerializeField] private Transform[] spawnLocations;
    [SerializeField] private GameObject[] WhatToSpawn;
    [SerializeField] private GameObject[] WhatToSpawnClone;  // instantiated clone


    public void spawnAudioPrefab(int Num,int sound, bool attachTo = false)
    {
        WhatToSpawnClone[sound] = Instantiate(WhatToSpawn[sound], spawnLocations[Num].transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        // spawns prefabs number
        Debug.Log("Spawning audio at " + Num);

        if (attachTo)
        {
            WhatToSpawnClone[sound].transform.parent = spawnLocations[Num].transform;
            // If TRUE, spawns object as child of spawn location
        }
    }
}
