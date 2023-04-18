using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Cal's code starts here*/
public class ActivatePlayer : MonoBehaviour
{
    private bool activate = false;
    [SerializeField] private GameObject player_boy;
    [SerializeField] private GameObject player_girl;
    [SerializeField] private GameObject girl_cam;
    [SerializeField] private GameObject boy_cam;

    // Update is called once per frame
    void Update()
    {
        if(!activate)
        {
            activate = true;

            //Enable the relevant player and cameras
            if(GameManager.GetBoy())
            {
                //Enable the boy model
                boy_cam.SetActive(true);
                player_boy.SetActive(true);

                //Disable Girl model
                girl_cam.SetActive(false);
                player_girl.SetActive(false);

            }
            else if(GameManager.GetGirl())
            {
                //Enable the girl model
                girl_cam.SetActive(true);
                player_girl.SetActive(true);

                //Disable Boy model
                boy_cam.SetActive(false);
                player_boy.SetActive(false);
            }
        }
    }
}
/*Cal's code ends here*/
