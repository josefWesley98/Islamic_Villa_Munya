using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //[SerializeField] SpawnAudioPrefabs SpawnAudio;
    // Get AudioManager for audio spawning

    public void PlayGame()                          // when playgame is called, it switches scene to "PlayerTesting"
    {
        SceneManager.LoadScene("IraklisScene");  
    }

    public void PlayGame2()                         // when playgame2 is called, it switches scene to "SampleScene" which is the ai testing
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void QuitGame()                           // when quitgame is called, it quits the application
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
