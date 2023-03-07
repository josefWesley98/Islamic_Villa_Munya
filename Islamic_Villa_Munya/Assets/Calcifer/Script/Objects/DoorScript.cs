using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*Cal's script starts here*/
public class DoorScript : MonoBehaviour
{
    private Keyboard kb;
    public Transform doorTransform;  // The transform of the door object
    public float openAngle = 90f;    // The angle to open the door

    private bool isOpen = false;     // Whether the door is currently open

    private void OnCollisionEnter(Collision other)
    {
        if (!isOpen && other.gameObject.tag == "Player")
        {
            Debug.Log(GameManager.GetHaveKey());
            if(GameManager.GetHaveKey())
            {
            Debug.Log("trigger");
            // Open the door
            doorTransform.Rotate(Vector3.up, openAngle);
            isOpen = true;
            }
        }
    }
}

/*Cal's script ends here*/
