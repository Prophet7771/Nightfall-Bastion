using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Private Variables

    [Header("Player Data")]
    PlayerCharacter player;
    PlayerInputActions playerInput;
    Rigidbody rb;
    Camera playerCam;
    bool isIdle = true,
        isAiming = true;

    [Header("Raycast Data")]
    Ray aimRay;
    RaycastHit hit;
    Vector2 mousePos;

    [SerializeField]
    private float moveSpeed = 8f;

    [SerializeField]
    private GameObject playerMesh;

    [Header("Cam Move Values")]
    float camSpeed = 2f;

    #endregion

    #region Animation Movement When Aiming - Variables

    Transform camTransform;

    Vector3 camForward;
    Vector3 move;
    Vector3 moveInput;

    float forwardAmount;
    float turnAmount;

    #endregion

    #region Getters & Setters

    public bool GetIsAiming
    {
        get { return isAiming; }
    }

    #endregion

    #region Delegates

    public delegate void RunningState(bool val);
    public RunningState runningState;
    public delegate void AimState(bool val);
    public AimState aimState;
    public delegate void Locomotion(float x, float y);
    public Locomotion locomotion;

    #endregion

    private void Awake()
    {
        player = GetComponent<PlayerCharacter>();
        playerInput = new PlayerInputActions();
        rb = GetComponent<Rigidbody>();
        playerCam = Camera.main;

        camTransform = playerCam.transform;
        // camTransform = transform;
    }

    #region Event Handlers

    private void OnEnable()
    {
        playerInput.Enable();

        playerInput.OnFoot.MoveCamera.performed += MoveCam;
        playerInput.OnFoot.Aim.started += OnAimStart;
        playerInput.OnFoot.Aim.canceled += OnAimStop;
    }

    private void OnDisable()
    {
        playerInput.OnFoot.MoveCamera.performed -= MoveCam;
        playerInput.OnFoot.Aim.started -= OnAimStart;
        playerInput.OnFoot.Aim.canceled -= OnAimStop;

        playerInput.Disable();
    }

    #endregion

    private void Update()
    {
        Vector3 localForward = Camera.main.transform.localRotation * Vector3.forward;

        localForward.y = 0;
        localForward.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(localForward);

        playerMesh.transform.rotation = targetRotation;

        playerCam.transform.LookAt(playerCam.transform.parent);

        Vector3 lookToward = new Vector3(camTransform.forward.x, 0, camTransform.forward.z);

        playerMesh.transform.LookAt(playerMesh.transform.position + lookToward);
    }

    void FixedUpdate()
    {
        moveInput = playerInput.OnFoot.Movement.ReadValue<Vector3>();
        isIdle = moveInput == Vector3.zero;

        #region Animation Movement When Aiming - Fixed Update

        if (camTransform != null)
        {
            camForward = Vector3.Scale(camTransform.forward, new Vector3(1, 0, 1)).normalized;
            move = moveInput.z * camForward + moveInput.x * camTransform.right;
        }
        else
            move = moveInput.z * Vector3.forward + moveInput.x * Vector3.right;

        if (move.magnitude > 1)
            move.Normalize();

        Move(move);

        #endregion

        runningState?.Invoke(!isIdle);
        aimState?.Invoke(isAiming);

        rb.velocity = moveInput.normalized * moveSpeed * Time.fixedDeltaTime; // If character spins, freeze the rotation on all axis
    }

    #region Animation Movement When Aiming - Functions

    private void Move(Vector3 move)
    {
        if (move.magnitude > 1)
            move.Normalize();

        this.moveInput = move;

        ConvertMoveInput();
        UpdateAnimator();
    }

    private void ConvertMoveInput()
    {
        Vector3 localMove = playerMesh.transform.InverseTransformDirection(moveInput);
        turnAmount = localMove.x;

        forwardAmount = localMove.z;
    }

    private void UpdateAnimator() => locomotion?.Invoke(turnAmount, forwardAmount); // Just Testing
    #endregion

    private void OnAimStart(InputAction.CallbackContext ctx) => isAiming = true;

    private void OnAimStop(InputAction.CallbackContext ctx) => isAiming = false;

    private void MoveCam(InputAction.CallbackContext ctx)
    {
        float mouseX = ctx.ReadValue<Vector2>().x;
        float mouseY = ctx.ReadValue<Vector2>().y;

        Vector3 yRotation = new Vector3(0, mouseY * Time.deltaTime * camSpeed, 0);

        playerCam.transform.position += -yRotation;

        // Clamp the camera's y position between -1.5f and 1.5f
        Vector3 clampedPosition = playerCam.transform.position;
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, 0f, 5f);

        // Set the clamped position back to the camera
        playerCam.transform.position = clampedPosition;
        transform.Rotate(new Vector3(0, mouseX * Time.deltaTime * camSpeed, 0));
    }
}
