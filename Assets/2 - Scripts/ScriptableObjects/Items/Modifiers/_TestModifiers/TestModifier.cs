using UnityEngine;

[CreateAssetMenu(fileName = "TestModifier", menuName = "Items/Modifiers/TestModifier", order = 1)]
public class TestModifier : Item, IModifier
{
    [SerializeField]
    public float castDelay;
    [SerializeField]
    public float rechargeDelay;
    [SerializeField]
    public float powerConsumption;
    [SerializeField]
    public float spreadX;
    [SerializeField]
    public float spreadY;

    public void Awake() 
    {
        this.Name = Constants.TEST_MODIFIER;
        this.Id = Constants.TEST_MODIFIER_ID;
        this.isStackable = false;
    }

    public float CastDelay { get => castDelay; }
    public float RechargeDelay { get => rechargeDelay; }
    public float PowerConsumption { get => powerConsumption; }
    public float XSpread { get => spreadX; }
    public float YSpread { get => spreadY; }

}
