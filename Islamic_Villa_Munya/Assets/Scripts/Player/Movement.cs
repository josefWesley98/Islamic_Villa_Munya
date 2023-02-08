using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    PlayerControls playercontrols;
    InputAction movement;
    Rigidbody rb;
    private bool isMoving = false;

    [SerializeField]float moveSpeed = 5.0f;
    // Start is called before the first frame update
    //InputAction.CallbackContext context
    private void Awake() => playercontrols = new PlayerControls();
    
    void Start()
    {
       rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        playercontrols.Enable();
        
        movement = playercontrols.Player.Move;
        movement.Enable();

    }

    private void OnDisable()
    {
        playercontrols.Disable();
        movement.Disable();
      
    }
    // Update is called once per frame
    private void Update()
    {
       
    }
    private void FixedUpdate()
    {
        DoMovement();
        DoRotation();
    }

    private void DoMovement()
    {
        Vector2 moveDirection = movement.ReadValue<Vector2>();

        if(moveDirection.x > 0)
        {
            rb.AddForce(moveSpeed * transform.right * 10.0f, ForceMode.Acceleration);
            isMoving = true;
        }
        else if(moveDirection.x < 0)
        {
            rb.AddForce(moveSpeed * -transform.right * 10.0f, ForceMode.Acceleration);
            isMoving = true;
        }
        if(moveDirection.y > 0)
        {
            rb.AddForce(moveSpeed * transform.forward * 10.0f, ForceMode.Acceleration);
            isMoving = true;
        }
        else if(moveDirection.y < 0)
        {
            rb.AddForce(moveSpeed * -transform.forward * 10.0f, ForceMode.Acceleration);
            isMoving = true;
        }
    }
    private void Deceleration()
    {
        if(!isMoving)
        {
            
        }
    }
    private void DoRotation()
    {

    }
  
}
