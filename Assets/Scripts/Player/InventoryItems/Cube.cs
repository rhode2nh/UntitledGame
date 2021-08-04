using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cube", menuName = "Items/Cube", order = 1)]
public class Cube : Item
{
    private void Awake()
    {
        this.Name = Constants.CUBE;
        this.Id = Constants.CUBE_ID;
        this.Count = 1;
        this.IsStackable = true;
    }
}
