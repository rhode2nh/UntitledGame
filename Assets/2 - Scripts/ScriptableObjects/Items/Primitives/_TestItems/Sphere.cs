using UnityEngine;

[CreateAssetMenu(fileName = "Sphere", menuName = "Inventory/Items/Sphere", order = 1)]
public class Sphere : Item
{
    private void Awake()
    {
        this.Name = Constants.SPHERE;
        this.isStackable = false;
    }
}
