using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "New Craft Command", menuName = "Utilities/DeveloperConsole/Commands/Craft Command")]
public class CraftCommand : ConsoleCommand
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
            if (operation.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError("\"" + args[0] + "\"" + " does not exist.");
            }

            if (operation.Result is ICraftable)
            {
                var craftable = operation.Result as ICraftable;
                if (GameEvents.current.Craft(craftable.Recipe) == true)
                {
                    Debug.Log("Item was crafted");
                }
                else
                {
                    Debug.Log("Item could not be crafted.");
                }
            }
        }
    }

    private void OnDestroy()
    {
        Addressables.Release(handle);
    }
}
