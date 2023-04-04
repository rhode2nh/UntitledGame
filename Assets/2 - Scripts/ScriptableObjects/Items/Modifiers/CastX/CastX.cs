using UnityEngine;

[CreateAssetMenu(fileName = "CastX", menuName = "Items/Modifiers/New CastX", order = 1)]
public class CastX : Modifier, ICastX
{
    [SerializeField]
    public int modifiersPerCast;

    public new void Awake()
    {
        this.Name = Constants.MOD_CASTX;
        this.isStackable = false;
    }

    public int ModifiersPerCast { get => modifiersPerCast; } 
}
