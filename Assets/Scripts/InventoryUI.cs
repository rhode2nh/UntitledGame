using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public InventoryObject inventory;
    public GameObject itemGroup;
    public GameObject inventoryItem;
    public Transform itemsParent;

    private InventoryUISlot[] slots;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        inventory.onItemChangedCallback += UpdateUI;
        slots = itemsParent.GetComponentsInChildren<InventoryUISlot>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.Size())
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
