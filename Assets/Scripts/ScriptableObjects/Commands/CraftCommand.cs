using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "New Craft Command", menuName = "Utilities/DeveloperConsole/Commands/Craft Command")]
public class CraftCommand : ConsoleCommand
{
    public Inventory InventoryObject;
    public string[] args;
    private AsyncOperationHandle<Recipe> handle;
    
    public override bool Process(string[] args)
    {
        if (args.Length == 0)
            return false;
        this.args = args;

        var load = "Assets/Addressables/ScriptableObjects/Recipes/" + args[0] + "Recipe.asset";
        handle = Addressables.LoadAssetAsync<Recipe>(load);
        handle.Completed += Handle_Completed;
        return true;
    }
    private void Handle_Completed(AsyncOperationHandle<Recipe> operation)
    {
        if (args.Length == 1)
        {
            if (operation.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError("\"" + args[0] + "\"" + " does not exist.");
            }

            if (operation.Result.Craft(InventoryObject) == true)
            {
                Debug.Log("Item was crafted");
            }
            else
            {
                Debug.Log("Item could not be crafted.");
            }
        }
    }

    private void OnDestroy()
    {
        Addressables.Release(handle);
    }
}
