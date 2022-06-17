using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class Inventory : ScriptableObject
{
    public List<InventorySlot> items = new List<InventorySlot>();
    public int maxSize = 0;
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
    public Item item;
    public int count;
    [SerializeField]
    private List<Properties> _properties;
    public Dictionary<string, object> properties;

    public InventorySlot(Item item, int count, Dictionary<string, object> properties = null)
    {
        this.item = item;
        this.count = count;
        this.properties = properties ?? new Dictionary<string, object>();
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

