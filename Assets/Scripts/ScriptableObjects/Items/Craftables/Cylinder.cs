using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cylinder", menuName = "Inventory/Items/Craftables/Cylinder", order = 1)]
public class Cylinder : Item, ICraftable
{
    [SerializeField]
    private Recipe recipe;

    private void Awake()
    {
        this.Name = Constants.CYLINDER;
        this.Id = Constants.CYLINDER_ID;
        this.type = ItemType.TEST;
    }
    public Recipe Recipe { get => recipe; }
}
