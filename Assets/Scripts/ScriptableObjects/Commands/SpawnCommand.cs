using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spawn Command", menuName = "Utilities/DeveloperConsole/Commands/Spawn Command")]
public class SpawnCommand : ConsoleCommand
{
    public Vector3 position;
    public Quaternion rotation;
    public override bool Process(string[] args)
    {
        if (args.Length == 0)
            return false;
        var load = "Prefabs/Entities/" + Constants.WORLD_ITEM_PREFIX + "_" + args[0];
        var itemToSpawn = Resources.Load(load);
        if (itemToSpawn == null)
        {
            Debug.Log("\"" + args[0] + "\"" + " does not exist.");
            return false;
        }
        if (args.Length == 2)
        {
            if (int.TryParse(args[1], out int result))
            {
                int yOffset = 0;
                for (int i = 0; i < result; i++)
                {
                    var instance = Instantiate(itemToSpawn, new Vector3(position.x, position.y + yOffset, position.z), rotation) as GameObject;
                    instance.transform.parent = null;
                    yOffset += Mathf.FloorToInt(instance.GetComponentInChildren<Collider>().bounds.size.y);
                }
            } 
            else
            {
                var instance = Instantiate(itemToSpawn, position, rotation) as GameObject;
                instance.transform.parent = null;
            }
        } 
        else
        {
            var instance = Instantiate(itemToSpawn, position, rotation) as GameObject;
            instance.transform.parent = null;
        }

        return true;
    }
}
