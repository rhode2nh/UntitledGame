public interface IWeapon : IEquippable
{
    public float CastDelay { get; set; }
    public float RechargeTime { get; set; }
}
