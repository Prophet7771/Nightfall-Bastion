using UnityEngine;
using ItemTypes;

public class InventoryItem : MonoBehaviour
{
    #region Constructor

    public InventoryItem(InventoryItem item)
    {
        this.itemType = item.GetItemType;
        this.itemName = item.GetName;
        this.itemPrefabName = item.GetPrefabName;
        this.itemIcon = item.GetIcon;
        this.itemPrefab = item.GetItemPrefab;
        this.weaponData = item.GetWeaponData;
        this.isStackable = item.GetIsStackable;
        this.maxStack = item.GetStackTotal;
    }

    #endregion

    #region Basic Variables

    [SerializeField] ItemType itemType = ItemType.Weapon;
    [SerializeField] string itemName = "_NoName";
    [SerializeField] string itemPrefabName = "_NoName";
    [SerializeField] Sprite itemIcon;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] WeaponSystem weaponData;
    [SerializeField] bool isStackable = false;
    [SerializeField] int maxStack = 5;
    [SerializeField] int stackAmount = 0;

    #endregion

    #region Getters and Setters

    public ItemType GetItemType { get { return itemType; } }
    public string GetName { get { return itemName; } }
    public string GetPrefabName { get { return itemPrefabName; } }
    public Sprite GetIcon { get { return itemIcon; } }
    public WeaponSystem GetWeaponData { get { return weaponData; } }
    public GameObject GetItemPrefab { get { return itemPrefab; } }
    public bool GetIsStackable { get { return isStackable; } }
    public int GetStackTotal { get { return maxStack; } }
    public int StackAmount
    {
        get { return stackAmount; }
        set
        {
            if (value <= maxStack)
                stackAmount = value;
        }
    }

    #endregion

    #region Event Handlers

    private void Awake()
    {

    }

    #endregion

    #region Basic Functions

    public void UseItem()
    {

    }

    #endregion
}
