using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

/*Cal's code starts here*/

//This is a test for loading as seamlessly as possible between the hub and the villa vice versa
//Dynamic Level loading
public class HUBTransitionController : MonoBehaviour
{
    public string hub_scene_name;
    private bool is_loading = false;

    private void OnTriggerEnter(Collider other)
    {
        if(!is_loading && other.tag == "Player" && GameManager.GetHUBTravel())
        {
            //If the player has picked up the artefact then they can return to the HUB world. Initiate loading
            StartCoroutine(LoadHubScene());
        }
    }

    private IEnumerator LoadHubScene()
    {
        is_loading = true;
        yield return null; //Wait for next frame

        AsyncOperation async_load = SceneManager.LoadSceneAsync(hub_scene_name, LoadSceneMode.Additive);

        while(!async_load.isDone)
        {
            //Display loading progress? Possibly keep player walking down endless corridor
            yield return null;
        }

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(hub_scene_name));

        is_loading = false;
    }
}

/*Cal's code ends here*/