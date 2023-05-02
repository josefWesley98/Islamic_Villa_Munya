using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private bool replace_artefact;
    private bool can_place = false;
    private bool do_until = true;

    void Start()
    {
        kb = InputSystem.GetDevice<Keyboard>();
        artefact.SetActive(true);
        if(IO != null)
        {
            IO.enabled = false;
        }
        renderer.material = transparentMat;
    }

    private void Update() 
    {
        replace_artefact = kb.fKey.isPressed;

        if(do_until)
        {
            if(GameManager.GetArtefactPlaced(artefact_id))
            {
                renderer.material = normalMat;
                if(IO != null)
                {
                    IO.enabled = true;
                }
                do_until = false;
            }
            else if(replace_artefact && can_place)
            {
                Destroy(empty_pedestal);
                GameManager.SetArtefactPlaced(artefact_id, true);
                Debug.Log("is being called1");
            }
        }

    }

    //If the player has collected relevant artefact then make it visible when the player interacts with the pedestal/display case
    //We will check for the id of the artefact. If it is the second pedestal, then set the ID to 1 and we check for the second artefact
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player" && GameManager.GetArtefactCollected(artefact_id))
        {
            can_place = true;
            Debug.Log("boop");
        }
    }

}
/*Cal's script starts here*/
