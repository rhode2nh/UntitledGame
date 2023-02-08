using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;

public enum ItemType {
    Modifier,
    Implant,
    Equippable,
}

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class Inventory : ScriptableObject
{
    public int maxSize = 0;
    public List<ItemType> allowedItems = new List<ItemType>();
    public List<Slot> items = new List<Slot>();

    public void InitializeInventory()
    {
        for (int i = 0; i < maxSize; i++)
        {
            items.Add(new Slot(-1, GameEvents.current.GetEmptyItem(), 1));
        }
    }

    public int AvailableSlots()
    {
        int availableSlots = 0;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item.Id == -1)
            {
                availableSlots++;
            }
        }
        return availableSlots;
    }
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

    public Slot(int id = -1, Item item = null, int count = 1, Dictionary<string, object> properties = null)
    {
        this.id = id;
        this.item = item;
        this.count = count;
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

