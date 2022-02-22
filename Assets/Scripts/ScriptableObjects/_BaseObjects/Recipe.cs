using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct ItemAmount
{
    public Item item;
    [Range(1, 999)]
    public int count;
}

[CreateAssetMenu(fileName = "New Recipe", menuName = "Recipes/Recipe")]
public class Recipe : ScriptableObject
{
    public List<ItemAmount> RequiredItems;
    public List<ItemAmount> Results;

    public bool CanCraft(Inventory inventory)
    {
        return RequiredItems.All(x => inventory.HasItem(x.item, x.count));
    }

    public bool Craft(Inventory inventory)
    {
        if (CanCraft(inventory))
        {
            RequiredItems.ForEach(x => inventory.RemoveItem(x.item, x.count));
            Results.ForEach(x => inventory.AddItem(x.item, x.count));
            return true;
        }

        return false;
    }
}
