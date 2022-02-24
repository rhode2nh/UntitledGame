using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cylinder", menuName = "Inventory/Items/Craftables/Cylinder", order = 1)]
public class Cylinder : Craftable
{
    private void Awake()
    {
        this.Name = Constants.CYLINDER;
        this.Id = Constants.CYLINDER_ID;
        this.type = ItemType.TEST;
    }
}
