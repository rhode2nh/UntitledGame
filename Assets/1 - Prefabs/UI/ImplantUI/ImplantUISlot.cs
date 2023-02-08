using System.Collections.Generic;

public class ImplantUISlot : UISlot
{
    public BodyPart allowedImplant;

    public override void OnRemoveButton()
    {
        GameEvents.current.AddItemToPlayerInventory(slot);
    }
}
