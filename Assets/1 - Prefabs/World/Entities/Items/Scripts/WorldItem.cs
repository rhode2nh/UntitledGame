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
    void Start()
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
                var slots = new List<Slot>();
                properties.Add(Constants.P_W_MODIFIER_SLOT_INDICES_LIST, new List<int>());
                properties.Add(Constants.P_W_MAX_SLOTS_INT, 10);
                for (int i = 0; i < 10; i++)
                {
                    slots.Add(new Slot(GameEvents.current.GetEmptySlot()));
                }
                properties.Add(Constants.P_W_MODIFIERS_LIST, slots);
                goto default;
            case IImplant i:
                properties.Add(Constants.P_IMP_QUALITY_LEVEL_INT, i.QualityLevel);
                properties.Add(Constants.P_IMP_BODY_PART_IMPLANTTYPE, i.BodyPart);
                properties.Add(Constants.P_IMP_STATS_DICT, new TestStats(1, 1));
                goto default;

            default:
                break;
        }
    }
}
