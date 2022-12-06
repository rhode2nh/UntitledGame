using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour
{
    public GameObject equipmentUISlot;
    public Transform equipmentParent; 

    private List<EquipmentUISlot> slots = new List<EquipmentUISlot>();

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onUpdateWeaponGUI += UpdateUI;
        GameEvents.current.onGetCurrentWeaponFromSlot += GetCurrentWeaponFromSlot;
        // TODO: GET MAX SIZE FROM EQUIPMENT INVENTORY
        for (int i = 0; i < 4; i++)
        {
            GameObject instancedSlot = Instantiate(equipmentUISlot);
            instancedSlot.transform.SetParent(equipmentParent, false);
            slots.Add(instancedSlot.GetComponentInChildren<EquipmentUISlot>());
        }
    }

    InventorySlot GetCurrentWeaponFromSlot(int index)
    {
        if (slots[index].itemInSlot == false)
        {
            return null;
        }
        else
        {
            return slots[index].item;
        }
    }

    void UpdateUI(List<InventorySlot> items, int maxSlots)
    {
        // Setup available slots to choose from
        List<bool> availableUiSlots = new List<bool>(new bool[slots.Count]);
        for (int i = 0; i < availableUiSlots.Count; i++)
        {
            availableUiSlots[i] = true;
        }
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].slotUIIndex != -1)
            {
                availableUiSlots[items[i].slotUIIndex] = false;
            }
        }

        // Clear slots to reinitialize them
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].ClearSlot();
        }

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].slotUIIndex != -1)
            {
                slots[items[i].slotUIIndex].AddItem(items[i]);
            }
            else if (items[i].slotUIIndex == -1)
            {
                int nextAvailableSlotIndex = -1;
                // Grab the next available slot
                for (int j = 0; j < availableUiSlots.Count; j++)
                {
                    if (availableUiSlots[j] == true)
                    {
                        nextAvailableSlotIndex = j;
                        availableUiSlots[j] = false;
                        break;
                    }
                }
                items[i].slotUIIndex = nextAvailableSlotIndex;
                slots[items[i].slotUIIndex].AddItem(items[i]);
            }
        }
    }
}
