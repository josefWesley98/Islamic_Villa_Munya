using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
/*Cal's script starts here*/
public class PlaceArtefact : MonoBehaviour
{
    private Keyboard kb;
    [SerializeField] private GameObject artefact;
    [SerializeField] private GameObject empty_pedestal;
    [SerializeField] private int artefact_id;
    [SerializeField] private MeshRenderer renderer;
    [SerializeField] private InteractableObject IO;
    [SerializeField] private Material transparentMat;
    [SerializeField] private Material normalMat;
    [SerializeField] private BoxCollider collider;
    [SerializeField] private MeshRenderer paintingRenderer;
    private bool replace_artefact;
    private bool can_place = false;
    private bool do_until = true;
    private bool check_reset = true;

//Leon audio
    AudioMixer mixer;
    AudioClip placeClip;
    AudioSource placeSound;

    void Start()
    {
        mixer = Resources.Load("NewAudioMixer") as AudioMixer;
        placeClip = Resources.Load("sfx_place_artefact 1") as AudioClip;
        placeSound = gameObject.AddComponent<AudioSource>();
        placeSound.clip = placeClip;
        placeSound.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
//leon end

        kb = InputSystem.GetDevice<Keyboard>();
        artefact.SetActive(true);
        if(IO != null)
        {
            IO.enabled = false;
        }
        if(collider != null)
        {
            collider.enabled = false;
        }
        if(paintingRenderer != null)
        {
            paintingRenderer.enabled = false;
        }
        renderer.material = transparentMat;
    }

    private void Update() 
    {
        //Debug.Log("Can we place?? " + can_place);
        replace_artefact = kb.fKey.isPressed;

        //If the player has not collected the total of the id of the current artefact, reset it in the museum
        if(empty_pedestal.activeSelf == false && (GameManager.GetArtefactCounter() < artefact_id || GameManager.GetArtefactCounter() == 0))
        {
            do_until = true;
            can_place = false;

            GameManager.SetArtefactPlaced(artefact_id, false);

            empty_pedestal.SetActive(true);
            renderer.material = transparentMat;

            if (IO != null)
            {
                IO.enabled = false;
            }
            if (collider != null)
            {
                collider.enabled = false;
            }
            if (paintingRenderer != null)
            {
                paintingRenderer.enabled = false;
            }

        }

        if(do_until)
        {
            if(GameManager.GetArtefactPlaced(artefact_id))
            {
                Debug.Log(artefact_id + " I have beeen plaaaaaaaaaced");
                //play leon audio
                placeSound.Play();
                //leon end

                renderer.material = normalMat;
                if(IO != null)
                {
                    IO.enabled = true;
                }
                if(collider != null)
                {
                    collider.enabled = true;
                }
                if(paintingRenderer != null)
                {
                    paintingRenderer.enabled = true;
                }
                do_until = false;
            }
            else if(replace_artefact && can_place)
            {
                GameManager.SetArtefactPlaced(artefact_id, true);
            }
           
        }
        if(GameManager.GetArtefactPlaced(artefact_id) && empty_pedestal != null)
        {
            empty_pedestal.SetActive(false);
        }
    }

    //If the player has collected relevant artefact then make it visible when the player interacts with the pedestal/display case
    //We will check for the id of the artefact. If it is the second pedestal, then set the ID to 1 and we check for the second artefact
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player" && GameManager.GetArtefactCollected(artefact_id))
        {
            can_place = true;
        }
    }

    public int artefactID()
    {
        return artefact_id;
    }

}
/*Cal's script starts here*/
