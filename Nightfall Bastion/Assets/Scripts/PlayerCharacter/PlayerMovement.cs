using System;
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
        isAiming = false;

    [Header("Raycast Data")]
    Ray aimRay;
    RaycastHit hit;
    Vector2 mousePos;

    [SerializeField]
    private float moveSpeed = 8f;

    [SerializeField]
    private GameObject playerMesh;

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
    }

    #region Event Handlers

    private void OnEnable()
    {
        playerInput.Enable();

        playerInput.OnFoot.MousePosition.performed += OnMouseMove;
        playerInput.OnFoot.Aim.started += OnAimStart;
        playerInput.OnFoot.Aim.canceled += OnAimStop;
    }

    private void OnDisable()
    {
        playerInput.OnFoot.MousePosition.performed -= OnMouseMove;
        playerInput.OnFoot.Aim.started -= OnAimStart;
        playerInput.OnFoot.Aim.canceled -= OnAimStop;

        playerInput.Disable();
    }

    #endregion

    private void Update()
    {
        if (isAiming)
        {
            // aimRay = playerCam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (player.GetPointerHit)
            {
                Vector3 lookTowards = new Vector3(
                    player.GetOutHit.point.x - playerMesh.transform.position.x,
                    0,
                    player.GetOutHit.point.z - playerMesh.transform.position.z
                );

                playerMesh.transform.rotation = Quaternion.Slerp(
                    playerMesh.transform.rotation,
                    Quaternion.LookRotation(lookTowards),
                    15 * Time.deltaTime
                );
            }
        }
        else
        {
            if (!isIdle)
                playerMesh.transform.rotation = Quaternion.Slerp(
                    playerMesh.transform.rotation,
                    Quaternion.LookRotation(moveInput),
                    15 * Time.deltaTime
                );
        }
    }

    void FixedUpdate()
    {
        moveInput = playerInput.OnFoot.Movement.ReadValue<Vector3>();
        isIdle = moveInput == Vector3.zero;

        #region Animation Movement When Aiming - Fixed Update

        if (isAiming)
        {
            if (camTransform != null)
            {
                camForward = Vector3.Scale(camTransform.up, new Vector3(1, 0, 1)).normalized;
                move = moveInput.z * camForward + moveInput.x * camTransform.right;
            }
            else
                move = moveInput.z * Vector3.forward + moveInput.x * Vector3.right;

            if (move.magnitude > 1)
                move.Normalize();

            Move(move);
        }

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

    private void OnMouseMove(InputAction.CallbackContext ctx) =>
        mousePos = playerCam.ScreenToWorldPoint(ctx.ReadValue<Vector2>());
}
