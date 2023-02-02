using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject slotPrefab;
    public int numSlots;

    private List<InventoryUISlot> slots = new List<InventoryUISlot>();

    void Awake()
    {
        GameEvents.current.onUpdateInventoryGUI += UpdateUI;
        for (int i = 0; i < numSlots; i++)
        {
            GameObject instancedSlot = Instantiate(slotPrefab);
            instancedSlot.transform.SetParent(itemsParent, false);
            slots.Add(instancedSlot.GetComponentInChildren<InventoryUISlot>());
        }
    }

    private void Start()
    {
        // TODO
        // This needs to be implemented when a save state system is created.
        //UpdateUI();
    }

    private void UpdateUI(List<Slot> items)
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
                Debug.Log(items[i].item.name);
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
