using System;
using System.Collections.Generic;
using System.Linq;
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

            Slot equipment = GameEvents.current.RemoveItemFromPlayerInventory(equipmentId);
            List<Modifier> modifierList = (List<Modifier>)equipment.properties[Constants.P_W_MODIFIERS_LIST];
            List<int> modifierSlotIndices = (List<int>)equipment.properties[Constants.P_W_MODIFIER_SLOT_INDICES];
            int maxSlots = (int)equipment.properties[Constants.P_W_MAX_SLOTS];
            var modifiersToAdd = GameEvents.current.GetAllModifiers();
            for (int i = 0; i < modifiersToAdd.Count; i++)
            {
                if (modifierList.Count >= maxSlots)
                {
                    break;
                }
                var modifier = GameEvents.current.RemoveItemFromPlayerInventory(modifiersToAdd[i].id);
                var availableIndices = new List<int>();
                for (int j = 0; j < maxSlots; j++)
                {
                    availableIndices.Add(j);
                }
                availableIndices = availableIndices.Except(modifierSlotIndices).ToList();
                modifierSlotIndices.Insert(availableIndices.First(), availableIndices.First());
                modifierList.Insert(availableIndices.First(), (Modifier)modifier.item);
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
