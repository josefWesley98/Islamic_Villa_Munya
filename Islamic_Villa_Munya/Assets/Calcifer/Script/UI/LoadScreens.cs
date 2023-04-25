using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LoadScreens : MonoBehaviour
{
    //Images for the loading screen
    [SerializeField] private Sprite[] load_screen;
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

        //Wait for this amount of time before changing the image
        yield return new WaitForSeconds(3);

        //Get a random index and use this for the next image
        random_num = Random.Range(0, load_screen.Length);

        //Switch to the next image
        image_ref.sprite = load_screen[random_num];
        run = true;
    }
}
