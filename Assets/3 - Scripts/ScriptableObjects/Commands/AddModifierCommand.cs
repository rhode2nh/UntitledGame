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
        if (args.Length != 2)
            return false;
        this.args = args;

        if (Int32.TryParse(args[0], out int equipmentId) && Int32.TryParse(args[1], out int modifierId))
        {
            if (!GameEvents.current.CheckType(equipmentId, typeof(IEquippable)))
            {
                Debug.Log("Item must be equippable.");
                return false;
            }

            if (!GameEvents.current.CheckType(modifierId, typeof(IModifier)))
            {
                Debug.Log("Item must be a modifier.");
                return false;
            }
            InventorySlot equipment = GameEvents.current.RemoveItemFromPlayerInventory(equipmentId);
            InventorySlot modifier = GameEvents.current.RemoveItemFromPlayerInventory(modifierId);
            List<Modifier> modifierList = (List<Modifier>)equipment.properties[Constants.P_W_MODIFIERS_LIST];
            modifierList.Add((Modifier)modifier.item);
            GameEvents.current.AddItemToPlayerInventory(equipment);
            return true;
        }
        else
        {
            Debug.Log("Ids could not parsed: " + args[0] + "," + args[1]);
            return false;
        }
    }
}
