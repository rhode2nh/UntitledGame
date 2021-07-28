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
    public void RemoveItem(InventoryItem item)
    {
        items.Remove(item);
    }

    public void AddItem(InventoryItem item)
    {
        var itemToUpdate = items.FirstOrDefault(x => x.Name == "Copper");
        if (item.IsStackable && itemToUpdate != null)
        {
            itemToUpdate.Count += 1;
        } 
        else
        {
            items.Add(item);
        }
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
