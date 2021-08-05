using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public string itemName;
    public ItemObject item;
    public int count;
    //ItemStats itemStats;

    // Start is called before the first frame update
    void Start()
    {
        tag = Constants.WORLD_ITEM;
        count = 1;
    }

    void UpdateCount(int _count)
    {
        this.count = _count;
    }
}
