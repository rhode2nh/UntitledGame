using UnityEngine;

[CreateAssetMenu(fileName = "Cylinder", menuName = "Inventory/Items/Craftables/Cylinder", order = 1)]
public class Cylinder : Item, ICraftable
{
    [SerializeField]
    private Recipe recipe;

    private void Awake()
    {
        this.Name = Constants.CYLINDER;
        this.isStackable = true;
    }
    public Recipe Recipe { get => recipe; }
}
