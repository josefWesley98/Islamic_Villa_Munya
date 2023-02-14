using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAudioPrefabs : MonoBehaviour
{
    [SerializeField] private Transform[] spawnLocations;
    [SerializeField] private GameObject[] WhatToSpawn;
    //[SerializeField] private GameObject[] WhatToSpawnClone; // instantiated objects

    // Start is called before the first frame update
    void Start()
    {

    }

    public void spawnAudioPrefab(int Num, bool attachTo = false)
    {
        WhatToSpawn[Num] = Instantiate(WhatToSpawn[Num], spawnLocations[Num].transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;

        if (attachTo)
        {
            WhatToSpawn[Num].transform.parent = spawnLocations[Num].transform;
        }
    }
}