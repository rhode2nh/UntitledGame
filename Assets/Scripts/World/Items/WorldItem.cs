using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public string itemName;
    public ItemObject item;
    private Transform childObject;
    public int count;
    //ItemStats itemStats;

    // Start is called before the first frame update
    void Start()
    {
        childObject = transform.GetChild(0);
        childObject.tag = Constants.WORLD_ITEM;
        count = 1;
        //itemStats = new ItemStats();
    }

    void UpdateCount(int _count)
    {
        this.count = _count;
    }
}
