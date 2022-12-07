using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Clear Modifiers Command", menuName = "Utilities/DeveloperConsole/Commands/Clear Modifiers Command")]
public class ClearModifiers : ConsoleCommand
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

            Slot equipment = GameEvents.current.RemoveItemFromPlayerInventory(equipmentId);
            List<Modifier> modifierList = (List<Modifier>)equipment.properties[Constants.P_W_MODIFIERS_LIST];
            List<int> modifierSlotIndices = (List<int>)equipment.properties[Constants.P_W_MODIFIER_SLOT_INDICES];
            modifierList.Clear();
            modifierSlotIndices.Clear();
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
