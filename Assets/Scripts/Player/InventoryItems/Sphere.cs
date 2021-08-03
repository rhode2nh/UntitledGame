using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : InventoryItem
{
    public Sphere()
    {
        Name = Constants.SPHERE;
        Count = 1;
        IsStackable = true;
    }
}
