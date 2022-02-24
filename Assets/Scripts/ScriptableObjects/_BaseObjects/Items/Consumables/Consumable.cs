using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Item
{
    public ItemStats itemStats;

    public void Consume(PlayerStats playerStats)
    {
        playerStats.ApplyConsumable(itemStats);
        //Destroy(this);
    }
}
