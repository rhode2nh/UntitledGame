using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "New Add Item To Inventory Command", menuName = "Utilities/DeveloperConsole/Commands/Add Item To Inventory Command")]
public class AddItemToInventoryCommand : ConsoleCommand
{
    public string[] args;
    private AsyncOperationHandle<GameObject> handle;

    public override bool Process(string[] args)
    {
        if (args.Length == 0) {
            Debug.Log("Usage: [item ids]");
            return false;
        }
        this.args = args;

        var load = "Assets/Addressables/Prefabs/Entities/" + Constants.WORLD_ITEM_PREFIX + "_" + args[0] + ".prefab";
        handle = Addressables.LoadAssetAsync<GameObject>(load);
        handle.Completed += Handle_Completed;
        return true;
    }

    private void Handle_Completed(AsyncOperationHandle<GameObject> operation)
    {
        if (operation.Status == AsyncOperationStatus.Failed)
        {
            Debug.LogError("\"" + args[0] + "\"" + " does not exist.");
            Debug.Log("Usage: [item ids]");
        }


        if (args.Length == 1)
        {
            var instantiatedObject = Instantiate(operation.Result);
            WorldItem worldItem = instantiatedObject.GetComponent<WorldItem>();
            GameEvents.current.AddItemToPlayerInventory(new Slot(worldItem.id, worldItem.item, 1, worldItem.properties));
            Debug.Log("Item added: " + worldItem.id);
            Destroy(instantiatedObject);
        }

        else if (args.Length >= 2)
        {
            if (int.TryParse(args[1], out int result))
            {
                for (int i = 0; i < result; i++)
                {
                    var instantiatedObject = Instantiate(operation.Result);
                    WorldItem worldItem = instantiatedObject.GetComponent<WorldItem>();
                    GameEvents.current.AddItemToPlayerInventory(new Slot(worldItem.id, worldItem.item, 1, worldItem.properties));
                    Debug.Log("Item added: " + worldItem.id);
                    Destroy(instantiatedObject);
                }
            }
        }

    }

    private void OnDestroy()
    {
        Addressables.Release(handle);
    }
}
