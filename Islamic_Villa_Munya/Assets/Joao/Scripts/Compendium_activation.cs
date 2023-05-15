using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compendium_activation : MonoBehaviour
{
    [SerializeField] private GameObject _Compendium;
    [SerializeField] GameObject notifications;

    private bool CursorUni = false;

    public void Museum_compendium_Activation()
    {
        CursorUni = true;
        GameManager.SetPauseCursor(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log(CursorUni);

        _Compendium.SetActive(true);
        Time.timeScale = 0f;
        notifications.SetActive(false);
    }

    public void Museum_compendium_Deactivation()
    {
        CursorUni = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log(CursorUni);

        GameManager.SetPauseCursor(CursorUni);
        _Compendium.SetActive(false);
        Time.timeScale = 1f;
        notifications.SetActive(true);
    }

    private void Update()
    {
        /*if (Input.GetKeyDown("space"))
        {
            Museum_compendium_Activation();
        }*/
    }
}
