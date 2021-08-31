using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherInventoryUI : MonoBehaviour
{
    public ChestInventory otherInventory;
    public Transform itemsParent;
    public GameObject slotPrefab;
    public int numSlots;

    private List<InventoryUISlot> slots = new List<InventoryUISlot>();
    // Start is called before the first frame update
    void Awake()
    {
        otherInventory.onItemChangedCallback += UpdateUI;
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
            if (i < otherInventory.Size())
            {
                slots[i].AddItem(otherInventory.inventory[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
