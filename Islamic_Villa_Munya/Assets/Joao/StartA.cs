using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartA : MonoBehaviour
{
    public SpawnAudioPrefabs SpawnAudio;
    // Start is called before the first frame update
    void Start()
    {
        SpawnAudio.spawnAudioPrefab(0, true);
        SpawnAudio.spawnAudioPrefab(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
