using UnityEngine;

[CreateAssetMenu(fileName = "Modifier", menuName = "Items/Modifiers/New Modifier", order = 1)]
public class Modifier : Item, IModifier
{
    [SerializeField]
    public float castDelay;
    [SerializeField]
    public float rechargeDelay;
    [SerializeField]
    public float powerConsumption;

    public void Awake() 
    {
        this.Name = Constants.TEST_MODIFIER;
        this.Id = Constants.TEST_MODIFIER_ID;
        this.isStackable = false;
    }

    public float CastDelay { get => castDelay; }
    public float RechargeDelay { get => rechargeDelay; }
    public float PowerConsumption { get => powerConsumption; }

}
