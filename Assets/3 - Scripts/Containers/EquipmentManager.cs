using System.Linq;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public Inventory equipmentInventory;
    private bool hasItem = false;

    private void Start()
    {
        GameEvents.current.onEquip += Equip;
        GameEvents.current.onClearInventory += ClearInventory;
        GameEvents.current.onUnequip += Unequip;
    }

    /// <summary>
    /// Adds by id to the equipment inventory.
    /// </summary>
    /// <param name="id">The item id.</param>
    public void Equip(int id)
    {
        if (equipmentInventory.items.Count >= equipmentInventory.maxSize)
            return;
        if (!GameEvents.current.CheckType(id, typeof(IEquippable)))
        {
            Debug.Log("Item is not equippable");
            return;
        }

        InventorySlot itemToEquip = GameEvents.current.RemoveItemFromPlayerInventory(id);
        if (!hasItem)
        {
            equipmentInventory.items.Add(itemToEquip);
        }
        hasItem = false;
    }

    /// <summary>
    /// Removes an item from the equipment inventory.
    /// </summary>
    public InventorySlot Unequip(int id)
    {
        InventorySlot itemToUnequip = equipmentInventory.items.FirstOrDefault(x => x.id == id);
        equipmentInventory.items.Remove(itemToUnequip);
        return itemToUnequip;
    }

    /// <summary>
    /// Get the number of unique items in the inventory.
    /// </summary>
    /// <returns>The number of unique items.</returns>
    public int NumUniqueItems()
    {
        return equipmentInventory.items.Count;
    }

    /// <summary>
    /// Remove an item at the specified index in the inventory.
    /// </summary>
    /// <param name="index">The location to remove the item.</param>
    /// <returns>The item removed.</returns>
    public Item RemoveItem(int index)
    {
        if (equipmentInventory.items[index].count == 1)
        {
            Item item = equipmentInventory.items[index].item;
            equipmentInventory.items.RemoveAt(index);
            return item;
        }
        else
        {
            equipmentInventory.items[index].count--;
            Item item = equipmentInventory.items[index].item;
            return item;
        }
    }

    public Item RemoveItem(string name, int count = 1)
    {
        InventorySlot removedItem = equipmentInventory.items.FirstOrDefault(x => x.item.Name == name.ToUpper());
        if (removedItem.count - count <= 0)
        {
            equipmentInventory.items.Remove(removedItem);
        }
        else
        {
            removedItem.count -= count;
        }

        GameEvents.current.UpdateInventoryGUI(equipmentInventory.items);
        return removedItem.item;
    }

    public Item GetItem(string name)
    {
        InventorySlot removedItem = equipmentInventory.items.FirstOrDefault(x => x.item.Name == name.ToUpper());
        return removedItem.item;
    }

    /// <summary>
    /// Remove the specified item from the inventory.
    /// </summary>
    /// <param name="item">The item to remove from the inventory.</param>
    /// <returns>The item removed.</returns>
    public Item RemoveItem(Item item, int count = 1)
    {
        InventorySlot removedItem = equipmentInventory.items.FirstOrDefault(x => x.item == item);
        if (removedItem.count - count <= 0)
        {
            equipmentInventory.items.Remove(removedItem);
        }
        else
        {
            removedItem.count -= count;
        }

        GameEvents.current.UpdateInventoryGUI(equipmentInventory.items);
        return removedItem.item;
    }

    public Item RemoveLastItem()
    {
        Item item = RemoveItem(equipmentInventory.items.Count - 1);
        return item;
    }

    public void ClearInventory()
    {
        equipmentInventory.items.Clear();
    }
}
