using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class Inventory : ScriptableObject
{
    public int maxSize = 0;
    public List<Slot> items = new List<Slot>();
}

[System.Serializable]
public struct Properties
{
    public string key;
    public object value;
}

[System.Serializable]
public class Slot
{
    public int id;
    public Item item;
    public int count;
    [SerializeField]
    private List<Properties> _properties;
    public Dictionary<string, object> properties;
    public int slotUIIndex;

    public Slot(int id, Item item, int count, int slotUIIndex = -1, Dictionary<string, object> properties = null)
    {
        this.id = id;
        this.item = item;
        this.count = count;
        this.slotUIIndex = slotUIIndex;
        this.properties = properties.CopyProperties();
        this._properties = new List<Properties>();

        // Show key value pairs in the inspector.
        SerializeProperties();
    }

    public Slot(Slot slot)
    {
        this.id = slot.id;
        this.item = slot.item;
        this.count = slot.count;
        this.slotUIIndex = slot.slotUIIndex;
        this.properties = slot.properties.CopyProperties() ?? new Dictionary<string, object>(); 
        this._properties = new List<Properties>();

        // Show key value pairs in the inspector.
        SerializeProperties();
    }

    public void AddCount(int value)
    {
        count += value;
    }

    public void SerializeProperties()
    {
        foreach(KeyValuePair<string, object> entry in properties)
        {
            Properties kvp = new Properties();
            kvp.key = entry.Key;
            kvp.value = entry.Value;
            _properties.Add(kvp);
        }
    }
}

