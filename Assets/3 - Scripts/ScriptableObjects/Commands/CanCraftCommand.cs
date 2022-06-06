using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "New Can Craft Command", menuName = "Utilities/DeveloperConsole/Commands/Can Craft Command")]
public class CanCraftCommand : ConsoleCommand
{
    public Inventory inventory;
    public string[] args;
    private AsyncOperationHandle<Item> handle;
    public override bool Process(string[] args)
    {
        if (args.Length == 0)
            return false;
        this.args = args;

        var load = "Assets/Addressables/ScriptableObjects/Items/" + args[0] + ".asset";
        handle = Addressables.LoadAssetAsync<Item>(load);
        handle.Completed += Handle_Completed;
        return true;
    }

    private void Handle_Completed(AsyncOperationHandle<Item> operation)
    {
        if (args.Length == 1)
        {
            // TODO 
            if (operation.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError("\"" + args[0] + "\"" + " does not exist.");
            }

            if (operation.Result is ICraftable)
            {
                var craftable = operation.Result as ICraftable;
                if (inventory.CanCraft(craftable.Recipe))
                {
                    Debug.Log("The item can be crafted!");
                }
                else
                {
                    Debug.Log("The item cannot be crafted.");
                }
            }
        }
    }

    private void OnDestroy()
    {
        Addressables.Release(handle);
    }
}
