using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    public event Action<Item, int> onItemPickup;
    public event Action<List<InventorySlot>> onUpdateInventoryGUI;
    public event Func<Recipe, bool> onCanCraft;
    public event Func<Recipe, bool> onCraft;
    public event Action<Item> onConsume;
    public event Func<string, Item> onGetItem;
    public event Func<string, bool> onHasItem;

    public void Awake()
    {
        current = this;
    }

    public void ItemPickup(Item item, int amount)
    {
        if (onItemPickup != null)
        {
            onItemPickup(item, amount);
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
}
