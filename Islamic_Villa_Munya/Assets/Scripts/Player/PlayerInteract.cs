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

    private void Awake() => playercontrols = new PlayerControls();

    void Start()
    {
        movement = GetComponent<Movement>();
        ASC = GetComponent<AnimationStateController>(); 
        inspect.gameObject.SetActive(false);
        infoObject.SetActive(false);
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
        if(other.GetComponent<InteractalbeObject>())
        {
            //LookAt = other.GetComponent<InteractalbeObject>().GetLookAt();
            interactableObj = other.GetComponent<InteractalbeObject>();
            objRenderer = other.GetComponent<MeshRenderer>();
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
            ASC.enabled = true;
            //controller.HideCursor();
            objRenderer.enabled = true;
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
            ASC.enabled = true;
            //controller.HideCursor();
        }

        if(isInspecting)
        {
            controller.ShowCursor();
            objRenderer.enabled = false;
            ASC.enabled = false;
            cameraPos.position = new Vector3(transform.position.x,transform.position.y + 1.15f, transform.position.z);
            
            
           
            //cameraPos.localRotation = Quaternion.Euler(cameraPos.localRotation.x, cameraPos.localRotation.y,cameraPos.localRotation.z);
            firstPersonCamera.Priority = 15;
            //closeButton.SetActive(true);
            infoObject.SetActive(true);
            info.text = interactableObj.GetArtifactInfo();
            if(!artifactGenerated)
            {
                Ray ray = cam.ViewportPointToRay(new Vector3(0.5f,0.5f,0f));
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit)){
                    //destination = hit.point;
                   // destination = ray.GetPoint(distFromCam);
                }
                else
                {
                    //destination = ray.GetPoint(distFromCam);
                }
                Quaternion rotation = Quaternion.identity;
                destination = new Vector3(destination.x + positionOffset.x,destination.y + positionOffset.y,destination.z + positionOffset.z);
                var inspectObject = Instantiate(displayObject,destination, rotation) as GameObject;
                inspectObject.GetComponent<MeshRenderer>().enabled = true;
                inspectObject.transform.localScale = scale;
                inspectObject.transform.eulerAngles = rotationForSpawn;
                holder = inspectObject;
                artifactGenerated = true;  
                Vector3 direction =  destination - transform.position;
                direction.y = 0;
                // Calculate the target rotation using Quaternion.LookRotation
                Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
                cameraPos.localRotation = targetRotation;
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
            
            isInspecting = true;
        }
    }
    public void ButtonCloseInspection()
    {
        //controller.HideCursor();
        isInspecting = false;
        objRenderer.enabled = true;
        ASC.enabled = true;
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
