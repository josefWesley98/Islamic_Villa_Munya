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
                Debug.Log("Selected boy");
                boy_cam.SetActive(true);
                player_boy.SetActive(true);
            }
            else if(GameManager.GetGirl())
            {
                //Enable the girl model
                Debug.Log("Selected girl");
                girl_cam.SetActive(true);
                player_girl.SetActive(true);
            }
        }
    }
}
/*Cal's code ends here*/
