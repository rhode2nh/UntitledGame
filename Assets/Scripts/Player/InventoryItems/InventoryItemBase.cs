using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemBase : InventoryItem
{
    public InventoryItemBase()
    {
        Name = Constants.INVENTORY_ITEM_BASE;
        Count = 1;
        IsStackable = true;
    }
}
