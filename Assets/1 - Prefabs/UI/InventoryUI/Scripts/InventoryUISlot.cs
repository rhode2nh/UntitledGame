using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUISlot : UISlot, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject infoPanel;
    public override void OnRemoveButton()
    {
        if (slot.item is IModifier)
        {
            Slot curWeapon = GameEvents.current.GetCurrentWeapon();
            if (curWeapon.item == GameEvents.current.GetEmptyItem())
            {
                return;
            }
            List<Slot> modifierSlotList = (List<Slot>)curWeapon.properties[Constants.P_W_MODIFIERS_LIST];
            List<int> modifierSlotIndices = (List<int>)curWeapon.properties[Constants.P_W_MODIFIER_SLOT_INDICES_LIST];
            int maxSlots = (int)curWeapon.properties[Constants.P_W_MAX_SLOTS_INT];
            bool emptySlotFound = false;
            for (int i = 0; i < maxSlots; i++)
            {
                if (modifierSlotList[i].id == -1)
                {
                    modifierSlotList[i] = slot;
                    emptySlotFound = true;
                    break;
                }
            }
            if (emptySlotFound)
            {
                GameEvents.current.RemoveItemFromPlayerInventory(slot.id);
            }
            GameEvents.current.UpdateCurrentWeapon(curWeapon);
        }
        else if (slot.item is IWeapon)
        {
            GameEvents.current.Equip(slot.id);
            GameEvents.current.UpdateEquipmentContainer();
        }
        else if (slot.item is IImplant)
        {
            GameEvents.current.AddItemToImplantInventory(slot);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (slot.item != GameEvents.current.GetEmptyItem())
        {
            Debug.Log(slot.item.name);
            infoPanel.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Leaving inventory slot");
        infoPanel.SetActive(false);
    }
}
