using UnityEngine;
using UnityEngine.UI;

public class EquipmentUISlot : MonoBehaviour
{
    public int equipmentIndex;

    public Slot slot;
    public Image inventorySlotSprite;
    public Button button;
    public bool itemInSlot;

    public void AddItem(Slot newSlot)
    {
        slot = newSlot;
        inventorySlotSprite.sprite = slot.item.sprite;
        inventorySlotSprite.enabled = true;
        button.interactable = true;
        itemInSlot = true;
    }

    public void ClearSlot()
    {
        slot = null;
        inventorySlotSprite.sprite = null;
        inventorySlotSprite.enabled = false;
        button.interactable = false;
        itemInSlot = false;
    }

    public void OnRemoveButton()
    {
        GameEvents.current.RemoveWeaponFromEquipmentInventory(slot.id);
    }
}
