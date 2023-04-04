using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldItemData
{
    public string name;
    public Vector3 pos;
    public string itemId;
}

[System.Serializable]
public class ItemData
{
    public string id;
    public int count;
    public string itemId;
    public int P_W_MAX_SLOTS_INT;
    public List<string> P_W_MODIFIERS_LIST = new List<string>();
    public int P_IMP_QUALITY_LEVEL_INT;
    public BodyPart P_IMP_BODY_PART_IMPLANTTYPE;
    public TestStats P_IMP_STATS_DICT = new TestStats();
    public TestStats P_IMP_REQUIRED_STATS_DICT = new TestStats();
}

[System.Serializable]
public class GameData
{
    public List<WorldItemData> itemsData = new List<WorldItemData>();
    public List<WorldItemData> instancedItemsData = new List<WorldItemData>();
    public List<ItemData> inventoryItemsData = new List<ItemData>();
    public List<ItemData> equippedItemsData = new List<ItemData>();
    public List<ItemData> implantItemsData = new List<ItemData>();
    public int curEquipmentIndex = 0;

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load.
    public GameData()
    {
    }
}
