using UnityEngine;

[CreateAssetMenu(fileName = "Sphere", menuName = "Inventory/Items/Sphere", order = 1)]
public class Sphere : Item
{
    private void Awake()
    {
        this.Name = Constants.SPHERE;
        this.Id = Constants.SPHERE_ID;
        this.isStackable = false;
    }
}
