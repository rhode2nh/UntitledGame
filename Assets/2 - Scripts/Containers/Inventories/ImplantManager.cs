using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImplantManager : MonoBehaviour
{
    public Inventory implantInventory;
    // Start is called before the first frame update
    void Start()
    {
        implantInventory.InitializeInventory();
        GameEvents.current.onAddItemToImplantInventory += AddItem;
        GameEvents.current.onClearInventory += ClearInventory;
        GameEvents.current.onRemoveImplant += RemoveImplant;
        GameEvents.current.onGetImplantStats += GetImplantStats;
    }

    public void AddItem(Slot slot)
    {
        var imp = slot.item as IImplant;
        int impIndex = (int)imp.BodyPart;
        if (implantInventory.items[impIndex].item == GameEvents.current.GetEmptyItem())
        {
            implantInventory.items[impIndex] = new Slot(slot);       
            GameEvents.current.RemoveItemFromPlayerInventory(slot.id);
            GameEvents.current.UpdateImplantGUI(implantInventory.items);
            GameEvents.current.CalculateBuffedStats();
        }
    }

    public Slot RemoveImplant(int id)
    {
        Slot implantToRemove = new Slot(implantInventory.items.FirstOrDefault(x => x.id == id));
        int index = implantInventory.items.FindIndex(x => x.id == id);
        implantInventory.items[index] = new Slot(-1, GameEvents.current.GetEmptyItem(), 1);
        GameEvents.current.UpdateImplantGUI(implantInventory.items);
        return implantToRemove;
    }

    public void ClearInventory()
    {
        implantInventory.items.Clear();
    }

    public List<TestStats> GetImplantStats()
    {
        return implantInventory.items.Where(x => x.item != GameEvents.current.GetEmptyItem()).Select(x => (TestStats)x.properties[Constants.P_IMP_STATS_DICT]).ToList();
    }

}
