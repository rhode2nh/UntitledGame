using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    public Item emptyItem;

    public event Action<Slot> onAddItemToPlayerInventory;
    public event Action<Slot, int> onAddItemToPlayerInventoryAtIndex;
    public event Action<Slot> onAddItemToImplantInventory;
    public event Action<List<Slot>> onUpdateInventoryGUI;
    public event Action<List<Slot>> onUpdateImplantGUI;
    public event Action<List<Slot>, List<int>, int> onUpdateModifierGUI;
    public event Action<List<Slot>> onUpdateWeaponGUI;
    public event Action<string[]> onUpdateWeaponStatsGUI;
    public event Action onIsCastDelayBarLoading;
    public event Action onIsRechargeDelayBarLoading;
    public event Action onStopLoadingBars;
    public event Func<Recipe, bool> onCanCraft;
    public event Func<Recipe, bool> onCraft;
    public event Action<Item> onConsume;
    public event Func<string, Slot> onGetItem;
    public event Func<string, bool> onHasItem;
    public event Action<string> onEquip;
    public event Action<Slot, int> onEquipAtIndex;
    public event Action onEquipFirstOccurence;
    public event Action onUnEquipFirstOccurence;
    public event Func<string, Slot> onUnequip;
    public event Func<string, Slot> onRemoveItemFromPlayerInventory;
    public event Func<string, Slot> onRemoveImplant;
    public event Action onClearInventory;
    public event Func<string, System.Type[], bool> onCheckType;
    public event Func<System.Type, Slot> onRemoveItemByType;
    public event Func<List<Slot>> onGetAllModifiers;
    public event Action onUpdateEquipmentContainer;
    public event Action<int> onSpawnObject;
    public event Func<int, int, Slot> onRemoveModifierFromWeapon;
    public event Func<string, Slot> onRemoveWeaponFromEquipmentInventory;
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
    public event Func<int> onGetCurEquipmentIndex;
    public event Func<bool> onIsPlayerDead;
    public event Action<int> onHurtPlayer;
    public event Action<Slot, bool> onDropItem;
    public event Action<int, int> onSwitchInventoryItems;
    public event Action<int, int> onSwitchEquipmentItems;
    public event Action<int, int> onSwitchFromInventoryToEquipment;
    
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

    public void AddItemToPlayerInventoryAtIndex(Slot item, int index)
    {
        if (onAddItemToPlayerInventoryAtIndex != null)
        {
            onAddItemToPlayerInventoryAtIndex(item, index);
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

    public void UpdateWeaponGUI(List<Slot> weapons)
    {
        if (onUpdateWeaponGUI != null)
        {
            onUpdateWeaponGUI(weapons);
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

    public Slot GetItem(string id)
    {
        if (onGetItem != null)
        {
            return onGetItem(id);
        }

        return null;
    }

    public bool HasItem(string id)
    {
        if (onHasItem != null)
        {
            return onHasItem(id);
        }
        
        return false;
    }

    public void Equip(string id)
    {
        if (onEquip != null)
        {
            onEquip(id);
        }
    }

    public void EquipAtIndex(Slot slot, int index)
    {
        if (onEquipAtIndex != null)
        {
            onEquipAtIndex(slot, index);
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

    public Slot RemoveItemFromPlayerInventory(string id)
    {
        if (onRemoveItemFromPlayerInventory != null)
        {
            return onRemoveItemFromPlayerInventory(id);
        }

        return null;
    }

    public Slot RemoveImplant(string id)
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

    public bool CheckType(string id, params Type[] types)
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

    public Slot Unequip(string id)
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

    public Slot RemoveWeaponFromEquipmentInventory(string id)
    {
        if (onRemoveWeaponFromEquipmentInventory != null)
        {
            return onRemoveWeaponFromEquipmentInventory(id);
        }
        else {
            return GetEmptySlot();
        }
    }

    public Slot RemoveModifierFromWeapon(int index, int equipmentId)
    {
        if (onRemoveModifierFromWeapon != null)
        {
            return onRemoveModifierFromWeapon(index, equipmentId);
        } 
        else {
            return GetEmptySlot();
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
        return new Slot(emptyItem, 1);
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

    public int GetCurEquipmentIndex()
    {
        if (onGetCurEquipmentIndex != null)
        {
            return onGetCurEquipmentIndex();
        }
        else
        {
            return 0;
        }
    }

    public bool IsPlayerDead()
    {
        if (onIsPlayerDead != null)
        {
            return onIsPlayerDead();
        }
        else
        {
            return false;
        }
    }

    public void HurtPlayer(int damage)
    {
        if (onHurtPlayer != null)
        {
            onHurtPlayer(damage);
        }
    }

    public void DropItem(Slot item, bool useMousePos = false) {
        if (onDropItem != null) {
            onDropItem(item, useMousePos);
        }
    }

    public void SwitchInventoryItems(int index1, int index2) {
        if (onSwitchInventoryItems != null) {
            onSwitchInventoryItems(index1, index2);
        }
    }

    public void SwitchEquipmentItems(int index1, int index2) {
        if (onSwitchEquipmentItems != null) {
            onSwitchEquipmentItems(index1, index2);
        }
    }
}
