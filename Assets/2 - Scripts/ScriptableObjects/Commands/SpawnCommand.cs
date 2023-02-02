using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "New Spawn Command", menuName = "Utilities/DeveloperConsole/Commands/Spawn Command")]
public class SpawnCommand : ConsoleCommand
{
    public Vector3 position;
    public Quaternion rotation;
    public string[] args;
    private AsyncOperationHandle<GameObject> handle;
    public override bool Process(string[] args)
    {
        if (args.Length == 0)
            return false;
        args[0].Replace("_", "");
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
        }
        if (args.Length == 2 && operation.Status == AsyncOperationStatus.Succeeded)
        {
            if (int.TryParse(args[1], out int result))
            {
                int yOffset = 0;
                for (int i = 0; i < result; i++)
                {
                    var instance = Instantiate(operation.Result, new Vector3(position.x, position.y + yOffset, position.z), rotation);
                    instance.transform.parent = null;
                    yOffset += Mathf.FloorToInt(instance.GetComponentInChildren<Collider>().bounds.size.y);
                }
            } 
            else
            {
                var instance = Instantiate(operation.Result, position, rotation);
                instance.transform.parent = null;
            }
        } 
        else
        {
            var instance = Instantiate(operation.Result, position, rotation);
            instance.transform.parent = null;
        }
    }

    private void OnDestroy()
    {
        Addressables.Release(handle);
    }
}
