using UnityEngine;
using UnityEngine.UI;

public class InventoryUISlot : MonoBehaviour
{
    public InventoryObject inventory;
    public PlayerController playerController;
    private InventorySlot item;
    private Text inventorySlotText;
    private Image inventorySlotSprite;

    private void Start()
    {
        inventorySlotText = GetComponentInChildren<Text>();
        inventorySlotSprite = gameObject.transform.GetChild(0).GetComponent<Image>();
    }

    public void AddItem(InventorySlot newItem)
    {
        item = newItem;
        inventorySlotText.text = item.item.Name + ": " + item.count;
        inventorySlotSprite.sprite = item.item.sprite;
        inventorySlotSprite.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;
        inventorySlotText.text = null;
        inventorySlotSprite.sprite = null;
        inventorySlotSprite.enabled = false;
    }

    public void OnRemoveButton()
    {
        playerController.DropLastItemInInventory();
    }
}
