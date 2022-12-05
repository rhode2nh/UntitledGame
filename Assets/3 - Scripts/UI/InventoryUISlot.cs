using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUISlot : MonoBehaviour
{
    public Inventory inventory;
    private InventorySlot item;
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
        if (item.item is IModifier)
        {
            InventorySlot curWeapon = GameEvents.current.GetCurrentWeapon();
            if (curWeapon == null)
            {
                return;
            }
            List<Modifier> modifierList = (List<Modifier>)curWeapon.properties[Constants.P_W_MODIFIERS_LIST];
            List<int> modifierSlotIndices = (List<int>)curWeapon.properties[Constants.P_W_MODIFIER_SLOT_INDICES];
            int maxSlots = (int)curWeapon.properties[Constants.P_W_MAX_SLOTS];
            if (modifierList.Count == maxSlots)
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
            modifierList.Insert(availableIndices.First(), (Modifier)item.item);
            GameEvents.current.RemoveItemFromPlayerInventory(item.id);
            GameEvents.current.UpdateCurrentWeapon(curWeapon);
        }
        else if (item.item is IWeapon)
        {
            GameEvents.current.Equip(item.id);
            GameEvents.current.UpdateEquipmentContainer();
        }
    }
}
