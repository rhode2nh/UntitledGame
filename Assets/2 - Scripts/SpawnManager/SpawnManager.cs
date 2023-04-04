using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour, IDataPersistence
{
    public void LoadData(GameData data)
    {
        foreach (var itemToSpawn in data.instancedItemsData)
        {
            GameObject prefab = DatabaseManager.instance.GetPrefabItem(itemToSpawn.itemId);
            Instantiate(prefab, itemToSpawn.pos, new Quaternion());
            prefab.GetComponent<WorldItem>().isInstance = true;
        }
        data.instancedItemsData.Clear();
    }

    public void SaveData(ref GameData data)
    {

    }
}
