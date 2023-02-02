using UnityEngine;

public class ModifierUISlot : MonoBehaviour
{
    public int slotUIIndex;
    public int modifierIndex;
    public int equipmentId;

    public void OnRemoveButton()
    {
        GameEvents.current.RemoveModifierFromWeapon(modifierIndex, equipmentId);
    }
}
