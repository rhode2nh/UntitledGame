using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImplantManager : MonoBehaviour, IDataPersistence
{
    public static ImplantManager instance;
    public Inventory implantInventory;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Equipment Inventory Manager in the scene.");
        }
        instance = this;

        GameEvents.current.onAddItemToImplantInventory += AddItem;
        GameEvents.current.onClearInventory += ClearInventory;
        GameEvents.current.onRemoveImplant += RemoveImplant;
        GameEvents.current.onGetImplantStats += GetImplantStats;
    }

    public void AddItem(Slot slot)
    {
        if (slot.item is not IImplant) {
            return;
        }
        var imp = slot.item as IImplant;
        int impIndex = (int)imp.BodyPart;
        var buffedStats = GameEvents.current.GetBuffedStats();
        var requiredStats = (TestStats)slot.properties[Constants.P_IMP_REQUIRED_STATS_DICT]; 
        if (buffedStats < requiredStats)
        {
            Debug.Log("Can't equip implant. Requirements not met.");
            Debug.Log("Current Stats:\nagility: " + buffedStats.agility + "\nstrength: " + buffedStats.strength);
            Debug.Log("Required Stats:\nagility: " + requiredStats.agility + "\nstrength: " + requiredStats.strength);
            return;
        }
        if (implantInventory.items[impIndex].item == GameEvents.current.GetEmptyItem())
        {
            implantInventory.items[impIndex] = new Slot(slot);       
            GameEvents.current.RemoveItemFromPlayerInventory(slot.id);
            GameEvents.current.UpdateImplantGUI(implantInventory.items);
            GameEvents.current.CalculateBuffedStats();
            var newBuffedStats = GameEvents.current.GetBuffedStats();
            GameEvents.current.UpdateStatsPanel(newBuffedStats);
        }
    }

    public Slot RemoveImplant(string id)
    {
        Slot implantToRemove = implantInventory.items.FirstOrDefault(x => x.id == id);
        if (implantToRemove == null) {
            return GameEvents.current.GetEmptySlot();
        }
        int index = implantInventory.items.FindIndex(x => x.id == id);
        implantInventory.items[index] = new Slot(GameEvents.current.GetEmptyItem(), 1);
        GameEvents.current.CalculateBuffedStats();
        GameEvents.current.UpdateImplantGUI(implantInventory.items);
        var newBuffedStats = GameEvents.current.GetBuffedStats();
        GameEvents.current.UpdateStatsPanel(newBuffedStats);
        return implantToRemove;
    }

    public void ClearInventory()
    {
        for (int i = 0; i < implantInventory.items.Count; i++) {
            implantInventory.items[i] = GameEvents.current.GetEmptySlot();
        }
    }

    public List<TestStats> GetImplantStats()
    {
        return implantInventory.items.Where(x => x.item != GameEvents.current.GetEmptyItem()).Select(x => (TestStats)x.properties[Constants.P_IMP_STATS_DICT]).ToList();
    }

    public void SaveData(ref GameData data)
    {
        try {
            data.implantItemsData.Clear();
            foreach (var slot in implantInventory.items)
            {
                data.implantItemsData.Add(StateManager.SaveItemData(slot));
            }
            implantInventory.items.Clear();
        }
        catch (Exception e)
        {
            Debug.Log(e);
            implantInventory.items.Clear();
        }
    }

    public void LoadData(GameData data)
    {
        if (data.implantItemsData.Count == 0 || data.implantItemsData.All(x => x.itemId == "-1"))
        {
            implantInventory.InitializeInventory();
        }
        else
        {
            foreach (var itemData in data.implantItemsData)
            {
                implantInventory.items.Add(StateManager.LoadItemData(itemData));
            }
        }
        Debug.Log(implantInventory.items.Count);
        GameEvents.current.UpdateImplantGUI(implantInventory.items);
        GameEvents.current.CalculateBuffedStats();
        var newBuffedStats = GameEvents.current.GetBuffedStats();
        GameEvents.current.UpdateStatsPanel(newBuffedStats);
    }

}
