using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New UnequipToPlayerInventoryCommand", menuName = "Utilities/DeveloperConsole/Commands/Unequip To Player Inventory Command")]
public class UnequipToPlayerInventory : ConsoleCommand
{
    public string[] args;
    
    public override bool Process(string[] args)
    {
        if (args.Length == 0)
        {
            GameEvents.current.UnEquipFirstOccurence();
            GameEvents.current.UpdateEquipmentContainer();
            return true;
        }
        this.args = args;

        if (Int32.TryParse(args[0], out int id))
        {
            InventorySlot itemToMove = GameEvents.current.Unequip(id);
            GameEvents.current.AddItemToPlayerInventory(new InventorySlot(itemToMove));
            GameEvents.current.UpdateEquipmentContainer();
            Debug.Log("Item was moved!");
        }
        else
        {
            Debug.Log("The id could not parsed: " + id);
        }

        return true;
    }
}
