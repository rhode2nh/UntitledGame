using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Stores all SO items.
/// </summary>
[CreateAssetMenu(fileName = "Prefab Database", menuName = "Database/Prefab Database", order = 1)]
public class PrefabDatabase : ScriptableObject
{
    [SerializeField]
    public List<GameObject> prefabDatabase;

    public GameObject GetItem(string itemId)
    {
        return prefabDatabase.Where(x => x.GetComponent<WorldItem>().item.Id == itemId).First();
    }
}
