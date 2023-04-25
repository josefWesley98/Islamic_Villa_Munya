using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerInteract : MonoBehaviour
{   
    [Header("Script Reference")]
    [SerializeField] private NIThirdPersonController controller;
    [SerializeField] private AnimationStateController ASC;

    [Header("FP virtual Camera reference")]
    [SerializeField] private CinemachineVirtualCamera firstPersonCamera;
    [SerializeField] private Transform fpCameraPos;
    
    [Header("Normal Camera Ref and Positin Ref")]
    [SerializeField] private Camera cam;

    [Header("Text Mesh Pro Textbox References")]
    [SerializeField] private TMPro.TMP_Text inspect;
    [SerializeField] private TMPro.TMP_Text info;

    [Header("UI GameObjects")]
    [SerializeField] private GameObject infoObject;
    [SerializeField] private GameObject leaveInspectUI;

    [Header("Offset For Player Height")]
    [SerializeField] private float distFromCam = 1.5f;

    private PlayerControls playercontrols;
    private InputAction inspectPress;

    private MeshRenderer objRenderer;
    private SkinnedMeshRenderer objRenderer2;
    
    private InteractableObject interactableObj;
    private GameObject displayObject;
    private GameObject holder;
    GameObject phObject;
    
    private Transform LookAt;
    private Vector3 destination;
    private Vector3 scale;
    private Vector3 rotationForSpawn;
    private Vector3 positionOffset;

    private bool artifactGenerated = false;
    private bool isTouching = false;
    private bool isInspecting = false;
    private bool canInspect = false;
    private bool inspectionCooldown = false;

    private float inspectionCooldownTimer = 0.0f;
    private float inspectionCooldownTime = 2.0f;

    private void Awake() => playercontrols = new PlayerControls();

    void Start()
    {
        inspect.gameObject.SetActive(false);
        infoObject.SetActive(false);
        leaveInspectUI.SetActive(false);
    }

    private void OnEnable()
    {
        playercontrols.Enable();
        
        inspectPress = playercontrols.Player.Inspect;
        inspectPress.Enable();
        inspectPress.performed += _ => Inspecting(_);
        inspectPress.canceled += _ => Inspecting(_);
    }

    private void OnDisable()
    {
        playercontrols.Disable();
        inspectPress.Disable();
    }

    void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<InteractableObject>())
        {       
            interactableObj = other.GetComponent<InteractableObject>();

            //gathers a reference to either the mesh or skinned mesh renderer for future use on the object.
            if(other.GetComponent<MeshRenderer>() != null)
            {
                objRenderer = other.GetComponent<MeshRenderer>();
            }
            if(other.GetComponent<SkinnedMeshRenderer>() != null)
            {
                objRenderer2 = other.GetComponent<SkinnedMeshRenderer>();
            }
            
            // sets up all the variables that are declared within each interactable object.
            scale = other.GetComponent<InteractableObject>().GetScale();
            rotationForSpawn =  other.GetComponent<InteractableObject>().GetRotation();
            positionOffset = other.GetComponent<InteractableObject>().GetPositionOffset();
            destination = other.gameObject.transform.position;
            displayObject = other.gameObject;
            inspect.gameObject.SetActive(true);
            isTouching = true;
            canInspect = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // resets the objects back to visible and destroys the smaller copy that is generated for inspecting.
        if(other.GetComponent<InteractableObject>())
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
            // resets the camera priority so it goes back to the third person cam.
            firstPersonCamera.Priority = 5;

            isTouching = false;
            canInspect = false;
            isInspecting = false;

            // disables any UI.

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
        // a cool down so when engaging and disengaging between viewing and closing it doesnt bug out.
        if(inspectionCooldown)
        {
            inspectionCooldownTimer += Time.deltaTime;
            if(inspectionCooldownTimer >= inspectionCooldownTime)
            {
                inspectionCooldownTimer = 0f;
                inspectionCooldown = false;
            }
        }

        // ends te objects visibility if isnt touching
        if(!isTouching || isInspecting)
        {
            inspect.gameObject.SetActive(false);
        }
        
        // if isnt inspecting set UI object to inactive.(redunency)
        if(isInspecting == false)
        {
            infoObject.SetActive(false);
        }

        // if out of range of any artifacts the n dont let the player inspect.
        if(!canInspect)
        {
            isInspecting = false;
        }

        // if the player is inspecting then do the setup and enable the appropiate UI and generate the the mini game object for inspection while disabling the main artifacts draw. 
        if(isInspecting)
        {
            // ui enabled.
            leaveInspectUI.SetActive(true);
            controller.ShowCursor();

            // stop renderering primary artifact.
            if(objRenderer != null && objRenderer.enabled != false)
            {
                objRenderer.enabled = false;
            }
            if(objRenderer2 != null && objRenderer2.enabled != false)
            {
                objRenderer2.enabled = false;
            }

            // position the camera and make it the active cam.
            fpCameraPos.position = new Vector3(transform.position.x,transform.position.y + 1.3f, transform.position.z);
            firstPersonCamera.Priority = 15;

            // activate the info about artifact UI Object
            infoObject.SetActive(true);
            info.text = interactableObj.GetArtifactInfo();
            
            // generate the new smaller artifact that is for inspecting.
            if(!artifactGenerated)
            {
                // reset rotation.
                Quaternion rotation = Quaternion.identity;
                // position.
                destination = new Vector3(destination.x + positionOffset.x,destination.y + positionOffset.y,destination.z + positionOffset.z);
                // the insantiation of the object.
                var inspectObject = Instantiate(displayObject,destination, rotation) as GameObject;
                // scale and rotation for on spawn
                inspectObject.transform.localScale = scale;
                inspectObject.transform.eulerAngles = rotationForSpawn;

                // gathering a reference to destory the object later.
                holder = inspectObject;
                // stops the if from looping as the generation condition is met.
                artifactGenerated = true;  

                // rotates the camera to have a front row view of the object.
                Vector3 direction =  destination - transform.position;
                direction.y = 0;
                // Calculate the target rotation using Quaternion.LookRotation
                Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
                fpCameraPos.localRotation = targetRotation;
                phObject = inspectObject;
            }

            // enables this copys version of the mesh renderer so it shows to the player.
            if(holder.GetComponent<MeshRenderer>() != null)
            {
                if(!holder.GetComponent<MeshRenderer>().enabled)
                {
                    holder.GetComponent<MeshRenderer>().enabled = true;
                }
            }

            // same as above but incase it uses a skinned mesh renderer.
            if(holder.GetComponent<SkinnedMeshRenderer>() != null)
            {
                if(!holder.GetComponent<SkinnedMeshRenderer>().enabled)
                {
                    holder.GetComponent<SkinnedMeshRenderer>().enabled = true;
                }
            }       
        }
        else
        {
            // handles destruction.
            artifactGenerated = false;
            firstPersonCamera.Priority = 5;
            leaveInspectUI.SetActive(false);

            if(holder != null)
            {
                Destroy(holder);
            }
        }
    }
    private void Inspecting(InputAction.CallbackContext context)
    {
        // this ends the inspection mechanic and resets the original artifact to its visible state.
        if(isInspecting)
        {
            isInspecting = false;
            if(interactableObj)
            {
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
            inspectionCooldown = true;
            leaveInspectUI.SetActive(false);
        }
        // when inspecting is pressed this activates the inspection mechanic.
        if(canInspect && !inspectionCooldown)
        {
            isInspecting = true;
            leaveInspectUI.SetActive(true);
        }
    }
    
    public bool GetIsInspecting()
    {
        return isInspecting;
    }
}
