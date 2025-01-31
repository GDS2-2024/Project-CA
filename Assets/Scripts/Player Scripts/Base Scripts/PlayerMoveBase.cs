using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveBase : MonoBehaviour
{

    private playerState currentState;

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
        moveZ = Input.GetAxisRaw("Vertical");
        moveX = Input.GetAxisRaw("Horizontal");

        moveDir = playerCam.transform.TransformDirection(new Vector3(moveX, 0f, moveZ).normalized);
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
        mouseX = Input.GetAxis("Mouse X") * cameraSens;
        mouseY = Input.GetAxis("Mouse Y") * cameraSens;

        cameraYaw += mouseX;

        cameraPitch += mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, minClamp, maxClamp);

        Quaternion rotation = Quaternion.Euler(cameraPitch, cameraYaw, 0);
        Vector3 newPos = transform.position + rotation * cameraOffset;

        playerCam.transform.position = newPos;

        //rotate the character the face the x direction of the camera
        transform.rotation = Quaternion.Euler(0, cameraYaw, 0);

        Vector3 rightOffsetVec = playerCam.transform.right * rightOffset;
        Vector3 heightOffsetVec = playerCam.transform.up * heightOffset;
        playerCam.transform.LookAt(transform.position + rightOffsetVec + heightOffsetVec);
    }
}
