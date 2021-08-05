using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    CONSUMABLE,
    WEARABLE,
    TEST
}
public abstract class ItemObject : ScriptableObject
{
    public int Id;
    public string Name;
    public ItemType type;
    public GameObject prefab;
}
