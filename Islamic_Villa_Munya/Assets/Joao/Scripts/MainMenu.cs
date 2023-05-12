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

    [Header("Volume settings")]
    // Access the audio mixer and audio source where sound is playing
    [SerializeField] private AudioMixer Mixer;
    [SerializeField] private AudioSource AudioSource;
    // The sliders for volume SFX and Music
    [SerializeField] private Slider slider;
    [SerializeField] private Slider slider2;
    // Beginning values for the sliders when first starting the game
    float volumeMusic = 0.25f;
    float volumeSFX = 1.0f;

    [Header("Difficulty Settings")]
    //Different bools used to configure lore complexity when loading game
    public bool Easy = false;
    public bool Medium = false;
    public bool Hard = false;

    void Start()
    {
        AudioSource = AudioSource.GetComponent<AudioSource>();                          //Getting access to the audio scource where the audio is playing

        slider.value = PlayerPrefs.GetFloat("MusicParam", volumeMusic);          //Sets value of slider to the saved value on disk thanks to player preference (MusicParam) or default when starting (volumeMusic)
        AudioSource.volume = PlayerPrefs.GetFloat("MusicParam", volumeMusic);    //Sets value of audio source to the saved value on disk thanks to player preference (MusicParam) or default when starting (volumeMusic)

        slider2.value = PlayerPrefs.GetFloat("SFXParam", volumeSFX);                    //Sets value of slider to the saved value on disk thanks to player preference (SFXParam) or default when starting (volumeSFX)
        volumeSFX = PlayerPrefs.GetFloat("SFXParam", volumeSFX);                        //Sets value of VolumwSFX to the saved value on disk thanks to player preference (SFXParam) or default when starting (volumeSFX)
        
        SetLevel(volumeMusic);
        SetLevelSFX(volumeSFX);
    }

    public void PlayGame()                          // when playgame is called, it switches scene to the museum hub (beginning of the game and the Hub)
    {
        SceneManager.LoadScene("IraklisScene");  
    }

    //----//
    public void EASY()                  // when easy is pressed, save bool of easy to game manager, setting lore difficulty to Primary level and switches scene to the museum hub (beginning of the game and the Hub)
    {
        Easy = true;
        Medium = false;
        Hard = false;
        GameManager.SetEasy(Easy);
        GameManager.SetMedium(Medium);
        GameManager.SetHard(Hard);
        SceneManager.LoadScene("IraklisScene");
    }

    public void MEDIUM()                // when easy is pressed, save bool of medium to game manager, setting lore difficulty to Secondary level and switches scene to the museum hub (beginning of the game and the Hub)
    {
        Easy = false;
        Medium = true;
        Hard = false;
        GameManager.SetEasy(Easy);
        GameManager.SetMedium(Medium);
        GameManager.SetHard(Hard);
        SceneManager.LoadScene("IraklisScene");
    }

    public void HARD()                  // when easy is pressed, save bool of hard to game manager, setting lore difficulty to Univeristy level and switches scene to the museum hub (beginning of the game and the Hub)
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
        print("Set SFXParam player prefs to " +  volumeMusic);
    }

    public void SetLevelSFX(float slider2Value)
    {
        //mixer volume change
        Mixer.SetFloat("SFXParam", Mathf.Log10(slider2Value) * 20);
        //change volume to new value set by slider
        volumeSFX = slider2Value;
        //saved to disk
        PlayerPrefs.SetFloat("SFXParam", volumeSFX);
        print("Set SFXParam player prefs to " +  volumeSFX);
    }
}
