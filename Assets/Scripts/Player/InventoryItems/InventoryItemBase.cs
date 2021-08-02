using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemBase : InventoryItem
{
    public InventoryItemBase()
    {
        Name = "InventoryItemBase";
        Count = 1;
        IsStackable = true;
    }
}
