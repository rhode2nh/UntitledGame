using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    public GameObject equipmentUISlot;
    public Transform equipmentParent; 

    private List<IUISlot> slots = new List<IUISlot>();

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onUpdateWeaponGUI += UpdateUI;
        GameEvents.current.onGetCurrentWeaponFromSlot += GetCurrentWeaponFromSlot;
        GameEvents.current.onSwitchActiveEquipmentUISlot += SwitchActiveEquipmentUISlot;

        // TODO: GET MAX SIZE FROM EQUIPMENT INVENTORY
        // TODO: GET ACTIVE SLOT FROM SAVE STATE
        for (int i = 0; i < 4; i++)
        {
            GameObject instancedSlot = Instantiate(equipmentUISlot);
            instancedSlot.GetComponentInChildren<IUISlot>().ClearSlot();
            if (i == 0)
            {
                var activeSlot = instancedSlot.GetComponentInChildren<EquipmentUISlot>();
                activeSlot.ActiveSlotImage.enabled = true;
            }
            instancedSlot.transform.SetParent(equipmentParent, false);
            slots.Add(instancedSlot.GetComponentInChildren<IUISlot>());
        }
    }

    Slot GetCurrentWeaponFromSlot(int index)
    {
        if (!slots[index].IsItemInSlot())
        {
            return null;
        }
        else
        {
            return slots[index].GetSlot();
        }
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
