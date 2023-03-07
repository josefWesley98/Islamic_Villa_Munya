using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TestingAudio : MonoBehaviour
{
    [SerializeField] SpawnAudioPrefabs SpawnAudio;
    // Get AudioManager for audio spawning

    
    void Start()
    {
        SpawnAudio.spawnAudioPrefab(0, 0,true);  //Spawn audio prefab (prefab is 3D in this case) as a child of spawn location BECAUSE of TRUE
        SpawnAudio.spawnAudioPrefab(1, 0);        //Spawn audio prefab (prefab is 2D in this case) and NO TRUE cause it is a 2D sound (ambient sound)

    }

}
