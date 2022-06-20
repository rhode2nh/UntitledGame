using UnityEngine;

[CreateAssetMenu(fileName = "TestConsumable", menuName = "Inventory/Items/Consumables/TestConsumable", order = 1)]
public class TestConsumable : Item, ICraftable, IConsumable 
{
    [SerializeField]
    private ItemStats itemStats;
    [SerializeField]
    private Recipe recipe;

    private void Awake()
    {
        this.Name = Constants.TEST_CONSUMABLE;
        this.Id = Constants.TEST_CONSUMABLE_ID;
        this.isStackable = true;
    }
    public ItemStats ItemStats { get => itemStats; }
    public Recipe Recipe { get => recipe; }
}
