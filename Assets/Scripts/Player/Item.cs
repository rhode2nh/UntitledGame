using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    //public int Id { get; set; }
    //public int Count { get; set; }
    //public string Name { get; set; }
    //public ItemType type { get; set; }
    //public bool IsStackable { get; set; }
    public int Id;
    public int Count;
    public string Name;
    public ItemType type;
    public bool IsStackable;
}
