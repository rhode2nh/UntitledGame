using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Item> items;
    public int MaxItems { get; set; }

    private void Start()
    {
        items = new List<Item>();       
    }

    public List<Item> GetItems()
    {
        return items;
    }

    public int Count()
    {
        return items.Count;
    }

    public Item RemoveItem(int index)
    {
        if (items[index].Count == 1)
        {
            Item item = Instantiate(items[index]);
            items.RemoveAt(index);
            return item;
        }
        else
        {
            items[index].Count--;
            Item item = Instantiate(items[index]);
            item.Count = 1;
            return item;
        }
    }

    public Item RemoveLastItem()
    {
        return RemoveItem(items.Count - 1);
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
    }

    public void AddItem(Item item)
    {
        var itemToUpdate = items.FirstOrDefault(x => x.Id == item.Id);
        if (item.IsStackable && itemToUpdate != null)
        {
            for (int i = 0; i < item.Count; i++)
            {
                itemToUpdate.Count += 1;
            }
        } 
        else
        {
            items.Add(item);
        }
    }

    public bool isItemInInventory(string itemName)
    {   
        return items.FirstOrDefault(x => x.Name == itemName) != null ? true : false;
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
