using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "New Add Item To Inventory Command", menuName = "Utilities/DeveloperConsole/Commands/Add Item To Inventory Command")]
public class AddItemToInventoryCommand : ConsoleCommand
{
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
        if (operation.Status == AsyncOperationStatus.Failed)
        {
            Debug.LogError("\"" + args[0] + "\"" + " does not exist.");
        }

        else if (args.Length == 1)
        {
            GameEvents.current.AddItemToPlayerInventory(operation.Result.GetInstanceID(), operation.Result, 1);
            Debug.Log("Item added");
        }

        else if (args.Length >= 2)
        {
            if (int.TryParse(args[1], out int result))
            {
                GameEvents.current.AddItemToPlayerInventory(operation.Result.GetInstanceID(), operation.Result, result);
                Debug.Log("Items added");
            }
        }
    }

    private void OnDestroy()
    {
        Addressables.Release(handle);
    }
}
