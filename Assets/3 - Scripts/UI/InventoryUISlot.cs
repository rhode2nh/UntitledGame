using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUISlot : MonoBehaviour
{
    private Slot slot;
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
        slot.slotUIIndex = -1;
        if (slot.item is IModifier)
        {
            Slot curWeapon = GameEvents.current.GetCurrentWeapon();
            if (curWeapon == null)
            {
                return;
            }
            List<Slot> modifierSlotList = (List<Slot>)curWeapon.properties[Constants.P_W_MODIFIERS_LIST];
            List<int> modifierSlotIndices = (List<int>)curWeapon.properties[Constants.P_W_MODIFIER_SLOT_INDICES];
            int maxSlots = (int)curWeapon.properties[Constants.P_W_MAX_SLOTS];
            if (modifierSlotList.Count == maxSlots)
            {
                return;
            }
            var availableIndices = new List<int>();
            for (int j = 0; j < maxSlots; j++)
            {
                availableIndices.Add(j);
            }
            availableIndices = availableIndices.Except(modifierSlotIndices).ToList();
            modifierSlotIndices.Insert(availableIndices.First(), availableIndices.First());
            modifierSlotList.Insert(availableIndices.First(), slot);
            GameEvents.current.RemoveItemFromPlayerInventory(slot.id);
            GameEvents.current.UpdateCurrentWeapon(curWeapon);
        }
        else if (slot.item is IWeapon)
        {
            GameEvents.current.Equip(slot.id);
            GameEvents.current.UpdateEquipmentContainer();
        }
    }
}
