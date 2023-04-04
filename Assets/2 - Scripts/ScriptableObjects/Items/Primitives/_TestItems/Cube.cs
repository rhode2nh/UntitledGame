using UnityEngine;

[CreateAssetMenu(fileName = "Cube", menuName = "Inventory/Items/Cube", order = 1)]
public class Cube : Item
{
    private void Awake()
    {
        this.Name = Constants.CUBE;
        this.isStackable = true;
    }
}
