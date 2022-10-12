using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trigger", menuName = "Items/Modifiers/Projectiles/New Trigger", order = 1)]
public class Trigger : Modifier, ITrigger, IProjectile
{
    [SerializeField]
    public GameObject triggerProjectilePrefab;

    public GameObject ProjectilePrefab { get => triggerProjectilePrefab; }
}
