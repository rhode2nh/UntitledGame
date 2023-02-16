using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    public Item emptyItem;

    public event Action<Slot> onAddItemToPlayerInventory;
    public event Action<Slot> onAddItemToImplantInventory;
    public event Action<List<Slot>> onUpdateInventoryGUI;
    public event Action<List<Slot>> onUpdateImplantGUI;
    public event Action<List<Slot>, List<int>, int> onUpdateModifierGUI;
    //TODO: CREATE A GUN SO!!!!
    public event Action<List<Slot>, int> onUpdateWeaponGUI;
    public event Action<string[]> onUpdateWeaponStatsGUI;
    public event Action onIsCastDelayBarLoading;
    public event Action onIsRechargeDelayBarLoading;
    public event Action onStopLoadingBars;
    public event Func<Recipe, bool> onCanCraft;
    public event Func<Recipe, bool> onCraft;
    public event Action<Item> onConsume;
    public event Func<int, Slot> onGetItem;
    public event Func<int, bool> onHasItem;
    public event Action<int> onEquip;
    public event Action onEquipFirstOccurence;
    public event Action onUnEquipFirstOccurence;
    public event Func<int, Slot> onUnequip;
    public event Func<int, Slot> onRemoveItemFromPlayerInventory;
    public event Func<int, Slot> onRemoveImplant;
    public event Action onClearInventory;
    public event Func<int, System.Type[], bool> onCheckType;
    public event Func<System.Type, Slot> onRemoveItemByType;
    public event Func<List<Slot>> onGetAllModifiers;
    public event Action onUpdateEquipmentContainer;
    public event Action<int> onSpawnObject;
    public event Action<int, int> onRemoveModifierFromWeapon;
    public event Action<int> onRemoveWeaponFromEquipmentInventory;
    public event Func<bool, Slot> onGetCurrentWeapon;
    public event Func<int, Slot> onGetCurrentWeaponFromSlot;
    public event Action<Slot> onUpdateCurrentWeapon;
    public event Action<string> onUpdateHoverText;
    public event Action<int, int> onSwitchActiveEquipmentUISlot;
    public event Func <List<TestStats>> onGetImplantStats;
    public event Action onCalculateBuffedStats;
    public event Func<TestStats> onGetBuffedStats;
    public event Action onDeactivateInfoPanel;
    public event Action onActivateInfoPanel;
    public event Action<string> onSetInfoText;
    public event Action<int, int> onUpdateStatsPanel;
    
    public void Awake()
    {
        current = this;
    }

    public void AddItemToPlayerInventory(Slot item)
    {
        if (onAddItemToPlayerInventory != null)
        {
            onAddItemToPlayerInventory(item);
        }
    }

    public void AddItemToImplantInventory(Slot item)
    {
        if (onAddItemToImplantInventory != null)
        {
            onAddItemToImplantInventory(item);
        }
    }

    public void UpdateInventoryGUI(List<Slot> items)
    {
        if (onUpdateInventoryGUI != null)
        {
            onUpdateInventoryGUI(items);
        }
    }

    public void UpdateImplantGUI(List<Slot> items)
    {
        if (onUpdateImplantGUI != null)
        {
            onUpdateImplantGUI(items);
        }
    }

    public void UpdateModifierGUI(List<Slot> items, List<int> modifierSlotIndices, int maxSlots)
    {
        if (onUpdateModifierGUI != null)
        {
            onUpdateModifierGUI(items, modifierSlotIndices, maxSlots);
        }
    }

    public void UpdateWeaponGUI(List<Slot> weapons, int maxSlots)
    {
        if (onUpdateWeaponGUI != null)
        {
            onUpdateWeaponGUI(weapons, maxSlots);
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

    public Slot GetItem(int id)
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

    public Slot RemoveItemFromPlayerInventory(int id)
    {
        if (onRemoveItemFromPlayerInventory != null)
        {
            return onRemoveItemFromPlayerInventory(id);
        }

        return null;
    }

    public Slot RemoveImplant(int id)
    {
        if (onRemoveImplant != null)
        {
            return onRemoveImplant(id);
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

    public Slot RemoveItemByType(Type type)
    {
        if (onRemoveItemByType != null)
        {
            return onRemoveItemByType(type);
        }

        return null;
    }

    public List<Slot> GetAllModifiers()
    {
        if (onGetAllModifiers != null)
        {
            return onGetAllModifiers();
        }

        return new List<Slot>();
    }

    public Slot Unequip(int id)
    {
        if (onUnequip != null)
        {
            return onUnequip(id);
        }

        return null;
    }

    public Slot GetCurrentWeapon()
    {
        if (onGetCurrentWeapon != null)
        {
            return onGetCurrentWeapon(false);
        }

        return null;
    }

    public Slot GetCurrentWeaponFromSlot(int index)
    {
        if (onGetCurrentWeaponFromSlot != null)
        {
            return onGetCurrentWeaponFromSlot(index);
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

    public void StopLoadingBars()
    {
        if (onStopLoadingBars != null)
        {
            onStopLoadingBars();
        }
    }

    public void UpdateCurrentWeapon(Slot updatedWeapon)
    {
        if (onUpdateCurrentWeapon != null)
        {
            onUpdateCurrentWeapon(updatedWeapon);
        }
    }

    public void RemoveWeaponFromEquipmentInventory(int index)
    {
        if (onRemoveWeaponFromEquipmentInventory != null)
        {
            onRemoveWeaponFromEquipmentInventory(index);
        }
    }

    public void RemoveModifierFromWeapon(int index, int equipmentId)
    {
        if (onRemoveModifierFromWeapon != null)
        {
            onRemoveModifierFromWeapon(index, equipmentId);
        }
    }

    public void UpdateHoverText(string text)
    {
        if (onUpdateHoverText != null)
        {
            onUpdateHoverText(text);
        }
    }
    
    public Item GetEmptyItem()
    {
        return current.emptyItem;
    }

    public Slot GetEmptySlot()
    {
        return new Slot(-1, emptyItem, 1);
    }

    public void SwitchActiveEquipmentUISlot(int prevEquipmentUISlot, int curEquipmentUISlot)
    {
        if (onSwitchActiveEquipmentUISlot != null)
        {
            onSwitchActiveEquipmentUISlot(prevEquipmentUISlot, curEquipmentUISlot);
        }
    }

    public List<TestStats> GetImplantStats()
    {
        if (onGetImplantStats != null)
        {
            return onGetImplantStats();
        }

        return new List<TestStats>();
    }

    public void CalculateBuffedStats()
    {
        if (onCalculateBuffedStats != null)
        {
            onCalculateBuffedStats();
        }
    }

    public void DeactivateInfoPanel()
    {
        if (onDeactivateInfoPanel != null)
        {
            onDeactivateInfoPanel();
        }
    }

    public void ActivateInfoPanel()
    {
        if (onActivateInfoPanel != null)
        {
            onActivateInfoPanel();
        }
    }

    public void SetInfoText(string newText)
    {
        if (onSetInfoText != null)
        {
            onSetInfoText(newText);
        }
    }

    public TestStats GetBuffedStats()
    {
        if (onGetBuffedStats != null)
        {
            return onGetBuffedStats();
        }
        else
        {
            return new TestStats();
        }
    }

    public void UpdateStatsPanel(int agility, int strength)
    {
        if (onUpdateStatsPanel != null)
        {
            onUpdateStatsPanel(agility, strength);
        }
    }
}
