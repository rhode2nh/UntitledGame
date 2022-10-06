using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class Inventory : ScriptableObject
{
    public int maxSize = 0;
    public List<InventorySlot> items = new List<InventorySlot>();
}

[System.Serializable]
public struct Properties
{
    public string key;
    public object value;
}

[System.Serializable]
public class InventorySlot
{
    public int id;
    public Item item;
    public int count;
    [SerializeField]
    private List<Properties> _properties;
    public Dictionary<string, object> properties;

    public InventorySlot(int id, Item item, int count, Dictionary<string, object> properties = null)
    {
        this.id = id;
        this.item = item;
        this.count = count;
        this.properties = properties.CopyProperties();
        this._properties = new List<Properties>();

        // Show key value pairs in the inspector.
        SerializeProperties();
    }

    public InventorySlot(InventorySlot item)
    {
        this.id = item.id;
        this.item = item.item;
        this.count = item.count;
        this.properties = item.properties.CopyProperties() ?? new Dictionary<string, object>(); 
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

