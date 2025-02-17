using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveBase : MonoBehaviour
{

    private playerState currentState;

    //Controller
    private InputDevice thisController;
    private PlayerController controllerScript;

    //Movement variables
    private Rigidbody rb;
    private Vector3 moveDir;
    private float moveX;
    private float moveZ;

    public float moveSpeed;
    public float jumpHeight;

    //Camera variables
    private float mouseX;
    private float mouseY;
    private float cameraPitch;
    private float cameraYaw;
    private float minClamp = 30f;
    private float maxClamp = 120f;


    public Camera playerCam;
    public float cameraSens;
    public Vector3 cameraOffset;
    public float rightOffset;
    public float heightOffset;
    public float smoothSpeed;

    public enum playerState
    {
        Idle,
        Move
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        currentState = playerState.Idle;

        controllerScript = gameObject.GetComponent<PlayerController>();
        thisController = controllerScript.GetController();
    }

    // Update is called once per frame
    void Update()
    {
        MoveDirection();

        HandleCamera();
    }

    private void FixedUpdate()
    {
        Move();
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
        //gets vertical and horizontal input from the input device
        if (thisController is Keyboard keyboard)
        {
            moveZ = keyboard.wKey.isPressed ? 1 : keyboard.sKey.isPressed ? -1 : 0;
            moveX = keyboard.dKey.isPressed ? 1 : keyboard.aKey.isPressed ? -1 : 0;
        }
        else if (thisController is Gamepad controller)
        {
            moveZ = controller.leftStick.up.isPressed ? 1 : controller.leftStick.down.isPressed ? -1 : 0;
            moveX = controller.leftStick.right.isPressed ? 1 : controller.leftStick.left.isPressed ? -1 : 0;
        }


        //move direction is based on the direction the camera is facing
        moveDir = playerCam.transform.TransformDirection(new Vector3(moveX, 0f, moveZ).normalized);
        //Stops players from running into the air
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

    void Move()
    {
        Vector3 newVelocity = moveDir * moveSpeed;
        rb.velocity = new Vector3(newVelocity.x, rb.velocity.y, newVelocity.z);
    }

    void HandleCamera()
    {
        //Gets input from the input device
        if (thisController is Keyboard)
        {
            Mouse mouse = Mouse.current;
            mouseX = mouse.delta.x.ReadValue() * cameraSens;
            mouseY = mouse.delta.y.ReadValue() * cameraSens;
        }
        else if (thisController is Gamepad controller)
        {
            mouseX = controller.rightStick.right.isPressed ? 1 * cameraSens : controller.rightStick.left.isPressed ? -1 * cameraSens : 0;
            mouseY = controller.rightStick.up.isPressed ? 1 * cameraSens : controller.rightStick.down.isPressed ? -1 * cameraSens : 0;
        }

        cameraYaw += mouseX;

        cameraPitch += mouseY;
        //stops the player from looking too high or too low
        cameraPitch = Mathf.Clamp(cameraPitch, minClamp, maxClamp);

        //Rotate the camera around the player
        Quaternion rotation = Quaternion.Euler(cameraPitch, cameraYaw, 0);
        Vector3 newPos = transform.position + rotation * cameraOffset;

        playerCam.transform.position = newPos;

        //rotate the character the face the x direction of the camera
        transform.rotation = Quaternion.Euler(0, cameraYaw, 0);

        //offsets ensure that the character isn't positioned in the middle of the screen for the players POV
        Vector3 rightOffsetVec = playerCam.transform.right * rightOffset;
        Vector3 heightOffsetVec = playerCam.transform.up * heightOffset;
        //ensure that the camera is always looking at the player
        playerCam.transform.LookAt(transform.position + rightOffsetVec + heightOffsetVec);
    }
}
