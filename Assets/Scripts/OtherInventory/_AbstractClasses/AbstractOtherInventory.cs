using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractOtherInventory: MonoBehaviour
{
    public List<InventorySlot> inventory = new List<InventorySlot>();
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    private bool hasItem = false;
    public void AddItem(ItemObject item, int amount)
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

    public int Size()
    {
        return inventory.Count;
    }

    public ItemObject RemoveItem(int index)
    {
        if (inventory[index].count == 1)
        {
            ItemObject item = inventory[index].item;
            inventory.RemoveAt(index);
            return item;
        }
        else
        {
            inventory[index].count--;
            ItemObject item = inventory[index].item;
            return item;
        }
    }

    public ItemObject RemoveItem(ItemObject item)
    {
        InventorySlot removedItem = inventory.FirstOrDefault(x => x.item == item);
        if (removedItem.count == 1)
        {
            inventory.Remove(removedItem);
        }
        else
        {
            removedItem.count--;
        }
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
        return removedItem.item;
    }

    public ItemObject RemoveLastItem()
    {
        ItemObject item = RemoveItem(inventory.Count - 1);
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
        return item;
    }
}
