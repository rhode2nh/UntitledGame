using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImplantUISlot : UISlot, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public BodyPart allowedImplant;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (slot.item != GameEvents.current.GetEmptyItem())
        {
            GameEvents.current.ActivateInfoPanel();
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
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        GameEvents.current.DeactivateInfoPanel();
    }

    public override void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right) {
            Slot removedItem = GameEvents.current.RemoveImplant(slot.id);
            GameEvents.current.DeactivateInfoPanel();
            if (removedItem.item == GameEvents.current.GetEmptyItem()) {
                return;
            }

            GameEvents.current.DropItem(removedItem);  
        } else if (eventData.button == PointerEventData.InputButton.Left && eventData.clickCount == 2) {
            if (slot.item == GameEvents.current.GetEmptyItem())
            {
                return;
            }

            Slot implant = GameEvents.current.RemoveImplant(slot.id);
            GameEvents.current.AddItemToPlayerInventory(implant);
            GameEvents.current.CalculateBuffedStats();
        }
    }
    
    public override void AddItemToInventory(Slot slot, int index) {
        GameEvents.current.AddItemToImplantInventory(slot);
    }

    public override Slot RemoveItemFromInventory(string id) {
        return GameEvents.current.RemoveImplant(id);
    }

    public override bool CanAddToInventory(Slot slot) {
        if (slot.item == GameEvents.current.GetEmptyItem()) {
            return true;
        }
        if (slot.item is IImplant) {
            if (((IImplant)slot.item).BodyPart == allowedImplant) {
                var buffedStats = GameEvents.current.GetBuffedStats();
                var requiredStats = (TestStats)slot.properties[Constants.P_IMP_REQUIRED_STATS_DICT]; 
                if (buffedStats >= requiredStats)
                {
                    return true;
                }
                else {
                    Debug.Log("Can't equip implant. Requirements not met.");
                    Debug.Log("Current Stats:\nagility: " + buffedStats.agility + "\nstrength: " + buffedStats.strength);
                    Debug.Log("Required Stats:\nagility: " + requiredStats.agility + "\nstrength: " + requiredStats.strength);
                }
            }
        }
        return false;
    }
}
