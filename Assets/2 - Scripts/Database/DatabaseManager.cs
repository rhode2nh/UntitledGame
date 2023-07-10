using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{

    public static DatabaseManager instance { get; private set; }
    public ItemDatabase itemDatabase;
    public PrefabDatabase prefabItemDatabase;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Database Manager in the scene.");
        }
        instance = this;
    }

    public Item GetItem(string id)
    {
        return itemDatabase.itemDatabase.Where(x => x.Id == id).First();
    }

    public GameObject GetPrefabItem(string itemId)
    {
        return prefabItemDatabase.GetItem(itemId);
    }

    public List<GameObject> GetGameObjects() {
        return prefabItemDatabase.prefabDatabase;
    }
}
