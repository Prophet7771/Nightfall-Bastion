using System.Collections.Generic;
using UnityEngine;

namespace ItemTypes
{
    public enum ItemType { Weapon, Consumable, Collectable };
}

public class InventorySystem : MonoBehaviour
{
    #region Basic Variables

    [SerializeField] List<InventoryItem> inventoryItems = new List<InventoryItem>();
    [SerializeField] List<InventorySlot> inventorySlotItems = new List<InventorySlot>();
    [SerializeField] bool isContainer = false;

    [Header("User Interface")]
    [SerializeField] GameObject inventorySlots;
    [SerializeField] GameObject slotItem;
    [SerializeField] InventorySlot inventorySlot;

    #endregion

    #region Event Handlers

    private void Awake()
    {
    }

    #endregion

    #region Basic Functions

    public void AddToInventory(InventoryItem item)
    {
        if (item.GetIsStackable)
            AddToStack(item);
        else
            AddSingleItem(item);
    }

    private void AddSingleItem(InventoryItem item)
    {
        inventoryItems.Add(item);
        ++item.StackAmount;
        GameObject tempSlotObject = Instantiate(slotItem, inventorySlots.transform);
        InventorySlot tempSlot = tempSlotObject.GetComponent<InventorySlot>();
        tempSlot.UpdateUI(item);
        inventorySlotItems.Add(tempSlot);
    }

    private void AddToStack(InventoryItem item)
    {
        int loopCount = 0;
        bool foundItem = false;

        foreach (var tempItem in inventoryItems)
        {
            if (tempItem.GetName != item.GetName)
            {
                loopCount++;
                Debug.Log("Not Item Stack");
                continue;
            }

            if (tempItem.StackAmount == tempItem.GetStackTotal)
            {
                loopCount++;
                Debug.Log("Stack Full");
                continue;
            }

            ++tempItem.StackAmount;

            foundItem = true;

            inventorySlotItems[loopCount].UpdateStackCount(tempItem.StackAmount);

            loopCount++;

            Debug.Log("Added to Stack");

            return;
        }

        if (!foundItem)
            AddSingleItem(item);
    }

    #endregion
}
