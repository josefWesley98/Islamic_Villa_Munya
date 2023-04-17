using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerInteract : MonoBehaviour
{
    Movement movement;
    PlayerControls playercontrols;
    InputAction inspectPress;
    InteractalbeObject interactableObj;
    AnimationStateController ASC;
    MeshRenderer objRenderer;
    SkinnedMeshRenderer objRenderer2;
    GameObject displayObject;
    GameObject holder;
    Transform LookAt;
    Vector3 destination;
    Vector3 scale;
    Vector3 rotationForSpawn;
    Vector3 positionOffset;
    bool artifactGenerated = false;
    [SerializeField] NIThirdPersonController controller;
    [SerializeField] CinemachineVirtualCamera firstPersonCamera;
    [SerializeField] Transform cameraPos;
    [SerializeField] TMPro.TMP_Text inspect;
    [SerializeField] TMPro.TMP_Text info;
    [SerializeField] GameObject infoObject;
    //[SerializeField] GameObject closeButton;
    [SerializeField] Camera cam;
    [SerializeField] float distFromCam = 1.5f;
    private bool isTouching = false;
    public bool isInspecting = false;
    private bool canInspect = false;
    GameObject phObject;

    private void Awake() => playercontrols = new PlayerControls();

    void Start()
    {
        movement = GetComponent<Movement>();
        ASC = GetComponent<AnimationStateController>(); 
        inspect.gameObject.SetActive(false);
        //infoObject.SetActive(false);
        //closeButton.SetActive(false);
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
        Debug.Log("Object name: " + other.name + ", Active: " + other.gameObject.activeSelf);
        if(other.GetComponent<InteractalbeObject>())
        {
            Debug.Log("Interacting");
            //LookAt = other.GetComponent<InteractalbeObject>().GetLookAt();
            interactableObj = other.GetComponent<InteractalbeObject>();
            if(other.GetComponent<MeshRenderer>() != null)
            {
                objRenderer = other.GetComponent<MeshRenderer>();
            }
            if(other.GetComponent<SkinnedMeshRenderer>() != null)
            {
                objRenderer2 = other.GetComponent<SkinnedMeshRenderer>();
            }
            scale = other.GetComponent<InteractalbeObject>().GetScale();
            rotationForSpawn =  other.GetComponent<InteractalbeObject>().GetRotation();
            positionOffset = other.GetComponent<InteractalbeObject>().GetPositionOffset();
            destination = other.gameObject.transform.position;
            displayObject = other.gameObject;
            inspect.gameObject.SetActive(true);
            isTouching = true;
            canInspect = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<InteractalbeObject>())
        {
            isInspecting = false;

            if(objRenderer != null)
            {
                objRenderer.enabled = true;
            }

            if(objRenderer2 != null)
            {
                objRenderer2.enabled = true;
            }
            firstPersonCamera.Priority = 5;

            isTouching = false;
            canInspect = false;
            isInspecting = false;

            inspect.gameObject.SetActive(false);
            if(holder != null)
            {
                Destroy(holder);
            }
            phObject = null;
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
            //infoObject.SetActive(false);
        }

        if(!canInspect)
        {
            isInspecting = false;
            //ASC.enabled = true;
            //controller.HideCursor();
        }

        if(isInspecting)
        {
            controller.ShowCursor();
            if(objRenderer != null && objRenderer.enabled != false)
            {
                objRenderer.enabled = false;
            }
            if(objRenderer2 != null && objRenderer2.enabled != false)
            {
                objRenderer2.enabled = false;
            }
            //ASC.enabled = false;
            cameraPos.position = new Vector3(transform.position.x,transform.position.y + 1.15f, transform.position.z);
            firstPersonCamera.Priority = 15;

            infoObject.SetActive(true);
            info.text = interactableObj.GetArtifactInfo();
            
            if(!artifactGenerated)
            {
                 Debug.Log("Am I ever called?");
                Quaternion rotation = Quaternion.identity;
                destination = new Vector3(destination.x + positionOffset.x,destination.y + positionOffset.y,destination.z + positionOffset.z);
                var inspectObject = Instantiate(displayObject,destination, rotation) as GameObject;
                inspectObject.transform.localScale = scale;
                inspectObject.transform.eulerAngles = rotationForSpawn;
                holder = inspectObject;
                artifactGenerated = true;  
                Vector3 direction =  destination - transform.position;
                direction.y = 0;
                // Calculate the target rotation using Quaternion.LookRotation
                Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
                cameraPos.localRotation = targetRotation;
                phObject = inspectObject;
            }
            if(phObject.GetComponent<MeshRenderer>() != null)
            {
                if(!phObject.GetComponent<MeshRenderer>().enabled)
                {
                    phObject.GetComponent<MeshRenderer>().enabled = true;
                }
            }
            if(phObject.GetComponent<SkinnedMeshRenderer>() != null)
            {
                if(!phObject.GetComponent<SkinnedMeshRenderer>().enabled)
                {
                    phObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
                }
            }       
        }
        else
        {
            artifactGenerated = false;
            firstPersonCamera.Priority = 5;
            //closeButton.SetActive(false);
            if(holder != null)
            {
                Destroy(holder);
            }
        }
    }
    private void Inspecting(InputAction.CallbackContext context)
    {
        if(canInspect)
        {
            Debug.Log("Can inspect");       
            isInspecting = true;
        }
    }
    public void ButtonCloseInspection()
    {
        isInspecting = false;
        if(objRenderer != null)
        {
            objRenderer.enabled = true;
        }
        if(objRenderer2 != null)
        {
            objRenderer2.enabled = true;
        }
        if(holder != null)
        {
            Destroy(holder);
        }
    }
    public bool GetIsInspecting()
    {
        return isInspecting;
    }
}
