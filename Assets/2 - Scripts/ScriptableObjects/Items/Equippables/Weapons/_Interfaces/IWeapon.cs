public interface IWeapon : IEquippable
{
    public float CastDelay { get; set; }
    public float RechargeTime { get; set; }
    public float XSpread { get; set; }
    public float YSpread { get; set; }
    public float MaxSlots { get; set; }
}
