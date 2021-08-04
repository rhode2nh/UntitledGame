using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryItem
{
    public Item itemReference { get; set; }
    
    //public InventoryItem(InventoryItem item)
    //{
    //    Name = item.Name;
    //    Count = item.Count;
    //    IsStackable = item.IsStackable;
    //    type = item.type;
    //    IsWearableItem = item.IsWearableItem;
    //}
}
