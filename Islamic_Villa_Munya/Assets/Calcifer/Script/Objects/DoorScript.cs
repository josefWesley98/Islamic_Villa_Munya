using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*Cal's script starts here*/
public class DoorScript : MonoBehaviour
{
    public float open_angle = 70f;

    public Transform pivot;
    bool is_open = false;

    private Keyboard kb;

    //Open the door if the player has the key
    private void Start() 
    {
        kb = InputSystem.GetDevice<Keyboard>();
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log(GameManager.GetHaveKey());
            if(GameManager.GetHaveKey() && kb.spaceKey.isPressed)
            {
                Debug.Log("should be opening");
                pivot.Rotate(Vector3.up, open_angle);
                is_open = true;
            }
        }
        
    }
}

/*Cal's script ends here*/
