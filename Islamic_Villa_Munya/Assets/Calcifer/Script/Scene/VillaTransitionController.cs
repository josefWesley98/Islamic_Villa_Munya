using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

/*Cal's code starts here*/

//This is a test for loading as seamlessly as possible between the hub and the villa vice versa
//Dynamic Level loading
public class VillaTransitionController : MonoBehaviour
{
    public string villa_scene_name;
    private bool is_loading = false;
    [SerializeField] private Animator transition;
    [SerializeField] private float transition_t = 10f;
    [SerializeField] private GameObject load_screens_ref;
    [SerializeField] private GameObject transition_UI;
    [SerializeField] private GameObject loading_ref;

    private void OnTriggerEnter(Collider other)
    {
        //Set the current artefact to not collected here so when the player collides with the travel hit box, nothing happens before getting next artefact
        GameManager.SetCurrentArtefactCollected(false);
        
        if(!is_loading && other.tag == "Player")// && !GameManager.GetHaveKey())
        {
            StartCoroutine(LoadVillaScene());
        }
      
    }

    private IEnumerator LoadVillaScene()
    {
        transition.SetTrigger("Start");

        transition_UI.SetActive(true);

        yield return new WaitForSeconds(transition_t);

        load_screens_ref.SetActive(true);
        loading_ref.SetActive(true);

        is_loading = true;
        //yield return null; //Wait for next frame



        yield return new WaitForSeconds(transition_t);

        // AsyncOperation async_load = SceneManager.LoadSceneAsync(villa_scene_name, LoadSceneMode.Single);

        SceneManager.LoadScene(villa_scene_name);
        Debug.Log("Loading");

        //  async_load.allowSceneActivation = false;
        
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
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(villa_scene_name));

        is_loading = false;
    }
}

/*Cal's code ends here*/