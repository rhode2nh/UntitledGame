using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            instancedSlot.transform.SetParent(itemsParent);
            slots.Add(instancedSlot.GetComponentInChildren<InventoryUISlot>());
        }
    }

    private void Start()
    {
        // TODO
        // This needs to be implemented when a save state system is created.
        //UpdateUI();
    }

    private void UpdateUI(List<InventorySlot> items)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < items.Count)
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
