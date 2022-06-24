using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public Item item;
    public int count;
    public int id;
    public Dictionary<string, object> properties;
    //ItemStats itemStats;

    // Start is called before the first frame update
    void Awake()
    {
        tag = Constants.WORLD_ITEM;
        count = 1;
        id = this.GetInstanceID();
        properties = new Dictionary<string, object>();
        InitializeProperties();
    }

    void InitializeProperties()
    {
        switch(item)
        {
            case IWeapon w:
                properties.Add(Constants.P_W_MODIFIERS_LIST, new List<Modifier>());
                goto default;
            default:
                break;
        }
    }
}
