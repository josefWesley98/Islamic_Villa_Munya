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
    [SerializeField] private AudioMixer Mixer;
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private Slider slider;
    float volume = 0.2f;

    [Header("Cursor settings")]
    [SerializeField] private PlayerControls controls;
    InputAction pause;
    private bool CursorUni = false;

    private void Awake()
    {
        //Assign the new input controller
        GameManager.SetPauseCursor(CursorUni);
        Cursor.visible = false;
        controls = new PlayerControls();
        
    }
    //deacitvates cursor and centers it so it doesn't move

    private void OnEnable()
    {
        //Initialise the controls and assign to input actions
        controls.Enable();

        pause = controls.Player.Pause;

        pause.performed += _ => SetCursor(_);
    }
    //changes controls to enabled when in pause menu

    private void OnDisable()
    {
        pause.Disable();
    }
    //changes controls to disabled when in pause menu

    private void Start()
    {
        //StartCoroutine(ControlsOff());
        //GameManager.SetPauseCursor(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        AudioSource = AudioSource.GetComponent<AudioSource>();
        slider.value = PlayerPrefs.GetFloat("Volume", AudioSource.volume);
        AudioSource.volume = PlayerPrefs.GetFloat("Volume", AudioSource.volume);
    }
    //cursor is in a locked state and not visible, gets audioSource and the slider value, sets it to medium volume

    public void Pause()
    {
        pauseMenu.SetActive(true);
        notifications.SetActive(false);
        Time.timeScale = 0f;
        Cursor.visible = true;
    }
    //pause menu is activated. time jumps to zero (it freezes), deactivates notifications so no controls are shown and activates the cursor

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        notifications.SetActive(true);        
        CursorUni = false;        
        Debug.Log(CursorUni);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        //Cursor.visible = false;//GameManager.SetPauseCursor(true);
    }
    //pause menu is deactivated. time jumps to 1 (normal speed), activates notifications in case some where hidden while the pause menu was activated and sets the cursor to false

    public void Home()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
    //loads the Menu scene from the scene manager and sets time to 1 (normal speed) so it actually changes without problem

    public void SetLevel (float sliderValue)
    {
        Mixer.SetFloat("Volume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("Volume", volume);
    }
    //sets game volume to slider value

    private void Update()
    {
        if(CursorUni)
        {
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
        {
            if(pause.triggered)
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

    /*public IEnumerator ControlsOff()
    {
        yield return new WaitForSeconds(10f);
        ControlMenu.SetActive(false);
        ControlMenu2.SetActive(false);
    }*/
}
