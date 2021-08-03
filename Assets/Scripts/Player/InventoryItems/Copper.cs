using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Copper : InventoryItem
{
    public Copper()
    {
        Name = Constants.COPPER;
        Count = 1;
        IsStackable = true;
    }
}
