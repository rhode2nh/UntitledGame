using System.Text;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentUISlot : UISlot 
{
    public int equipmentIndex;
    public Image ActiveSlotImage;

    public override void OnRemoveButton()
    {
        GameEvents.current.RemoveWeaponFromEquipmentInventory(slot.id);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (slot.item != GameEvents.current.GetEmptyItem())
        {
            GameEvents.current.ActivateInfoPanel();
            var equippable = slot.item as IWeapon;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"Name:\t\t{slot.item.name}\n");
            stringBuilder.Append($"Max Slots:\t\t{equippable.MaxSlots.ToString()}\n");
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
}
