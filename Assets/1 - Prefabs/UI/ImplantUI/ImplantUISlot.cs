using UnityEngine;

public class ImplantUISlot : UISlot
{
    public BodyPart allowedImplant;

    public override void OnRemoveButton()
    {
        if (slot.item == GameEvents.current.GetEmptyItem())
        {
            return;
        }


        TestStats stats = (TestStats)slot.properties[Constants.P_IMP_STATS_DICT];  
        Slot implant = GameEvents.current.RemoveImplant(slot.id);
        GameEvents.current.AddItemToPlayerInventory(implant);
    }
}
