using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LoadScreens : MonoBehaviour
{
    //Images for the loading screen
    [SerializeField] private Sprite[] load_screen;
    [SerializeField] private float switch_time;
    private Image otherWillys;
    private int willys = 0;
    private bool run = true;

    // Start is called before the first frame update
    void Start()
    {
        otherWillys = gameObject.GetComponent<Image>();
        otherWillys.sprite = load_screen[0];
    }

    // Update is called once per frame
    void Update()
    {


        //After a certain amount of time, change the image
        if(run)
        {
            run = false;
            StartCoroutine(SwitchImage());
        }
    }

    private IEnumerator SwitchImage()
    {
        Debug.Log("Being called here BABY!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        //Wait for this amount of time before changing the image
        
        yield return new WaitForSeconds(switch_time);

        Debug.Log("Being called here AGAIN BABY!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        
        //Get a random index and use this for the next image
        willys = Random.Range(0, load_screen.Length);

        //Switch to the next image
        otherWillys.sprite = load_screen[willys];
        run = true;
    }
}
