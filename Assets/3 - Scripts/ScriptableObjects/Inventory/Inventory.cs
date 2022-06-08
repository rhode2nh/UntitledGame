using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class Inventory : ScriptableObject
{
    public List<InventorySlot> items = new List<InventorySlot>();
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
