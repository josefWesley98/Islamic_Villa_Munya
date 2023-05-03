using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //[SerializeField] SpawnAudioPrefabs SpawnAudio;
    // Get AudioManager for audio spawning
    [SerializeField] private AudioMixer Mixer;
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private GameObject DifficultyMenu;
    [SerializeField] private Slider slider;
    [SerializeField] private Slider slider2;

    float volumeMusic = 0.5f;

    float volumeSFX = 1.0f;
    //
    public bool BOY = false;
    public bool GIRL = false;
    //
    public bool Easy = false;
    public bool Medium = false;
    public bool Hard = false;

    void Start()
    {
        AudioSource = AudioSource.GetComponent<AudioSource>();
        //AudioSource.volume = PlayerPrefs.GetFloat("SliderVolumeLevel", volume);
        slider.value = PlayerPrefs.GetFloat("MusicParam", AudioSource.volume);
        AudioSource.volume = PlayerPrefs.GetFloat("MusicParam", AudioSource.volume);
        //print("Got volume from player prefs " + volume);
        slider2.value = PlayerPrefs.GetFloat("SFXParam", volumeSFX);
        volumeSFX = PlayerPrefs.GetFloat("SFXParam", volumeSFX);
    }

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
        GameManager.SetEasy(Easy);
        GameManager.SetMedium(Medium);
        GameManager.SetHard(Hard);
        SceneManager.LoadScene("IraklisScene");
    }

    public void MEDIUM()                          // when medium is pressed, save bool of medium"
    {
        Easy = false;
        Medium = true;
        Hard = false;
        GameManager.SetEasy(Easy);
        GameManager.SetMedium(Medium);
        GameManager.SetHard(Hard);
        SceneManager.LoadScene("IraklisScene");
    }

    public void HARD()                          // when hard is pressed, save bool of hard"
    {
        Easy = false;
        Medium = false;
        Hard = true;
        GameManager.SetEasy(Easy);
        GameManager.SetMedium(Medium);
        GameManager.SetHard(Hard);
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
        //mixer volume change
        Mixer.SetFloat("MusicParam", Mathf.Log10(sliderValue) * 20);
        //change volume to new value set by slider
        volumeMusic = sliderValue;
        //saved to disk
        PlayerPrefs.SetFloat("MusicParam", volumeMusic);
        //print("Set volume player prefs to " +  volumeMusic);
    }

    public void SetLevelSFX(float slider2Value)
    {
        //mixer volume change
        Mixer.SetFloat("SFXParam", Mathf.Log10(slider2Value) * 20);
        //change volume to new value set by slider
        volumeSFX = slider2Value;
        //saved to disk
        PlayerPrefs.SetFloat("SFXParam", volumeSFX);
        //print("Set volume player prefs to " +  volumeSFX);
    }
}
