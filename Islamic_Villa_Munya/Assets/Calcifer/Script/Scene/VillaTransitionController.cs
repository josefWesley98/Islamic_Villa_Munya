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
    [SerializeField] private float transition_t = 2f;

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

        yield return new WaitForSeconds(transition_t);

        is_loading = true;
        yield return null; //Wait for next frame

        AsyncOperation async_load = SceneManager.LoadSceneAsync(villa_scene_name, LoadSceneMode.Additive);

        while(!async_load.isDone)
        {
            //Display loading progress? Possibly keep player walking down endless corridor
            yield return null;
        }

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(villa_scene_name));

        is_loading = false;
    }
}

/*Cal's code ends here*/