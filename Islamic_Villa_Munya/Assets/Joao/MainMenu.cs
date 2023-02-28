using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //[SerializeField] SpawnAudioPrefabs SpawnAudio;
    // Get AudioManager for audio spawning


    void Start()
    {
        //SpawnAudio.spawnAudioPrefab(0);        //Spawn audio prefab (prefab is 2D in this case) and NO TRUE cause it is a 2D sound (ambient sound)

    }
    // Start is called before the first frame update
    public void PlayGame()
    {
        SceneManager.LoadScene("PlayerTesting");
    }

    public void PlayGame2()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
