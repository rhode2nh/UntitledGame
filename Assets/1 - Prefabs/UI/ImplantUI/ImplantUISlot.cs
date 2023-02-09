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

        Slot implant = GameEvents.current.RemoveImplant(slot.id);
        GameEvents.current.AddItemToPlayerInventory(implant);
    }
}
