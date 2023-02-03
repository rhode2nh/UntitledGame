using System.Collections.Generic;
using UnityEngine;

public enum InventoryType {
    Player,
    Equipment,
    Gun,
}

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject slotPrefab;
    public InventoryType inventoryType;
    private int numSlots;

    private List<InventoryUISlot> slots = new List<InventoryUISlot>();

    void Awake()
    {
        InitializeNumSlots();
        for (int i = 0; i < numSlots; i++)
        {
            GameObject instancedSlot = Instantiate(slotPrefab);
            instancedSlot.GetComponentInChildren<InventoryUISlot>().ClearSlot();
            instancedSlot.transform.SetParent(itemsParent, false);
            slots.Add(instancedSlot.GetComponentInChildren<InventoryUISlot>());
        }
    }

    private void Start()
    {
        // TODO
        // This needs to be implemented when a save state system is created.
        //UpdateUI();
        GameEvents.current.onUpdateInventoryGUI += UpdateUI;
    }

    private void UpdateUI(List<Slot> items)
    {
        // if the id is -1, then it's the empty item
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item.Id != -1)
            {
                slots[i].AddItem(items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }

        }
    }

    private void InitializeNumSlots()
    {
        // TODO: expand
        switch (inventoryType)
        {
            case InventoryType.Player:
                numSlots = 10;
                break;
            case InventoryType.Equipment:
                numSlots = 4;
                break;
            case InventoryType.Gun:
                numSlots = 10;
                break;
        }
    }
}
