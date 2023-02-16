using System.Text;
using UnityEngine.EventSystems;

public class ImplantUISlot : UISlot
{
    public BodyPart allowedImplant;

    public override void OnRemoveButton()
    {
        if (slot.item == GameEvents.current.GetEmptyItem())
        {
            return;
        }

        Slot implant = GameEvents.current.RemoveImplant(slot.id);
        GameEvents.current.AddItemToPlayerInventory(implant);
        GameEvents.current.CalculateBuffedStats();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (slot.item != GameEvents.current.GetEmptyItem())
        {
            var requiredStats = (TestStats)slot.properties[Constants.P_IMP_REQUIRED_STATS_DICT];
            GameEvents.current.ActivateInfoPanel();
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
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        GameEvents.current.DeactivateInfoPanel();
    }
}
