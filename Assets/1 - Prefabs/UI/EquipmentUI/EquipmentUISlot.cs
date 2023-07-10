using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentUISlot : UISlot, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public int equipmentIndex;
    public Image ActiveSlotImage;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (slot.item != GameEvents.current.GetEmptyItem())
        {
            GameEvents.current.ActivateInfoPanel();
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
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        GameEvents.current.DeactivateInfoPanel();
    }

    public override void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right) {
            Slot removedItem = GameEvents.current.RemoveWeaponFromEquipmentInventory(slot.id);
            GameEvents.current.DeactivateInfoPanel();
            if (removedItem.item == GameEvents.current.GetEmptyItem()) {
                return;
            }

            GameEvents.current.DropItem(removedItem);  
        } else if (eventData.button == PointerEventData.InputButton.Left && eventData.clickCount == 2) {
            Slot removedItem = GameEvents.current.RemoveWeaponFromEquipmentInventory(slot.id);
            GameEvents.current.AddItemToPlayerInventory(removedItem);
        }
    }

    public override void AddItemToInventory(Slot slot, int index) {
        GameEvents.current.EquipAtIndex(slot, index);
    }

    public override Slot RemoveItemFromInventory(string id) {
        return GameEvents.current.RemoveWeaponFromEquipmentInventory(id);
    }

    public override bool CanAddToInventory(Slot slot) {
        return slot.item is IEquippable || slot.item == GameEvents.current.GetEmptyItem();
    }
}
