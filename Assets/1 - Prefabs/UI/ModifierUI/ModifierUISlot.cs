
using System.Text;
using UnityEngine.EventSystems;

public class ModifierUISlot : UISlot
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

    public override void OnRemoveButton()
    {
        Slot removedModifier = GameEvents.current.RemoveModifierFromWeapon(modifierIndex, equipmentId);
        GameEvents.current.AddItemToPlayerInventory(removedModifier);
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
        }
    }
}
