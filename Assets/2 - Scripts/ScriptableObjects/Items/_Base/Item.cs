using UnityEngine;
using System;

/// <summary>
/// The base entity that exists for things in the world.
/// </summary>
public abstract class Item : ScriptableObject
{
    public string Id;
    public string Name;
    public GameObject worldItemPrefab;
    public Mesh equipmentMesh;
    public GameObject testPrefab;
    public Sprite sprite;
    public bool isStackable;

    protected Item()
    {
        Id = Guid.NewGuid().ToString();
    }
}
