using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Items/Modifiers/Projectiles/New Projectile", order = 1)]
public class Projectile : Modifier, IProjectile
{
    [SerializeField]
    public GameObject projectilePrefab;
    public float timeAlive;

    public new void Awake() 
    {
        this.Name = Constants.MOD_PROJECTILE;
        this.Id = Constants.MOD_PROJECTILE_ID;
        this.isStackable = false;
        this.timeAlive = 2.0f;
    }

    public GameObject ProjectilePrefab { get => projectilePrefab; }
}
