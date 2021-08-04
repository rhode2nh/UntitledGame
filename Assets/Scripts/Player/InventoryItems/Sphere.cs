using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sphere", menuName = "Items/Sphere", order = 1)]
public class Sphere : Item
{
    private void Awake()
    {
        this.Name = Constants.SPHERE;
        this.Id = Constants.SPHERE_ID;
        this.Count = 1;
        this.IsStackable = true;
    }
}
