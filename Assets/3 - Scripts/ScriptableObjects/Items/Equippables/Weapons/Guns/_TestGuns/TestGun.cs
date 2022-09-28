using UnityEngine;

[CreateAssetMenu(fileName = "TestGun", menuName = "Items/Weapons/Guns/TestGun", order = 1)]
public class TestGun : Item, IGun
{
    [SerializeField]
    private float castDelay = 0.0f;
    [SerializeField]
    private float rechargeTime = 0.0f;
    [SerializeField]
    private float xSpread = 0.0f;
    [SerializeField]
    private float ySpread = 0.0f;

    private void Awake()
    {
        this.Name = Constants.TEST_GUN;
        this.Id = Constants.TEST_GUN_ID;
        this.isStackable = false;
    }

    public float CastDelay { get => castDelay; set => castDelay = value; }
    public float RechargeTime { get => rechargeTime; set => rechargeTime = value; }
    public float XSpread { get => xSpread; set => xSpread = value; }
    public float YSpread { get => ySpread; set => ySpread = value; }
}
