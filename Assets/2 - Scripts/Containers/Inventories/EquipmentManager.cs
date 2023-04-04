using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentManager : MonoBehaviour, IDataPersistence
{
    public Inventory equipmentInventory;
    private bool hasItem = false;

    private void Start()
    {
        GameEvents.current.onEquip += Equip;
        GameEvents.current.onEquipFirstOccurence += Equip;
        GameEvents.current.onUnEquipFirstOccurence += UnEquipFirstOccurence;
        GameEvents.current.onUnequip += Unequip;
    }

    /// <summary>
    /// Adds by id to the equipment inventory.
    /// </summary>
    /// <param name="id">The item id.</param>
    public void Equip(string id)
    {
        if (equipmentInventory.AvailableSlots() == 0)
            return;
        if (!GameEvents.current.CheckType(id, typeof(IEquippable)))
        {
            Debug.Log("Item is not equippable");
            return;
        }

        Slot itemToEquip = GameEvents.current.RemoveItemFromPlayerInventory(id);
        if (!hasItem)
        {
            for (int i = 0; i < equipmentInventory.maxSize; i++)
            {
                if (equipmentInventory.items[i].item == GameEvents.current.GetEmptyItem())
                {
                    equipmentInventory.items[i] = itemToEquip;
                    break;
                }
            }
        }
        hasItem = false;
    }

    /// <summary>
    /// Removes the first occurence of an equippable item and moves it back to the player inventory
    /// </summary>
    public void UnEquipFirstOccurence()
    {
        if (equipmentInventory.items.Count() != 0)
        {
            var itemToMove = equipmentInventory.items[0];
            GameEvents.current.AddItemToPlayerInventory(itemToMove);
            equipmentInventory.items.RemoveAt(0);
        }
    }
    
    /// <summary>
    /// Equip the first item found in the player inventory.
    /// </summary>
    public void Equip()
    {
        if (equipmentInventory.items.Count >= equipmentInventory.maxSize)
            return;

        Slot itemToEquip = GameEvents.current.RemoveItemByType(typeof(IEquippable));
        equipmentInventory.items.Add(itemToEquip);
    }

    /// <summary>
    /// Removes an item from the equipment inventory.
    /// </summary>
    public Slot Unequip(string id)
    {
        Slot itemToUnequip = new Slot(equipmentInventory.items.FirstOrDefault(x => x.id == id));
        if (itemToUnequip.item == GameEvents.current.GetEmptyItem())
        {
            return itemToUnequip;
        }
        int index = equipmentInventory.items.FindIndex(x => x.id == id);
        equipmentInventory.items[index] = new Slot(GameEvents.current.GetEmptyItem(), 1);
        return itemToUnequip;
    }

    /// <summary>
    /// Get the number of unique items in the inventory.
    /// </summary>
    /// <returns>The number of unique items.</returns>
    public int Count()
    {
        return equipmentInventory.items.Count;
    }

    /// <summary>
    /// Remove an item at the specified index in the inventory.
    /// </summary>
    /// <param name="index">The location to remove the item.</param>
    /// <returns>The item removed.</returns>
    public Item RemoveItem(int index)
    {
        if (equipmentInventory.items[index].count == 1)
        {
            Item item = equipmentInventory.items[index].item;
            equipmentInventory.items.RemoveAt(index);
            return item;
        }
        else
        {
            equipmentInventory.items[index].count--;
            Item item = equipmentInventory.items[index].item;
            return item;
        }
    }

    public Item RemoveItem(string name, int count = 1)
    {
        Slot removedItem = equipmentInventory.items.FirstOrDefault(x => x.item.Name == name.ToUpper());
        if (removedItem.count - count <= 0)
        {
            equipmentInventory.items.Remove(removedItem);
        }
        else
        {
            removedItem.count -= count;
        }

        GameEvents.current.UpdateInventoryGUI(equipmentInventory.items);
        return removedItem.item;
    }

    public Item GetItem(string name)
    {
        Slot removedItem = equipmentInventory.items.FirstOrDefault(x => x.item.Name == name.ToUpper());
        return removedItem.item;
    }

    public Slot GetItem(int index)
    {
        return new Slot(equipmentInventory.items[index]);
    }

    /// <summary>
    /// Remove the specified item from the inventory.
    /// </summary>
    /// <param name="item">The item to remove from the inventory.</param>
    /// <returns>The item removed.</returns>
    public Item RemoveItem(Item item, int count = 1)
    {
        Slot removedItem = equipmentInventory.items.FirstOrDefault(x => x.item == item);
        if (removedItem.count - count <= 0)
        {
            equipmentInventory.items.Remove(removedItem);
        }
        else
        {
            removedItem.count -= count;
        }

        GameEvents.current.UpdateInventoryGUI(equipmentInventory.items);
        return removedItem.item;
    }

    public bool IsEmpty()
    {
        return !equipmentInventory.items.Any();
    }

    public int MaxSize()
    {
        return equipmentInventory.maxSize;
    }

    public List<Slot> GetAllEquipment()
    {
        List<Slot> equipment = new List<Slot>();
        foreach (var item in equipmentInventory.items)
        {
            equipment.Add(item);
        }
        return equipment;
    }

    public void ClearInventory()
    {
        equipmentInventory.items.Clear();
    }

    public void SaveData(ref GameData data)
    {
        try {
            data.equippedItemsData.Clear();
            foreach (var slot in equipmentInventory.items)
            {
                data.equippedItemsData.Add(StateManager.SaveItemData(slot));
            }
            equipmentInventory.items.Clear();
        }
        catch (Exception e)
        {
            Debug.Log(e);
            equipmentInventory.items.Clear();
        }
    }

    public void LoadData(GameData data)
    {
        if (data.equippedItemsData.Count == 0 || data.equippedItemsData.All(x => x.itemId == "-1"))
        {
            equipmentInventory.InitializeInventory();
        }
        else
        {
            foreach (var itemData in data.equippedItemsData)
            {
                equipmentInventory.items.Add(StateManager.LoadItemData(itemData));
            }
        }
        GameEvents.current.UpdateEquipmentContainer();
    }
}
