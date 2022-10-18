using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyProjectile : MonoBehaviour
{
    public ScriptableObject projectileSO;

    void Start()
    {
        var projectile = projectileSO as Projectile;
        Destroy(gameObject, projectile.timeAlive);
    }
}
