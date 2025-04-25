using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveBase : MonoBehaviour
{
    //Controller
    private InputDevice thisController;
    
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
    private bool movementDisabled = false;

    //Camera variables
    public Camera playerCam;
    private float cameraPitch;
    private float cameraYaw;
    private float minClamp = -89.0f;
    private float maxClamp = 89.0f;
    private bool cameraDisabled = false;

    //Camera offsests
    private float camDepthOffset = 3f;
    private float camSideOffset = 1f;
    private float camHeightOffset = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        thisController = GetComponent<PlayerController>().GetController();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetMovementInput();
        HandleCamera();
    }

    private void FixedUpdate()
    {
        CheckIsGrounded();
        HandleMovement();
    }

    public void CheckIsGrounded()
    {
        RaycastHit hit;
        float raycastDistance = playerCollider.height / 2 + 0.1f;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance);
    }

    void GetMovementInput()
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
    }

    private void Jump()
    {
        if (!isGrounded) { return; }
        rb.AddForce(Vector3.up * 10 * jumpForce, ForceMode.Impulse);
    }

    void HandleMovement()
    {
        if (movementDisabled) { return; }
        Vector3 desiredVelocity = moveDir * moveSpeed;
        if (moveDir.magnitude > 0)
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
        if (cameraDisabled) { return; }
        if (thisController is Keyboard)
        {
            Mouse mouse = Mouse.current;
            inputX = mouse.delta.x.ReadValue() * mouseSens;
            inputY = mouse.delta.y.ReadValue() * mouseSens;
        }
        else if (thisController is Gamepad controller)
        {
            inputX = controller.rightStick.ReadValue().x * controlXSens * Time.deltaTime;
            inputY = controller.rightStick.ReadValue().y * controlYSens * Time.deltaTime;
        }
        // Update yaw and pitch
        cameraYaw += inputX;
        cameraPitch -= inputY;
        cameraPitch = Mathf.Clamp(cameraPitch, minClamp, maxClamp);

        // Update camera position and rotation
        Quaternion rotation = Quaternion.Euler(cameraPitch, cameraYaw, 0);
        Vector3 targetPos = transform.position + transform.right * camSideOffset + Vector3.up * camHeightOffset;
        Vector3 cameraPos = rotation * new Vector3(0, 0, -camDepthOffset) + targetPos;
        playerCam.transform.position = cameraPos;
        playerCam.transform.LookAt(transform.position + transform.right * camSideOffset + Vector3.up * camHeightOffset);
        
        // Rotate player horizontally
        transform.Rotate(Vector3.up * inputX, Space.World);
    }

    public void InitializeCameraRotation(float initialYaw, float initialPitch)
    {
        cameraYaw = initialYaw;
        cameraPitch = initialPitch;
    }

    // Temporary Disable/Enable
    public void TempDisableMovement(float duration)
    {
        StartCoroutine(TempAction(duration, () => movementDisabled = true, () => movementDisabled = false));
    }

    public void TempDisableCamera(float duration)
    {
        StartCoroutine(TempAction(duration, () => cameraDisabled = true, () => cameraDisabled = false));
    }

    public void TempSlowMovement(float duration)
    {
        StartCoroutine(TempAction(duration, () => moveSpeed *= 0.5f, () => moveSpeed *= 2f));
    }

    // Manual control methods
    public void DisableMovement() => movementDisabled = true;
    public void EnableMovement() => movementDisabled = false;

    public void DisableCamera() => cameraDisabled = true;
    public void EnableCamera() => cameraDisabled = false;
    
    // General utility coroutine
    private IEnumerator TempAction(float duration, Action onStart, Action onEnd)
    {
        onStart?.Invoke();
        yield return new WaitForSeconds(duration);
        onEnd?.Invoke();
    }
}
