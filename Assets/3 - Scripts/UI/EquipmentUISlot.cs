using UnityEngine;
using UnityEngine.UI;

public class EquipmentUISlot : MonoBehaviour
{
    public int slotUIIndex;
    public int equipmentIndex;

    public InventorySlot item;
    public Image inventorySlotSprite;
    public Button button;
    public bool itemInSlot;

    public void AddItem(InventorySlot newItem)
    {
        item = newItem;
        inventorySlotSprite.sprite = item.item.sprite;
        inventorySlotSprite.enabled = true;
        button.interactable = true;
        itemInSlot = true;
    }

    public void ClearSlot()
    {
        item = null;
        inventorySlotSprite.sprite = null;
        inventorySlotSprite.enabled = false;
        button.interactable = false;
        itemInSlot = false;
    }

    public void OnRemoveButton()
    {
        item.slotUIIndex = -1;
        GameEvents.current.RemoveWeaponFromEquipmentInventory(equipmentIndex);
    }
}
