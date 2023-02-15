using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
public class PlayerInteract : MonoBehaviour
{
    Movement movement;
    PlayerControls playercontrols;
    InputAction inspectPress;
    InteractalbeObject interactableObj;

    [SerializeField] TMPro.TMP_Text inspect;
    [SerializeField] TMPro.TMP_Text info;
    [SerializeField] GameObject infoObject;
    [SerializeField] GameObject closeButton;

    private bool isTouching = false;
    public bool isInspecting = false;
    private bool canInspect = false;

    private void Awake() => playercontrols = new PlayerControls();

    void Start()
    {
        movement = GetComponent<Movement>();
        inspect.gameObject.SetActive(false);
        infoObject.SetActive(false);
        closeButton.SetActive(false);
    }

    private void OnEnable()
    {
        playercontrols.Enable();
        
        inspectPress = playercontrols.Player.Inspect;
        inspectPress.Enable();
        inspectPress.performed += Inspecting;
    }

    private void OnDisable()
    {
        playercontrols.Disable();
        inspectPress.Disable();
    }

    void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<InteractalbeObject>())
        {
            interactableObj = other.GetComponent<InteractalbeObject>();
            inspect.gameObject.SetActive(true);
            isTouching = true;
            canInspect = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<InteractalbeObject>())
        {
            isTouching = false;
            canInspect = false;
            Debug.Log("is exiting");
        }
    }

    void Update()
    {
        if(!isTouching || isInspecting)
        {
            inspect.gameObject.SetActive(false);
        }
        
        if(isInspecting == false)
        {
            infoObject.SetActive(false);
        }

        if(!canInspect)
        {
            isInspecting = false;
        }

        if(isInspecting)
        {
            closeButton.SetActive(true);
            infoObject.SetActive(true);
            info.text =interactableObj.GetArtifactInfo();
        }
        else
        {
            closeButton.SetActive(false);
        }
    }
    private void Inspecting(InputAction.CallbackContext context)
    {
        if(canInspect)
        {
            isInspecting = true;
        }
    }
    public void ButtonCloseInspection()
    {
        isInspecting = false;
    }
}
