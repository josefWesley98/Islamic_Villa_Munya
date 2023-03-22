using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //[SerializeField] SpawnAudioPrefabs SpawnAudio;
    // Get AudioManager for audio spawning
    [SerializeField] private AudioMixer Mixer;
    [SerializeField] private AudioSource AudioSource;


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

    public void SetLevel(float sliderValue)
    {
        Mixer.SetFloat("Volume", Mathf.Log10(sliderValue) * 20);
    }
}
