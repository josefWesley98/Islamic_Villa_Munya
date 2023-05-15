using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.UI;

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
    [SerializeField] private GameObject textBackdrop;

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
    private bool destroyAfterView = false;
    private bool inspectionCooldown = false;
    private bool isReset = false;
    private bool firstArtefactPlaced = false;
    private bool secondArtefactPlaced = false;
    private bool doSetup = false;
    private bool cursorState = false;
    private float inspectionCooldownTimer = 0.0f;
    private float inspectionCooldownTime = 1.0f;
    private int artifactToBePlaced = -1;

    private void Awake() => playercontrols = new PlayerControls();

    void Start()
    {
        inspect.gameObject.SetActive(false);
        infoObject.SetActive(false);
        leaveInspectUI.SetActive(false);
        placeArtefactUI.SetActive(false);
        textBackdrop.SetActive(false);
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
            
            // sets up all the variables that are declared within each interactable object.
            scale = interactableObj.GetScale();
            rotationForSpawn =  interactableObj.GetRotation();
            positionOffset = interactableObj.GetPositionOffset();
            destroyAfterView = interactableObj.GetDestroyAfterView();
            destination = other.gameObject.transform.position;
            displayObject = other.gameObject;

            int pedID = interactableObj.GetPedestalID();
            
            if(pedID <= GameManager.GetTotalArtefacts())
            {
                if(GameManager.GetArtefactToBePlaced(pedID))
                {
                    inspect.gameObject.SetActive(false);
                    placeArtefactUI.SetActive(true);
                    artifactToBePlaced = pedID;
                }
            }
            else
            {
                artifactToBePlaced = -1;
                inspect.gameObject.SetActive(true);
                placeArtefactUI.SetActive(false);
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
            ResetUI();
            DestroyClonedArtifact();
            TurnOnPrimaryArtefact();
            ResetVariables();
            isReset = true;
            cursorState = false;
            if(isInspecting)
            {
                
                if(destroyAfterView)
                {
                    displayObject.GetComponent<InteractableObject>().SetVisibilityCheck(true);
                    displayObject.SetActive(false);
                }
            }
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
        textBackdrop.SetActive(false);
    }
    private void TurnOnInspectUI()
    {
        inspect.gameObject.SetActive(false);
        leaveInspectUI.SetActive(true);
        infoObject.SetActive(true);
        leaveInspectUI.gameObject.SetActive(true);
        textBackdrop.SetActive(true);
    }
    private void TurnOffPrimaryArtefact()
    {
        if(objRenderer != null && objRenderer.enabled != false)
        {
            objRenderer.enabled = false;
        }
    }
    private void TurnOnPrimaryArtefact()
    {
        if(interactableObj)
        {
            if(interactableObj.GetComponent<MeshRenderer>())
            {
                if(!interactableObj.GetComponent<MeshRenderer>().enabled)
                {
                    interactableObj.GetComponent<MeshRenderer>().enabled = true;
                }
            }
        }
        if(objRenderer != null)
        {
            if(!objRenderer.enabled)
            {
                objRenderer.enabled = true;
            }
        }
        
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
            if(doSetup)
            {
                TurnOffPrimaryArtefact();
                doSetup = false;
            }

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
            if(destroyAfterView)
            {
                displayObject.GetComponent<InteractableObject>().SetVisibilityCheck(true);
                displayObject.SetActive(false);
            }
            isInspecting = false;
            cursorState = false;
            if(!isReset)
            {
                TurnOnPrimaryArtefact();
                ResetUI();
                DestroyClonedArtifact();
                TurnOnPrimaryArtefact();
                ResetVariables();
                isReset = true;
            }
        
            inspectionCooldown = true;
        }
        // when inspecting is pressed this activates the inspection mechanic.
        if(canInspect && !inspectionCooldown)
        {
            
            inspectionCooldown = true;
            isReset = false;
            if(artifactToBePlaced != -1)
            {
                if(GameManager.GetArtefactToBePlaced(artifactToBePlaced))
                {
                    GameManager.SetArtefactToBePlaced(false, artifactToBePlaced);
                    GameManager.SetArtefactPlaced(artifactToBePlaced, false);
                    placeArtefactUI.SetActive(false);
                    inspectionCooldown = true;
                    isInspecting = false;
                    isReset = false;
                    artifactToBePlaced = -1; 
                }
            }
            else
            {
                cursorState = true;
                isReset = false;
                isInspecting = true;
                inspectionCooldown = true;
                doSetup = true;
            }        
            
        }
    }
    
    public bool GetIsInspecting()
    {
        return isInspecting;
    }
}
