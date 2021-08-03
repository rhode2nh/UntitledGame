using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<InventoryItem> items;
    public int MaxItems { get; set; }

    private void Start()
    {
        items = new List<InventoryItem>();       
    }

    public List<InventoryItem> GetItems()
    {
        return items;
    }

    public void RemoveItem(int index)
    {
        items.RemoveAt(index);
    }

    public InventoryItem RemoveItem(string itemName)
    {
        var item = items.FirstOrDefault(x => x.Name == itemName);

        if (item.Count == 1)
        {
            items.Remove(item);
        }
        else if (item.Count > 1)
        {
            item.Count--;
        }
        return CreateItem(itemName);
    }

    public void RemoveItem(InventoryItem item)
    {
        items.Remove(item);
    }

    public void AddItem(InventoryItem item)
    {
        var itemToUpdate = items.FirstOrDefault(x => x.Name == item.Name);
        if (item.IsStackable && itemToUpdate != null)
        {
            itemToUpdate.Count += 1;
        } 
        else
        {
            items.Add(item);
        }
    }

    public void AddItem(string itemName)
    {
        var itemToUpdate = items.FirstOrDefault(x => x.Name == itemName);
        if (itemToUpdate != null)
        {
            if (itemToUpdate.IsStackable)
            {
                itemToUpdate.Count += 1;
            } 
        }
        else
        {
            items.Add(CreateItem(itemName));
        }
    }

    public bool isItemInInventory(string itemName)
    {   
        return items.FirstOrDefault(x => x.Name == itemName) != null ? true : false;
    }

    public InventoryItem CreateItem(string itemName)
    {
        Type t = Type.GetType(itemName);
        InventoryItem item = (InventoryItem)Activator.CreateInstance(t);
        return item;
    }

    public void PrintItems()
    {
        string debugString = "";
        debugString += "Name\tCount\n";
        foreach (var item in items)
        {
            debugString += item.Name + "\t" + item.Count + "\n";
        }

        debugString += "Total Count: " + items.Count + "\n";
        Debug.Log(debugString);
    }
}
