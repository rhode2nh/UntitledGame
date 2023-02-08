
public class ModifierUISlot : UISlot
{
    public int slotUIIndex;
    public int modifierIndex;
    public int equipmentId;

    public override void ClearSlot()
    {
        slot = null;
        slotSprite.sprite = null;
        slotSprite.enabled = false;
        button.interactable = false;
        isItemInSlot = false;
        modifierIndex = -1;
    }

    public override void OnRemoveButton()
    {
        GameEvents.current.RemoveModifierFromWeapon(modifierIndex, equipmentId);
    }
}
