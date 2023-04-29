using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LoadScreens : MonoBehaviour
{
    //Images for the loading screen
    [SerializeField] private Sprite[] load_screen;
    [SerializeField] private float switch_time;
    private Image image_ref;
    private int rand_num = 0;
    private bool run = true;

    // Start is called before the first frame update
    void Start()
    {
        image_ref = gameObject.GetComponent<Image>();
        rand_num = Random.Range(0, load_screen.Length);
        image_ref.sprite = load_screen[rand_num];
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
        //Wait for this amount of time before changing the image
        yield return new WaitForSeconds(switch_time);

        //Get a random index and use this for the next image
        rand_num = Random.Range(0, load_screen.Length);

        //Switch to the next image
        image_ref.sprite = load_screen[rand_num];
        run = true;
    }

    public float GetSwitchTime()
    {
        return switch_time;
    }
}
