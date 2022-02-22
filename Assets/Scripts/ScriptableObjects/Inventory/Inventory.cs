using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class Inventory : ScriptableObject
{
    public List<InventorySlot> inventory = new List<InventorySlot>();
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    private bool hasItem = false;

    /// <summary>
    /// Adds and item with the specified amount to the inventory.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <param name="amount">The number of the item to add.</param>
    public void AddItem(Item item, int amount = 1)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].item == item)
            {
                hasItem = true;
                inventory[i].AddCount(amount);
                break;
            }
        }

        if (!hasItem)
        {
            inventory.Add(new InventorySlot(item, amount));
        }
        hasItem = false;
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

    public bool HasItem(Item item, int count = 1)
    {
        return inventory.Any(x => x.item == item && x.count >= count);
    }

    /// <summary>
    /// Get the number of unique items in the inventory.
    /// </summary>
    /// <returns>The number of unique items.</returns>
    public int NumUniqueItems()
    {
        return inventory.Count;
    }

    /// <summary>
    /// Remove an item at the specified index in the inventory.
    /// </summary>
    /// <param name="index">The location to remove the item.</param>
    /// <returns>The item removed.</returns>
    public Item RemoveItem(int index)
    {
        if (inventory[index].count == 1)
        {
            Item item = inventory[index].item;
            inventory.RemoveAt(index);
            return item;
        }
        else
        {
            inventory[index].count--;
            Item item = inventory[index].item;
            return item;
        }
    }

    /// <summary>
    /// Remove the specified item from the inventory.
    /// </summary>
    /// <param name="item">The item to remove from the inventory.</param>
    /// <returns>The item removed.</returns>
    public Item RemoveItem(Item item, int count = 1)
    {
        InventorySlot removedItem = inventory.FirstOrDefault(x => x.item == item);
        if (removedItem.count - count <= 0)
        {
            inventory.Remove(removedItem);
        }
        else
        {
            removedItem.count -= count;
        }
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
        return removedItem.item;
    }

    public Item RemoveLastItem()
    {
        Item item = RemoveItem(inventory.Count - 1);
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
        return item;
    }
}

[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int count;
    public InventorySlot(Item item, int count)
    {
        this.item = item;
        this.count = count;
    }

    public void AddCount(int value)
    {
        count += value;
    }
}
