using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StateManager
{
    public static Slot LoadItemData(ItemData itemData)
    {
        Dictionary<string, object> properties = new Dictionary<string, object>();
        var item = DatabaseManager.instance.GetItem(itemData.itemId);
        if (item is IWeapon)
        {
            List<Slot> modifierList = new List<Slot>();
            foreach (var modifierId in itemData.P_W_MODIFIERS_LIST)
            {
                var modifier = DatabaseManager.instance.GetItem(modifierId);
                modifierList.Add(new Slot(modifier));
            }
            properties.Add(Constants.P_W_MODIFIERS_LIST, modifierList);
            properties.Add(Constants.P_W_MAX_SLOTS_INT, itemData.P_W_MAX_SLOTS_INT);
            properties.Add(Constants.P_W_MODIFIER_SLOT_INDICES_LIST, new List<int>());
        }
        if (item is IImplant)
        {
            properties.Add(Constants.P_IMP_QUALITY_LEVEL_INT, itemData.P_IMP_QUALITY_LEVEL_INT);
            properties.Add(Constants.P_IMP_BODY_PART_IMPLANTTYPE, itemData.P_IMP_BODY_PART_IMPLANTTYPE);
            properties.Add(Constants.P_IMP_STATS_DICT, itemData.P_IMP_STATS_DICT);
            properties.Add(Constants.P_IMP_REQUIRED_STATS_DICT, itemData.P_IMP_REQUIRED_STATS_DICT);
        }
        return new Slot(itemData.id, item, itemData.count, properties);
    }

    public static ItemData SaveItemData(Slot slot)
    {
        ItemData itemData = new ItemData();
        itemData.id = slot.id;
        itemData.count = slot.count;
        itemData.itemId = slot.item.Id;
        if (slot.item is IWeapon)
        {
            var modifierList = (List<Slot>)slot.properties[Constants.P_W_MODIFIERS_LIST];
            foreach (var modifier in modifierList)
            {
                itemData.P_W_MODIFIERS_LIST.Add(modifier.item.Id);
            }
            itemData.P_W_MAX_SLOTS_INT = (int)slot.properties[Constants.P_W_MAX_SLOTS_INT];
        }
        if (slot.item is IImplant)
        {
            //itemData.P_IMP_QUALITY_LEVEL_INT = (int)slot.properties[Constants.P_IMP_QUALITY_LEVEL_INT];
            //itemData.P_IMP_BODY_PART_IMPLANTTYPE = (BodyPart)slot.properties[Constants.P_IMP_BODY_PART_IMPLANTTYPE];
            itemData.P_IMP_STATS_DICT = (TestStats)slot.properties[Constants.P_IMP_STATS_DICT];
            itemData.P_IMP_REQUIRED_STATS_DICT = (TestStats)slot.properties[Constants.P_IMP_REQUIRED_STATS_DICT];
        }
        return itemData;
    }
}
