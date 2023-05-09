using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Compendium : MonoBehaviour
{
    [Header("Entries_paramaters")]                          // different entries types, unknown and Discovered
    [SerializeField] private GameObject Unknown_Entry;
    [SerializeField] private GameObject Discovered_Entry;

    [SerializeField] private int artefact_id;

    private bool do_until = true;
    // Start is called before the first frame update
    void Start()
    {
        Unknown_Entry.SetActive(true);
        Discovered_Entry.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(do_until)
        {
            if(GameManager.GetArtefactCollected(artefact_id))
            {
                Unknown_Entry.SetActive(false);
                Discovered_Entry.SetActive(true);

                do_until = false;
            }
        }
    }
}
