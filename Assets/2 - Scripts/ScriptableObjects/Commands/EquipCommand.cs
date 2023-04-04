using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equip Command", menuName = "Utilities/DeveloperConsole/Commands/Equip Command")]
public class EquipCommand : ConsoleCommand
{
    public string[] args;
    
    public override bool Process(string[] args)
    {
        if (args.Length == 0)
        {
            GameEvents.current.EquipFirstOccurence();
            GameEvents.current.UpdateEquipmentContainer();
            return true;
        }
        this.args = args;

        if (Int32.TryParse(args[0], out int id))
        {
            GameEvents.current.Equip(id.ToString());
            GameEvents.current.UpdateEquipmentContainer();
        }
        else
        {
            Debug.Log("The id could not parsed: " + id);
            return false;
        }

        return true;
    }
}
