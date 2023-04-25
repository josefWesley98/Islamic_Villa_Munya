using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LoadScreens : MonoBehaviour
{
    //Images for the loading screen
    [SerializeField] private Sprite[] load_screen;
    [SerializeField] private float switch_time = 1f;
    private float counter = 0f;
    private Image image_ref;
    private int random_num = 0;
    private bool run = true;

    // Start is called before the first frame update
    void Start()
    {
        image_ref = gameObject.GetComponent<Image>();
        image_ref.sprite = load_screen[0];
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
        random_num = Random.Range(0, load_screen.Length);
        Debug.Log("Being called here BABY!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        //Wait for this amount of time before changing the image
        yield return new WaitForSeconds(switch_time);
        Debug.Log("Being called here AGAIN BABY!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        //Get a random index and use this for the next image

        //Switch to the next image
        image_ref.sprite = load_screen[random_num];
        run = true;
    }
}
