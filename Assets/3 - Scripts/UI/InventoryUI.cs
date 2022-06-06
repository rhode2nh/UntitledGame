using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public Transform itemsParent;
    public GameObject slotPrefab;
    public int numSlots;

    private List<InventoryUISlot> slots = new List<InventoryUISlot>();

    void Awake()
    {
        inventory.onItemChangedCallback += UpdateUI;
        for (int i = 0; i < numSlots; i++)
        {
            GameObject instancedSlot = Instantiate(slotPrefab);
            instancedSlot.transform.SetParent(itemsParent);
            slots.Add(instancedSlot.GetComponentInChildren<InventoryUISlot>());
        }
    }

    private void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < inventory.NumUniqueItems())
            {
                slots[i].AddItem(inventory.inventory[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
