using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Items/Modifiers/Projectiles/New Projectile", order = 1)]
public class Projectile : Modifier, IProjectile
{
    [SerializeField]
    public GameObject projectilePrefab;
    public float timeAlive;
    public float hitPoints;

    public new void Awake() 
    {
        this.Name = Constants.MOD_PROJECTILE;
        this.isStackable = false;
        this.timeAlive = 2.0f;
        this.hitPoints = 20.0f;
    }

    public GameObject ProjectilePrefab { get => projectilePrefab; }
    public float HitPoints { get => hitPoints; }
}
