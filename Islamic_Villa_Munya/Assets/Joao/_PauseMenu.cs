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
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject ControlMenu;
    [SerializeField] private AudioMixer Mixer;
    [SerializeField] private AudioSource AudioSource;

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

    private void OnEnable()
    {
        //Initialise the controls and assign to input actions
        controls.Enable();

        pause = controls.Player.Pause;

        pause.performed += _ => SetCursor(_);
    }

    private void OnDisable()
    {
        pause.Disable();
    }

    private void Start()
    {
        StartCoroutine(ControlsOff());
        GameManager.SetPauseCursor(true);
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
    }

    public void Home()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void SetLevel (float sliderValue)
    {
        Mixer.SetFloat("Volume", Mathf.Log10(sliderValue) * 20);
    }
    private void Update()
    {
        if(CursorUni)
        {
            if(pause.triggered)
            {
                CursorUni = false;
                GameManager.SetPauseCursor(CursorUni);
                Debug.Log(CursorUni);
                Resume();
            }
        }
        else
        {
            if(pause.triggered)
            {
                CursorUni = true;
                GameManager.SetPauseCursor(CursorUni);
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

    public IEnumerator ControlsOff()
    {
        yield return new WaitForSeconds(5f);
        ControlMenu.SetActive(false);
    }
}
