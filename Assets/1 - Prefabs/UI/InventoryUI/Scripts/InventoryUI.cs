using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject slotPrefab;
    private int numSlots = 10;

    private List<IUISlot> slots = new List<IUISlot>();

    void Awake()
    {
        for (int i = 0; i < numSlots; i++)
        {
            GameObject instancedSlot = Instantiate(slotPrefab);
            instancedSlot.GetComponentInChildren<IUISlot>().ClearSlot();
            instancedSlot.transform.SetParent(itemsParent, false);
            slots.Add(instancedSlot.GetComponentInChildren<IUISlot>());
        }

        GameEvents.current.onUpdateInventoryGUI += UpdateUI;
        GameEvents.current.DeactivateInfoPanel();
    }

    private void UpdateUI(List<Slot> items)
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
}
