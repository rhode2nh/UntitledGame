using System;
using System.Collections.Generic;
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
}
