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
    [SerializeField] private GameObject placeArtefactUI;

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
    private bool isReset = false;
    private bool firstArtefactPlaced = false;
    private float inspectionCooldownTimer = 0.0f;
    private float inspectionCooldownTime = 1.0f;

    private void Awake() => playercontrols = new PlayerControls();

    void Start()
    {
        inspect.gameObject.SetActive(false);
        infoObject.SetActive(false);
        leaveInspectUI.SetActive(false);
        placeArtefactUI.SetActive(false);
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
           
            objRenderer = other.GetComponent<MeshRenderer>();
            
            // if(other.GetComponent<SkinnedMeshRenderer>() != null)
            // {
            //     objRenderer2 = other.GetComponent<SkinnedMeshRenderer>();
            // }
            
            // sets up all the variables that are declared within each interactable object.
            scale = other.GetComponent<InteractableObject>().GetScale();
            rotationForSpawn =  other.GetComponent<InteractableObject>().GetRotation();
            positionOffset = other.GetComponent<InteractableObject>().GetPositionOffset();
            destination = other.gameObject.transform.position;
            displayObject = other.gameObject;
         
            inspect.gameObject.SetActive(true);
            placeArtefactUI.SetActive(false);
            
            if(GameManager.GetArtifactOneToBePlaced() && interactableObj.GetComponent<InteractableObject>().GetPedestalID() == 0 && !firstArtefactPlaced)
            {
                inspect.gameObject.SetActive(false);
                placeArtefactUI.SetActive(true);
            }
            isTouching = true;
            canInspect = true;

            if(GameManager.GetEasy())
            {
                info.text = interactableObj.GetArtifactInfoEasy();
            }
            else if(GameManager.GetMedium())
            {
                info.text = interactableObj.GetArtifactInfoMedium();
            }
            else if(GameManager.GetHard())
            {
                info.text = interactableObj.GetArtifactInfoHard();
            }
            else
            {
                info.text = interactableObj.GetArtifactInfo();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // resets the objects back to visible and destroys the smaller copy that is generated for inspecting.
        if(other.GetComponent<InteractableObject>() && !isReset)
        {

            TurnOnPrimaryArtefact();
            ResetVariables();
            ResetUI();
            DestroyClonedArtifact();
            isReset = true;
        }
        else
        {
            inspect.gameObject.SetActive(false);
            placeArtefactUI.SetActive(false);
        }
    }
    private void ResetVariables()
    {
        artifactGenerated = false;
        isInspecting = false;
        firstPersonCamera.Priority = 5;
        isTouching = false;
        canInspect = false;
        isInspecting = false;
    }
    private void ResetUI()
    {
        leaveInspectUI.SetActive(false);
        infoObject.SetActive(false);
        inspect.gameObject.SetActive(false);
        leaveInspectUI.gameObject.SetActive(false);
    }
    private void TurnOnInspectUI()
    {
        inspect.gameObject.SetActive(false);
        leaveInspectUI.SetActive(true);
        infoObject.SetActive(true);
        leaveInspectUI.gameObject.SetActive(true);
        //info.text = interactableObj.GetArtifactInfo();
        
    }
    private void TurnOffPrimaryArtefact()
    {
        if(objRenderer != null && objRenderer.enabled != false)
        {
            objRenderer.enabled = false;
        }
        // if(objRenderer2 != null && objRenderer2.enabled != false)
        // {
        //     objRenderer2.enabled = false;
        // }
    }
    private void TurnOnPrimaryArtefact()
    {
        if(interactableObj)
        {
            if(interactableObj.GetComponent<MeshRenderer>())
            {
                //if(!interactableObj.GetComponent<MeshRenderer>().enabled)
               // {
                    interactableObj.GetComponent<MeshRenderer>().enabled = true;
                    Debug.Log("Re enabled main artefact via interact.");
               // }
            }
        }
        if(objRenderer != null)
        {
            if(!objRenderer.enabled)
            {
                objRenderer.enabled = true;
                Debug.Log("Re enabled main artefact from obj renderer.");
            }
        }
        // if(objRenderer2 != null)
        // {
        //     objRenderer2.enabled = true;
        // }
        // if(interactableObj)
        // {
        //     if(interactableObj.GetComponent<SkinnedMeshRenderer>())
        //     {
        //         interactableObj.GetComponent<SkinnedMeshRenderer>().enabled = true;
        //     }
        // }
        
    }
    private void PositionCamera()
    {
        fpCameraPos.position = new Vector3(transform.position.x,transform.position.y + 1.3f, transform.position.z);
        firstPersonCamera.Priority = 15;
    }
    private void CreateClonedArtifact()
    {
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
    private void DestroyClonedArtifact()
    {
        if(holder != null)
        {
            Destroy(holder);
        }
        phObject = null;
        interactableObj = null;
        objRenderer = null;
        objRenderer2 = null;
    }
    private void DoInspectionCooldown()
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
    }
    private void Checks()
    {
        // if out of range of any artifacts the n dont let the player inspect.
        if(!canInspect)
        {
            isInspecting = false;
        }
    }
    private void DoInspecting()
    {
        if(isInspecting)
        {
            // ui enabled.
            TurnOnInspectUI();

            // stop rendering primary artifact.
            TurnOffPrimaryArtefact();

            // position the camera and make it the active cam.
            PositionCamera(); 
            
            // generate the new smaller artifact that is for inspecting.
            CreateClonedArtifact();
           
        }
    }
    void Update()
    {
        
        DoInspectionCooldown();

        Checks();
        
        DoInspecting();
        
    }
    private void Inspecting(InputAction.CallbackContext context)
    {
        // this ends the inspection mechanic and resets the original artifact to its visible state.
        if(isInspecting && !inspectionCooldown)
        {
            isInspecting = false;

            if(!isReset)
            {
                TurnOnPrimaryArtefact();
                ResetUI();
                ResetVariables();
                DestroyClonedArtifact();
                isReset = true;
            }
        
            inspectionCooldown = true;
        }
        
        //GameManager.SetArtefactCollected(0, true);
        // when inspecting is pressed this activates the inspection mechanic.
        if(canInspect && !inspectionCooldown)
        {
            if(GameManager.GetArtifactOneToBePlaced() && interactableObj.GetComponent<InteractableObject>().GetPedestalID() == 0 && !firstArtefactPlaced)
            {
                GameManager.SetArtifactOneToBePlaced(false);
                placeArtefactUI.SetActive(false);
                inspectionCooldown = true;
                firstArtefactPlaced = true;
                isReset = false;
            }
            else
            {
                isReset = false;
                isInspecting = true;
                inspectionCooldown = true;
            }
            isReset = false;
        }
    }
    
    public bool GetIsInspecting()
    {
        return isInspecting;
    }
}
