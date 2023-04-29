using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*Cal's script starts here*/
public class LoadingMessages : MonoBehaviour
{
    //Array for the loading screen messages
    [SerializeField] TMP_Text[] messages;
    private bool run = true;
    private int rand_num = 0;
    private TMP_Text text_ref;
    [SerializeField] LoadScreens loading_ref;

    private void Start() 
    {
        //Reference to the text component that we assign the messages to
        text_ref = gameObject.GetComponent<TMP_Text>();

        rand_num = Random.Range(0, messages.Length);
        text_ref.text = messages[rand_num].text;
    }

    // Update message after every few seconds.
    void Update()
    {
        if(run)
        {
            Debug.Log(text_ref.text);
            Debug.Log("Shall we switch message?");
            run = false;
            StartCoroutine(SwitchMessage());
        }
    }

    private IEnumerator SwitchMessage()
    {
        //Wait this long before changing message
        yield return new WaitForSeconds(loading_ref.GetSwitchTime());

        //Get a random number for selecting the next message to display
        rand_num = Random.Range(0, messages.Length);

        //Switch to the next message
        if(text_ref != null)
        {
            text_ref.text = messages[rand_num].text;
        }

        //Allow updating of text
        run = true;
    }
}
/*Cal's script ends here*/