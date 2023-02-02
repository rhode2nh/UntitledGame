using UnityEngine;

[CreateAssetMenu(fileName = "CastX", menuName = "Items/Modifiers/New CastX", order = 1)]
public class CastX : Modifier, ICastX
{
    [SerializeField]
    public int modifiersPerCast;

    public new void Awake()
    {
        this.Name = Constants.MOD_CASTX;
        this.Id = Constants.MOD_CASTX_ID;
        this.isStackable = false;
    }

    public int ModifiersPerCast { get => modifiersPerCast; } 
}
