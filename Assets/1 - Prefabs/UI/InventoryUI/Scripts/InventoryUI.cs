using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject slotPrefab;
    public GameObject infoPanel;
    private int numSlots = 10;

    private List<IUISlot> slots = new List<IUISlot>();

    void Start()
    {
        for (int i = 0; i < numSlots; i++)
        {
            GameObject instancedSlot = Instantiate(slotPrefab);
            instancedSlot.GetComponentInChildren<IUISlot>().ClearSlot();
            instancedSlot.GetComponent<InventoryUISlot>().infoPanel = infoPanel;
            instancedSlot.transform.SetParent(itemsParent, false);
            slots.Add(instancedSlot.GetComponentInChildren<IUISlot>());
        }
        GameEvents.current.onUpdateInventoryGUI += UpdateUI;
        infoPanel.SetActive(false);
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
}
