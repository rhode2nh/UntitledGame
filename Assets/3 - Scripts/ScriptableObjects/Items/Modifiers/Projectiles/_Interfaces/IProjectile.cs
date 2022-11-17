using UnityEngine;

public interface IProjectile 
{
    public GameObject ProjectilePrefab { get; }
    public float HitPoints { get; }
}
