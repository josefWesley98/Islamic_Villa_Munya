using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

/*Cal's code starts here*/

//This is a test for loading as seamlessly as possible between the hub and the villa vice versa
//Dynamic Level loading
public class HUBTransitionController : MonoBehaviour
{
    [SerializeField] private string hub_scene_name;
    private bool is_loading = false;
    [SerializeField] private Animator transition;
    [SerializeField] private float transition_t = 10f;
    private int total_artefact_collected = 0;
    [SerializeField] private GameObject load_screens_ref;
    [SerializeField] private GameObject transition_UI;
    [SerializeField] private GameObject loading_ref;

    //Each time an artefact is collected, move this trigger elsewhere and reset it
    [SerializeField] private Transform[] spawn_points;

    private void Start() 
    {
        //Reposition the hub trigger to next artefact
        //Check how many artefacts have been collected and then choose the next spawn point.
        total_artefact_collected = GameManager.GetArtefactCounter();
        transform.position = spawn_points[total_artefact_collected].position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!is_loading && other.tag == "Player" && GameManager.GetHUBTravel() && GameManager.GetCurrentArtefactCollected())
        {
            //If the player has picked up the artefact then they can return to the HUB world. Initiate loading
            StartCoroutine(LoadHubScene());
        }
    }

    private IEnumerator LoadHubScene()
    {
        transition.SetTrigger("Start");

        transition_UI.SetActive(true);

        yield return new WaitForSeconds(transition_t);

        load_screens_ref.SetActive(true);
        loading_ref.SetActive(true);

        is_loading = true;
        //yield return null; //Wait for next frame

        yield return new WaitForSeconds(6f);

        // AsyncOperation async_load = SceneManager.LoadSceneAsync(hub_scene_name, LoadSceneMode.Single);

        SceneManager.LoadScene(hub_scene_name);

        // async_load.allowSceneActivation = false;

        // while(!async_load.isDone)
        // {
        //     if (async_load.progress >= 0.9f)
        //     {
        //         async_load.allowSceneActivation = true;
        //     }
        //     //Display loading progress? Possibly keep player walking down endless corridor
        //     yield return null;
        // }

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(hub_scene_name));

        is_loading = false;
    }
}

/*Cal's code ends here*/