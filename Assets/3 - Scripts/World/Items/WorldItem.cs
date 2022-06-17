using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public string itemName;
    public Item item;
    public int count;
    public Dictionary<string, object> properties;
    //ItemStats itemStats;

    // Start is called before the first frame update
    void Start()
    {
        tag = Constants.WORLD_ITEM;
        count = 1;
        properties = new Dictionary<string, object>();
        InitializeProperties();
    }

    void InitializeProperties()
    {
        switch(item)
        {
            case IWeapon w:
                properties.Add(Constants.P_W_MODIFIERS_LIST, new List<TestModifier>());
                goto default;
            default:
                properties.Add(Constants.P_I_ID_STRING, this.GetInstanceID());
                break;
        }
    }
}
