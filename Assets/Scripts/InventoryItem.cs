using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryItem
{
    public string Name { get; set; }
    public int Count { get; set; }
    public bool IsStackable { get; set; }
    public ItemType type { get; set; }
}
