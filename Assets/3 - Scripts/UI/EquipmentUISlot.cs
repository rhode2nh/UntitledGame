using UnityEngine;

public class EquipmentUISlot : MonoBehaviour
{
    public int slotUIIndex;
    public int equipmentIndex;

    public void OnRemoveButton()
    {
        GameEvents.current.RemoveWeaponFromEquipmentInventory(equipmentIndex);
    }
}
