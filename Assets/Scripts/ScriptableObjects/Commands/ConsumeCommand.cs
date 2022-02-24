using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consume Command", menuName = "Utilities/DeveloperConsole/Commands/Consume Command")]
public class ConsumeCommand : ConsoleCommand
{
    public Inventory Inventory;
    public PlayerStats playerStats;
    //public string[] args;
    //private AsyncOperationHandle<Recipe> handle;
    
    public override bool Process(string[] args)
    {
        if (args.Length == 0)
            return false;

        if (args.Length == 1)
        {
            if (!Inventory.HasItem(args[0]))
            {
                return false;
            }
            Consumable item = (Consumable)Inventory.RemoveItem(args[0]);
            item.Consume(playerStats);
        }
        return true;
    }
    //private void Handle_Completed(AsyncOperationHandle<Recipe> operation)
    //{
    //    if (args.Length == 1)
    //    {
    //        if (operation.Status == AsyncOperationStatus.Failed)
    //        {
    //            Debug.LogError("\"" + args[0] + "\"" + " does not exist.");
    //        }

    //        if (operation.Result.Craft(InventoryObject) == true)
    //        {
    //            Debug.Log("Item was crafted");
    //        }
    //        else
    //        {
    //            Debug.Log("Item could not be crafted.");
    //        }
    //    }
    //}

    //private void OnDestroy()
    //{
    //    Addressables.Release(handle);
    //}
}
