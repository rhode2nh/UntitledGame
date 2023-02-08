using System.Collections.Generic;
using System.Linq;

public class InventoryUISlot : UISlot
{
    public override void OnRemoveButton()
    {
        if (slot.item is IModifier)
        {
            Slot curWeapon = GameEvents.current.GetCurrentWeapon();
            if (curWeapon == null)
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
    }
}
