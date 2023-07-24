using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class InventoryManager : MonoBehaviour, IDataPersistence
{
    public Inventory inventory;
    public PlayerStats playerStats;
    private bool hasItem;

    private void Start()
    {
        GameEvents.current.onAddItemToPlayerInventory += AddItem;
        GameEvents.current.onAddItemToPlayerInventoryAtIndex += AddItem;
        GameEvents.current.onCanCraft += CanCraft;
        GameEvents.current.onCraft += Craft;
        GameEvents.current.onConsume += Consume;
        GameEvents.current.onGetItem += GetItem;
        GameEvents.current.onHasItem += HasItem;
        GameEvents.current.onRemoveItemFromPlayerInventory += RemoveItem;
        GameEvents.current.onCheckType += CheckType;
        GameEvents.current.onRemoveItemByType += RemoveItemByType;
        GameEvents.current.onGetAllModifiers += GetAllModifiers;
        GameEvents.current.onSwitchInventoryItems += SwitchInventoryItems;
        GameEvents.current.onClearInventory += ClearInventory;
        hasItem = false;
    }

    /// <summary>
    /// Adds an item with the specified amount to the inventory.
    /// </summary>
    /// <param name="item">The item to add.</param>
    public void AddItem(Slot item)
    {
        if (item.item.isStackable)
        {
            for (int i = 0; i < inventory.items.Count; i++)
            {
                if (inventory.items[i].item == item.item)
                {
                    hasItem = true;
                    inventory.items[i].AddCount(item.count);
                    break;
                }
            }
        }

        if (!hasItem)
        {
            for (int i = 0; i < inventory.maxSize; i++)
            {
                if (inventory.items[i].item == GameEvents.current.GetEmptyItem())
                {
                    inventory.items[i] = new Slot(item);
                    break;
                }
            }
        }
        hasItem = false;
        GameEvents.current.UpdateInventoryGUI(inventory.items);
    }

    /// <summary>
    /// Adds an item to the inventory at the specified index.
    /// </summary>
    /// <param name="item">The item to add.</param>
    public void AddItem(Slot item, int index)
    {
        // if (item.item.isStackable)
        // {
        //     for (int i = 0; i < inventory.items.Count; i++)
        //     {
        //         if (inventory.items[i].item == item.item)
        //         {
        //             hasItem = true;
        //             inventory.items[i].AddCount(item.count);
        //             break;
        //         }
        //     }
        // }

        // if (!hasItem)
        // {
        //     for (int i = 0; i < inventory.maxSize; i++)
        //     {
        //         if (inventory.items[i].item == GameEvents.current.GetEmptyItem())
        //         {
        //             inventory.items[i] = new Slot(item);
        //             break;
        //         }
        //     }
        // }
        // hasItem = false;
        if (inventory.items[index].item != GameEvents.current.GetEmptyItem()) {
            return;
        }
        inventory.items[index] = new Slot(item);
        GameEvents.current.UpdateInventoryGUI(inventory.items);
    }

    public bool HasItem(string id)
    {
        return inventory.items.Any(x => x.id == id);
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

    public Slot RemoveItem(string id)
    {
        Slot removedItem = inventory.items.FirstOrDefault(x => x.id == id);
        if (removedItem == null) {
            return GameEvents.current.GetEmptySlot();
        }
        if (removedItem.count == 1)
        {
            int index = inventory.items.IndexOf(removedItem);
            inventory.items[index] = GameEvents.current.GetEmptySlot();
        }
        else
        {
            removedItem.count -= 1;
        }

        GameEvents.current.UpdateInventoryGUI(inventory.items);
        return removedItem;
    }

    public Item RemoveItem(string name, int count = 1)
    {
        Slot removedItem = inventory.items.FirstOrDefault(x => x.item.Name == name.ToUpper());
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

    public Slot GetItem(string id)
    {
        Slot removedItem = inventory.items.FirstOrDefault(x => x.id == id);
        return removedItem;
    }

    /// <summary>
    /// Remove the specified item from the inventory.
    /// </summary>
    /// <param name="item">The item to remove from the inventory.</param>
    /// <returns>The item removed.</returns>
    public Item RemoveItem(Item item, int count = 1)
    {
        Slot removedItem = inventory.items.FirstOrDefault(x => x.item == item);
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

    /// <summary>
    /// Removes the first occurance of an item by it's type
    /// </summary>
    /// <params name="type">The type of item.</param>
    /// <returns>The item removed.</returns>
    public Slot RemoveItemByType(System.Type type)
    {
        foreach (var item in inventory.items)
        {
            if (CheckType(item.id, type))
            {
                return RemoveItem(item.id);
            }
        }

        return null;
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
            //recipe.Results.ForEach(x => this.AddItem(x.item, x.count));
            return true;
        }

        return false;
    }

    public void Consume(Item item)
    {
        //if (HasItem(item) && item is IConsumable)
        //{
        //    var consumable = item as IConsumable;
        //    ApplyConsumable(consumable.ItemStats);
        //    RemoveItem(item);
        //}
    }

    //public void ApplyConsumable(ItemStats stats)
    //{
    //    //foreach (var attribute in stats.attributes)
    //    //{
    //    //    playerStats.attributes[attribute.Key].RawValue += attribute.Value.RawValue;
    //    //    playerStats.attributes[attribute.Key].BuffPercentage += attribute.Value.BuffPercentage;
    //    //}
    //}

    public void ClearInventory()
    {
        for (int i = 0; i < inventory.items.Count; i++) {
            inventory.items[i] = GameEvents.current.GetEmptySlot();
        }
    }

    public bool CheckType(string id, params System.Type[] types)
    {
        var item = inventory.items.FirstOrDefault(x => x.id == id);
        return types.All(type => type.IsAssignableFrom(item.item.GetType()));
    }

    public List<Slot> GetAllModifiers()
    {
        List<Slot> modifierList = new List<Slot>();

        foreach (var inventoryItem in inventory.items)
        {
            if (CheckType(inventoryItem.id, typeof(IModifier)))
            {
                modifierList.Add(inventoryItem);
            }
        }

        return modifierList;
    }

    public void SaveData(ref GameData data)
    {
        try {
            data.inventoryItemsData.Clear();
            foreach (var slot in inventory.items)
            {
                data.inventoryItemsData.Add(StateManager.SaveItemData(slot));
            }
            inventory.items.Clear();
        }
        catch (Exception e)
        {
            Debug.Log(e);
            inventory.items.Clear();
        }
    }

    public void LoadData(GameData data)
    {
        if (data.inventoryItemsData.Count == 0 || data.inventoryItemsData.All(x => x.itemId == "-1"))
        {
            inventory.InitializeInventory();
        }
        else
        {
            foreach (var itemData in data.inventoryItemsData)
            {
                inventory.items.Add(StateManager.LoadItemData(itemData));
            }
        }
        GameEvents.current.UpdateInventoryGUI(inventory.items);
    }

    public void SwitchInventoryItems(int index1, int index2) {
        Slot item1 = inventory.items[index1];
        inventory.items[index1] = inventory.items[index2];
        inventory.items[index2] = item1;
        GameEvents.current.UpdateInventoryGUI(inventory.items);
    }
}
