using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveBase : MonoBehaviour
{
    private playerState currentState;

    //Controller
    private InputDevice thisController;
    private PlayerController controllerScript;
    
    //Inputs
    private float inputX;
    private float inputY;
    public float mouseSens;
    public float controlXSens;
    public float controlYSens;

    //Movement variables
    private Rigidbody rb;
    private CapsuleCollider playerCollider;
    private Vector3 moveDir;
    public float moveSpeed;
    public float jumpForce;
    public bool isGrounded { get; private set; }
    public bool movementDisabled = false;

    //Camera variables
    public Camera playerCam;
    private float cameraPitch;
    private float cameraYaw;
    private float minClamp = -89.0f;
    private float maxClamp = 89.0f;

    //Camera offsests
    public float distance;
    public float offsetDistance;
    public float heightOffset; 

    public enum playerState
    {
        Idle,
        Move
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = playerState.Idle;

        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        controllerScript = gameObject.GetComponent<PlayerController>();
        thisController = controllerScript.GetController();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        MoveDirection();
        HandleCamera();
    }

    private void FixedUpdate()
    {
        CheckIsGrounded();
        Move();
    }

    public void CheckIsGrounded()
    {
        RaycastHit hit;
        float raycastDistance = playerCollider.height / 2 + 0.1f;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance);
    }

    public void SetState(playerState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
            OnStateEnter(newState);
        }
    }

    public void OnStateEnter(playerState state)
    {
        switch (state)
        {
            case playerState.Idle:
                break;
            case playerState.Move:
                break;
        }
    }

    void MoveDirection()
    {
        float moveX = 0;
        float moveZ = 0;
        if (thisController is Keyboard keyboard)
        {
            moveZ = keyboard.wKey.isPressed ? 1 : keyboard.sKey.isPressed ? -1 : 0;
            moveX = keyboard.dKey.isPressed ? 1 : keyboard.aKey.isPressed ? -1 : 0;
            if (keyboard.spaceKey.wasPressedThisFrame) { Jump(); }
        }
        else if (thisController is Gamepad controller)
        {
            moveZ = controller.leftStick.up.isPressed ? 1 : controller.leftStick.down.isPressed ? -1 : 0;
            moveX = controller.leftStick.right.isPressed ? 1 : controller.leftStick.left.isPressed ? -1 : 0;
            if (controller.buttonSouth.wasPressedThisFrame) { Jump(); }
        }

        moveDir = transform.TransformDirection(new Vector3(moveX, 0f, moveZ).normalized);
        moveDir.y = 0;

        if (moveDir.z > 0 || moveDir.x > 0)
        {
            SetState(playerState.Move);
        }
        else
        {
            SetState(playerState.Idle);
        }
    }

    private void Jump()
    {
        if (!isGrounded) { return; }
        rb.AddForce(Vector3.up * 10 * jumpForce, ForceMode.Impulse);
    }

    void Move()
    {
        if (movementDisabled) { return; }
        Vector3 desiredVelocity = moveDir * moveSpeed;

        if (moveDir.magnitude > 0 && isGrounded)
        {           
            // If move input then add to existing velocity
            Vector3 velocityChange = desiredVelocity - new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(velocityChange, ForceMode.VelocityChange);
        } else if (moveDir.magnitude == 0 && isGrounded)
        {
            // If no input and grounded, apply friction to slow down movement
            Vector3 dampedVelocity = new Vector3(rb.velocity.x * 0.85f, rb.velocity.y, rb.velocity.z * 0.85f);
            rb.velocity = new Vector3(dampedVelocity.x, rb.velocity.y, dampedVelocity.z);
        }
        // If the player is in the air, this class does not manipulate velocity
    }

    void HandleCamera()
    {
        //Gets input from the input device
        if (thisController is Keyboard)
        {
            Mouse mouse = Mouse.current;
            inputX = mouse.delta.x.ReadValue() * mouseSens;
            inputY = mouse.delta.y.ReadValue() * mouseSens;
        }
        else if (thisController is Gamepad controller)
        {
            inputX = controller.rightStick.ReadValue().x * controlXSens;
            inputY = controller.rightStick.ReadValue().y * controlYSens;
        }

        // Update yaw and pitch
        cameraYaw += inputX;
        cameraPitch -= inputY;
        cameraPitch = Mathf.Clamp(cameraPitch, minClamp, maxClamp);

        // Update camera position and rotation
        Quaternion rotation = Quaternion.Euler(cameraPitch, cameraYaw, 0);
        Vector3 targetPos = transform.position + transform.right * offsetDistance + Vector3.up * heightOffset;
        Vector3 cameraPos = rotation * new Vector3(0, 0, -distance) + targetPos;
        playerCam.transform.position = cameraPos;
        playerCam.transform.rotation = rotation;
        playerCam.transform.LookAt(transform.position + transform.right * offsetDistance + Vector3.up * heightOffset);
        
        // Rotate player horizontally to face direction of the camera's yaw
        transform.rotation = Quaternion.Euler(0, cameraYaw, 0);
    }

    public void TempDisableMovement(float duration)
    {
        StartCoroutine(DisableMovement(duration));
    }

    private IEnumerator DisableMovement(float duration)
    {
        movementDisabled = true;
        yield return new WaitForSeconds(duration);
        movementDisabled = false;
    }
}
