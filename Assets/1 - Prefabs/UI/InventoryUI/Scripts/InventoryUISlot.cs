using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUISlot : UISlot, IDragHandler, IBeginDragHandler, IEndDragHandler
{
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
                if (modifierSlotList[i].item == GameEvents.current.GetEmptyItem())
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
        GameEvents.current.DeactivateInfoPanel();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (slot.item != GameEvents.current.GetEmptyItem())
        {
            GameEvents.current.ActivateInfoPanel();
            if (slot.item is IModifier)
            {
                var modifier = slot.item as IModifier;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append($"Name:\t\t{slot.item.name}\n");
                stringBuilder.Append($"Cast Delay:\t\t{modifier.CastDelay.ToString("0.0")}\n");
                stringBuilder.Append($"Recharge Time:\t{modifier.RechargeDelay.ToString("0.0")}\n");
                stringBuilder.Append($"X Spread:\t\t{modifier.XSpread.ToString("0.0")}\n");
                stringBuilder.Append($"Y Spread:\t\t{modifier.YSpread.ToString("0.0")}\n");
                GameEvents.current.SetInfoText(stringBuilder.ToString());
            }
            else if (slot.item is IWeapon)
            {
                var equippable = slot.item as IWeapon;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append($"Name:\t\t{slot.item.name}\n");
                stringBuilder.Append($"Max Slots:\t\t{slot.properties[Constants.P_W_MAX_SLOTS_INT].ToString()}\n");
                stringBuilder.Append($"Cast Delay:\t\t{equippable.CastDelay.ToString("0.0")}\n");
                stringBuilder.Append($"Recharge Time:\t{equippable.RechargeTime.ToString("0.0")}\n");
                stringBuilder.Append($"X Spread:\t\t{equippable.XSpread.ToString("0.0")}\n");
                stringBuilder.Append($"Y Spread:\t\t{equippable.YSpread.ToString("0.0")}\n");
                GameEvents.current.SetInfoText(stringBuilder.ToString());
            }
            else if (slot.item is IImplant)
            {
                var requiredStats = (TestStats)slot.properties[Constants.P_IMP_REQUIRED_STATS_DICT];
                var implant = slot.item as IImplant;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append($"Name:\t\t{slot.item.name}\n");
                stringBuilder.Append($"Body Part:\t\t{implant.BodyPart.ToString()}\n");
                stringBuilder.Append($"Agility:\t\t{implant.TestStats.agility.ToString()}\n");
                stringBuilder.Append($"Strength:\t\t{implant.TestStats.strength.ToString()}\n");
                stringBuilder.Append($"\nRequired Stats\n");
                stringBuilder.Append($"Agility:\t\t{requiredStats.agility.ToString()}\n");
                stringBuilder.Append($"Strength:\t\t{requiredStats.strength.ToString()}\n");
                GameEvents.current.SetInfoText(stringBuilder.ToString());
            }
            else
            {
                GameEvents.current.SetInfoText("This hasn't be setup yet.");
            }
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        GameEvents.current.DeactivateInfoPanel();
    }

    public override void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right) {
            Slot removedItem = GameEvents.current.RemoveItemFromPlayerInventory(slot.id);
            GameEvents.current.DeactivateInfoPanel();
            if (removedItem.item == GameEvents.current.GetEmptyItem()) {
                return;
            }

            GameEvents.current.DropItem(removedItem);  
        }
    }

    public override void OnEndDrag(PointerEventData data) {
        base.OnEndDrag(data);
        // if (data.pointerEnter.GetComponent<InventoryUISlot>() != null) {
        //     int indexToSwap = data.pointerEnter.GetComponent<InventoryUISlot>().index;
        //     GameEvents.current.SwitchInventoryItems(index, indexToSwap);
        // } else {
            // var slotToSwap = data.pointerEnter.GetComponent<UISlot>();
            // if (slotToSwap.CanAddToInventory(slot) && CanAddToInventory(slotToSwap.slot)) {
            //     Slot curSlot = RemoveItemFromInventory(slot.id);
            //     Slot otherSlot = slotToSwap.RemoveItemFromInventory(slotToSwap.slot.id);
            //     AddItemToInventory(otherSlot, index);
            //     slotToSwap.AddItemToInventory(curSlot, slotToSwap.index);
            // }
        // }
    }

    public override void AddItemToInventory(Slot slot, int index) {
        GameEvents.current.AddItemToPlayerInventoryAtIndex(slot, index);
    }

    public override Slot RemoveItemFromInventory(string id) {
        return GameEvents.current.RemoveItemFromPlayerInventory(id);
    }

    public override bool CanAddToInventory(Slot slot) {
        return true;
    }
}
