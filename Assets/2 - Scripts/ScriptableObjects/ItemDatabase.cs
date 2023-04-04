using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Stores all SO items.
/// </summary>
[CreateAssetMenu(fileName = "New Item Database", menuName = "Database/Item Database", order = 1)]
public class ItemDatabase : ScriptableObject
{
    [SerializeField]
    public List<Item> itemDatabase;

    public Item GetItem(string id)
    {
        return itemDatabase.Where(x => x.Id == id).First();
    }
}
