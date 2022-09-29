using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Add All Modifiers Command", menuName = "Utilities/DeveloperConsole/Commands/Add All Modifiers Command")]
public class AddAllModifiersCommand : ConsoleCommand
{
    public string[] args;
    
    public override bool Process(string[] args)
    {
        if (args.Length == 0)
            return false;
        this.args = args;
        
        if (Int32.TryParse(args[0], out int equipmentId))
        {
            if (!GameEvents.current.CheckType(equipmentId, typeof(IEquippable)))
            {
                Debug.Log("Item must be equippable.");
                return false;
            }

            InventorySlot equipment = GameEvents.current.RemoveItemFromPlayerInventory(equipmentId);
            List<Modifier> modifierList = (List<Modifier>)equipment.properties[Constants.P_W_MODIFIERS_LIST];
            var modifiersToAdd = GameEvents.current.GetAllModifiers();
            for (int i = 0; i < modifiersToAdd.Count; i++)
            {
                var modifier = GameEvents.current.RemoveItemFromPlayerInventory(modifiersToAdd[i].id);
                modifierList.Add((Modifier)modifier.item);
            }
            GameEvents.current.AddItemToPlayerInventory(equipment);
            return true;
        }
        else
        {
            Debug.Log("Usage: [equipment id] [modifier ids]");
            return false;
        }
    }
}
