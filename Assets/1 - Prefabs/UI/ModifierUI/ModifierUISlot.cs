
using System.Collections.Generic;
using System.Text;
using UnityEngine.EventSystems;

public class ModifierUISlot : UISlot, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public int slotUIIndex;
    public int modifierIndex;
    public int equipmentId;

    public override void ClearSlot()
    {
        slot = GameEvents.current.GetEmptySlot();
        slotSprite.sprite = null;
        slotSprite.enabled = false;
        button.interactable = false;
        isItemInSlot = false;
        modifierIndex = -1;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (slot.item != GameEvents.current.GetEmptyItem())
        {
            GameEvents.current.ActivateInfoPanel();
            var modifier = slot.item as IModifier;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"Name:\t\t{slot.item.name}\n");
            stringBuilder.Append($"Cast Delay:\t\t{modifier.CastDelay.ToString("0.0")}\n");
            stringBuilder.Append($"Recharge Time:\t{modifier.RechargeDelay.ToString("0.0")}\n");
            stringBuilder.Append($"X Spread:\t\t{modifier.XSpread.ToString("0.0")}\n");
            stringBuilder.Append($"Y Spread:\t\t{modifier.YSpread.ToString("0.0")}\n");
            GameEvents.current.SetInfoText(stringBuilder.ToString());
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        GameEvents.current.DeactivateInfoPanel();
    }

    public override void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right) {
            Slot removedItem = GameEvents.current.RemoveModifierFromWeapon(modifierIndex, equipmentId);
            GameEvents.current.DeactivateInfoPanel();
            if (removedItem.item == GameEvents.current.GetEmptyItem()) {
                return;
            }

            GameEvents.current.DropItem(removedItem);  
        } else if (eventData.button == PointerEventData.InputButton.Left && eventData.clickCount == 2) {
            Slot removedModifier = GameEvents.current.RemoveModifierFromWeapon(modifierIndex, equipmentId);
            GameEvents.current.AddItemToPlayerInventory(removedModifier);
        }
    }

    public override void AddItemToInventory(Slot slot, int index) {
        Slot curWeapon = GameEvents.current.GetCurrentWeapon();
        if (curWeapon.item == GameEvents.current.GetEmptyItem())
        {
            return;
        }
        List<Slot> modifierSlotList = (List<Slot>)curWeapon.properties[Constants.P_W_MODIFIERS_LIST];
        List<int> modifierSlotIndices = (List<int>)curWeapon.properties[Constants.P_W_MODIFIER_SLOT_INDICES_LIST];
        if (modifierSlotList[index].item == GameEvents.current.GetEmptyItem()) {
            modifierSlotList[index] = slot;
        }
        GameEvents.current.UpdateCurrentWeapon(curWeapon);
    }

    public override Slot RemoveItemFromInventory(string id) {
        return GameEvents.current.RemoveModifierFromWeapon(index, GameEvents.current.GetCurEquipmentIndex());
    }

    public override bool CanAddToInventory(Slot slot) {
        return slot.item is IModifier || slot.item == GameEvents.current.GetEmptyItem();
    }
}
