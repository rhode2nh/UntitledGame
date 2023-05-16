using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldItem : MonoBehaviour, IDataPersistence
{
    public Item item;
    public int count;
    public int id;
    public Dictionary<string, object> properties;
    public bool isInstance;
    private GameObject model;
    //ItemStats itemStats;

    // Start is called before the first frame update
    void Start()
    {
        tag = Constants.WORLD_ITEM;
        count = 1;
        id = this.GetInstanceID();
        properties = new Dictionary<string, object>();
        InitializeProperties();
        Physics.IgnoreLayerCollision(7, 8);
        Physics.IgnoreLayerCollision(8, 8);
        model = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        model.transform.Rotate(transform.up, 30 * Time.deltaTime);
    }

    public void LoadData(GameData data)
    {
        var item = data.itemsData.Find(x => x.name == this.name);
        if (item == null)
        {
            //Destroy(gameObject);
            return;
        }
        transform.position = item.pos;
    }

    public void SaveData(ref GameData data)
    {
        if (this == null)
        {
            return;
        }

        if (data.itemsData.Any(x => x.name == this.name))
        {
            data.itemsData.Where(x => x.name == this.name)
                .Select(x => x.pos = this.transform.position);
        }
        else
        {
            WorldItemData worldItemData = new WorldItemData();
            worldItemData.name = this.name;
            worldItemData.pos = this.transform.position;
            worldItemData.itemId = this.item.Id;
            if (isInstance)
            {
                data.instancedItemsData.Add(worldItemData);
            }
            else
            {
                data.itemsData.Add(worldItemData);
            }
        }
    }

    void InitializeProperties()
    {
        switch(item)
        {
            case IWeapon w:
                var slots = new List<Slot>();
                properties.Add(Constants.P_W_MODIFIER_SLOT_INDICES_LIST, new List<int>());
                properties.Add(Constants.P_W_MAX_SLOTS_INT, Random.Range(1, 10));
                for (int i = 0; i < (int)properties[Constants.P_W_MAX_SLOTS_INT]; i++)
                {
                    slots.Add(new Slot(GameEvents.current.GetEmptySlot()));
                }
                properties.Add(Constants.P_W_MODIFIERS_LIST, slots);
                goto default;
            case IImplant i:
                properties.Add(Constants.P_IMP_QUALITY_LEVEL_INT, i.QualityLevel);
                properties.Add(Constants.P_IMP_BODY_PART_IMPLANTTYPE, i.BodyPart);
                properties.Add(Constants.P_IMP_STATS_DICT, new TestStats(1, 1));
                properties.Add(Constants.P_IMP_REQUIRED_STATS_DICT, new TestStats(Random.Range(1, 4), Random.Range(1, 4)));
                goto default;

            default:
                break;
        }
    }
}
