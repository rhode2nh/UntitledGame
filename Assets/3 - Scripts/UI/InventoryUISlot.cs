using UnityEngine;
using UnityEngine.UI;

public class InventoryUISlot : MonoBehaviour
{
    public Inventory inventory;
    private PlayerController playerController;
    private InventorySlot item;
    public Text inventorySlotText;
    public Image inventorySlotSprite;
    public Button button;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag(Constants.PLAYER).GetComponent<PlayerController>();
    }

    public void AddItem(InventorySlot newItem)
    {
        item = newItem;
        inventorySlotText.text = item.item.Name + ": " + item.count;
        inventorySlotSprite.sprite = item.item.sprite;
        inventorySlotSprite.enabled = true;
        button.interactable = true;
    }

    public void ClearSlot()
    {
        item = null;
        inventorySlotText.text = null;
        inventorySlotSprite.sprite = null;
        inventorySlotSprite.enabled = false;
        button.interactable = false;
    }

    public void OnRemoveButton()
    {
        playerController.DropItemInInventory(item.item);
    }
}
