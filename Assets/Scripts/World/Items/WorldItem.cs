using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public string itemName;
    public Item itemReference;
    private Transform childObject;
    //ItemStats itemStats;

    // Start is called before the first frame update
    void Start()
    {
        childObject = transform.GetChild(0);
        childObject.tag = Constants.WORLD_ITEM;
        //itemStats = new ItemStats();
    }

    //public void UpdateStatsOnInstantiation(ItemStats newItemStats)
    //{
    //    this.itemStats = newItemStats;
    //}

    //public ItemStats getItemStats()
    //{
    //    return itemStats;
    //}
    public void UpdateItemReference(Item itemReference)
    {
        this.itemReference.Name = itemReference.Name;
        this.itemReference.Id = itemReference.Id;
        this.itemReference.Count = itemReference.Count;
        this.itemReference.IsStackable = itemReference.IsStackable;
    }
}
