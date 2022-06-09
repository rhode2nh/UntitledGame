using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "New Equip Command", menuName = "Utilities/DeveloperConsole/Commands/Equip Command")]
public class EquipCommand : ConsoleCommand
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
        if (args.Length == 1)
        {
            if (operation.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError("\"" + args[0] + "\"" + " does not exist.");
            }

            if (operation.Result is IEquippable)
            {
                if (GameEvents.current.HasItem(operation.Result.Name))
                {
                    GameEvents.current.Equip(operation.Result, 1);
                    Debug.Log("Item was equipped!");
                }
                else
                {
                    Debug.Log("Item cannot be equppied");
                }
            }
        }
    }

    private void OnDestroy()
    {
        Addressables.Release(handle);
    }
}
