using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Add Modifier Command", menuName = "Utilities/DeveloperConsole/Commands/Add Modifier Command")]
public class AddModifierCommand : ConsoleCommand
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
                Debug.Log("Usage: [equipment id] [modifier ids]");
                return false;
            }

            InventorySlot equipment = GameEvents.current.RemoveItemFromPlayerInventory(equipmentId);
            List<Modifier> modifierList = (List<Modifier>)equipment.properties[Constants.P_W_MODIFIERS_LIST];
            for (int i = 1; i < args.Length; i++)
            {
                if (Int32.TryParse(args[i], out int modifierId))
                {
                    if (!GameEvents.current.CheckType(modifierId, typeof(IModifier)))
                    {
                        Debug.Log("Item must be a modifier.");
                        return false;
                    }
                    InventorySlot modifier = GameEvents.current.RemoveItemFromPlayerInventory(modifierId);
                    modifierList.Add((Modifier)modifier.item);
                }
                else
                {
                    GameEvents.current.AddItemToPlayerInventory(equipment);
                    Debug.Log("Modifier id could not parsed: " + args[i]);
                    Debug.Log("Usage: [equipment id] [modifier ids]");
                    return false;
                }
            }
            GameEvents.current.AddItemToPlayerInventory(equipment);
            return true;
        }
        else
        {
            Debug.Log("Equipment id could not parsed: " + args[0]);
            Debug.Log("Usage: [equipment id] [modifier ids]");
            return false;
        }
    }
}
