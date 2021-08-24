using UnityEngine;
using UnityEngine.UI;

public class InventoryUISlot : MonoBehaviour
{
    public InventoryObject inventory;
    private PlayerController playerController;
    private InventorySlot item;
    private Text inventorySlotText;
    private Image inventorySlotSprite;
    private Button button;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag(Constants.PLAYER).GetComponent<PlayerController>();
        inventorySlotText = GetComponentInChildren<Text>();
        inventorySlotSprite = gameObject.transform.GetChild(0).GetComponent<Image>();
        button = GetComponent<Button>();
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