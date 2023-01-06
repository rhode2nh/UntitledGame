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
                properties.Add(Constants.P_W_MODIFIER_SLOT_INDICES_LIST, new List<int>());
                properties.Add(Constants.P_W_MAX_SLOTS_INT, 10);
                goto default;
            case IImplant i:
                properties.Add(Constants.P_IMP_QUALITY_LEVEL_INT, i.QualityLevel);
                properties.Add(Constants.P_IMP_BODY_PART_IMPLANTTYPE, i.BodyPart);
                goto default;

            default:
                break;
        }
    }
}
