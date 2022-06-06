using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consume Command", menuName = "Utilities/DeveloperConsole/Commands/Consume Command")]
public class ConsumeCommand : ConsoleCommand
{
    public Inventory inventory;
    public PlayerStats playerStats;
    
    public override bool Process(string[] args)
    {
        if (args.Length == 0)
            return false;

        if (args.Length == 1)
        {
            if (!inventory.HasItem(args[0]))
            {
                return false;
            }

            Item item = inventory.GetItem(args[0]);
            if (item is IConsumable)
            {
                var consumable = item as IConsumable;
                inventory.RemoveItem(args[0]);
                playerStats.ApplyConsumable(consumable.ItemStats);
            }
        }
        return true;
    }
}
