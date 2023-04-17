using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    //[SerializeField] SpawnAudioPrefabs SpawnAudio;
    // Get AudioManager for audio spawning
    [SerializeField] private AudioMixer Mixer;
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private GameObject DifficultyMenu;
    //
    public bool BOY = false;
    public bool GIRL = false;
    //
    public bool Easy = false;
    public bool Medium = false;
    public bool Hard = false;



    public void PlayGame()                          // when playgame is called, it switches scene to "PlayerTesting"
    {
        SceneManager.LoadScene("IraklisScene");  
    }

    public void PlayGame2()                         // when playgame2 is called, it switches scene to "SampleScene" which is the ai testing
    {
        SceneManager.LoadScene("SampleScene");
    }
    //----//

    public void DifficultyBoY()                          // when boy is selected, a bool for boy is saved"
    {
        BOY = true;
        GIRL = false;
        DifficultyMenu.SetActive(true);
    }

    public void DifficultyGirl()                          // when girl is selected, a bool for girl is saved"
    {
        BOY = false;
        GIRL = true;
        DifficultyMenu.SetActive(true);
    }

    //----//
    public void EASY()                          // when easy is pressed, save bool of easy"
    {
        Easy = true;
        Medium = false;
        Hard = false;
        SceneManager.LoadScene("IraklisScene");
    }

    public void MEDIUM()                          // when medium is pressed, save bool of medium"
    {
        Easy = false;
        Medium = true;
        Hard = false;
        SceneManager.LoadScene("IraklisScene");
    }

    public void HARD()                          // when hard is pressed, save bool of hard"
    {
        Easy = false;
        Medium = false;
        Hard = true;
        SceneManager.LoadScene("IraklisScene");
    }

    /// 
    ///

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
