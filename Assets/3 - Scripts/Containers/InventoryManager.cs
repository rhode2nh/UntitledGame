using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Inventory inventory;
    public PlayerStats playerStats;
    private bool hasItem;

    private void Start()
    {
        GameEvents.current.onAddItemToPlayerInventory += AddItem;
        GameEvents.current.onCanCraft += CanCraft;
        GameEvents.current.onCraft += Craft;
        GameEvents.current.onConsume += Consume;
        GameEvents.current.onGetItem += GetItem;
        GameEvents.current.onHasItem += HasItem;
        GameEvents.current.onRemoveItem += RemoveItem;
        hasItem = false;
    }

    /// <summary>
    /// Adds and item with the specified amount to the inventory.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <param name="amount">The number of the item to add.</param>
    public void AddItem(Item item, int amount = 1, Dictionary<string, object> properties = null)
    {
        if (properties == null)
        {
            properties = new Dictionary<string, object>();
        }

        for (int i = 0; i < inventory.items.Count; i++)
        {
            if (inventory.items[i].item == item)
            {
                hasItem = true;
                inventory.items[i].AddCount(amount);
                break;
            }
        }

        if (!hasItem)
        {
            inventory.items.Add(new InventorySlot(item, amount, properties));
        }
        hasItem = false;
        GameEvents.current.UpdateInventoryGUI(inventory.items);
    }

    public bool HasItem(string name)
    {
        return inventory.items.Any(x => x.item.Name.Equals(name));
    }

    public bool HasItem(Item item, int count = 1)
    {
        return inventory.items.Any(x => x.item == item && x.count >= count);
    }

    /// <summary>
    /// Get the number of unique items in the inventory.
    /// </summary>
    /// <returns>The number of unique items.</returns>
    public int NumUniqueItems()
    {
        return inventory.items.Count;
    }

    /// <summary>
    /// Remove an item at the specified index in the inventory.
    /// </summary>
    /// <param name="index">The location to remove the item.</param>
    /// <returns>The item removed.</returns>
    public Item RemoveItem(int index)
    {
        if (inventory.items[index].count == 1)
        {
            Item item = inventory.items[index].item;
            inventory.items.RemoveAt(index);
            return item;
        }
        else
        {
            inventory.items[index].count--;
            Item item = inventory.items[index].item;
            return item;
        }
    }

    public Item RemoveItem(string name, int count = 1)
    {
        InventorySlot removedItem = inventory.items.FirstOrDefault(x => x.item.Name == name.ToUpper());
        if (removedItem.count - count <= 0)
        {
            inventory.items.Remove(removedItem);
        }
        else
        {
            removedItem.count -= count;
        }

        GameEvents.current.UpdateInventoryGUI(inventory.items);
        return removedItem.item;
    }

    public Item GetItem(string name)
    {
        InventorySlot removedItem = inventory.items.FirstOrDefault(x => x.item.Name == name.ToUpper());
        return removedItem.item;
    }

    /// <summary>
    /// Remove the specified item from the inventory.
    /// </summary>
    /// <param name="item">The item to remove from the inventory.</param>
    /// <returns>The item removed.</returns>
    public Item RemoveItem(Item item, int count = 1)
    {
        InventorySlot removedItem = inventory.items.FirstOrDefault(x => x.item == item);
        if (removedItem.count - count <= 0)
        {
            inventory.items.Remove(removedItem);
        }
        else
        {
            removedItem.count -= count;
        }

        GameEvents.current.UpdateInventoryGUI(inventory.items);
        return removedItem.item;
    }

    public Item RemoveLastItem()
    {
        Item item = RemoveItem(inventory.items.Count - 1);
        return item;
    }

    public bool CanCraft(Recipe recipe)
    {
        return recipe.RequiredItems.All(x => this.HasItem(x.item, x.count));
    }

    public bool Craft(Recipe recipe)
    {
        if (CanCraft(recipe))
        {
            recipe.RequiredItems.ForEach(x => this.RemoveItem(x.item, x.count));
            recipe.Results.ForEach(x => this.AddItem(x.item, x.count));
            return true;
        }

        return false;
    }

    public void Consume(Item item)
    {
        if (HasItem(item) && item is IConsumable)
        {
            var consumable = item as IConsumable;
            ApplyConsumable(consumable.ItemStats);
            RemoveItem(item);
        }
    }

    public void ApplyConsumable(ItemStats stats)
    {
        foreach (var attribute in stats.attributes)
        {
            playerStats.attributes[attribute.Key].RawValue += attribute.Value.RawValue;
            playerStats.attributes[attribute.Key].BuffPercentage += attribute.Value.BuffPercentage;
        }
    }
}
