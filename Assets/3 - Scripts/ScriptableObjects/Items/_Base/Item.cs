using UnityEngine;

/// <summary>
/// The base entity that exists for things in the world.
/// </summary>
public abstract class Item : ScriptableObject
{
    public int Id;
    public string Name;
    public GameObject prefab;
    public Sprite sprite;
    public bool isStackable;
}
