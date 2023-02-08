using UnityEngine.UI;

public class EquipmentUISlot : UISlot 
{
    public int equipmentIndex;
    public Image ActiveSlotImage;

    public override void OnRemoveButton()
    {
        GameEvents.current.RemoveWeaponFromEquipmentInventory(slot.id);
    }
}
