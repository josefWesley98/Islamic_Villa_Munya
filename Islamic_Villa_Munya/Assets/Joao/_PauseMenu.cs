using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class _PauseMenu : MonoBehaviour
{
    [Header("Pause Menu Settings")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject notifications;

    /*[SerializeField] GameObject ControlMenu;
    [SerializeField] GameObject ControlMenu2;*/

    [Header("Volume settings")]
    // Access the audio mixer and audio source where sound is playing
    [SerializeField] private AudioMixer Mixer;
    [SerializeField] private AudioSource AudioSource;
    // The sliders for volume SFX and Music
    [SerializeField] private Slider slider;
    [SerializeField] private Slider slider2;
    // The float values of the different volumes
    float volume;
    float volumeSFX;

    [Header("Cursor settings")]
    // the fields for the cursor
    [SerializeField] private PlayerControls controls;
    InputAction pause;
    private bool CursorUni = false;
    

    
    private void Awake()
    {
        //Assign the new input controller
        GameManager.SetPauseCursor(CursorUni);
        Cursor.visible = false;
        controls = new PlayerControls();
        
    }                                                           //deacitvates cursor and centers it so it doesn't move
    

    private void OnEnable()
    {
        //Initialise the controls and assign to input actions
        controls.Enable();

        pause = controls.Player.Pause;

        pause.performed += _ => SetCursor(_);
    }                                                           //changes controls to enabled when in pause menu

    private void OnDisable()
    {
        pause.Disable();
    }                                                           //changes controls to disabled when in pause menu

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;                                       //cursor is locked so it won't move
        Cursor.visible = false;                                                         //sets cursor visibality to false                                 

        AudioSource = AudioSource.GetComponent<AudioSource>();                          //Getting access to the audio scource where the audio is playing

        slider.value = PlayerPrefs.GetFloat("MusicParam", AudioSource.volume);          //Sets value of slider to the saved value on disk thanks to player preference (MusicParam)
        AudioSource.volume = PlayerPrefs.GetFloat("MusicParam", AudioSource.volume);    //Sets value of audio source to the saved value on disk thanks to player preference (MusicParam)
        //print("Got volume from player prefs " + volume);
        slider2.value = PlayerPrefs.GetFloat("SFXParam", volumeSFX);                    //Sets value of slider to the saved value on disk thanks to player preference (SFXParam)
        volumeSFX = PlayerPrefs.GetFloat("SFXParam", volumeSFX);                        //Sets value of VolumwSFX to the saved value on disk thanks to player preference (SFXParam)
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        notifications.SetActive(false);
        Time.timeScale = 0f;
        Cursor.visible = true;
    }                                                           //pause menu is activated. time jumps to zero (it freezes), deactivates notifications so no controls are shown and activates the cursor
    

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        notifications.SetActive(true);        
        CursorUni = false;        
        Debug.Log(CursorUni);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }                                                           //pause menu is deactivated. time jumps to 1 (normal speed), activates notifications in case some where hidden while the pause menu was activated and sets the cursor to false

    public void Home()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }                                                           //loads the Menu scene from the scene manager and sets time to 1 (normal speed) so it actually changes without problem

    public void SetLevelMusic (float sliderValue)                    // slider for volume Music
    {
        Mixer.SetFloat("MusicParam", Mathf.Log10(sliderValue) * 20);
        //change volume to new value set by slider
        volume = sliderValue;
        PlayerPrefs.SetFloat("MusicParam", volume);
        print("Set MusicParam player prefs to " + volume);

        AudioSource.volume = PlayerPrefs.GetFloat("MusicParam", AudioSource.volume);
    }
    //sets game volume to slider value
    public void SetLevelSFX(float slider2Value)                 // slider for volume SFX
    {
        //mixer volume change
        Mixer.SetFloat("SFXParam", Mathf.Log10(slider2Value) * 20);
        //change volume to new value set by slider
        volumeSFX = slider2Value;
        //saved to disk
        PlayerPrefs.SetFloat("SFXParam", volumeSFX);
        print("Set SFXParam player prefs to " +  volumeSFX);
    }
    
    private void Update()                                       // if statement for cursor mode and visibality, 
    {
        if(CursorUni)
        {                                                       //when not in pause menu it locks the cursor and makes it invisible 
            if(pause.triggered)
            {
                CursorUni = false;
                //GameManager.SetPauseCursor(CursorUni);
                //GameManager.SetPauseCursor(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Debug.Log(CursorUni);
                Resume();
            }
        }
        else
        {                                                       //when in pause menu it frees the cursor and makes it visible
            if (pause.triggered)
            {
                CursorUni = true;
                //GameManager.SetPauseCursor(CursorUni);
                GameManager.SetPauseCursor(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = false;
                Debug.Log(CursorUni);
                Pause();
            }
        }
    }

    //Set the cursor here for gamemanager so the player can access it
    private void SetCursor(InputAction.CallbackContext cursor)
    {
        //Placeholder
        Debug.Log("Boo!");
    }
                                                                //a debug log to make sure it works correctly
}
