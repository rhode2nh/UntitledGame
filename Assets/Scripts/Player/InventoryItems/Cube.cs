using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : InventoryItem
{
    public Cube()
    {
        Name = Constants.CUBE;
        Count = 1;
        IsStackable = true;
    }
}
