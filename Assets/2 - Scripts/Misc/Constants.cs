public class Constants
{
    // Item Indices
    public const int CUBE_ID = 0;
    public const int SPHERE_ID = 1;
    public const int PRIMITIVE_ITEM_BASE_ID = 2;
    public const int SCREW_ID = 3;
    public const int CYLINDER_ID = 4;
    public const int TEST_CONSUMABLE_ID = 5;
    public const int TEST_MODIFIER_ID = 6;
    public const int TEST_GUN_ID = 7;
    public const int MOD_PROJECTILE_ID = 8;
    public const int MOD_CASTX_ID = 9;
    public const int IMPLANT_ID = 10;

    // Prefixes
    public const string INVENTORY_ITEM_PREFIX = "II";
    public const string WORLD_ITEM_PREFIX = "WI";
    public const string CONSUMABLE_ITEM_PREFIX = "CI";

    // Tags
    public const string WORLD_ITEM = "WORLD_ITEM";
    public const string BUTTON = "BUTTON";
    public const string CHEST = "CHEST";
    public const string PLAYER = "Player";

    // Inventory/World Item Names
    public const string INVENTORY_ITEM_BASE = "INVENTORY_ITEM_BASE";
    public const string CUBE = "CUBE";
    public const string COPPER = "COPPER";
    public const string SPHERE = "SPHERE";
    public const string PRIMITIVE_ITEM_BASE = "PRIMITIVE_ITEM_BASE";
    public const string SCREW = "SCREW";
    public const string CYLINDER = "CYLINDER";
    public const string TEST_CONSUMABLE = "TEST_CONSUMABLE";
    public const string TEST_MODIFIER = "TEST_MODIFIER";
    public const string TEST_GUN = "TEST_GUN";
    public const string MOD_PROJECTILE = "MOD_PROJECTILE";
    public const string MOD_TRAJECTORY = "MOD_TRAJECTORY";
    public const string MOD_BULLET = "MOD_BULLET";
    public const string MOD_CASTX = "MOD_CASTX";
    public const string IMPLANT = "IMPLANT";

    // Stat Names
    public const string AGILITY = "AGILITY";
    public const string STRENGTH = "STRENGTH";
    public const string STAMINA = "STAMINA";
    public const string SPEED = "SPEED";

    // Mutable Item Properties
    // These constants will follow a convention as such: P_[item type acroynm]_[property]_[value data type]
    public const string P_W_MODIFIERS_LIST = "P_W_MODIFIERS_LIST";
    public const string P_W_MAX_SLOTS_INT = "P_W_MAX_SLOTS_INT";
    public const string P_W_MODIFIER_SLOT_INDICES_LIST = "P_W_MODIFIER_SLOT_INDICES_LIST";
    public const string P_KNOCKBACK = "P_KNOCKBACK";

    public const string P_IMP_QUALITY_LEVEL_INT = "P_IMP_QUALITY_LEVEL_INT";
    public const string P_IMP_BODY_PART_IMPLANTTYPE = "P_IMP_BODY_PART_IMPLANTTYPE";
    public const string P_IMP_STATS_DICT = "P_IMP_STATS_DICT";
    public const string P_IMP_REQUIRED_STATS_DICT = "P_IMP_REQUIRED_STATS_DICT";
}
