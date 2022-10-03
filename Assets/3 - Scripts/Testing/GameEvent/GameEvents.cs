using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    public event Action<InventorySlot> onAddItemToPlayerInventory;
    public event Action<List<InventorySlot>> onUpdateInventoryGUI;
    public event Action<List<Modifier>, int> onUpdateModifierGUI;
    public event Action<string[]> onUpdateWeaponStatsGUI;
    public event Action onIsCastDelayBarLoading;
    public event Action onIsRechargeDelayBarLoading;
    public event Func<Recipe, bool> onCanCraft;
    public event Func<Recipe, bool> onCraft;
    public event Action<Item> onConsume;
    public event Func<int, InventorySlot> onGetItem;
    public event Func<int, bool> onHasItem;
    public event Action<int> onEquip;
    public event Action onEquipFirstOccurence;
    public event Action onUnEquipFirstOccurence;
    public event Func<int, InventorySlot> onUnequip;
    public event Func<int, InventorySlot> onRemoveItemFromPlayerInventory;
    public event Action onClearInventory;
    public event Func<int, System.Type[], bool> onCheckType;
    public event Func<System.Type, InventorySlot> onRemoveItemByType;
    public event Func<List<InventorySlot>> onGetAllModifiers;
    public event Action onUpdateEquipmentContainer;
    public event Action<int> onSpawnObject;
    public event Action<int, int> onRemoveModifierFromWeapon;

    public void Awake()
    {
        current = this;
    }

    public void AddItemToPlayerInventory(InventorySlot item)
    {
        if (onAddItemToPlayerInventory != null)
        {
            onAddItemToPlayerInventory(item);
        }
    }

    public void UpdateInventoryGUI(List<InventorySlot> items)
    {
        if (onUpdateInventoryGUI != null)
        {
            onUpdateInventoryGUI(items);
        }
    }

    public void UpdateModifierGUI(List<Modifier> items, int maxSlots)
    {
        if (onUpdateModifierGUI != null)
        {
            onUpdateModifierGUI(items, maxSlots);
        }
    }

    public void UpdateWeaponStatsGUI(string[] stats)
    {
        if (onUpdateWeaponStatsGUI != null)
        {
            onUpdateWeaponStatsGUI(stats);
        }
    }

    public bool CanCraft(Recipe recipe)
    {
        if (onCanCraft != null)
        {
            return onCanCraft(recipe);
        }

        return false;
    }

    public bool Craft(Recipe recipe)
    {
        if (onCraft != null)
        {
            return onCraft(recipe);
        }

        return false;
    }

    public void Consume(Item item)
    {
        if (onConsume != null)
        {
            onConsume(item);
        }
    }

    public InventorySlot GetItem(int id)
    {
        if (onGetItem != null)
        {
            return onGetItem(id);
        }

        return null;
    }

    public bool HasItem(int id)
    {
        if (onHasItem != null)
        {
            return onHasItem(id);
        }
        
        return false;
    }

    public void Equip(int id)
    {
        if (onEquip != null)
        {
            onEquip(id);
        }
    }

    public void EquipFirstOccurence()
    {
        if (onEquipFirstOccurence != null)
        {
            onEquipFirstOccurence();
        }
    }

    public void UnEquipFirstOccurence()
    {
        if (onUnEquipFirstOccurence != null)
        {
            onUnEquipFirstOccurence();
        }
    }

    public InventorySlot RemoveItemFromPlayerInventory(int id)
    {
        if (onRemoveItemFromPlayerInventory != null)
        {
            return onRemoveItemFromPlayerInventory(id);
        }

        return null;
    }

    public void ClearInventory()
    {
        if (onClearInventory != null)
        {
            onClearInventory();
        }
    }

    public bool CheckType(int id, params Type[] types)
    {
        if (onCheckType != null)
        {
            return onCheckType(id, types);
        }

        return false;
    }

    public InventorySlot RemoveItemByType(Type type)
    {
        if (onRemoveItemByType != null)
        {
            return onRemoveItemByType(type);
        }

        return null;
    }

    public List<InventorySlot> GetAllModifiers()
    {
        if (onGetAllModifiers != null)
        {
            return onGetAllModifiers();
        }

        return new List<InventorySlot>();
    }

    public InventorySlot Unequip(int id)
    {
        if (onUnequip != null)
        {
            return onUnequip(id);
        }

        return null;
    }

    public void UpdateEquipmentContainer()
    {
        if (onUpdateEquipmentContainer != null)
        {
            onUpdateEquipmentContainer();
        }
    }

    public void SpawnObject(int id)
    {
        if (onSpawnObject != null)
        {
            onSpawnObject(id);
        }
    }

    public void CastDelayBarLoading()
    {
        if (onIsCastDelayBarLoading != null)
        {
            onIsCastDelayBarLoading();
        }
    }

    public void RechargeDelayBarLoading()
    {
        if (onIsRechargeDelayBarLoading != null)
        {
            onIsRechargeDelayBarLoading();
        }
    }

    public void RemoveModifierFromWeapon(int index, int equipmentId)
    {
        if (onRemoveModifierFromWeapon != null)
        {
            onRemoveModifierFromWeapon(index, equipmentId);
        }
    }
}
