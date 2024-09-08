using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{
    #region Variables

    [Header("Player Data")]
    PlayerInputActions playerInput;
    PlayerMovement playerMovement;
    Camera playerCam;

    [Header("Raycast Data")]
    Ray pointerRay;
    RaycastHit hit;
    bool didPointerHit;
    Interactable currInteractable;
    bool canInteract = false;

    [SerializeField]
    TMP_Text interactMessage;

    [Header("User Interface")]
    [SerializeField]
    GameObject pauseMenu;

    #endregion

    #region Weapon Data


    #endregion

    #region Properties

    public PlayerInputActions GetInputActions
    {
        get { return playerInput; }
    }

    public string SetInteractMsg
    {
        set { interactMessage.text = value; }
    }

    public Ray PointerRay
    {
        get { return pointerRay; }
        set { pointerRay = value; }
    }

    public bool GetPointerHit
    {
        get { return didPointerHit; }
    }

    public RaycastHit GetOutHit
    {
        get { return hit; }
    }

    #endregion

    #region Delegates

    public delegate void OnHolster(bool val);
    public OnHolster onHolster;
    public delegate void OnUseWeapon();
    public OnUseWeapon onUseWeapon;

    #endregion

    #region Event Handlers

    private void OnEnable()
    {
        playerInput.Enable();

        // playerInput.OnFoot.Holster.performed += OnWeaponHolster;
        // playerInput.OnFoot.Interaction.performed += OnInteract;
        // playerInput.OnFoot.InventorySystem.performed += ToggleInventory;
        // playerInput.OnFoot.Fire.started += UseWeapon;
    }

    private void OnDisable()
    {
        // playerInput.OnFoot.Holster.performed -= OnWeaponHolster;
        // playerInput.OnFoot.Interaction.performed -= OnInteract;
        // playerInput.OnFoot.InventorySystem.performed -= ToggleInventory;
        // playerInput.OnFoot.Fire.started -= UseWeapon;

        playerInput.Disable();
    }

    #endregion

    #region Update Functions

    private void Update()
    {
        pointerRay = playerCam.ScreenPointToRay(Mouse.current.position.ReadValue());

        didPointerHit = Physics.Raycast(pointerRay, out hit);

        HandleInteraction();
    }

    private void HandleInteraction()
    {
        GameObject hitObject = hit.transform.gameObject;

        if (didPointerHit)
        {
            if (hitObject.tag == "InteractableItem") { }
        }
        else
        {
            interactMessage.text = "";
        }
    }

    #endregion

    #region Start Functions

    private void Awake()
    {
        playerInput = new PlayerInputActions();
        playerCam = Camera.main;
        playerMovement = GetComponent<PlayerMovement>();
    }

    #endregion

    #region Base Functions


    #endregion
}
