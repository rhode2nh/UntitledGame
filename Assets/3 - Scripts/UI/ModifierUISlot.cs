using UnityEngine;

public class ModifierUISlot : MonoBehaviour
{
    public int index;
    public int equipmentId;

    public void OnRemoveButton()
    {
        GameEvents.current.RemoveModifierFromWeapon(index, equipmentId);
    }
}
