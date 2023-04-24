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
    private bool replace_artefact = false;

    // Start is called before the first frame update
    void Start()
    {
        kb = InputSystem.GetDevice<Keyboard>();
        artefact.SetActive(false);
    }

    private void Update() 
    {
        replace_artefact = kb.fKey.isPressed;
    }

    //If the player has collected relevant artefact then make it visible when the player interacts with the pedestal/display case
    //We will check for the id of the artefact. If it is the second pedestal, then set the ID to 1 and we check for the second artefact
    private void OnCollisionEnter(Collision other) 
    {
        if(replace_artefact && other.gameObject.tag == "Player" && GameManager.GetArtefactCollected(artefact_id))
        {
            Debug.Log("Placing artefact");
            Destroy(empty_pedestal);
            artefact.SetActive(true);
        }
    }

}
/*Cal's script starts here*/
