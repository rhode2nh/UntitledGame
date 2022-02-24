using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestConsumable", menuName = "Inventory/Items/Consumables/TestConsumable", order = 1)]
public class TestConsumable : Consumable
{
    private void Awake()
    {
        this.Name = Constants.TEST_CONSUMABLE;
        this.Id = Constants.TEST_CONSUMABLE_ID;
        this.type = ItemType.CONSUMABLE;
    }
}
