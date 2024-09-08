using fLibrary;
using ItemTypes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    #region Variables

    [Header("Interaction")]
    [SerializeField]
    string interactionMessage = "Press 'E' to interact";

    [SerializeField]
    InventoryItem itemData;

    [Header("VFX")]
    [SerializeField]
    Outline outline;

    WeaponType weaponType;
    PlayerCharacter player;
    PlayerInputActions inputAction;

    #endregion

    #region Getters & Setters

    public string GetInteractMsg
    {
        get { return interactionMessage; }
    }

    #endregion

    #region Unity Events

    public delegate void OnInteract();
    public OnInteract onInteract;

    #endregion

    #region Event Handlers

    private void Awake()
    {
        if (!itemData)
        {
            itemData = GetComponent<InventoryItem>();
            weaponType = itemData.GetWeaponData.weaponType;
        }
    }

    private void OnEnable() { }

    private void OnDisable()
    {
        if (inputAction == null)
            return;
    }

    #endregion

    #region Basic Functions

    public void SetPlayer(PlayerCharacter val)
    {
        player = val;
    }

    public void Interact()
    {
        switch (itemData.GetItemType)
        {
            case ItemType.Weapon:
                // if (!player.GetEquipedWeapon)
                //     player.EquipWeapon(weaponType, itemData.GetPrefabName);
                break;
            case ItemType.Consumable:
                break;
            case ItemType.Collectable:
                break;
            default:
                break;
        }

        player.GetComponent<InventorySystem>().AddToInventory(new InventoryItem(itemData));

        gameObject.SetActive(false);
        player.SetInteractMsg = "";
        Invoke("DestroySelf", 2f);
    }

    private void DestroySelf() => Destroy(gameObject);

    #endregion

    #region Hover Visuals

    public void HoverVisual(bool val)
    {
        if (val)
        {
            outline.OutlineColor = new Color(0, 255, 0, 255);
            outline.OutlineMode = Outline.Mode.OutlineAll;
        }
        else
        {
            outline.OutlineColor = new Color(255, 255, 255, 0);
            outline.OutlineMode = Outline.Mode.OutlineHidden;
        }
    }

    public void HoverVisual(bool val, bool canInteract)
    {
        if (val)
        {
            if (canInteract)
                outline.OutlineColor = new Color(0, 255, 0, 255);
            else
                outline.OutlineColor = new Color(255, 0, 0, 255);

            outline.OutlineMode = Outline.Mode.OutlineAll;
        }
        else
        {
            outline.OutlineColor = new Color(255, 255, 255, 0);
            outline.OutlineMode = Outline.Mode.OutlineHidden;
        }
    }

    #endregion
}
