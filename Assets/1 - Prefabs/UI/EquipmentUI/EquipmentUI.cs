using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    public GameObject equipmentUISlot;
    public Transform equipmentParent; 

    private List<IUISlot> slots = new List<IUISlot>();

    void Awake()
    {
        GameEvents.current.onUpdateWeaponGUI += UpdateUI;
        GameEvents.current.onGetCurrentWeaponFromSlot += GetCurrentWeaponFromSlot;
        GameEvents.current.onSwitchActiveEquipmentUISlot += SwitchActiveEquipmentUISlot;
        
        // TODO: GET MAX SIZE FROM EQUIPMENT INVENTORY
        for (int i = 0; i < 4; i++)
        {
            GameObject instancedSlot = Instantiate(equipmentUISlot);
            instancedSlot.GetComponentInChildren<IUISlot>().ClearSlot();
            instancedSlot.transform.SetParent(equipmentParent, false);
            slots.Add(instancedSlot.GetComponentInChildren<IUISlot>());
        }
        int activeSlotIndex = GameEvents.current.GetCurEquipmentIndex();
        var activeSlot = slots[activeSlotIndex] as EquipmentUISlot;
        activeSlot.ActiveSlotImage.enabled = true;
    }

    Slot GetCurrentWeaponFromSlot(int index)
    {
        return slots[index].GetSlot();
    }

    private void UpdateUI(List<Slot> items, int maxSlots)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item != GameEvents.current.GetEmptyItem())
            {
                slots[i].AddItem(items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }

    public void SwitchActiveEquipmentUISlot(int prevActiveSlot, int curActiveSlot)
    {
        var prevEquipmentUISlot = slots[prevActiveSlot] as EquipmentUISlot;
        prevEquipmentUISlot.ActiveSlotImage.enabled = false;
        var curEquipmentUISlot = slots[curActiveSlot] as EquipmentUISlot;
        curEquipmentUISlot.ActiveSlotImage.enabled = true;
    }
}
