using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> inventory = new List<InventorySlot>();
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

    public ItemObject RemoveLastItem()
    {
        return RemoveItem(inventory.Count - 1);
    }
}

[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int count;
    public InventorySlot(ItemObject item, int count)
    {
        this.item = item;
        this.count = count;
    }

    public void AddCount(int value)
    {
        count += value;
    }
}
