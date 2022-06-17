using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    public event Action<int, Item, int, Dictionary<string, object>> onAddItemToPlayerInventory;
    public event Action<List<InventorySlot>> onUpdateInventoryGUI;
    public event Func<Recipe, bool> onCanCraft;
    public event Func<Recipe, bool> onCraft;
    public event Action<Item> onConsume;
    public event Func<string, Item> onGetItem;
    public event Func<string, bool> onHasItem;
    public event Action<int> onEquip;
    public event Func<int, InventorySlot> onUnequip;
    public event Func<int, InventorySlot> onRemoveItemFromPlayerInventory;
    public event Action onClearInventory;
    public event Func<int, bool> onIsItemEquippable;

    public void Awake()
    {
        current = this;
    }

    public void AddItemToPlayerInventory(int id, Item item, int amount, Dictionary<string, object> properties = null)
    {
        if (onAddItemToPlayerInventory != null)
        {
            onAddItemToPlayerInventory(id, item, amount, properties);
        }
    }

    public void UpdateInventoryGUI(List<InventorySlot> items)
    {
        if (onUpdateInventoryGUI != null)
        {
            onUpdateInventoryGUI(items);
        }
    }

    public bool CanCraft(Recipe recipe)
    {
        if (onCanCraft != null)
        {
            return onCanCraft(recipe);
        }

        return false;
    }

    public bool Craft(Recipe recipe)
    {
        if (onCraft != null)
        {
            return onCraft(recipe);
        }

        return false;
    }

    public void Consume(Item item)
    {
        if (onConsume != null)
        {
            onConsume(item);
        }
    }

    public Item GetItem(string name)
    {
        if (onGetItem != null)
        {
            return onGetItem(name);
        }

        return null;
    }

    public bool HasItem(string name)
    {
        if (onHasItem != null)
        {
            return onHasItem(name);
        }
        
        return false;
    }

    public void Equip(int id)
    {
        if (onEquip != null)
        {
            onEquip(id);
        }
    }

    public InventorySlot RemoveItemFromPlayerInventory(int id)
    {
        if (onRemoveItemFromPlayerInventory != null)
        {
            return onRemoveItemFromPlayerInventory(id);
        }

        return null;
    }

    public void ClearInventory()
    {
        if (onClearInventory != null)
        {
            onClearInventory();
        }
    }

    public bool IsItemEquippable(int id)
    {
        if (onIsItemEquippable != null)
        {
            return onIsItemEquippable(id);
        }

        return false;
    }

    public InventorySlot Unequip(int id)
    {
        if (onUnequip != null)
        {
            return onUnequip(id);
        }

        return null;
    }
}
