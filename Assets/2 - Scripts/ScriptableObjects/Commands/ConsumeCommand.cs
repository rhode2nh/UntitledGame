using UnityEngine;

[CreateAssetMenu(fileName = "New Consume Command", menuName = "Utilities/DeveloperConsole/Commands/Consume Command")]
public class ConsumeCommand : ConsoleCommand
{
    public override bool Process(string[] args)
    {
        if (args.Length == 0)
            return false;

        if (args.Length == 1)
        {
            //if (!GameEvents.current.HasItem(args[0]))
            //{
            //    Debug.Log("You do not have this item, or it cannot be consumed.");
            //    return false;
            //}

            //Item item = GameEvents.current.GetItem(args[0]);
            //if (item is IConsumable)
            //{
            //    GameEvents.current.Consume(item);
            //    Debug.Log("Item Consumed!");
            //}
        }
        return true;
    }
}
