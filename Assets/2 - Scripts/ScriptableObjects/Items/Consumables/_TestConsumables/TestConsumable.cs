using UnityEngine;

[CreateAssetMenu(fileName = "TestConsumable", menuName = "Inventory/Items/Consumables/TestConsumable", order = 1)]
public class TestConsumable : Item, ICraftable, IConsumable 
{
    [SerializeField]
    private Recipe recipe;

    private void Awake()
    {
        this.Name = Constants.TEST_CONSUMABLE;
        this.isStackable = true;
    }
    public Recipe Recipe { get => recipe; }
}
