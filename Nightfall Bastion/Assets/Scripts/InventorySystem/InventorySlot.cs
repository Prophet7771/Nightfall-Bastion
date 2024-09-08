using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    #region Basic Variables

    [Header("User Interface")]
    [SerializeField] Image itemImage;
    [SerializeField] TMP_Text stackText;

    InventoryItem slotData;

    #endregion

    #region Event Handlers

    private void OnEnable()
    {
        if (slotData)
            itemImage.sprite = slotData.GetIcon;
    }

    public void UpdateUI(InventoryItem itemData)
    {
        slotData = itemData;
        itemImage.sprite = itemData.GetIcon;
    }

    #endregion

    #region Basic Functions

    public void UpdateStackCount(int amount)
    {
        stackText.gameObject.SetActive(true);
        stackText.text = amount.ToString();
    }

    #endregion

}
