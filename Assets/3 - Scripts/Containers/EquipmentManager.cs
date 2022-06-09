using System.Linq;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public Inventory equipmentInventory;
    private bool hasItem = false;

    private void Start()
    {
        GameEvents.current.onEquip += Equip;
    }

    /// <summary>
    /// Adds and item with the specified amount to the equipment inventory.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <param name="amount">The number of the item to add.</param>
    public void Equip(Item item, int amount = 1)
    {
        if (equipmentInventory.items.Count == equipmentInventory.maxSize)
            return;
        Item itemToEquip = GameEvents.current.RemoveItem(item, amount);

        for (int i = 0; i < equipmentInventory.items.Count; i++)
        {
            if (equipmentInventory.items[i].item == itemToEquip)
            {
                hasItem = true;
                equipmentInventory.items[i].AddCount(amount);
                break;
            }
        }

        if (!hasItem)
        {
            equipmentInventory.items.Add(new InventorySlot(itemToEquip, amount));
        }
        hasItem = false;
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
}
